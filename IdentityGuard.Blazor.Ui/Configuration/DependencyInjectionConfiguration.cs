using System;
using System.Net.Http;
using IdentityGuard.Blazor.Ui.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityGuard.Blazor.Ui.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void Register(IServiceCollection services, IConfiguration configuration, string baseAddress)
        {
            services.AddAuthorizationCore();
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });
            RegisterApiHttpClient(services, configuration);

            //Register Managers

            //Register Services
            services.AddTransient<Services.IAuthorizationService, AuthorizationService>();
            services.AddTransient<Services.IDirectoryService, DirectoryService>();
            services.AddTransient<Services.IUserService, UserService>();

            services.AddGraphClient(configuration[ConfigurationNames.ApiScope]);

        }

        private static void RegisterApiHttpClient(IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration[ConfigurationNames.ApiUrl];
            var scope = configuration[ConfigurationNames.ApiScope];

            services.AddHttpClient("Api", client =>
                client.BaseAddress = new Uri(url));

            services.AddHttpClient("ApiAuthenticated", client =>
                    client.BaseAddress = new Uri(url))
                .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
                    .ConfigureHandler(new[] { url }, new[] { scope }));
        }
    }
}