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

        public bool CanPerformAdminActions = false;

        private bool IsConfiguredForAuthenticatedUser = false;

        protected override Task OnParametersSetAsync()
        {
            return RefreshMenu();
        }

        
        public async Task RefreshMenu()
        {
            if (IsConfiguredForAuthenticatedUser) return;

            var state = await AuthenticationStateService.GetAuthenticationStateAsync();

            if (!state.User.Identity.IsAuthenticated)
            {
                CanPerformAdminActions = false;
            }
            else
            {
                CanPerformAdminActions = await AuthorizationService.IsAuthorized(AuthorizedActions.ManageDirectories);
                IsConfiguredForAuthenticatedUser = true;
            }

            StateHasChanged();
        }
    }
}
