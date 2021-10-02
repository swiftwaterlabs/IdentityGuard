using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityGuard.Core.Repositories;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;

namespace IdentityGuard.Core.Managers
{
    public class ApplicationHealthManager
    {
        private readonly DirectoryRepository _directoryRepository;
        private readonly DirectoryManager _directoryManager;
        private readonly UserService _userService;

        public ApplicationHealthManager(DirectoryRepository directoryRepository,
            DirectoryManager directoryManager,
            UserService userService)
        {
            _directoryRepository = directoryRepository;
            _directoryManager = directoryManager;
            _userService = userService;
        }


        public async Task<ApplicationHealth> Get()
        {
            return new ApplicationHealth
            {
                IsHealthy = true,
                DependencyHealth = new Dictionary<string, bool>
                {
                    { "CosmosDb",await GetCosmosData()},
                    { "Graph",await GetGraphData()},
                }
            };
        }

        private async Task<bool> GetCosmosData()
        {
            try
            {
                await _directoryRepository.Get();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> GetGraphData()
        {
            try
            {
                var directories = await _directoryManager.Get();
                foreach(var directory in directories)
                {
                    await _userService.SearchUser(directory, Guid.NewGuid().ToString());
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}