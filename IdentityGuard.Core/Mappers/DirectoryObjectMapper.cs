using System.Linq;

namespace IdentityGuard.Core.Mappers
{
    public class DirectoryObjectMapper
    {
        public Shared.Models.DirectoryObject Map(Shared.Models.Directory directory, Microsoft.Graph.DirectoryObject toMap)
        {
            if (toMap == null) return null;

            if (toMap is Microsoft.Graph.Group group)
            {
                return new Shared.Models.DirectoryObject
                {
                    Id = group.Id,
                    DisplayName = group.DisplayName,
                    Type = string.Join(',',group.GroupTypes),
                    ManagementUrl = directory.PortalUrl
                };
            }
            if (toMap is Microsoft.Graph.Application application)
            {
                return new Shared.Models.DirectoryObject
                {
                    Id = application.Id,
                    DisplayName = application.DisplayName,
                    Type = "Application",
                    ManagementUrl = directory.PortalUrl
                };
            }
            if (toMap is Microsoft.Graph.ServicePrincipal servicePrincipal)
            {
                return new Shared.Models.DirectoryObject
                {
                    Id = servicePrincipal.Id,
                    DisplayName = servicePrincipal.DisplayName,
                    Type = "Service Principal",
                    ManagementUrl = directory.PortalUrl
                };
            }

            return new Shared.Models.DirectoryObject
            {
                Id = toMap.Id,
                Type = toMap.ODataType
            };
            
        }
    }
}
