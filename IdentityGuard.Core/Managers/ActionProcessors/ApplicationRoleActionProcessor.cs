using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using IdentityGuard.Shared.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers.ActionProcessors
{
    public class ApplicationRoleActionProcessor : IActionProcessor
    {
        private readonly ApplicationService _applicationService;
        private readonly UserService _userService;
        private readonly GroupService _groupService;
        private readonly ServicePrincipalService _servicePrincipalService;

        public ApplicationRoleActionProcessor(ApplicationService applicationService,
            UserService userService,
            GroupService groupService,
            ServicePrincipalService servicePrincipalService)
        {
            _applicationService = applicationService;
            _userService = userService;
            _groupService = groupService;
            _servicePrincipalService = servicePrincipalService;
        }

        public string ActionObjectType => AccessReviewActionObjectTypes.ApplicationRoleMembership;

        public async Task<IEnumerable<AccessReviewAction>> ProcessActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> actions, DirectoryObject requestingUser)
        {
            if (!actions.Any()) return new List<AccessReviewAction>();

            if(accessReview.ObjectType == ObjectTypes.Group)
            {
                return await ProcessGroup(directory, accessReview, actions, requestingUser);
            }
            else if(accessReview.ObjectType == ObjectTypes.User)
            {
                return await ProcessUser(directory, accessReview, actions, requestingUser);
            }

            return new List<AccessReviewAction>();
        }

        private async Task<IEnumerable<AccessReviewAction>> ProcessGroup(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> actions, DirectoryObject requestingUser)
        {
            var group = await _groupService.Get(directory, accessReview.ObjectId);
            var assignments = await _groupService.GetApplicationRoles(directory, group.Id);

            var result = await RemoveAssignments(directory, accessReview, actions, assignments, requestingUser);

            return result;
        }

        private async Task<IEnumerable<AccessReviewAction>> ProcessUser(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> actions, DirectoryObject requestingUser)
        {
            var user = await _userService.Get(directory, accessReview.ObjectId);
            var assignments = await _userService.GetApplicationRoles(directory, user.Id);

            var result = await RemoveAssignments(directory, accessReview, actions, assignments, requestingUser);

            return result;
        }

        private async Task<IEnumerable<AccessReviewAction>> RemoveAssignments(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> actions, List<ApplicationRole> assignments, DirectoryObject requestingUser)
        {
            var assignmentIdsToRemove = actions.Select(a => a.ActionObjectId).ToList();
            var assignmentsToRemove = assignments
                .Where(a => assignmentIdsToRemove.Contains(a.Id));

            var assignmentsByServicePrincipal = assignmentsToRemove.ToLookup(a => a.AssignedTo.Id);

            var result = new List<AccessReviewAction>();
            foreach (var application in assignmentsByServicePrincipal)
            {
                var servicePrincipal = await _servicePrincipalService.Get(directory, application.Key);
                var applicationData = await _applicationService.GetByAppId(directory, servicePrincipal.AppId);

                var assignmentsToRemoveForApplication = application.Select(a => a.Id);

                await _servicePrincipalService.RemoveRoleAssignments(directory, servicePrincipal.Id, assignmentsToRemoveForApplication);

                result.AddRange(application.Select(assignment=>
                new AccessReviewAction
                {
                    Id = Guid.NewGuid().ToString(),
                    Action = AccessReviewActionTypes.Remove,
                    Relation = AccessReviewActionObjectTypes.ApplicationPermission,
                    ParentObjectId = accessReview.ObjectId,
                    ParentObjectType = accessReview.ObjectType,
                    ParentObjectDisplayName = accessReview.DisplayName,
                    ActionObjectId = assignment.Id,
                    ActionObjectDisplayName = applicationData.Roles.GetValueOrDefault(assignment.Role.Id)?.DisplayName,
                    ActionObjectType = assignment.AssignmentType,
                    RequestedAt = ClockService.Now,
                    RequestedBy = requestingUser,
                    Status = AccessReviewActionStatus.Complete
                }
                ));
            }
            return result;
        }
    }
}
