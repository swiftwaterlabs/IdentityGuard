using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityGuard.Api.Extensions;
using IdentityGuard.Core.Managers;
using IdentityGuard.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Functions
{
    public class ApplicationFunctions
    {
        private readonly ApplicationManager _applicationManager;
        private readonly AuthorizationManager _authorizationManager;

        public ApplicationFunctions(ApplicationManager applicationManager, AuthorizationManager authorizationManager)
        {
            _applicationManager = applicationManager;
            _authorizationManager = authorizationManager;
        }


        [Function("application-getbyid")]
        public async Task<HttpResponseData> GetById(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "application/{directoryId}/{id}")]
            HttpRequestData req,
           FunctionContext executionContext,
           string directoryId,
           string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageApplications, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _applicationManager.Get(directoryId, id);

            if (string.IsNullOrEmpty(data?.Id)) return req.NotFoundResponse();

            return await req.OkResponseAsync(data);
        }

        [Function("application-search")]
        public async Task<HttpResponseData> Search(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "application/search")]
            HttpRequestData req,
           FunctionContext executionContext)
        {
            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageApplications, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _applicationManager.Search(req.GetBody<List<string>>());

            return await req.OkResponseAsync(data);
        }

        [Function("applicationaccess-getbyid")]
        public async Task<HttpResponseData> GetAccessById(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "application/{directoryId}/{id}/access")]
            HttpRequestData req,
           FunctionContext executionContext,
           string directoryId,
           string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageApplications, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _applicationManager.GetAccess(directoryId, id);

            if (string.IsNullOrEmpty(data?.Application?.Id)) return req.NotFoundResponse();

            return await req.OkResponseAsync(data);
        }
    }
}
