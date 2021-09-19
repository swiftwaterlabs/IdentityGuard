namespace IdentityGuard.Api.Tests.TestUtility.TestContexts
{
    public class TestContext
    {
        public DataTestContext Data = new DataTestContext();
        public AuthenticatedUserContext Identity = new AuthenticatedUserContext();
    }
}