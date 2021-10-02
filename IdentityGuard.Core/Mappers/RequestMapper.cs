using IdentityGuard.Core.Configuration;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdentityGuard.Core.Mappers
{
    public class RequestMapper
    {
        public Models.Data.RequestData Map(Shared.Models.Request toMap)
        {
            if (toMap == null) return null;
            return new Models.Data.RequestData
            {
                Id = toMap.Id,
                Action = toMap.Action,
                Area = CosmosConfiguration.DefaultPartitionKey,
                DirectoryId = toMap.DirectoryId,
                ObjectId = toMap.ObjectId,
                ObjectType = toMap.ObjectType,
                RequestedAt = toMap.RequestedAt,
                RequestedBy = toMap.RequestedBy,
                Status = toMap.Status,
                AdditionalData = toMap.AdditionalData
            };
        }

        public Shared.Models.Request Map(Models.Data.RequestData toMap)
        {
            if (toMap == null) return null;
            return new Shared.Models.Request
            {
                Id = toMap.Id,
                Action = toMap.Action,
                DirectoryId = toMap.DirectoryId,
                ObjectId = toMap.ObjectId,
                ObjectType = toMap.ObjectType,
                RequestedAt = toMap.RequestedAt,
                RequestedBy = toMap.RequestedBy,
                CompletedBy = toMap.CompletedBy,
                CompletedAt = toMap.CompletedAt,
                Status = toMap.Status,
                AdditionalData = toMap.AdditionalData
            };
        }

        public Shared.Models.Request Map(Shared.Models.AccessReviewRequest toMap, 
            RequestStatus status, 
            DirectoryObject requestedBy)
        {
            if (toMap == null) return null;

            return new Shared.Models.Request
            {
                Id = toMap.Id,
                Action = RequestType.AccessReview,
                DirectoryId = toMap.DirectoryId,
                ObjectId = toMap.ObjectId,
                ObjectType = toMap.ObjectType,
                RequestedAt = ClockService.Now,
                RequestedBy = requestedBy,
                CompletedBy = null,
                CompletedAt = null,
                Status = status,
            };
        }

        public Shared.Models.Request Map(AccessReview accessReview, 
            IEnumerable<AccessReviewActionRequest> toMap, 
            RequestStatus status, 
            DirectoryObject requestedBy)
        {
            if (accessReview == null) return null;
            if (toMap == null) return null;
            if (!toMap.Any()) return null;

            return new Shared.Models.Request
            {
                Id = accessReview.Id,
                Action = RequestType.AccessReviewAction,
                DirectoryId = accessReview.DirectoryId,
                ObjectId = accessReview.ObjectId,
                ObjectType = accessReview.ObjectType,
                RequestedAt = ClockService.Now,
                RequestedBy = requestedBy,
                CompletedBy = null,
                CompletedAt = null,
                Status = status,
                AdditionalData = new Dictionary<string, object>
                {
                    {"RequestedActions",toMap},
                    {"AccessReviewId",accessReview.Id},
                }
            };
        }
    }
}
