using System.Threading.Tasks;
using IdentityGuard.Api.Extensions;
using IdentityGuard.Core.Managers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Functions
{
    public class AuthorizationFunction
    {
        private readonly AuthorizationManager _authorizationManager;

        public AuthorizationFunction(AuthorizationManager authorizationManager)
        {
            _authorizationManager = authorizationManager;
        }

        [Function("authorization-get")]
        public Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get",Route = "authorization/{action}")] HttpRequestData req,
            FunctionContext executionContext,
            string action)
        {
            var data = _authorizationManager.IsAuthorized(action, req.Identities);

            return req.OkResponseAsync(data);
        }
    }
}
