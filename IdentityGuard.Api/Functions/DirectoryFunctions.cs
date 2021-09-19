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
        public Task<HttpResponseData> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "directory")]
            HttpRequestData req,
            FunctionContext executionContext)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageDirectories, req.Identities)) return req.UnauthorizedResponseAsync();

            var data = _directoryManager.Get();

            return req.OkResponseAsync(data);
        }

        [Function("directory-getbyid")]
        public Task<HttpResponseData> GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "directory/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageDirectories, req.Identities)) return req.UnauthorizedResponseAsync();

            var data = _directoryManager.GetById(id);

            return req.OkResponseAsync(data);
        }

        [Function("directory-post")]
        public Task<HttpResponseData> Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "directory/")]
            HttpRequestData req,
            FunctionContext executionContext)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageDirectories, req.Identities)) return req.UnauthorizedResponseAsync();

            var data = _directoryManager.Add(req.GetBody<Directory>());

            return req.OkResponseAsync(data);
        }

        [Function("directory-put")]
        public Task<HttpResponseData> Put(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "directory/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageDirectories, req.Identities)) return req.UnauthorizedResponseAsync();

            var data = _directoryManager.Update(id, req.GetBody<Directory>());

            return req.OkResponseAsync(data);
        }

        [Function("directory-delete")]
        public Task<HttpResponseData> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "directory/{id}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string id)
        {

            if (!_authorizationManager.IsAuthorized(AuthorizedActions.ManageDirectories, req.Identities)) return req.UnauthorizedResponseAsync();

            var data = _directoryManager.Delete(id);

            return req.OkResponseAsync(data);
        }
    }
}
