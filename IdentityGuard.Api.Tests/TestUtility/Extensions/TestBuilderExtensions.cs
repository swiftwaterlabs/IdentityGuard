using System.IO;
using System.Net.Http;
using IdentityGuard.Api.Tests.TestUtility.Fakes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

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

        public static HttpRequestData PostRequest<T>(this TestBuilder testBuilder, T body)
        {
            var request = new HttpRequestDataFake(testBuilder.Context(), HttpMethod.Post, testBuilder.Context);
            request.SetBody(body);

            return request;
        }

        public static HttpRequestData PutRequest<T>(this TestBuilder testBuilder, T body)
        {
            var request = new HttpRequestDataFake(testBuilder.Context(), HttpMethod.Put, testBuilder.Context);
            request.SetBody(body);

            return request;
        }

        public static HttpRequestData DeleteRequest(this TestBuilder testBuilder)
        {
            return new HttpRequestDataFake(testBuilder.Context(), HttpMethod.Delete, testBuilder.Context);
        }
    }
}