using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace IdentityGuard.Blazor.Ui.Services
{
    public interface IAuthorizationService
    {
        Task<bool> IsAuthorized(string action);
    }

    public class AuthorizationService : AbstractHttpService, IAuthorizationService
    {
        public AuthorizationService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public Task<bool> IsAuthorized(string action) => Get<bool>($"api/authorization/{action}");
    }
}