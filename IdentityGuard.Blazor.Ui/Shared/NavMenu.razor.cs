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
        public bool CanViewUsers = false;

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
                CanViewUsers = false;
            }
            else
            {
                CanPerformAdminActions = await AuthorizationService.IsAuthorized(AuthorizedActions.ManageDirectories);
                CanViewUsers = await AuthorizationService.IsAuthorized(AuthorizedActions.ManageUsers);
                IsConfiguredForAuthenticatedUser = true;
            }

            StateHasChanged();
        }
    }
}
