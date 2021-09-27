using System.Collections.Generic;

namespace IdentityGuard.Shared.Models
{
    public class ServicePrincipal
    {
        public string Id { get; set; }
        public string DirectoryId { get; set; }
        public string DirectoryName { get; set; }
        public string AppId { get;set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public List<DirectoryObject> Owners { get; set; }
        public Dictionary<string,Role> Roles { get; set; }
        public Dictionary<string,ApplicationPermission> Permissions { get; set; }
    }

    
}
