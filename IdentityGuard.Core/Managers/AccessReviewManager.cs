using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityGuard.Core.Repositories;
using IdentityGuard.Shared.Models;
using IdentityGuard.Core.Extensions;
using System.Linq;

namespace IdentityGuard.Core.Managers
{
    public class AccessReviewManager
    {
        private readonly IAccessReviewRepository _accessReviewRepository;

        public AccessReviewManager(IAccessReviewRepository accessReviewRepository)
        {
            _accessReviewRepository = accessReviewRepository;
        }
        public Task<AccessReview> Add(AccessReview toSave)
        {
            toSave.Id = Guid.NewGuid().ToString();

            return _accessReviewRepository.Save(toSave);
        }

        public async Task<AccessReview> Complete(string id, IEnumerable<ClaimsIdentity> currentUser)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            
            var existing = await Get(id);
            var user = GetUser(currentUser);

            if (!IsUserAssignedToReview(existing,user)) throw new UnauthorizedAccessException();

            existing.Status = AccessReviewStatus.Complete;
            existing.CompletedAt = DateTime.Now;
            existing.CompletedBy = user;

            return await _accessReviewRepository.Save(existing);
        }

        private bool IsUserAssignedToReview(AccessReview review, DirectoryObject user)
        {
            return review.AssignedTo.Any(a => a.Id == user.Id);
        }

        private DirectoryObject GetUser(IEnumerable<ClaimsIdentity> currentUser)
        {
            return new DirectoryObject
            {
                Id = currentUser.GetUserId(),
                DirectoryId = currentUser.GetUserDirectoryId()
            };
        }

        public async Task<AccessReview> Abandon(string id, IEnumerable<ClaimsIdentity> currentUser)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));

            var existing = await Get(id);
            var user = GetUser(currentUser);

            if (!IsUserAssignedToReview(existing, user)) throw new UnauthorizedAccessException();

            existing.Status = AccessReviewStatus.Abandoned;
            existing.CompletedAt = DateTime.Now;
            existing.CompletedBy = user;

            return await _accessReviewRepository.Save(existing);
        }

        public Task<ICollection<AccessReview>> GetPending(IEnumerable<ClaimsIdentity> currentUser)
        {
            var userId = currentUser.GetUserId();

            if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException();

            var results = _accessReviewRepository.Get(userId, AccessReviewStatus.New, AccessReviewStatus.InProgress);

            return results;
        }

        public Task<ICollection<AccessReview>> GetCompleted(IEnumerable<ClaimsIdentity> currentUser)
        {
            var userId = currentUser.GetUserId();

            if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException();

            var results = _accessReviewRepository.Get(userId, AccessReviewStatus.Complete);

            return results;
        }

        public Task<AccessReview> Get(string id)
        {
            var result = _accessReviewRepository.GetById(id);

            return result;
        }

        public Task Delete(string id)
        {
            return _accessReviewRepository.Delete(id);
        }


    }
}