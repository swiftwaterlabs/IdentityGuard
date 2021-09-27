using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityGuard.Shared.Models;

namespace IdentityGuard.Core.Repositories
{
    public interface IAccessReviewRepository
    {
        Task Delete(string id);
        Task<ICollection<AccessReview>> Get(string userId, params AccessReviewStatus[] status);
        Task<AccessReview> GetById(string id);
        Task<AccessReview> Save(AccessReview toSave);
    }
}