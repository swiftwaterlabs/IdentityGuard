using IdentityGuard.Api.Extensions;
using IdentityGuard.Core.Managers;
using IdentityGuard.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityGuard.Api.Functions
{
    public class AccessReviewFunctions
    {
        private readonly AuthorizationManager _authorizationManager;
        private readonly AccessReviewManager _accessReviewManager;

        public AccessReviewFunctions(AuthorizationManager authorizationManager, AccessReviewManager accessReviewManager)
        {
            _authorizationManager = authorizationManager;
            _accessReviewManager = accessReviewManager;
        }

        [Function("accessreview-getpending")]
        public async Task<HttpResponseData> GetPending(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "accessreview/search/pending")]
            HttpRequestData req,
           FunctionContext executionContext)
        {

            var user = req.GetRequestingUser();
            if (!_authorizationManager.IsAuthorized(AuthorizedActions.AccessReviewContributor, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _accessReviewManager.GetPending(user);

            return await req.OkResponseAsync(data);
        }

        [Function("accessreview-getcomplete")]
        public async Task<HttpResponseData> GetComplete(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "accessreview/search/complete")]
            HttpRequestData req,
           FunctionContext executionContext)
        {

            var user = req.GetRequestingUser();
            if (!_authorizationManager.IsAuthorized(AuthorizedActions.AccessReviewContributor, user)) return req.UnauthorizedResponse();

            var data = await _accessReviewManager.GetCompleted(user);

            return await req.OkResponseAsync(data);
        }

        [Function("accessreview-getbyid")]
        public async Task<HttpResponseData> GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "accessreview/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.AccessReviewContributor, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _accessReviewManager.Get(id);

            if (string.IsNullOrEmpty(data?.Id)) return req.NotFoundResponse();

            return await req.OkResponseAsync(data);
        }

        [Function("accessreview-request")]
        public async Task<HttpResponseData> Requeset(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "accessreview/")]
            HttpRequestData req,
           FunctionContext executionContext)
        {
            var user = req.GetRequestingUser();
            if (!_authorizationManager.IsAuthorized(AuthorizedActions.AccessReviewContributor, user)) return req.UnauthorizedResponse();

            var data = await _accessReviewManager.Request(req.GetBody<AccessReviewRequest>(),user);

            return await req.OkResponseAsync(data);
        }

        [Function("accessreview-complete")]
        public async Task<HttpResponseData> Complete(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "accessreview/{id}/complete")]
            HttpRequestData req,
           FunctionContext executionContext,
           string id)
        {
            var user = req.GetRequestingUser();
            if (!_authorizationManager.IsAuthorized(AuthorizedActions.AccessReviewContributor, user)) return req.UnauthorizedResponse();

            var data = await _accessReviewManager.Complete(id,user);

            return await req.OkResponseAsync(data);
        }

        [Function("accessreview-abandon")]
        public async Task<HttpResponseData> Abandon(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "accessreview/{id}/abandon")]
            HttpRequestData req,
           FunctionContext executionContext,
           string id)
        {
            var user = req.GetRequestingUser();
            if (!_authorizationManager.IsAuthorized(AuthorizedActions.AccessReviewContributor, user)) return req.UnauthorizedResponse();

            var data = await _accessReviewManager.Abandon(id, user);

            return await req.OkResponseAsync(data);
        }

        [Function("accessreview-applychanges")]
        public async Task<HttpResponseData> ApplyChanges(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "accessreview/{id}/actions")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {
            var user = req.GetRequestingUser();
            if (!_authorizationManager.IsAuthorized(AuthorizedActions.AccessReviewContributor, user)) return req.UnauthorizedResponse();

            var data = req.GetBody<List<AccessReviewActionRequest>>();
            var review = await _accessReviewManager.Get(id);

            return await req.OkResponseAsync(review);
        }
    }
}
