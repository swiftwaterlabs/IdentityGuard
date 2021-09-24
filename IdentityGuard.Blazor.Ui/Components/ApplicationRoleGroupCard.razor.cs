using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
