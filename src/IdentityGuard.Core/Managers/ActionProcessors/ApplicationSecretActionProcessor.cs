using IdentityGuard.Core.Services;
using IdentityGuard.Shared.Models;
using IdentityGuard.Shared.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Managers.ActionProcessors
{
    public class ApplicationSecretActionProcessor : IActionProcessor
    {
        private readonly ApplicationService _applicationService;

        public ApplicationSecretActionProcessor(ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        public string ActionObjectType => AccessReviewActionObjectTypes.ApplicationSecret;

        public async Task<IEnumerable<AccessReviewAction>> ProcessActions(Directory directory, AccessReview accessReview, IEnumerable<AccessReviewActionRequest> actions, DirectoryObject requestingUser)
        {
            if (!actions.Any()) return new List<AccessReviewAction>();

            var secretIds = actions.Select(a => a.ActionObjectId).ToList();

            var application = await _applicationService.Get(directory, accessReview.ObjectId);

            var secretsToRemove = application.Secrets
                .Where(s => secretIds.Contains(s.Id));

            await RemovePasswordSecrets(directory, application, secretsToRemove);
            await RemoveCertificateSecrets(directory, application, secretsToRemove);

            return secretsToRemove
                .Select(secret => new AccessReviewAction
                {
                    Id = Guid.NewGuid().ToString(),
                    Action = AccessReviewActionTypes.Remove,
                    Relation = AccessReviewActionObjectTypes.ApplicationSecret,
                    ParentObjectId = accessReview.ObjectId,
                    ParentObjectType = accessReview.ObjectType,
                    ParentObjectDisplayName = accessReview.DisplayName,
                    ActionObjectId = secret.Id,
                    ActionObjectDisplayName = secret.DisplayName,
                    ActionObjectType = secret.Type,
                    RequestedAt = ClockService.Now,
                    RequestedBy = requestingUser,
                    Status = AccessReviewActionStatus.Complete
                });
        }

        private async Task RemovePasswordSecrets(Directory directory, Application application, IEnumerable<ApplicationSecret> secretsToRemove)
        {
            var passwordSecrets = secretsToRemove.Where(s => s.Type == ApplicationSecretType.Password);
            var passwordSecretIds = passwordSecrets.Select(p => p.Id).ToList();
            if (passwordSecretIds.Any())
            {
                await _applicationService.RemovePasswordSecrets(directory, application.Id, passwordSecretIds);
            }
        }

        private async Task RemoveCertificateSecrets(Directory directory, Application application, IEnumerable<ApplicationSecret> secretsToRemove)
        {
            var certificateSecrets = secretsToRemove.Where(s => s.Type == ApplicationSecretType.Certificate);
            var certificiateSecretIds = certificateSecrets.Select(p => p.Id).ToList();
            if (certificiateSecretIds.Any())
            {
                await _applicationService.RemoveCertificateSecrets(directory, application.Id, certificiateSecretIds);

            }
        }


    }
}
