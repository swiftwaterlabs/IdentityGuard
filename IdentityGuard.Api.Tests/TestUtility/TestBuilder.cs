using System.Collections.Generic;
using Azure.Security.KeyVault.Secrets;
using IdentityGuard.Api.Configuration;
using IdentityGuard.Api.Functions;
using IdentityGuard.Tests.Shared.Fakes;
using IdentityGuard.Tests.Shared.TestContexts;
using IdentityGuard.Core.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityGuard.Tests.Shared;
using IdentityGuard.Tests.Shared.Extensions;

namespace IdentityGuard.Api.Tests.TestUtility
{
    public class TestBuilder:ITestBuilder
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
            ConfigureApplicationConfiguration(services);
            ConfigureApplication(services);
            ConfigureFunctions(services);
            ConfigureFakes(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private static void ConfigureApplication(IServiceCollection services)
        {
            var properties = new Dictionary<object, object>();
            var context = new HostBuilderContext(properties);
            DependencyInjectionConfiguration.Configure(context, services);
        }

        private static void ConfigureApplicationConfiguration(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
        }

        private static void ConfigureFunctions(IServiceCollection services)
        {
            services.AddFunctionsWorkerCore();
            services.AddFunctionsWorkerDefaults();

            services.AddTransient<AuthorizationFunction>();
            services.AddTransient<HealthFunction>();
            services.AddTransient<DirectoryFunctions>();
        }

        private static void ConfigureFakes(IServiceCollection services)
        {
            services.AddSingleton<TestContext>();
            services.ReplaceTransient<CosmosClient, CosmosClientFake>();
            services.ReplaceTransient<SecretClient, SecretClientFake>();
            services.ReplaceTransient<ICosmosLinqQueryFactory, CosmosLinqQueryFactoryFake>();
        }

        public T Get<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public TestContext Context => _serviceProvider.GetService<TestContext>();

    }
}