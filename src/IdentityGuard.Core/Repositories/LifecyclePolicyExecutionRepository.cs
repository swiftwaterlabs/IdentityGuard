using IdentityGuard.Core.Configuration;
using IdentityGuard.Core.Mappers;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Repositories
{
    public interface ILifecyclePolicyExecutionRepository
    {
        Task<LifecyclePolicyExecution> Save(LifecyclePolicyExecution toSave);
    }

    public class LifecyclePolicyExecutionRepository : ILifecyclePolicyExecutionRepository
    {
        private readonly CosmosDbService _cosmosDbService;
        private readonly LifecyclePolicyExecutionMapper _lifecyclePolicyExecutionMapper;

        public LifecyclePolicyExecutionRepository(CosmosDbService cosmosDbService, LifecyclePolicyExecutionMapper lifecyclePolicyExecutionMapper)
        {
            _cosmosDbService = cosmosDbService;
            _lifecyclePolicyExecutionMapper = lifecyclePolicyExecutionMapper;
        }

        public async Task<LifecyclePolicyExecution> Save(LifecyclePolicyExecution toSave)
        {
            toSave.Id ??= Guid.NewGuid().ToString();

            var data = _lifecyclePolicyExecutionMapper.Map(toSave);
            var result = await _cosmosDbService.Save(data,
                CosmosConfiguration.Containers.LifecyclePolicyExecutions);

            var savedData = _lifecyclePolicyExecutionMapper.Map(result);
            return savedData;
            
        }
    }
}
