using IdentityGuard.Core.Mappers;
using IdentityGuard.Core.Repositories;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using IdentityGuard.Shared.Models.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers
{
    public class UserPolicyManager
    {
        private readonly IUserPolicyRepository _userPolicyRepository;
        private readonly DirectoryManager _directoryManager;
        private readonly UserService _userService;
        private readonly RequestManager _requestManager;
        private readonly ILogger<UserPolicyManager> _logger;

        public UserPolicyManager(IUserPolicyRepository userPolicyRepository,
            DirectoryManager directoryManager,
            UserService userService,
            RequestManager requestManager,
            ILogger<UserPolicyManager> logger)
        {
            _userPolicyRepository = userPolicyRepository;
            _directoryManager = directoryManager;
            _userService = userService;
            _requestManager = requestManager;
            _logger = logger;
        }

        public Task<ICollection<UserPolicy>> Get()
        {
            return _userPolicyRepository.Get();
        }

        public Task<UserPolicy> Get(string id)
        {
            return _userPolicyRepository.GetById(id);
        }

        public async Task<UserPolicy> Add(UserPolicy toAdd)
        {
            toAdd.Id = Guid.NewGuid().ToString();
            await ApplyDirectory(toAdd);

            var result = await _userPolicyRepository.Save(toAdd);
            return result;
        }

        public async Task<UserPolicy> Update(string id, UserPolicy toUpdate)
        {
            toUpdate.Id = id;
            await ApplyDirectory(toUpdate);

            var result = await _userPolicyRepository.Save(toUpdate);
            return result;
        }

        private async Task ApplyDirectory(UserPolicy toApply)
        {
            var directory = await _directoryManager.GetById(toApply.Id);
            toApply.DirectoryName = directory?.Domain;
        };

        public Task Delete(string id)
        {
            return _userPolicyRepository.Delete(id);
        }

        public async Task ApplyAll()
        {
            var policies = await _userPolicyRepository
                .Get();

            var applyTasks = policies
                .Where(p => p.Enabled)
                .Select(ApplyPolicy);

            await Task.WhenAll(applyTasks);
        }

        public async Task ApplyPolicy(UserPolicy toApply)
        {
            var directory = await _directoryManager.GetById(toApply.DirectoryId);

            var users = await _userService.Query(directory, toApply.Query);

            switch(toApply.Action)
            {
                case UserPolicyAction.Delete:
                {
                    await DeleteUsers(directory, users);
                    return;
                }
                case UserPolicyAction.Disable:
                {
                    await DisableUsers(directory, users);
                    return;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(toApply.Action), toApply.Action, "Unsupported action");
                }
            }
        }

        private async Task DisableUsers(Directory directory, IEnumerable<User> users)
        {
            var disableTasks = users
                .Where(u => u.Enabled)
                .Select(u => DisableUser(directory, u));

            await Task.WhenAll(disableTasks);
        }

        private async Task DisableUser(Directory directory, User user)
        {
            var disableRequest = new ObjectDisableRequest
            {
                DirectoryId = user.DirectoryId,
                ObjectId = user.Id,
                ObjectType = ObjectTypes.User
            }; 
            var request = await _requestManager.Save(disableRequest, RequestStatus.InProgress);
            var status = request.Status;
            try
            {
                await _userService.DisableUser(directory, user.Id);
                status = RequestStatus.Complete;
            }
            catch (Exception exception)
            {
                _logger.LogError("Error when disabling user", exception);
                status = RequestStatus.Failed;
            }
            finally
            {
                await _requestManager.UpdateStatus(request.Id, status, null);
            }
            
        }

        private async Task DeleteUsers(Directory directory, IEnumerable<User> users)
        {
            var deleteTasks = users
               .Select(u => DisableUser(directory, u));

            await Task.WhenAll(deleteTasks);
        }

        private async Task DeleteUser(Directory directory, User user)
        {
            var deleteRequest = new ObjectDisableRequest
            {
                DirectoryId = user.DirectoryId,
                ObjectId = user.Id,
                ObjectType = ObjectTypes.User
            };
            var request = await _requestManager.Save(deleteRequest, RequestStatus.InProgress);
            var status = request.Status;
            try
            {
                await _userService.DeleteUser(directory, user.Id);
                status = RequestStatus.Complete;
            }
            catch (Exception exception)
            {
                _logger.LogError("Error when deleting user", exception);
                status = RequestStatus.Failed;
            }
            finally
            {
                await _requestManager.UpdateStatus(request.Id, status, null);
            }
        }

    }
}
