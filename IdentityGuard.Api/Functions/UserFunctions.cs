using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityGuard.Api.Extensions;
using IdentityGuard.Core.Managers;
using IdentityGuard.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Functions
{
    public class UserFunctions
    {
        private readonly UserManager _userManager;
        private readonly AuthorizationManager _authorizationManager;

        public UserFunctions(UserManager userManager, AuthorizationManager authorizationManager)
        {
            _userManager = userManager;
            _authorizationManager = authorizationManager;
        }

        [Function("user-claims")]
        public Task<HttpResponseData> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/claims")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var result = _userManager.GetClaims(req.GetRequestingUser());

            return req.OkResponseAsync(result);

        }

        [Function("user-search")]
        public async Task<HttpResponseData> Search(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/search/{userType}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string userType)
        {
            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageUserAccessReviews, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _userManager.Search(userType, req.GetBody<List<string>>());

            return await req.OkResponseAsync(data);
        }
    }
}
