using IdentityGuard.Blazor.Ui.Models;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Components.AccessReviews
{
    public partial class GroupAccess
    {
        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public IGroupService GroupService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        IWindowService WindowService { get; set; }

        [Parameter]
        public string DirectoryId { get; set; }

        [Parameter]
        public string GroupId { get; set; }

        [Parameter]
        public bool AllowNewAccessReview { get; set; } = false;

        [Parameter]
        public RenderFragment Actions { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; } = false;

        [Parameter]
        public Action<string, string> OnItemRemoved { get; set; } = (type, id) => { };

        [Parameter]
        public Action<string, string> OnItemAdded { get; set; } = (type, id) => { };

        public bool IsLoading { get; set; } = false;
        public bool IsGroupLoaded { get; set; } = false;

        public string GroupName { get; set; }
        public string DirectoryName { get; set; }
        public string ManagementUrl { get; set; }

        public Dictionary<string, string> UserAttributes { get; set; }
        public ILookup<string, DirectoryObject> OwnersByType { get; set; }
        public ILookup<string, DirectoryObject> MembersByType { get; set; }
        public ILookup<string, ApplicationRole> ApplicationRolesByAssignmentType { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (string.IsNullOrEmpty(DirectoryId) || string.IsNullOrEmpty(GroupId)) return;
            if (IsGroupLoaded) return;

            await LoadData();
        }

        public Task RefreshData()
        {
            return LoadData();
        }

        private async Task LoadData()
        {
            IsLoading = true;

            var data = await GroupService.GroupAccess(DirectoryId, GroupId);

            IsGroupLoaded = data != null;

            if (data != null)
            {
                DirectoryName = data.Group.DirectoryName;
                GroupName = data.Group.DisplayName;

                UserAttributes = new Dictionary<string, string>
                {
                    {"Description",data.Group.Description},
                    {"Type",string.Join(",",data.Group.Types)},
                    {"Source",data.Group.Source},
                    {"Membership Rules",data.Group.DynamicMembershipRule}
                };

                OwnersByType = data.Group.Owners.ToLookup(o => o.Type);
                MembersByType = data.Group.Members.ToLookup(o => o.Type);
                ApplicationRolesByAssignmentType = data.RoleMemberships.ToLookup(o => o.AssignmentType);

                ManagementUrl = data.Group.ManagementUrl;
            }
            IsLoading = false;
        }

        public async Task StartReview()
        {

        }

        public Task ShowManagementPage()
        {
            return WindowService.Open(ManagementUrl);
        }
    }
}
