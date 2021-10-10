using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityGuard.Blazor.Ui.Themes;
using MudBlazor.ThemeManager;
using MudBlazor;
using IdentityGuard.Blazor.Ui.Models;

namespace IdentityGuard.Blazor.Ui.Shared
{
    public partial class MainLayout:IDisposable
    {
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        [Inject]
        AppState AppState { get; set; }

        public NavMenu NavMenu { get; set; }

        public bool IsDrawerOpen = false;

        protected override async Task OnInitializedAsync()
        {
            AppState.OnChange += StateHasChanged;

            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            RequireAuthenticatedUser(state.User);

            StateHasChanged();
        }

        public void Dispose()
        {
            AppState.OnChange -= StateHasChanged;
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

        private readonly ThemeManagerTheme _themeManager = new()
        {
            Theme = new EnterpriseTheme(),
            DrawerClipMode = DrawerClipMode.Always,
            FontFamily = "Montserrat",
            DefaultBorderRadius = 6,
            AppBarElevation = 1,
            DrawerElevation = 1
        };

        void DrawerToggle()
        {
            IsDrawerOpen = !IsDrawerOpen;
            NavMenu?.RefreshMenu();
        }
    }
}
