using IdentityGuard.Core.Models.Data;
using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using IdentityGuard.Tests.Shared.Extensions;
using IdentityGuard.Worker.Functions;
using IdentityGuard.Worker.Tests.TestUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IdentityGuard.Worker.Tests.Functions
{
    public class AccessReviewFunctionTests
    {
        [Fact]
        public async Task Request_ValidInputs_CreateReview()
        {
            // Given
            var builder = new TestBuilder();
            var directory = builder.WithDirectory("my-directory");
            var application = builder.WithApplication(directory.Id, directory.Name, "my-app");

            var function = builder.Get<AccessReviewFunctions>();

            var request = new AccessReviewRequest
            {
                ObjectId = application.Id,
                ObjectType = ObjectTypes.Application,
                DirectoryId = directory.Id,
                AssignedTo = new List<DirectoryObject>
                {
                    new DirectoryObject
                    {
                        DirectoryId = directory.Id,
                        Id = Guid.NewGuid().ToString()
                    }
                }
            };
            var message = JsonConvert.SerializeObject(request);

            // When
            await function.Request(message, builder.Context());

            // Then
            Assert.Single(builder.Context.Data.AccessReviews);
            var actualReview = builder.Context.Data.AccessReviews.Values.First();
            AssertAccessReviewData(request, actualReview, directory,application);

            Assert.Single(builder.Context.Data.Requests);
            var actualRequest = builder.Context.Data.Requests.Values.First();
            AssertRequestData(request, actualRequest, directory);
        }

        private void AssertAccessReviewData(AccessReviewRequest request, 
            AccessReviewData actual,
            DirectoryData directory,
            Microsoft.Graph.Application toReview)
        {
            Assert.Equal(request.ObjectId, actual.ObjectId);
            Assert.Equal(request.ObjectType, actual.ObjectType);
            Assert.Equal(toReview.DisplayName, actual.DisplayName);
            Assert.Equal(request.DirectoryId, actual.DirectoryId);
            Assert.Equal(directory.Domain, actual.DirectoryName);
            Assert.Equal(directory.CanManageObjects, actual.CanManageObjects);
            Assert.Equal(AccessReviewStatus.New, actual.Status);

            Assert.NotNull(actual.Id);
            Assert.Empty(actual.Actions);

            Assert.Null(actual.CompletedAt);
            Assert.Null(actual.CompletedBy);

            Assert.NotNull(actual.CreatedBy);
            Assert.Equal(actual.CreatedAt, ClockService.Now);

            Assert.Single(actual.AssignedTo);
            Assert.Equal(request.AssignedTo[0].DirectoryId, actual.AssignedTo[0].DirectoryId);
            Assert.Equal(request.AssignedTo[0].Id, actual.AssignedTo[0].Id);

        }

        private void AssertRequestData(AccessReviewRequest request, 
            RequestData actual,
            DirectoryData directory)
        {
            Assert.Equal(request.ObjectId, actual.ObjectId);
            Assert.Equal(request.ObjectType, actual.ObjectType);
            Assert.Equal(request.DirectoryId, actual.DirectoryId);
            Assert.Equal(RequestType.AccessReview, actual.Action);
            Assert.Equal(RequestStatus.Complete, actual.Status);

            Assert.Null(actual.CompletedAt);
            Assert.Null(actual.CompletedBy);

            Assert.NotNull(actual.RequestedBy);
            Assert.Equal(actual.RequestedAt,ClockService.Now);
        }
    }
}
