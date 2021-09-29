using System;

namespace IdentityGuard.Shared.Models
{
    public class AccessReviewAction
    {
        public string Id { get; set; }
        public string Action { get; set; }

        public string ParentObjectType { get; set; }
        public string ParentObjectId { get; set; }
        public string ParentObjectDisplayName { get; set; }

        public string ActionObjectType { get; set; }
        public string ActionObjectId { get; set; }
        public string ActionObjectDisplayName { get; set; }

        public AccessReviewActionStatus Status { get; set; }
        public DirectoryObject RequestedBy { get; set; }
        public DateTime RequestedAt { get; set; }

    }

    public class AccessReviewActionRequest
    {
        public string Action { get; set; }
        public string ActionObjectType { get; set; }
        public string ActionObjectId { get; set; }

    }


    public enum AccessReviewActionStatus
    {
        Pending,
        Complete,
        Failed
    }

    public static class AccessReviewActionTypes
    {
        public const string Remove = "Remove";
    }
}