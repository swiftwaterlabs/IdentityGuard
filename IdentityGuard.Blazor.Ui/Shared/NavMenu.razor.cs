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
        public IAuthorizationService AuthorizationService { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateService { get; set; }

        public bool CanPerformAdminActions = false;

        protected override async Task OnParametersSetAsync()
        {
            var state = await AuthenticationStateService.GetAuthenticationStateAsync();

            if (!state.User.Identity.IsAuthenticated)
            {
                CanPerformAdminActions = false;
                StateHasChanged();
            }
            else
            {
                CanPerformAdminActions = await AuthorizationService.IsAuthorized(AuthorizedActions.ManageDirectories);
                StateHasChanged();
            }
        }
    }
}
