using System.Net;
using System.Threading.Tasks;
using IdentityGuard.Api.Functions;
using IdentityGuard.Api.Tests.TestUtility;
using IdentityGuard.Api.Tests.TestUtility.Extensions;
using IdentityGuard.Shared.Models;
using IdentityGuard.Tests.Shared.Extensions;
using Xunit;

namespace IdentityGuard.Api.Tests.Functions
{
    public class HealthFunctionTests
    {
        [Fact]
        public async Task Probe_NoInputs_ReturnsOk()
        {
            var builder = new TestBuilder();

            var function = builder.Get<HealthFunction>();

            var result = await function.Probe(builder.GetRequest(), builder.Context());

            Assert.Equal(HttpStatusCode.OK,result.StatusCode);
        }

        [Fact]
        public async Task About_NoInputs_ReturnsAboutInfo()
        {
            var builder = new TestBuilder();

            var function = builder.Get<HealthFunction>();

            var result = await function.About(builder.GetRequest(), builder.Context());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            
            var value = result.Value<AboutInfo>();
            Assert.NotNull(value.ApplicationName);
            Assert.NotNull(value.ApplicationVersion);
        }

        [Fact]
        public async Task Status_NoInputs_ReturnsAboutInfo()
        {
            var builder = new TestBuilder();

            var function = builder.Get<HealthFunction>();

            var result = await function.Status(builder.GetRequest(), builder.Context());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var value = result.Value<ApplicationHealth>();
            Assert.True(value.IsHealthy);
            Assert.NotEmpty(value.DependencyHealth);
        }
    }
}