using System.Threading.Tasks;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityGuard.Blazor.Ui.Shared
{
    public partial class NavMenu
    {
        [Inject]
        public Services.IAuthorizationService AuthorizationService { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateService { get; set; }

        public bool IsLoading { get; set; } = false;
        public bool CanPerformAdminActions { get; set; } = false;
        public bool CanPerformAccessReviews { get; set; } = false;

        private bool IsConfiguredForAuthenticatedUser = false;

        protected override Task OnParametersSetAsync()
        {
            return RefreshMenu();
        }

        
        public async Task RefreshMenu()
        {
            if (IsConfiguredForAuthenticatedUser) return;

            IsLoading = true;
            var state = await AuthenticationStateService.GetAuthenticationStateAsync();

            if (!state.User.Identity.IsAuthenticated)
            {
                CanPerformAdminActions = false;
                CanPerformAccessReviews = false;
            }
            else
            {
                CanPerformAdminActions = await AuthorizationService.IsAuthorized(AuthorizedActions.ManageDirectories);
                CanPerformAccessReviews = await AuthorizationService.IsAuthorized(AuthorizedActions.AccessReviewContributor);
                IsConfiguredForAuthenticatedUser = true;
            }

            IsLoading = false;

            StateHasChanged();
        }
    }
}
