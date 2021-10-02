using IdentityGuard.Tests.Shared;
using IdentityGuard.Worker.Configuration;
using IdentityGuard.Worker.Functions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace IdentityGuard.Worker.Tests.TestUtility
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
        }
    }
}
