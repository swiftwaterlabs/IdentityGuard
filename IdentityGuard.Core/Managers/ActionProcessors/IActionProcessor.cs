﻿using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers.ActionProcessors
{
    public interface IActionProcessor
    {
        string ActionObjectType { get;}
        Task<IEnumerable<AccessReviewAction>> ProcessApplicationRoleActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> requestedActions, DirectoryObject requestingUser);


    }
}
