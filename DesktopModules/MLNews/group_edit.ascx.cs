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
    public partial class group_edit : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private void BindControls()
        {
            ArrayList tabs = DotNetNuke.Common.Globals.GetPortalTabs(PortalSettings.DesktopTabs, false, true, false, true);
            for (int i = 0; i < tabs.Count; i++)
            {
                DotNetNuke.Entities.Tabs.TabInfo tab = (DotNetNuke.Entities.Tabs.TabInfo)tabs[i];
                ddlDesktopListID.Items.Add(new ListItem(tab.TabName, tab.TabID.ToString()));
                ddlDesktopViewID.Items.Add(new ListItem(tab.TabName, tab.TabID.ToString()));
            }
        }

        private void LoadData(string id)
        {
            NewsGroupController db = new NewsGroupController();
            NewsGroupInfo group = db.Load(id);

            txtDescription.Text = group.Description;
            txtGroupCode.Text = group.NewsGroupCode;
            txtGroupName.Text = group.NewsGroupName;
            txtOrderNumber.Text = group.OrderNumber.ToString();
            

            int res = -1;
            int iIndex = group.NewsGroupID.IndexOf('_');
            if (iIndex >= 0 && int.TryParse(group.NewsGroupID.Substring(0, iIndex), out res) && res == PortalId)
            {
                lblGroupID.Text = group.NewsGroupID.Substring(iIndex + 1);
            }
            else
            {
                lblGroupID.Text = group.NewsGroupID;
            }

            lblGroupID.Visible = true;
            txtGroupID.Visible = false;

            ddlDesktopListID.SelectedValue = group.DesktopListID.ToString();
            ddlDesktopViewID.SelectedValue = group.DesktopViewID.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (!RoleProvider.IsAdminRole(PortalSettings.AdministratorRoleName))
                //{
                //    lblRestricted.Visible = true;
                //    return;
                //}

                if (!Page.IsPostBack)
                {
                    BindControls();
                    if (Request.QueryString["id"] != null)
                    {
                        LoadData(Request.QueryString["id"]);
                        btnDelete.Visible = true;
                        btnDelete.Attributes.Add("onclick", "if(!confirm('" + Localization.GetString("lblConfirmDelete", Localization.GetResourceFile(this, "group_edit.ascx")) + "')) {return false;};");
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
                NewsGroupInfo group = new NewsGroupInfo();
                if (Request.QueryString["id"] != null)
                    group.NewsGroupID = Request.QueryString["id"];
                else
                    group.NewsGroupID = PortalId.ToString() + "_" + txtGroupID.Text;
                group.Description = txtDescription.Text.Trim();
                group.NewsGroupCode = txtGroupCode.Text.Trim();
                group.NewsGroupName = txtGroupName.Text;
                group.OrderNumber = Convert.ToInt32(txtOrderNumber.Text);
                group.PortalID = PortalId;
                group.DesktopListID = Convert.ToInt32(ddlDesktopListID.SelectedValue);
                group.DesktopViewID = Convert.ToInt32(ddlDesktopViewID.SelectedValue);
                if (group.NewsGroupID == "") group.NewsGroupID = PortalId.ToString() + "_" + lblGroupID.Text;
                NewsGroupController db = new NewsGroupController();
                if (Request.QueryString["id"] != null)
                    db.Update(group);
                else
                    db.Insert(group);

                string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "admin_group", "mid/" + this.ModuleId.ToString());
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
                NewsGroupController db = new NewsGroupController();
                db.Delete(Request.QueryString["id"]);

                string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "admin_group", "mid/" + this.ModuleId.ToString());
                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "admin_group", "mid/" + this.ModuleId.ToString());
            Response.Redirect(url);
        }
    }
}