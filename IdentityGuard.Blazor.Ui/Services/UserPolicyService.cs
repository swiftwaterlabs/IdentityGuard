using IdentityGuard.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Services
{
    public interface IUserPolicyService
    {
        Task<List<UserPolicy>> Get();
        Task<UserPolicy> Get(string id);
        Task<List<User>> Audit(string id);
        Task<UserPolicy> Post(UserPolicy toSave);
        Task<UserPolicy> Put(UserPolicy toSave);
        Task Delete(string id);
    }

    public class UserPolicyService : AbstractHttpService, IUserPolicyService
    {
        public UserPolicyService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public Task<List<UserPolicy>> Get() => Get<List<UserPolicy>>("api/policy/user");

        public Task<UserPolicy> Get(string id) => Get<UserPolicy>($"api/policy/user/{id}");

        public Task<List<User>> Audit(string id) => Get<List<User>>($"api/policy/user/{id}/audit");


        public Task<UserPolicy> Post(UserPolicy toSave) => Post<UserPolicy, UserPolicy>($"api/policy/user",toSave);

        public Task<UserPolicy> Put(UserPolicy toSave) => Put<UserPolicy, UserPolicy>($"api/policy/user/{toSave.Id}", toSave);

        public Task Delete(string id) => base.Delete($"api/policy/user/{id}");
    }
}