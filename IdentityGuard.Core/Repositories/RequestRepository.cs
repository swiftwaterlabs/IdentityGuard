using IdentityGuard.Core.Configuration;
using IdentityGuard.Core.Mappers;
using IdentityGuard.Core.Models.Data;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Repositories
{
    public interface IRequestRepository
    {
        Task<Request> GetById(string id);
        Task<Request> Save(Request toSave);
    }

    public class RequestRepository:IRequestRepository
    {
        private readonly CosmosDbService _cosmosDbService;
        private readonly RequestMapper _requestMapper;

        public RequestRepository(CosmosDbService cosmosDbService, RequestMapper requestMapper)
        {
            _cosmosDbService = cosmosDbService;
            _requestMapper = requestMapper;
        }

        public async Task<Request> GetById(string id)
        {
            var data = await _cosmosDbService
                .Get<RequestData>(id, CosmosConfiguration.Containers.Requests, CosmosConfiguration.DefaultPartitionKey);

            var result = _requestMapper.Map(data);
            return result;
        }

        public async Task<Request> Save(Request toSave)
        {
            toSave.Id = toSave.Id ?? Guid.NewGuid().ToString();

            var data = _requestMapper.Map(toSave);
            var result = await _cosmosDbService.Save(data,
                CosmosConfiguration.Containers.Requests);

            var savedData = _requestMapper.Map(result);
            return savedData;
        }
    }
}
