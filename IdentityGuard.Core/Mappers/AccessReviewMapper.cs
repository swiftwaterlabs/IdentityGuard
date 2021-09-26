﻿

namespace IdentityGuard.Core.Mappers
{
    public class AccessReviewMapper
    {
        public Shared.Models.AccessReview Map(Models.Data.AccessReviewData toMap)
        {
            if (toMap == null) return null;

            return new Shared.Models.AccessReview
            {
                Id = toMap.Id,
                ObjectType = toMap.ObjectType,
                ObjectId = toMap.ObjectId,
                DirectoryId = toMap.DirectoryId,
                DisplayName = toMap.DisplayName,
                CreatedBy = toMap.CreatedBy,
                CreatedAt = toMap.CreatedAt,
                AssignedTo = toMap.AssignedTo,
                CompletedBy = toMap.CompletedBy,
                CompletedAt = toMap.CompletedAt,
                Status = toMap.Status
            };
        }

        public Models.Data.AccessReviewData Map(Shared.Models.AccessReview toMap)
        {
            if (toMap == null) return null;

            return new Models.Data.AccessReviewData
            {
                Id = toMap.Id,
                ObjectType = toMap.ObjectType,
                ObjectId = toMap.ObjectId,
                DirectoryId = toMap.DirectoryId,
                DisplayName = toMap.DisplayName,
                CreatedBy = toMap.CreatedBy,
                CreatedAt = toMap.CreatedAt,
                AssignedTo = toMap.AssignedTo,
                CompletedBy = toMap.CompletedBy,
                CompletedAt = toMap.CompletedAt,
                Status = toMap.Status
            };
        }
    }
}