using System.Net;
using System.Threading.Tasks;
using IdentityGuard.Core.Managers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace IdentityGuard.Api.Functions
{
    public class HealthFunction
    {
        private readonly AboutManager _aboutManager;
        private readonly ApplicationHealthManager _applicationHealthManager;

        public HealthFunction(AboutManager aboutManager, ApplicationHealthManager applicationHealthManager)
        {
            _aboutManager = aboutManager;
            _applicationHealthManager = applicationHealthManager;
        }
        [Function("health-probe")]
        public HttpResponseData Probe([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health/probe")] HttpRequestData req,
            FunctionContext executionContext)
        { 
            var response = req.CreateResponse(HttpStatusCode.OK);

            return response;
        }

        [Function("health-about")]
        public async Task<HttpResponseData> About([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health/about")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var aboutInfo = _aboutManager.Get();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(aboutInfo);

            return response;
        }

        [Function("health-status")]
        public async Task<HttpResponseData> Status([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health/status")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var aboutInfo = _applicationHealthManager.Get();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(aboutInfo);

            return response;
        }
    }
}
