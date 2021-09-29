using IdentityGuard.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IdentityGuard.Core.Models.Data
{
    public class RequestData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Area { get; set; }
        public string ObjectType { get; set; }
        public string ObjectId { get; set; }
        public string DirectoryId { get; set; }
        public string Action { get; set; }
        public DirectoryObject RequestedBy { get; set; }
        public DateTime RequestedAt { get; set; }
        public DirectoryObject CompletedBy { get; set; }
        public DateTime? CompletedAt { get; set; }
        public RequestStatus Status { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }
    }
}
