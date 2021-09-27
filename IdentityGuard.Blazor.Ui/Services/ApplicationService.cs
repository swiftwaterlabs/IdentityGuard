using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityGuard.Shared.Models;

namespace IdentityGuard.Blazor.Ui.Services
{
    public interface IApplicationService
    {
        Task<List<Application>> Search(ICollection<string> names);
        Task<ApplicationAccess> ApplicationAccess(string directoryId, string id);
        Task<Application> Get(string directoryId, string id);
    }

    public class ApplicationService : AbstractHttpService, IApplicationService
    {
        public ApplicationService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }
    
        public Task<List<Application>> Search(ICollection<string> names) => Post<List<Application>, ICollection<string>>("api/application/search", names);

        public Task<ApplicationAccess> ApplicationAccess(string directoryId, string id) => Get<ApplicationAccess>($"api/application/{directoryId}/{id}/access");
        public Task<Application> Get(string directoryId, string id) => Get<Application>($"api/application/{directoryId}/{id}");
    }
}