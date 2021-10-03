using IdentityGuard.Core.Extensions;
using IdentityGuard.Core.Repositories;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using IdentityGuard.Shared.Models.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers
{
    public class LifecyclePolicyExecutionManager
    {
        private readonly ILifecyclePolicyRepository _userPolicyRepository;
        private readonly DirectoryManager _directoryManager;
        private readonly UserService _userService;
        private readonly RequestManager _requestManager;
        private readonly ILogger<LifecyclePolicyManager> _logger;
        private readonly ILifecyclePolicyExecutionRepository _lifecyclePolicyExecutionRepository;

        public LifecyclePolicyExecutionManager(ILifecyclePolicyRepository userPolicyRepository,
            DirectoryManager directoryManager,
            UserService userService,
            RequestManager requestManager,
            ILogger<LifecyclePolicyManager> logger,
            ILifecyclePolicyExecutionRepository lifecyclePolicyExecutionRepository)
        {
            _userPolicyRepository = userPolicyRepository;
            _directoryManager = directoryManager;
            _userService = userService;
            _requestManager = requestManager;
            _logger = logger;
            _lifecyclePolicyExecutionRepository = lifecyclePolicyExecutionRepository;
        }
        public async Task ApplyAll(DateTime nextExecution)
        {
            var policies = await _userPolicyRepository
                .Get();

            var applyTasks = policies
                .Where(p => p.Enabled)
                .Select(p => ApplyPolicy(p, nextExecution));

            await Task.WhenAll(applyTasks);
        }

        public async Task ApplyPolicy(LifecyclePolicy toApply, DateTime nextExecution)
        {
            var execution = new LifecyclePolicyExecution
            {
                PolicyId = toApply.Id,
                Start = ClockService.Now,
                Status = LifecyclePolicyStatus.InProgress,
                Next = nextExecution
            };
            try
            {
                var directory = await _directoryManager.GetById(toApply.DirectoryId);

                var resolvedQuery = toApply.Query.ResolveQueryParameters();
                var users = await _userService.Query(directory, resolvedQuery);

                execution.AffectedObjects = users.Count;

                switch (toApply.Action)
                {
                    case LifecyclePolicyAction.Delete:
                        {
                            await DeleteUsers(directory, users);
                            break;
                        }
                    case LifecyclePolicyAction.Disable:
                        {
                            await DisableUsers(directory, users);
                            break;
                        }
                    default:
                        {
                            throw new ArgumentOutOfRangeException(nameof(toApply.Action), toApply.Action, "Unsupported action");
                        }
                }

                execution.Status = LifecyclePolicyStatus.Complete;
            }
            catch (Exception exception)
            {
                _logger.LogError("Error when applying policy", exception);
                execution.Status = LifecyclePolicyStatus.Failed;
            }
            finally
            {
                execution.End = ClockService.Now;

                await _lifecyclePolicyExecutionRepository.Save(execution);
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
               .Select(u => DeleteUser(directory, u));

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

        public async Task<ICollection<User>> AuditPolicy(string id)
        {
            var toAudit = await _userPolicyRepository.GetById(id);
            var directory = await _directoryManager.GetById(toAudit.DirectoryId);

            var resolvedQuery = toAudit.Query.ResolveQueryParameters();
            var users = await _userService.Query(directory, resolvedQuery);

            return users;
        }
    }
}
