using IdentityGuard.Core.Mappers;
using IdentityGuard.Core.Repositories;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using IdentityGuard.Shared.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers
{
    public class RequestManager
    {
        private readonly IRequestRepository _requestRepository;
        private readonly RequestMapper _requestMapper;

        public RequestManager(IRequestRepository requestRepository, RequestMapper requestMapper)
        {
            _requestRepository = requestRepository;
            _requestMapper = requestMapper;
        }

        public Task<Request> Save(AccessReviewRequest accessRequest, RequestStatus status, DirectoryObject requestedBy)
        {
            var request = _requestMapper.Map(accessRequest, status, requestedBy);
            request.Status = status;

            return _requestRepository.Save(request);
        }

        public Task<Request> Save(AccessReview accessReview, IEnumerable<AccessReviewActionRequest> accessRequest, RequestStatus status, DirectoryObject requestedBy)
        {
            var request = _requestMapper.Map(accessReview, accessRequest, status, requestedBy);
            request.Status = status;

            return _requestRepository.Save(request);
        }

        public Task<Request> Save(ObjectDisableRequest disableRequest, RequestStatus status)
        {
            var request = _requestMapper.Map(disableRequest, status);
            request.Status = status;

            return _requestRepository.Save(request);
        }

        public Task<Request> Save(ObjectDeleteRequest deleteRequest, RequestStatus status)
        {
            var request = _requestMapper.Map(deleteRequest, status);
            request.Status = status;

            return _requestRepository.Save(request);
        }

        public async Task UpdateStatus(string id, RequestStatus status, DirectoryObject user)
        {
            var request = await _requestRepository.GetById(id);

            request.Status = status;

            if(status == RequestStatus.Complete)
            {
                request.CompletedBy = user;
                request.CompletedAt = ClockService.Now;
            }

            await _requestRepository.Save(request);
        }

        
    }
}
