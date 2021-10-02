using System.Collections.Generic;
using Azure.Security.KeyVault.Secrets;
using IdentityGuard.Tests.Shared.Fakes;
using IdentityGuard.Tests.Shared.TestContexts;
using IdentityGuard.Core.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityGuard.Tests.Shared;
using IdentityGuard.Tests.Shared.Extensions;
using IdentityGuard.Core.Configuration;
using IdentityGuard.Core.Factories;

namespace IdentityGuard.Tests.Shared
{
    public class TestBuilderBase
    {
        private static ServiceProvider _serviceProvider;

        public TestBuilderBase() : this(new ServiceCollection())
        {

        }
        private TestBuilderBase(IServiceCollection services)
        {
            Configure(services);
        }

        private void Configure(IServiceCollection services)
        {
            ConfigureApplicationConfiguration(services);
            ConfigureApplication(services);
            ConfigureFunctionsWorker(services);
            ConfigureFunctions(services);
            ConfigureFakes(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private static void ConfigureApplicationConfiguration(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
        }

        private static void ConfigureFunctionsWorker(IServiceCollection services)
        {
            services.AddFunctionsWorkerCore();
            services.AddFunctionsWorkerDefaults();
        }

        protected virtual void ConfigureApplication(IServiceCollection services)
        {

        }

        protected virtual void ConfigureFunctions(IServiceCollection services)
        {
            
        }

        private static void ConfigureFakes(IServiceCollection services)
        {
            services.AddSingleton<TestContext>();
            services.ReplaceTransient<CosmosClient, CosmosClientFake>();
            services.ReplaceTransient<SecretClient, SecretClientFake>();
            services.ReplaceTransient<ICosmosLinqQueryFactory, CosmosLinqQueryFactoryFake>();
            services.ReplaceTransient<IGraphClientFactory, GraphClientFactoryFake>();
        }

        public T Get<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public TestContext Context => _serviceProvider.GetService<TestContext>();
    }
}
