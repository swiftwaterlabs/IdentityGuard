namespace IdentityGuard.Shared.Models
{
    public class ApplicationRole
    {
        public string Id { get; set; }

        public string AssignmentType { get; set; }
        public DirectoryObject AssignedBy { get; set; }

        public Role Role { get; set; }
        public DirectoryObject AssignedTo { get; set; }
    }

    
}
