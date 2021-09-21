using System.Threading.Tasks;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace IdentityGuard.Blazor.Ui.Shared
{
    public partial class NavMenu
    {
        [Inject]
        public IAuthorizationService AuthorizationService { get; set; }

        public bool CanPerformAdminActions = false;

        protected override async Task OnInitializedAsync()
        {
            CanPerformAdminActions = true;//await AuthorizationService.IsAuthorized(AuthorizedActions.ManageDirectories);
        }
    }
}
