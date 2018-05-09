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
    public partial class casauan_thuoc_multi : DotNetNuke.Entities.Modules.PortalModuleBase
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
        public string thapphan = ".";

        private void BindThuocControls(DateTime ThoiDiem)
        {
            DataTable tblThuoc = csCont.LoadVatTu_ConTonKho("TTY", ThoiDiem);
            ddlThuoc1.DataSource = tblThuoc;
            ddlThuoc1.DataValueField = "IDVatTu";
            ddlThuoc1.DataTextField = "TenVatTu";
            ddlThuoc1.DataBind();
            if (ddlThuoc1.Items.Count == 0) ddlThuoc1.Items.Add(new ListItem("", "0"));
            ddlThuoc2.DataSource = tblThuoc;
            ddlThuoc2.DataValueField = "IDVatTu";
            ddlThuoc2.DataTextField = "TenVatTu";
            ddlThuoc2.DataBind();
            if (ddlThuoc2.Items.Count == 0) ddlThuoc2.Items.Add(new ListItem("", "0"));
            ddlThuoc3.DataSource = tblThuoc;
            ddlThuoc3.DataValueField = "IDVatTu";
            ddlThuoc3.DataTextField = "TenVatTu";
            ddlThuoc3.DataBind();
            if (ddlThuoc3.Items.Count == 0) ddlThuoc3.Items.Add(new ListItem("", "0"));
            ddlThuoc4.DataSource = tblThuoc;
            ddlThuoc4.DataValueField = "IDVatTu";
            ddlThuoc4.DataTextField = "TenVatTu";
            ddlThuoc4.DataBind();
            if (ddlThuoc4.Items.Count == 0) ddlThuoc4.Items.Add(new ListItem("", "0"));
            ddlThuoc5.DataSource = tblThuoc;
            ddlThuoc5.DataValueField = "IDVatTu";
            ddlThuoc5.DataTextField = "TenVatTu";
            ddlThuoc5.DataBind();
            if (ddlThuoc5.Items.Count == 0) ddlThuoc5.Items.Add(new ListItem("", "0"));
            ddlThuoc6.DataSource = tblThuoc;
            ddlThuoc6.DataValueField = "IDVatTu";
            ddlThuoc6.DataTextField = "TenVatTu";
            ddlThuoc6.DataBind();
            if (ddlThuoc6.Items.Count == 0) ddlThuoc6.Items.Add(new ListItem("", "0"));
            ddlThuoc7.DataSource = tblThuoc;
            ddlThuoc7.DataValueField = "IDVatTu";
            ddlThuoc7.DataTextField = "TenVatTu";
            ddlThuoc7.DataBind();
            if (ddlThuoc7.Items.Count == 0) ddlThuoc7.Items.Add(new ListItem("", "0"));
            ddlThuoc8.DataSource = tblThuoc;
            ddlThuoc8.DataValueField = "IDVatTu";
            ddlThuoc8.DataTextField = "TenVatTu";
            ddlThuoc8.DataBind();
            if (ddlThuoc8.Items.Count == 0) ddlThuoc8.Items.Add(new ListItem("", "0"));
            ddlThuoc9.DataSource = tblThuoc;
            ddlThuoc9.DataValueField = "IDVatTu";
            ddlThuoc9.DataTextField = "TenVatTu";
            ddlThuoc9.DataBind();
            if (ddlThuoc9.Items.Count == 0) ddlThuoc9.Items.Add(new ListItem("", "0"));

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
                if (Request.QueryString["csaid"] == null)
                {
                    btnSaveThuoc.Visible = false;
                    return;
                }
                if (System.Threading.Thread.CurrentThread.CurrentCulture.ToString() == "vi-VN")
                    thapphan = ",";
                else thapphan = ".";
                scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_TTY_Scale"]);
                scaleCT = scale;
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/impromptu/jquery.js"));
                if (!Page.IsPostBack)
                {
                    hdCaSauAn.Value = Request.QueryString["csaid"];
                    DataTable tbl = csCont.CaSauAn_GetByID(int.Parse(hdCaSauAn.Value));
                    tblThuocChuong.Visible = true;
                    if (tbl.Rows.Count == 1) BindThuocControls(Convert.ToDateTime(tbl.Rows[0]["ThoiDiem"]));
                    ddlLoaiCa_SelectedIndexChanged(null, null);
                }

                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    btnSaveThuoc.Visible = false;
                    pnlCommand.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                btnSaveThuoc.Visible = false;
            }
        }

        protected void ddlLoaiCa_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdLoaiCa.Value = ddlLoaiCa.SelectedValue;
            BindChuong();
            if (ddlLoaiCa.SelectedValue == "1") scaleCT = scaleCT + 2;
            txtThapPhan.Text = scaleCT.ToString();
        }

        public void BindChuong()
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
                string NguoiChoAn = "";
                DateTime NgayAn = DateTime.MinValue;
                DataTable tblChuong;

                tblChuong = csCont.CaSauAn_GetChuongByLoaiCa(int.Parse(hdCaSauAn.Value), int.Parse(ddlLoaiCa.SelectedValue));

                grvDanhSach.DataSource = tblChuong;
                grvDanhSach.DataBind();
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
                cTongSoLuong += SoLuongCa;
                cTongSLTT += SLTT;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTongSoLuong = (Label)(e.Row.FindControl("lblTongSoLuong"));
                Label lblTongSLTT = (Label)(e.Row.FindControl("lblTongSLTT"));
                Label lblTongSLG = (Label)(e.Row.FindControl("lblTongSLG"));
                lblTongSoLuong.Text = Config.ToXVal2(cTongSoLuong, 0);
                lblTongSLTT.Text = Config.ToXVal2(cTongSLTT, 0);
                lblTongSLG.Text = Config.ToXVal2(cTongSoLuong - cTongSLTT, 0);
            }
        }

        protected void btnSaveThuoc_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            int res = 1;
            if (ddlLoaiCa.SelectedValue == "0")
            {
                string ress = "";
                if (txtKhoiLuong1.Text != "")
                {
                    ress += csCont.CaSauAn_CanInsertUpdateThuoc_SS(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc1.SelectedValue), decimal.Parse(txtKhoiLuong1.Text), !chkThemVao.Checked);
                }
                if (txtKhoiLuong2.Text != "")
                {
                    ress += csCont.CaSauAn_CanInsertUpdateThuoc_SS(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc2.SelectedValue), decimal.Parse(txtKhoiLuong2.Text), !chkThemVao.Checked);
                }
                if (txtKhoiLuong3.Text != "")
                {
                    ress += csCont.CaSauAn_CanInsertUpdateThuoc_SS(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc3.SelectedValue), decimal.Parse(txtKhoiLuong3.Text), !chkThemVao.Checked);
                }
                if (txtKhoiLuong4.Text != "")
                {
                    ress += csCont.CaSauAn_CanInsertUpdateThuoc_SS(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc4.SelectedValue), decimal.Parse(txtKhoiLuong4.Text), !chkThemVao.Checked);
                }
                if (txtKhoiLuong5.Text != "")
                {
                    ress += csCont.CaSauAn_CanInsertUpdateThuoc_SS(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc5.SelectedValue), decimal.Parse(txtKhoiLuong5.Text), !chkThemVao.Checked);
                }
                if (txtKhoiLuong6.Text != "")
                {
                    ress += csCont.CaSauAn_CanInsertUpdateThuoc_SS(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc6.SelectedValue), decimal.Parse(txtKhoiLuong6.Text), !chkThemVao.Checked);
                }
                if (txtKhoiLuong7.Text != "")
                {
                    ress += csCont.CaSauAn_CanInsertUpdateThuoc_SS(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc7.SelectedValue), decimal.Parse(txtKhoiLuong7.Text), !chkThemVao.Checked);
                }
                if (txtKhoiLuong8.Text != "")
                {
                    ress += csCont.CaSauAn_CanInsertUpdateThuoc_SS(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc8.SelectedValue), decimal.Parse(txtKhoiLuong8.Text), !chkThemVao.Checked);
                }
                if (txtKhoiLuong9.Text != "")
                {
                    ress += csCont.CaSauAn_CanInsertUpdateThuoc_SS(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc9.SelectedValue), decimal.Parse(txtKhoiLuong9.Text), !chkThemVao.Checked);
                }
                if (ress == "")
                {
                    if (txtKhoiLuong1.Text != "")
                    {
                        csCont.CaSauAn_InsertUpdateThuoc_SS_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc1.SelectedValue), decimal.Parse(txtKhoiLuong1.Text), UserId);
                    }
                    if (txtKhoiLuong2.Text != "")
                    {
                        csCont.CaSauAn_InsertUpdateThuoc_SS_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc2.SelectedValue), decimal.Parse(txtKhoiLuong2.Text), UserId);
                    }
                    if (txtKhoiLuong3.Text != "")
                    {
                        csCont.CaSauAn_InsertUpdateThuoc_SS_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc3.SelectedValue), decimal.Parse(txtKhoiLuong3.Text), UserId);
                    }
                    if (txtKhoiLuong4.Text != "")
                    {
                        csCont.CaSauAn_InsertUpdateThuoc_SS_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc4.SelectedValue), decimal.Parse(txtKhoiLuong4.Text), UserId);
                    }
                    if (txtKhoiLuong5.Text != "")
                    {
                        csCont.CaSauAn_InsertUpdateThuoc_SS_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc5.SelectedValue), decimal.Parse(txtKhoiLuong5.Text), UserId);
                    }
                    if (txtKhoiLuong6.Text != "")
                    {
                        csCont.CaSauAn_InsertUpdateThuoc_SS_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc6.SelectedValue), decimal.Parse(txtKhoiLuong6.Text), UserId);
                    }
                    if (txtKhoiLuong7.Text != "")
                    {
                        csCont.CaSauAn_InsertUpdateThuoc_SS_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc7.SelectedValue), decimal.Parse(txtKhoiLuong7.Text), UserId);
                    }
                    if (txtKhoiLuong8.Text != "")
                    {
                        csCont.CaSauAn_InsertUpdateThuoc_SS_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc8.SelectedValue), decimal.Parse(txtKhoiLuong8.Text), UserId);
                    }
                    if (txtKhoiLuong9.Text != "")
                    {
                        csCont.CaSauAn_InsertUpdateThuoc_SS_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc9.SelectedValue), decimal.Parse(txtKhoiLuong9.Text), UserId);
                    }
                    ddlThuoc1.SelectedIndex = 0;
                    txtKhoiLuong1.Text = "";
                    ddlThuoc2.SelectedIndex = 0;
                    txtKhoiLuong2.Text = "";
                    ddlThuoc3.SelectedIndex = 0;
                    txtKhoiLuong3.Text = "";
                    ddlThuoc4.SelectedIndex = 0;
                    txtKhoiLuong4.Text = "";
                    ddlThuoc5.SelectedIndex = 0;
                    txtKhoiLuong5.Text = "";
                    ddlThuoc6.SelectedIndex = 0;
                    txtKhoiLuong6.Text = "";
                    ddlThuoc7.SelectedIndex = 0;
                    txtKhoiLuong7.Text = "";
                    ddlThuoc8.SelectedIndex = 0;
                    txtKhoiLuong8.Text = "";
                    ddlThuoc9.SelectedIndex = 0;
                    txtKhoiLuong9.Text = "";
                    Page.ClientScript.RegisterStartupScript(typeof(string), "daluuxong", "alert('Đã thêm thành công');window.opener.finishEdit();", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "daluuxong", "alert('Một số loại cá không cho thuốc được:" + ress + "');window.opener.finishEdit();", true);
                }
                return;
            }
            string StrPhanCachKhuChuong = "";
            string khuchuong = "";
            string currkhuchuong = "";
            string StrSoLuongChuong = "";
            string StrSoLuongChuongTT = "";
            string StrChuong = "";
            int SoLuongCa = 0;
            int SoLuongTT = 0;
            int index = 0;
            foreach (GridViewRow r in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(r.FindControl("chkChon"));
                Label lblSoLuong = (Label)(r.FindControl("lblSoLuong"));
                Label lblSLTT = (Label)(r.FindControl("lblSLTT"));
                Label lblChuong = (Label)(r.FindControl("lblChuong"));
                if (chkChon.Checked)
                {
                    StrSoLuongChuong += "@" + lblSoLuong.Text + "@";
                    StrSoLuongChuongTT += "@" + lblSLTT.Text + "@";
                    StrChuong += "@" + lblChuong.ToolTip + "@";
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

            decimal KL1 = 0, KL2 = 0, KL3 = 0, KL4 = 0, KL5 = 0, KL6 = 0, KL7 = 0, KL8 = 0, KL9 = 0;
            string StrKL1 = "", StrKL2 = "", StrKL3 = "", StrKL4 = "", StrKL5 = "", StrKL6 = "", StrKL7 = "", StrKL8 = "", StrKL9 = "";
            decimal TongDoiChieu1 = 0, TongDoiChieu2 = 0, TongDoiChieu3 = 0, TongDoiChieu4 = 0, TongDoiChieu5 = 0, TongDoiChieu6 = 0, TongDoiChieu7 = 0, TongDoiChieu8 = 0, TongDoiChieu9 = 0;
            int h = 0;
            string f = "{0:0.";
            for (int i = 0; i < int.Parse(txtThapPhan.Text); i++)
            {
                f += "#";
            }
            f += "}";
            foreach (GridViewRow r in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(r.FindControl("chkChon"));
                Label lblSoLuong = (Label)(r.FindControl("lblSoLuong"));
                if (chkChon.Checked)
                {
                    if (txtKhoiLuong1.Text != "")
                    {
                        decimal KhoiLuong = decimal.Parse(txtKhoiLuong1.Text);
                        KL1 = KhoiLuong * decimal.Parse(lblSoLuong.Text) / Convert.ToDecimal(SoLuongCa);
                        string sKL1 = String.Format(f, KL1);
                        decimal dKL1 = Convert.ToDecimal(sKL1);
                        TongDoiChieu1 += dKL1;
                        if (h == index - 1)
                        {
                            if (TongDoiChieu1 != KhoiLuong)
                            {
                                decimal temp = dKL1 + KhoiLuong - TongDoiChieu1;
                                string sTemp = String.Format(f, temp);
                                decimal dTemp = Convert.ToDecimal(sTemp);
                                if (dTemp <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL1 += "@" + sTemp.Replace(',', '.') + "@";
                            }
                            else
                            {
                                if (dKL1 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL1 += "@" + sKL1.Replace(',', '.') + "@";
                            }
                        }
                        else
                        {
                            if (dKL1 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                            StrKL1 += "@" + sKL1.Replace(',', '.') + "@";
                        }
                    }
                    if (txtKhoiLuong2.Text != "")
                    {
                        decimal KhoiLuong = decimal.Parse(txtKhoiLuong2.Text);
                        KL2 = KhoiLuong * decimal.Parse(lblSoLuong.Text) / Convert.ToDecimal(SoLuongCa);
                        string sKL2 = String.Format(f, KL2);
                        decimal dKL2 = Convert.ToDecimal(sKL2);
                        TongDoiChieu2 += dKL2;
                        if (h == index - 1)
                        {
                            if (TongDoiChieu2 != KhoiLuong)
                            {
                                decimal temp = dKL2 + KhoiLuong - TongDoiChieu2;
                                string sTemp = String.Format(f, temp);
                                decimal dTemp = Convert.ToDecimal(sTemp);
                                if (dTemp <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL2 += "@" + sTemp.Replace(',', '.') + "@";
                            }
                            else
                            {
                                if (dKL2 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL2 += "@" + sKL2.Replace(',', '.') + "@";
                            }
                        }
                        else
                        {
                            if (dKL2 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                            StrKL2 += "@" + sKL2.Replace(',', '.') + "@";
                        }
                    }
                    if (txtKhoiLuong3.Text != "")
                    {
                        decimal KhoiLuong = decimal.Parse(txtKhoiLuong3.Text);
                        KL3 = KhoiLuong * decimal.Parse(lblSoLuong.Text) / Convert.ToDecimal(SoLuongCa);
                        string sKL3 = String.Format(f, KL3);
                        decimal dKL3 = Convert.ToDecimal(sKL3);
                        TongDoiChieu3 += dKL3;
                        if (h == index - 1)
                        {
                            if (TongDoiChieu3 != KhoiLuong)
                            {
                                decimal temp = dKL3 + KhoiLuong - TongDoiChieu3;
                                string sTemp = String.Format(f, temp);
                                decimal dTemp = Convert.ToDecimal(sTemp);
                                if (dTemp <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL3 += "@" + sTemp.Replace(',', '.') + "@";
                            }
                            else
                            {
                                if (dKL3 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL3 += "@" + sKL3.Replace(',', '.') + "@";
                            }
                        }
                        else
                        {
                            if (dKL3 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                            StrKL3 += "@" + sKL3.Replace(',', '.') + "@"; 
                        }
                    }
                    if (txtKhoiLuong4.Text != "")
                    {
                        decimal KhoiLuong = decimal.Parse(txtKhoiLuong4.Text);
                        KL4 = KhoiLuong * decimal.Parse(lblSoLuong.Text) / Convert.ToDecimal(SoLuongCa);
                        string sKL4 = String.Format(f, KL4);
                        decimal dKL4 = Convert.ToDecimal(sKL4);
                        TongDoiChieu4 += dKL4;
                        if (h == index - 1)
                        {
                            if (TongDoiChieu4 != KhoiLuong)
                            {
                                decimal temp = dKL4 + KhoiLuong - TongDoiChieu4;
                                string sTemp = String.Format(f, temp);
                                decimal dTemp = Convert.ToDecimal(sTemp);
                                if (dTemp <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL4 += "@" + sTemp.Replace(',', '.') + "@";
                            }
                            else
                            {
                                if (dKL4 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL4 += "@" + sKL4.Replace(',', '.') + "@";
                            }
                        }
                        else
                        {
                            if (dKL4 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                            StrKL4 += "@" + sKL4.Replace(',', '.') + "@";
                        }
                    }
                    if (txtKhoiLuong5.Text != "")
                    {
                        decimal KhoiLuong = decimal.Parse(txtKhoiLuong5.Text);
                        KL5 = KhoiLuong * decimal.Parse(lblSoLuong.Text) / Convert.ToDecimal(SoLuongCa);
                        string sKL5 = String.Format(f, KL5);
                        decimal dKL5 = Convert.ToDecimal(sKL5);
                        TongDoiChieu5 += dKL5;
                        if (h == index - 1)
                        {
                            if (TongDoiChieu5 != KhoiLuong)
                            {
                                decimal temp = dKL5 + KhoiLuong - TongDoiChieu5;
                                string sTemp = String.Format(f, temp);
                                decimal dTemp = Convert.ToDecimal(sTemp);
                                if (dTemp <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL5 += "@" + sTemp.Replace(',', '.') + "@";
                            }
                            else
                            {
                                if (dKL5 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL5 += "@" + sKL5.Replace(',', '.') + "@";
                            }
                        }
                        else
                        {
                            if (dKL5 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                            StrKL5 += "@" + sKL5.Replace(',', '.') + "@";
                        }
                    }
                    if (txtKhoiLuong6.Text != "")
                    {
                        decimal KhoiLuong = decimal.Parse(txtKhoiLuong6.Text);
                        KL6 = KhoiLuong * decimal.Parse(lblSoLuong.Text) / Convert.ToDecimal(SoLuongCa);
                        string sKL6 = String.Format(f, KL6);
                        decimal dKL6 = Convert.ToDecimal(sKL6);
                        TongDoiChieu6 += dKL6;
                        if (h == index - 1)
                        {
                            if (TongDoiChieu6 != KhoiLuong)
                            {
                                decimal temp = dKL6 + KhoiLuong - TongDoiChieu6;
                                string sTemp = String.Format(f, temp);
                                decimal dTemp = Convert.ToDecimal(sTemp);
                                if (dTemp <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL6 += "@" + sTemp.Replace(',', '.') + "@";
                            }
                            else
                            {
                                if (dKL6 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL6 += "@" + sKL6.Replace(',', '.') + "@";
                            }
                        }
                        else
                        {
                            if (dKL6 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                            StrKL6 += "@" + sKL6.Replace(',', '.') + "@";
                        }
                    }
                    if (txtKhoiLuong7.Text != "")
                    {
                        decimal KhoiLuong = decimal.Parse(txtKhoiLuong7.Text);
                        KL7 = KhoiLuong * decimal.Parse(lblSoLuong.Text) / Convert.ToDecimal(SoLuongCa);
                        string sKL7 = String.Format(f, KL7);
                        decimal dKL7 = Convert.ToDecimal(sKL7);
                        TongDoiChieu7 += dKL7;
                        if (h == index - 1)
                        {
                            if (TongDoiChieu7 != KhoiLuong)
                            {
                                decimal temp = dKL7 + KhoiLuong - TongDoiChieu7;
                                string sTemp = String.Format(f, temp);
                                decimal dTemp = Convert.ToDecimal(sTemp);
                                if (dTemp <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL7 += "@" + sTemp.Replace(',', '.') + "@";
                            }
                            else
                            {
                                if (dKL7 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL7 += "@" + sKL7.Replace(',', '.') + "@";
                            }
                        }
                        else
                        {
                            if (dKL7 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                            StrKL7 += "@" + sKL7.Replace(',', '.') + "@";
                        }
                    }
                    if (txtKhoiLuong8.Text != "")
                    {
                        decimal KhoiLuong = decimal.Parse(txtKhoiLuong8.Text);
                        KL8 = KhoiLuong * decimal.Parse(lblSoLuong.Text) / Convert.ToDecimal(SoLuongCa);
                        string sKL8 = String.Format(f, KL8);
                        decimal dKL8 = Convert.ToDecimal(sKL8);
                        TongDoiChieu8 += dKL8;
                        if (h == index - 1)
                        {
                            if (TongDoiChieu8 != KhoiLuong)
                            {
                                decimal temp = dKL8 + KhoiLuong - TongDoiChieu8;
                                string sTemp = String.Format(f, temp);
                                decimal dTemp = Convert.ToDecimal(sTemp);
                                if (dTemp <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL8 += "@" + sTemp.Replace(',', '.') + "@";
                            }
                            else
                            {
                                if (dKL8 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL8 += "@" + sKL8.Replace(',', '.') + "@";
                            }
                        }
                        else
                        {
                            if (dKL8 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                            StrKL8 += "@" + sKL8.Replace(',', '.') + "@";
                        }
                    }
                    if (txtKhoiLuong9.Text != "")
                    {
                        decimal KhoiLuong = decimal.Parse(txtKhoiLuong9.Text);
                        KL9 = KhoiLuong * decimal.Parse(lblSoLuong.Text) / Convert.ToDecimal(SoLuongCa);
                        string sKL9 = String.Format(f, KL9);
                        decimal dKL9 = Convert.ToDecimal(sKL9);
                        TongDoiChieu9 += dKL9;
                        if (h == index - 1)
                        {
                            if (TongDoiChieu9 != KhoiLuong)
                            {
                                decimal temp = dKL9 + KhoiLuong - TongDoiChieu9;
                                string sTemp = String.Format(f, temp);
                                decimal dTemp = Convert.ToDecimal(sTemp);
                                if (dTemp <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL9 += "@" + sTemp.Replace(',', '.') + "@";
                            }
                            else
                            {
                                if (dKL9 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                                StrKL9 += "@" + sKL9.Replace(',', '.') + "@";
                            }
                        }
                        else
                        {
                            if (dKL9 <= 0) { lblMessage.Text = "Có chuồng thuốc không hợp lệ. Đề nghị tăng số chữ số thập phân."; return; }
                            StrKL9 += "@" + sKL9.Replace(',', '.') + "@";
                        }
                    }
                    h++;
                }
            }
            string sRes = "";
            if (txtKhoiLuong1.Text != "")
            {
                res = csCont.CaSauAn_CanInsertUpdateThuoc(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc1.SelectedValue), decimal.Parse(txtKhoiLuong1.Text), int.Parse(ddlLoaiCa.SelectedValue), !chkThemVao.Checked, StrChuong);
                if (res == 0) sRes += ddlThuoc1.SelectedValue + ", ";
                else if (res == -1) sRes += ddlThuoc1.SelectedValue + ": đã nhập, ";
            }
            if (txtKhoiLuong2.Text != "")
            {
                res = csCont.CaSauAn_CanInsertUpdateThuoc(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc2.SelectedValue), decimal.Parse(txtKhoiLuong2.Text), int.Parse(ddlLoaiCa.SelectedValue), !chkThemVao.Checked, StrChuong);
                if (res == 0) sRes += ddlThuoc2.SelectedValue + ", ";
                else if (res == -1) sRes += ddlThuoc2.SelectedValue + ": đã nhập, ";
            }
            if (txtKhoiLuong3.Text != "")
            {
                res = csCont.CaSauAn_CanInsertUpdateThuoc(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc3.SelectedValue), decimal.Parse(txtKhoiLuong3.Text), int.Parse(ddlLoaiCa.SelectedValue), !chkThemVao.Checked, StrChuong);
                if (res == 0) sRes += ddlThuoc3.SelectedValue + ", ";
                else if (res == -1) sRes += ddlThuoc3.SelectedValue + ": đã nhập, ";
            }
            if (txtKhoiLuong4.Text != "")
            {
                res = csCont.CaSauAn_CanInsertUpdateThuoc(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc4.SelectedValue), decimal.Parse(txtKhoiLuong4.Text), int.Parse(ddlLoaiCa.SelectedValue), !chkThemVao.Checked, StrChuong);
                if (res == 0) sRes += ddlThuoc4.SelectedValue + ", ";
                else if (res == -1) sRes += ddlThuoc4.SelectedValue + ": đã nhập, ";
            }
            if (txtKhoiLuong5.Text != "")
            {
                res = csCont.CaSauAn_CanInsertUpdateThuoc(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc5.SelectedValue), decimal.Parse(txtKhoiLuong5.Text), int.Parse(ddlLoaiCa.SelectedValue), !chkThemVao.Checked, StrChuong);
                if (res == 0) sRes += ddlThuoc5.SelectedValue + ", ";
                else if (res == -1) sRes += ddlThuoc5.SelectedValue + ": đã nhập, ";
            }
            if (txtKhoiLuong6.Text != "")
            {
                res = csCont.CaSauAn_CanInsertUpdateThuoc(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc6.SelectedValue), decimal.Parse(txtKhoiLuong6.Text), int.Parse(ddlLoaiCa.SelectedValue), !chkThemVao.Checked, StrChuong);
                if (res == 0) sRes += ddlThuoc6.SelectedValue + ", ";
                else if (res == -1) sRes += ddlThuoc6.SelectedValue + ": đã nhập, ";
            }
            if (txtKhoiLuong7.Text != "")
            {
                res = csCont.CaSauAn_CanInsertUpdateThuoc(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc7.SelectedValue), decimal.Parse(txtKhoiLuong7.Text), int.Parse(ddlLoaiCa.SelectedValue), !chkThemVao.Checked, StrChuong);
                if (res == 0) sRes += ddlThuoc7.SelectedValue + ", ";
                else if (res == -1) sRes += ddlThuoc7.SelectedValue + ": đã nhập, ";
            }
            if (txtKhoiLuong8.Text != "")
            {
                res = csCont.CaSauAn_CanInsertUpdateThuoc(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc8.SelectedValue), decimal.Parse(txtKhoiLuong8.Text), int.Parse(ddlLoaiCa.SelectedValue), !chkThemVao.Checked, StrChuong);
                if (res == 0) sRes += ddlThuoc8.SelectedValue + ", ";
                else if (res == -1) sRes += ddlThuoc8.SelectedValue + ": đã nhập, ";
            }
            if (txtKhoiLuong9.Text != "")
            {
                res = csCont.CaSauAn_CanInsertUpdateThuoc(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc9.SelectedValue), decimal.Parse(txtKhoiLuong9.Text), int.Parse(ddlLoaiCa.SelectedValue), !chkThemVao.Checked, StrChuong);
                if (res == 0) sRes += ddlThuoc9.SelectedValue + ", ";
                else if (res == -1) sRes += ddlThuoc9.SelectedValue + ": đã nhập, ";
            }
            if (sRes != "")
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "daluuxong", "alert('Có một số loại thuốc không xử lý được: " + sRes + "');window.opener.finishEdit();", true);
            }
            else
            {
                if (txtKhoiLuong1.Text != "")
                {
                    csCont.CaSauAn_InsertUpdateThuoc_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc1.SelectedValue), decimal.Parse(txtKhoiLuong1.Text), int.Parse(ddlLoaiCa.SelectedValue), SoLuongCa, SoLuongTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL1, StrPhanCachKhuChuong, UserId, !chkThemVao.Checked);
                }
                if (txtKhoiLuong2.Text != "")
                {
                    csCont.CaSauAn_InsertUpdateThuoc_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc2.SelectedValue), decimal.Parse(txtKhoiLuong2.Text), int.Parse(ddlLoaiCa.SelectedValue), SoLuongCa, SoLuongTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL2, StrPhanCachKhuChuong, UserId, !chkThemVao.Checked);
                }
                if (txtKhoiLuong3.Text != "")
                {
                    csCont.CaSauAn_InsertUpdateThuoc_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc3.SelectedValue), decimal.Parse(txtKhoiLuong3.Text), int.Parse(ddlLoaiCa.SelectedValue), SoLuongCa, SoLuongTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL3, StrPhanCachKhuChuong, UserId, !chkThemVao.Checked);
                }
                if (txtKhoiLuong4.Text != "")
                {
                    csCont.CaSauAn_InsertUpdateThuoc_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc4.SelectedValue), decimal.Parse(txtKhoiLuong4.Text), int.Parse(ddlLoaiCa.SelectedValue), SoLuongCa, SoLuongTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL4, StrPhanCachKhuChuong, UserId, !chkThemVao.Checked);
                }
                if (txtKhoiLuong5.Text != "")
                {
                    csCont.CaSauAn_InsertUpdateThuoc_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc5.SelectedValue), decimal.Parse(txtKhoiLuong5.Text), int.Parse(ddlLoaiCa.SelectedValue), SoLuongCa, SoLuongTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL5, StrPhanCachKhuChuong, UserId, !chkThemVao.Checked);
                }
                if (txtKhoiLuong6.Text != "")
                {
                    csCont.CaSauAn_InsertUpdateThuoc_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc6.SelectedValue), decimal.Parse(txtKhoiLuong6.Text), int.Parse(ddlLoaiCa.SelectedValue), SoLuongCa, SoLuongTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL6, StrPhanCachKhuChuong, UserId, !chkThemVao.Checked);
                }
                if (txtKhoiLuong7.Text != "")
                {
                    csCont.CaSauAn_InsertUpdateThuoc_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc7.SelectedValue), decimal.Parse(txtKhoiLuong7.Text), int.Parse(ddlLoaiCa.SelectedValue), SoLuongCa, SoLuongTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL7, StrPhanCachKhuChuong, UserId, !chkThemVao.Checked);
                }
                if (txtKhoiLuong8.Text != "")
                {
                    csCont.CaSauAn_InsertUpdateThuoc_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc8.SelectedValue), decimal.Parse(txtKhoiLuong8.Text), int.Parse(ddlLoaiCa.SelectedValue), SoLuongCa, SoLuongTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL8, StrPhanCachKhuChuong, UserId, !chkThemVao.Checked);
                }
                if (txtKhoiLuong9.Text != "")
                {
                    csCont.CaSauAn_InsertUpdateThuoc_NoCheck(int.Parse(hdCaSauAn.Value), int.Parse(ddlThuoc9.SelectedValue), decimal.Parse(txtKhoiLuong9.Text), int.Parse(ddlLoaiCa.SelectedValue), SoLuongCa, SoLuongTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL9, StrPhanCachKhuChuong, UserId, !chkThemVao.Checked);
                }

                ddlThuoc1.SelectedIndex = 0;
                txtKhoiLuong1.Text = "";
                ddlThuoc2.SelectedIndex = 0;
                txtKhoiLuong2.Text = "";
                ddlThuoc3.SelectedIndex = 0;
                txtKhoiLuong3.Text = "";
                ddlThuoc4.SelectedIndex = 0;
                txtKhoiLuong4.Text = "";
                ddlThuoc5.SelectedIndex = 0;
                txtKhoiLuong5.Text = "";
                ddlThuoc6.SelectedIndex = 0;
                txtKhoiLuong6.Text = "";
                ddlThuoc7.SelectedIndex = 0;
                txtKhoiLuong7.Text = "";
                ddlThuoc8.SelectedIndex = 0;
                txtKhoiLuong8.Text = "";
                ddlThuoc9.SelectedIndex = 0;
                txtKhoiLuong9.Text = "";
                Page.ClientScript.RegisterStartupScript(typeof(string), "daluuxong", "alert('Đã lưu xong!');window.opener.finishEdit();", true);
            }
        }
    }
}