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
    public partial class news_view : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private void FillContent(NewsInfo news)
        {
            lblContent.Text = news.Content;
            lblDescription.Text = news.Description;
            lblHeadline.Text = news.Headline;
            hdAllowComment.Value = news.AllowComment.ToString();
        }

        private void LoadContent(int newsID)
        {
            NewsController db = new NewsController();
            NewsInfo news = db.Load(newsID);
            if (news != null)
            {
                FillContent(news);
                db.UpdateTotalView(newsID);

                CategoryController catCont = new CategoryController();
                CategoryInfo cat = catCont.Load(news.CatID);
                string catCode = cat.CatCode;
                int startBooking = catCode.IndexOf("@@Booking_");
                if (startBooking == 0)
                {
                    string tempCode = catCode.Substring(10);
                    int stopBooking = tempCode.IndexOf(',');
                    string bookingPage = tempCode.Substring(0, stopBooking);
                    int bookingPageID = Convert.ToInt32(bookingPage);
                    lnkBooking.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(bookingPageID, "", "BookingID/" + newsID.ToString());
                    lnkBooking.ImageUrl = DotNetNuke.Common.Globals.ApplicationPath + "/images/booking_" + System.Threading.Thread.CurrentThread.CurrentCulture.ToString() + ".gif";
                }
            }
            else
            {
                //if (System.Threading.Thread.CurrentThread.CurrentCulture.ToString() != Common.Globals.GetPortalSettings().DefaultLanguage) ContainerControl.Visible = false;
                //ContainerControl.Visible = false;
                //Response.Redirect("http://" + PortalSettings.PortalAlias.HTTPAlias);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string source = "0";
                if (Settings["source"] != null)
                    source = Settings["source"].ToString();
                int newsID = 0;
                switch (source)
                {
                    case "0":
                        if (Request.QueryString["id"] != null)
                        {
                            newsID = Convert.ToInt32(Request.QueryString["id"]);
                        }
                        else
                        {
                            tblContent.Visible = false;
                        }
                        break;
                    case "1":
                        newsID = Convert.ToInt32(Settings["newsid"]);
                        break;
                }
                LoadContent(newsID);
                if (hdAllowComment.Value == "True")
                {
                    writeComment.PIntNewsID = newsID;
                    LoadComment(newsID);
                }
                else
                {
                    lblReaderComment.Visible = false;
                    writeComment.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void LoadComment(int NewsID)
        {
            XmlDocument doc = null;
            NewsController engine = new NewsController();
            int numRecord = 0;
            doc = engine.LoadCommentXML(NewsID, 1, out numRecord);
            if (numRecord == 0)
            {
                lblReaderComment.Visible = false;
            }
            else
            {
                if (Settings["CommentListTemplate"] != null)
                {
                    //string template = "Portals/" + PortalId.ToString() + "/Xsl/" + Settings["CommentListTemplate"].ToString();
                    string template = PortalSettings.HomeDirectory + "Xsl/" + Settings["CommentListTemplate"].ToString();
                    DotNetNuke.NewsProvider.Utils.XMLTransform(xmlTransformer, template, doc);
                }
                else
                {
                    string template = PortalSettings.HomeDirectory + "Xsl/comment_list.xsl";
                    DotNetNuke.NewsProvider.Utils.XMLTransform(xmlTransformer, template, doc);
                }
            }
        }
    }
}