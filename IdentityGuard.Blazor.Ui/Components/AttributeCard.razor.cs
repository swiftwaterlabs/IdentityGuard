using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class AttributeCard
    {
        [Parameter]
        public Dictionary<string, string> Data { get; set; } = new();
    }
}
