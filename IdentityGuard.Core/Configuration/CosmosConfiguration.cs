namespace IdentityGuard.Core.Configuration
{
    public static class CosmosConfiguration
    {
        public const string DatabaseId = "IdentityGuard";

        public const string DefaultPartitionKey = "Default";

        public static CosmosContainers Containers = new CosmosContainers();

    }

    public class CosmosContainers
    {
        public const string Directories = "Directory";
    }


}