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
        private readonly GroupService _groupService;
        private readonly DirectoryManager _directoryManager;
        private readonly RequestManager _requestManager;

        public AccessReviewManager(IAccessReviewRepository accessReviewRepository,
            ApplicationService applicationService,
            UserService userService,
            GroupService groupService,
            DirectoryManager directoryManager,
            RequestManager requestManager)
        {
            _accessReviewRepository = accessReviewRepository;
            _applicationService = applicationService;
            _userService = userService;
            _groupService = groupService;
            _directoryManager = directoryManager;
            _requestManager = requestManager;
        }
        public async Task<AccessReview> Request(AccessReviewRequest request, IEnumerable<ClaimsIdentity> currentUser)
        {
            var requestingUser = currentUser.GetUser();
            var requestData = await _requestManager.Save(request, RequestStatus.New, requestingUser);

            RequestStatus status = RequestStatus.New;

            try
            {
                var directory = await _directoryManager.GetById(request.DirectoryId);

                var accessReview = new AccessReview
                {
                    Id = Guid.NewGuid().ToString(),
                    AssignedTo = request.AssignedTo,
                    ObjectId = request.ObjectId,
                    ObjectType = request.ObjectType,
                    DirectoryId = request.DirectoryId,
                    DirectoryName = directory?.Domain,
                    CreatedAt = DateTime.Now,
                    CreatedBy = requestingUser,
                    Status = AccessReviewStatus.New
                };

                await AddDisplayName(accessReview);

                var result = await _accessReviewRepository.Save(accessReview);

                await _requestManager.UpdateStatus(requestData.Id, RequestStatus.Complete, requestingUser);
                status = RequestStatus.Complete;

                return result;
            }
            catch
            {
                status = RequestStatus.Failed;
                throw;
            }
            finally
            {
                await _requestManager.UpdateStatus(requestData.Id, status, requestingUser);
                
            }
        }

        private async Task AddDisplayName(AccessReview review)
        {
            var directory = await _directoryManager.GetById(review.DirectoryId);
            if (directory == null) return;

            switch (review.ObjectType)
            {
                case ObjectTypes.Application:
                    {
                        var data = await _applicationService.Get(directory, review.ObjectId);
                        review.DisplayName = data?.DisplayName;
                        return;
                    }
                case ObjectTypes.User:
                    {
                        var data = await _userService.Get(directory, review.ObjectId);
                        review.DisplayName = data?.DisplayName;
                        return;
                    }
                case ObjectTypes.Group:
                    {
                        var data = await _groupService.Get(directory, review.ObjectId);
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

            var existing = await _accessReviewRepository.GetById(id);
            var user = currentUser.GetUser();

            if (!existing.IsUserAssignedToReview(user)) throw new UnauthorizedAccessException();

            existing.Status = AccessReviewStatus.Complete;
            existing.CompletedAt = DateTime.Now;
            existing.CompletedBy = user;

            return await _accessReviewRepository.Save(existing);
        }

        public async Task<AccessReview> Abandon(string id, IEnumerable<ClaimsIdentity> currentUser)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));

            var existing = await _accessReviewRepository.GetById(id);
            var user = currentUser.GetUser();

            if (!existing.IsUserAssignedToReview(user)) throw new UnauthorizedAccessException();

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

        public async Task<AccessReview> Get(string id, IEnumerable<ClaimsIdentity> currentUser)
        {
            var result = await _accessReviewRepository.GetById(id);

            var user = currentUser.GetUser();
            if (!result.IsUserAssignedToReview(user)) throw new UnauthorizedAccessException();

            return result;
        }

        public async Task Delete(string id, IEnumerable<ClaimsIdentity> currentUser)
        {
            var existing = await _accessReviewRepository.GetById(id);
            var user = currentUser.GetUser();

            if (!existing.IsUserAssignedToReview(user)) throw new UnauthorizedAccessException();

            await _accessReviewRepository.Delete(id);
        }


    }
}