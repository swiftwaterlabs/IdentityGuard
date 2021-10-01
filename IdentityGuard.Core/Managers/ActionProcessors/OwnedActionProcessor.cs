using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers.ActionProcessors
{
    public class OwnedActionProcessor : IActionProcessor
    {
        private readonly DirectoryObjectService _directoryObjectService;
        private readonly ApplicationService _applicationService;
        private readonly ServicePrincipalService _servicePrincipalService;
        private readonly GroupService _groupService;

        public OwnedActionProcessor(DirectoryObjectService directoryObjectService,
            ApplicationService applicationService,
            ServicePrincipalService servicePrincipalService,
            GroupService groupService)
        {
            _directoryObjectService = directoryObjectService;
            _applicationService = applicationService;
            _servicePrincipalService = servicePrincipalService;
            _groupService = groupService;
        }

        public string ActionObjectType => AccessReviewActionObjectTypes.Owned;

        public async Task<IEnumerable<AccessReviewAction>> ProcessActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> actions, DirectoryObject requestingUser)
        {
            var result = new List<AccessReviewAction>();
            if (!actions.Any()) return result;

            var ownedObjectTasks = actions
                .Select(a => _directoryObjectService.Get(directory, a.ActionObjectId));

            var ownedObjects = await Task.WhenAll(ownedObjectTasks);
            var ownedObjectsByType = ownedObjects.ToLookup(o => o.Type);

            foreach(var type in ownedObjectsByType)
            {
                result.AddRange(await ProcessApplications(directory, accessReview, type, requestingUser));
                result.AddRange(await ProcessServicePrincipals(directory, accessReview, type, requestingUser));
                result.AddRange(await ProcessGroups(directory, accessReview, type, requestingUser));
            }

            return result;
        }

        private async Task<IEnumerable<AccessReviewAction>> ProcessApplications(Directory directory, AccessReview accessReview, IEnumerable<DirectoryObject> removeOwnersFrom, DirectoryObject requestingUser)
        {
            var applications = removeOwnersFrom
                .Where(d => d.Type == ObjectTypes.Application)
                .ToList();

            var removeTasks = applications
                .Select(a => _applicationService.RemoveOwners(directory, a.Id, new[] { accessReview.ObjectId }));

            await Task.WhenAll(removeTasks);

            return applications
                .Select(a => new AccessReviewAction 
                {
                    Id = Guid.NewGuid().ToString(),
                    Action = AccessReviewActionTypes.Remove,
                    Relation = AccessReviewActionObjectTypes.Owned,
                    ParentObjectId = a.Id,
                    ParentObjectType = ObjectTypes.Application,
                    ParentObjectDisplayName = a.DisplayName,
                    ActionObjectId = accessReview.ObjectId,
                    ActionObjectDisplayName = accessReview.DisplayName,
                    ActionObjectType = accessReview.ObjectType,
                    RequestedAt = DateTime.Now,
                    RequestedBy = requestingUser,
                    Status = AccessReviewActionStatus.Complete
                });
        }

        private async Task<IEnumerable<AccessReviewAction>> ProcessServicePrincipals(Directory directory, AccessReview accessReview, IEnumerable<DirectoryObject> removeOwnersFrom, DirectoryObject requestingUser)
        {
            var servicePrincipals = removeOwnersFrom
                .Where(d => d.Type == ObjectTypes.ServicePrincipal)
                .ToList();

            var removeTasks = servicePrincipals
                .Select(a => _servicePrincipalService.RemoveOwners(directory, a.Id, new[] { accessReview.ObjectId }));

            await Task.WhenAll(removeTasks);

            return servicePrincipals
                .Select(a => new AccessReviewAction
                {
                    Id = Guid.NewGuid().ToString(),
                    Action = AccessReviewActionTypes.Remove,
                    Relation = AccessReviewActionObjectTypes.Owned,
                    ParentObjectId = a.Id,
                    ParentObjectType = ObjectTypes.ServicePrincipal,
                    ParentObjectDisplayName = a.DisplayName,
                    ActionObjectId = accessReview.ObjectId,
                    ActionObjectDisplayName = accessReview.DisplayName,
                    ActionObjectType = accessReview.ObjectType,
                    RequestedAt = DateTime.Now,
                    RequestedBy = requestingUser,
                    Status = AccessReviewActionStatus.Complete
                });
        }

        private async Task<IEnumerable<AccessReviewAction>> ProcessGroups(Directory directory, AccessReview accessReview, IEnumerable<DirectoryObject> removeOwnersFrom, DirectoryObject requestingUser)
        {
            var groups = removeOwnersFrom
                .Where(d => d.Type == ObjectTypes.Group)
                .ToList();

            var removeTasks = groups
                .Select(a => _groupService.RemoveOwners(directory, a.Id, new[] { accessReview.ObjectId }));

            await Task.WhenAll(removeTasks);

            return groups
                .Select(a => new AccessReviewAction
                {
                    Id = Guid.NewGuid().ToString(),
                    Action = AccessReviewActionTypes.Remove,
                    Relation = AccessReviewActionObjectTypes.Owned,
                    ParentObjectId = a.Id,
                    ParentObjectType = ObjectTypes.Group,
                    ParentObjectDisplayName = a.DisplayName,
                    ActionObjectId = accessReview.ObjectId,
                    ActionObjectDisplayName = accessReview.DisplayName,
                    ActionObjectType = accessReview.ObjectType,
                    RequestedAt = DateTime.Now,
                    RequestedBy = requestingUser,
                    Status = AccessReviewActionStatus.Complete
                });
        }
    }
}
