using Microsoft.Extensions.Configuration;

namespace IdentityGuard.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool IsDevelopment(this IConfiguration configuration)
        {
            return configuration["ASPNETCORE_ENVIRONMENT"] == "Development";
        }
    }
}