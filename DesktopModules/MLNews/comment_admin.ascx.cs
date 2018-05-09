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
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DotNetNuke.News
{
    public partial class comment_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        int pg = 0;
        private void BindCatDDL()
        {
            CategoryController catdb = new CategoryController();
            DataTable dt = catdb.LoadTree(false, PortalId, "");
            DataRow r = dt.NewRow();
            r["CatName"] = "";
            r["CatID"] = "0";
            dt.Rows.InsertAt(r,0);
            ddlCat.DataSource = dt;
            ddlCat.DataTextField = "CatName";
            ddlCat.DataValueField = "CatID";
            ddlCat.DataBind();
        }

        private void BindNewsDDL()
        {
            NewsController newsCont = new NewsController();
            DataTable tblNews = newsCont.GetNewsByCat_All(ddlCat.SelectedValue, PortalId);
            DataRow r = tblNews.NewRow();
            r["ID"] = 0;
            r["Headline"] = "";
            tblNews.Rows.InsertAt(r, 0);
            ddlNews.DataSource = tblNews;
            ddlNews.DataTextField = "Headline";
            ddlNews.DataValueField = "ID";
            ddlNews.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    lnkBack.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "mid/" + this.ModuleId.ToString());
                    if (Request.QueryString["Status"] != null)
                    {
                        ddlStatus.SelectedValue = Request.QueryString["Status"];
                    }
                    if (Request.QueryString["pg"] != null)
                    {
                        pg = Convert.ToInt32(Request.QueryString["pg"]) - 1;
                    }
                    BindCatDDL();
                    if (ddlCat.Items.Count > 0)
                    {
                        if (Request.QueryString["catid"] != null)
                        {
                            ddlCat.SelectedValue = Request.QueryString["catid"];
                        }
                        BindNewsDDL();
                        if (Request.QueryString["newsid"] != null)
                        {
                            ddlNews.SelectedValue = Request.QueryString["newsid"];
                        }
                        BindComment();
                    }
                }
                else
                {
                    pg = 0;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void BindComment()
        {
            paging.Visible = false;
            int pagesize = 10;
            if (Settings["pageSizeAdmin"] != null)
            {
                pagesize = int.Parse(Settings["pageSizeAdmin"].ToString());
            }
            if (ddlNews.Items.Count > 0)
            {   
                NewsController engine = new NewsController();
                int newpg = pg + 1;
                string deleteFormatUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "delete_comment", "mid/" + this.ModuleId.ToString(), "id/@@id", "action/@@action", "pg/" + newpg.ToString(), "catid/" + ddlCat.SelectedValue, "newsid/" + ddlNews.SelectedValue, "status/" + ddlStatus.SelectedValue);
                int NumRecord = 0;
                int pages = 0;
                XmlDocument doc = engine.LoadPageCommentXML_All(deleteFormatUrl, int.Parse(ddlNews.SelectedValue), ddlCat.SelectedValue, PortalId, int.Parse(ddlStatus.SelectedValue), pg, pagesize, out NumRecord);
                pages = NumRecord / pagesize;
                if (pagesize * pages < NumRecord) pages++;
                if (pg >= pages && pages > 0)
                {
                    deleteFormatUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "delete_comment", "mid/" + this.ModuleId.ToString(), "id/@@id", "action/@@action", "pg/" + pg.ToString(), "catid/" + ddlCat.SelectedValue, "newsid/" + ddlNews.SelectedValue, "status/" + ddlStatus.SelectedValue);
                    doc = engine.LoadPageCommentXML_All(deleteFormatUrl, int.Parse(ddlNews.SelectedValue), ddlCat.SelectedValue, PortalId, int.Parse(ddlStatus.SelectedValue), pg-1, pagesize, out NumRecord);
                    pg--;
                }
                if (pages > 1)
                {
                    string format = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "admin_comment", "mid/" + this.ModuleId.ToString(), "pg/@@page", "catid/" + ddlCat.SelectedValue, "newsid/" + ddlNews.SelectedValue, "status/" + ddlStatus.SelectedValue);
                    paging.Visible = true;
                    paging.showing(pages, pg, format);
                }
                string template = "DesktopModules/MLNews/Xsl/comment_admin.xsl";
                DotNetNuke.NewsProvider.Utils.XMLTransform(xmlTransformer, template, doc);
            }
        }

        protected void ddlCat_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            BindNewsDDL();
            ddlNews_SelectedIndexChanged(null, null);
        }

        protected void ddlNews_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            BindComment();
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            BindComment();
        }
    }
}