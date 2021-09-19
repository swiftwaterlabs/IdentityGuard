using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityGuard.Api.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Functions
{
    public class UserFunctions
    {
        [Function("user-claims")]
        public Task<HttpResponseData> About(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/claims")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var claims = req.Identities?
                .SelectMany(i => i?.Claims ?? new List<Claim>())
                .Select(c => new KeyValuePair<string, string>(c?.Type, c?.Value))
                .ToList();

            var result = claims ?? new List<KeyValuePair<string, string>>();

            return req.OkResponseAsync(result);

        }
    }
}
