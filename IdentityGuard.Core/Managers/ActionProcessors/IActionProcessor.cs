using IdentityGuard.Shared.Models;
using IdentityGuard.Shared.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers.ActionProcessors
{
    public interface IActionProcessor
    {
        string ActionObjectType { get;}
        Task<IEnumerable<AccessReviewAction>> ProcessActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> requestedActions, DirectoryObject requestingUser);


    }
}
