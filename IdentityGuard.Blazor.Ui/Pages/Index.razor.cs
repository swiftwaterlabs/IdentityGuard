using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Pages
{
    public partial class Index
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public void ShowAccessReviewPage()
        {
            NavigationManager.NavigateTo(Paths.AccessReviews);
        }
    }
}
