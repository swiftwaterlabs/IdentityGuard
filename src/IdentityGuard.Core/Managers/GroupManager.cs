using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers
{
    public class GroupManager
    {
        private readonly GroupService _groupService;
        private readonly DirectoryManager _directoryManager;
        private readonly ServicePrincipalService _servicePrincipalService;

        public GroupManager(GroupService groupService,
            DirectoryManager directoryManager,
            ServicePrincipalService servicePrincipalService)
        {
            _groupService = groupService;
            _directoryManager = directoryManager;
            _servicePrincipalService = servicePrincipalService;
        }

        public async Task<Group> Get(string directoryId, string id,
            bool includeOwners = false,
            bool inclueMembers = false)
        {
            var directory = await _directoryManager.GetById(directoryId);

            var group = await _groupService.Get(directory, id, includeOwners: includeOwners, includeMembers:inclueMembers);

            return group;
        }

        public async Task<List<Group>> Search(List<string> names)
        {
            if (names == null || !names.Any()) return new List<Group>();

            var directories = await _directoryManager.Get();

            var searchTasks = directories
                .AsParallel()
                .Select(directory => SearchDirectory(directory, names));

            var searchResult = await Task.WhenAll(searchTasks);

            var result = searchResult
                .SelectMany(r => r)
                .ToList();

            return result;
        }

        private async Task<List<Shared.Models.Group>> SearchDirectory(Directory directory, List<string> names)
        {
            var nameTasks = names
                .AsParallel()
                .Select(name => _groupService.Search(directory, name));

            var nameResult = await Task.WhenAll(nameTasks);

            var results = nameResult.SelectMany(r => r);

            var uniqueResults = results
                .ToLookup(u => $"{u.DirectoryId}|{u.Id}")
                .Select(u => u.First())
                .ToList();

            return uniqueResults;
        }

        public async Task<GroupAccess> GetAccess(string directoryId, string id)
        {
            var directory = await _directoryManager.GetById(directoryId);
            var group = await Get(directoryId, id, includeOwners: true, inclueMembers: true);

            var roleAssignments = await _groupService.GetApplicationRoles(directory, id);
            await ApplyRoleNames(directory, roleAssignments);

            return new GroupAccess
            {
                DirectoryId = directory.Id,
                DirectoryName = directory.Domain,
                Group = group,
                RoleMemberships = roleAssignments
            };
        }

        private async Task ApplyRoleNames(Directory directory, List<ApplicationRole> roleAssignments)
        {
            var servicePrincipals = await GetServicePrincipalsAssignedTo(directory, roleAssignments);
            var servicePrincipalsById = servicePrincipals
                .Where(s=>s!=null)
                .ToDictionary(s => s.Id);

            Parallel.ForEach(roleAssignments, item =>
            {
                if (servicePrincipalsById.TryGetValue(item.AssignedTo.Id, out ServicePrincipal servicePrincipal))
                {
                    if (servicePrincipal.Roles.TryGetValue(item.Role.Id, out Role role))
                    {
                        item.Role.DisplayName = role.DisplayName;
                    }
                }
            });
        }

        private async Task<ServicePrincipal[]> GetServicePrincipalsAssignedTo(Directory directory, List<ApplicationRole> roleAssignments)
        {
            var servicePrincipalIds = roleAssignments
                .Select(r => r.AssignedTo.Id)
                .Distinct();

            var servicePrincipalTasks = servicePrincipalIds
                .Select(s => _servicePrincipalService.Get(directory, s));
            var servicePrincipals = await Task.WhenAll(servicePrincipalTasks);

            return servicePrincipals;
        }
    }
}
