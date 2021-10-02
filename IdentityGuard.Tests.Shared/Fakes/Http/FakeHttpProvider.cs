using IdentityGuard.Tests.Shared.Fakes.Http.RequestHandlers;
using IdentityGuard.Tests.Shared.TestContexts;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityGuard.Tests.Shared.Fakes.Http
{
    public class FakeHttpProvider : IHttpProvider
    {
        private readonly List<IHttpRequestHandler> _requestHandlers;

        public FakeHttpProvider(TestContext context)
        {
            _requestHandlers = new List<IHttpRequestHandler>
            {
                new GraphApplicationHttpRequestHandler(context),
                new GraphUserHttpRequestHandler(context)
            };
        }

        public ISerializer Serializer => new Serializer();

        public TimeSpan OverallTimeout { get; set; } = TimeSpan.FromMinutes(10);

        public void Dispose()
        {

        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var handler = _requestHandlers.FirstOrDefault(h => h.AppliesTo(request));
            if (handler != null)
            {
                var result = await handler.ProcessAsync(request);
                return result;
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            var handler = _requestHandlers.FirstOrDefault(h => h.AppliesTo(request));
            if (handler != null)
            {
                var result = await handler.ProcessAsync(request);
                return result;
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }
    }
}
