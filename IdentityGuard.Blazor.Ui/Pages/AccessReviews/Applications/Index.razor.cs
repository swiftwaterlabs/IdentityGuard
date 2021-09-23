using IdentityGuard.Blazor.Ui.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Pages.AccessReviews.Applications
{
    public partial class Index
    {
        [Inject]
        public AppState AppState { get; set; }

        protected override Task OnInitializedAsync()
        {
            AppState.SetBreadcrumbs(
                new BreadcrumbItem("Access Reviews", Paths.AccessReviews),
                new BreadcrumbItem("Applications", Paths.ApplicationAccessReviews)
                );
            return base.OnInitializedAsync();
        }
    }
}
