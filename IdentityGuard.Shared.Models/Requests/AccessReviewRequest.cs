using System.Collections.Generic;

namespace IdentityGuard.Shared.Models.Requests
{
    public class AccessReviewRequest
    {
        public string Id { get; set; }
        public string ObjectType { get; set; }
        public string ObjectId { get; set; }
        public string DirectoryId { get; set; }
        public List<DirectoryObject> AssignedTo { get; set; }

    }
}