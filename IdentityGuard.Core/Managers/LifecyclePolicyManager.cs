using IdentityGuard.Core.Repositories;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers
{
    public class LifecyclePolicyManager
    {
        private readonly ILifecyclePolicyRepository _userPolicyRepository;
        private readonly DirectoryManager _directoryManager;
        private readonly UserService _userService;
        private readonly RequestManager _requestManager;
        private readonly ILogger<LifecyclePolicyManager> _logger;
        private readonly ILifecyclePolicyExecutionRepository _lifecyclePolicyExecutionRepository;

        public LifecyclePolicyManager(ILifecyclePolicyRepository userPolicyRepository,
            DirectoryManager directoryManager,
            UserService userService,
            RequestManager requestManager,
            ILogger<LifecyclePolicyManager> logger,
            ILifecyclePolicyExecutionRepository lifecyclePolicyExecutionRepository)
        {
            _userPolicyRepository = userPolicyRepository;
            _directoryManager = directoryManager;
            _userService = userService;
            _requestManager = requestManager;
            _logger = logger;
            _lifecyclePolicyExecutionRepository = lifecyclePolicyExecutionRepository;
        }

        public Task<ICollection<LifecyclePolicy>> Get()
        {
            return _userPolicyRepository.Get();
        }

        public Task<LifecyclePolicy> Get(string id)
        {
            return _userPolicyRepository.GetById(id);
        }

        public async Task<LifecyclePolicy> Add(LifecyclePolicy toAdd)
        {
            toAdd.Id = Guid.NewGuid().ToString();
            await ApplyDirectory(toAdd);

            var result = await _userPolicyRepository.Save(toAdd);
            return result;
        }

        public async Task<LifecyclePolicy> Update(string id, LifecyclePolicy toUpdate)
        {
            toUpdate.Id = id;
            await ApplyDirectory(toUpdate);

            var result = await _userPolicyRepository.Save(toUpdate);
            return result;
        }

        private async Task ApplyDirectory(LifecyclePolicy toApply)
        {
            var directory = await _directoryManager.GetById(toApply.DirectoryId);
            toApply.DirectoryName = directory?.Domain;
        }

        public Task Delete(string id)
        {
            return _userPolicyRepository.Delete(id);
        }

        

    }
}
