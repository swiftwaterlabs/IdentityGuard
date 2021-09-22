using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Pages.Directories
{
    public partial class Index
    {
        [Inject]
        public IDirectoryService DirectoryService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<Directory> Data { get; set; } = new List<Directory>();

        public bool IsLoading { get; set; }

        protected override Task OnInitializedAsync()
        {
            return RefreshData();
        }

        private async Task RefreshData()
        {
            IsLoading = true;

            Data = await DirectoryService.Get();

            IsLoading = false;
        }

        public void ShowNew()
        {
            NavigationManager.NavigateTo("directory/new");
        }

        public void ShowEdit(string id)
        {
            NavigationManager.NavigateTo($"directory/{id}");
        }


    }
}
