using IdentityGuard.Blazor.Ui.Models;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Pages.LifecycleManagement
{
    public partial class PolicyDetails
    {
        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public ILifecyclePolicyService PolicyService { get; set; }

        [Inject] 
        public IDirectoryService DirectoryService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Id { get; set; }

        public bool IsLoading { get; set; } = false;

        public bool IsWaiting { get; set; } = false;

        public bool AreAffectedUsersShown { get; set; } = false;

        public bool IsNew { get; set; } = false;

        public bool IsEditing { get; set; } = false;

        public bool IsDeleteDialogOpen { get; set; } = false;

        public LifecyclePolicy Data { get; set; }

        public List<Directory> AvailableDirectories { get; set; }

        public List<User> AffectedUsers { get; set; } = new();

        public List<string> AvailableObjectTypes = new List<string>
        {
            ObjectTypes.User
        };

        public string TestPolicyErrorMessage { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            IsNew = string.Equals("new", Id, StringComparison.InvariantCultureIgnoreCase);
            IsEditing = IsNew;

            AppState.SetBreadcrumbs(
               new BreadcrumbItem("Lifecycle Management", Paths.Policies),
               new BreadcrumbItem("Policies", Paths.Policies)
           );

            await LoadData();

            if (!IsNew)
            {
                AppState.SetBreadcrumbs(
                   new BreadcrumbItem("Lifecycle Management", Paths.Policies),
                   new BreadcrumbItem("Policies", Paths.Policies),
                   new BreadcrumbItem(Data?.ObjectType, Paths.Policies),
                   new BreadcrumbItem(Data?.Name, NavigationManager.Uri)
                   );
            }
           
        }

        private async Task LoadData()
        {
            HideAffectedUsers();
            IsLoading = true;

            if (IsNew)
            {
                Data = new LifecyclePolicy 
                { 
                    Enabled = true, 
                    ObjectType = AvailableObjectTypes.First() 
                };
            }
            else
            {
                Data = await PolicyService.Get(Id);
            }

            AvailableDirectories = await DirectoryService.Get();

            IsLoading = false;
        }

        public void Edit()
        {
            HideAffectedUsers();
            IsEditing = true;
        }

        public void ShowDeleteDialog()
        {
            IsDeleteDialogOpen = true;
        }

        public void HideDeleteDialog()
        {
            IsDeleteDialogOpen = true;
        }

        public async Task Save()
        {
            IsLoading = true;
            if (IsNew)
            {
                await PolicyService.Post(Data);
            }
            else
            {
                await PolicyService.Put(Data);
            }

            IsLoading = false;

            NavigationManager.NavigateTo(Paths.Policies);
        }

        public Task Cancel()
        {
            if (IsNew)
            {
                NavigationManager.NavigateTo(Paths.Policies);
                return Task.CompletedTask;
            }

            IsEditing = false;
            return LoadData();
        }

        public async Task Delete()
        {
            IsWaiting = true;

            HideDeleteDialog();
            await PolicyService.Delete(Id);

            IsWaiting = false;

            NavigationManager.NavigateTo(Paths.Policies);
        }

        public async Task TestPolicy()
        {
            IsWaiting = true;
            try
            {
                TestPolicyErrorMessage = null;
                AffectedUsers = await PolicyService.Audit(this.Id);
            }
            catch
            {
                TestPolicyErrorMessage = "Invalid Query / Error when executing";
            }
            IsWaiting = false;
            ShowAffectedUsers();
        }

        public void ShowAffectedUsers()
        {
            AreAffectedUsersShown = true;
        }

        public void HideAffectedUsers()
        {
            AreAffectedUsersShown = false;
        }
    }
}
