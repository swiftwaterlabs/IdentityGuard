namespace IdentityGuard.Core.Mappers
{
    public class ApplicationRoleMapper
    {
        public Shared.Models.ApplicationRole Map(Shared.Models.Directory directory, Microsoft.Graph.AppRoleAssignment toMap)
        {
            if (toMap == null) return null;

            return new Shared.Models.ApplicationRole
            {
                Id = toMap.Id,
                Role = new Shared.Models.Role
                {
                    Id = toMap.AppRoleId.ToString(),
                    DisplayName = "Unknown",
                },
                AssignmentType = toMap.PrincipalType,
                AssignedBy = new Shared.Models.DirectoryObject
                {
                    Id = toMap.PrincipalId.ToString(),
                    DisplayName = toMap.PrincipalDisplayName,
                    Type = toMap.PrincipalType,
                    ManagementUrl = directory.PortalUrl
                }, 
                AssignedTo = new Shared.Models.DirectoryObject
                {
                    Id = toMap.ResourceId.ToString(),
                    DisplayName = toMap.ResourceDisplayName,
                    Type = "Application",
                    ManagementUrl = directory.PortalUrl
                }
            };
        }
    }
}
