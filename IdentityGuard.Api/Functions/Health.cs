using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace IdentityGuard.Api.Functions
{
    public class Health
    {
        [Function("health-probe")]
        public HttpResponseData Probe([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health/probe")] HttpRequestData req,
            FunctionContext executionContext)
        { 
            var response = req.CreateResponse(HttpStatusCode.OK);

            return response;
        }
    }
}
