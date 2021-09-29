using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class RoleGroupCard
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public IEnumerable<Role> Data { get; set; }

        [Parameter]
        public bool ShowDefaultAccess { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; } = false;
    }
}
