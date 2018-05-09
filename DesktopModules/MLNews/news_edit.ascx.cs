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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Localization;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Net.Mail;
using DotNetNuke.Entities.Portals;

namespace DotNetNuke.News
{
    public partial class news_edit : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private void BindControls()
        {
            CategoryController catdb = new CategoryController();
            DataTable dt;
            //if (DotNetNuke.Security.PortalSecurity.IsInRole("Administrators"))
            //{
                dt = catdb.LoadTree(false, PortalId, "");
            //}
            //else
            //{
            //    dt = catdb.LoadTree(false, UserId, PortalId, "");
            //}

            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "CatName";
            ddlCategory.DataValueField = "CatID";
            ddlCategory.DataBind();

            NewsGroupController newsGroupCont = new NewsGroupController();
            DataTable dt1 = newsGroupCont.LoadTree(false, PortalId);
            DataRow r = dt1.NewRow();
            r["NewsGroupName"] = Localization.GetString("lblShareGroup", Localization.GetResourceFile(this, "news_edit.ascx"));
            r["NewsGroupID"] = "__Shared__";
            dt1.Rows.Add(r);
            lstChkNewsGroup.DataSource = dt1;
            lstChkNewsGroup.DataTextField = "NewsGroupName";
            lstChkNewsGroup.DataValueField = "NewsGroupID";
            lstChkNewsGroup.DataBind();
        }

        private bool IsNewsGroupInTable(string NewsGroupID, DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                if ((string)row["NewsGroupID"] == NewsGroupID)
                {
                    return true;
                }
            }
            return false;
        }

        private void LoadData(int id)
        {
            if (Request.QueryString["cat"] != null)
            {
                lblCatString.Text = Request.QueryString["cat"];
            }
            NewsController db = new NewsController();
            NewsInfo news = db.LoadNoML(id);

            txtDescription.Text = news.Description;
            txtHeadline.Text = news.Headline;
            if (news.ImageUrl != null && news.ImageUrl != "" && news.ImageUrl.LastIndexOf(';')>-1)
            {
                ctlURL.Url = news.ImageUrl.Substring(news.ImageUrl.LastIndexOf(';') + 1);
                chkImageURL.Checked = true;
            }
            else
            {
                chkImageURL.Checked = false;
            }
            
            txtKeyWords.Text = news.KeyWords;

            ddlCategory.SelectedValue = news.CatID;
            chkPublished.Checked = news.Published;
            chkAllowComment.Checked = news.AllowComment;

            DataTable fDt = db.GetNewsGroupByNews(id);
            foreach (ListItem item in lstChkNewsGroup.Items)
            {
                if (IsNewsGroupInTable(item.Value, fDt))
                {
                    item.Selected = true;
                    if(item.Value == "__Shared__") hdShared.Value = "1";
                }
            }

            teContent.Text = news.Content;
            //txtModifyDate.Text = news.ModifyDate.ToShortDateString();
            txtModifyDate.Text = news.ModifyDate.ToString("dd/MM/yyyy HH:mm:ss");
            txtStartDate.Text = news.StartDate == null ? "" : ((DateTime)news.StartDate).ToShortDateString();
            txtEndDate.Text = news.EndDate == null ? "" : ((DateTime)news.EndDate).ToShortDateString();

            int Feature = news.Feature;
            int New = Feature % 2;
            int Hot = (Feature - New) / 2;
            chkNew.Checked = Convert.ToBoolean(New);
            chkHot.Checked = Convert.ToBoolean(Hot);

            txtWriter.Text = news.Writer;
            txtDonVi.Text = news.DonVi;
            chkFromOuter.Checked = news.FromOuter;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {   
                if (!Page.IsPostBack)
                {
                    cmdModifyDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtModifyDate);
                    cmdStartDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtStartDate);
                    cmdEndDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtEndDate);
                    BindControls();

                    if (Request.QueryString["id"] != null)
                    {
                        int id = Convert.ToInt32(Request.QueryString["id"]);
                        LoadData(id);
                        btnDelete.Visible = true;
                        btnDelete.Attributes.Add("onclick", "if(!confirm('" + Localization.GetString("lblConfirmDelete", Localization.GetResourceFile(this, "news_edit.ascx")) + "')) {return false;};");
                    }
                    else if (lblNewsID.Text != "")
                    {
                        int id = Convert.ToInt32(lblNewsID.Text);
                        LoadData(id);
                        btnDelete.Visible = true;
                        btnDelete.Attributes.Add("onclick", "if(!confirm('" + Localization.GetString("lblConfirmDelete", Localization.GetResourceFile(this, "news_edit.ascx")) + "')) {return false;};");
                    }
                    else
                    {
                        //txtModifyDate.Text = DateTime.Now.ToShortDateString();
                        txtModifyDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        btnDelete.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "admin_news", "mid/" + this.ModuleId.ToString(), "cat/" + lblCatString.Text);
            Response.Redirect(url);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                NewsController db = new NewsController();
                if (Request.QueryString["id"] != null)
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    db.Delete(id);
                    db.DeleteNewsGroupNews(id);
                    string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "admin_news", "mid/" + this.ModuleId.ToString(), "cat/" + lblCatString.Text);
                    Response.Redirect(url);
                }
                else if (lblNewsID.Text != "")
                {
                    int id = Convert.ToInt32(lblNewsID.Text);
                    db.Delete(id);
                    db.DeleteNewsGroupNews(id);
                    string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "admin_news", "mid/" + this.ModuleId.ToString(), "cat/" + lblCatString.Text);
                    Response.Redirect(url);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                NewsInfo news = new NewsInfo();
                if (Request.QueryString["id"] != null)
                {
                    news.ID = Convert.ToInt32(Request.QueryString["id"]);
                }
                else if (lblNewsID.Text != "")
                {
                    news.ID = Convert.ToInt32(lblNewsID.Text);
                }
                else
                {
                    news.ID = 0;
                }
                news.CatID = ddlCategory.SelectedValue;
                news.Content = Server.HtmlDecode(teContent.Text);
                news.CreateID = this.UserId;
                news.ModifyID = this.UserId;
                news.Description = Server.HtmlDecode(txtDescription.Text);

                if (chkImageURL.Checked)
                {
                    NewsController newsCont = new NewsController();
                    DataTable dt = newsCont.GetFileInfo(int.Parse(ctlURL.Url.Substring(7)));
                    if (dt.Rows.Count == 1)
                    {
                        string imagePath = PortalSettings.HomeDirectory + dt.Rows[0]["Folder"].ToString() + dt.Rows[0]["FileName"].ToString();
                        news.ImageUrl = imagePath + ";" + ctlURL.Url;
                        Resize(imagePath, 600);
                    }
                }
                else
                {
                    news.ImageUrl = "";
                }

                news.Headline = txtHeadline.Text;
                news.AllowComment = chkAllowComment.Checked;
                news.Published = chkPublished.Checked;
                news.KeyWords = txtKeyWords.Text.Trim();
                news.ModifyDate = Convert.ToDateTime(txtModifyDate.Text);
                news.StartDate = txtStartDate.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtStartDate.Text);
                news.EndDate = txtEndDate.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtEndDate.Text);
                if (chkNew.Checked)
                {
                    news.Feature += 1;
                }
                if (chkHot.Checked)
                {
                    news.Feature += 2;
                }
                news.Writer = txtWriter.Text;
                news.DonVi = txtDonVi.Text;
                news.FromOuter = chkFromOuter.Checked;

                NewsController db = new NewsController();
                if (news.ID != 0)
                    db.Update(news);
                else
                    db.Insert(news);

                string NewsGroupString = "";
                foreach (ListItem item in lstChkNewsGroup.Items)
                {
                    if (item.Selected == true)
                    {
                        NewsGroupString += "@" + item.Value + "@";
                        if (item.Value == "__Shared__" && hdShared.Value == "0")
                        {
                            SendMailToSuperUser(new string[] { "thanhtuyen@khatoco.com", "nguyensaoky@khatoco.com" }, news.Headline);
                            hdShared.Value = "1";
                        }
                    }
                }
                db.UpdateNewsGroupNews(news.ID, NewsGroupString);

                //Resize image
                string pattern = "(?<=<img[^<]+?src=\")[^\"]+";
                MatchCollection mc  = Regex.Matches(news.Content, pattern, RegexOptions.Multiline);  
                foreach( Match m in mc )
                {
                    Resize(m.Value, 600);
                }  
                //Resize image

                string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "admin_news", "mid/" + this.ModuleId.ToString(), "cat/" + lblCatString.Text);
                Response.Redirect(url);
            }
            catch (Exception ex)
            {
            }
        }

        public void SendMailToSuperUser(string[] arrEmail, string tieude)
        {
            try
            {
                PortalController portalCont = new PortalController();
                DataTable tb = portalCont.GetPortalMail(PortalId);
                SmtpClient smtpClient = new SmtpClient();
                if (tb.Rows.Count > 0)
                {
                    MailMessage mail = new MailMessage();
                    foreach (string email in arrEmail)
                    {
                        if (email != "")
                        {
                            mail.To.Add(email);
                        }
                    }
                    mail.Subject = "Website Khatoco: Có bài viết mới chia sẻ từ " + PortalSettings.PortalName;
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    mail.Body = "Tiêu đề: " + tieude;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.IsBodyHtml = true;

                    string server = Convert.ToString(tb.Rows[0]["MailServer"]);
                    int SmtpPort = 25;
                    int portPos = server.IndexOf(":");
                    if (portPos > -1)
                    {
                        SmtpPort = int.Parse(server.Substring(portPos + 1, server.Length - portPos - 1));
                        server = server.Substring(0, portPos);
                    }
                    smtpClient.Host = server;
                    smtpClient.Port = SmtpPort;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(Convert.ToString(tb.Rows[0]["MailAccount"]), Convert.ToString(tb.Rows[0]["MailPassword"]));
                    mail.From = new MailAddress(Convert.ToString(tb.Rows[0]["MailAccount"]));
                    smtpClient.Send(mail);
                }
            }
            catch (Exception)
            {
            }
        }

        private void saveImage(string path, Bitmap img, long quality)
        {
            int extIdx = path.LastIndexOf('.');
            string ext = path.Substring(extIdx + 1);
            ext = ext.ToLower();
            if (ext == "jpg") ext = "jpeg";
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
            ImageCodecInfo codec = this.getEncoderInfo("image/" + ext);
            if (codec == null) return;
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, codec, encoderParams);
        }

        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

        private System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, int imageWidth, long length, out long quality)
        {
            quality = 100;
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;
            int destWidth = sourceWidth;
            int destHeight = sourceHeight;
            float nPercent = 0;
            nPercent = ((float)imageWidth / (float)sourceWidth);

            if (nPercent < 1.0)
            {
                //quality = (long)(quality * nPercent);
                quality = 75;
                destWidth = (int)(sourceWidth * nPercent);
                destHeight = (int)(sourceHeight * nPercent);
                System.Drawing.Bitmap b = new System.Drawing.Bitmap(destWidth, destHeight);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((System.Drawing.Image)b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                g.Dispose();
                return (System.Drawing.Image)b;
            }
            else
            {
                if (length > 71680)
                {
                    quality = 75;
                    return imgToResize;
                }
                else
                {
                    return null;
                }
            }
        }

        public void Resize(string imagePath, int preferWidth)
        {
            try
            {
                long quality = 100;
                string fullPath = Server.MapPath(imagePath);
                MemoryStream ms = new MemoryStream(File.ReadAllBytes(fullPath));
                System.Drawing.Image b = System.Drawing.Image.FromStream(ms);
                System.Drawing.Image i = resizeImage(b, preferWidth, ms.Length, out quality);
                if (quality < 100) saveImage(fullPath, (Bitmap)i, quality);
                ms.Dispose();
            }
            catch (Exception)
            {
            }
        }

        protected void btnSavePreview_Click(object sender, EventArgs e)
        {
            try
            {
                NewsInfo news = new NewsInfo();
                if (Request.QueryString["id"] != null)
                {
                    news.ID = Convert.ToInt32(Request.QueryString["id"]);
                }
                else if (lblNewsID.Text != "")
                {
                    news.ID = Convert.ToInt32(lblNewsID.Text);
                }
                else
                {
                    news.ID = 0;
                }
                news.CatID = ddlCategory.SelectedValue;
                news.Content = Server.HtmlDecode(teContent.Text);
                news.CreateID = this.UserId;
                news.ModifyID = this.UserId;
                news.Description = Server.HtmlDecode(txtDescription.Text);

                if (chkImageURL.Checked)
                {
                    NewsController newsCont = new NewsController();
                    DataTable dt = newsCont.GetFileInfo(int.Parse(ctlURL.Url.Substring(7)));
                    if (dt.Rows.Count == 1)
                    {
                        string imagePath = PortalSettings.HomeDirectory + dt.Rows[0]["Folder"].ToString() + dt.Rows[0]["FileName"].ToString();
                        news.ImageUrl = imagePath + ";" + ctlURL.Url;
                        Resize(imagePath, 600);
                    }
                }
                else
                {
                    news.ImageUrl = "";
                }

                news.Headline = txtHeadline.Text;
                news.AllowComment = chkAllowComment.Checked;
                news.Published = chkPublished.Checked;
                news.KeyWords = txtKeyWords.Text.Trim();
                news.ModifyDate = Convert.ToDateTime(txtModifyDate.Text);
                news.StartDate = txtStartDate.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtStartDate.Text);
                news.EndDate = txtEndDate.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtEndDate.Text);
                if (chkNew.Checked)
                {
                    news.Feature += 1;
                }
                if (chkHot.Checked)
                {
                    news.Feature += 2;
                }
                news.Writer = txtWriter.Text;
                if (txtDonVi.Text == "") news.DonVi = PortalSettings.PortalName;
                else news.DonVi = txtDonVi.Text;
                news.FromOuter = chkFromOuter.Checked;

                NewsController db = new NewsController();
                if (news.ID != 0)
                    db.Update(news);
                else
                    db.Insert(news);

                string NewsGroupString = "";
                foreach (ListItem item in lstChkNewsGroup.Items)
                {
                    if (item.Selected == true)
                    {
                        NewsGroupString += "@" + item.Value + "@";
                        if (item.Value == "__Shared__" && hdShared.Value == "0")
                        {
                            SendMailToSuperUser(new string[] { "nguyensaoky@khatoco.com" }, news.Headline);
                            hdShared.Value = "1";
                        }
                    }
                }
                db.UpdateNewsGroupNews(news.ID, NewsGroupString);

                btnDelete.Visible = true;
                btnDelete.Attributes["onclick"] = "if(!confirm('" + Localization.GetString("lblConfirmDelete", Localization.GetResourceFile(this, "news_edit.ascx")) + "')) {return false;};";
                lblNewsID.Text = news.ID.ToString();

                //Resize image
                string pattern = "(?<=<img[^<]+?src=\")[^\"]+";
                MatchCollection mc = Regex.Matches(news.Content, pattern, RegexOptions.Multiline);
                foreach (Match m in mc)
                {
                    Resize(m.Value, 600);
                }
                //Resize image

                CategoryController catCont = new CategoryController();
                CategoryInfo catInfo = catCont.Load(news.CatID);
                string previewUrl = DotNetNuke.Common.Globals.NavigateURL(catInfo.DesktopViewID, "", "id/" + news.ID.ToString());
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "preview", "window.open('" + previewUrl + "','','width=800,height=600,scrollbars=1')", true);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}