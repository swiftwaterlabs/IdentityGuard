using IdentityGuard.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Services
{
    public interface ILifecyclePolicyService
    {
        Task<List<LifecyclePolicy>> Get();
        Task<LifecyclePolicy> Get(string id);
        Task<List<User>> Audit(string id);
        Task<LifecyclePolicy> Post(LifecyclePolicy toSave);
        Task<LifecyclePolicy> Put(LifecyclePolicy toSave);
        Task Delete(string id);
    }

    public class LifecyclePolicyService : AbstractHttpService, ILifecyclePolicyService
    {
        public LifecyclePolicyService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public Task<List<LifecyclePolicy>> Get() => Get<List<LifecyclePolicy>>("api/lifecycle/policy");

        public Task<LifecyclePolicy> Get(string id) => Get<LifecyclePolicy>($"api/lifecycle/policy/{id}");

        public Task<List<User>> Audit(string id) => Get<List<User>>($"api/lifecycle/policy/{id}/audit");


        public Task<LifecyclePolicy> Post(LifecyclePolicy toSave) => Post<LifecyclePolicy, LifecyclePolicy>($"api/lifecycle/policy",toSave);

        public Task<LifecyclePolicy> Put(LifecyclePolicy toSave) => Put<LifecyclePolicy, LifecyclePolicy>($"api/lifecycle/policy/{toSave.Id}", toSave);

        public Task Delete(string id) => base.Delete($"api/lifecycle/policy/{id}");
    }
}