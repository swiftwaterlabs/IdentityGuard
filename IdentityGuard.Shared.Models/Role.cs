namespace IdentityGuard.Shared.Models
{
    public class Role
    {
        public const string DefaultAccessId = "00000000-0000-0000-0000-000000000000";

        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
    }

    
}
