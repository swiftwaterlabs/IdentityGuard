using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers.ActionProcessors
{
    public class OwnerActionProcessor : IActionProcessor
    {
        private readonly GroupService _groupService;
        private readonly ApplicationService _applicationService;
        private readonly ServicePrincipalService _servicePrincipalService;
        private readonly DirectoryObjectService _directoryObjectService;

        public OwnerActionProcessor(GroupService groupService,
            ApplicationService applicationService,
            ServicePrincipalService servicePrincipalService,
            DirectoryObjectService directoryObjectService)
        {
            _groupService = groupService;
            _applicationService = applicationService;
            _servicePrincipalService = servicePrincipalService;
            _directoryObjectService = directoryObjectService;
        }

        public string ActionObjectType => AccessReviewActionObjectTypes.Owner;

        public async Task<IEnumerable<AccessReviewAction>> ProcessActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> actions, DirectoryObject requestingUser)
        {
            var result = new List<AccessReviewAction>();
            if (!actions.Any()) return result;

            if (accessReview.ObjectType == ObjectTypes.Application)
            {
                result.AddRange(await ProcessApplication(directory, accessReview, actions, requestingUser));

            }
            else if (accessReview.ObjectType == ObjectTypes.Group)
            {
                result.AddRange(await ProcessGroup(directory, accessReview, actions, requestingUser));
            }

            return result;
        }


        private async Task<IEnumerable<AccessReviewAction>> ProcessApplication(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> actions, DirectoryObject requestingUser)
        {
            var applicationOwnerActions = actions
                .Where(a => a.ActionObjectSubType == ObjectTypes.Application);

            var servicePrincipalOwnerActions = actions
                .Where(a => a.ActionObjectSubType == ObjectTypes.ServicePrincipal);

            var applicationData = await _applicationService.Get(directory, accessReview.ObjectId);

            var results = new List<AccessReviewAction>();

            if (applicationOwnerActions.Any())
            {
                var applicationResults = await RemoveApplicationOwners(directory, accessReview, requestingUser, applicationOwnerActions, applicationData);

                results.AddRange(applicationResults);
            }

            if (servicePrincipalOwnerActions.Any())
            {
                var servicePrincipalResults = await RemoveServicePrincipalOwners(directory, requestingUser, servicePrincipalOwnerActions, applicationData);

                results.AddRange(servicePrincipalResults);
            }

            return results;
        }

        private async Task<IEnumerable<AccessReviewAction>> RemoveApplicationOwners(Directory directory, AccessReview accessReview, DirectoryObject requestingUser, IEnumerable<AccessReviewActionRequest> applicationOwnerActions, Application applicationData)
        {
            var applicationOwners = applicationOwnerActions
                .Select(a => a.ActionObjectId);

            var ownerDataTasks = applicationOwners
                .Select(a => _directoryObjectService.Get(directory, a));

            var ownerData = await Task.WhenAll(ownerDataTasks);

            await _applicationService.RemoveOwners(directory, applicationData.Id, applicationOwners);
            var applicationResults = ownerData
                .Select(a => new AccessReviewAction
                {
                    Id = Guid.NewGuid().ToString(),
                    Action = AccessReviewActionTypes.Remove,
                    Relation = AccessReviewActionObjectTypes.Owner,
                    ParentObjectId = accessReview.ObjectId,
                    ParentObjectType = ObjectTypes.Application,
                    ParentObjectDisplayName = accessReview.DisplayName,
                    ActionObjectId = a.Id,
                    ActionObjectDisplayName = a.DisplayName,
                    ActionObjectType = a.Type,
                    RequestedAt = DateTime.Now,
                    RequestedBy = requestingUser,
                    Status = AccessReviewActionStatus.Complete
                });
            return applicationResults;
        }

        private async Task<IEnumerable<AccessReviewAction>> RemoveServicePrincipalOwners(Directory directory, DirectoryObject requestingUser, IEnumerable<AccessReviewActionRequest> servicePrincipalOwnerActions, Application applicationData)
        {
            var servicePrincipalOwners = servicePrincipalOwnerActions
                .Select(a => a.ActionObjectId);

            var ownerDataTasks = servicePrincipalOwners
               .Select(a => _directoryObjectService.Get(directory, a));

            var ownerData = await Task.WhenAll(ownerDataTasks);

            var servicePrincipal = await _servicePrincipalService.GetByAppId(directory, applicationData.AppId);
            await _servicePrincipalService.RemoveOwners(directory, servicePrincipal.Id, servicePrincipalOwners);

            var servicePrincipalResults = ownerData
                .Select(a => new AccessReviewAction
                {
                    Id = Guid.NewGuid().ToString(),
                    Action = AccessReviewActionTypes.Remove,
                    Relation = AccessReviewActionObjectTypes.Owner,
                    ParentObjectId = servicePrincipal.Id,
                    ParentObjectType = ObjectTypes.ServicePrincipal,
                    ParentObjectDisplayName = servicePrincipal.DisplayName,
                    ActionObjectId = a.Id,
                    ActionObjectDisplayName = a.DisplayName,
                    ActionObjectType = a.Type,
                    RequestedAt = DateTime.Now,
                    RequestedBy = requestingUser,
                    Status = AccessReviewActionStatus.Complete
                });
            return servicePrincipalResults;
        }

        private async Task<IEnumerable<AccessReviewAction>> ProcessGroup(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> actions, DirectoryObject requestingUser)
        {
            var ownersToRemove = actions.Select(a => a.ActionObjectId);
            var groupData = await _groupService.Get(directory, accessReview.ObjectId);
            await _groupService.RemoveOwners(directory, groupData.Id, ownersToRemove);

            var results = actions
                .Select(a => new AccessReviewAction
                {
                    Id = Guid.NewGuid().ToString(),
                    Action = a.Action,
                    Relation = a.ActionObjectType,
                    ParentObjectId = groupData.Id,
                    ParentObjectType = ObjectTypes.Group,
                    ParentObjectDisplayName = groupData.DisplayName,
                    ActionObjectId = a.ActionObjectId, 
                    RequestedAt = DateTime.Now, 
                    RequestedBy = requestingUser, 
                    Status = AccessReviewActionStatus.Complete
                });

            return results;
        }
    }
}
