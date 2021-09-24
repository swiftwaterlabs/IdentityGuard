using IdentityGuard.Blazor.Ui.Models;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
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
        public bool IsUserLoaded { get; set; } = false;
        public bool ShowDefaultAccessRoles { get; set; } = true;

        public UserAccess UserAccess { get; set; }

        public string UserName { get; set; }
        public string DirectoryName { get; set; }
        
        public Dictionary<string,string> UserAttributes { get; set; }
        public ILookup<string, DirectoryObject> OwnedObjectsByType { get; set; }
        public ILookup<string,DirectoryObject> GroupMembershipByType { get; set; }
        public ILookup<string,ApplicationRole> ApplicationRolesByAssignmentType { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await LoadUserData();

            if (!IsUserLoaded) return;

            AppState.SetBreadcrumbs(
                new BreadcrumbItem("Access Reviews", Paths.AccessReviews),
                new BreadcrumbItem("Users", Paths.UserAccessReviews),
                new BreadcrumbItem(UserAccess.User.DisplayName, "#")
                );
        }

        private async Task LoadUserData()
        {
            IsLoading = true;

            UserAccess = await UserService.UserAccess(DirectoryId, UserId);

            IsUserLoaded = UserAccess != null;

            if (UserAccess != null)
            {
                DirectoryName = UserAccess.User.DirectoryName;
                UserName = UserAccess.User.DisplayName;

                UserAttributes = new Dictionary<string, string>
                {
                    {"First Name",UserAccess.User.GivenName},
                    {"Last Name",UserAccess.User.SurName},
                    {"Email",UserAccess.User.EmailAddress},
                    {"Job Title",UserAccess.User.JobTitle},
                    {"Company",UserAccess.User.Company},
                    {"Type",UserAccess.User.Type},
                };

                OwnedObjectsByType = UserAccess.OwnedObjects.ToLookup(o => o.Type);
                GroupMembershipByType = UserAccess.GroupMembership.ToLookup(o => string.IsNullOrEmpty(o.SubType) ? o.Type : o.SubType);
                ApplicationRolesByAssignmentType = UserAccess.RoleMemberships.ToLookup(o => o.AssignmentType);
            }
            IsLoading = false;
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
