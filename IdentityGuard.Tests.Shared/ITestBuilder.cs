using IdentityGuard.Tests.Shared.TestContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Tests.Shared
{
    public interface ITestBuilder
    {
        TestContext Context { get; }
    }
}
