using IdentityGuard.Tests.Shared.TestContexts;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityGuard.Tests.Shared.Fakes.Http.RequestHandlers
{
    public class GraphUserHttpRequestHandler : IHttpRequestHandler
    {
        private TestContext _context;

        public GraphUserHttpRequestHandler(TestContext context)
        {
            _context = context;
        }

        public bool AppliesTo(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> ProcessAsync(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }
    }
}
