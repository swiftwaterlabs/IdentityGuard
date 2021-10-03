using IdentityGuard.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Repositories
{
    public interface IUserPolicyRepository
    {
        Task Delete(string id);
        Task<ICollection<UserPolicy>> Get();
        Task<UserPolicy> GetById(string id);
        Task<UserPolicy> Save(UserPolicy toSave);
    }
}
