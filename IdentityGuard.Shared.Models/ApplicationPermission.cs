namespace IdentityGuard.Shared.Models
{
    public class ApplicationPermission
    {
        public string Id { get; set; }
        public string ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
    
}
