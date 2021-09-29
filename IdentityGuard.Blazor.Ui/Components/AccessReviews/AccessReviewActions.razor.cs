using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Components.AccessReviews
{
    public partial class AccessReviewActions
    {
        [Parameter]
        public IEnumerable<AccessReviewAction> Actions { get; set; }
    }
}
