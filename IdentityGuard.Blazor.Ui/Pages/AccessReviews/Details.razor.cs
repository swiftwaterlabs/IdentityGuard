using IdentityGuard.Blazor.Ui.Models;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
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

        public Components.AccessReviews.ApplicationAccess ApplicationComponent { get; set; }
        public Components.AccessReviews.UserAccess UserComponent { get; set; }
        public Components.AccessReviews.GroupAccess GroupComponent { get; set; }
        public AccessReview AccessReview { get; set; }

        public bool CanPerformActions { get; set; } = false;

        public bool IsLoading { get; set; } = false;
        public bool IsRequesting { get; set; } = false;
        public bool IsApplyChangesDialogOpen { get; set; } = false;
        public bool IsAbandonDialogOpen { get; set; } = false;
        public bool IsCompleteDialogOpen { get; set; } = false;

        private Dictionary<string,AccessReviewActionRequest> ActionsTaken { get; set; } = new();

        protected override async Task OnParametersSetAsync()
        {
            await LoadData();

            AppState.SetBreadcrumbs(
               new BreadcrumbItem("Access Reviews", Paths.AccessReviews),
               new BreadcrumbItem(AccessReview?.DirectoryName, Paths.AccessReviews),
               new BreadcrumbItem(AccessReview?.ObjectType, Paths.AccessReviews),
               new BreadcrumbItem(AccessReview?.DisplayName, NavigationManager.Uri)
               );
        }

        private async Task LoadData()
        {
            IsLoading = true;

            AccessReview = await AccessReviewService.Get(Id);
            CanPerformActions = AccessReview != null &&
                (AccessReview.Status == AccessReviewStatus.New || AccessReview.Status == AccessReviewStatus.InProgress);

            IsLoading = false;           
        }

        public async Task Complete()
        {
            IsRequesting = true;

            await AccessReviewService.Complete(Id);
            NavigationManager.NavigateTo(Paths.AccessReviews);

            IsRequesting = false;
        }

        public async Task Abandon()
        {
            IsRequesting = true;

            await AccessReviewService.Abandon(Id);
            NavigationManager.NavigateTo(Paths.AccessReviews);

            IsRequesting = false;
        }

        public async Task ApplyChanges()
        {
            HideApplyChangesDialog();
            IsRequesting = true;

            await AccessReviewService.ApplyChanges(Id,ActionsTaken.Values);
            ActionsTaken.Clear();

            IsRequesting = false;

            await RefreshData();
        }

        private async Task RefreshData()
        {
            await LoadData();
            IsLoading = true;

            await InvokeAsync(() => {
                StateHasChanged();
            });
            IsLoading = false;
        }

        public void ShowAbandonDialog()
        {
            IsAbandonDialogOpen = true;
        }

        public void HideAbandonDialog()
        {
            IsAbandonDialogOpen = false;
        }

        public void ShowCompleteDialog()
        {
            IsCompleteDialogOpen = true;
        }

        public void HideCompleteDialog()
        {
            IsCompleteDialogOpen = false;
        }

        public void ShowApplyChangesDialog()
        {
            IsApplyChangesDialogOpen = true;
        }

        public void HideApplyChangesDialog()
        {
            IsApplyChangesDialogOpen = false;
            StateHasChanged();
        }

        private void RemoveAccessReviewItem(string type, string id)
        {
            var key = GetActionKey(type, id);
            if(!ActionsTaken.ContainsKey(key))
            {
                var action = new AccessReviewActionRequest
                {
                    Action = AccessReviewActionTypes.Remove,
                    ActionObjectId = id,
                    ActionObjectType = type
                };
                ActionsTaken.Add(key, action);
                
            }

            StateHasChanged();
        }

        private void AddAccessReviewItem(string type, string id)
        {
            var key = GetActionKey(type, id);
            if (ActionsTaken.ContainsKey(key))
            {
                ActionsTaken.Remove(key);
            }

            StateHasChanged();
        }

        private string GetActionKey(string type, string id)
        {
            return $"{type}|{id}";
        }

        public bool HasPendingActions()
        {
            return ActionsTaken.Any();
        }
    }
}
