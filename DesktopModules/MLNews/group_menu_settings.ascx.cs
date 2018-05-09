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

namespace DotNetNuke.News
{
    public partial class group_menu_settings : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public override void LoadSettings()
        {
            try
            {
                if (ModuleSettings["code"] != null) txtCode.Text = ModuleSettings["code"].ToString();
                if (ModuleSettings["CSSPrefix"] != null)
                {
                    txtCSSPrefix.Text = ModuleSettings["CSSPrefix"].ToString();
                }
                if (ModuleSettings["source"] != null) lstRdSource.SelectedValue = ModuleSettings["source"].ToString();
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