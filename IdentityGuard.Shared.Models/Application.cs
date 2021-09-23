using System.Collections.Generic;

namespace IdentityGuard.Shared.Models
{
    public class Application
    {
        public string Id { get; set; }
        public string AppId { get; set; }
        public string DisplayName { get; set; }

        public string DirectoryId { get; set; }
        public string DirectoryName { get; set; }

        public List<DirectoryObject> Owners { get; set; }

        public Dictionary<string, Role> Roles { get; set; }

        public string ManagementUrl { get; set; }

        public ServicePrincipal ServicePrincipal { get; set; }
    }

    
}
