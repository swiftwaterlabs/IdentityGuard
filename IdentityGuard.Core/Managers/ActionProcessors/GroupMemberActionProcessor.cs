using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers.ActionProcessors
{
    public class GroupMemberActionProcessor : IActionProcessor
    {
        private readonly GroupService _groupService;
        private readonly DirectoryObjectService _directoryObjectService;

        public GroupMemberActionProcessor(GroupService groupService,
            DirectoryObjectService directoryObjectService)
        {
            _groupService = groupService;
            _directoryObjectService = directoryObjectService;
        }
        public string ActionObjectType => AccessReviewActionObjectTypes.GroupMembers;

        public async Task<IEnumerable<AccessReviewAction>> ProcessActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> actions, DirectoryObject requestingUser)
        {
            if (!actions.Any()) return new List<AccessReviewAction>();

            var memberTasks = actions
                .Select(a => _directoryObjectService.Get(directory, a.ActionObjectId));

            var membersToRemove = await Task.WhenAll(memberTasks);

            var memberObjectIds = membersToRemove
                .Select(o => o.Id);

            await _groupService.RemoveMembers(directory, accessReview.ObjectId, memberObjectIds);

            return membersToRemove
                .Select(m=>new AccessReviewAction 
                {
                    Id = Guid.NewGuid().ToString(),
                    Action = AccessReviewActionTypes.Remove,
                    Relation = AccessReviewActionObjectTypes.GroupMembers,
                    ParentObjectId = accessReview.ObjectId,
                    ParentObjectType = accessReview.ObjectType,
                    ParentObjectDisplayName = accessReview.DisplayName,
                    ActionObjectId = m.Id,
                    ActionObjectDisplayName = m.DisplayName,
                    ActionObjectType = m.Type,
                    RequestedAt = ClockService.Now,
                    RequestedBy = requestingUser,
                    Status = AccessReviewActionStatus.Complete
                });
        }
    }
}
