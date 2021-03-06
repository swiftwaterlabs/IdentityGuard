using IdentityGuard.Shared.Models;
using Newtonsoft.Json;
using System;

namespace IdentityGuard.Core.Models.Data
{
    public class LifecyclePolicyExecutionData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Area { get; set; }
        public string PolicyId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime Next { get; set; }
        public int AffectedObjects { get; set; }
        public LifecyclePolicyStatus Status { get; set; }
    }
}
