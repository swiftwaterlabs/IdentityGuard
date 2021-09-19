using Microsoft.Extensions.Hosting;
using IdentityGuard.Api.Configuration;

namespace IdentityGuard.Api
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(DependencyInjectionConfiguration.Configure)
                .Build();

            host.Run();
        }
    }
}