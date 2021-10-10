using IdentityGuard.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Repositories
{
    public interface ILifecyclePolicyRepository
    {
        Task Delete(string id);
        Task<ICollection<LifecyclePolicy>> Get();
        Task<LifecyclePolicy> GetById(string id);
        Task<LifecyclePolicy> Save(LifecyclePolicy toSave);
    }
}
