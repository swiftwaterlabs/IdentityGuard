using System.Threading.Tasks;
using IdentityGuard.Api.Extensions;
using IdentityGuard.Core.Managers;
using IdentityGuard.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Functions
{
    public class LifecyclePolicyFunctions
    {
        private readonly LifecyclePolicyManager _lifecyclePolicyManager;
        private readonly LifecyclePolicyExecutionManager _lifecyclePolicyExecutionManager;
        private readonly AuthorizationManager _authorizationManager;

        public LifecyclePolicyFunctions(LifecyclePolicyManager lifecyclePolicyManager, 
            LifecyclePolicyExecutionManager lifecyclePolicyExecutionManager,
            AuthorizationManager authorizationManager)
        {
            _lifecyclePolicyManager = lifecyclePolicyManager;
            _lifecyclePolicyExecutionManager = lifecyclePolicyExecutionManager;
            _authorizationManager = authorizationManager;
        }

        [Function("lifecyclepolicy-get")]
        public async Task<HttpResponseData> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lifecycle/policy")]
            HttpRequestData req,
            FunctionContext executionContext)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.LifecyclePolicyContributor, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _lifecyclePolicyManager.Get();

            return await req.OkResponseAsync(data);
        }

        [Function("lifecyclepolicy-getbyid")]
        public async Task<HttpResponseData> GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lifecycle/policy/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.LifecyclePolicyContributor, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _lifecyclePolicyManager.Get(id);

            if (string.IsNullOrEmpty(data?.Id)) return req.NotFoundResponse();

            return await req.OkResponseAsync(data);
        }

        [Function("lifecyclepolicy-post")]
        public async Task<HttpResponseData> Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "lifecycle/policy/")]
            HttpRequestData req,
            FunctionContext executionContext)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.LifecyclePolicyContributor, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _lifecyclePolicyManager.Add(req.GetBody<LifecyclePolicy>());

            return await req.OkResponseAsync(data);
        }

        [Function("lifecyclepolicy-put")]
        public async Task<HttpResponseData> Put(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "lifecycle/policy/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.LifecyclePolicyContributor, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _lifecyclePolicyManager.Update(id, req.GetBody<LifecyclePolicy>());

            if (string.IsNullOrEmpty(data?.Id)) return req.NotFoundResponse();

            return await req.OkResponseAsync(data);
        }

        [Function("lifecyclepolicy-delete")]
        public async Task<HttpResponseData> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "lifecycle/policy/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.LifecyclePolicyContributor, req.GetRequestingUser())) return req.UnauthorizedResponse();

            await _lifecyclePolicyManager.Delete(id);

            return await req.OkResponseAsync();
        }

        [Function("lifecyclepolicy-audit")]
        public async Task<HttpResponseData> Test(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lifecycle/policy/{id}/audit")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.LifecyclePolicyContributor, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _lifecyclePolicyExecutionManager.AuditPolicy(id);

            return await req.OkResponseAsync(data);
        }
    }
}
