using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class ApplicationPermissionCards
    {
        [Parameter]
        public ILookup<string, ApplicationPermission> Data { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; } = false;
    }
}
