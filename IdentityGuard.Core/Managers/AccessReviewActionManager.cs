using IdentityGuard.Core.Extensions;
using IdentityGuard.Core.Repositories;
using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers
{
    public class AccessReviewActionManager
    {
        private readonly IAccessReviewRepository _accessReviewRepository;
        private readonly RequestManager _requestManager;

        public AccessReviewActionManager(IAccessReviewRepository accessReviewRepository,
            RequestManager requestManager)
        {
            _accessReviewRepository = accessReviewRepository;
            _requestManager = requestManager;
        }

        public async Task<AccessReview> ApplyChanges(string id, IEnumerable<AccessReviewActionRequest> requests, IEnumerable<ClaimsIdentity> currentUser)
        {
            var existing = await _accessReviewRepository.GetById(id);

            var user = currentUser.GetUser();
            if (!existing.IsUserAssignedToReview(user)) throw new UnauthorizedAccessException();

            var request = await _requestManager.Save(existing, requests, RequestStatus.New, user);
            
            // Do work here
            
            await _requestManager.UpdateStatus(request.Id, RequestStatus.Complete, user);

            return existing;
        }


    }
}
