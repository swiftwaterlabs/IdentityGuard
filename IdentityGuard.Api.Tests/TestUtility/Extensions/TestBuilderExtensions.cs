using System.Net.Http;
using IdentityGuard.Api.Tests.TestUtility.Fakes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Tests.TestUtility.Extensions
{
    public static class TestBuilderExtensions
    {
        public static FunctionContext Context(this TestBuilder testBuilder)
        {
            return new FunctionContextFake(testBuilder.Context);
        }

        public static HttpRequestData GetRequest(this TestBuilder testBuilder)
        {
            return new HttpRequestDataFake(testBuilder.Context(),HttpMethod.Get,testBuilder.Context);
        }

        public static HttpRequestData PostRequest(this TestBuilder testBuilder)
        {
            return new HttpRequestDataFake(testBuilder.Context(), HttpMethod.Post, testBuilder.Context);
        }

        public static HttpRequestData PutRequest(this TestBuilder testBuilder)
        {
            return new HttpRequestDataFake(testBuilder.Context(), HttpMethod.Put, testBuilder.Context);
        }

        public static HttpRequestData DeleteRequest(this TestBuilder testBuilder)
        {
            return new HttpRequestDataFake(testBuilder.Context(), HttpMethod.Delete, testBuilder.Context);
        }
    }
}