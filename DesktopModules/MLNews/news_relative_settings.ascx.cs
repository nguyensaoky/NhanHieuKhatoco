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
    public partial class news_relative_settings : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        private void LoadDropdown()
        {
            string path = PortalSettings.HomeDirectoryMapPath + "Xsl";
            DotNetNuke.NewsProvider.Utils.BindTemplateByName(ddlTemplate, path, "news_relative*.xsl");
        }

        public override void LoadSettings()
        {
            try
            {
                LoadDropdown();
                if (ModuleSettings["limits"] != null) txtLimits.Text = ModuleSettings["limits"].ToString();
                if (ModuleSettings["source"] != null) lstRdSource.SelectedValue = ModuleSettings["source"].ToString();
                if (ModuleSettings["template"] != null) ddlTemplate.SelectedValue = ModuleSettings["template"].ToString();
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
                objModules.UpdateModuleSetting(ModuleId, "source", lstRdSource.SelectedValue);
                objModules.UpdateModuleSetting(ModuleId, "template", ddlTemplate.Text);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}