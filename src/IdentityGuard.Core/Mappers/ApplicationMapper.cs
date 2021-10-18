using System.Collections.Generic;
using System.Linq;
using IdentityGuard.Core.Extensions;

namespace IdentityGuard.Core.Mappers
{
    public class ApplicationMapper
    {
        private readonly DirectoryObjectMapper _directoryObjectMapper;

        public ApplicationMapper(DirectoryObjectMapper directoryObjectMapper)
        {
            _directoryObjectMapper = directoryObjectMapper;
        }

        public Shared.Models.Application Map(Shared.Models.Directory directory, 
            Microsoft.Graph.Application toMap,
            ICollection<Microsoft.Graph.DirectoryObject> owners)
        {
            if (toMap == null) return null;

            var passwordSecrets = toMap.PasswordCredentials?.Select(Map)?.ToList() ?? new List<Shared.Models.ApplicationSecret>();
            var keySecrets = toMap.KeyCredentials?.Select(Map).ToList() ?? new List<Shared.Models.ApplicationSecret>();

            var ownerData = owners?.Select(o=>_directoryObjectMapper.Map(directory,o))?.ToList() ?? new List<Shared.Models.DirectoryObject>();

            return new Shared.Models.Application
            {
                Id = toMap.Id,
                DisplayName = toMap.DisplayName,
                DirectoryId = directory.Id,
                DirectoryName = directory.Domain,
                AppId = toMap.AppId,
                Roles = toMap.AppRoles?.Select(Map)?.Where(s => s != null).ToDictionary(r=>r.Id) ?? new Dictionary<string,Shared.Models.Role>(),
                ManagementUrl = toMap.GetPortalUrl(directory),
                Secrets = passwordSecrets.Union(keySecrets).ToList(),
                Owners = ownerData,
                Permissions = toMap.RequiredResourceAccess?.Select(Map)?.SelectMany(r=>r)?.ToList() ?? new List<Shared.Models.ApplicationPermission>(),
                
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
                Description = toMap.Description,
                Source = toMap.Origin
            };
        }

        private Shared.Models.ApplicationSecret Map(Microsoft.Graph.PasswordCredential toMap)
        {
            if (toMap == null) return null;

            return new Shared.Models.ApplicationSecret
            {
                Id = toMap.KeyId.ToString(),
                DisplayName = toMap.DisplayName,
                Type = Shared.Models.ApplicationSecretType.Password,
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
                Type = Shared.Models.ApplicationSecretType.Certificate,
                ExpiresAt = toMap.EndDateTime.GetValueOrDefault().DateTime
            };
        }

        public IEnumerable<Shared.Models.ApplicationPermission> Map(Microsoft.Graph.RequiredResourceAccess toMap)
        {
            foreach(var item in toMap.ResourceAccess)
            {
                yield return new Shared.Models.ApplicationPermission
                {
                    Id = item.Id.ToString(),
                    ResourceId = toMap.ResourceAppId,
                    ResourceName = "Unkown",
                    Type = item.Type,
                    DisplayName = "Unkown"
                };
            }
        }
    }
}
