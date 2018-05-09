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
    public partial class casauan_edit_thuoc : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        decimal TongKhoiLuong = 0;
        decimal TongKLTT = 0;
        decimal TongKLG = 0;

        decimal cTongKhoiLuong = 0;
        decimal cTongKLTT = 0;
        int cTongSoLuong = 0;
        int cTongSLTT = 0;

        int ChuongScale = 1;
        public int scale = 0;
        public int scaleCT = 0;
        private void BindCaSauAnControls()
        {
            txtThoiDiem.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
        public string thapphan = ".";

        private void BindThuocControls(DateTime ThoiDiem)
        {
            DataTable tblThuoc = csCont.LoadVatTu_ConTonKho("TTY", ThoiDiem);
            ddlThuoc.DataSource = tblThuoc;
            ddlThuoc.DataValueField = "IDVatTu";
            ddlThuoc.DataTextField = "TenVatTu";
            ddlThuoc.DataBind();
            if (ddlThuoc.Items.Count == 0) ddlThuoc.Items.Add(new ListItem("", "0"));

            DataTable tblLoaiCa = csCont.LoadLoaiCa(1);
            ddlLoaiCa.DataSource = tblLoaiCa;
            ddlLoaiCa.DataValueField = "IDLoaiCa";
            ddlLoaiCa.DataTextField = "TenLoaiCa";
            ddlLoaiCa.DataBind();
            ddlLoaiCa.Items.Add(new ListItem("Các loại SS", "0"));
        }

        private void BindThuocControls_Edit()
        {
            DataTable tblThuoc = csCont.LoadVatTu("TTY");
            ddlThuoc.DataSource = tblThuoc;
            ddlThuoc.DataValueField = "IDVatTu";
            ddlThuoc.DataTextField = "TenVatTu";
            ddlThuoc.DataBind();
            if (ddlThuoc.Items.Count == 0) ddlThuoc.Items.Add(new ListItem("", "0"));

            DataTable tblLoaiCa = csCont.LoadLoaiCa(1);
            ddlLoaiCa.DataSource = tblLoaiCa;
            ddlLoaiCa.DataValueField = "IDLoaiCa";
            ddlLoaiCa.DataTextField = "TenLoaiCa";
            ddlLoaiCa.DataBind();
            ddlLoaiCa.Items.Add(new ListItem("Các loại SS", "0"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.ToString() == "vi-VN")
                    thapphan = ",";
                else thapphan = ".";
                scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_TTY_Scale"]);
                scaleCT = scale;
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/impromptu/jquery.js"));
                if (!Page.IsPostBack)
                {
                    lnkCancel.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "mid/" + this.ModuleId.ToString());
                    if (Request.QueryString["csaid"] != null)
                    {
                        hdCaSauAn.Value = Request.QueryString["csaid"];
                    }
                    if (hdCaSauAn.Value == "")
                    {
                        tblThuocChuong.Visible = false;
                        BindCaSauAnControls();
                    }
                    else
                    {
                        tblThuocChuong.Visible = true;
                        BindCaSauAnControls();
                        LoadCaSauAn(int.Parse(hdCaSauAn.Value));
                        BindThuocControls_Edit();
                        LoadThuoc();
                    }
                }

                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    btnSaveCaSauAn.Visible = false;
                    btnSaveThuoc.Visible = false;
                    pnlCommand.Visible = false;
                    lnkThuocMulti.Visible = false;
                }
                else
                {
                    if (ddlRowStatus.SelectedValue == "0")
                    {
                        pnlCommand.Visible = false;
                    }
                    else
                    {
                        pnlCommand.Visible = true;
                    }
                }

                if (hdCaSauAn.Value != "") 
                { 
                    lnkThucAn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "casauan_edit_thucan", "csaid/" + hdCaSauAn.Value, "mid/" + this.ModuleId.ToString());
                    DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
                    int ThuocMultiPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauAnThuocMulti"], PortalId).TabID;
                    lnkThuocMulti.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(ThuocMultiPage, "", "csaid/" + hdCaSauAn.Value) + "','CaSauAn',900,600);";
                    //lnkThuocMulti.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "casauan_thuoc_multi", "csaid/" + hdCaSauAn.Value, "mid/" + this.ModuleId.ToString()) + "','CaSauAn',900,600);";
                }
                if (ddlLoaiCa.SelectedValue == "1") scaleCT = scaleCT + 2;
                lnkPhanBoThuoc.Attributes["onclick"] = "phanbothuoc();";
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                btnSaveCaSauAn.Visible = false;
                btnSaveThuoc.Visible = false;
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadThuoc();
        }

        public void LoadCaSauAn(int CaSauAn)
        {
            DataTable tblCSA = csCont.CaSauAn_GetByID(CaSauAn);
            if (tblCSA.Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "khongcodulieu", "alert('Không có dữ liệu');", true);
                btnSaveThuoc.Visible = false;
                btnSaveCaSauAn.Visible = false;
                return;
            }
            txtThoiDiem.Text = ((DateTime)tblCSA.Rows[0]["ThoiDiem"]).ToString("dd/MM/yyyy HH:mm:ss");
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            DateTime dtNgayChoAn = DateTime.Parse(txtThoiDiem.Text, ci);
            if (dtNgayChoAn < Config.NgayKhoaSo())
            {
                btnSaveThuoc.Enabled = false;
                btnSaveThuoc.CssClass = "buttondisable";
                btnSaveCaSauAn.Enabled = false;
                btnSaveCaSauAn.CssClass = "buttondisable";
            }
            else
            {
                btnSaveThuoc.Visible = true;
                btnSaveCaSauAn.Visible = true;
                if (Convert.ToBoolean(tblCSA.Rows[0]["Lock"]))
                {
                    btnSaveThuoc.Visible = false;
                    btnSaveCaSauAn.Visible = false;
                }
            }
        }

        protected void ddlRowStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadThuoc();
        }

        protected void ddlLoaiCa_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdLoaiCa.Value = ddlLoaiCa.SelectedValue;
            BindChuongThuoc();
        }

        protected void ddlThuoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DonViTinh = "";
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            decimal soluong = csCont.LoadVatTu_Ngay_ByID(int.Parse(ddlThuoc.SelectedValue), DateTime.Parse(txtThoiDiem.Text, ci), out DonViTinh);
            lblTon.Text = Config.ToXVal2(soluong, scale) + " " + DonViTinh;
            hdThuoc.Value = ddlThuoc.SelectedValue;
            BindChuongThuoc();
        }

        public void LoadThuoc()
        {
            DataTable tblThuoc = csCont.CaSauAn_GetThuoc(int.Parse(hdCaSauAn.Value), int.Parse(ddlRowStatus.SelectedValue));
            grvThuoc.DataSource = tblThuoc;
            grvThuoc.DataBind();
            if(tblThuoc.Rows.Count > 0)
            {
                tdChuong.Visible = true;
                if (hdLoaiCa.Value != "") ddlLoaiCa.SelectedValue = hdLoaiCa.Value;
                else ddlLoaiCa.SelectedValue = tblThuoc.Rows[0]["LoaiCa"].ToString();
                if (hdThuoc.Value != "") ddlThuoc.SelectedValue = hdThuoc.Value;
                else ddlThuoc.SelectedValue = tblThuoc.Rows[0]["Thuoc"].ToString();
                ddlThuoc_SelectedIndexChanged(null, null);
            }
            else
            {
                if (ddlRowStatus.SelectedValue == "0")
                {
                    tdChuong.Visible = false;
                }
                else
                {
                    tdChuong.Visible = true;
                    ddlThuoc_SelectedIndexChanged(null, null);
                }
            }
        }

        public void BindChuongThuoc()
        {
            if (ddlLoaiCa.SelectedValue == "0")
            {
                grvDanhSach.Visible = false;
            }
            else
            {
                grvDanhSach.Visible = true;
                decimal KhoiLuong = 0;
                int SoLuongCa = 0;
                int SoLuongTT = 0;
                DateTime NgayAn = DateTime.MinValue;
                DataTable tblChuong;
                if (ddlRowStatus.SelectedValue == "1")
                    tblChuong = csCont.CaSauAn_GetChuongByThuocByLoaiCa(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc.SelectedValue), int.Parse(ddlLoaiCa.SelectedValue), out KhoiLuong, out SoLuongCa, out SoLuongTT, out NgayAn);
                else
                    tblChuong = csCont.CaSauAn_GetChuongByThuocByLoaiCa_Delete(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc.SelectedValue), int.Parse(ddlLoaiCa.SelectedValue), out KhoiLuong, out SoLuongCa, out SoLuongTT, out NgayAn);
                int maxTest = 3;
                string s = "";
                int temp = 1;
                int countTest = 0;
                for (int i = 0; i < tblChuong.Rows.Count; i++)
                {
                    if (tblChuong.Rows[i]["KhoiLuong"] != DBNull.Value)
                    {
                        s = Convert.ToDecimal(tblChuong.Rows[i]["KhoiLuong"]).ToString("0.#####");
                        int vitriphancach = s.LastIndexOf(thapphan);
                        if (vitriphancach >= 0) temp = s.Length - vitriphancach - 1;
                        if (temp > ChuongScale) ChuongScale = temp;
                        countTest++;
                        if (countTest == maxTest) break;
                    }
                }

                grvDanhSach.DataSource = tblChuong;
                grvDanhSach.DataBind();
                txtKhoiLuongPhanBo.Text = txtKhoiLuong.Text = KhoiLuong.ToString("0.#####");
                txtSoCaAn.Text = Config.ToXVal2(SoLuongCa,0);
                hdNgayAn.Value = NgayAn.ToString("dd/MM/yyyy");
            }
        }

        protected void grvThuoc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                Label lblTenLoaiCa = (Label)(e.Row.FindControl("lblTenLoaiCa"));
                lblTenLoaiCa.Text = r["TenLoaiCa"].ToString();
                Label lblTenVatTu = (Label)(e.Row.FindControl("lblTenVatTu"));
                lblTenVatTu.Text = r["TenVatTu"].ToString();
                Label lblKhoiLuong = (Label)(e.Row.FindControl("lblKhoiLuong"));
                decimal KhoiLuong = Convert.ToDecimal(r["KhoiLuong"]);
                lblKhoiLuong.Text = Config.ToXVal2(KhoiLuong,scale);
                Label lblSoLuongCa = (Label)(e.Row.FindControl("lblSoLuongCa"));
                int SoLuongCa = Convert.ToInt32(r["SoLuongCa"]);
                lblSoLuongCa.Text = Config.ToXVal2(SoLuongCa, 0);
                Label lblSLTT = (Label)(e.Row.FindControl("lblSLTT"));
                int SLTT = Convert.ToInt32(r["SoLuongTT"]);
                lblSLTT.Text = Config.ToXVal2(SLTT, 0);
                Label lblSLG = (Label)(e.Row.FindControl("lblSLG"));
                int SLG = SoLuongCa - SLTT;
                lblSLG.Text = Config.ToXVal2(SLG, 0);

                Label lblKLG = (Label)(e.Row.FindControl("lblKLG"));
                if (SoLuongCa != 0 && r["KhoiLuongGiong"] != DBNull.Value)
                    lblKLG.Text = Config.ToXVal2(r["KhoiLuongGiong"], scale);
                else lblKLG.Text = Config.ToXVal2(0, scale);
                decimal KLG = decimal.Parse(lblKLG.Text);

                Label lblKLTT = (Label)(e.Row.FindControl("lblKLTT"));
                decimal KLTT = KhoiLuong - KLG;
                lblKLTT.Text = Config.ToXVal2(KLTT, scale);

                Button btnXemThayDoi = (Button)(e.Row.FindControl("btnXemThayDoi"));
                btnXemThayDoi.CommandArgument = r["ID"].ToString();
                btnXemThayDoi.CommandName = ddlRowStatus.SelectedValue;
                TongKhoiLuong += KhoiLuong;
                TongKLTT += KLTT;
                btnXemThayDoi.Attributes["style"] = "background-image:url('" + ModulePath + "images/log.gif');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;width:16px;";
                e.Row.Attributes["id"] = "row_" + btnXemThayDoi.ClientID;
                e.Row.Attributes["onclick"] = "setSelectedRow(this,'" + btnXemThayDoi.ClientID + "')";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTongKhoiLuong = (Label)(e.Row.FindControl("lblTongKhoiLuong"));
                Label lblTongKLTT = (Label)(e.Row.FindControl("lblTongKLTT"));
                Label lblTongKLG = (Label)(e.Row.FindControl("lblTongKLG"));
                lblTongKhoiLuong.Text = Config.ToXVal2(TongKhoiLuong, scale);
                lblTongKLTT.Text = Config.ToXVal2(TongKLTT, scale);
                TongKLG = TongKhoiLuong - TongKLTT;
                lblTongKLG.Text = Config.ToXVal2(TongKLG, scale);
            }
        }

        protected void grvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSTT = (Label)(e.Row.FindControl("lblSTT"));
                int t = e.Row.RowIndex + 1;
                lblSTT.Text = t.ToString();
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(e.Row.FindControl("chkChon"));
                TextBox txtKL = (TextBox)(e.Row.FindControl("txtKhoiLuong"));
                Label lblSoLuong = (Label)(e.Row.FindControl("lblSoLuong"));
                int SoLuongCa = Convert.ToInt32(r["SoLuong"]);
                lblSoLuong.Text = r["SoLuong"].ToString();
                Label lblChuong = (Label)(e.Row.FindControl("lblChuong"));
                lblChuong.Text = r["Chuong"].ToString();
                lblChuong.ToolTip = r["IDChuong"].ToString();

                Label lblSLTT = (Label)(e.Row.FindControl("lblSLTT"));
                int SLTT = Convert.ToInt32(r["SoLuongTT"]);
                lblSLTT.Text = Config.ToXVal2(SLTT, 0);
                Label lblSLG = (Label)(e.Row.FindControl("lblSLG"));
                int SLG = SoLuongCa - SLTT;
                lblSLG.Text = Config.ToXVal2(SLG, 0);

                chkChon.Attributes["onclick"] = "chon_click(event, this);";
                if (r["KhoiLuong"] != DBNull.Value && Convert.ToDecimal(r["KhoiLuong"]) != 0)
                {
                    //txtKL.Text = Config.ToXVal2(r["KhoiLuong"], ChuongScale);
                    txtKL.Text = Convert.ToDecimal(r["KhoiLuong"]).ToString("0.#####");
                }

                Label lblKLTT = (Label)(e.Row.FindControl("lblKLTT"));
                decimal KhoiLuong = decimal.Parse(txtKL.Text);
                decimal KLTT = KhoiLuong * SLTT / SoLuongCa;
                lblKLTT.Text = Config.ToXVal2(KLTT, ChuongScale);
                if (decimal.Parse(lblKLTT.Text) == 0) lblKLTT.Text = Config.ToXVal2(KLTT, ChuongScale + 1);
                if (decimal.Parse(lblKLTT.Text) == 0) lblKLTT.Text = "0";
                Label lblKLG = (Label)(e.Row.FindControl("lblKLG"));
                decimal KLG = KhoiLuong - KLTT;
                lblKLG.Text = Config.ToXVal2(KLG, ChuongScale);
                if (decimal.Parse(lblKLG.Text) == 0) lblKLG.Text = Config.ToXVal2(KLG, ChuongScale + 1);
                if (decimal.Parse(lblKLG.Text) == 0) lblKLG.Text = "0";
                Label lblTrungBinh = (Label)(e.Row.FindControl("lblTrungBinh"));
                decimal TrungBinh = KhoiLuong / SoLuongCa;
                lblTrungBinh.Text = Config.ToXVal2(TrungBinh, ChuongScale + 1);
                if (decimal.Parse(lblTrungBinh.Text) == 0) lblTrungBinh.Text = Config.ToXVal2(TrungBinh, ChuongScale + 2);

                HyperLink lnkDelKhoiLuong = (HyperLink)(e.Row.FindControl("lnkDelKhoiLuong"));
                lnkDelKhoiLuong.Attributes["onclick"] = "setVal('" + lnkDelKhoiLuong.ClientID + "',0);";
                lnkDelKhoiLuong.Attributes["style"] = "cursor:pointer;";

                TextBox txtGhiChu = (TextBox)(e.Row.FindControl("txtGhiChu"));
                Button btnSaveGhiChu = (Button)(e.Row.FindControl("btnSaveGhiChu"));
                txtGhiChu.Text = r["Note"].ToString();
                btnSaveGhiChu.Visible = true;
                btnSaveGhiChu.CommandArgument = e.Row.RowIndex.ToString() + ";" + r["IDChuong"].ToString();
                if ((r["Lock"] != DBNull.Value && Convert.ToBoolean(r["Lock"]) == true) || ddlRowStatus.SelectedValue == "0")
                    btnSaveGhiChu.Visible = false;
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    btnSaveGhiChu.Visible = false;
                    lnkDelKhoiLuong.Visible = false;
                }
                //if (KhoiLuong != 0)
                //{
                    cTongKhoiLuong += KhoiLuong;
                    cTongKLTT += decimal.Parse(lblKLTT.Text);
                    cTongSoLuong += SoLuongCa;
                    cTongSLTT += SLTT;
                //}
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtTongKhoiLuong = (TextBox)(e.Row.FindControl("txtTongKhoiLuong"));
                Label lblTongKLTT = (Label)(e.Row.FindControl("lblTongKLTT"));
                Label lblTongKLG = (Label)(e.Row.FindControl("lblTongKLG"));
                Label lblTongSoLuong = (Label)(e.Row.FindControl("lblTongSoLuong"));
                Label lblTongSLTT = (Label)(e.Row.FindControl("lblTongSLTT"));
                Label lblTongSLG = (Label)(e.Row.FindControl("lblTongSLG"));
                txtTongKhoiLuong.Text = Config.ToXVal2(cTongKhoiLuong, ChuongScale);
                lblTongKLTT.Text = Config.ToXVal2(cTongKLTT, ChuongScale);
                lblTongKLG.Text = Config.ToXVal2(cTongKhoiLuong - cTongKLTT, ChuongScale);
                lblTongSoLuong.Text = Config.ToXVal2(cTongSoLuong, 0);
                lblTongSLTT.Text = Config.ToXVal2(cTongSLTT, 0);
                lblTongSLG.Text = Config.ToXVal2(cTongSoLuong - cTongSLTT, 0);
            }
        }

        protected void btnXemThayDoi_Click(object sender, EventArgs e)
        {
            DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
            int CaSauAnThuocLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauAnThuocLichSuPage"], PortalId).TabID;
            Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauAnThuocLichSuPage, "", "status/" + ((Button)sender).CommandName, "IDCaSauAnThuoc/" + ((Button)sender).CommandArgument) + "','',1000,600);</script>", false);
        }

        protected void btnSaveCaSauAn_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                if (txtThoiDiem.Text == "")
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "ngaychoanrong", "alert('Bạn phải nhập ngày cho ăn!');", true);
                    return;
                }
                else if (!txtThoiDiem.Text.Contains(" 23:00:00"))
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "ngaychoansai", "alert('Giờ cho ăn phải là 23:00:00!');", true);
                    return;
                }
                DateTime dtNgayChoAn = DateTime.Parse(txtThoiDiem.Text, ci);
                if (dtNgayChoAn < Config.NgayKhoaSo())
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Ngày cho ăn không được trước ngày khóa sổ');", true);
                    return;
                }
                if (hdCaSauAn.Value != "")
                {
                    int res = csCont.CaSauAn_Update(int.Parse(hdCaSauAn.Value), dtNgayChoAn, UserId);
                    if (res == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "ngaykhonghople", "alert('Ngày cho ăn không hợp lệ!');", true);
                    }
                    else
                    {
                        UpdateThucAn(int.Parse(hdCaSauAn.Value));
                        UpdateThuoc(int.Parse(hdCaSauAn.Value));
                        LoadThuoc();
                        Page.ClientScript.RegisterStartupScript(typeof(string), "ngayhople", "alert('Đã cập nhật ngày cho ăn!');", true);
                    }
                }
                else
                {
                    int res = csCont.CaSauAn_ThemMoi(dtNgayChoAn, UserId, "");
                    if (res != 0)
                    {
                        hdCaSauAn.Value = res.ToString();
                        tblThuocChuong.Visible = true;
                        lnkThucAn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "casauan_edit_thucan", "csaid/" + hdCaSauAn.Value, "mid/" + this.ModuleId.ToString());
                        DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
                        int ThuocMultiPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauAnThuocMulti"], PortalId).TabID;
                        lnkThuocMulti.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(ThuocMultiPage, "", "csaid/" + hdCaSauAn.Value) + "','CaSauAn',900,600);";
                        //lnkThuocMulti.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "casauan_thuoc_multi", "csaid/" + hdCaSauAn.Value, "mid/" + this.ModuleId.ToString()) + "','CaSauAn',900,600);";
                        BindThuocControls(dtNgayChoAn);
                        ddlThuoc_SelectedIndexChanged(null, null);
                        if (ddlLoaiCa.SelectedValue == "1") scaleCT = scaleCT + 2;
                        Page.ClientScript.RegisterStartupScript(typeof(string), "initthapphan", "$('#txtThapPhan').val('" + scaleCT.ToString() + "');", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "ngaykhonghople", "alert('Ngày cho ăn không hợp lệ!');", true);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnSaveGhiChu_Click(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            string[] s = ((Button)sender).CommandArgument.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            int i = int.Parse(s[0]);
            int Chuong = int.Parse(s[1]);
            GridViewRow r = grvDanhSach.Rows[i];
            TextBox txtGhiChu = (TextBox)r.FindControl("txtGhiChu");
            csCont.InsertUpdateDeleteNote(DateTime.Parse(hdNgayAn.Value, ci), Chuong, txtGhiChu.Text, UserId);
            Page.ClientScript.RegisterStartupScript(typeof(string), "daluuxong", "alert('Đã lưu');", true);
        }

        protected void btnSaveThuoc_Click(object sender, EventArgs e)
        {
            int res;
            if (ddlThuoc.SelectedValue == "0")
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "chuacovattu", "alert('Chưa chọn thuốc');", true);
                return;
            }
            if (ddlLoaiCa.SelectedValue == "0")
            {
                string ress = csCont.CaSauAn_InsertUpdateThuoc_SS(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc.SelectedValue), decimal.Parse(txtKhoiLuong.Text), UserId);
                if (ress == "")
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "daluuxong", "alert('Đã thêm thành công');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "daluuxong", "alert('Một số loại cá không cho thuốc được:" + ress + "');", true);
                }
                LoadThuoc();
                return;
            }
            string StrPhanCachKhuChuong = "";
            string khuchuong = "";
            string currkhuchuong = "";
            string StrSoLuongChuong = "";
            string StrSoLuongChuongTT = "";
            string StrChuong = "";
            string StrKL = "";
            decimal testKL;
            int SoLuongCa = 0;
            int SoLuongTT = 0;
            int index = 0;

            foreach (GridViewRow r in grvDanhSach.Rows)
            {
                TextBox txtKL = (TextBox)(r.FindControl("txtKhoiLuong"));
                Label lblSoLuong = (Label)(r.FindControl("lblSoLuong"));
                Label lblSLTT = (Label)(r.FindControl("lblSLTT"));
                Label lblChuong = (Label)(r.FindControl("lblChuong"));
                testKL = -1;
                if (Decimal.TryParse(txtKL.Text, out testKL) && testKL > 0)
                {
                    StrSoLuongChuong += "@" + lblSoLuong.Text + "@";
                    StrSoLuongChuongTT += "@" + lblSLTT.Text + "@";
                    StrChuong += "@" + lblChuong.ToolTip + "@";
                    StrKL += "@" + txtKL.Text.Replace(",", ".") + "@";
                    SoLuongCa += int.Parse(lblSoLuong.Text);
                    SoLuongTT += int.Parse(lblSLTT.Text);
                    currkhuchuong = lblChuong.Text.Substring(0, 2);
                    if (currkhuchuong != khuchuong)
                    {
                        StrPhanCachKhuChuong += "@" + index + "@";
                        khuchuong = currkhuchuong;
                    }
                    index++;
                }
            }
            if (StrPhanCachKhuChuong != "") StrPhanCachKhuChuong = StrPhanCachKhuChuong.Substring(3) + "@" + index.ToString() + "@";
            res = csCont.CaSauAn_InsertUpdateThuoc(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc.SelectedValue), decimal.Parse(txtKhoiLuong.Text), int.Parse(ddlLoaiCa.SelectedValue), SoLuongCa, SoLuongTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL, StrPhanCachKhuChuong, UserId, true);
            if (res == 0)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "daluuxong", "alert('Lưu không thành công! Có thể thời điểm cho ăn hoặc khối lượng thuốc không hợp lệ');", true);
            }
            else
            {
                LoadThuoc();
            }
        }

        protected void UpdateThucAn(int CSAID)
        {
            decimal KhoiLuong = 0;
            string NguoiChoAn = "";
            DateTime NgayAn = DateTime.MinValue;

            string StrPhanCachKhuChuong = "";
            string khuchuong = "";
            string currkhuchuong = "";
            string StrSoLuongChuong = "";
            string StrSoLuongChuongTT = "";
            string StrChuong = "";
            string StrKL = "";
            int SoLuongCa = 0;
            int SoLuongTT = 0;
            int index = 0;

            DataTable tblThucAn = csCont.CaSauAn_GetThucAn(CSAID, 1);
            foreach (DataRow rThucAn in tblThucAn.Rows)
            {
                SoLuongCa = 0;
                SoLuongTT = 0;
                StrSoLuongChuong = "";
                StrSoLuongChuongTT = "";
                StrKL = "";
                StrChuong = "";
                StrPhanCachKhuChuong = "";
                currkhuchuong = "";
                khuchuong = "";

                DataTable tblChuong = csCont.CaSauAn_GetChuongByThucAnByLoaiCa(CSAID, Convert.ToInt32(rThucAn["ThucAn"]),
                    Convert.ToInt32(rThucAn["LoaiCa"]), out KhoiLuong, out SoLuongCa, out SoLuongTT, out NguoiChoAn, out NgayAn);
                SoLuongCa = 0;
                SoLuongTT = 0;
                index = 0;
                foreach (DataRow rChuong in tblChuong.Rows)
                {
                    if (rChuong["KhoiLuong"] != DBNull.Value && Convert.ToDecimal(rChuong["KhoiLuong"]) != 0)
                    {
                        StrSoLuongChuong += "@" + rChuong["SoLuong"].ToString() + "@";
                        StrSoLuongChuongTT += "@" + rChuong["SoLuongTT"].ToString() + "@";
                        StrChuong += "@" + rChuong["IDChuong"].ToString() + "@";
                        StrKL += "@" + Convert.ToDecimal(rChuong["KhoiLuong"]).ToString("0.#####").Replace(",", ".") + "@";
                        SoLuongCa += Convert.ToInt32(rChuong["SoLuong"]);
                        SoLuongTT += Convert.ToInt32(rChuong["SoLuongTT"]);
                        currkhuchuong = rChuong["Chuong"].ToString().Substring(0, 2);
                        if (currkhuchuong != khuchuong)
                        {
                            StrPhanCachKhuChuong += "@" + index + "@";
                            khuchuong = currkhuchuong;
                        }
                        index++;
                    }
                }

                if (StrPhanCachKhuChuong != "") StrPhanCachKhuChuong = StrPhanCachKhuChuong.Substring(3) + "@" + index.ToString() + "@";

                csCont.CaSauAn_InsertUpdateThucAn_NoCheck_ForUpdateHangLoat(CSAID, Convert.ToInt32(rThucAn["ThucAn"]),
                    KhoiLuong, Convert.ToInt32(rThucAn["LoaiCa"]), SoLuongCa, SoLuongTT, StrSoLuongChuong,
                    StrSoLuongChuongTT, StrChuong, StrKL, StrPhanCachKhuChuong, UserId);
            }
        }

        protected void UpdateThuoc(int CSAID)
        {
            decimal KhoiLuong = 0;
            DateTime NgayAn = DateTime.MinValue;

            string StrPhanCachKhuChuong = "";
            string khuchuong = "";
            string currkhuchuong = "";
            string StrSoLuongChuong = "";
            string StrSoLuongChuongTT = "";
            string StrChuong = "";
            string StrKL = "";
            int SoLuongCa = 0;
            int SoLuongTT = 0;
            int index = 0;

            DataTable tblThuoc = csCont.CaSauAn_GetThuoc(CSAID, 1);
            foreach (DataRow rThuoc in tblThuoc.Rows)
            {
                SoLuongCa = 0;
                SoLuongTT = 0;
                StrSoLuongChuong = "";
                StrSoLuongChuongTT = "";
                StrKL = "";
                StrChuong = "";
                StrPhanCachKhuChuong = "";
                currkhuchuong = "";
                khuchuong = "";

                DataTable tblChuong = csCont.CaSauAn_GetChuongByThuocByLoaiCa(CSAID, Convert.ToInt32(rThuoc["Thuoc"]),
                    Convert.ToInt32(rThuoc["LoaiCa"]), out KhoiLuong, out SoLuongCa, out SoLuongTT, out NgayAn);
                SoLuongCa = 0;
                SoLuongTT = 0;
                index = 0;
                foreach (DataRow rChuong in tblChuong.Rows)
                {
                    if (rChuong["KhoiLuong"] != DBNull.Value && Convert.ToDecimal(rChuong["KhoiLuong"]) != 0)
                    {
                        StrSoLuongChuong += "@" + rChuong["SoLuong"].ToString() + "@";
                        StrSoLuongChuongTT += "@" + rChuong["SoLuongTT"].ToString() + "@";
                        StrChuong += "@" + rChuong["IDChuong"].ToString() + "@";
                        StrKL += "@" + Convert.ToDecimal(rChuong["KhoiLuong"]).ToString("0.#####").Replace(",", ".") + "@";
                        SoLuongCa += Convert.ToInt32(rChuong["SoLuong"]);
                        SoLuongTT += Convert.ToInt32(rChuong["SoLuongTT"]);
                        currkhuchuong = rChuong["Chuong"].ToString().Substring(0, 2);
                        if (currkhuchuong != khuchuong)
                        {
                            StrPhanCachKhuChuong += "@" + index + "@";
                            khuchuong = currkhuchuong;
                        }
                        index++;
                    }
                }

                if (StrPhanCachKhuChuong != "") StrPhanCachKhuChuong = StrPhanCachKhuChuong.Substring(3) + "@" + index.ToString() + "@";

                csCont.CaSauAn_InsertUpdateThuoc_NoCheck_ForUpdateHangLoat(CSAID, Convert.ToInt32(rThuoc["Thuoc"]),
                    KhoiLuong, Convert.ToInt32(rThuoc["LoaiCa"]), SoLuongCa, SoLuongTT, StrSoLuongChuong,
                    StrSoLuongChuongTT, StrChuong, StrKL, StrPhanCachKhuChuong, UserId);
            }
        }
    }
}