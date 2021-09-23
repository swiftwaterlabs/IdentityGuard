using System.Collections.Generic;

namespace IdentityGuard.Shared.Models
{
    public class ApplicationAccess
    {
        public string DirectoryId { get; set; }
        public string DirectoryName { get; set; }
        public Application Application { get; set; }

        public List<ApplicationRole> RoleMemberships { get; set; }

        public List<ApplicationSecret> Secrets { get; set; }
    }
    
}
