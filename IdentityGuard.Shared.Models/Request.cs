using System;
using System.Collections.Generic;

namespace IdentityGuard.Shared.Models
{
    public class Request
    {
        public string Id { get; set; }
        public string ObjectType { get; set; }
        public string ObjectId { get; set; }
        public string DirectoryId { get; set; }
        public string Action { get; set; }
        public DirectoryObject RequestedBy { get; set; }
        public DirectoryObject CompletedBy { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public RequestStatus Status { get; set; }
        public Dictionary<string,object> AdditionalData { get; set; }
    }

    public enum RequestStatus
    {
        New,
        InProgress,
        Complete,
        Failed
    }

    public static class RequestType
    {
        public const string AccessReview = "AccessReview";
        public const string AccessReviewAction = "AccessReviewAction";
        public const string ObjectDisable = "ObjectDisable";
        public const string ObjectDelete = "ObjectDelete";
    }
}