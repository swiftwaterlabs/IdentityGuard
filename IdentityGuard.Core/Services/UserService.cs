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

        public UserService(IGraphClientFactory graphClientFactory, UserMapper userMapper)
        {
            _graphClientFactory = graphClientFactory;
            _userMapper = userMapper;
        }

        public async Task<List<User>> SearchUser(Shared.Models.Directory directory, string type, string name, UserSearchType searchType)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var filter = GetSearchFilter(type, name, searchType);
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

        private static string GetSearchFilter(string userType, string name, UserSearchType searchType)
        {
            var encodedName = System.Web.HttpUtility.UrlEncode(name);
            return searchType switch
            {
                UserSearchType.UserPrincipalName => $"userType eq '{userType}' and userPrincipalName eq '{encodedName}'",
                UserSearchType.Email => $"userType eq '{userType}' and mail eq '{encodedName}'",
                _ => throw new ArgumentOutOfRangeException(nameof(searchType), "Only UserPrincipalName and Email are supported")
            };
        }
    }
}
