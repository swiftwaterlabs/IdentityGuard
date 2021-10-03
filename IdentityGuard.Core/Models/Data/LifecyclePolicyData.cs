using IdentityGuard.Shared.Models;
using Newtonsoft.Json;
using System;

namespace IdentityGuard.Core.Models.Data
{
    public class LifecyclePolicyData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string ObjectType { get; set; }
        public string DirectoryId { get; set; }
        public string DirectoryName { get; set; }
        public LifecyclePolicyAction Action { get; set; }
        public string Query { get; set; }
        public bool Enabled { get; set; }
        public DateTime? LastExecuted { get; set; }
        public DateTime? NextExecution { get; set; }
    }
}
