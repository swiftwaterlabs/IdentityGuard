﻿using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class DirectoryObjectCards
    {
        [Parameter]
        public ILookup<string, DirectoryObject> Data { get; set; }
    }
}