using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityGuard.Tests.Shared.Fakes.Http
{
    public class FakeAuthenticationProvider : Microsoft.Graph.IAuthenticationProvider
    {
        public Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            return Task.CompletedTask;
        }
    }
}
