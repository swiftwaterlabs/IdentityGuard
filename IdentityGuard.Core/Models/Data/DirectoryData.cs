using IdentityGuard.Shared.Models;
using Newtonsoft.Json;

namespace IdentityGuard.Core.Models.Data
{
    public class DirectoryData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Area { get; set; }
        public string TenantId { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string GraphUrl { get; set; }
        public string PortalUrl { get; set; }
        public bool IsDefault { get; set; }
        public string ClientId { get; set; }
        public DirectoryClientType ClientType { get; set; }
    }
}