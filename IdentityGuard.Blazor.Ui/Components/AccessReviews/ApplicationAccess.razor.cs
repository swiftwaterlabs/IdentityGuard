using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Components.AccessReviews
{
    public partial class ApplicationAccess
    {
        [Inject]
        public IApplicationService ApplicationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        IWindowService WindowService { get; set; }

        [Parameter]
        public string DirectoryId { get; set; }

        [Parameter]
        public string ApplicationId { get; set; }

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
        public bool IsApplicationLoaded { get; set; } = false;

        public string ApplicationName { get; set; }
        public string DirectoryName { get; set; }
        public string ManagementUrl { get; set; }

        

        public Dictionary<string, string> UserAttributes { get; set; }
        public ILookup<string, DirectoryObject> OwnersByType { get; set; }
        public ILookup<string, ApplicationSecret> SecretsByType { get; set; }
        public ILookup<string, ApplicationPermission> PermissionsByResource { get; set; }
        public ILookup<string, Role> RolesByType { get; set; }
        

        protected override async Task OnParametersSetAsync()
        {
            if (string.IsNullOrEmpty(DirectoryId) || string.IsNullOrEmpty(ApplicationId)) return;
            if (IsApplicationLoaded) return;

            await LoadData();
        }

        public Task RefreshData()
        {
            return LoadData();
        }

        private async Task LoadData()
        {
            IsLoading = true;

            var data = await ApplicationService.ApplicationAccess(DirectoryId, ApplicationId);

            IsApplicationLoaded = data != null;

            if (data != null)
            {
                DirectoryName = data.Application.DirectoryName;
                ApplicationName = data.Application.DisplayName;

                UserAttributes = new Dictionary<string, string>
                {
                    {"Client Id",data.Application.AppId},
                    {"Service Principal Name",data.Application.ServicePrincipal?.DisplayName},
                    {"Service Principal Type",data.Application.ServicePrincipal?.Type},
                };

                OwnersByType = MergeOwners(data.Application);
                SecretsByType = data.Application.Secrets.ToLookup(o => o.Type);
                PermissionsByResource = data.Application.Permissions.ToLookup(p => p.ResourceName);
                RolesByType = data.Application.Roles.Values.ToLookup(o => o.Source);

                ManagementUrl = data.Application.ManagementUrl;
            }
            IsLoading = false;
        }

        private ILookup<string, DirectoryObject> MergeOwners(Application toMerge)
        {
            var applicationOwners = toMerge
                .Owners
                .Select(o => new KeyValuePair<string, DirectoryObject>(ObjectTypes.Application, o));

            var servicePrincipalOwners = toMerge
                .ServicePrincipal?
                .Owners?
                .Select(o => new KeyValuePair<string, DirectoryObject>(ObjectTypes.ServicePrincipal, o));

            if (servicePrincipalOwners != null)
            {
                return applicationOwners
                    .Union(servicePrincipalOwners)
                    .ToLookup(o => o.Key, o => o.Value);
            }

            return applicationOwners
                .ToLookup(o => o.Key, o => o.Value);

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
