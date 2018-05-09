#region DotNetNuke License

// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2006
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

#endregion

using System;
using DotNetNuke.Common;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.UI.WebControls;
using DotNetNuke.NewsProvider;
using System.Data;
using System.Data.SqlClient;
using DotNetNuke.Entities.Users;

namespace DotNetNuke.UI.Skins.Controls
{
    /// <summary></summary>
    /// <returns></returns>
    /// <remarks></remarks>
    /// <history>
    /// 	[cniknet]	10/15/2004	Replaced public members with properties and removed
    ///                             brackets from property names
    /// </history>
    public partial class AdminNavList : SkinObjectBase
    {
        private string mode;
        public string Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        protected void Page_Load( Object sender, EventArgs e )
        {
            try
            {
                if (!IsPostBack)
                {
                    DataTable dtTabAdmin;
                    DataTable dtTabAdminChildren;
                    if (mode == "Host")
                    {
                        if (UserController.GetCurrentUserInfo().IsSuperUser)
                        {
                            dtTabAdmin = LoadTabByName("Host", null);
                            dtTabAdminChildren = LoadChild(Convert.ToInt32(dtTabAdmin.Rows[0]["TabID"]));
                            ddlAdminNavList.DataSource = dtTabAdminChildren;
                            ddlAdminNavList.DataTextField = "TabName";
                            ddlAdminNavList.DataValueField = "TabID";
                            ddlAdminNavList.DataBind();
                            lblTitle.Text = "Host";
                        }
                        else
                        {
                            ddlAdminNavList.Visible = false;
                            lblTitle.Visible = false;
                            btnGo.Visible = false;
                        }
                    }
                    else if (mode == "Admin")
                    {
                        dtTabAdmin = LoadTabByName("Admin", PortalSettings.PortalId);
                        dtTabAdminChildren = LoadChild(Convert.ToInt32(dtTabAdmin.Rows[0]["TabID"]));
                        ddlAdminNavList.DataSource = dtTabAdminChildren;
                        ddlAdminNavList.DataTextField = "TabName";
                        ddlAdminNavList.DataValueField = "TabID";
                        ddlAdminNavList.DataBind();
                        lblTitle.Text = "Admin";
                    }
                }
            }
            catch( Exception exc ) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException( this, exc );
            }
        }

        public DataTable LoadTabByName(string TabName, int? PortalID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@TabName", TabName);
                param[1] = new SqlParameter("@PortalId", PortalID);
                result = DataProvider.SelectSP("GetTabByName", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadChild(int TabID)
        {
            DataTable result = null;
            try
            {
                SqlParameter param = new SqlParameter("@ParentId", TabID);
                result = DataProvider.SelectSP("GetTabsByParentId", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        
        protected void btnGo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(int.Parse(ddlAdminNavList.SelectedValue)));
        }
    }
}