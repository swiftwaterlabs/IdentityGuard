using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class ApplicationRoleCards
    {
        [Parameter]
        public ILookup<string, ApplicationRole> Data { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; } = false;

        [Parameter]
        public Action<string, string> OnItemRemoved { get; set; } = (type, id) => { };

        [Parameter]
        public Action<string, string> OnItemAdded { get; set; } = (type, id) => { };

        public bool ShowDefaultAccess { get; set; }
    }
}
