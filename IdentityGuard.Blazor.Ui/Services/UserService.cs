using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace IdentityGuard.Blazor.Ui.Services
{
    public interface IUserService
    {
        Task<List<KeyValuePair<string, string>>> GetUserClaims();
    }

    public class UserService : AbstractHttpService, IUserService
    {
        public UserService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public Task<List<KeyValuePair<string, string>>> GetUserClaims() => Get<List<KeyValuePair<string, string>>>("api/user/claims");

    }
}