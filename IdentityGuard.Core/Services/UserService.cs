using IdentityGuard.Core.Factories;
using IdentityGuard.Core.Mappers;
using IdentityGuard.Core.Models;
using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<User>> SearchUser(Shared.Models.Directory directory, string name, UserSearchType searchType)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var filter = GetSearchFilter(name, searchType);
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

        private static string GetSearchFilter(string name, UserSearchType searchType)
        {
            var encodedName = System.Web.HttpUtility.UrlEncode(name);
            return searchType switch
            {
                UserSearchType.UserPrincipalName => $"userPrincipalName eq '{encodedName}'",
                UserSearchType.Email => $"mail eq '{encodedName}'",
                _ => throw new ArgumentOutOfRangeException(nameof(searchType), "Only UserPrincipalName and Email are supported")
            };
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
    }
}
