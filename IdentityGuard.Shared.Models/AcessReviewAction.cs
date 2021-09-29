using System;

namespace IdentityGuard.Shared.Models
{
    public class AcessReviewAction
    {
        public string Id { get; set; }
        public string Action { get; set; }
        public string ActionObjectType { get; set; }
        public string ActionObjectId { get; set; }
        public AccessReviewActionStatus Status { get; set; }
        public DirectoryObject RequestedBy { get; set; }
        public DateTime RequestedAt { get; set; }

    }


    public enum AccessReviewActionStatus
    {
        Pending,
        Complete,
        Failed
    }
}