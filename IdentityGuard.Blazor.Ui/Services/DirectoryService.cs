using IdentityGuard.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Services
{
    public interface IDirectoryService
    {
        Task<List<Directory>> Get();
        Task<Directory> Get(string id);
        Task<Directory> Post(Directory toSave);
        Task<Directory> Put(Directory toSave);
        Task Delete(string id);
    }

    public class DirectoryService : AbstractHttpService, IDirectoryService
    {
        public DirectoryService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public Task<List<Directory>> Get() => Get<List<Directory>>("api/directory");

        public Task<Directory> Get(string id) => Get<Directory>($"api/directory/{id}");

        public Task<Directory> Post(Directory toSave) => Post<Directory,Directory>($"api/directory",toSave);

        public Task<Directory> Put(Directory toSave) => Put<Directory, Directory>($"api/directory/{toSave.Id}", toSave);

        public Task Delete(string id) => base.Delete($"api/directory/{id}");
    }
}