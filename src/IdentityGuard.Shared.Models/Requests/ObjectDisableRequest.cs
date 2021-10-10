using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityGuard.Shared.Models.Requests
{
    public class ObjectDisableRequest
    {
        public string DirectoryId { get; set; }
        public string ObjectId { get; set; }
        public string ObjectType { get; set; }
    }
}
