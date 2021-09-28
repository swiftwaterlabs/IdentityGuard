using IdentityGuard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityGuard.Core.Extensions
{
    public static class GraphObjectExtensions
    {
        public static string GetPortalUrl(this Microsoft.Graph.Application data, Directory directory)
        {
            return $"{directory.PortalUrl.Trim().TrimEnd('/')}/#@{directory.Domain}/#blade/Microsoft_AAD_RegisteredApps/ApplicationMenuBlade/Overview/appId/{data.AppId}";
        }

        public static string GetPortalUrl(this Microsoft.Graph.Group data, Directory directory)
        {
            return $"{directory.PortalUrl.Trim().TrimEnd('/')}/#@{directory.Domain}/#blade/Microsoft_AAD_IAM/GroupDetailsMenuBlade/Overview/groupId/{data.Id}";
        }

        public static string GetPortalUrl(this Microsoft.Graph.User data, Directory directory)
        {
            return $"{directory.PortalUrl.Trim().TrimEnd('/')}/#@{directory.Domain}/#blade/Microsoft_AAD_IAM/UserDetailsMenuBlade/Profile/userId/{data.Id}";

        }
    }
}
