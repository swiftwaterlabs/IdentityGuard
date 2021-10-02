using IdentityGuard.Tests.Shared.Fakes;
using Microsoft.Azure.Functions.Worker;

namespace IdentityGuard.Tests.Shared.Extensions
{
    public static class TestBuilderExtensions
    {
        public static FunctionContext Context(this TestBuilderBase testBuilder)
        {
            return new FunctionContextFake(testBuilder.Context);
        } 
    }
}