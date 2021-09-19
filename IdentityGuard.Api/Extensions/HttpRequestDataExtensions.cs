using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Extensions
{
    public static class HttpRequestDataExtensions
    {
        public static Task<HttpResponseData> UnauthorizedResponseAsync(this HttpRequestData request)
        {
            return Task.FromResult(request.CreateResponse(HttpStatusCode.Forbidden));
        }

        public static Task<HttpResponseData> OkResponseAsync(this HttpRequestData request)
        {
            return Task.FromResult(request.CreateResponse(HttpStatusCode.OK));
        }

        public static async Task<HttpResponseData> OkResponseAsync<T>(this HttpRequestData request, T data)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(data);

            return response;
        }
    }
}