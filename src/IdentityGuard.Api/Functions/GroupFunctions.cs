using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using IdentityGuard.Api.Extensions;
using IdentityGuard.Core.Managers;
using IdentityGuard.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace IdentityGuard.Api.Functions
{
    public class GroupFunctions
    {
        private readonly AuthorizationManager _authorizationManager;
        private readonly GroupManager _groupManager;

        public GroupFunctions(AuthorizationManager authorizationManager,
            GroupManager groupManager)
        {
            _authorizationManager = authorizationManager;
            _groupManager = groupManager;
        }

        [Function("group-getbyid")]
        public async Task<HttpResponseData> GetById(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "group/{directoryId}/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string directoryId,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageGroups, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _groupManager.Get(directoryId, id);

            if (string.IsNullOrEmpty(data?.Id)) return req.NotFoundResponse();

            return await req.OkResponseAsync(data);
        }

        [Function("group-search")]
        public async Task<HttpResponseData> Search(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "group/search")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageGroups, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _groupManager.Search(req.GetBody<List<string>>());

            return await req.OkResponseAsync(data);
        }

        [Function("groupaccess-getbyid")]
        public async Task<HttpResponseData> GetAccessById(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "group/{directoryId}/{id}/access")]
            HttpRequestData req,
            FunctionContext executionContext,
            string directoryId,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageGroups, req.GetRequestingUser())) return req.UnauthorizedResponse();

            var data = await _groupManager.GetAccess(directoryId, id);

            if (string.IsNullOrEmpty(data?.Group?.Id)) return req.NotFoundResponse();

            return await req.OkResponseAsync(data);
        }
    }
}
