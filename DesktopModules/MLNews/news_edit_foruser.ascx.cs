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

namespace DotNetNuke.News
{
    public partial class news_edit_foruser : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private void BindControls()
        {
            string[] cats = null;
            if (Settings["cats"] != null && Settings["cats"].ToString() != "")
            {
                cats = Settings["cats"].ToString().Split(new char[] { ',' }, StringSplitOptions.None);
            }
            CategoryController catCont = new CategoryController();
            CategoryInfo cat;
            if (cats != null)
            {
                foreach (string c in cats)
                {
                    cat = catCont.Load(c);
                    ddlCategory.Items.Add(new ListItem(cat.CatName, c));
                }
            }
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
                    if (ddlCategory.Items.Count == 0)
                    {
                        btnSave.Enabled = false;
                        btnDelete.Enabled = false;
                    }

                    if (Request.QueryString["id"] != null)
                    {
                        int id = Convert.ToInt32(Request.QueryString["id"]);
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
            string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "mid/" + this.ModuleId.ToString(), "cat/" + lblCatString.Text);
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
                    string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "mid/" + this.ModuleId.ToString(), "cat/" + lblCatString.Text);
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
                        news.ImageUrl = PortalSettings.HomeDirectory +  dt.Rows[0]["Folder"].ToString() + dt.Rows[0]["FileName"].ToString() + ";" + ctlURL.Url;
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
                NewsController db = new NewsController();
                if (Request.QueryString["id"] != null)
                    db.Update(news);
                else
                    db.Insert(news);

                string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "mid/" + this.ModuleId.ToString(), "cat/" + lblCatString.Text);
                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}