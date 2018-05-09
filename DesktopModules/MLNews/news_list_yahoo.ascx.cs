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
    public partial class news_list_yahoo : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        public string slideHeight = "300";
        public string leftPartWidth = "400";
        public string rightPartWidth = "250";
        public string scrollDownPosition = "260";
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
                string titleCSSClass = "";
                if (Settings["titleCSSClass"] != null)
                {
                    titleCSSClass = Settings["titleCSSClass"].ToString();
                }
                int thumnailHeight = 100;
                if (Settings["thumbnailHeight"] != null)
                {
                    thumnailHeight = int.Parse(Settings["thumbnailHeight"].ToString());
                }
                int thumnailWidth = 100;
                if (Settings["thumbnailWidth"] != null)
                {
                    thumnailWidth = int.Parse(Settings["thumbnailWidth"].ToString());
                }
                if (Settings["slideHeight"] != null)
                {
                    slideHeight = Settings["slideHeight"].ToString();
                    int iScrollDownPosition = int.Parse(slideHeight) - 40;
                    scrollDownPosition = iScrollDownPosition.ToString();
                }
                if (Settings["leftPartWidth"] != null)
                {
                    leftPartWidth = Settings["leftPartWidth"].ToString();
                }
                if (Settings["rightPartWidth"] != null)
                {
                    rightPartWidth = Settings["rightPartWidth"].ToString();
                }
                int titleLength = 150;
                if (Settings["titleLength"] != null)
                {
                    titleLength = int.Parse(Settings["titleLength"].ToString());
                }
                int descLength = 150;
                if (Settings["descLength"] != null)
                {
                    descLength = int.Parse(Settings["descLength"].ToString());
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
                            returnText = engine.LoadTextAsYahooSlideshow(categoryid, pagesize, thumnailHeight, thumnailWidth, titleLength, descLength);
                        }
                        else
                            returnText = "";
                        break;
                    case "1": // theo newsgroup từ URL
                        if (Request.QueryString["newsgroupid"] != null)
                        {
                            newsgroupid = Request.QueryString["newsgroupid"];
                            returnText = engine.LoadTextByNewsGroupAsYahooSlideshow(newsgroupid, pagesize, thumnailHeight, thumnailWidth, titleLength, descLength);
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
                        returnText = engine.LoadTextAsYahooSlideshow(categoryid, pagesize, thumnailHeight, thumnailWidth, titleLength, descLength);
                        break;
                    case "3": // theo newsgroup được chọn
                        newsgroupid = this.Settings["newsgroupid"].ToString();
                        returnText = engine.LoadTextByNewsGroupAsYahooSlideshow(newsgroupid, pagesize, thumnailHeight, thumnailWidth, titleLength, descLength);
                        break;
                    case "4": // theo cat code
                        categorycode = this.Settings["categorycode"].ToString();
                        returnText = engine.LoadTextByCatCodeAsYahooSlideshow(PortalId, categorycode, pagesize, thumnailHeight, thumnailWidth, titleLength, descLength);
                        break;
                    case "5": // theo newsgroup code
                        newsgroupcode = this.Settings["newsgroupcode"].ToString();
                        returnText = engine.LoadTextByGroupCodeAsYahooSlideshow(PortalId, newsgroupcode, pagesize, thumnailHeight, thumnailWidth, titleLength, descLength);
                        break;
                    case "6": // những bài đọc nhiều nhất
                        returnText = engine.LoadHotTextAsYahooSlideshow(pagesize, "", PortalId, thumnailHeight, thumnailWidth, titleLength, descLength);
                        break;
                    case "7": // những bài mới nhất
                        returnText = engine.LoadNewestTextAsYahooSlideshow(pagesize, "", PortalId, thumnailHeight, thumnailWidth, titleLength, descLength);
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
                    ltNewsList.Text = LoadData();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}