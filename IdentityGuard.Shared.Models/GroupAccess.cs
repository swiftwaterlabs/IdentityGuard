using System.Collections.Generic;

namespace IdentityGuard.Shared.Models
{
    public class GroupAccess
    {
        public string DirectoryId { get; set; }
        public string DirectoryName { get; set; }
        public Group Group { get; set; }

        public List<ApplicationRole> RoleMemberships { get; set; }
    }

}
