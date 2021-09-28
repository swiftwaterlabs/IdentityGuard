using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityGuard.Blazor.Ui.Models;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace IdentityGuard.Blazor.Ui.Pages.AccessReviews
{
    public partial class Start
    {
        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public IApplicationService ApplicationService { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public IGroupService GroupService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public bool IsLoading { get; set; } = false;
        public bool HasSearched { get; set; } = false;
        public bool ArePagesVisible { get; set; } = false;

        public bool IsAccessReviewRequestVisible { get; set; } = false;

        public Dictionary<string, string> ObjectTypes { get; set; } = new ();
        public string SelectedObjectType { get; set; }

        public string SearchText { get; set; }

        public List<DirectoryObject> SearchResults { get; set; } = new ();

        public HashSet<DirectoryObject> SelectedResults { get; set; } = new ();

        protected override Task OnInitializedAsync()
        {
            AppState.SetBreadcrumbs(
                new BreadcrumbItem("Access Reviews", Paths.AccessReviews),
                new BreadcrumbItem("Start", NavigationManager.Uri)
            );

            ObjectTypes.Add("User", "User");
            ObjectTypes.Add("Group","Group");
            ObjectTypes.Add("Application","Application");

            SelectedObjectType = ObjectTypes.Keys.First();

            return base.OnParametersSetAsync();
        }

        public async Task Search()
        {
            HasSearched = true;
            SearchResults = new List<DirectoryObject>();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return;
            }
            switch (SelectedObjectType)
            {
                case "Application":
                {
                    IsLoading = true;
                    var results = await ApplicationService.Search(new[] {SearchText});
                    SearchResults = results
                        .Select(Map)
                        .OrderBy(r=>r.DirectoryName).ThenBy(r=>r.DisplayName)
                        .ToList();
                    ArePagesVisible = SearchResults.Count > 10;
                    IsLoading = false;
                    return;
                }
                case "User":
                {
                    IsLoading = true;
                    var results = await UserService.Search(new[] { SearchText });
                    SearchResults = results
                        .Select(Map)
                        .OrderBy(r => r.DirectoryName).ThenBy(r => r.DisplayName)
                        .ToList();
                    ArePagesVisible = SearchResults.Count > 10;
                    IsLoading = false;
                    return;
                }
                case "Group":
                {
                    IsLoading = true;
                    var results = await GroupService.Search(new[] { SearchText });
                    SearchResults = results
                        .Select(Map)
                        .OrderBy(r => r.DirectoryName).ThenBy(r => r.DisplayName)
                        .ToList();
                    ArePagesVisible = SearchResults.Count > 10;
                    IsLoading = false;
                    return;
                }
                default:
                {
                    return;
                }

            }
        }

        private DirectoryObject Map(Application toMap)
        {
            return new DirectoryObject
            {
                Id = toMap.Id,
                DisplayName = toMap.DisplayName,
                DirectoryName = toMap.DirectoryName,
                DirectoryId = toMap.DirectoryId,
                Type = "Application",
                SubType = toMap.ServicePrincipal?.Type ?? "Application"
            };
        }

        private DirectoryObject Map(User toMap)
        {
            return new DirectoryObject
            {
                Id = toMap.Id,
                DisplayName = toMap.DisplayName,
                DirectoryName = toMap.DirectoryName,
                DirectoryId = toMap.DirectoryId,
                Type = "User",
                SubType = toMap.Type
            };
        }

        private DirectoryObject Map(Group toMap)
        {
            return new DirectoryObject
            {
                Id = toMap.Id,
                DisplayName = toMap.DisplayName,
                DirectoryName = toMap.DirectoryName,
                DirectoryId = toMap.DirectoryId,
                Type = "Group",
                SubType = string.Join(",",toMap.Types ?? new List<string>())
            };
        }

        public async Task OnSearchKeyPress(KeyboardEventArgs input)
        {
            if (input.Code == "Enter")
            {
                await Search();
            }
        }

        public async Task RequestAccessReviews()
        {
            IsAccessReviewRequestVisible = true;
        }

        public void ShowAccess(DirectoryObject item)
        {
            var path = GetStartReviewPath(item);
            NavigationManager.NavigateTo(path);
        }

        private string GetStartReviewPath(DirectoryObject item)
        {
            switch (SelectedObjectType)
            {
                case "Application":
                {
                    return $"{Paths.ApplicationAccessReviews}/{item.DirectoryId}/{item.Id}";
                }
                case "User":
                {
                    return $"{Paths.UserAccessReviews}/{item.DirectoryId}/{item.Id}";
                }
                case "Group":
                {
                    return $"{Paths.GroupAccessReviews}/{item.DirectoryId}/{item.Id}";
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(item), item.Type);
                }

            }
        }
    }
}
