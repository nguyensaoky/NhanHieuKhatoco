using System;
using System.Data;
using System.Data.SqlClient;
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
    public partial class news_list_querystring : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private int CountPage(int countrecord, int pagesize)
        {
            int pg = countrecord / pagesize;
            if (pagesize * pg < countrecord) pg++;
            return pg;
        }

        private int CountPageByCat(string catid, int pagesize)
        {
            if (System.Threading.Thread.CurrentThread.CurrentCulture.ToString() != Common.Globals.GetPortalSettings().DefaultLanguage)
            {
                string strSQL = "News_countNewsByCat_Language";
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@CatID", catid);
                p[1] = new SqlParameter("@Locale", System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                object o = DotNetNuke.NewsProvider.DataProvider.ExecuteScalarSP(strSQL, p);
                if (o != null)
                {
                    int countrecord = Convert.ToInt32(o);
                    int pg = countrecord / pagesize;
                    if (pagesize * pg < countrecord) pg++;
                    return pg;
                }
                return 0;
            }
            else
            {
                string strSQL = "News_countNewsByCat";
                SqlParameter p = new SqlParameter("@CatID", catid);
                object o = DotNetNuke.NewsProvider.DataProvider.ExecuteScalarSP(strSQL, p);
                if (o != null)
                {
                    int countrecord = Convert.ToInt32(o);
                    int pg = countrecord / pagesize;
                    if (pagesize * pg < countrecord) pg++;
                    return pg;
                }
                return 0;
            }
        }

        private int CountPageByGroup(string newsgroupid, int pagesize)
        {
            if (System.Threading.Thread.CurrentThread.CurrentCulture.ToString() != Common.Globals.GetPortalSettings().DefaultLanguage)
            {
                string strSQL = "News_countNewsByGroup_Language";
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@NewsGroupID", newsgroupid);
                p[1] = new SqlParameter("@Locale", System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                object o = DotNetNuke.NewsProvider.DataProvider.ExecuteScalarSP(strSQL, p);
                if (o != null)
                {
                    int countrecord = Convert.ToInt32(o);
                    int pg = countrecord / pagesize;
                    if (pagesize * pg < countrecord) pg++;
                    return pg;
                }
                return 0;
            }
            else
            {
                string strSQL = "News_countNewsByGroup";
                SqlParameter p = new SqlParameter("@NewsGroupID", newsgroupid);
                object o = DotNetNuke.NewsProvider.DataProvider.ExecuteScalarSP(strSQL, p);
                if (o != null)
                {
                    int countrecord = Convert.ToInt32(o);
                    int pg = countrecord / pagesize;
                    if (pagesize * pg < countrecord) pg++;
                    return pg;
                }
                return 0;
            }
        }

        private XmlDocument LoadData()
        {
            try
            {
                int pg = 0;
                int pagesize = Convert.ToInt32(Settings["limits"]);
                if (pagesize == 0) pagesize = 4;

                string source = "0";
                if (Request.QueryString["source"] != null)
                    source = Request.QueryString["source"];
                else if (Settings["source"] != null)
                    source = Settings["source"].ToString();
                int pageNum;
                int numRecord;
                string newsgroupid = "";
                string categoryid = "";
                string newsgroupcode = "";
                string categorycode = "";

                XmlDocument doc = null;
                NewsController engine = new NewsController();
                bool displayPaging = true;
                if (Settings["displayPaging"] != null)
                {
                    displayPaging = bool.Parse(Settings["displayPaging"].ToString());
                }
                bool displayTitle = true;
                if (Settings["displayTitle"] != null)
                {
                    displayTitle = bool.Parse(Settings["displayTitle"].ToString());
                }
                string titleCSSClass = "";
                if (Settings["titleCSSClass"] != null)
                {
                    titleCSSClass = Settings["titleCSSClass"].ToString();
                }
                int numShortNews = 0;
                if (Settings["numShortNews"] != null)
                {
                    numShortNews = int.Parse(Settings["numShortNews"].ToString());
                }
                int imageWidth = 100;
                if (Settings["imageWidth"] != null)
                {
                    imageWidth = int.Parse(Settings["imageWidth"].ToString());
                }
                MLCategoryInfo category;
                switch (source)
                {
                    case "0": // theo cat từ URL
                        if (Request.QueryString["categoryid"] != null)
                        {
                            if (Request.QueryString["pg"] != null)
                                pg = Convert.ToInt32(Request.QueryString["pg"]) - 1;
                            categoryid = Request.QueryString["categoryid"];

                            if (displayTitle)
                            {
                                category = MLCategoryController.GetCategory(categoryid, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), false);   
                                if (category.MLCatName == null)
                                {
                                    //ltTitle.Text = @"<div class=""" + titleCSSClass + @"_outer"">" + @"<div class=""" + titleCSSClass + @"_inner""><a href=""" + DotNetNuke.Common.Globals.NavigateURL(category.DesktopListID, "", "categoryid/" + categoryid) + @""">" + category.CatName + @"</a></div></div>";
                                    ltTitle.Text = @"<div class=""" + titleCSSClass + @"""><a href=""" + DotNetNuke.Common.Globals.NavigateURL(category.DesktopListID, "", "categoryid/" + categoryid) + @""">" + category.CatName + @"</a></div>";
                                }
                                else
                                {
                                    //ltTitle.Text = @"<div class=""" + titleCSSClass + @"_outer"">" + @"<div class=""" + titleCSSClass + @"_inner""><a href=""" + DotNetNuke.Common.Globals.NavigateURL(category.DesktopListID, "", "categoryid/" + categoryid) + @""">" + category.MLCatName.StringTextOrFallBack + @"</a></div></div>";
                                    ltTitle.Text = @"<div class=""" + titleCSSClass + @"""><a href=""" + DotNetNuke.Common.Globals.NavigateURL(category.DesktopListID, "", "categoryid/" + categoryid) + @""">" + category.MLCatName.StringTextOrFallBack + @"</a></div>";
                                }
                            }

                            doc = engine.LoadXML(categoryid, pg, pagesize, imageWidth, numShortNews);
                            int pages = CountPageByCat(categoryid, pagesize);
                            if (pages > 1)
                            {
                                string format = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "categoryid/" + categoryid, "pg/@@page");
                                paging.Visible = true;
                                paging.showing(pages, pg, format);
                            }
                            if (!displayPaging)
                            {
                                paging.Visible = false;
                            }
                        }
                        else
                            doc = new XmlDocument();
                        break;
                    case "1": // theo newsgroup từ URL
                        if (Request.QueryString["newsgroupid"] != null)
                        {
                            if (Request.QueryString["pg"] != null)
                                pg = Convert.ToInt32(Request.QueryString["pg"]) - 1;
                            newsgroupid = Request.QueryString["newsgroupid"];

                            doc = engine.LoadXMLByNewsGroup(newsgroupid, pg, pagesize, imageWidth, numShortNews);
                            int pages = CountPageByGroup(newsgroupid, pagesize);
                            if (pages > 1)
                            {
                                string format = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "newsgroupid/" + newsgroupid, "pg/@@page");
                                paging.Visible = true;
                                paging.showing(pages, pg, format);
                            }
                            if (!displayPaging)
                            {
                                paging.Visible = false;
                            }
                        }
                        else
                            doc = new XmlDocument();
                        break;
                    case "2": // theo cat được chọn
                        categoryid = this.Settings["categoryid"].ToString();
                        if (Request.QueryString["categoryid"] != null)
                        {
                            if (categoryid == Request.QueryString["categoryid"])
                            {
                                if (Request.QueryString["pg"] != null)
                                {
                                    pg = Convert.ToInt32(Request.QueryString["pg"]) - 1;
                                }
                            }
                            else if (Session["currentPage_" + PortalSettings.ActiveTab.TabID.ToString() + "_cat_" + categoryid] != null)
                            {
                                pg = (int)Session["currentPage_" + PortalSettings.ActiveTab.TabID.ToString() + "_cat_" + categoryid];
                            }
                        }
                        if (displayTitle)
                        {
                            category = MLCategoryController.GetCategory(categoryid, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), false);
                            if (category.MLCatName == null)
                            {
                                //ltTitle.Text = @"<div class=""" + titleCSSClass + @"_outer"">" + @"<div class=""" + titleCSSClass + @"_inner""><a href=""" + DotNetNuke.Common.Globals.NavigateURL(category.DesktopListID, "", "categoryid/" + categoryid) + @""">" + category.CatName + @"</a></div></div>";
                                ltTitle.Text = @"<div class=""" + titleCSSClass + @"""><a href=""" + DotNetNuke.Common.Globals.NavigateURL(category.DesktopListID, "", "categoryid/" + categoryid) + @""">" + category.CatName + @"</a></div>";
                            }
                            else
                            {
                                //ltTitle.Text = @"<div class=""" + titleCSSClass + @"_outer"">" + @"<div class=""" + titleCSSClass + @"_inner""><a href=""" + DotNetNuke.Common.Globals.NavigateURL(category.DesktopListID, "", "categoryid/" + categoryid) + @""">" + category.MLCatName.StringTextOrFallBack + @"</a></div></div>";
                                ltTitle.Text = @"<div class=""" + titleCSSClass + @"""><a href=""" + DotNetNuke.Common.Globals.NavigateURL(category.DesktopListID, "", "categoryid/" + categoryid) + @""">" + category.MLCatName.StringTextOrFallBack + @"</a></div>";
                            }
                        }
                        doc = engine.LoadXML(categoryid, pg, pagesize, imageWidth, numShortNews);
                        pageNum = CountPageByCat(categoryid, pagesize);
                        if (pageNum > 1)
                        {
                            string format = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "categoryid/" + categoryid, "pg/@@page");
                            paging.Visible = true;
                            paging.showing(pageNum, pg, format);
                            Session["currentPage_" + PortalSettings.ActiveTab.TabID.ToString() + "_cat_" + categoryid] = pg;
                        }
                        if (!displayPaging)
                        {
                            paging.Visible = false;
                        }
                        break;
                    case "3": // theo newsgroup được chọn
                        newsgroupid = this.Settings["newsgroupid"].ToString();
                        if (Request.QueryString["newsgroupid"] != null)
                        {
                            if (newsgroupid == Request.QueryString["newsgroupid"])
                            {
                                if (Request.QueryString["pg"] != null)
                                {
                                    pg = Convert.ToInt32(Request.QueryString["pg"]) - 1;
                                }
                            }
                            else if (Session["currentPage_" + PortalSettings.ActiveTab.TabID.ToString() + "_group_" + newsgroupid] != null)
                            {
                                pg = (int)Session["currentPage_" + PortalSettings.ActiveTab.TabID.ToString() + "_group_" + newsgroupid];
                            }
                        }
                        doc = engine.LoadXMLByNewsGroup(newsgroupid, pg, pagesize, imageWidth, numShortNews);
                        pageNum = CountPageByGroup(newsgroupid, pagesize);
                        if (pageNum > 1)
                        {
                            string format = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "newsgroupid/" + newsgroupid, "pg/@@page");
                            paging.Visible = true;
                            paging.showing(pageNum, pg, format);
                            Session["currentPage_" + PortalSettings.ActiveTab.TabID.ToString() + "_group_" + newsgroupid] = pg;
                        }
                        if (!displayPaging)
                        {
                            paging.Visible = false;
                        }
                        break;
                    case "4": // theo cat code
                        categorycode = this.Settings["categorycode"].ToString();
                        if (Request.QueryString["categorycode"] != null)
                        {
                            if (categorycode == Request.QueryString["categorycode"])
                            {
                                if (Request.QueryString["pg"] != null)
                                {
                                    pg = Convert.ToInt32(Request.QueryString["pg"]) - 1;
                                }
                            }
                            else if (Session["currentPage_" + PortalSettings.ActiveTab.TabID.ToString() + "_catcode_" + categorycode] != null)
                            {
                                pg = (int)Session["currentPage_" + PortalSettings.ActiveTab.TabID.ToString() + "_catcode_" + categorycode];
                            }
                        }
                        numRecord = 0;
                        doc = engine.LoadXMLByCatCode(PortalId, categorycode, pg, pagesize, out numRecord, imageWidth, numShortNews);
                        pageNum = CountPage(numRecord, pagesize);
                        if (pageNum > 1)
                        {
                            string format = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "categorycode/" + categorycode, "pg/@@page");
                            paging.Visible = true;
                            paging.showing(pageNum, pg, format);
                            Session["currentPage_" + PortalSettings.ActiveTab.TabID.ToString() + "_catcode_" + categorycode] = pg;
                        }
                        if (!displayPaging)
                        {
                            paging.Visible = false;
                        }
                        break;
                    case "5": // theo newsgroup code
                        newsgroupcode = this.Settings["newsgroupcode"].ToString();
                        if (Request.QueryString["newsgroupcode"] != null)
                        {
                            if (newsgroupcode == Request.QueryString["newsgroupcode"])
                            {
                                if (Request.QueryString["pg"] != null)
                                {
                                    pg = Convert.ToInt32(Request.QueryString["pg"]) - 1;
                                }
                            }
                            else if (Session["currentPage_" + PortalSettings.ActiveTab.TabID.ToString() + "_groupcode_" + newsgroupcode] != null)
                            {
                                pg = (int)Session["currentPage_" + PortalSettings.ActiveTab.TabID.ToString() + "_groupcode_" + newsgroupcode];
                            }
                        }
                        numRecord = 0;
                        doc = engine.LoadXMLByGroupCode(PortalId, newsgroupcode, pg, pagesize, out numRecord, imageWidth, numShortNews);
                        pageNum = CountPage(numRecord, pagesize);
                        if (pageNum > 1)
                        {
                            string format = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "newsgroupcode/" + newsgroupcode, "pg/@@page");
                            paging.Visible = true;
                            paging.showing(pageNum, pg, format);
                            Session["currentPage_" + PortalSettings.ActiveTab.TabID.ToString() + "_groupcode_" + newsgroupcode] = pg;
                        }
                        if (!displayPaging)
                        {
                            paging.Visible = false;
                        }
                        break;
                    case "6": // những bài đọc nhiều nhất
                        doc = engine.LoadHotXML(pagesize, "", PortalId, imageWidth, numShortNews);
                        break;
                    case "7": // những bài mới nhất
                        doc = engine.LoadNewestXML(pagesize, "", PortalId, imageWidth, numShortNews);
                        break;
                }
                return doc;
            }
            catch (Exception ex)
            {
                return new XmlDocument();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    NewsController engine = new NewsController();
                    XmlDocument doc = LoadData();
                    XmlElement root = doc.DocumentElement;
                    if (doc.InnerXml != "<newslist></newslist>")
                    {
                        if (Settings["template"] != null)
                        {
                            string template = PortalSettings.HomeDirectory + "Xsl/" + Settings["template"].ToString();
                            DotNetNuke.NewsProvider.Utils.XMLTransform(ltNewsList, template, doc);
                        }
                    }
                    else
                    {
                        if (System.Threading.Thread.CurrentThread.CurrentCulture.ToString() != Common.Globals.GetPortalSettings().DefaultLanguage) ContainerControl.Visible = false;
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