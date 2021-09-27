using System.Linq;

namespace IdentityGuard.Core.Mappers
{
    public class DirectoryObjectMapper
    {
        public Shared.Models.DirectoryObject Map(Shared.Models.Directory directory, Microsoft.Graph.DirectoryObject toMap)
        {
            if (toMap == null) return null;

            if (toMap is Microsoft.Graph.User user)
            {
                return new Shared.Models.DirectoryObject
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    Type = "User",
                    DirectoryId =  directory.Id,
                    DirectoryName = directory.Domain,
                    ManagementUrl = directory.PortalUrl
                };
            }
            if (toMap is Microsoft.Graph.Group group)
            {
                return new Shared.Models.DirectoryObject
                {
                    Id = group.Id,
                    DisplayName = group.DisplayName,
                    Type = "Group",
                    SubType = string.Join(',', group.GroupTypes),
                    DirectoryId = directory.Id,
                    DirectoryName = directory.Domain,
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
                    DirectoryId = directory.Id,
                    DirectoryName = directory.Domain,
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
                    DirectoryId = directory.Id,
                    DirectoryName = directory.Domain,
                    ManagementUrl = directory.PortalUrl
                };
            }
            if (toMap is Microsoft.Graph.DirectoryRole directoryRole)
            {
                return new Shared.Models.DirectoryObject
                {
                    Id = directoryRole.Id,
                    DisplayName = directoryRole.DisplayName,
                    Type = "Directory Role",
                    DirectoryId = directory.Id,
                    DirectoryName = directory.Domain,
                    ManagementUrl = directory.PortalUrl
                };
            }

            return new Shared.Models.DirectoryObject
            {
                Id = toMap.Id,
                DisplayName = "Unknown",
                Type = toMap.ODataType,
                DirectoryId = directory.Id,
                DirectoryName = directory.Domain,
                ManagementUrl = directory.PortalUrl
            };
            
        }
    }
}
