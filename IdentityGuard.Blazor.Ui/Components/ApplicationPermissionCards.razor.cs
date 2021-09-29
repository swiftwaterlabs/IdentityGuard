using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class ApplicationPermissionCards
    {
        [Parameter]
        public ILookup<string, ApplicationPermission> Data { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; } = false;

        [Parameter]
        public Action<string, string> OnItemRemoved { get; set; } = (type, id) => { };

        [Parameter]
        public Action<string, string> OnItemAdded { get; set; } = (type, id) => { };
    }
}
