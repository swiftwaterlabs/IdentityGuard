namespace IdentityGuard.Shared.Models
{

    public static class AccessReviewActionTypes
    {
        public const string Remove = "Remove";
    }

    public static class AccessReviewActionObjectTypes
    {
        public const string Owner = "Owner";
        public const string Owned = "Owned";
        public const string GroupMembers = "GroupMembers";
        public const string GroupMembership = "GroupMembership";
        public const string ApplicationRoleMembership = "ApplicationRoleMembership";
        public const string ApplicationSecret = "ApplicationSecret";
        public const string ApplicationPermission = "ApplicationPermission";
        public const string Role = "Role";
    }
}