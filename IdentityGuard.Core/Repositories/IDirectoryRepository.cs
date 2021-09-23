using IdentityGuard.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Repositories
{
    public interface IDirectoryRepository
    {
        Task Delete(string id);
        Task<ICollection<Directory>> Get();
        Task<Directory> GetById(string id);
        Task<Directory> GetDefault();
        Task<Directory> Save(Directory toSave);
    }
}