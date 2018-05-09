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
using DotNetNuke.Services.Localization;
using System.Collections.Generic;

namespace DotNetNuke.Modules.QLCS
{
    public partial class note_multichuong : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        private void BindControls()
        {
            DataTable dtChuong = csCont.LoadChuong(1);
            ddlChuong.DataSource = dtChuong;
            ddlChuong.DataTextField = "Chuong";
            ddlChuong.DataValueField = "IDChuong";
            ddlChuong.DataBind();
            ddlChuong.Items.Insert(0, new ListItem("Chung", "-1"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/impromptu/jquery.js"));
                if (!Page.IsPostBack)
                {
                    BindControls();
                    lnkCancel.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "");
                }
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    Save.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");

                DateTime Ngay = DateTime.MaxValue;
                if (txtNgay.Text != "")
                {
                    Ngay = DateTime.Parse(txtNgay.Text, ci);
                    if (Ngay < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Ngày không được trước ngày khóa sổ!');", true);
                        return;
                    }
                }
                string StrChuong = Config.GetSelectedValues_At(ddlChuong);
                csCont.InsertUpdateMultiNote(Ngay, StrChuong, txtNote.Text, UserId, chkReplace.Checked);
                Page.ClientScript.RegisterStartupScript(typeof(string), "insertsucess", "alert('Đã xong!');", true);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}