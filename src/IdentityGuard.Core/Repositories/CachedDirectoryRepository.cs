using IdentityGuard.Shared.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Repositories
{
    public class CachedDirectoryRepository : IDirectoryRepository
    {
        private readonly DirectoryRepository _directoryRepository;
        private readonly ConcurrentDictionary<string, Directory> _cache;

        public CachedDirectoryRepository(DirectoryRepository directoryRepository)
        {
            _directoryRepository = directoryRepository;
            _cache = new ConcurrentDictionary<string, Directory>();
        }
        public Task Delete(string id)
        {
            ClearCache();

            return _directoryRepository.Delete(id);
        }

        public Task<ICollection<Directory>> Get()
        {
            return _directoryRepository.Get();
        }

        public async Task<Directory> GetById(string id)
        {
            var existing = GetCachedItem(id);
            if (existing != null) return existing;

            var data = await _directoryRepository.GetById(id);
            SetCachedItem(id, data);

            return data;
        }

        public async Task<Directory> GetDefault()
        {
            var existing = GetCachedItem("default");
            if (existing != null) return existing;

            var data = await _directoryRepository.GetDefault();
            SetCachedItem("default", data);

            return data;
        }

        public Task<Directory> Save(Directory toSave)
        {
            ClearCache();

            return _directoryRepository.Save(toSave);
        }

        private void ClearCache()
        {
            _cache.Clear();
        }

        private Directory GetCachedItem(string id)
        {
            if (_cache.TryGetValue(id, out Directory existing)) return existing;

            return null;
        }

        private void SetCachedItem(string id, Directory directory)
        {
            _cache.AddOrUpdate(id, directory, (id, existing) => { return directory;  });
        }

    }
}