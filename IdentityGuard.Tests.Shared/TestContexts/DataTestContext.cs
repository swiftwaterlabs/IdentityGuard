using System.Collections.Concurrent;
using System.Security.Claims;
using IdentityGuard.Core.Models.Data;

namespace IdentityGuard.Tests.Shared.TestContexts
{
    public class DataTestContext
    {
        public ConcurrentDictionary<string, DirectoryData> Directories = new ConcurrentDictionary<string, DirectoryData>();
    }

    public class AuthenticatedUserContext
    {
        public ClaimsIdentity AuthenticatedUser = new ClaimsIdentity();
    }
}