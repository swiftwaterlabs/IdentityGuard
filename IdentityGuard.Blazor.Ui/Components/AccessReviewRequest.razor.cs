using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Components
{
    public partial class AccessReviewRequest
    {
        [Parameter]
        public List<DirectoryObject> ObjectsToReview { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public IAccessReviewService AccessReviewService { get; set; }

        public bool IsLoading { get; set; } = false;
        public bool HasSearched { get; set; } = false;
        public bool ArePagesVisible { get; set; } = false;
        public string SearchText { get; set; }

        public List<User> SearchResults { get; set; } = new();

        public HashSet<User> SelectedResults { get; set; } = new();

        public Dictionary<string,User> Reviewers { get; set; } = new();

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
    }
}
