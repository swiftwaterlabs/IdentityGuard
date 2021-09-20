using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityGuard.Core.Extensions;
using Microsoft.Extensions.Configuration;

namespace IdentityGuard.Core.Managers
{
    public class UserManager
    {
        private readonly IConfiguration _configuration;

        public UserManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<KeyValuePair<string, string>> GetClaims(IEnumerable<ClaimsIdentity> identities)
        {
            if(_configuration.IsDevelopment()) return new List<KeyValuePair<string, string>>();

            var claims = identities?
                .SelectMany(i => i?.Claims ?? new List<Claim>())
                .Select(c => new KeyValuePair<string, string>(c?.Type, c?.Value))
                .ToList();

            var result = claims ?? new List<KeyValuePair<string, string>>();

            return result;
        }
    }
}