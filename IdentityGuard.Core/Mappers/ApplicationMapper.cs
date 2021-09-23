using System.Collections.Generic;
using System.Linq;

namespace IdentityGuard.Core.Mappers
{
    public class ApplicationMapper
    {
        public Shared.Models.Application Map(Shared.Models.Directory directory, Microsoft.Graph.Application toMap)
        {
            if (toMap == null) return null;

            var passwordSecrets = toMap.PasswordCredentials?.Select(Map)?.ToList() ?? new List<Shared.Models.ApplicationSecret>();
            var keySecrets = toMap.KeyCredentials?.Select(Map).ToList() ?? new List<Shared.Models.ApplicationSecret>();

            return new Shared.Models.Application
            {
                Id = toMap.Id,
                DisplayName = toMap.DisplayName,
                DirectoryId = directory.Id,
                DirectoryName = directory.Domain,
                AppId = toMap.AppId,
                Roles = toMap.AppRoles?.Select(Map)?.ToDictionary(r=>r.Id) ?? new Dictionary<string,Shared.Models.Role>(),
                ManagementUrl = directory.PortalUrl,
                Secrets = passwordSecrets.Union(keySecrets).ToList()
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

        private Shared.Models.ApplicationSecret Map(Microsoft.Graph.PasswordCredential toMap)
        {
            if (toMap == null) return null;

            return new Shared.Models.ApplicationSecret
            {
                Id = toMap.KeyId.ToString(),
                DisplayName = toMap.DisplayName,
                Type = "Password",
                ExpiresAt = toMap.EndDateTime.GetValueOrDefault().DateTime
            };
        }

        private Shared.Models.ApplicationSecret Map(Microsoft.Graph.KeyCredential toMap)
        {
            if (toMap == null) return null;

            return new Shared.Models.ApplicationSecret
            {
                Id = toMap.KeyId.ToString(),
                DisplayName = toMap.DisplayName,
                Type = "Certificate",
                ExpiresAt = toMap.EndDateTime.GetValueOrDefault().DateTime
            };
        }
    }
}
