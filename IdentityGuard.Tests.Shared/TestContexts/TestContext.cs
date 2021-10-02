namespace IdentityGuard.Tests.Shared.TestContexts
{
    public class TestContext
    {
        public DataTestContext Data = new DataTestContext();
        public GraphApiContext GraphApi = new GraphApiContext();
        public AuthenticatedUserContext Identity = new AuthenticatedUserContext();
    }
}