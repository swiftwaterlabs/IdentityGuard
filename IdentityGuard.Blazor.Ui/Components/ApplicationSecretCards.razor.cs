using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class ApplicationSecretCards
    {
        [Parameter]
        public ILookup<string, ApplicationSecret> Data { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; } = false;
    }
}
