using IdentityGuard.Core.Factories;
using IdentityGuard.Core.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Services
{
    public class ApplicationService
    {
        private readonly IGraphClientFactory _graphClientFactory;
        private readonly ApplicationMapper _applicationMapper;

        public ApplicationService(IGraphClientFactory graphClientFactory,
            ApplicationMapper applicationMapper)
        {
            _graphClientFactory = graphClientFactory;
            _applicationMapper = applicationMapper;
        }

        public async Task<Shared.Models.Application> Get(Shared.Models.Directory directory, string id, bool includeOwners = false)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var data = await client
                .Applications[id]
                .Request()
                .GetAsync();

            var owners = new List<Microsoft.Graph.DirectoryObject>();
            if(includeOwners)
            {
                owners = await GetOwners(client, id);
            }
            var result = _applicationMapper.Map(directory, data, owners);

            return result;
        }

        private async Task<List<Microsoft.Graph.DirectoryObject>> GetOwners(Microsoft.Graph.IGraphServiceClient client, string id)
        {
            List<Microsoft.Graph.DirectoryObject> result = new List<Microsoft.Graph.DirectoryObject>();

            var ownersRequest = await client.Applications[id]
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

        public async Task<List<Shared.Models.Application>> Search(Shared.Models.Directory directory, string name)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var filter = GetSearchFilter(name);
            var searchRequest = await client.Applications
                .Request()
                .Filter(filter)
                .GetAsync();

            var result = new List<Microsoft.Graph.Application>();
            while (searchRequest != null)
            {
                result.AddRange(searchRequest);

                if (searchRequest.NextPageRequest == null) break;
                searchRequest = await searchRequest.NextPageRequest.GetAsync();
            };

            var applications = result
                .Select(u => _applicationMapper.Map(directory, u, new List<Microsoft.Graph.DirectoryObject>()))
                .ToList();

            return applications;
        }

        private static string GetSearchFilter(string name)
        {
            var encodedName = System.Web.HttpUtility.UrlEncode(name);
            var filter = $"startsWith(displayName,'{encodedName}')";
            return filter;
        }

        public async Task RemoveOwners(Shared.Models.Directory directory, string id, IEnumerable<string> toRemove)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var removeTasks = toRemove.Select(o => RemoveOwner(client, id, o)).ToArray();
            await Task.WhenAll(removeTasks);
        }

        private Task RemoveOwner(Microsoft.Graph.IGraphServiceClient client, string id, string ownerId)
        {
            return client.Applications[id]
                .Owners[ownerId]
                .Reference
                .Request()
                .DeleteAsync();

        }
    }
}
