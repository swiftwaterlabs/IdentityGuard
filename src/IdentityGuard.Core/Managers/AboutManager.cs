using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityGuard.Core.Managers
{
    public class AboutManager
    {
        private readonly IConfiguration _configuration;
        private readonly EnvironmentService _environmentService;

        public AboutManager(IConfiguration configuration, EnvironmentService environmentService)
        {
            _configuration = configuration;
            _environmentService = environmentService;
        }
        public AboutInfo Get()
        {
            return new AboutInfo
            {
                ApplicationName = _environmentService.GetApplicationName(),
                ApplicationVersion = _environmentService.GetApplicationVersion(),
                ReleaseDate = _configuration["Release:ReleaseDate"]
            };
        }
    }
}