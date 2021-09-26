using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityGuard.Core.Repositories;
using IdentityGuard.Shared.Models;
using IdentityGuard.Core.Extensions;
using System.Linq;
using IdentityGuard.Core.Services;

namespace IdentityGuard.Core.Managers
{
    public class AccessReviewManager
    {
        private readonly IAccessReviewRepository _accessReviewRepository;
        private readonly ApplicationService _applicationService;
        private readonly UserService _userService;
        private readonly DirectoryManager _directoryManager;

        public AccessReviewManager(IAccessReviewRepository accessReviewRepository,
            ApplicationService applicationService,
            UserService userService,
            DirectoryManager directoryManager)
        {
            _accessReviewRepository = accessReviewRepository;
            _applicationService = applicationService;
            _userService = userService;
            _directoryManager = directoryManager;
        }
        public async Task<AccessReview> Request(AccessReviewRequest request, IEnumerable<ClaimsIdentity> currentUser)
        {
            var accessReview = new AccessReview
            {
                Id = Guid.NewGuid().ToString(),
                AssignedTo = request.AssignedTo,
                ObjectId = request.ObjectId,
                ObjectType = request.ObjectType,
                DirectoryId = request.DirectoryId,
                CreatedAt = DateTime.Now,
                CreatedBy = GetUser(currentUser),
                Status = AccessReviewStatus.New
            };

            await AddDisplayName(accessReview);

            var result = await _accessReviewRepository.Save(accessReview);
            return result;
        }

        private async Task AddDisplayName(AccessReview review)
        {
            var directory = await _directoryManager.GetById(review.DirectoryId);
            if (directory == null) return;

            switch (review.ObjectType.ToLower())
            {
                case "application":
                    {
                        var data = await _applicationService.Get(directory, review.ObjectId);
                        review.DisplayName = data?.DisplayName;
                        return;
                    }
                case "user":
                    {
                        var data = await _userService.Get(directory, review.ObjectId);
                        review.DisplayName = data?.DisplayName;
                        return;
                    }
                default:
                    {
                        return;
                    }
            }
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