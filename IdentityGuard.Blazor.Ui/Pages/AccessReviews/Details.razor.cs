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

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Id { get; set; }

        public AccessReview AccessReview { get; set; }

        public bool CanPerformActions { get; set; } = false;

        protected override async Task OnParametersSetAsync()
        {
            AccessReview = await AccessReviewService.Get(Id);
            CanPerformActions = AccessReview != null && 
                (AccessReview.Status == AccessReviewStatus.New || AccessReview.Status == AccessReviewStatus.InProgress);

            AppState.SetBreadcrumbs(
               new BreadcrumbItem("Access Reviews", Paths.AccessReviews),
               new BreadcrumbItem(AccessReview?.ObjectType, Paths.AccessReviews),
               new BreadcrumbItem(AccessReview?.DisplayName, Paths.AccessReviews)
               );
        }

        public async Task Complete()
        {
            await AccessReviewService.Complete(Id);

            NavigationManager.NavigateTo(Paths.AccessReviews);
        }

        public async Task Abandon()
        {
            await AccessReviewService.Abandon(Id);

            NavigationManager.NavigateTo(Paths.AccessReviews);
        }
    }
}
