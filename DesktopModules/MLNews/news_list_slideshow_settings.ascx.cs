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

using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common.Utilities;
using System.Data.SqlClient;

namespace DotNetNuke.News
{
    public partial class news_list_slideshow_settings : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        private void LoadDropdown()
        {
            CategoryController catdb = new CategoryController();
            DataTable dt = catdb.LoadTree(false, PortalId, "");
            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "CatName";
            ddlCategory.DataValueField = "CatID";
            ddlCategory.DataBind();

            string strSQL = "News_getNewsGroupByPortal";
            SqlParameter para = new SqlParameter("@PortalID", PortalId);
            DataTable dt1 = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, para);
            ddlNewsGroup.DataSource = dt1;
            ddlNewsGroup.DataTextField = "NewsGroupName";
            ddlNewsGroup.DataValueField = "NewsGroupID";
            ddlNewsGroup.DataBind();
        }

        public override void LoadSettings()
        {
            try
            {
                LoadDropdown();
                if (ModuleSettings["limits"] != null) txtLimits.Text = ModuleSettings["limits"].ToString();
                if (ModuleSettings["source"] != null) radSource.SelectedValue = ModuleSettings["source"].ToString();
                if (ModuleSettings["categoryid"] != null) ddlCategory.SelectedValue = ModuleSettings["categoryid"].ToString();
                if (ModuleSettings["newsgroupid"] != null) ddlNewsGroup.SelectedValue = ModuleSettings["newsgroupid"].ToString();
                if (ModuleSettings["categorycode"] != null) txtCategoryCode.Text = ModuleSettings["categorycode"].ToString();
                if (ModuleSettings["newsgroupcode"] != null) txtNewsGroupCode.Text = ModuleSettings["newsgroupcode"].ToString();
                if (ModuleSettings["timeout"] != null) txtTimeOut.Text = ModuleSettings["timeout"].ToString();
                if (ModuleSettings["displayHeadline"] != null) chkDisplayHeadline.Checked = Convert.ToBoolean(ModuleSettings["displayHeadline"].ToString());
                if (ModuleSettings["imageWidth"] != null) txtImageWidth.Text = ModuleSettings["imageWidth"].ToString();
                if (ModuleSettings["displayTitle"] != null) chkDisplayTitle.Checked = bool.Parse(ModuleSettings["displayTitle"].ToString());
                if (ModuleSettings["titleCSSClass"] != null) txtTitleCSSClass.Text = ModuleSettings["titleCSSClass"].ToString();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            try
            {
                DotNetNuke.Entities.Modules.ModuleController objModules = new DotNetNuke.Entities.Modules.ModuleController();
                objModules.UpdateModuleSetting(ModuleId, "limits", txtLimits.Text);
                objModules.UpdateModuleSetting(ModuleId, "source", radSource.SelectedValue);
                objModules.UpdateModuleSetting(ModuleId, "categoryid", ddlCategory.SelectedValue);
                objModules.UpdateModuleSetting(ModuleId, "newsgroupid", ddlNewsGroup.SelectedValue);
                objModules.UpdateModuleSetting(ModuleId, "categorycode", txtCategoryCode.Text);
                objModules.UpdateModuleSetting(ModuleId, "newsgroupcode", txtNewsGroupCode.Text);
                objModules.UpdateModuleSetting(ModuleId, "timeout", txtTimeOut.Text);
                objModules.UpdateModuleSetting(ModuleId, "displayHeadline", chkDisplayHeadline.Checked.ToString());
                objModules.UpdateModuleSetting(ModuleId, "imageWidth", txtImageWidth.Text);
                objModules.UpdateModuleSetting(ModuleId, "displayTitle", chkDisplayTitle.Checked.ToString());
                objModules.UpdateModuleSetting(ModuleId, "titleCSSClass", txtTitleCSSClass.Text);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}