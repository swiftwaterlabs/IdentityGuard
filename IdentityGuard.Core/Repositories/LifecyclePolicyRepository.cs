using IdentityGuard.Core.Configuration;
using IdentityGuard.Core.Mappers;
using IdentityGuard.Core.Models.Data;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Repositories
{
    public class LifecyclePolicyRepository : ILifecyclePolicyRepository
    {
        private readonly CosmosDbService _cosmosDbService;
        private readonly ICosmosLinqQueryFactory _cosmosLinqQueryFactory;
        private readonly LifecyclePolicyMapper _userPolicyMapper;

        public LifecyclePolicyRepository(CosmosDbService cosmosDbService, 
            ICosmosLinqQueryFactory cosmosLinqQueryFactory,
            LifecyclePolicyMapper userPolicyMapper)
        {
            _cosmosDbService = cosmosDbService;
            _cosmosLinqQueryFactory = cosmosLinqQueryFactory;
            _userPolicyMapper = userPolicyMapper;
        }

        public Task Delete(string id)
        {
            return _cosmosDbService.Delete<LifecyclePolicyData>(id, CosmosConfiguration.Containers.LifecyclePolicies, CosmosConfiguration.DefaultPartitionKey);
        }

        public Task<ICollection<LifecyclePolicy>> Get()
        {
            var query = _cosmosDbService
               .Query<LifecyclePolicyData>(CosmosConfiguration.Containers.LifecyclePolicies);

            var result = _cosmosDbService.ExecuteRead(query, _userPolicyMapper.Map);

            return result;
        }

        public async Task<LifecyclePolicy> GetById(string id)
        {
            var data = await _cosmosDbService
               .Get<LifecyclePolicyData>(id, CosmosConfiguration.Containers.LifecyclePolicies, CosmosConfiguration.DefaultPartitionKey);

            var result = _userPolicyMapper.Map(data);
            return result;
        }

        public async Task<LifecyclePolicy> Save(LifecyclePolicy toSave)
        {
            var data = _userPolicyMapper.Map(toSave);
            var result = await _cosmosDbService.Save(data,
                CosmosConfiguration.Containers.LifecyclePolicies);

            var savedData = _userPolicyMapper.Map(result);
            return savedData;
        }
    }
}
