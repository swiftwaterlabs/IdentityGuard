namespace IdentityGuard.Shared.Models
{
    public class AccessReviewActionRequest
    {
        public string Action { get; set; }
        public string ActionObjectType { get; set; }
        public string ActionObjectId { get; set; }

    }
}