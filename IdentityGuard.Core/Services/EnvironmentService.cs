namespace IdentityGuard.Core.Services
{
    public class EnvironmentService
    {
        public string GetApplicationName()
        {
            return Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default?.Application?.ApplicationName;
        }

        public string GetApplicationVersion()
        {
            return Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default?.Application?.ApplicationVersion;
        }
    }
}