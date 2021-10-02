using System;
using IdentityGuard.Core.Configuration;
using IdentityGuard.Core.Models.Data;
using IdentityGuard.Shared.Models;
using IdentityGuard.Tests.Shared;

namespace IdentityGuard.Tests.Shared.Extensions
{
    public static class DataExtensions
    {
        public static DirectoryData WithDirectory(this ITestBuilder root,
            string name,
            string tenantId = null,
            string domain = null,
            string graphUrl = "https://graph.microsoft.com",
            DirectoryClientType clientType = DirectoryClientType.Application,
            string clientId = "client-id",
            bool isDefault = true
        )
        {
            var data = new DirectoryData
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = tenantId  ?? Guid.NewGuid().ToString(),
                Area = CosmosConfiguration.DefaultPartitionKey,
                Name = name,
                Domain = domain ?? Guid.NewGuid().ToString(),
                GraphUrl = graphUrl,
                ClientType = clientType,
                ClientId = clientId,
                IsDefault = isDefault

            };

            root.Context.Data.Directories.AddOrUpdate(data.Id, data, (id, existing) => { return data; });

            return data;
        }
    }
}