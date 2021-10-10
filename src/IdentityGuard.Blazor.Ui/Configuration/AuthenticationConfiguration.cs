using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityGuard.Blazor.Ui.Configuration
{
    public static class AuthenticationConfiguration
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorizationCore();
            services.AddMsalAuthentication(options =>
            {
                var apiScope = configuration[ConfigurationNames.ApiScope];

                configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add(apiScope);

                options.ProviderOptions.LoginMode = "redirect";
            });
        }
    }
}