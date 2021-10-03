using IdentityGuard.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Services
{
    public interface IUserPolicyService
    {
        Task<List<LifecyclePolicy>> Get();
        Task<LifecyclePolicy> Get(string id);
        Task<List<User>> Audit(string id);
        Task<LifecyclePolicy> Post(LifecyclePolicy toSave);
        Task<LifecyclePolicy> Put(LifecyclePolicy toSave);
        Task Delete(string id);
    }

    public class UserPolicyService : AbstractHttpService, IUserPolicyService
    {
        public UserPolicyService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public Task<List<LifecyclePolicy>> Get() => Get<List<LifecyclePolicy>>("api/policy/user");

        public Task<LifecyclePolicy> Get(string id) => Get<LifecyclePolicy>($"api/policy/user/{id}");

        public Task<List<User>> Audit(string id) => Get<List<User>>($"api/policy/user/{id}/audit");


        public Task<LifecyclePolicy> Post(LifecyclePolicy toSave) => Post<LifecyclePolicy, LifecyclePolicy>($"api/policy/user",toSave);

        public Task<LifecyclePolicy> Put(LifecyclePolicy toSave) => Put<LifecyclePolicy, LifecyclePolicy>($"api/policy/user/{toSave.Id}", toSave);

        public Task Delete(string id) => base.Delete($"api/policy/user/{id}");
    }
}