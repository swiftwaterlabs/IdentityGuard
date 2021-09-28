using IdentityGuard.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Services
{
    public interface IGroupService
    {
        Task<List<Group>> Search(ICollection<string> names);
        Task<GroupAccess> ApplicationAccess(string directoryId, string id);
        Task<Group> Get(string directoryId, string id);
    }

    public class GroupService : AbstractHttpService, IGroupService
    {
        public GroupService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public Task<List<Group>> Search(ICollection<string> names) => Post<List<Group>, ICollection<string>>("api/group/search", names);

        public Task<GroupAccess> ApplicationAccess(string directoryId, string id) => Get<GroupAccess>($"api/group/{directoryId}/{id}/access");
        public Task<Group> Get(string directoryId, string id) => Get<Group>($"api/group/{directoryId}/{id}");
    }
}
