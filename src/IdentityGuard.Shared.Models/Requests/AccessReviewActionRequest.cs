namespace IdentityGuard.Shared.Models.Requests
{
    public class AccessReviewActionRequest
    {
        public string Action { get; set; }
        public string ActionObjectType { get; set; }
        public string ActionObjectSubType { get; set; }
        public string ActionObjectId { get; set; }

    }
}