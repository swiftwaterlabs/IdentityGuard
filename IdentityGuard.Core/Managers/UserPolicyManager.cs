using IdentityGuard.Core.Repositories;
using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers
{
    public class UserPolicyManager
    {
        private readonly UserPolicyRepository _userPolicyRepository;

        public UserPolicyManager(UserPolicyRepository userPolicyRepository)
        {
            _userPolicyRepository = userPolicyRepository;
        }

        public Task<ICollection<UserPolicy>> Get()
        {
            return _userPolicyRepository.Get();
        }

        public Task<UserPolicy> Get(string id)
        {
            return _userPolicyRepository.GetById(id);
        }

        public Task<UserPolicy> Add(UserPolicy toAdd)
        {
            toAdd.Id = Guid.NewGuid().ToString();

            return _userPolicyRepository.Save(toAdd);
        }

        public Task<UserPolicy> Update(string id, UserPolicy toUpdate)
        {
            toUpdate.Id = id;

            return _userPolicyRepository.Save(toUpdate);
        }

        public Task Delete(string id)
        {
            return _userPolicyRepository.Delete(id);
        }

        public async Task ApplyPolicy(UserPolicy toApply)
        {

        }
    }
}
