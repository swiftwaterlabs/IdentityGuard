using IdentityGuard.Core.Managers;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityGuard.Api.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void Configure(HostBuilderContext context, IServiceCollection services)
        {
            
            services.AddTransient<AboutManager>();
            services.AddTransient<ApplicationHealthManager>();

            services.AddTransient<EnvironmentService>();
        }
    }
}