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

namespace IdentityGuard.Blazor.Ui.Shared
{
    public partial class MainLayout
    {
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        public NavMenu NavMenu { get; set; }

        public bool IsDrawerOpen = false;

        public List<BreadcrumbItem> BreadCrumbs = new();

        protected override async Task OnInitializedAsync()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            RequireAuthenticatedUser(state.User);

            StateHasChanged();
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
