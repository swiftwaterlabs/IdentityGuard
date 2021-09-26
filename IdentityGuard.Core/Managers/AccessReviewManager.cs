using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityGuard.Core.Repositories;
using IdentityGuard.Shared.Models;

namespace IdentityGuard.Core.Managers
{
    public class AccessReviewManager
    {
        private readonly AccessReviewRepository _accessReviewRepository;

        public AccessReviewManager(AccessReviewRepository accessReviewRepository)
        {
            _accessReviewRepository = accessReviewRepository;
        }
        public Task Add(AccessReview toSave)
        {
            toSave.Id = Guid.NewGuid().ToString();

            return _accessReviewRepository.Save(toSave);
        }

        public Task<AccessReview> Complete(AccessReview toSave, IEnumerable<ClaimsIdentity> currentUser)
        {
            return _accessReviewRepository.Save(toSave);
        }

        public Task<ICollection<AccessReview>> GetPending(IEnumerable<ClaimsIdentity> currentUser)
        {

        }

        public Task<ICollection<AccessReview>> GetCompleted(IEnumerable<ClaimsIdentity> currentUser)
        {

        }

        public Task<AccessReview> Get(string id)
        {
            var result = _accessReviewRepository.Get(id);

            return result;
        }

        public Task Delete(string id)
        {
            return _accessReviewRepository.Delete(id);
        }


    }
}