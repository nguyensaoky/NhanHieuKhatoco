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
using System.Collections.Generic;

namespace DotNetNuke.News
{
    public partial class cat_menu_settings : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void BindCatDDL()
        {
            CategoryController catdb = new CategoryController();
            DataTable dt = catdb.LoadTree(false, PortalId, "");
            DataRow row = dt.NewRow();
            row["CatName"] = "";
            row["CatID"] = 0;
            dt.Rows.InsertAt(row, 0);
            ddlParentCat.DataSource = dt;
            ddlParentCat.DataTextField = "CatName";
            ddlParentCat.DataValueField = "CatID";
            ddlParentCat.DataBind();
        }

        public override void LoadSettings()
        {
            try
            {
                BindCatDDL();
                if (ModuleSettings["code"] != null) txtCode.Text = ModuleSettings["code"].ToString();
                if (ModuleSettings["CSSPrefix"] != null)
                {
                    txtCSSPrefix.Text = ModuleSettings["CSSPrefix"].ToString();
                }
                if (ModuleSettings["source"] != null) lstRdSource.SelectedValue = ModuleSettings["source"].ToString();
                if (ModuleSettings["parentcat"] != null) ddlParentCat.SelectedValue = ModuleSettings["parentcat"].ToString();
                if (ModuleSettings["menuwidth"] != null) txtMenuWidth.Text = ModuleSettings["menuwidth"].ToString();
                if (ModuleSettings["ExpandAll"] != null)
                {
                    chkExpandAll.Checked = Convert.ToBoolean(ModuleSettings["ExpandAll"]);
                }
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
                objModules.UpdateModuleSetting(ModuleId, "code", txtCode.Text);
                objModules.UpdateModuleSetting(ModuleId, "source", lstRdSource.SelectedValue);
                objModules.UpdateModuleSetting(ModuleId, "parentcat", ddlParentCat.Text);
                objModules.UpdateModuleSetting(ModuleId, "menuwidth", txtMenuWidth.Text);
                objModules.UpdateModuleSetting(ModuleId, "ExpandAll", chkExpandAll.Checked.ToString());
                objModules.UpdateModuleSetting(ModuleId, "CSSPrefix", txtCSSPrefix.Text);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}