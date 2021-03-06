using IdentityGuard.Blazor.Ui.Models;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Pages.AccessReviews.Applications
{
    public partial class Current
    {
        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public IApplicationService ApplicationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string DirectoryId { get; set; }

        [Parameter]
        public string ApplicationId { get; set; }

       
        

        protected override async Task OnParametersSetAsync()
        {
            var applicationData = await ApplicationService.Get(DirectoryId, ApplicationId);

            AppState.SetBreadcrumbs(
                new BreadcrumbItem("Access Reviews", Paths.AccessReviews),
                new BreadcrumbItem(applicationData?.DirectoryName, Paths.AccessReviews),
                new BreadcrumbItem("Application", Paths.AccessReviews),
                new BreadcrumbItem(applicationData?.DisplayName, NavigationManager.Uri)
                );
        }

        
    }
}
