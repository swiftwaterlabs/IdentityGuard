using IdentityGuard.Blazor.Ui.Components.AccessReviews;
using IdentityGuard.Shared.Models;
using IdentityGuard.Shared.Models.Requests;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityGuard.Blazor.Ui.Services
{
    public interface IAccessReviewService
    {
        Task<List<AccessReview>> GetPending();
        Task<List<AccessReview>> GetComplete();
        Task<AccessReview> Get(string id);
        Task<AccessReview> Request(IdentityGuard.Shared.Models.Requests.AccessReviewRequest request);
        Task<AccessReview> Complete(string id);
        Task<AccessReview> Abandon(string id);
        Task<AccessReview> ApplyChanges(string id, IEnumerable<AccessReviewActionRequest> actions);
        
    }

    public class AccessReviewService: AbstractHttpService, IAccessReviewService
    {
        public AccessReviewService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public Task<AccessReview> Get(string id) => Get<AccessReview>($"api/accessreview/{id}");
        public Task<AccessReview> Request(IdentityGuard.Shared.Models.Requests.AccessReviewRequest request) => Post<AccessReview, IdentityGuard.Shared.Models.Requests.AccessReviewRequest>($"api/accessreview", request);
        public Task<List<AccessReview>> GetPending() => Get<List<AccessReview>>("api/accessreview/search/pending");
        public Task<List<AccessReview>> GetComplete() => Get<List<AccessReview>>("api/accessreview/search/complete");
        public Task<AccessReview> Complete(string id) => Post<AccessReview,string>($"api/accessreview/{id}/complete", id);
        public Task<AccessReview> Abandon(string id) => Post<AccessReview,string>($"api/accessreview/{id}/abandon", id);
        public Task<AccessReview> ApplyChanges(string id, IEnumerable<AccessReviewActionRequest> actions) => Post<AccessReview, IEnumerable<AccessReviewActionRequest>>($"api/accessreview/{id}/actions", actions);

    }
}