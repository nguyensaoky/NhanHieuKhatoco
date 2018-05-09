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

namespace DotNetNuke.News
{
    public partial class cat_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (!RoleProvider.IsAdminRole(PortalSettings.AdministratorRoleName))
                //{
                //    lblRestricted.Visible = true;
                //    return;
                //}

                if (!Page.IsPostBack)
                {
                    lnkBack.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "mid/" + this.ModuleId.ToString());
                    lnkAdd.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit_cat", "mid/" + this.ModuleId.ToString());
                    CategoryController engine = new CategoryController();
                    string editFormatUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit_cat", "mid/" + this.ModuleId.ToString(), "id/@@catid");
                    XmlDocument doc = engine.LoadTreeXML(editFormatUrl, false, PortalId, 0, "", "");
                    string template = "DesktopModules/MLNews/Xsl/cat_admin.xsl";
                    DotNetNuke.NewsProvider.Utils.XMLTransform(xmlTransformer, template, doc);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}