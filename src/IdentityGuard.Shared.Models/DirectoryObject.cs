namespace IdentityGuard.Shared.Models
{
    public class DirectoryObject
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }

        public string DirectoryId { get; set; }
        public string DirectoryName { get; set; }

        public string Type { get; set; }
        public string SubType { get; set; }
        public string ManagementUrl { get; set; }
    }
}
