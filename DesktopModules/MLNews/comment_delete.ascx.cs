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

using DotNetNuke.Services.Localization;

namespace DotNetNuke.News
{
    public partial class comment_delete : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                NewsController db = new NewsController();
                if (Request.QueryString["id"] != null)
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    if (Request.QueryString["action"] != null)
                    {
                        string action = Request.QueryString["action"];
                        if (action == "delete")
                        {
                            db.DeleteComment(id);
                        }
                        else if (action == "changestatus")
                        {
                            db.UpdateCommentStatus(id);
                        }
                        string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "admin_comment", "mid/" + this.ModuleId.ToString(), "pg/" + Request.QueryString["pg"], "catid/" + Request.QueryString["catid"], "newsid/" + Request.QueryString["newsid"], "status/" + Request.QueryString["status"]);
                        Response.Redirect(url);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}