using IdentityGuard.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityGuard.Core.Mappers
{
    public class LifecyclePolicyMapper
    {
        public Shared.Models.LifecyclePolicy Map(Models.Data.LifecyclePolicyData toMap)
        {
            if (toMap == null) return null;

            return new Shared.Models.LifecyclePolicy
            {
                Id = toMap.Id,
                Name = toMap.Name,
                ObjectType = toMap.ObjectType,
                DirectoryId = toMap.DirectoryId,
                DirectoryName = toMap.DirectoryName,
                Action = toMap.Action,
                Query = toMap.Query,
                Enabled = toMap.Enabled
            };
        }

        public Models.Data.LifecyclePolicyData Map(Shared.Models.LifecyclePolicy toMap)
        {
            if (toMap == null) return null;

            return new Models.Data.LifecyclePolicyData
            {
                Id = toMap.Id,
                Name = toMap.Name,
                Area = CosmosConfiguration.DefaultPartitionKey,
                ObjectType = toMap.ObjectType,
                DirectoryId = toMap.DirectoryId,
                DirectoryName = toMap.DirectoryName,
                Action = toMap.Action,
                Query = toMap.Query,
                Enabled = toMap.Enabled
            };
        }
    }
}
