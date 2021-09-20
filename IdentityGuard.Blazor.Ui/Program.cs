using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Threading.Tasks;
using BlazorApplicationInsights;
using IdentityGuard.Blazor.Ui.Configuration;
using MudBlazor.Services;

namespace IdentityGuard.Blazor.Ui
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddBlazorApplicationInsights(async applicationInsights =>
            {
                var instrumentationKey = builder.Configuration[ConfigurationNames.ApplicationInsightsInstrumentationKey];
                await applicationInsights.SetInstrumentationKey(instrumentationKey);
                await applicationInsights.LoadAppInsights();
            });
            builder.Services.AddMudServices();

            DependencyInjectionConfiguration.Register(builder.Services, builder.Configuration, builder.HostEnvironment.BaseAddress);
            AuthenticationConfiguration.Register(builder.Services, builder.Configuration);


            await builder.Build().RunAsync();
        }
    }
}
