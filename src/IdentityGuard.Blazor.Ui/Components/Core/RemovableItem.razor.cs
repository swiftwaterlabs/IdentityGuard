using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Components.Core
{
    public partial class RemovableItem
    {
        [Parameter]
        public bool IsRemoved { get; set; } = false;

        [Parameter]
        public MudBlazor.Typo Typo { get; set; } = MudBlazor.Typo.body1;

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public string Type { get; set; }

        [Parameter]
        public string SubType { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]
        public Action<string,string,string> OnItemRemoved { get; set; } = (type,subType,id) => { };

        [Parameter]
        public Action<string,string,string> OnItemAdded { get; set; } = (type,subType,id) => { };

        public string GetIcon()
        {
            if (IsRemoved) return MudBlazor.Icons.Material.Filled.Undo;

            return MudBlazor.Icons.Material.Filled.Delete;
        }

        public string GetTextStyle()
        {
            if (IsRemoved) return "text-decoration: line-through";

            return "";
        }
        public void CommandClicked()
        {
            if (Disabled) return;

            IsRemoved = !IsRemoved;

            if (IsRemoved)
            {
                OnItemRemoved(Type,SubType,Id);
            }

            if(!IsRemoved)
            {
                OnItemAdded(Type,SubType,Id);
            }

        }

    }
}
