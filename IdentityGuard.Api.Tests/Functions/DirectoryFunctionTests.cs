using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using IdentityGuard.Api.Functions;
using IdentityGuard.Api.Tests.TestUtility;
using IdentityGuard.Api.Tests.TestUtility.Extensions;
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

            var expected = builder.WithDirectory(name: "test-domain");

            var function = builder.Get<DirectoryFunctions>();

            var result = await function.Get(builder.GetRequest(), builder.Context());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var value = result.Value<List<Directory>>();

            Assert.Single(value);

        }
    }
}