using System.Collections.Generic;
using IdentityGuard.Shared.Models;

namespace IdentityGuard.Core.Managers
{
    public class ApplicationHealthManager
    {
        public ApplicationHealth Get()
        {
            return new ApplicationHealth
            {
                IsHealthy = true,
                DependencyHealth = new Dictionary<string, bool>
                {
                    { "Default",true}
                }
            };
        }
    }
}