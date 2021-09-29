using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class ApplicationRoleCards
    {
        [Parameter]
        public ILookup<string, ApplicationRole> Data { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; } = false;

        public bool ShowDefaultAccess { get; set; }
    }
}
