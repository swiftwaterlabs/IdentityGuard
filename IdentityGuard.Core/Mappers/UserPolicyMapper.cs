using IdentityGuard.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityGuard.Core.Mappers
{
    public class UserPolicyMapper
    {
        public Shared.Models.UserPolicy Map(Models.Data.UserPolicyData toMap)
        {
            if (toMap == null) return null;

            return new Shared.Models.UserPolicy
            {
                Id = toMap.Id,
                Name = toMap.Name,
                DirectoryId = toMap.DirectoryId,
                DirectoryName = toMap.DirectoryName,
                Action = toMap.Action,
                Query = toMap.Query,
                Enabled = toMap.Enabled
            };
        }

        public Models.Data.UserPolicyData Map(Shared.Models.UserPolicy toMap)
        {
            if (toMap == null) return null;

            return new Models.Data.UserPolicyData
            {
                Id = toMap.Id,
                Name = toMap.Name,
                Area = CosmosConfiguration.DefaultPartitionKey,
                DirectoryId = toMap.DirectoryId,
                DirectoryName = toMap.DirectoryName,
                Action = toMap.Action,
                Query = toMap.Query,
                Enabled = toMap.Enabled
            };
        }
    }
}
