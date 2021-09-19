using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityGuard.Shared.Models;

namespace IdentityGuard.Core.Managers
{
    public class AuthorizationManager
    {
        public bool IsAuthorized(string action, IEnumerable<ClaimsIdentity> identities)
        {
            return action?.ToLower() switch
            {
                AuthorizedActions.ViewApplicationInfo => true,
                _ => false
            };
        }
    }
}