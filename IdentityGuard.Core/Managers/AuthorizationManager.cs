using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityGuard.Core.Extensions;
using IdentityGuard.Core.Models;
using IdentityGuard.Shared.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityGuard.Core.Managers
{
    public class AuthorizationManager
    {
        private readonly IConfiguration _configuration;

        private Dictionary<string, List<string>> _authorizedRoles = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase)
        {
            {AuthorizedActions.ViewApplicationInfo,new List<string>() },
            {AuthorizedActions.ManageUsers,new List<string>() },
            {AuthorizedActions.ManageGroups,new List<string>() },
            {AuthorizedActions.ManageApplications,new List<string>() },
            {AuthorizedActions.AccessReviewContributor,new List<string>() },
            {AuthorizedActions.ManageDirectories,new List<string>{ ApplicationRoles.DirectoryAdmin} }
        };

        public AuthorizationManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsAuthorized(string action, IEnumerable<ClaimsIdentity> identities)
        {
            if (_configuration.IsDevelopment()) return true;

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
                .Where(c => c.Type == identity.RoleClaimType || string.Equals(c.Type,"roles"))
                .Select(c => c.Value);

            var userIsInRole = userRoles
                .Join(roles, role1=>role1,role2=>role2,(role1,role2)=>role1)
                .Any();

            return userIsInRole;

        }
    }
}