using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Services
{
    public abstract class AbstractHttpService
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        protected const string _DefaultKey = "ApiAuthenticated";

        public AbstractHttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected HttpClient GetClient(string client)
        {
            return _httpClientFactory.CreateClient(client);
        }

        protected Task<T> Get<T>(string uri)
        {
            var serviceAccountClient = GetClient(_DefaultKey);
            return serviceAccountClient.GetFromJsonAsync<T>(uri);
        }

        protected Task<HttpResponseMessage> Post<T>(string uri, T obj)
        {
            var serviceAccountClient = GetClient(_DefaultKey);
            return serviceAccountClient.PostAsJsonAsync<T>(uri, obj);
        }

        protected async Task<TResult> Post<TResult, TRequest>(string uri, TRequest obj)
        {
            var serviceAccountClient = GetClient(_DefaultKey);
            var result = await serviceAccountClient.PostAsJsonAsync<TRequest>(uri, obj);

            var resultData = await result.Content.ReadAsAsync<TResult>();

            return resultData;
        }

        protected Task<HttpResponseMessage> Put<T>(string uri, T obj)
        {
            var serviceAccountClient = GetClient(_DefaultKey);
            return serviceAccountClient.PutAsJsonAsync<T>(uri, obj);
        }

        protected async Task<TResult> Put<TResult, TRequest>(string uri, TRequest obj)
        {
            var serviceAccountClient = GetClient(_DefaultKey);
            var result = await serviceAccountClient.PutAsJsonAsync<TRequest>(uri, obj);

            var resultData = await result.Content.ReadAsAsync<TResult>();

            return resultData;
        }

        protected Task Delete(string uri)
        {
            var serviceAccountClient = GetClient(_DefaultKey);
            return serviceAccountClient.DeleteAsync(uri);
        }
    }
}