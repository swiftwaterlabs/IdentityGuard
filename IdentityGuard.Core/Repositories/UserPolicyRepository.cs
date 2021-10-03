using IdentityGuard.Core.Configuration;
using IdentityGuard.Core.Mappers;
using IdentityGuard.Core.Models.Data;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Repositories
{
    public class UserPolicyRepository : IUserPolicyRepository
    {
        private readonly CosmosDbService _cosmosDbService;
        private readonly ICosmosLinqQueryFactory _cosmosLinqQueryFactory;
        private readonly UserPolicyMapper _userPolicyMapper;

        public UserPolicyRepository(CosmosDbService cosmosDbService, 
            ICosmosLinqQueryFactory cosmosLinqQueryFactory,
            UserPolicyMapper userPolicyMapper)
        {
            _cosmosDbService = cosmosDbService;
            _cosmosLinqQueryFactory = cosmosLinqQueryFactory;
            _userPolicyMapper = userPolicyMapper;
        }

        public Task Delete(string id)
        {
            return _cosmosDbService.Delete<UserPolicyData>(id, CosmosConfiguration.Containers.UserPolicies, CosmosConfiguration.DefaultPartitionKey);
        }

        public Task<ICollection<UserPolicy>> Get()
        {
            var query = _cosmosDbService
               .Query<UserPolicyData>(CosmosConfiguration.Containers.UserPolicies);

            var result = _cosmosDbService.ExecuteRead(query, _userPolicyMapper.Map);

            return result;
        }

        public async Task<UserPolicy> GetById(string id)
        {
            var data = await _cosmosDbService
               .Get<UserPolicyData>(id, CosmosConfiguration.Containers.UserPolicies, CosmosConfiguration.DefaultPartitionKey);

            var result = _userPolicyMapper.Map(data);
            return result;
        }

        public async Task<UserPolicy> Save(UserPolicy toSave)
        {
            var data = _userPolicyMapper.Map(toSave);
            var result = await _cosmosDbService.Save(data,
                CosmosConfiguration.Containers.UserPolicies);

            var savedData = _userPolicyMapper.Map(result);
            return savedData;
        }
    }
}
