using System.Threading.Tasks;
using IdentityGuard.Api.Extensions;
using IdentityGuard.Core.Managers;
using IdentityGuard.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Functions
{
    public class HealthFunction
    {
        private readonly AuthorizationManager _authorizationManager;
        private readonly AboutManager _aboutManager;
        private readonly ApplicationHealthManager _applicationHealthManager;

        public HealthFunction(AuthorizationManager authorizationManager,
            AboutManager aboutManager, 
            ApplicationHealthManager applicationHealthManager)
        {
            _authorizationManager = authorizationManager;
            _aboutManager = aboutManager;
            _applicationHealthManager = applicationHealthManager;
        }
        [Function("health-probe")]
        public Task<HttpResponseData> Probe([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health/probe")] HttpRequestData req,
            FunctionContext executionContext)
        {
            return req.OkResponseAsync();
        }

        [Function("health-about")]
        public Task<HttpResponseData> About(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health/about")]
            HttpRequestData req,
            FunctionContext executionContext)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ViewApplicationInfo, req.GetRequestingUser())) return req.UnauthorizedResponseAsync();

            var data = _aboutManager.Get();

            return req.OkResponseAsync(data);

        }

        [Function("health-status")]
        public Task<HttpResponseData> Status([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health/status")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var data = _applicationHealthManager.Get();

            return req.OkResponseAsync(data);
        }
    }
}
