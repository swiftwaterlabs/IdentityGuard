using System;

namespace IdentityGuard.Shared.Models
{
    public class User
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }

        public string DirectoryId { get; set; }
        public string DirectoryName { get; set; }

        public string GivenName { get; set; }
        public string SurName { get; set; }
        public string EmailAddress { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreationType { get; set; }
        public string Status { get; set; }
    }
}
