using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class ApplicationRoleGroupCard
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public IEnumerable<ApplicationRole> Data { get; set; }

        [Parameter]
        public bool ShowDefaultAccess { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; } = false;

        [Parameter]
        public Action<string, string, string> OnItemRemoved { get; set; } = (type, subType, id) => { };

        [Parameter]
        public Action<string, string, string> OnItemAdded { get; set; } = (type, subType, id) => { };
    }
}
