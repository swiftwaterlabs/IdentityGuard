using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;

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

        [Parameter]
        public bool ReadOnly { get; set; } = false;

        [Parameter]
        public Action<string, string> OnItemRemoved { get; set; } = (type, id) => { };

        [Parameter]
        public Action<string, string> OnItemAdded { get; set; } = (type, id) => { };
    }
}
