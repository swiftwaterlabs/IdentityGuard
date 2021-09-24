using IdentityGuard.Blazor.Ui.Models;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq;
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

        public bool IsLoading { get; set; } = false;

        public UserAccess UserAccess { get; set; }

        public ILookup<string, DirectoryObject> OwnedObjectsByType { get; set; }
        public ILookup<string,DirectoryObject> GroupMembershipByType { get; set; }
        public ILookup<string,ApplicationRole> ApplicationRolesByAssignmentType { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            IsLoading = true;
            
            UserAccess = await UserService.UserAccess(DirectoryId, UserId);
            OwnedObjectsByType = UserAccess.OwnedObjects.ToLookup(o => o.Type);
            GroupMembershipByType = UserAccess.GroupMembership.ToLookup(o => string.IsNullOrEmpty(o.SubType) ? o.Type : o.SubType);
            ApplicationRolesByAssignmentType = UserAccess.RoleMemberships.ToLookup(o => o.AssignmentType);
            
            IsLoading = false;

            if (UserAccess == null) return;

            AppState.SetBreadcrumbs(
                new BreadcrumbItem("Access Reviews", Paths.AccessReviews),
                new BreadcrumbItem("Users", Paths.UserAccessReviews),
                new BreadcrumbItem(UserAccess.User.DisplayName, "#")
                );
        }

        public async Task StartReview()
        {
            
        }

        public void Back()
        {
            NavigationManager.NavigateTo(Paths.UserAccessReviews);
        }
    }
}
