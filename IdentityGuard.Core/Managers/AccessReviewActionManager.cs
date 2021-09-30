using IdentityGuard.Core.Extensions;
using IdentityGuard.Core.Repositories;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
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

        public AccessReviewActionManager(IAccessReviewRepository accessReviewRepository,
            RequestManager requestManager,
            DirectoryManager directoryManager,
            UserService userService,
            GroupService groupService,
            ApplicationService applicationService,
            ServicePrincipalService servicePrincipalService)
        {
            _accessReviewRepository = accessReviewRepository;
            _requestManager = requestManager;
            _directoryManager = directoryManager;
            _userService = userService;
            _groupService = groupService;
            _applicationService = applicationService;
            _servicePrincipalService = servicePrincipalService;
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
                var actions = await ProcessActions(existing, requests);

                existing.Actions ??= new List<AccessReviewAction>();
                existing.Actions.AddRange(actions);

                await _accessReviewRepository.Save(existing);

                requestStatus = RequestStatus.Complete;
            }
            catch
            {
                requestStatus = RequestStatus.Failed;
            }
            finally
            {
                await _requestManager.UpdateStatus(request.Id, requestStatus, user);
            }

            var afterUpdate = await _accessReviewRepository.GetById(id);
            return afterUpdate;
        }

        private async Task<List<AccessReviewAction>> ProcessActions(AccessReview accessReview, IEnumerable<AccessReviewActionRequest> requestedActions)
        {
            var result = new List<AccessReviewAction>();

            var directory = await _directoryManager.GetById(accessReview.DirectoryId);

            result.AddRange(await ProcessOwnedActions(directory, accessReview, requestedActions));
            result.AddRange(await ProcessOwnerActions(directory, accessReview, requestedActions));
            result.AddRange(await ProcessGroupMembershipActions(directory, accessReview, requestedActions));
            result.AddRange(await ProcessGroupMemberActions(directory, accessReview, requestedActions));
            result.AddRange(await ProcessApplicationRoleMembershipActions(directory, accessReview, requestedActions));
            result.AddRange(await ProcessApplicationSecretActions(directory, accessReview, requestedActions));
            result.AddRange(await ProcessApplicationPermissionActions(directory, accessReview, requestedActions));
            result.AddRange(await ProcessApplicationRoleActions(directory, accessReview, requestedActions));

            return result;
        }

        private async Task<IEnumerable<AccessReviewAction>> ProcessApplicationRoleActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> requestedActions)
        {
            var actions = GetActions(AccessReviewActionObjectTypes.Role, requestedActions);
            if (!actions.Any()) return new List<AccessReviewAction>();

            return new List<AccessReviewAction>();
        }

        private async Task<IEnumerable<AccessReviewAction>> ProcessApplicationPermissionActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> requestedActions)
        {
            var actions = GetActions(AccessReviewActionObjectTypes.ApplicationPermission, requestedActions);
            if (!actions.Any()) return new List<AccessReviewAction>();

            return new List<AccessReviewAction>();
        }

        private async Task<IEnumerable<AccessReviewAction>> ProcessApplicationSecretActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> requestedActions)
        {
            var actions = GetActions(AccessReviewActionObjectTypes.ApplicationSecret, requestedActions);
            if (!actions.Any()) return new List<AccessReviewAction>();

            return new List<AccessReviewAction>();
        }

        private async Task<IEnumerable<AccessReviewAction>> ProcessApplicationRoleMembershipActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> requestedActions)
        {
            var actions = GetActions(AccessReviewActionObjectTypes.ApplicationRoleMembership, requestedActions);
            if (!actions.Any()) return new List<AccessReviewAction>();

            return new List<AccessReviewAction>();
        }

        private async Task<IEnumerable<AccessReviewAction>> ProcessGroupMemberActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> requestedActions)
        {
            var actions = GetActions(AccessReviewActionObjectTypes.GroupMembers, requestedActions);
            if (!actions.Any()) return new List<AccessReviewAction>();

            return new List<AccessReviewAction>();
        }

        private async Task<IEnumerable<AccessReviewAction>> ProcessOwnerActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> requestedActions)
        {
            var result = new List<AccessReviewAction>();
            var actions = GetActions(AccessReviewActionObjectTypes.Owner, requestedActions);
            if (!actions.Any()) return result;

            if (accessReview.ObjectType == ObjectTypes.Application)
            {
                var applicationOwners = actions
                    .Where(a => a.ActionObjectSubType == ObjectTypes.Application)
                    .Select(a => a.ActionObjectId);
                var servicePrincipalOwners = actions
                    .Where(a => a.ActionObjectSubType == ObjectTypes.ServicePrincipal)
                    .Select(a => a.ActionObjectId);

                var applicationData = await _applicationService.Get(directory, accessReview.ObjectId);

                if (applicationOwners.Any())
                {
                    await _applicationService.RemoveOwners(directory, applicationData.Id, applicationOwners);
                }

                if (servicePrincipalOwners.Any())
                {
                    var servicePrincipal = await _servicePrincipalService.GetByAppId(directory, applicationData.AppId);
                    await _servicePrincipalService.RemoveOwners(directory, servicePrincipal.Id, servicePrincipalOwners);
                }
                
            }
            if (accessReview.ObjectType == ObjectTypes.Group)
            {
                var ownersToRemove = actions.Select(a => a.ActionObjectId);
                var groupData = await _groupService.Get(directory,accessReview.ObjectId);
                await _groupService.RemoveOwners(directory, groupData.Id, ownersToRemove);
            }

            return result;
        }

        private async Task<List<AccessReviewAction>> ProcessOwnedActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> requestedActions)
        {
            var actions = GetActions(AccessReviewActionObjectTypes.Owned, requestedActions);
            if (!actions.Any()) return new List<AccessReviewAction>();

            return new List<AccessReviewAction>();
        }

        private async Task<List<AccessReviewAction>> ProcessGroupMembershipActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> requestedActions)
        {
            var actions = GetActions(AccessReviewActionObjectTypes.GroupMembership, requestedActions);
            if (!actions.Any()) return new List<AccessReviewAction>();

            return new List<AccessReviewAction>();
        }

        private List<AccessReviewActionRequest> GetActions(string type,IEnumerable<AccessReviewActionRequest> requested)
        {
            return requested
                .Where(r => string.Equals(type, r.ActionObjectType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

    }
}
