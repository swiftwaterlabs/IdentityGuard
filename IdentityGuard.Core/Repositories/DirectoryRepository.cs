using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityGuard.Core.Mappers;
using IdentityGuard.Shared.Models;

namespace IdentityGuard.Core.Repositories
{
    public class DirectoryRepository
    {
        private readonly DirectoryMapper _directoryMapper;

        public DirectoryRepository(DirectoryMapper directoryMapper)
        {
            _directoryMapper = directoryMapper;
        }

        public async Task<ICollection<Directory>> Get()
        {
            return new List<Directory>();
        }

        public async Task<Directory> Get(string id)
        {
            return new Directory();
        }

        public async Task<Directory> Save(Directory toSave)
        {
            return new Directory();
        }
    }
}