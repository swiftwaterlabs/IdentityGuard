using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityGuard.Core.Extensions;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityGuard.Core.Managers
{
    public class UserManager
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly ServicePrincipalService _servicePrincipalService;
        private readonly DirectoryManager _directoryManager;

        public UserManager(IConfiguration configuration, 
            UserService userService, 
            ServicePrincipalService servicePrincipalService,
            DirectoryManager directoryManager)
        {
            _configuration = configuration;
            _userService = userService;
            _servicePrincipalService = servicePrincipalService;
            _directoryManager = directoryManager;
        }

        public List<KeyValuePair<string, string>> GetClaims(IEnumerable<ClaimsIdentity> identities)
        {
            if(_configuration.IsDevelopment()) return new List<KeyValuePair<string, string>>();

            var claims = identities?
                .SelectMany(i => i?.Claims ?? new List<Claim>())
                .Select(c => new KeyValuePair<string, string>(c?.Type, c?.Value))
                .ToList();

            var result = claims ?? new List<KeyValuePair<string, string>>();

            return result;
        }

        public async Task<Shared.Models.User> Get(string directoryId, string id)
        {
            var directory = await _directoryManager.GetById(directoryId);
            var user = await _userService.Get(directory, id);

            return user;
        }

        public async Task<List<Shared.Models.User>> Search(string userType, List<string> names)
        {
            if (names == null || !names.Any()) return new List<User>();

            var directories = await _directoryManager.Get();

            var searchTasks = directories
                .AsParallel()
                .Select(directory => SearchDirectory(directory, userType, names));

            var searchResult = await Task.WhenAll(searchTasks);

            var result = searchResult
                .SelectMany(r => r)
                .ToList();

            return result;

        }

        private async Task<List<Shared.Models.User>> SearchDirectory(Directory directory, string userType, List<string> names)
        {
            var upnTasks = names
                .AsParallel()
                .Select(name => _userService.SearchUser(directory, userType, name, Models.UserSearchType.UserPrincipalName));

            var emailTasks = names
                .AsParallel()
                .Select(name => _userService.SearchUser(directory, userType, name, Models.UserSearchType.UserPrincipalName));

            var upnResult = await Task.WhenAll(upnTasks);
            var emailResult = await Task.WhenAll(emailTasks);

            var results = upnResult.SelectMany(r => r)
                .Union(emailResult.SelectMany(r => r));

            var uniqueResults = results
                .ToLookup(u => $"{u.DirectoryId}|{u.Id}")
                .Select(u => u.First())
                .ToList();

            return uniqueResults;
        }

        public async Task<UserAccess> GetAccess(string directoryId, string userId)
        {
            var directory = await _directoryManager.GetById(directoryId);
            var user = await _userService.Get(directory, userId);
            if (user?.Id == null) return null;

            var ownedObjectsTask = _userService.GetOwnedObjects(directory, userId);
            var memberObjectsTask = _userService.GetMemberOf(directory, userId);
            var roleAssignmentsTask = _userService.GetApplicationRoles(directory, userId);

            await Task.WhenAll(ownedObjectsTask, memberObjectsTask, roleAssignmentsTask);

            var ownedObjects = await ownedObjectsTask;
            var memberObjects = await memberObjectsTask;
            var roleAssignments = await roleAssignmentsTask;

            await ApplyRoleNames(directory, roleAssignments);

            return new UserAccess
            {
                User = user,
                DirectoryId = directory.Id,
                DirectoryName = directory.Domain,
                OwnedObjects = ownedObjects,
                GroupMembership = memberObjects,
                RoleMemberships = roleAssignments
            };
        }

        private async Task ApplyRoleNames(Directory directory, List<ApplicationRole> roleAssignments)
        {
            var servicePrincipals = await GetServicePrincipalsAssignedTo(directory, roleAssignments);
            var servicePrincipalsById = servicePrincipals.ToDictionary(s => s.Id);

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