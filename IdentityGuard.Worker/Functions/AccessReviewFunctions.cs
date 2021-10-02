using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityGuard.Core.Managers;
using IdentityGuard.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;

namespace IdentityGuard.Worker.Functions
{
    public class AccessReviewFunctions
    {
        private readonly AccessReviewManager _accessReviewManager;

        public AccessReviewFunctions(AccessReviewManager accessReviewManager)
        {
            _accessReviewManager = accessReviewManager;
        }

        [Function("accessreview-requst")]
        public async Task Request([ServiceBusTrigger("accessreview-request", Connection = "ServiceBus:Endpoint")] string message, FunctionContext context)
        {
            var request = JsonConvert.DeserializeObject<AccessReviewRequest>(message);

            await _accessReviewManager.Request(request, new List<ClaimsIdentity>());
        }
    }
}
