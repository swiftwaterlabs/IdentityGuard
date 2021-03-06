using IdentityGuard.Core.Extensions;
using IdentityGuard.Core.Managers.ActionProcessors;
using IdentityGuard.Core.Repositories;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using IdentityGuard.Shared.Models.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers
{
    public class AccessReviewActionManager
    {
        private readonly IAccessReviewRepository _accessReviewRepository;
        private readonly RequestManager _requestManager;
        private readonly DirectoryManager _directoryManager;
        private readonly UserService _userService;
        private readonly GroupService _groupService;
        private readonly ApplicationService _applicationService;
        private readonly ServicePrincipalService _servicePrincipalService;
        private readonly IEnumerable<IActionProcessor> _processors;
        private readonly ILogger<AccessReviewActionManager> _logger;

        public AccessReviewActionManager(IAccessReviewRepository accessReviewRepository,
            RequestManager requestManager,
            DirectoryManager directoryManager,
            UserService userService,
            GroupService groupService,
            ApplicationService applicationService,
            ServicePrincipalService servicePrincipalService,
            IEnumerable<IActionProcessor> processors,
            ILogger<AccessReviewActionManager> logger)
        {
            _accessReviewRepository = accessReviewRepository;
            _requestManager = requestManager;
            _directoryManager = directoryManager;
            _userService = userService;
            _groupService = groupService;
            _applicationService = applicationService;
            _servicePrincipalService = servicePrincipalService;
            _processors = processors;
            _logger = logger;
        }

        public async Task<AccessReview> ApplyChanges(string id, IEnumerable<AccessReviewActionRequest> requests, IEnumerable<ClaimsIdentity> currentUser)
        {
            var existing = await _accessReviewRepository.GetById(id);

            var user = currentUser.GetUser();
            if (!existing.IsUserAssignedToReview(user)) throw new UnauthorizedAccessException();

            if (!requests.Any()) return existing;

            var requestStatus = RequestStatus.New;
            var request = await _requestManager.Save(existing, requests, requestStatus, user);

            try
            {
                var actions = await ProcessActions(existing, requests, user);

                existing.Actions ??= new List<AccessReviewAction>();
                existing.Actions.AddRange(actions);

                await _accessReviewRepository.Save(existing);

                requestStatus = RequestStatus.Complete;
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                requestStatus = RequestStatus.Failed;
            }
            finally
            {
                await _requestManager.UpdateStatus(request.Id, requestStatus, user);
            }

            var afterUpdate = await _accessReviewRepository.GetById(id);
            return afterUpdate;
        }

        private async Task<List<AccessReviewAction>> ProcessActions(AccessReview accessReview, 
            IEnumerable<AccessReviewActionRequest> requestedActions,
            DirectoryObject currentUser)
        {
            var directory = await _directoryManager.GetById(accessReview.DirectoryId);

            if (!directory.CanManageObjects) throw new UnauthorizedAccessException($"Directory {directory?.Domain} does not support management of objects");

            var result = new List<AccessReviewAction>();
            foreach(var processor in _processors)
            {
                var actions = GetActions(processor.ActionObjectType, requestedActions);
                if(actions.Any())
                {
                    var actionResults = await processor.ProcessActions(directory, accessReview, actions, currentUser);

                    result.AddRange(actionResults);
                }
            }
            
            return result;
        }

        private List<AccessReviewActionRequest> GetActions(string type,IEnumerable<AccessReviewActionRequest> requested)
        {
            return requested
                .Where(r => string.Equals(type, r.ActionObjectType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

    }
}
