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

            var result = _applicationMapper.Map(directory, data);

            return result;
        }

        public async Task<List<Shared.Models.Application>> Search(Shared.Models.Directory directory, string name)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            string filter = GetSearchFilter(name);
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

            var users = result
                .Select(u => _applicationMapper.Map(directory, u))
                .ToList();

            return users;
        }

        private static string GetSearchFilter(string name)
        {
            var encodedName = System.Web.HttpUtility.UrlEncode(name);
            var filter = $"displayName contains '{encodedName}'";
            return filter;
        }
    }
}
