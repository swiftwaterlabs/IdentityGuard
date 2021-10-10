using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using IdentityGuard.Shared.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers.ActionProcessors
{
    public class ApplicationPermissionActionProcessor : IActionProcessor
    {
        private readonly ApplicationService _applicationService;

        public ApplicationPermissionActionProcessor(ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public string ActionObjectType => AccessReviewActionObjectTypes.ApplicationPermission;

        public async Task<IEnumerable<AccessReviewAction>> ProcessActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> actions, DirectoryObject requestingUser)
        {
            if (!actions.Any()) return new List<AccessReviewAction>();

            var permissionIds = actions.Select(a => a.ActionObjectId).ToList();

            var application = await _applicationService.Get(directory, accessReview.ObjectId);

            var permissionsToRemove = application
                .Permissions
                .Where(p => permissionIds.Contains(p.Id));

            var permissionsByResource = permissionsToRemove.ToLookup(p => p.ResourceId);

            foreach(var resource in permissionsByResource)
            {
                var permissionIdsToRemove = resource.Select(r => r.Id);
                await _applicationService.RemovePermissions(directory, application.Id, resource.Key, permissionIdsToRemove);
            }

            return permissionsToRemove
                .Select(permission => new AccessReviewAction
                {
                    Id = Guid.NewGuid().ToString(),
                    Action = AccessReviewActionTypes.Remove,
                    Relation = AccessReviewActionObjectTypes.ApplicationPermission,
                    ParentObjectId = accessReview.ObjectId,
                    ParentObjectType = accessReview.ObjectType,
                    ParentObjectDisplayName = accessReview.DisplayName,
                    ActionObjectId = permission.Id,
                    ActionObjectDisplayName = permission.DisplayName,
                    ActionObjectType = permission.Type,
                    RequestedAt = ClockService.Now,
                    RequestedBy = requestingUser,
                    Status = AccessReviewActionStatus.Complete
                });
        }

    }
}
