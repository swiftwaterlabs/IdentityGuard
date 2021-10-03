using IdentityGuard.Core.Factories;
using IdentityGuard.Tests.Shared.Fakes.Http;
using IdentityGuard.Tests.Shared.TestContexts;
using Microsoft.Graph;
using System;
using System.Threading.Tasks;

namespace IdentityGuard.Tests.Shared.Fakes
{
    public class GraphClientFactoryFake : IGraphClientFactory
    {
        private readonly TestContext _context;

        public GraphClientFactoryFake(TestContext context)
        {
            _context = context;
        }

        public Task<IGraphServiceClient> CreateAsync(string directoryId)
        {
            return CreateAsync();
        }

        public Task<IGraphServiceClient> CreateAsync(IdentityGuard.Shared.Models.Directory directory)
        {
            return CreateAsync();
        }

        public Task<IGraphServiceClient> CreateAsync()
        {
            var authenticationProvider = new FakeAuthenticationProvider();
            var provider = new FakeHttpProvider(_context);
            IGraphServiceClient graphClient = new GraphServiceClient(authenticationProvider, provider);

            return Task.FromResult(graphClient);
        }
    }
}
