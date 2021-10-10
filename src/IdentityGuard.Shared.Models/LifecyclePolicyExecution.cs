using System;

namespace IdentityGuard.Shared.Models
{
    public class LifecyclePolicyExecution
    {
        public string Id { get; set; }
        public string PolicyId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime Next { get; set; }
        public int AffectedObjects { get; set; }
        public LifecyclePolicyStatus Status { get; set; }
    }

    public enum LifecyclePolicyStatus
    {
        New,
        InProgress,
        Complete,
        Failed
    }
}
