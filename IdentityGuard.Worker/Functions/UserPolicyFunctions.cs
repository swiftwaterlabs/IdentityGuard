using System.Threading.Tasks;
using IdentityGuard.Core.Managers;
using Microsoft.Azure.Functions.Worker;

namespace IdentityGuard.Worker.Functions
{
    public class UserPolicyFunctions
    {
        private readonly LifecyclePolicyExecutionManager _lifecyclePolicyExecutionManager;

        public UserPolicyFunctions(LifecyclePolicyExecutionManager lifecyclePolicyExecutionManager)
        {
            _lifecyclePolicyExecutionManager = lifecyclePolicyExecutionManager;
        }
        [Function("userpolicy-apply")]
        public Task Apply([TimerTrigger("0 */5 * * * *")] TimerInfo timer, FunctionContext context)
        {
            return _lifecyclePolicyExecutionManager.ApplyAll(timer.ScheduleStatus.Next);
        }
    }
}
