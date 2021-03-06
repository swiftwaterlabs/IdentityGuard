using System;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using IdentityGuard.Core.Configuration;
using IdentityGuard.Core.Factories;
using IdentityGuard.Core.Managers;
using IdentityGuard.Core.Managers.ActionProcessors;
using IdentityGuard.Core.Mappers;
using IdentityGuard.Core.Repositories;
using IdentityGuard.Core.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityGuard.Core.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void ConfigureCore(this IServiceCollection services)
        {
            // Factories
            services.AddScoped<IGraphClientFactory, CachedGraphClientFactory>();
            services.AddTransient<GraphClientFactory>();

            // Managers
            services.AddTransient<AboutManager>();
            services.AddTransient<ApplicationManager>();
            services.AddTransient<ApplicationHealthManager>();
            services.AddTransient<AuthorizationManager>();
            services.AddTransient<DirectoryManager>();
            services.AddTransient<UserManager>();
            services.AddTransient<AccessReviewManager>();
            services.AddTransient<AccessReviewActionManager>();
            services.AddTransient<RequestManager>();
            services.AddTransient<GroupManager>();
            services.AddTransient<LifecyclePolicyManager>();
            services.AddTransient<LifecyclePolicyExecutionManager>();

            // Processors
            services.AddTransient<IActionProcessor, OwnerActionProcessor>();
            services.AddTransient<IActionProcessor, OwnedActionProcessor>();
            services.AddTransient<IActionProcessor, GroupMembershipActionProcessor>();
            services.AddTransient<IActionProcessor, GroupMemberActionProcessor>();
            services.AddTransient<IActionProcessor, ApplicationSecretActionProcessor>();
            services.AddTransient<IActionProcessor, ApplicationPermissionActionProcessor>();
            services.AddTransient<IActionProcessor, ApplicationRoleActionProcessor>();

            // Mappers
            services.AddTransient<ApplicationMapper>();
            services.AddTransient<ApplicationRoleMapper>();
            services.AddTransient<DirectoryMapper>();
            services.AddTransient<DirectoryObjectMapper>();
            services.AddTransient<ServicePrincipalMapper>();
            services.AddTransient<UserMapper>();
            services.AddTransient<AccessReviewMapper>();
            services.AddTransient<RequestMapper>();
            services.AddTransient<GroupMapper>();
            services.AddTransient<LifecyclePolicyMapper>();
            services.AddTransient<LifecyclePolicyExecutionMapper>();

            // Repositories
            services.AddTransient<DirectoryRepository>();
            services.AddScoped<IDirectoryRepository, CachedDirectoryRepository>();
            services.AddTransient<IAccessReviewRepository, AccessReviewRepository>();
            services.AddTransient<IRequestRepository, RequestRepository>();
            services.AddTransient<ILifecyclePolicyRepository, LifecyclePolicyRepository>();
            services.AddTransient<ILifecyclePolicyExecutionRepository, LifecyclePolicyExecutionRepository>();

            // Services
            services.AddTransient<ApplicationService>();
            services.AddTransient<EnvironmentService>();
            services.AddTransient<ServicePrincipalService>();
            services.AddTransient<UserService>();
            services.AddTransient<GroupService>();
            services.AddTransient<DirectoryObjectService>();

            ConfigureKeyVault(services);
            ConfigureCosmosDb(services);
        }

        private static void ConfigureKeyVault(IServiceCollection services)
        {
            services.AddScoped(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                var endpoint = configuration[ConfigurationNames.KeyVault.BaseUri];
                var endpointUrl = new Uri(endpoint);
                var managedIdentityClientId = configuration[ConfigurationNames.KeyVault.ManagedIdentityClient];

                var credentials = GetCredential(managedIdentityClientId);

                return new SecretClient(vaultUri: endpointUrl, credential: credentials);
            });

        }

        private static TokenCredential GetCredential(string clientId)
        {
            if (string.IsNullOrWhiteSpace(clientId)) return new DefaultAzureCredential();

            return new ManagedIdentityCredential(clientId);
        }

        private static void ConfigureCosmosDb(IServiceCollection services)
        {
            services.AddScoped(provider =>
            {
                var secretClient = provider.GetService<SecretClient>();
                var configuration = provider.GetService<IConfiguration>();
                var endpoint = configuration[ConfigurationNames.Cosmos.BaseUri];

                var secret = secretClient.GetSecret(SecretNames.CosmosPrimaryKey);
                var key = secret.Value.Value;

                var client = GetCosmosClient(endpoint, key);

                return client;
            });

            services.AddTransient(provider =>
            {
                var client = provider.GetService<CosmosClient>();
                var queryFactory = provider.GetService<ICosmosLinqQueryFactory>();
                return new CosmosDbService(client, CosmosConfiguration.DatabaseId, queryFactory);
            });

            services.AddTransient<ICosmosLinqQueryFactory, CosmosLinqQueryFactory>();
        }

        private static CosmosClient GetCosmosClient(string endpoint, string key)
        {
            var options = new CosmosClientOptions
            {
                AllowBulkExecution = true,
                MaxRetryAttemptsOnRateLimitedRequests = 19,
                MaxRetryWaitTimeOnRateLimitedRequests = TimeSpan.FromMinutes(1),
                SerializerOptions = new CosmosSerializationOptions
                {
                    IgnoreNullValues = true
                }
            };

            var client = new CosmosClient(endpoint, key, options);
            return client;
        }
    }
}
