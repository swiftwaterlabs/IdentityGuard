using System.Threading.Tasks;
using IdentityGuard.Core.Managers;
using Microsoft.Azure.Functions.Worker;

namespace IdentityGuard.Worker.Functions
{
    public class UserPolicyFunctions
    {
        private readonly UserPolicyManager _userPolicyManager;

        public UserPolicyFunctions(UserPolicyManager userPolicyManager)
        {
            _userPolicyManager = userPolicyManager;
        }
        [Function("userpolicy-apply")]
        public Task Apply([TimerTrigger("0 */5 * * * *")] TimerInfo timer, FunctionContext context)
        {
            return _userPolicyManager.ApplyAll();
        }
    }
}
