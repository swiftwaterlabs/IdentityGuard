using IdentityGuard.Core.Factories;
using IdentityGuard.Core.Mappers;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Services
{
    public class DirectoryObjectService
    {
        private readonly IGraphClientFactory _graphClientFactory;
        private readonly DirectoryObjectMapper _directoryObjectMapper;

        public DirectoryObjectService(IGraphClientFactory graphClientFactory,
            DirectoryObjectMapper directoryObjectMapper)
        {
            _graphClientFactory = graphClientFactory;
            _directoryObjectMapper = directoryObjectMapper;
        }

        public async Task<Shared.Models.DirectoryObject> Get(Shared.Models.Directory directory, string id)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var data = await client
                .DirectoryObjects[id]
                .Request()
                .GetAsync();

            var result = _directoryObjectMapper.Map(directory, data);

            return result;
        }
    }
}
