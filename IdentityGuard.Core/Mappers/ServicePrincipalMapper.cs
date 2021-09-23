using IdentityGuard.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace IdentityGuard.Core.Mappers
{
    public class ServicePrincipalMapper
    {
        public Shared.Models.ServicePrincipal Map(Shared.Models.Directory directory, Microsoft.Graph.ServicePrincipal toMap)
        {
            if (toMap == null) return null;

            return new Shared.Models.ServicePrincipal
            {
                Id = toMap.Id,
                DisplayName = toMap.DisplayName,
                DirectoryId = directory.Id,
                DirectoryName = directory.Domain,
                Type = toMap.ServicePrincipalType,
                AppId = toMap.AppId,
                Roles = Map(toMap.AppRoles)
            };
        }

        private Dictionary<string, Role> Map(IEnumerable<Microsoft.Graph.AppRole> toMap)
        {
            var rolesById = toMap?.Select(Map)?.ToDictionary(r => r.Id) ?? new Dictionary<string, Shared.Models.Role>();

            const string defaultRole = "00000000-0000-0000-0000-000000000000";
            if (!rolesById.ContainsKey(defaultRole))
            {
                rolesById.Add(defaultRole, new Role { Id = defaultRole, DisplayName = "Default Access" });
            }

            return rolesById;
        }

        private Shared.Models.Role Map(Microsoft.Graph.AppRole toMap)
        {
            if (toMap == null) return null;

            return new Shared.Models.Role
            {
                Id = toMap.Id.ToString(),
                DisplayName = toMap.DisplayName
            };
        }
    }
}
