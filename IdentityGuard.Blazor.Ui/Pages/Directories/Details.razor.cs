using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Pages.Directories
{
    public partial class Details
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        public IDirectoryService DirectoryService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public List<BreadcrumbItem> BreadCrumbs { get; set; }

        public bool IsNew { get; set; }

        public bool IsLoading { get; set; }

        public bool IsEditing { get; set; }

        public Directory Data { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            IsNew = string.Equals("new", Id, StringComparison.InvariantCultureIgnoreCase);
            IsEditing = IsNew;

            BreadCrumbs.Clear();
            BreadCrumbs.Add(new BreadcrumbItem("Directories", Paths.Directories));

            await LoadData();
        }

        private async Task LoadData()
        {
            IsLoading = true;

            if (IsNew)
            {
                Data = new Directory { ClientType = DirectoryClientType.ManagedIdentity };
            }
            else
            {
                Data = await DirectoryService.Get(Id);
            }
            

            IsLoading = false;
        }

        public void Edit()
        {
            IsEditing = true;
        }

        public async Task Save()
        {
            IsLoading = true;
            if(IsNew)
            {
                await DirectoryService.Post(Data);
            }
            else
            {
                await DirectoryService.Put(Data);
            }

            IsLoading = false;

            NavigationManager.NavigateTo("directory");
        }

        public Task Cancel()
        {
            if(IsNew)
            {
                NavigationManager.NavigateTo("directory");
                return Task.CompletedTask;
            }

            IsEditing = false;
            return LoadData();
        }

        public void Back()
        {
            NavigationManager.NavigateTo("directory");
        }
    }
}
