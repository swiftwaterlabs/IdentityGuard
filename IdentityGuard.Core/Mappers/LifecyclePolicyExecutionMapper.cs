using IdentityGuard.Core.Configuration;

namespace IdentityGuard.Core.Mappers
{
    public class LifecyclePolicyExecutionMapper
    {
        public Shared.Models.LifecyclePolicyExecution Map(Models.Data.LifecyclePolicyExecutionData toMap)
        {
            return new Shared.Models.LifecyclePolicyExecution
            {
                Id = toMap.Id,
                PolicyId = toMap.PolicyId,
                Start = toMap.Start,
                End = toMap.End,
                Next = toMap.Next,
                AffectedObjects = toMap.AffectedObjects,
                Status = toMap.Status
            };
        }

        public Models.Data.LifecyclePolicyExecutionData Map(Shared.Models.LifecyclePolicyExecution toMap)
        {
            return new Models.Data.LifecyclePolicyExecutionData
            {
                Id = toMap.Id,
                Area = CosmosConfiguration.DefaultPartitionKey,
                PolicyId = toMap.PolicyId,
                Start = toMap.Start,
                End = toMap.End,
                Next = toMap.Next,
                AffectedObjects = toMap.AffectedObjects,
                Status = toMap.Status
            };
        }
    }
}
