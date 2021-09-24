using IdentityGuard.Blazor.Ui.Models;
using IdentityGuard.Blazor.Ui.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;
using IdentityGuard.Shared.Models;
using System.Collections.Generic;

namespace IdentityGuard.Blazor.Ui.Pages.AccessReviews.Users
{
    public partial class Index
    {
        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        public bool IsSearchVisible { get; set; } = false;

        public string SearchText { get; set; }

        public List<User> UserSearchResults { get; set; } = new();

        public User SelectedUser { get; set; }

        public UserAccess UserAccess { get; set; }

        protected override Task OnInitializedAsync()
        {
            AppState.SetBreadcrumbs(
                new BreadcrumbItem("Access Reviews", Paths.AccessReviews),
                new BreadcrumbItem("Users", Paths.UserAccessReviews)
                );
            return base.OnInitializedAsync();
        }

        public void ShowUserSearch()
        {
            IsSearchVisible = true;
        }

        public async Task ShowUserAccessForSelectedUser()
        {
            CloseUserSearch();
            await ShowUserAccess(SelectedUser.DirectoryId, SelectedUser.Id);
        }

        public void CloseUserSearch()
        {
            IsSearchVisible = false;
        }

        public async Task SearchUsers()
        {
            SelectedUser = null;
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                UserSearchResults = new List<User>();
            }
            else
            {
                UserSearchResults = await UserService.Search(new[] { SearchText });
            }  
        }

        public async Task ShowUserAccess(string directoryId, string userId)
        {
            UserAccess = await UserService.UserAccess(directoryId, userId);
        }
    }
}
