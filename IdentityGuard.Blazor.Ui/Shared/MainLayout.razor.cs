using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Shared
{
    public partial class MainLayout
    {
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            RequireAuthenticatedUser(state.User);
        }

        private void RequireAuthenticatedUser(ClaimsPrincipal user)
        {
            var isUserAuthenticated = user?.Identity?.IsAuthenticated ?? false;
            var isAuthenticationCallback = Navigation.Uri.Contains("/authentication/", StringComparison.OrdinalIgnoreCase);

            if (!isUserAuthenticated && !isAuthenticationCallback)
            {
                Navigation.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(Navigation.Uri)}");
            }
        }
    }
}
