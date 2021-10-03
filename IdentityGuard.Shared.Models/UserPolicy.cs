using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityGuard.Shared.Models
{
    public class UserPolicy
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DirectoryId { get; set; }
        public string DirectoryName { get; set; }
        public UserPolicyAction Action { get; set; }
        public string Query { get; set; }
        public bool Enabled { get; set; }

        public DateTime? LastExecuted { get; set; }
        public DateTime? NextExecution { get; set; }
    }

    public enum UserPolicyAction
    {
        Audit,
        Disable,
        Delete
    }
}
