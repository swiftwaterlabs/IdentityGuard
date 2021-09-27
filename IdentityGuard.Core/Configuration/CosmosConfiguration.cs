namespace IdentityGuard.Core.Configuration
{
    public static class CosmosConfiguration
    {
        public const string DatabaseId = "identityguard";

        public const string DefaultPartitionKey = "Default";

        public static CosmosContainers Containers = new CosmosContainers();

    }

    public class CosmosContainers
    {
        public  string Directories = "Directories";
        public  string AccessReviews = "AccessReviews";
        public  string Requests = "Requests";
    }


}