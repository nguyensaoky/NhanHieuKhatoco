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
    public partial class general_settings : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public override void LoadSettings()
        {
            try
            {
                if (ModuleSettings["pageSizeAdmin"] != null) txtLimitAdmin.Text = ModuleSettings["pageSizeAdmin"].ToString();
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
                objModules.UpdateModuleSetting(ModuleId, "pageSizeAdmin", txtLimitAdmin.Text);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}