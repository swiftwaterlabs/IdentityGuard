using System.Collections.Generic;

namespace IdentityGuard.Shared.Models
{
    public class ApplicationHealth
    {
        public bool IsHealthy { get; set; }

        public Dictionary<string,bool> DependencyHealth { get; set; }
    }
}