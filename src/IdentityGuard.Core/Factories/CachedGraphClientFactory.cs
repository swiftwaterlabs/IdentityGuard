using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace IdentityGuard.Core.Factories
{
    public class CachedGraphClientFactory : IGraphClientFactory
    {
        private readonly GraphClientFactory _graphClientFactory;
        private ConcurrentDictionary<string, IGraphServiceClient> _cache = new ConcurrentDictionary<string,IGraphServiceClient>();

        public CachedGraphClientFactory(GraphClientFactory graphClientFactory)
        {
            _graphClientFactory = graphClientFactory;
        }
        public async Task<IGraphServiceClient> CreateAsync(string directoryId)
        {
            if (_cache.TryGetValue(directoryId, out IGraphServiceClient existing)) return existing;

            var client = await _graphClientFactory.CreateAsync(directoryId);
            _cache.AddOrUpdate(directoryId, client, (key, existing) => { return client; });

            return client;
        }

        public async Task<IGraphServiceClient> CreateAsync(Shared.Models.Directory directory)
        {
            if (_cache.TryGetValue(directory.Id, out IGraphServiceClient existing)) return existing;

            var client = await _graphClientFactory.CreateAsync(directory);
            _cache.AddOrUpdate(directory.Id, client, (key, existing) => { return client; });

            return client;
        }

        public async Task<IGraphServiceClient> CreateAsync()
        {
            if (_cache.TryGetValue("default", out IGraphServiceClient existing)) return existing;

            var client = await _graphClientFactory.CreateAsync();
            _cache.AddOrUpdate("default", client, (key, existing) => { return client; });

            return client;
        }
    }
}