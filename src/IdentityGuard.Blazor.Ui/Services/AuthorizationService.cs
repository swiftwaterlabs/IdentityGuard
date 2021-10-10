using System.Collections.Concurrent;
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

    public class CachedAuthorizationService : IAuthorizationService
    {
        private readonly AuthorizationService _authorizationService;
        private ConcurrentDictionary<string, bool> _cachedData = new();

        public CachedAuthorizationService(AuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        public async Task<bool> IsAuthorized(string action)
        {
            if (_cachedData.TryGetValue(action, out bool existing)) return existing;

            var authorized = await _authorizationService.IsAuthorized(action);
            _cachedData.AddOrUpdate(action, authorized, (key, existing) => { return authorized; });

            return authorized;
        }
    }
}