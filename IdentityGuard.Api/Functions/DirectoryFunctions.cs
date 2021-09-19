using System.Threading.Tasks;
using IdentityGuard.Api.Extensions;
using IdentityGuard.Core.Managers;
using IdentityGuard.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Functions
{
    public class DirectoryFunctions
    {
        private readonly DirectoryManager _directoryManager;
        private readonly AuthorizationManager _authorizationManager;

        public DirectoryFunctions(DirectoryManager directoryManager, AuthorizationManager authorizationManager)
        {
            _directoryManager = directoryManager;
            _authorizationManager = authorizationManager;
        }

        [Function("directory-get")]
        public async Task<HttpResponseData> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "directory")]
            HttpRequestData req,
            FunctionContext executionContext)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageDirectories, req.Identities)) return req.UnauthorizedResponse();

            var data = await _directoryManager.Get();

            return await req.OkResponseAsync(data);
        }

        [Function("directory-getbyid")]
        public async Task<HttpResponseData> GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "directory/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageDirectories, req.Identities)) return req.UnauthorizedResponse();

            var data = await _directoryManager.GetById(id);

            return await req.OkResponseAsync(data);
        }

        [Function("directory-post")]
        public async Task<HttpResponseData> Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "directory/")]
            HttpRequestData req,
            FunctionContext executionContext)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageDirectories, req.Identities)) return req.UnauthorizedResponse();

            var data = await _directoryManager.Add(req.GetBody<Directory>());

            return await req.OkResponseAsync(data);
        }

        [Function("directory-put")]
        public async Task<HttpResponseData> Put(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "directory/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageDirectories, req.Identities)) return req.UnauthorizedResponse();

            var data = await _directoryManager.Update(id, req.GetBody<Directory>());

            return await req.OkResponseAsync(data);
        }

        [Function("directory-delete")]
        public async Task<HttpResponseData> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "directory/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageDirectories, req.Identities)) return req.UnauthorizedResponse();

            await _directoryManager.Delete(id);

            return await req.OkResponseAsync();
        }
    }
}
