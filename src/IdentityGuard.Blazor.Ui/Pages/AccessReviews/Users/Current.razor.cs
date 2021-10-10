using IdentityGuard.Blazor.Ui.Models;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Pages.AccessReviews.Users
{
    public partial class Current
    {
        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string DirectoryId { get; set; }

        [Parameter]
        public string UserId { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var userData = await GetUserData();

            AppState.SetBreadcrumbs(
                new BreadcrumbItem("Access Reviews", Paths.AccessReviews),
                new BreadcrumbItem(userData?.DirectoryName, Paths.AccessReviews),
                new BreadcrumbItem("User", Paths.AccessReviews),
                new BreadcrumbItem(userData?.DisplayName, NavigationManager.Uri)
                );
        }

        private Task<User> GetUserData()
        {
            return UserService.Get(DirectoryId, UserId);
        }

   
    }
}
