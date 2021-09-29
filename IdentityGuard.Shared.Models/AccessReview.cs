using System;
using System.Collections.Generic;

namespace IdentityGuard.Shared.Models
{
    public class AccessReview
    {
        public string Id { get; set; }
        public string ObjectType { get; set; }
        public string DirectoryId { get; set; }
        public string DirectoryName { get; set; }
        public string ObjectId { get; set; }
        public string DisplayName { get; set; }
        public DirectoryObject CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<DirectoryObject> AssignedTo { get; set; }
        public DirectoryObject CompletedBy { get; set; }
        public AccessReviewStatus Status { get; set; }

        public List<AcessReviewAction> Actions { get; set; }
    }

    public enum AccessReviewStatus
    {
        New,
        InProgress,
        Complete,
        Abandoned
    }
}