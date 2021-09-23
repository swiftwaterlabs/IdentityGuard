using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers
{
    public class ApplicationManager
    {
        private readonly DirectoryManager _directoryManager;
        private readonly ApplicationService _applicationService;
        private readonly ServicePrincipalService _servicePrincipalService;

        public ApplicationManager(DirectoryManager directoryManager, 
            ApplicationService applicationService, 
            ServicePrincipalService servicePrincipalService)
        {
            _directoryManager = directoryManager;
            _applicationService = applicationService;
            _servicePrincipalService = servicePrincipalService;
        }

        public async Task<Application> Get(string directoryId, string id, bool includeOwners=false)
        {
            var directory = await _directoryManager.GetById(directoryId);

            var application = await _applicationService.Get(directory, id, includeOwners:includeOwners);
            var servicePrincipal = await _servicePrincipalService.GetByAppId(directory,application.AppId, includeOwners: includeOwners);

            application.ServicePrincipal = servicePrincipal;

            return application;
        }

        public async Task<List<Application>> Search(List<string> names)
        {
            if (names == null || !names.Any()) return new List<Application>();

            var directories = await _directoryManager.Get();

            var searchTasks = directories
                .AsParallel()
                .Select(directory => SearchDirectory(directory, names));

            var searchResult = await Task.WhenAll(searchTasks);

            var result = searchResult
                .SelectMany(r => r)
                .ToList();

            return result;
        }

        private async Task<List<Shared.Models.Application>> SearchDirectory(Directory directory, List<string> names)
        {
            var nameTasks = names
                .AsParallel()
                .Select(name => _applicationService.Search(directory, name));

            var nameResult = await Task.WhenAll(nameTasks);

            var results = nameResult.SelectMany(r => r);

            var uniqueResults = results
                .ToLookup(u => $"{u.DirectoryId}|{u.Id}")
                .Select(u => u.First())
                .ToList();

            return uniqueResults;
        }
    
        public async Task<ApplicationAccess> GetAccess(string directoryId, string id)
        {
            var directory = await _directoryManager.GetById(directoryId);
            var application = await Get(directoryId, id, includeOwners:true);

            return new ApplicationAccess
            {
                DirectoryId = directory.Id,
                DirectoryName = directory.Domain,
                Application = application
            };
        }
    }
}
