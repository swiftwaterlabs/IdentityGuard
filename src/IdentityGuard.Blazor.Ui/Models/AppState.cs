using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityGuard.Blazor.Ui.Models
{
    public class AppState
    {
        public event Action OnChange;

        private List<BreadcrumbItem> _breadCrumbs = new();

        public List<BreadcrumbItem> BreadCrumbs
        {
            get { return _breadCrumbs; }
        }

        public void SetBreadcrumbs(params BreadcrumbItem[] items)
        {
            _breadCrumbs = items?.ToList() ?? new List<BreadcrumbItem>();
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
