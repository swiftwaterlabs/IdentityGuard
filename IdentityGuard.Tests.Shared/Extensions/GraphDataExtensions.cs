using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityGuard.Tests.Shared.Extensions
{
    public static class GraphDataExtensions
    {
        public static Application WithApplication(this TestBuilderBase root,
            string directoryId,
            string directoryName,
            string name,
            string id = null,
            string clientId = null,
            List<string> owners = null)
        {
            var context = root.Context.GraphApi;

            if (!context.Applications.ContainsKey(directoryId))
            {
                context.Applications.Add(directoryId, new List<Application>());
            }

            var data = new Application
            {
                Id = id ?? Guid.NewGuid().ToString(),
                AppId = clientId ?? Guid.NewGuid().ToString(),
                DisplayName = name,
                PublisherDomain = directoryName,
                Owners = new ApplicationOwnersCollectionWithReferencesPage()
            };

            if (owners != null)
            {
                var ownerObjects = owners
                    .Select(o => new DirectoryObject { Id = o })
                    .ToList();
                ownerObjects.ForEach(o => data.Owners.Add(o));



            }

            context.Applications[directoryId].Add(data);

            return data;
        }
    }
}
