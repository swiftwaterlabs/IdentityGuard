using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class DirectoryObjectCards
    {
        [Parameter]
        public ILookup<string, DirectoryObject> Data { get; set; }

        [Parameter]
        public string Type { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; } = false;

        [Parameter]
        public Action<string, string, string> OnItemRemoved { get; set; } = (type, subType, id) => { };

        [Parameter]
        public Action<string, string, string> OnItemAdded { get; set; } = (type, subType, id) => { };
    }
}
