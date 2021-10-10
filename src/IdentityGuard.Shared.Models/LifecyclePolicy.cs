using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityGuard.Shared.Models
{
    public class LifecyclePolicy
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DirectoryId { get; set; }
        public string DirectoryName { get; set; }
        public string ObjectType { get; set; }
        public LifecyclePolicyAction Action { get; set; }
        public string Query { get; set; }
        public bool Enabled { get; set; }

        public DateTime? LastExecuted { get; set; }
        public DateTime? NextExecution { get; set; }
    }

    public enum LifecyclePolicyAction
    {
        Audit,
        Disable,
        Delete
    }
}
