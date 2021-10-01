using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers.ActionProcessors
{
    public class GroupMembershipActionProcessor : IActionProcessor
    {
        private readonly GroupService _groupService;

        public GroupMembershipActionProcessor(GroupService groupService)
        {
            _groupService = groupService;
        }

        public string ActionObjectType => AccessReviewActionObjectTypes.GroupMembership;

        public async Task<IEnumerable<AccessReviewAction>> ProcessActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> actions, DirectoryObject requestingUser)
        {
            if (!actions.Any()) return new List<AccessReviewAction>();

            var groupTasks = actions
                .Select(a => _groupService.Get(directory, a.ActionObjectId));

            var groups = await Task.WhenAll(groupTasks);

            var removeTasks = groups
                .Select(g => _groupService.RemoveMembers(directory, g.Id, new[] { accessReview.ObjectId }));

            await Task.WhenAll(removeTasks);

            return groups
               .Select(g => new AccessReviewAction
               {
                   Id = Guid.NewGuid().ToString(),
                   Action = AccessReviewActionTypes.Remove,
                   Relation = AccessReviewActionObjectTypes.GroupMembership,
                   ParentObjectId = g.Id,
                   ParentObjectType = ObjectTypes.Group,
                   ParentObjectDisplayName = g.DisplayName,
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
