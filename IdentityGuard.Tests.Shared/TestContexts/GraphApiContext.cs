using Microsoft.Graph;
using System.Collections.Generic;

namespace IdentityGuard.Tests.Shared.TestContexts
{
    public class GraphApiContext
    {
        public Dictionary<string, List<Application>> Applications = new Dictionary<string, List<Application>>();
    }
}
