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
using System.Xml;
using System.Data.SqlClient;
using System.Collections.Generic;
using DotNetNuke.Security.Roles;

namespace DotNetNuke.News
{
    public partial class cat_permission : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private void BindCatDDL()
        {
            CategoryController catdb = new CategoryController();
            DataTable dt = catdb.LoadTree(false, PortalId, "");
            ddlCat.DataSource = dt;
            ddlCat.DataTextField = "CatName";
            ddlCat.DataValueField = "CatID";
            ddlCat.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lnkBack.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "mid/" + this.ModuleId.ToString());
                    BindCatDDL();
                    RoleController objRoles = new RoleController();
                    ArrayList arrRoles;
                    arrRoles = objRoles.GetPortalRoles(PortalId);
                    for (int i = 0; i < arrRoles.Count; i++)
                    {
                        if (((RoleInfo)arrRoles[i]).RoleName == "Administrators")
                        {
                            arrRoles.RemoveAt(i);
                            break;
                        }
                    }
                    
                    grvRoles.DataSource = arrRoles;
                    grvRoles.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlCat_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            CategoryController catCont = new CategoryController();
            DataTable tb = catCont.LoadRolesByCat(ddlCat.SelectedValue);
            foreach (GridViewRow row in grvRoles.Rows)
            {
                ((CheckBox)(row.Cells[2].FindControl("chkChoose"))).Checked = false;
            }
            foreach (GridViewRow row in grvRoles.Rows)
            {
                foreach (DataRow dtRow in tb.Rows)
                {
                    if (row.Cells[0].Text == dtRow["RoleID"].ToString())
                    {
                        ((CheckBox)(row.Cells[2].FindControl("chkChoose"))).Checked = true;
                        break;
                    }
                }
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string sRoleString = "";
            foreach (GridViewRow row in grvRoles.Rows)
            {
                if (((CheckBox)(row.Cells[2].FindControl("chkChoose"))).Checked)
                {
                    sRoleString += "@" + row.Cells[0].Text + "@";
                }
            }
            CategoryController catCont = new CategoryController();
            catCont.InsertRole(ddlCat.SelectedValue, sRoleString);
        }
    }
}