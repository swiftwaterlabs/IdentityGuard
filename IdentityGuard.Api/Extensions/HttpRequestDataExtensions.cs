using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;

namespace IdentityGuard.Api.Extensions
{
    public static class HttpRequestDataExtensions
    {
        public static HttpResponseData UnauthorizedResponse(this HttpRequestData request)
        {
            return request.CreateResponse(HttpStatusCode.Forbidden);
        }

        public static HttpResponseData OkResponse(this HttpRequestData request)
        {
            return request.CreateResponse(HttpStatusCode.OK);
        }

        public static async Task<HttpResponseData> OkResponseAsync<T>(this HttpRequestData request, T data)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(data);

            return response;
        }
    }
}