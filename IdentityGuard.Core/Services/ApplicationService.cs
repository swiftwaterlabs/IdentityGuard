using IdentityGuard.Core.Factories;
using IdentityGuard.Core.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Services
{
    public class ApplicationService
    {
        private readonly IGraphClientFactory _graphClientFactory;
        private readonly ApplicationMapper _applicationMapper;

        public ApplicationService(IGraphClientFactory graphClientFactory,
            ApplicationMapper applicationMapper)
        {
            _graphClientFactory = graphClientFactory;
            _applicationMapper = applicationMapper;
        }

        public async Task<Shared.Models.Application> Get(Shared.Models.Directory directory, string id, bool includeOwners = false)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var data = await client
                .Applications[id]
                .Request()
                .GetAsync();

            var owners = new List<Microsoft.Graph.DirectoryObject>();
            if(includeOwners)
            {
                owners = await GetOwners(client, id);
            }
            var result = _applicationMapper.Map(directory, data, owners);

            return result;
        }

        private async Task<List<Microsoft.Graph.DirectoryObject>> GetOwners(Microsoft.Graph.IGraphServiceClient client, string id)
        {
            List<Microsoft.Graph.DirectoryObject> result = new List<Microsoft.Graph.DirectoryObject>();

            var ownersRequest = await client.Applications[id]
                .Owners
                .Request()
                .GetAsync();


            while (ownersRequest != null)
            {
                result.AddRange(ownersRequest);

                if (ownersRequest.NextPageRequest == null) break;
                ownersRequest = await ownersRequest.NextPageRequest.GetAsync();
            };

            return result;
        }

        public async Task<List<Shared.Models.Application>> Search(Shared.Models.Directory directory, string name)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var filter = GetSearchFilter(name);
            var searchRequest = await client.Applications
                .Request()
                .Filter(filter)
                .GetAsync();

            var result = new List<Microsoft.Graph.Application>();
            while (searchRequest != null)
            {
                result.AddRange(searchRequest);

                if (searchRequest.NextPageRequest == null) break;
                searchRequest = await searchRequest.NextPageRequest.GetAsync();
            };

            var applications = result
                .Select(u => _applicationMapper.Map(directory, u, new List<Microsoft.Graph.DirectoryObject>()))
                .ToList();

            return applications;
        }

        private static string GetSearchFilter(string name)
        {
            var encodedName = System.Web.HttpUtility.UrlEncode(name);
            var filter = $"startsWith(displayName,'{encodedName}')";
            return filter;
        }

        public async Task RemoveOwners(Shared.Models.Directory directory, string id, IEnumerable<string> toRemove)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var removeTasks = toRemove.Select(o => RemoveOwner(client, id, o)).ToArray();
            await Task.WhenAll(removeTasks);
        }

        private Task RemoveOwner(Microsoft.Graph.IGraphServiceClient client, string id, string ownerId)
        {
            return client.Applications[id]
                .Owners[ownerId]
                .Reference
                .Request()
                .DeleteAsync();

        }

        public async Task RemovePasswordSecrets(Shared.Models.Directory directory, string id, IEnumerable<string> toRemove)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var removeTasks = toRemove.Select(o => RemovePassword(client, id, o)).ToArray();
            await Task.WhenAll(removeTasks);
        }

        private Task RemovePassword(Microsoft.Graph.IGraphServiceClient client, string id, string toRemove)
        {
            var passwordGuid = Guid.Parse(toRemove);
            return client.Applications[id]
                .RemovePassword(passwordGuid)
                .Request()
                .PostAsync();

        }

        public async Task RemoveCertificateSecrets(Shared.Models.Directory directory, string id, IEnumerable<string> toRemove)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var removeTasks = toRemove.Select(o => RemoveKey(client, id, o)).ToArray();
            await Task.WhenAll(removeTasks);
        }

        private Task RemoveKey(Microsoft.Graph.IGraphServiceClient client, string id, string toRemove)
        {
            var keyGuid = Guid.Parse(toRemove);
            return client.Applications[id]
                .RemoveKey(keyGuid, null)
                .Request()
                .PostAsync();

        }

        public async Task RemovePermissions(Shared.Models.Directory directory, string id, string resourceId, IEnumerable<string> toRemove)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var updatedApplication = await RemovePermissionFromApplication(id, resourceId, toRemove, client);

            await client.Applications[id]
                .Request()
                .UpdateAsync(updatedApplication);

        }

        private static async Task<Microsoft.Graph.Application> RemovePermissionFromApplication(string id, string resourceId, IEnumerable<string> toRemove, Microsoft.Graph.IGraphServiceClient client)
        {
            var application = await client.Applications[id]
                .Request()
                .GetAsync();

            foreach (var resource in application.RequiredResourceAccess)
            {
                var realizedAccess = resource.ResourceAccess.ToList();
                if (resource.ResourceAppId == resourceId)
                {
                    foreach (var permissionId in toRemove)
                    {
                        var permission = resource.ResourceAccess.FirstOrDefault(a => a.Id?.ToString() == permissionId);
                        if (permission != null)
                        {
                            realizedAccess.Remove(permission);
                        }
                    }

                }
                resource.ResourceAccess = realizedAccess;
            }

            var result = CreatePermissionUpdateRequest(application);

            return result;
        }

        private static Microsoft.Graph.Application CreatePermissionUpdateRequest(Microsoft.Graph.Application application)
        {
            return new Microsoft.Graph.Application
            {
                Id = application.Id,
                RequiredResourceAccess = application.RequiredResourceAccess
            };
        }
    }
}
