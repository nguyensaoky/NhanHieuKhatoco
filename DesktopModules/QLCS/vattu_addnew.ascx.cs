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
    public partial class vattu_addnew : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        public string dataString = "";
        private CaSauController csCont = new CaSauController();

        private void InitDonViTinh()
        {
            DataTable tblDonViTinh = csCont.VatTu_GetDonViTinh();
            foreach (DataRow r in tblDonViTinh.Rows)
            {
                dataString += r["DonViTinh"].ToString() + ";";
            }
            if (dataString != "")
            {
                dataString = dataString.Remove(dataString.Length - 1);
            }
            Page.ClientScript.RegisterStartupScript(typeof(string), "initDonViTinh", "<script language=javascript>initDonViTinh();</script>", false);
        }

        private void BindControls()
        {
            int y = DateTime.Now.Year;
            for (int i = y; i >= y-9; i--)
            {
                ddlNam.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            if (Request.QueryString["loaivattu"] != null) ddlLoaiVatTu.SelectedValue = Request.QueryString["loaivattu"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                InitDonViTinh();
                Page.ClientScript.RegisterClientScriptInclude("ac_actb", ResolveUrl("~/js/autocomplete/actb.js"));
                Page.ClientScript.RegisterClientScriptInclude("ac_common", ResolveUrl("~/js/autocomplete/common.js"));
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
                Save.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtTenVatTu.Text == "" || txtDonViTinh.Text == "")
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "dulieuthieu", "alert('Bạn phải nhập đủ dữ liệu!');", true);
                    return;
                }
                int result = csCont.VatTu_ThemMoi(ddlLoaiVatTu.SelectedValue, txtTenVatTu.Text, txtTenVatTu.Text, txtDonViTinh.Text, decimal.Parse(txtSoLuongHienTai.Text), int.Parse(ddlThang.SelectedValue), int.Parse(ddlNam.SelectedValue));
                if (result == -1)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Thêm mới không thành công');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Thêm mới thành công!');", true);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}