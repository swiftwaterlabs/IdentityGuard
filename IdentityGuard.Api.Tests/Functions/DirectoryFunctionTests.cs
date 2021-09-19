using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityGuard.Api.Functions;
using IdentityGuard.Api.Tests.TestUtility;
using IdentityGuard.Api.Tests.TestUtility.Extensions;
using IdentityGuard.Core.Models.Data;
using IdentityGuard.Shared.Models;
using Xunit;

namespace IdentityGuard.Api.Tests.Functions
{
    public class DirectoryFunctionTests
    {
        [Fact]
        public async Task Get_UnauthorizedUser_ReturnsForbidden()
        {
            var builder = new TestBuilder();

            var function = builder.Get<DirectoryFunctions>();

            var result = await function.Get(builder.GetRequest(), builder.Context());

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);

        }

        [Fact]
        public async Task Get_AuthorizedUser_ReturnsDirectories()
        {
            var builder = new TestBuilder();
            builder.WithAuthenticatedUser("the-user",ApplicationRoles.DirectoryAdmin);

            var expected1 = builder.WithDirectory(name: "test-domain1");
            var expected2 = builder.WithDirectory(name: "test-domain2");

            var function = builder.Get<DirectoryFunctions>();

            var result = await function.Get(builder.GetRequest(), builder.Context());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var value = result.Value<List<Directory>>();

            Assert.Equal(2,value.Count);
            AssertDirectoryData(value.First(v=>v.Id == expected1.Id),expected1);
            AssertDirectoryData(value.First(v=>v.Id == expected2.Id),expected2);

        }

        [Fact]
        public async Task Get_ValidIdAndUnauthorizedUser_ReturnsForbidden()
        {
            var builder = new TestBuilder();

            var directory1 = builder.WithDirectory(name: "domain-1");

            var function = builder.Get<DirectoryFunctions>();

            var result = await function.GetById(builder.GetRequest(), builder.Context(),directory1.Id);

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);

        }

        [Fact]
        public async Task Get_ValidIdAndAuthorizedUser_ReturnsMatching()
        {
            var builder = new TestBuilder();
            builder.WithAuthenticatedUser("the-user",ApplicationRoles.DirectoryAdmin);
            
            var directory1 = builder.WithDirectory(name: "domain-1");

            var function = builder.Get<DirectoryFunctions>();

            var result = await function.GetById(builder.GetRequest(), builder.Context(), directory1.Id);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var value = result.Value<Directory>();

            AssertDirectoryData(value,directory1);

        }

        [Fact]
        public async Task Get_InvalidIdAndAuthorizedUser_ReturnsNotFound()
        {
            var builder = new TestBuilder();
            builder.WithAuthenticatedUser("the-user", ApplicationRoles.DirectoryAdmin);

            var directory1 = builder.WithDirectory(name: "domain-1");

            var function = builder.Get<DirectoryFunctions>();

            var result = await function.GetById(builder.GetRequest(), builder.Context(), "other-id");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Delete_ValidIdAndUnauthorizedUser_ReturnsForbidden()
        {
            var builder = new TestBuilder();

            var directory1 = builder.WithDirectory(name: "domain-1");

            var function = builder.Get<DirectoryFunctions>();

            var result = await function.Delete(builder.GetRequest(), builder.Context(), directory1.Id);

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Fact]
        public async Task Delete_ValidIdAndAuthorizedUser_ReturnsOk()
        {
            var builder = new TestBuilder();
            builder.WithAuthenticatedUser("the-user",ApplicationRoles.DirectoryAdmin);

            var directory1 = builder.WithDirectory(name: "domain-1");

            var function = builder.Get<DirectoryFunctions>();

            var result = await function.Delete(builder.GetRequest(), builder.Context(), directory1.Id);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(builder.Context.Data.Directories);
        }

        private void AssertDirectoryData(Directory actual, DirectoryData expected)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.TenantId, actual.TenantId);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Domain, actual.Domain);
            Assert.Equal(expected.GraphUrl, actual.GraphUrl);
            Assert.Equal(expected.ClientType, actual.ClientType);
            Assert.Equal(expected.ClientId, actual.ClientId);
            Assert.Equal(expected.IsDefault, actual.IsDefault);
        }
    }
}