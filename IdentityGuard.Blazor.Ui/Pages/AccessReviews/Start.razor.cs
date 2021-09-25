﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityGuard.Blazor.Ui.Models;
using IdentityGuard.Blazor.Ui.Services;
using IdentityGuard.Shared.Models;
using Microsoft.AspNetCore.Components;
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
        public NavigationManager NavigationManager { get; set; }

        public bool IsLoading { get; set; } = false;

        public Dictionary<string, string> ObjectTypes { get; set; } = new ();
        public string SelectedObjectType { get; set; }

        public string SearchText { get; set; }

        public List<DirectoryObject> SearchResults { get; set; } = new ();

        protected override Task OnInitializedAsync()
        {
            AppState.SetBreadcrumbs(
                new BreadcrumbItem("Access Reviews", Paths.AccessReviews),
                new BreadcrumbItem("Start", Paths.NewAccessReviews)
            );

            ObjectTypes.Add("User", "User");
            ObjectTypes.Add("Application","Application");

            SelectedObjectType = ObjectTypes.Keys.First();

            return base.OnParametersSetAsync();
        }

        public async Task Search()
        {
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
                    SearchResults = results.Select(Map).ToList();
                    IsLoading = false;
                    return;
                }
                case "User":
                {
                    IsLoading = true;
                    var results = await UserService.Search(new[] { SearchText });
                    SearchResults = results.Select(Map).ToList();
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
                Type = "Application"
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
                Type = "User"
            };
        }

        public void StartReview(DirectoryObject item)
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
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(item), item.Type);
                }

            }
        }
    }
}
