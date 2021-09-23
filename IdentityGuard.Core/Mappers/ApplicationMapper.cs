using System.Collections.Generic;
using System.Linq;

namespace IdentityGuard.Core.Mappers
{
    public class ApplicationMapper
    {
        public Shared.Models.Application Map(Shared.Models.Directory directory, Microsoft.Graph.Application toMap)
        {
            if (toMap == null) return null;

            return new Shared.Models.Application
            {
                Id = toMap.Id,
                DisplayName = toMap.DisplayName,
                DirectoryId = directory.Id,
                DirectoryName = directory.Domain,
                AppId = toMap.AppId,
                Roles = toMap.AppRoles?.Select(Map)?.ToDictionary(r=>r.Id) ?? new Dictionary<string,Shared.Models.Role>(),
                ManagementUrl = directory.PortalUrl
            };
        }

        private Shared.Models.Role Map(Microsoft.Graph.AppRole toMap)
        {
            if (toMap == null) return null;
            return new Shared.Models.Role
            {
                Id = toMap.Id.ToString(),
                DisplayName = toMap.DisplayName,
                Name = toMap.Value,
                Description = toMap.Description
            };
        }
    }
}
