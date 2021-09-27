using System;

namespace IdentityGuard.Shared.Models
{
    public class ApplicationSecret
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
    
}
