using System.Collections.Concurrent;
using System.Security.Claims;
using IdentityGuard.Core.Models.Data;

namespace IdentityGuard.Tests.Shared.TestContexts
{
    public class DataTestContext
    {
        public ConcurrentDictionary<string, DirectoryData> Directories = new ConcurrentDictionary<string, DirectoryData>();
        public ConcurrentDictionary<string, AccessReviewData> AccessReviews = new ConcurrentDictionary<string, AccessReviewData>();
        public ConcurrentDictionary<string, RequestData> Requests = new ConcurrentDictionary<string, RequestData>();
    }

    public class AuthenticatedUserContext
    {
        public ClaimsIdentity AuthenticatedUser = new ClaimsIdentity();
    }
}