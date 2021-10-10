using Microsoft.Extensions.Hosting;
using IdentityGuard.Api.Configuration;
using Microsoft.Extensions.Configuration;

namespace IdentityGuard.Api
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(DependencyInjectionConfiguration.Configure)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddEnvironmentVariables();
                })
                
                .Build();

            host.Run();
        }
    }
}