﻿using IdentityGuard.Blazor.Ui.Models;
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
                new BreadcrumbItem(UserName, "#")
                );
        }

        private async Task LoadUserData()
        {
            IsLoading = true;

            var userAccess = await UserService.UserAccess(DirectoryId, UserId);

            IsUserLoaded = userAccess != null;

            if (userAccess != null)
            {
                DirectoryName = userAccess.User.DirectoryName;
                UserName = userAccess.User.DisplayName;

                UserAttributes = new Dictionary<string, string>
                {
                    {"First Name",userAccess.User.GivenName},
                    {"Last Name",userAccess.User.SurName},
                    {"Email",userAccess.User.EmailAddress},
                    {"Job Title",userAccess.User.JobTitle},
                    {"Company",userAccess.User.Company},
                    {"Type",userAccess.User.Type},
                };

                OwnedObjectsByType = userAccess.OwnedObjects.ToLookup(o => o.Type);
                GroupMembershipByType = userAccess.GroupMembership.ToLookup(o => string.IsNullOrEmpty(o.SubType) ? o.Type : o.SubType);
                ApplicationRolesByAssignmentType = userAccess.RoleMemberships.ToLookup(o => o.AssignmentType);
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