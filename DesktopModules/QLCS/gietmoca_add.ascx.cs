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
    public partial class gietmoca_add : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        
        private void BindControls()
        {
            txtNgayMo.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindControls();
                    lnkCancel.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "");
                }
                DataTable tblSanPham = csCont.LoadVatTu_HienTai("SPGM_DangDung", "MoTa");
                foreach (DataRow r in tblSanPham.Rows)
                {
                    Label lbl = new Label();
                    lbl.Text = "<div style='width:100px;float:left;text-align:left;'>" + r["TenVatTu"].ToString() + "</div>";
                    dsSanPham.Controls.Add(lbl);
                    TextBox txt = new TextBox();
                    txt.ID = r["VatTu"].ToString();
                    txt.Attributes["style"] = "float:left;";
                    dsSanPham.Controls.Add(txt);
                    Label lblDVT = new Label();
                    lblDVT.Text = "<div style='text-align:left;'>&nbsp;" + r["DonViTinh"].ToString() + "</div><div style='clear:both;'></div>";
                    dsSanPham.Controls.Add(lblDVT);
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
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string strSanPham = "";
                string strKhoiLuong = "";
                DataTable tblSanPham = csCont.LoadVatTu_HienTai("SPGM_DangDung","MoTa");
                if(txtNgayMo.Text == "")
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "ngaymorong", "alert('Bạn phải nhập ngày mổ!');", true);
                    return;
                }
                foreach (DataRow r in tblSanPham.Rows)
                {
                    TextBox txt = (TextBox)dsSanPham.FindControl(r["VatTu"].ToString());
                    if (txt.Text != "" && decimal.Parse(txt.Text, ci) > 0)
                    {
                        strSanPham += "@" + r["VatTu"].ToString() + "@";
                        strKhoiLuong += "@" + txt.Text + "@";
                    }
                }
                DateTime dtNgayMo = DateTime.Parse(txtNgayMo.Text, ci);
                if (dtNgayMo < Config.NgayKhoaSo())
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Ngày mổ không được trước ngày khóa sổ!');", true);
                    return;
                }
                int GietMoCa = 0;
                int result = csCont.GietMoCa_ThemMoi(strSanPham, strKhoiLuong, DateTime.Parse(txtNgayMo.Text, ci), txtBienBan.Text.Trim(), 0, UserId, out GietMoCa);
                if (result == 0)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Thời điểm giết mổ không hợp lệ!');", true);
                    return;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Thêm mới thành công!');", true);
                    string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "gietmoca_chitiet", "mid/" + this.ModuleId.ToString(), "gmcid/" + GietMoCa.ToString());
                    Response.Redirect(url);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}