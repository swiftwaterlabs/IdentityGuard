using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class DirectoryObjectGroupCard
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public IEnumerable<DirectoryObject> Data { get; set; }

        [Parameter]
        public string Type { get; set; }
    }
}
