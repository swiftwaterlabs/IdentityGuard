using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityGuard.Shared.Models;

namespace IdentityGuard.Core.Managers
{
    public class AuthorizationManager
    {
        private Dictionary<string, List<string>> _authorizedRoles = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase)
        {
            {AuthorizedActions.ViewApplicationInfo,new List<string>() },
            {AuthorizedActions.ManageDirectories,new List<string>{ ApplicationRoles.DirectoryAdmin} }
        };

        public bool IsAuthorized(string action, IEnumerable<ClaimsIdentity> identities)
        {
            if (string.IsNullOrWhiteSpace(action)) return false;

            var hasAction =_authorizedRoles.TryGetValue(action.Trim(), out List<string> roles);

            if (!hasAction) return false;
            if (!roles.Any()) return true;

            var isAuthorizedForAction = identities?
                .Any(r => HasRole(r, roles)) ?? false;

            return isAuthorizedForAction;
        }

        private static bool HasRole(ClaimsIdentity identity, List<string> roles)
        {
            var userRoles = identity.Claims
                .Where(c => c.Type == identity.RoleClaimType)
                .Select(c => c.Value);

            var userIsInRole = userRoles
                .Join(roles, role1=>role1,role2=>role2,(role1,role2)=>role1)
                .Any();

            return userIsInRole;

        }
    }
}