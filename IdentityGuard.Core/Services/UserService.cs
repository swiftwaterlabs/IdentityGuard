using IdentityGuard.Core.Factories;
using IdentityGuard.Core.Mappers;
using IdentityGuard.Core.Models;
using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Services
{
    public class UserService
    {
        private readonly IGraphClientFactory _graphClientFactory;
        private readonly UserMapper _userMapper;
        private readonly DirectoryObjectMapper _directoryObjectMapper;
        private readonly ApplicationRoleMapper _applicationRoleMapper;

        public UserService(IGraphClientFactory graphClientFactory, 
            UserMapper userMapper,
            DirectoryObjectMapper directoryObjectMapper,
            ApplicationRoleMapper applicationRoleMapper)
        {
            _graphClientFactory = graphClientFactory;
            _userMapper = userMapper;
            _directoryObjectMapper = directoryObjectMapper;
            _applicationRoleMapper = applicationRoleMapper;
        }

        public async Task<Shared.Models.User> Get(Shared.Models.Directory directory, string id)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var data = await client.Users[id]
                .Request()
                .GetAsync();

            var user = _userMapper.Map(directory, data);
            return user;
        }

        public Task<List<User>> SearchUser(Shared.Models.Directory directory, string name)
        {
            var filter = GetSearchFilter(name);
            return Query(directory, filter);
        }

        public async Task<List<User>> Query(Shared.Models.Directory directory, string filter)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var searchRequest = await client.Users
                .Request()
                .Filter(filter)
                .GetAsync();

            var result = new List<Microsoft.Graph.User>();
            while (searchRequest != null)
            {
                result.AddRange(searchRequest);

                if (searchRequest.NextPageRequest == null) break;
                searchRequest = await searchRequest.NextPageRequest.GetAsync();
            };

            var users = result
                .Select(u => _userMapper.Map(directory, u))
                .ToList();

            return users;
        }

        private static string GetSearchFilter(string name)
        {
            var encodedName = System.Web.HttpUtility.UrlEncode(name);

            var searchFields = new[]
            {
                "userPrincipalName",
                "mail",
                "displayName"
            };

            var searchClauses = searchFields
                .Select(field => $"startswith({field},'{encodedName}')");

            var result = string.Join(" or ", searchClauses);
            return result;

        }

        public async Task<List<Shared.Models.DirectoryObject>> GetOwnedObjects(Shared.Models.Directory directory, string id)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var request = await client.Users[id]
                .OwnedObjects
                .Request()
                .GetAsync();

            var data = new List<Microsoft.Graph.DirectoryObject>();
            while (request != null)
            {
                data.AddRange(request);

                if (request.NextPageRequest == null) break;
                request = await request.NextPageRequest.GetAsync();
            };

            var result = data
                .Select(r => _directoryObjectMapper.Map(directory, r))
                .ToList();
            
            return result;
        }

        public async Task<List<Shared.Models.DirectoryObject>> GetMemberOf(Shared.Models.Directory directory, string id)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var request = await client.Users[id]
                .MemberOf
                .Request()
                .GetAsync();

            var data = new List<Microsoft.Graph.DirectoryObject>();
            while (request != null)
            {
                data.AddRange(request);

                if (request.NextPageRequest == null) break;
                request = await request.NextPageRequest.GetAsync();
            };

            var result = data
                .Select(r => _directoryObjectMapper.Map(directory, r))
                .ToList();

            return result;
        }

        public async Task<List<Shared.Models.ApplicationRole>> GetApplicationRoles(Shared.Models.Directory directory, string id)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var request = await client.Users[id]
                .AppRoleAssignments
                .Request()
                .GetAsync();

            var data = new List<Microsoft.Graph.AppRoleAssignment>();
            while (request != null)
            {
                data.AddRange(request);

                if (request.NextPageRequest == null) break;
                request = await request.NextPageRequest.GetAsync();
            };

            var result = data
                .Select(r => _applicationRoleMapper.Map(directory, r))
                .ToList();

            return result;
        }

        public async Task DisableUser(Directory directory, string id)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var user = new Microsoft.Graph.User
            {
                Id = id,
                AccountEnabled = false
            };

            await client.Users[id]
                .Request()
                .UpdateAsync(user);

        }

        public async Task DeleteUser(Directory directory, string id)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            await client.Users[id]
                .Request()
                .DeleteAsync();
        }
    }
}
