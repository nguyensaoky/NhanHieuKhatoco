using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace DotNetNuke.News
{
    public partial class group_menu : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    NewsGroupController engine = new NewsGroupController();
                    menu.MenuName = "BCM_" + ModuleId.ToString();
                    int menuwidth = Convert.ToInt32(Settings["menuwidth"]);
                    if (menuwidth > 0)
                    {
                        menu.MenuWidth = Convert.ToInt32(Settings["menuwidth"]);
                    }
                    bool ExpandAll = false;
                    if (Settings["ExpandAll"] != null)
                    {
                        ExpandAll = Convert.ToBoolean(Settings["ExpandAll"]);
                    }
                    string CSSPrefix = "";
                    if (Settings["CSSPrefix"] != null)
                    {
                        CSSPrefix = Settings["CSSPrefix"].ToString();
                    }
                    menu.ExpandAll = ExpandAll;
                    menu.CSSPrefix = CSSPrefix;
                    menu.DataSource = engine.LoadTreeForMenu(PortalId, Convert.ToInt32(Settings["source"]), Convert.ToString(Settings["code"]), System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}