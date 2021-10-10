using IdentityGuard.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityGuard.Worker.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void Configure(HostBuilderContext context, IServiceCollection services)
        {
            services.ConfigureCore();
        }
    }
}
