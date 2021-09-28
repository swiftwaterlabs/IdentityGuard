using IdentityGuard.Core.Factories;
using IdentityGuard.Core.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Services
{
    public class GroupService
    {
        private IGraphClientFactory _graphClientFactory;
        private GroupMapper _groupMapper;
        private readonly ApplicationRoleMapper _applicationRoleMapper;

        public GroupService(IGraphClientFactory graphClientFactory,
            GroupMapper groupMapper,
            ApplicationRoleMapper applicationRoleMapper)
        {
            _graphClientFactory = graphClientFactory;
            _groupMapper = groupMapper;
            _applicationRoleMapper = applicationRoleMapper;
        }

        public async Task<Shared.Models.Group> Get(Shared.Models.Directory directory, string id, bool includeOwners=false, bool includeMembers=false)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var data = await client.Groups[id]
                .Request()
                .GetAsync();

            var owners = new List<Microsoft.Graph.DirectoryObject>();
            if (includeOwners)
            {
                owners = await GetOwners(client, id);
            }

            var members = new List<Microsoft.Graph.DirectoryObject>();
            if (includeMembers)
            {
                members = await GetMembers(client, id);
            }

            var user = _groupMapper.Map(directory, data, owners, members);
            return user;
        }

        private async Task<List<Microsoft.Graph.DirectoryObject>> GetOwners(Microsoft.Graph.IGraphServiceClient client, string id)
        {
            List<Microsoft.Graph.DirectoryObject> result = new List<Microsoft.Graph.DirectoryObject>();

            var ownersRequest = await client.Groups[id]
                .Owners
                .Request()
                .GetAsync();


            while (ownersRequest != null)
            {
                result.AddRange(ownersRequest);

                if (ownersRequest.NextPageRequest == null) break;
                ownersRequest = await ownersRequest.NextPageRequest.GetAsync();
            };

            return result;
        }

        private async Task<List<Microsoft.Graph.DirectoryObject>> GetMembers(Microsoft.Graph.IGraphServiceClient client, string id)
        {
            List<Microsoft.Graph.DirectoryObject> result = new List<Microsoft.Graph.DirectoryObject>();

            var membersRequest = await client.Groups[id]
                .Members
                .Request()
                .GetAsync();


            while (membersRequest != null)
            {
                result.AddRange(membersRequest);

                if (membersRequest.NextPageRequest == null) break;
                membersRequest = await membersRequest.NextPageRequest.GetAsync();
            };

            return result;
        }

        public async Task<List<Shared.Models.Group>> Search(Shared.Models.Directory directory, string name)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var filter = GetSearchFilter(name);
            var searchRequest = await client.Groups
                .Request()
                .Filter(filter)
                .GetAsync();

            var result = new List<Microsoft.Graph.Group>();
            while (searchRequest != null)
            {
                result.AddRange(searchRequest);

                if (searchRequest.NextPageRequest == null) break;
                searchRequest = await searchRequest.NextPageRequest.GetAsync();
            };

            var groups = result
                .Select(u => _groupMapper.Map(directory, u, new List<Microsoft.Graph.DirectoryObject>(), new List<Microsoft.Graph.DirectoryObject>()))
                .ToList();

            return groups;
        }

        private static string GetSearchFilter(string name)
        {
            var encodedName = System.Web.HttpUtility.UrlEncode(name);
            var filter = $"startsWith(displayName,'{encodedName}')";
            return filter;
        }

        public async Task<List<Shared.Models.ApplicationRole>> GetApplicationRoles(Shared.Models.Directory directory, string id)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var request = await client.Groups[id]
                .AppRoleAssignments
                .Request()
                .GetAsync();

            var data = new List<Microsoft.Graph.AppRoleAssignment>();
            while (request != null)
            {
                data.AddRange(request);

                if (request.NextPageRequest == null) break;
                request = await request.NextPageRequest.GetAsync();
            };

            var result = data
                .Select(r => _applicationRoleMapper.Map(directory, r))
                .ToList();

            return result;
        }
    }
}
