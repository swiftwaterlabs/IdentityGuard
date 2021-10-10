using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityGuard.Shared.Models;

namespace IdentityGuard.Blazor.Ui.Services
{
    public interface IUserService
    {
        Task<List<KeyValuePair<string, string>>> GetUserClaims();
        Task<List<User>> Search(ICollection<string> names);
        Task<UserAccess> UserAccess(string directoryId, string id);
        Task<User> Get(string directoryId, string id);
    }

    public class UserService : AbstractHttpService, IUserService
    {
        public UserService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public Task<List<KeyValuePair<string, string>>> GetUserClaims() => Get<List<KeyValuePair<string, string>>>("api/user/claims");

        public Task<List<User>> Search(ICollection<string> names) => Post<List<User>, ICollection<string>>("api/user/search", names);

        public Task<UserAccess> UserAccess(string directoryId, string id) => Get<UserAccess>($"api/user/{directoryId}/{id}/access");
        public Task<User> Get(string directoryId, string id) => Get<User>($"api/user/{directoryId}/{id}");
    }
}