﻿using System;
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
using System.Collections.Generic;

namespace DotNetNuke.Modules.QLCS
{
    public partial class lydochet_edit : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        private void LoadData()
        {
            if (Request.QueryString["lydochetid"] != null)
            {
                DataTable tblCaSau = csCont.LoadLyDoChetByID(int.Parse(Request.QueryString["lydochetid"]));
                if (tblCaSau.Rows.Count != 0)
                { 
                    DataRow r = tblCaSau.Rows[0];
                    txtLyDo.Text = r["TenLyDoChet"].ToString();
                    chkActive.Checked = Convert.ToBoolean(r["Active"]);
                }
                else
                {
                    Save.Visible = false;
                    Page.ClientScript.RegisterStartupScript(typeof(string), "khongcodulieu", "alert('Không có dữ liệu');", true);
                }
            }
            else
            {
                chkActive.Enabled = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    LoadData();
                    lnkCancel.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "");
                    if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !UserInfo.IsInRole("QLCS"))
                    {
                        Save.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Save.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CaSauController csCont = new CaSauController();
                int result;
                int idlydochet = 0;
                if (Request.QueryString["lydochetid"] != null)
                {
                    csCont.UpdateLyDoChet(int.Parse(Request.QueryString["lydochetid"]), txtLyDo.Text, "", chkActive.Checked, "");
                }
                else
                {
                    csCont.InsertLyDoChet(txtLyDo.Text, "", true, "", out idlydochet);
                }
                Page.ClientScript.RegisterStartupScript(typeof(string), "updatesuccess", "alert('Cập nhật thành công!');", true);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}