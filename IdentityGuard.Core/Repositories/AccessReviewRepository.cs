using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityGuard.Core.Configuration;
using IdentityGuard.Core.Mappers;
using IdentityGuard.Core.Models.Data;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;

namespace IdentityGuard.Core.Repositories
{
    public class AccessReviewRepository:IAccessReviewRepository
    {
        private readonly CosmosDbService _cosmosDbService;
        private readonly ICosmosLinqQueryFactory _cosmosLinqQueryFactory;
        private readonly AccessReviewMapper _accessReviewMapper;

        public AccessReviewRepository(CosmosDbService cosmosDbService,
            ICosmosLinqQueryFactory cosmosLinqQueryFactory,
            AccessReviewMapper accessReviewMapper)
        {
            _cosmosDbService = cosmosDbService;
            _cosmosLinqQueryFactory = cosmosLinqQueryFactory;
            _accessReviewMapper = accessReviewMapper;
        }
        public Task Delete(string id)
        {
            var result = _cosmosDbService.Delete<AccessReviewData>(id,
                CosmosConfiguration.Containers.AccessReviews,
                CosmosConfiguration.DefaultPartitionKey);

            return result;
        }

        public Task<ICollection<AccessReview>> Get()
        {
            var query = _cosmosDbService
                .Query<AccessReviewData>(CosmosConfiguration.Containers.AccessReviews);

            var result = _cosmosDbService.ExecuteRead(query, _accessReviewMapper.Map);

            return result;
        }

        public async Task<AccessReview> Get(string id)
        {
            var data = await _cosmosDbService
                .Get<AccessReviewData>(id,CosmosConfiguration.Containers.AccessReviews,CosmosConfiguration.DefaultPartitionKey);

            var result = _accessReviewMapper.Map(data);
            return result;
        }

        public async Task<AccessReview> Save(AccessReview toSave)
        {
            var data = _accessReviewMapper.Map(toSave);
            var result = await _cosmosDbService.Save(data,
                CosmosConfiguration.Containers.AccessReviews);

            var savedData = _accessReviewMapper.Map(result);
            return savedData;
        }
    }
}