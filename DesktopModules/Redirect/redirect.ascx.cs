using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Globalization;

namespace DotNetNuke.Modules
{
    public partial class redirect : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DotNetNuke.Entities.Tabs.TabController tabCont = new DotNetNuke.Entities.Tabs.TabController();
                DotNetNuke.Entities.Tabs.TabInfo tab = tabCont.GetTabByName(UserInfo.Profile.IM, PortalId);
                if(tab != null && tab.TabID != null) Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tab.TabID));
            }
            catch (Exception ex)
            {
            }
        }
    }
}