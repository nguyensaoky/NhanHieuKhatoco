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
using System.Collections.Generic;

namespace DotNetNuke.News
{
    public partial class cat_edit : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private void BindControls()
        {
            ArrayList tabs = DotNetNuke.Common.Globals.GetPortalTabs(PortalSettings.DesktopTabs, false, true, false, true);
            for (int i = 0; i < tabs.Count; i++)
            {
                DotNetNuke.Entities.Tabs.TabInfo tab = (DotNetNuke.Entities.Tabs.TabInfo)tabs[i];
                ddlDesktopListID.Items.Add(new ListItem(tab.TabName, tab.TabID.ToString()));
                ddlDesktopViewID.Items.Add(new ListItem(tab.TabName, tab.TabID.ToString()));
                ddlBookingPageID.Items.Add(new ListItem(tab.TabName, tab.TabID.ToString()));
            }
            ddlBookingPageID.Items.Insert(0, new ListItem("", "-1"));
            CategoryController catdb = new CategoryController();
            string sCatID = (Request.QueryString["id"]==null)?"":Request.QueryString["id"];
            DataTable dt = catdb.LoadTree(false, PortalId, sCatID);
            DataRow row = dt.NewRow();
            row["CatName"] = "";
            row["CatID"] = 0;
            dt.Rows.InsertAt(row, 0);

            ddlParentID.DataSource = dt;
            ddlParentID.DataTextField = "CatName";
            ddlParentID.DataValueField = "CatID";
            ddlParentID.DataBind();

            if (Request.QueryString["id"] != null)
            {
                NewsController newsCont = new NewsController();
                Dictionary<int, NewsInfo> dicNews = newsCont.GetNewsByCat(Request.QueryString["id"]);
                ArrayList fLstNews = new ArrayList();
                NewsInfo info = new NewsInfo();
                info.ID = 0;
                info.Headline = "";
                fLstNews.Add(info);
                foreach (NewsInfo fNewsInfo in dicNews.Values)
                {
                    fLstNews.Add(fNewsInfo);
                }
                ddlNews.DataSource = fLstNews;
                ddlNews.DataTextField = "Headline";
                ddlNews.DataValueField = "ID";
                ddlNews.DataBind();
            }
        }

        private void LoadData(string id)
        {
            CategoryController db = new CategoryController();
            CategoryInfo cat = db.Load(id);

            int bookingPageID = -1;
            string catCode = cat.CatCode;
            int startBooking = catCode.IndexOf("@@Booking_");
            if (startBooking == 0)
            {
                string tempCode = catCode.Substring(10);
                int stopBooking = tempCode.IndexOf(',');
                string bookingPage = tempCode.Substring(0, stopBooking);
                bookingPageID = Convert.ToInt32(bookingPage);
                if (stopBooking + 1 < tempCode.Length)
                {
                    catCode = tempCode.Substring(stopBooking + 1);
                }
                else
                {
                    catCode = "";
                }
            }
            txtCatCode.Text = catCode;
            ddlBookingPageID.SelectedValue = bookingPageID.ToString();

            txtDescription.Text = cat.Description;
            txtCatName.Text = cat.CatName;
            txtOrderNumber.Text = cat.OrderNumber.ToString();
            ddlNews.SelectedValue = cat.NewsID.ToString();
            
            int res = -1;
            int iIndex = cat.CatID.IndexOf('_');
            if (iIndex >= 0 && int.TryParse(cat.CatID.Substring(0, iIndex), out res) && res==PortalId)
            {
                lblCatID.Text = cat.CatID.Substring(iIndex + 1);
            }
            else
            {
                lblCatID.Text = cat.CatID;
            }
            lblCatID.Visible = true;
            txtCatID.Visible = false;

            ddlDesktopListID.SelectedValue = cat.DesktopListID.ToString();
            ddlDesktopViewID.SelectedValue = cat.DesktopViewID.ToString();
            
            ddlParentID.SelectedValue = cat.ParentID.ToString();
            chkVisible.Checked = cat.Visible;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session["currentCulture"] = System.Threading.Thread.CurrentThread.CurrentCulture;
                Session["currentPortal"] = PortalId;

                if (!Page.IsPostBack)
                {
                    BindControls();

                    if (Request.QueryString["id"] != null)
                    {
                        LoadData(Request.QueryString["id"]);
                        btnDelete.Visible = true;
                        btnDelete.Attributes.Add("onclick", "if(!confirm('" + Localization.GetString("lblConfirmDelete", Localization.GetResourceFile(this, "cat_edit.ascx")) + "')) {return false;};");
                    }
                    else
                    {
                        btnDelete.Visible = false;
                    }
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
                CategoryInfo cat = new CategoryInfo();
                if (Request.QueryString["id"] != null)
                    cat.CatID = Request.QueryString["id"];
                else
                    cat.CatID = PortalId.ToString() + "_" + txtCatID.Text;

                cat.Description = txtDescription.Text.Trim();
                
                if (ddlBookingPageID.SelectedValue != "-1")
                {
                    cat.CatCode = "@@Booking_" + ddlBookingPageID.SelectedValue + "," + txtCatCode.Text.Trim();
                }
                else
                {
                    cat.CatCode = txtCatCode.Text.Trim();
                }

                cat.DesktopListID = Convert.ToInt32(ddlDesktopListID.SelectedValue);
                cat.DesktopViewID = Convert.ToInt32(ddlDesktopViewID.SelectedValue);
                cat.CatName = txtCatName.Text;
                cat.OrderNumber = Convert.ToInt32(txtOrderNumber.Text);
                cat.ParentID = ddlParentID.SelectedValue;
                cat.PortalID = PortalId;
                cat.Visible = chkVisible.Checked;
                try
                {
                    cat.NewsID = Convert.ToInt32(ddlNews.SelectedValue);
                }
                catch { }
                if (cat.CatID == "") cat.CatID = PortalId.ToString() + "_" + lblCatID.Text;

                CategoryController db = new CategoryController();
                if (Request.QueryString["id"] != null)
                    db.Update(cat);
                else
                    db.Insert(cat);

                string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "admin_cat", "mid/" + this.ModuleId.ToString());
                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                CategoryController db = new CategoryController();
                int res = db.Delete(Request.QueryString["id"]);
                if (res == -1)
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(string), "deletefail", "<script language=javascript>window.alert('" + Localization.GetString("lblDeleteFail", Localization.GetResourceFile(this, "cat_edit.ascx")) + "');</script>");
                }
                else
                {
                    string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "admin_cat", "mid/" + this.ModuleId.ToString());
                    Response.Redirect(url);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "admin_cat", "mid/" + this.ModuleId.ToString());
            Response.Redirect(url);
        }
    }
}