using System;

namespace IdentityGuard.Shared.Models
{
    public class Directory
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string GraphUrl { get; set; }
        public string PortalUrl { get; set; }
        public bool IsDefault { get; set; }
        public bool CanManageObjects { get; set; }
        public string ClientId { get; set; }
        public DirectoryClientType ClientType { get; set; }
    }
}
