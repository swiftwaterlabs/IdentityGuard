using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityGuard.Blazor.Ui.Models;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IdentityGuard.Blazor.Ui.Pages.AccessReviews
{
    public partial class Index
    {
        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAccessReviewService AccessReviewService { get; set; }

        public bool IsLoading { get; set; } = false;

        public List<AccessReview> PendingAccessReviews { get; set; } = new ();
        public List<AccessReview> CompletedAccessReviews { get; set; } = new ();

        protected override Task OnInitializedAsync()
        {
            AppState.SetBreadcrumbs(
                new BreadcrumbItem("Access Reviews", Paths.AccessReviews)
            );

            return LoadData();
        }

        private async Task LoadData()
        {
            IsLoading = true;

            var pendingTask = AccessReviewService.GetPending();
            var completeTask = AccessReviewService.GetComplete();

            await Task.WhenAll(pendingTask, completeTask);

            PendingAccessReviews = await pendingTask;
            CompletedAccessReviews = await completeTask;

            IsLoading = false;
        }

        public Task StartNewAccessReview()
        {
            return Task.CompletedTask;
        }

        public void ShowAccessReview(string id)
        {

        }

        public void ShowNew()
        {
            NavigationManager.NavigateTo(Paths.NewAccessReviews);
        }
    }
}
