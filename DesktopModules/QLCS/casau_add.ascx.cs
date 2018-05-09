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
    public partial class casau_add : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private bool isAdmin = false;
        private void BindControls()
        {
            CaSauController csCont = new CaSauController();
            DataTable tblLoaiCa = new DataTable();
            tblLoaiCa = csCont.LoadLoaiCa(1);
            ddlLoaiCa.DataSource = tblLoaiCa;
            ddlLoaiCa.DataTextField = "TenLoaiCa";
            ddlLoaiCa.DataValueField = "IDLoaiCa";
            ddlLoaiCa.DataBind();

            DataTable tblNguonGoc = new DataTable();
            tblNguonGoc = csCont.LoadNguonGoc(1);
            ddlNguonGoc.DataSource = tblNguonGoc;
            ddlNguonGoc.DataTextField = "TenNguonGoc";
            ddlNguonGoc.DataValueField = "IDNguonGoc";
            ddlNguonGoc.DataBind();

            DataTable tblChuong = new DataTable();
            tblChuong = csCont.LoadChuong(1);
            ddlChuong.DataSource = tblChuong;
            ddlChuong.DataTextField = "Chuong";
            ddlChuong.DataValueField = "IDChuong";
            ddlChuong.DataBind();

            DataTable tblCaMe = new DataTable();
            tblCaMe = csCont.LoadCaSauMe_AllTrangThai();
            ddlCaMe.DataSource = tblCaMe;
            ddlCaMe.DataTextField = "CaMe";
            ddlCaMe.DataValueField = "IDCaSau";
            ddlCaMe.DataBind();
            ddlCaMe.Items.Insert(0, new ListItem("", "0"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    txtNgayXuongChuong.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
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
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                int soluong = 1;
                if(!int.TryParse(txtSoLuong.Text.Trim(), out soluong))
                {
                    soluong = 1;
                }
                CaSauController csCont = new CaSauController();
                CaSauInfo cs = new CaSauInfo();
                cs.MaSo = txtMaSo.Text.Trim()==""?"0":txtMaSo.Text.Trim();
                cs.GioiTinh = int.Parse(ddlGioiTinh.SelectedValue);
                cs.Giong = chkGiong.Checked;
                cs.LoaiCa = int.Parse(ddlLoaiCa.SelectedValue);
                if (txtNgayNo.Text != "")
                {
                    cs.NgayNo = DateTime.Parse(txtNgayNo.Text, ci);
                }
                else
	            {
                    cs.NgayNo = null;
	            }
                if (txtNgayXuongChuong.Text != "")
                {
                    cs.NgayXuongChuong = DateTime.Parse(txtNgayXuongChuong.Text, ci);
                }
                else
	            {
                    cs.NgayXuongChuong = DateTime.Now;
	            }
                if (cs.NgayNo != null && cs.NgayNo > cs.NgayXuongChuong)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Ngày nở không được lớn hơn ngày xuống chuồng');", true);
                    return;
                }
                if (cs.NgayXuongChuong < Config.NgayKhoaSo())
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Ngày xuống chuồng không được trước ngày khóa sổ');", true);
                    return;
                }
                cs.NguonGoc = int.Parse(ddlNguonGoc.SelectedValue);
                cs.Chuong = int.Parse(ddlChuong.SelectedValue);
                cs.CaMe = int.Parse(ddlCaMe.SelectedValue);
                cs.Status = 0;
                cs.GhiChu = "";
                int result = csCont.InsertCaSauMultiple(soluong, cs, UserId);
                if(result == -1)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Thêm mới không thành công');", true);
                    return;
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