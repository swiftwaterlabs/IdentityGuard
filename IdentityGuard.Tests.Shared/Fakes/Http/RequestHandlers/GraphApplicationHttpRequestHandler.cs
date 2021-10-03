using IdentityGuard.Tests.Shared.Fakes.Http.Graph;
using IdentityGuard.Tests.Shared.TestContexts;
using Microsoft.Graph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Tests.Shared.Fakes.Http.RequestHandlers
{
    public class GraphApplicationHttpRequestHandler : IHttpRequestHandler
    {
        private TestContext _context;

        public GraphApplicationHttpRequestHandler(TestContext context)
        {
            _context = context;
        }
        public bool AppliesTo(HttpRequestMessage request)
        {
            return request.Method == HttpMethod.Get && request.RequestUri.AbsoluteUri.StartsWith(GraphUri.Applications);
        }

        public Task<HttpResponseMessage> ProcessAsync(HttpRequestMessage request)
        {
            var requestParts = request.RequestUri.AbsoluteUri.Replace(GraphUri.Applications, "").Split("/");
            var applicationId = requestParts.FirstOrDefault();
            var property = requestParts.Skip(1).FirstOrDefault();

            var data = _context
                .GraphApi
                .Applications
                .Values
                .SelectMany(a => a)
                .FirstOrDefault(a => a.Id == applicationId);

            if (data == null)
            {
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound));
            }

            if (string.Equals("owners", property, StringComparison.CurrentCultureIgnoreCase))
            {
                var responseData = new GraphResponse<IApplicationOwnersCollectionWithReferencesPage>
                {
                    value = data.Owners
                };
                var ownersResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(responseData))
                };
                return Task.FromResult(ownersResponse);
            }

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data))
            };
            return Task.FromResult(response);
        }
    }
}
