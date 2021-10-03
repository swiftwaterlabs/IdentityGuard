namespace IdentityGuard.Shared.Models.Requests
{
    public class ObjectDeleteRequest
    {
        public string DirectoryId { get; set; }
        public string ObjectId { get; set; }
        public string ObjectType { get; set; }
    }
}
