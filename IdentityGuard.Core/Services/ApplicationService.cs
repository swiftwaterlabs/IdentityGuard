using IdentityGuard.Core.Factories;
using IdentityGuard.Core.Mappers;
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
    }
}
