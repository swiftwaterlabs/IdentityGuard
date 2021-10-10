using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdentityGuard.Core.Extensions
{
    public static class AccessReviewExtensions
    {
        public static bool IsUserAssignedToReview(this AccessReview review, DirectoryObject user)
        {
            return review.AssignedTo.Any(a => a.Id == user.Id);
        }
    }
}
