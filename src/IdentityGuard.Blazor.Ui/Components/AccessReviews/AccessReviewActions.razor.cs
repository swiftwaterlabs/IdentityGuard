using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace IdentityGuard.Blazor.Ui.Components.AccessReviews
{
    public partial class AccessReviewActions
    {
        [Parameter]
        public IEnumerable<AccessReviewAction> Actions { get; set; }
    }
}
