using System.Net;
using System.Threading.Tasks;
using IdentityGuard.Api.Functions;
using IdentityGuard.Api.Tests.TestUtility;
using IdentityGuard.Api.Tests.TestUtility.Extensions;
using IdentityGuard.Shared.Models;
using Xunit;

namespace IdentityGuard.Api.Tests.Functions
{
    public class AuthorizationFunctionTests
    {
        [Theory]
        [InlineData(true,AuthorizedActions.ViewApplicationInfo)]
        [InlineData(false,"other")]
        [InlineData(false, "")]
        [InlineData(false, null)]
        public async Task IsAuthorized_ValidInput_ReturnsAuthorization(bool isAuthorized, string action, params string[] roles)
        {
            var builder = new TestBuilder();

            var function = builder.Get<AuthorizationFunction>();

            var result = await function.Get(builder.GetRequest(), builder.Context(), action);

            Assert.Equal(HttpStatusCode.OK,result.StatusCode);
            Assert.Equal(isAuthorized,result.Value<bool>());
            
        }
    }
}