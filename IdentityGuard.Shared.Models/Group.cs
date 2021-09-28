using System.Collections.Generic;

namespace IdentityGuard.Shared.Models
{
    public class Group
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }

        public string DirectoryId { get; set; }
        public string DirectoryName { get; set; }

        public List<string> Types { get; set; }
        public string DynamicMembershipRule { get; set; }

        public List<DirectoryObject> Owners { get; set; }
        public List<DirectoryObject> Members { get; set; }
    }
}
