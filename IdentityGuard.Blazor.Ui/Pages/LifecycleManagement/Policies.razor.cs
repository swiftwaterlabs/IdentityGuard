using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityGuard.Blazor.Ui.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IdentityGuard.Blazor.Ui.Pages.LifecycleManagement
{
    public partial class Policies
    {
        [Inject]
        public AppState AppState { get; set; }

        protected override Task OnInitializedAsync()
        {
            AppState.SetBreadcrumbs(
                new BreadcrumbItem("Lifecycle Management", Paths.Policies),
                new BreadcrumbItem("Policies", Paths.Policies)
            );
            return base.OnInitializedAsync();
        }
    }
}
