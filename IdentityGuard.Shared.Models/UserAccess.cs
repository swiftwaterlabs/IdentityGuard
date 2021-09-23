using System.Collections.Generic;

namespace IdentityGuard.Shared.Models
{
    public class UserAccess
    {
        public string DirectoryId { get; set; }
        public string DirectoryName { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }

        public List<DirectoryObject> OwnedObjects { get; set; }
        public List<DirectoryObject> GroupMembership {get;set;}
        public List<ApplicationRole> RoleMemberships { get; set; }
    }

    
}
