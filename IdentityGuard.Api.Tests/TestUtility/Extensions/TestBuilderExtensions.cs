using IdentityGuard.Api.Tests.TestUtility.Fakes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Tests.TestUtility.Extensions
{
    public static class TestBuilderExtensions
    {
        public static FunctionContext Context(this TestBuilder testBuilder)
        {
            return new FunctionContextFake();
        }

        public static HttpRequestData GetRequest(this TestBuilder testBuilder)
        {
            return new HttpRequestDataFake(testBuilder.Context(),"GET");
        }
    }
}