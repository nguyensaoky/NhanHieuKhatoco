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
    public partial class news_list_slideshow : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private string LoadData()
        {
            string returnText = "";
            try
            {
                int pg = 0;
                int pagesize = Convert.ToInt32(Settings["limits"]);
                if (pagesize == 0) pagesize = 4;

                string source = "0";
                if (Settings["source"] != null)
                    source = Settings["source"].ToString();
                int numRecord;
                string newsgroupid = "";
                string categoryid = "";
                string newsgroupcode = "";
                string categorycode = "";

                
                NewsController engine = new NewsController();
                bool displayTitle = true;
                if (Settings["displayTitle"] != null)
                {
                    displayTitle = bool.Parse(Settings["displayTitle"].ToString());
                }
                bool displayHeadline = false;
                if (Settings["displayHeadline"] != null)
                {
                    displayHeadline = bool.Parse(Settings["displayHeadline"].ToString());
                }
                int timeOut = 3000;
                if (Settings["timeOut"] != null)
                {
                    timeOut = int.Parse(Settings["timeout"].ToString());
                }
                string titleCSSClass = "";
                if (Settings["titleCSSClass"] != null)
                {
                    titleCSSClass = Settings["titleCSSClass"].ToString();
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
                            categoryid = Request.QueryString["categoryid"];
                            if (displayTitle)
                            {
                                category = MLCategoryController.GetCategory(categoryid, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), false);
                                if (category.MLCatName == null)
                                {
                                    ltTitle.Text = @"<div class=""" + titleCSSClass + @"_outer"">" + @"<div class=""" + titleCSSClass + @"_inner"">" + category.CatName + @"</div></div>";
                                }
                                else
                                {
                                    ltTitle.Text = @"<div class=""" + titleCSSClass + @"_outer"">" + @"<div class=""" + titleCSSClass + @"_inner"">" + category.MLCatName.StringTextOrFallBack + @"</div></div>";
                                }
                            }

                            returnText = engine.LoadText(categoryid, pagesize, imageWidth, timeOut, displayHeadline);
                        }
                        else
                            returnText = "";
                        break;
                    case "1": // theo newsgroup từ URL
                        if (Request.QueryString["newsgroupid"] != null)
                        {
                            newsgroupid = Request.QueryString["newsgroupid"];
                            returnText = engine.LoadTextByNewsGroup(newsgroupid, pagesize, imageWidth, timeOut, displayHeadline);
                        }
                        else
                            returnText = "";
                        break;
                    case "2": // theo cat được chọn
                        categoryid = this.Settings["categoryid"].ToString();
                        if (displayTitle)
                        {
                            category = MLCategoryController.GetCategory(categoryid, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), false);
                            if (category.MLCatName == null)
                            {
                                ltTitle.Text = @"<div class=""" + titleCSSClass + @"_outer"">" + @"<div class=""" + titleCSSClass + @"_inner"">" + category.CatName + @"</div></div>";
                            }
                            else
                            {
                                ltTitle.Text = @"<div class=""" + titleCSSClass + @"_outer"">" + @"<div class=""" + titleCSSClass + @"_inner"">" + category.MLCatName.StringTextOrFallBack + @"</div></div>";
                            }
                        }
                        returnText = engine.LoadText(categoryid, pagesize, imageWidth, timeOut, displayHeadline);
                        break;
                    case "3": // theo newsgroup được chọn
                        newsgroupid = this.Settings["newsgroupid"].ToString();
                        returnText = engine.LoadTextByNewsGroup(newsgroupid, pagesize, imageWidth, timeOut, displayHeadline);
                        break;
                    case "4": // theo cat code
                        categorycode = this.Settings["categorycode"].ToString();
                        returnText = engine.LoadTextByCatCode(PortalId, categorycode, pagesize, imageWidth, timeOut, displayHeadline);
                        break;
                    case "5": // theo newsgroup code
                        newsgroupcode = this.Settings["newsgroupcode"].ToString();
                        returnText = engine.LoadTextByGroupCode(PortalId, newsgroupcode, pagesize, imageWidth, timeOut, displayHeadline);
                        break;
                    case "6": // những bài đọc nhiều nhất
                        returnText = engine.LoadHotText(pagesize, "", PortalId, imageWidth, timeOut, displayHeadline);
                        break;
                    case "7": // những bài mới nhất
                        returnText = engine.LoadNewestText(pagesize, "", PortalId, imageWidth, timeOut, displayHeadline);
                        break;
                }
                return returnText;
            }
            catch (Exception ex)
            {
                return returnText;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    NewsController engine = new NewsController();
                    string newslist = LoadData();
                    ltNewsList.Text = newslist;
                    Session["backUrl"] = HttpContext.Current.Request.RawUrl;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}