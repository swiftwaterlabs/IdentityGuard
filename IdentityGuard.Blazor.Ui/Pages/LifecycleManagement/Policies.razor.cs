using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityGuard.Blazor.Ui.Models;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IdentityGuard.Blazor.Ui.Pages.LifecycleManagement
{
    public partial class Policies
    {
        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public IUserPolicyService UserPolicyService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public bool IsLoading { get; set; } = false;

        public List<UserPolicy> Data { get; set; } = new();

        protected override Task OnInitializedAsync()
        {
            AppState.SetBreadcrumbs(
                new BreadcrumbItem("Lifecycle Management", Paths.Policies),
                new BreadcrumbItem("Policies", Paths.Policies)
            );
            return base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            IsLoading = true;
            Data = await UserPolicyService.Get();
            IsLoading = false;
        }

        public void ShowNew()
        {
            NavigationManager.NavigateTo($"{Paths.Policies}/new");
        }

        public void ShowEdit(string id)
        {
            NavigationManager.NavigateTo($"{Paths.Policies}/{id}");
        }
    }
}
