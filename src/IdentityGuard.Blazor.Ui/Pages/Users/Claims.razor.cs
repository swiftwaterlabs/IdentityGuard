using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityGuard.Blazor.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityGuard.Blazor.Ui.Pages.Users
{
    public partial class Claims
    {
        [Inject]
        public IUserService Api { get; set; }

        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public List<KeyValuePair<string, string>> ClaimValues { get; set; } = new List<KeyValuePair<string, string>>();

        protected override async Task OnInitializedAsync()
        {
            var userState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (!userState.User.Identity.IsAuthenticated) return;

            ClaimValues = await Api.GetUserClaims();
        }

    }
}
