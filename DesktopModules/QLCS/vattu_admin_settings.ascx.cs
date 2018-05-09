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

namespace DotNetNuke.Modules.QLCS
{
    public partial class vattu_admin_settings : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public override void LoadSettings()
        {
            try
            {
                if (ModuleSettings["LoaiVatTu"] != null) ddlLoaiVatTu.SelectedValue = ModuleSettings["LoaiVatTu"].ToString();
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
                objModules.UpdateModuleSetting(ModuleId, "LoaiVatTu", ddlLoaiVatTu.SelectedValue);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}