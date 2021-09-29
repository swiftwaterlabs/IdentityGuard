using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class ApplicationPermissionGroupCard
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public IEnumerable<ApplicationPermission> Data { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; } = false;

        [Parameter]
        public Action<string, string> OnItemRemoved { get; set; } = (type, id) => { };

        [Parameter]
        public Action<string, string> OnItemAdded { get; set; } = (type, id) => { };
    }
}
