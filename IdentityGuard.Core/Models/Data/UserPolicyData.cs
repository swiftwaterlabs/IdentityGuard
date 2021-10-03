using IdentityGuard.Shared.Models;
using Newtonsoft.Json;

namespace IdentityGuard.Core.Models.Data
{
    public class UserPolicyData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string DirectoryId { get; set; }
        public string DirectoryName { get; set; }
        public UserPolicyAction Action { get; set; }
        public string Query { get; set; }
        public bool Enabled { get; set; }
    }
}
