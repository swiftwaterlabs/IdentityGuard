using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Components.Core
{
    public partial class Loadable
    {
        [Parameter]
        public bool IsLoading { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
