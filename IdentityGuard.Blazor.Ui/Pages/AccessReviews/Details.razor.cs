using IdentityGuard.Blazor.Ui.Models;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Pages.AccessReviews
{
    public partial class Details
    {
        [Inject]
        public AppState AppState { get; set; }
        

        [Inject]
        public IAccessReviewService AccessReviewService { get; set; }

        [Parameter]
        public string Id { get; set; }

        public AccessReview AccessReview { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            AccessReview = await AccessReviewService.Get(Id);

            AppState.SetBreadcrumbs(
               new BreadcrumbItem("Access Reviews", Paths.AccessReviews),
               new BreadcrumbItem(AccessReview?.ObjectType, Paths.AccessReviews),
               new BreadcrumbItem(AccessReview?.DisplayName, Paths.AccessReviews)
               );
        }

        public async Task Complete()
        {

        }

        public async Task Abandon()
        {
            
        }
    }
}
