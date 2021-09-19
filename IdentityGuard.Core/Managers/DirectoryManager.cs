using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityGuard.Core.Repositories;
using IdentityGuard.Shared.Models;

namespace IdentityGuard.Core.Managers
{
    public class DirectoryManager
    {
        private readonly DirectoryRepository _directoryRepository;

        public DirectoryManager(DirectoryRepository directoryRepository)
        {
            _directoryRepository = directoryRepository;
        }

        public Task<ICollection<Directory>> Get()
        {
            var result = _directoryRepository.Get();

            return result;
        }

        public Task<Directory> GetById(string id)
        {
            var result = _directoryRepository.GetById(id);

            return result;
        }

        public Task<Directory> GetDefault()
        {
            var result = _directoryRepository.GetDefault();

            return result;
        }

        public Task<Directory> Add(Directory toAdd)
        {
            var item = _directoryRepository.Save(toAdd);

            return item;
        }

        public Task<Directory> Update(string id, Directory toUpdate)
        {
            toUpdate.Id = id;
            var item = _directoryRepository.Save(toUpdate);

            return item;
        }

        public Task Delete(string id)
        {
            var item = _directoryRepository.Delete(id);

            return item;
        }
    }
}