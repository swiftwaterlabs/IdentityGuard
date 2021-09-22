namespace IdentityGuard.Core.Mappers
{
    public class UserMapper
    {
        public Shared.Models.User Map(Shared.Models.Directory directory, Microsoft.Graph.User toMap)
        {
            if (toMap == null) return null;

            return new Shared.Models.User
            {
                Id = toMap.Id,

                DirectoryId = directory.Id,
                DirectoryName = directory.Domain,
                
                DisplayName = toMap.DisplayName,
                GivenName = toMap.GivenName,
                SurName = toMap.Surname,
                EmailAddress = toMap.Mail,
                Type = toMap.UserType,
                CreatedAt = toMap.CreatedDateTime.GetValueOrDefault().DateTime,
                CreationType = toMap.CreationType,
                Status = toMap.State
            };
        }
    }
}
