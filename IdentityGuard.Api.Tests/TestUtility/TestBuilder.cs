using System.Collections.Generic;
using IdentityGuard.Api.Configuration;
using IdentityGuard.Api.Functions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityGuard.Tests.Shared;

namespace IdentityGuard.Api.Tests.TestUtility
{
    public class TestBuilder:TestBuilderBase
    {
        protected override void ConfigureApplication(IServiceCollection services)
        {
            var properties = new Dictionary<object, object>();
            var context = new HostBuilderContext(properties);
            DependencyInjectionConfiguration.Configure(context, services);
        }

        protected override void ConfigureFunctions(IServiceCollection services)
        {
            services.AddTransient<AccessReviewFunctions>();
            services.AddTransient<ApplicationFunctions>();
            services.AddTransient<AuthorizationFunction>();
            services.AddTransient<GroupFunctions>();
            services.AddTransient<HealthFunction>();
            services.AddTransient<DirectoryFunctions>();
            services.AddTransient<UserFunctions>();
        }
    }
}