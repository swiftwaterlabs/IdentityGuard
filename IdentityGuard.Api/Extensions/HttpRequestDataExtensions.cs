using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace IdentityGuard.Api.Extensions
{
    public static class HttpRequestDataExtensions
    {
        public static IEnumerable<ClaimsIdentity> GetRequestingUser(this HttpRequestData request)
        {
            if (request.Identities.GetEnumerator().Current != null) 
                return request.Identities.ToList();

            if (!request.Headers.Contains(HttpRequestHeader.Authorization.ToString())) 
                return new List<ClaimsIdentity>();

            var identities = request.Headers.GetValues(HttpRequestHeader.Authorization.ToString())
                   .Select(GetIdentityFromHeader)
                   .Where(u => u != null)
                   .ToList();

            return identities;
        }

        private static ClaimsIdentity GetIdentityFromHeader(string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader)) return null;

            var stream = authorizationHeader
              .Split(" ")
              .Last();

            if (string.IsNullOrEmpty(stream)) return null;

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var securityToken = jsonToken as JwtSecurityToken;
            
            if (securityToken == null) return null;

            return new ClaimsIdentity(securityToken.Claims);
        }

        public static HttpResponseData UnauthorizedResponse(this HttpRequestData request)
        {
            return request.CreateResponse(HttpStatusCode.Forbidden);
        }
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

        public static HttpResponseData NotFoundResponse(this HttpRequestData request)
        {
            return request.CreateResponse(HttpStatusCode.NotFound);
        }

        public static Task<HttpResponseData> NotFoundResponseAsync(this HttpRequestData request)
        {
            return Task.FromResult(request.CreateResponse(HttpStatusCode.NotFound));
        }

        public static T GetBody<T>(this HttpRequestData request)
        {
            if (request.Body == null) return default(T);

            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body);
            var bodyAsString = reader.ReadToEnd();

            var result = JsonConvert.DeserializeObject<T>(bodyAsString);

            return result;
        }
    }
}