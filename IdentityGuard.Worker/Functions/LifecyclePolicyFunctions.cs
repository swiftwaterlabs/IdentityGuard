using System.Threading.Tasks;
using IdentityGuard.Core.Managers;
using Microsoft.Azure.Functions.Worker;

namespace IdentityGuard.Worker.Functions
{
    public class LifecyclePolicyFunctions
    {
        private readonly LifecyclePolicyExecutionManager _lifecyclePolicyExecutionManager;

        public LifecyclePolicyFunctions(LifecyclePolicyExecutionManager lifecyclePolicyExecutionManager)
        {
            _lifecyclePolicyExecutionManager = lifecyclePolicyExecutionManager;
        }

        [Function("lifecyclepolicy-apply")]
        public async Task Apply([TimerTrigger("0 */1 * * * *")] TimerInfo timer, FunctionContext context)
        {
            await _lifecyclePolicyExecutionManager.ApplyAll(timer.ScheduleStatus.Next);
        }
    }
}
