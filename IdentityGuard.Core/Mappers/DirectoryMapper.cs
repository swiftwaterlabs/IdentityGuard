using IdentityGuard.Core.Configuration;
using IdentityGuard.Shared.Models;

namespace IdentityGuard.Core.Mappers
{
    public class DirectoryMapper
    {
        public Shared.Models.Directory Map(Models.Data.DirectoryData toMap)
        {
            return new Shared.Models.Directory
            {
                Id = toMap?.Id,
                TenantId = toMap?.TenantId,
                Name = toMap?.Name,
                Domain = toMap?.Domain,
                IsDefault = toMap?.IsDefault ?? false,
                GraphUrl = toMap?.GraphUrl,
                PortalUrl = toMap?.PortalUrl,
                ClientId = toMap?.ClientId,
                ClientType = toMap?.ClientType ?? DirectoryClientType.Application
            };
        }

        public Models.Data.DirectoryData Map(Shared.Models.Directory toMap)
        {
            return new Models.Data.DirectoryData
            {
                Id = toMap?.Id,
                TenantId = toMap?.TenantId,
                Area = CosmosConfiguration.DefaultPartitionKey,
                Name = toMap?.Name,
                Domain = toMap?.Domain,
                IsDefault = toMap?.IsDefault ?? false,
                GraphUrl = toMap?.GraphUrl,
                PortalUrl = toMap?.PortalUrl,
                ClientId = toMap?.ClientId,
                ClientType = toMap?.ClientType ?? DirectoryClientType.Application
            };
        }
    }
}