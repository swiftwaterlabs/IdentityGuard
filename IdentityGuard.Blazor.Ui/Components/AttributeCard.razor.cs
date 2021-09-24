using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class AttributeCard
    {
        [Parameter]
        public Dictionary<string, string> Data { get; set; } = new();
    }
}
