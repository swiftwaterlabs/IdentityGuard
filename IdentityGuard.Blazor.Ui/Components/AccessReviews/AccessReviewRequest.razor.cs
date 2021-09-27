using IdentityGuard.Blazor.Ui.Pages;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Components.AccessReviews
{
    public partial class AccessReviewRequest
    {
        [Parameter]
        public HashSet<DirectoryObject> ObjectsToReview { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public IAccessReviewService AccessReviewService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        public bool IsLoading { get; set; } = false;
        public bool IsRequesting { get; set; } = false;
        public bool HasSearched { get; set; } = false;
        public bool ArePagesVisible { get; set; } = false;
        public string SearchText { get; set; }
        public string Title { get; set; }
        public int ObjectUnderReviewCount { get; set; } = 1;

        public List<User> SearchResults { get; set; } = new();

        public HashSet<User> SelectedResults { get; set; } = new();

        public Dictionary<string,User> Reviewers { get; set; } = new();

        protected override void OnParametersSet()
        {
            SetTitle();

            base.OnParametersSet();
        }

        private void SetTitle()
        {
            ObjectUnderReviewCount = ObjectsToReview?.Count ?? 0;

            var names = ObjectsToReview?
                    .Select(o => o.DisplayName)
                    .OrderBy(o => o)
                    .Take(3);

            var namesJoined = string.Join(", ", names ?? new string[0]);

            Title = $"Select Reviewers for {namesJoined}";

            if (ObjectsToReview.Count > 3)
            {
                Title = $"{Title}...";
            }
        }

        public void AddReviewer()
        {
            foreach(var user in SelectedResults)
            {
                var key = GetReviewerKey(user);
                if(!Reviewers.ContainsKey(key))
                {
                    Reviewers.Add(key, user);
                }
            }
        }

        public void RemoveReviewer(User toRemove)
        {
            var key = GetReviewerKey(toRemove);
            if(Reviewers.ContainsKey(key))
            {
                Reviewers.Remove(key);
            }
        }

        private string GetReviewerKey(User user)
        {
            var key = $"{user.DirectoryId}|{user.Id}";
            return key;
        }

        public async Task Search()
        {
            HasSearched = true;
            SearchResults = new List<User>();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return;
            }

            IsLoading = true;

            var results = await UserService.Search(new[] { SearchText });
            SearchResults = results;
            ArePagesVisible = SearchResults.Count > 10;

            IsLoading = false;
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
            IsRequesting = true;
            var requests = ObjectsToReview
                 .Select(Map);

            var requestTasks = requests
                .Select(RequestAccessReview);

            await Task.WhenAll(requestTasks);

            IsRequesting = false;
            NavigationManager.NavigateTo(Paths.AccessReviews);

        }

        private IdentityGuard.Shared.Models.AccessReviewRequest Map(DirectoryObject toMap)
        {
            return new IdentityGuard.Shared.Models.AccessReviewRequest
            {
                ObjectId = toMap.Id,
                DirectoryId = toMap.DirectoryId,
                ObjectType = toMap.Type,
                AssignedTo = Reviewers.Values.Select(r=>Map(r)).ToList()
            };
        }

        private DirectoryObject Map(User toMap)
        {
            return new DirectoryObject
            {
                Id = toMap.Id,
                DirectoryId = toMap.DirectoryId
            };
        }

        private Task RequestAccessReview(IdentityGuard.Shared.Models.AccessReviewRequest toRequest)
        {
            return AccessReviewService.Request(toRequest);
        }
    }
}
