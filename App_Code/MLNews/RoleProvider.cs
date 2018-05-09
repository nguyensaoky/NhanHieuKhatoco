using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace DotNetNuke.News
{
    public class RoleProvider
    {
        public static bool IsAdminRole(string adminrole)
        {
            if (DotNetNuke.Security.PortalSecurity.IsInRoles(adminrole))
                return true;
            return false;
        }

    }
}
