using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityGuard.Core.Extensions
{
    public static class ClaimsIdentityExtensions
    {
        public static string GetUserId(this IEnumerable<ClaimsIdentity> identities)
        {
            var userIds = identities
                .SelectMany(i=>i.Claims)
                .Where(c=>c.Type == "oid")
                .Select(c=>c.Value);

            var userId = userIds.FirstOrDefault();

            return userId;
        }

        public static string GetUserDirectoryId(this IEnumerable<ClaimsIdentity> identities)
        {
            var userIds = identities
                .SelectMany(i => i.Claims)
                .Where(c => c.Type == "tid")
                .Select(c => c.Value);

            var userId = userIds.FirstOrDefault();

            return userId;
        }
    }
}