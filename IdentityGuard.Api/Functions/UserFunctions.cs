using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityGuard.Api.Extensions;
using IdentityGuard.Core.Managers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Functions
{
    public class UserFunctions
    {
        private readonly UserManager _userManager;

        public UserFunctions(UserManager userManager)
        {
            _userManager = userManager;
        }

        [Function("user-claims")]
        public Task<HttpResponseData> About(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/claims")]
            HttpRequestData req,
            FunctionContext executionContext,
            ClaimsPrincipal claimsPrincipal)
        {
            var result = _userManager.GetClaims(claimsPrincipal.Identities);

            return req.OkResponseAsync(result);

        }
    }
}
