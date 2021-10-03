using System.Threading.Tasks;
using IdentityGuard.Api.Extensions;
using IdentityGuard.Core.Managers;
using IdentityGuard.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Functions
{
    public class UserPolicyFunctions
    {
        private readonly UserPolicyManager _userPolicyManager;
        private readonly AuthorizationManager _authorizationManager;

        public UserPolicyFunctions(UserPolicyManager userPolicyManager, AuthorizationManager authorizationManager)
        {
            _userPolicyManager = userPolicyManager;
            _authorizationManager = authorizationManager;
        }

        [Function("userpolicy-get")]
        public async Task<HttpResponseData> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "policy/user")]
            HttpRequestData req,
            FunctionContext executionContext)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.UserPolicyContribtor, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _userPolicyManager.Get();

            return await req.OkResponseAsync(data);
        }

        [Function("userpolicy-getbyid")]
        public async Task<HttpResponseData> GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "policy/user/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.UserPolicyContribtor, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _userPolicyManager.Get(id);

            if (string.IsNullOrEmpty(data?.Id)) return req.NotFoundResponse();

            return await req.OkResponseAsync(data);
        }

        [Function("userpolicy-post")]
        public async Task<HttpResponseData> Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "policy/user/")]
            HttpRequestData req,
            FunctionContext executionContext)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.UserPolicyContribtor, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _userPolicyManager.Add(req.GetBody<UserPolicy>());

            return await req.OkResponseAsync(data);
        }

        [Function("userpolicy-put")]
        public async Task<HttpResponseData> Put(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "policy/user/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.UserPolicyContribtor, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _userPolicyManager.Update(id, req.GetBody<UserPolicy>());

            if (string.IsNullOrEmpty(data?.Id)) return req.NotFoundResponse();

            return await req.OkResponseAsync(data);
        }

        [Function("userpolicy-delete")]
        public async Task<HttpResponseData> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "policy/user/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.UserPolicyContribtor, req.GetRequestingUser())) return req.UnauthorizedResponse();

            await _userPolicyManager.Delete(id);

            return await req.OkResponseAsync();
        }
    }
}
