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
    public partial class foruser_settings : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public override void LoadSettings()
        {
            try
            {
                if (!IsPostBack)
                {
                    CategoryController catCont = new CategoryController();
                    DataTable tblCats = catCont.LoadTable(PortalId);
                    lstChkCats.DataSource = tblCats;
                    lstChkCats.DataTextField = "CatName";
                    lstChkCats.DataValueField = "CatID";
                    lstChkCats.DataBind();
                }
                if (ModuleSettings["pageSizeAdmin"] != null) txtLimitAdmin.Text = ModuleSettings["pageSizeAdmin"].ToString();
                if (ModuleSettings["cats"] != null && ModuleSettings["cats"].ToString() != "")
                {
                    string[] cats;
                    cats = ModuleSettings["cats"].ToString().Split(new char[] { ',' }, StringSplitOptions.None);
                    foreach (string c in cats)
                    {
                        foreach (ListItem l in lstChkCats.Items)
                        {
                            if (l.Value == c)
                            {
                                l.Selected = true;
                                break;
                            }
                        }
                        
                    }
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
                objModules.UpdateModuleSetting(ModuleId, "pageSizeAdmin", txtLimitAdmin.Text);
                string strCats = "";
                foreach (ListItem l in lstChkCats.Items)
                {
                    if (l.Selected == true)
                    {
                        strCats += "," + l.Value;
                    }
                }
                if (strCats != "")
                {
                    strCats = strCats.Substring(1);
                }
                objModules.UpdateModuleSetting(ModuleId, "cats", strCats);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}