using System.Collections.Generic;
using IdentityGuard.Api.Configuration;
using IdentityGuard.Api.Functions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityGuard.Api.Tests.TestUtility
{
    public class TestBuilder
    {
        private static ServiceProvider _serviceProvider;

        public TestBuilder() : this(new ServiceCollection())
        {

        }
        private TestBuilder(IServiceCollection services)
        {
            Configure(services);
        }

        private static void Configure(IServiceCollection services)
        {
            var properties = new Dictionary<object, object>();
            var context = new HostBuilderContext(properties);
            DependencyInjectionConfiguration.Configure(context, services);

            ConfigureFunctions(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private static void ConfigureFunctions(IServiceCollection services)
        {
            services.AddTransient<AuthorizationFunction>();
            services.AddTransient<HealthFunction>();
        }

        public T Get<T>()
        {
            return _serviceProvider.GetService<T>();
        }

    }
}