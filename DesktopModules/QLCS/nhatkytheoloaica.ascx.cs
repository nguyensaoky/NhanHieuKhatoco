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
using System.Data.SqlClient;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using DotNetNuke.Framework.Providers;

namespace DotNetNuke.Modules.QLCS
{
    public partial class nhatkytheoloaica : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");

        int scaleTA = 0;
        int scaleTTY = 0;

        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");

            txtTuNgay.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");

            ddlScaleTA.SelectedValue = ConfigurationManager.AppSettings["QLCS_VatTu_TA_Scale"];
            ddlScaleTTY.SelectedValue = ConfigurationManager.AppSettings["QLCS_VatTu_TTY_Scale"];

            DataTable tblLoaiCa = new DataTable();
            tblLoaiCa = csCont.LoadLoaiCa(1);
            lstLoaiCa.DataSource = tblLoaiCa;
            lstLoaiCa.DataTextField = "TenLoaiCa";
            lstLoaiCa.DataValueField = "IDLoaiCa";
            lstLoaiCa.DataBind();

            Config.LoadKhuChuong(lstKhuChuong);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
            try
            {
                scaleTA = int.Parse(ddlScaleTA.SelectedValue);
                scaleTTY = int.Parse(ddlScaleTTY.SelectedValue);
                if (!Page.IsPostBack)
                {
                    BindControls();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string[] aLoaiCa = Config.GetSelectedTexts(lstLoaiCa).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string filename = "NKLC";
                string tieude = "";
                string strSQL = "QLCS_BCTK_NhatKy_TheoLoaiCa_new_KhuChuong";
                if (ddlChuan.SelectedValue == "1")
                {
                    strSQL = "QLCS_BCTK_NhatKy_TheoLoaiCa_new_KhuChuong_ChuanCu";
                }
                SqlParameter[] param = new SqlParameter[9];
                if (txtTuNgay.Text == "")
                {
                    txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtDenNgay.Text == "")
                {
                    txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@StrLoaiCa", Config.GetSelectedValues_At(lstLoaiCa));
                param[1] = new SqlParameter("@StrKhuChuong", Config.GetSelectedValues_At(lstKhuChuong));
                param[2] = new SqlParameter("@TuNgay", DateTime.Parse(txtTuNgay.Text, ci));
                param[3] = new SqlParameter("@DenNgay", DateTime.Parse(txtDenNgay.Text, ci));
                param[4] = new SqlParameter("@OrderBy", ddlOrderBy.SelectedValue);
                param[5] = new SqlParameter("@ScaleTA", int.Parse(ddlScaleTA.SelectedValue));
                param[6] = new SqlParameter("@ScaleTTY", int.Parse(ddlScaleTTY.SelectedValue));
                param[7] = new SqlParameter("@PhanLoai", int.Parse(ddlPhanLoai.SelectedValue));
                param[8] = new SqlParameter("@ShowAll", chkShowAll.Checked);
                filename += txtTuNgay.Text + "__" + txtDenNgay.Text + ".xls";
                filename = filename.Replace("/", "_").Replace(" ", "");
                tieude += "<b>NHẬT KÝ CHĂN NUÔI THEO LOẠI CÁ " + Config.GetSelectedTexts(lstLoaiCa) + " - TỪ NGÀY " + txtTuNgay.Text + "  ĐẾN NGÀY " + txtDenNgay.Text + "</b>";

                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt;

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                StringBuilder sb = new StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table border='1'>");

                if (ddlOrderBy.SelectedValue == "Ngay")
                {
                    decimal SupTotalThucAn = 0; decimal SupTotalThucAnThua = 0; decimal SupTotalBST = 0;
                    int SupTotalChuyenDen = 0; int SupTotalChuyenDi = 0; int SupTotalChet = 0; int SupTotalLoaiThai = 0; int SupTotalGietMo = 0; int SupTotalBan = 0;
                    sb.Append(@"<tr><th rowspan=2 align='center'>Ngày</th><th rowspan=2 align='center'>Chuồng</th><th rowspan=2 align='center'>Số cá<br/>đầu kỳ</th><th rowspan='2' align='center'>Thức ăn</th><th rowspan='2' align='center'>Thức ăn<br/>thừa</th><th rowspan=2 align='center'>Thuốc</th><th colspan='6' align='center'>Biến động đàn</th><th rowspan=2 align='center'>Ghi chú</th></tr>
                        <tr><th align='center'>Nhập<br/>mới</th><th align='center'>Chuyển<br/>chuồng</th><th align='center'>Chết</th><th align='center'>Loại<br/>thải</th><th align='center'>Giết<br/>mổ</th><th align='center'>Bán</th></tr></thead><tbody>");
                    for (int j = 0; j < aLoaiCa.Length; j++)
                    {
                        DateTime currThoiDiem = DateTime.MinValue;
                        int TongSoChuong = 0; int TongSoCa = 0; decimal TongThucAn = 0; decimal TongThucAnThua = 0; decimal TongBST = 0; int TongChuyenDen = 0; int TongChuyenDi = 0; int TongChet = 0; int TongLoaiThai = 0; int TongGietMo = 0; int TongBan = 0;
                        decimal TotalThucAn = 0; decimal TotalThucAnThua = 0; decimal TotalBST = 0;
                        int TotalChuyenDen = 0; int TotalChuyenDi = 0; int TotalChet = 0; int TotalLoaiThai = 0; int TotalGietMo = 0; int TotalBan = 0;
                        dt = ds.Tables[j];
                        sb.Append("<tr><td align='center'><b><span style='color:#f00;'>" + aLoaiCa[j] + "</span></b></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow r = dt.Rows[i];
                            if (currThoiDiem != Convert.ToDateTime(r["ThoiDiem"]) && currThoiDiem != DateTime.MinValue)
                            {
                                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoChuong, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongSoCa, 0) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'><b>" + Config.ToXVal2(TongThucAn, scaleTA) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'><b>" + Config.ToXVal2(TongThucAnThua, scaleTA) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'><b>" + Config.ToXVal2(TongBST, scaleTTY) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "<b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "<b></td><td></td></tr>");

                                TotalThucAn += TongThucAn;
                                TotalThucAnThua += TongThucAnThua;
                                TotalBST += TongBST;
                                TotalChuyenDen += TongChuyenDen;
                                TotalChuyenDi += TongChuyenDi;
                                TotalChet += TongChet;
                                TotalLoaiThai += TongLoaiThai;
                                TotalGietMo += TongGietMo;
                                TotalBan += TongBan;

                                TongSoChuong = 0;
                                TongSoCa = 0;
                                TongThucAn = 0;
                                TongThucAnThua = 0;
                                TongBST = 0;
                                TongChuyenDen = 0;
                                TongChuyenDi = 0;
                                TongChet = 0;
                                TongLoaiThai = 0;
                                TongGietMo = 0;
                                TongBan = 0;
                            }
                            currThoiDiem = Convert.ToDateTime(r["ThoiDiem"]);
                            sb.Append("<tr><td align='center'>" + Convert.ToDateTime(r["ThoiDiem"]).ToString("dd/MM/yyyy") + "</td><td>" + r["TenChuong"].ToString() + "</td><td align='right'>" + r["SoCa"].ToString() + "</td><td align='left'>" + r["ThucAn"].ToString() + "</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(r["Thua"], scaleTA) + "</td><td align='left'>" + r["BST"].ToString() + "</td><td align='right'>" + r["ChuyenDen"].ToString() + "</td><td align='right'>" + r["ChuyenDi"].ToString() + "</td><td align='right'>" + r["Chet"].ToString() + "</td><td align='right'>" + r["LoaiThai"].ToString() + "</td><td align='right'>" + r["GietMo"].ToString() + "</td><td align='right'>" + r["Ban"].ToString() + "</td><td align='left'>" + r["Note"].ToString());
                            if (r["LyDoChet"].ToString() != "") sb.Append(" (Chết/loại thải: " + r["LyDoChet"].ToString() + ")");
                            sb.Append("</td></tr>");
                            TongSoChuong++;
                            TongSoCa += Convert.ToInt32(r["SoCa"]);
                            TongThucAn += r["TongThucAn"] != DBNull.Value ? Convert.ToDecimal(r["TongThucAn"]) : 0;
                            TongThucAnThua += r["Thua"] != DBNull.Value ? Convert.ToDecimal(r["Thua"]) : 0;
                            TongBST += r["TongBST"] != DBNull.Value ? Convert.ToDecimal(r["TongBST"]) : 0;
                            TongChuyenDen += r["ChuyenDen"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDen"]) : 0;
                            TongChuyenDi += r["ChuyenDi"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDi"]) : 0;
                            TongChet += r["Chet"] != DBNull.Value ? Convert.ToInt32(r["Chet"]) : 0;
                            TongLoaiThai += r["LoaiThai"] != DBNull.Value ? Convert.ToInt32(r["LoaiThai"]) : 0;
                            TongGietMo += r["GietMo"] != DBNull.Value ? Convert.ToInt32(r["GietMo"]) : 0;
                            TongBan += r["Ban"] != DBNull.Value ? Convert.ToInt32(r["Ban"]) : 0;
                        }
                        if (currThoiDiem != DateTime.MinValue)
                        {
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoChuong, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongSoCa, 0) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'><b>" + Config.ToXVal2(TongThucAn, scaleTA) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'><b>" + Config.ToXVal2(TongThucAnThua, scaleTA) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'><b>" + Config.ToXVal2(TongBST, scaleTTY) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "<b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "<b></td><td></td></tr>");
                            TotalThucAn += TongThucAn;
                            TotalThucAnThua += TongThucAnThua;
                            TotalBST += TongBST;
                            TotalChuyenDen += TongChuyenDen;
                            TotalChuyenDi += TongChuyenDi;
                            TotalChet += TongChet;
                            TotalLoaiThai += TongLoaiThai;
                            TotalGietMo += TongGietMo;
                            TotalBan += TongBan;
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'><b>Tổng cộng</b></td><td></td><td></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'><b>" + Config.ToXVal2(TotalThucAn, scaleTA) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'><b>" + Config.ToXVal2(TotalThucAnThua, scaleTA) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'><b>" + Config.ToXVal2(TotalBST, scaleTTY) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDen, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDi, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalBan, 0) + "</b></td><td></td></tr>");
                            SupTotalThucAn += Math.Round(TotalThucAn, scaleTA);
                            SupTotalThucAnThua += Math.Round(TotalThucAnThua, scaleTA);
                            SupTotalBST += Math.Round(TotalBST, scaleTTY);
                            SupTotalChuyenDen += TotalChuyenDen;
                            SupTotalChuyenDi += TotalChuyenDi;
                            SupTotalChet += TotalChet;
                            SupTotalLoaiThai += TotalLoaiThai;
                            SupTotalGietMo += TotalGietMo;
                            SupTotalBan += TotalBan;
                        }
                    }
                    sb.Append("<tr style='color:#F00;font-weight:bold;'><td align='center'>TỔNG CÁC LOẠI CÁ</td><td></td><td></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(SupTotalThucAn, scaleTA) + "</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(SupTotalThucAnThua, scaleTA) + "</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(SupTotalBST, scaleTTY) + "</td><td align='right'>" + Config.ToXVal2(SupTotalChuyenDen, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalChuyenDi, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalChet, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalLoaiThai, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalGietMo, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalBan, 0) + "</td><td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlOrderBy.SelectedValue == "Chuong")
                {
                    decimal SupTotalThucAn = 0; decimal SupTotalThucAnThua = 0; decimal SupTotalBST = 0;
                    int SupTotalChuyenDen = 0; int SupTotalChuyenDi = 0; int SupTotalChet = 0; int SupTotalLoaiThai = 0; int SupTotalGietMo = 0; int SupTotalBan = 0;
                    sb.Append(@"<tr><th rowspan=2 align='center'>Chuồng</th><th rowspan=2 align='center'>Ngày</th><th rowspan=2 align='center'>Số cá<br/>đầu kỳ</th><th rowspan='2' align='center'>Thức ăn</th><th rowspan='2' align='center'>Thức ăn<br/>thừa</th><th rowspan=2 align='center'>Thuốc</th><th colspan='6' align='center'>Biến động đàn</th><th rowspan=2 align='center'>Ghi chú</th></tr>
                        <tr><th align='center'>Nhập<br/>mới</th><th align='center'>Chuyển<br/>chuồng</th><th align='center'>Chết</th><th align='center'>Loại<br/>thải</th><th align='center'>Giết<br/>mổ</th><th align='center'>Bán</th></tr></thead><tbody>");
                    for (int j = 0; j < aLoaiCa.Length; j++)
                    {
                        string currTenChuong = "";
                        int TongSoNgay = 0; decimal TongThucAn = 0; decimal TongThucAnThua = 0; decimal TongBST = 0; int TongChuyenDen = 0; int TongChuyenDi = 0; int TongChet = 0; int TongLoaiThai = 0; int TongGietMo = 0; int TongBan = 0;
                        decimal TotalThucAn = 0; decimal TotalThucAnThua = 0; decimal TotalBST = 0;
                        int TotalChuyenDen = 0; int TotalChuyenDi = 0; int TotalChet = 0; int TotalLoaiThai = 0; int TotalGietMo = 0; int TotalBan = 0;
                        int TotalSoChuong = 0;
                        dt = ds.Tables[j];
                        sb.Append("<tr><td align='center'><b><span style='color:#f00;'>" + aLoaiCa[j] + "</span></b></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow r = dt.Rows[i];
                            if (currTenChuong != r["TenChuong"].ToString() && currTenChuong != "")
                            {
                                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoNgay, 0) + "</b></td><td></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'><b>" + Config.ToXVal2(TongThucAn, scaleTA) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'><b>" + Config.ToXVal2(TongThucAnThua, scaleTA) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'><b>" + Config.ToXVal2(TongBST, scaleTTY) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "</b></b></td><td></td></tr>");

                                TotalThucAn += TongThucAn;
                                TotalThucAnThua += TongThucAnThua;
                                TotalBST += TongBST;
                                TotalChuyenDen += TongChuyenDen;
                                TotalChuyenDi += TongChuyenDi;
                                TotalChet += TongChet;
                                TotalLoaiThai += TongLoaiThai;
                                TotalGietMo += TongGietMo;
                                TotalBan += TongBan;
                                TotalSoChuong++;

                                TongSoNgay = 0;
                                TongThucAn = 0;
                                TongThucAnThua = 0;
                                TongBST = 0;
                                TongChuyenDen = 0;
                                TongChuyenDi = 0;
                                TongChet = 0;
                                TongLoaiThai = 0;
                                TongGietMo = 0;
                                TongBan = 0;
                            }
                            currTenChuong = r["TenChuong"].ToString();
                            sb.Append("<tr><td>" + r["TenChuong"].ToString() + "</td><td align='center'>" + Convert.ToDateTime(r["ThoiDiem"]).ToString("dd/MM/yyyy") + "</td><td align='right'>" + r["SoCa"].ToString() + "</td><td align='left'>" + r["ThucAn"].ToString() + "</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(r["Thua"], scaleTA) + "</td><td align='left'>" + r["BST"].ToString() + "</td><td align='right'>" + r["ChuyenDen"].ToString() + "</td><td align='right'>" + r["ChuyenDi"].ToString() + "</td><td align='right'>" + r["Chet"].ToString() + "</td><td align='right'>" + r["LoaiThai"].ToString() + "</td><td align='right'>" + r["GietMo"].ToString() + "</td><td align='right'>" + r["Ban"].ToString() + "</td><td align='left'>" + r["Note"].ToString());
                            if (r["LyDoChet"].ToString() != "") sb.Append(" (Chết/loại thải: " + r["LyDoChet"].ToString() + ")");
                            sb.Append("</td></tr>");
                            TongSoNgay++;
                            TongThucAn += r["TongThucAn"] != DBNull.Value ? Convert.ToDecimal(r["TongThucAn"]) : 0;
                            TongThucAnThua += r["Thua"] != DBNull.Value ? Convert.ToDecimal(r["Thua"]) : 0;
                            TongBST += r["TongBST"] != DBNull.Value ? Convert.ToDecimal(r["TongBST"]) : 0;
                            TongChuyenDen += r["ChuyenDen"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDen"]) : 0;
                            TongChuyenDi += r["ChuyenDi"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDi"]) : 0;
                            TongChet += r["Chet"] != DBNull.Value ? Convert.ToInt32(r["Chet"]) : 0;
                            TongLoaiThai += r["LoaiThai"] != DBNull.Value ? Convert.ToInt32(r["LoaiThai"]) : 0;
                            TongGietMo += r["GietMo"] != DBNull.Value ? Convert.ToInt32(r["GietMo"]) : 0;
                            TongBan += r["Ban"] != DBNull.Value ? Convert.ToInt32(r["Ban"]) : 0;
                        }
                        if (currTenChuong != "")
                        {
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoNgay, 0) + "</b></td><td></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'><b>" + Config.ToXVal2(TongThucAn, scaleTA) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'><b>" + Config.ToXVal2(TongThucAnThua, scaleTA) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'><b>" + Config.ToXVal2(TongBST, scaleTTY) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "</b></b></td><td></td></tr>");
                            TotalThucAn += TongThucAn;
                            TotalThucAnThua += TongThucAnThua;
                            TotalBST += TongBST;
                            TotalChuyenDen += TongChuyenDen;
                            TotalChuyenDi += TongChuyenDi;
                            TotalChet += TongChet;
                            TotalLoaiThai += TongLoaiThai;
                            TotalGietMo += TongGietMo;
                            TotalBan += TongBan;
                            TotalSoChuong++;
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'><b>Tổng cộng<br/>(" + Config.ToXVal2(TotalSoChuong, 0) + " chuồng)</b></td><td></td><td></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'><b>" + Config.ToXVal2(TotalThucAn, scaleTA) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'><b>" + Config.ToXVal2(TotalThucAnThua, scaleTA) + "</b></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'><b>" + Config.ToXVal2(TotalBST, scaleTTY) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDen, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDi, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalBan, 0) + "</b></td><td></td></tr>");
                            SupTotalThucAn += Math.Round(TotalThucAn, scaleTA);
                            SupTotalThucAnThua += Math.Round(TotalThucAnThua, scaleTA);
                            SupTotalBST += Math.Round(TotalBST, scaleTTY);
                            SupTotalChuyenDen += TotalChuyenDen;
                            SupTotalChuyenDi += TotalChuyenDi;
                            SupTotalChet += TotalChet;
                            SupTotalLoaiThai += TotalLoaiThai;
                            SupTotalGietMo += TotalGietMo;
                            SupTotalBan += TotalBan;
                        }
                    }
                    sb.Append("<tr style='color:#F00;font-weight:bold;'><td align='center'>TỔNG CÁC LOẠI CÁ</td><td></td><td></td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(SupTotalThucAn, scaleTA) + "</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(SupTotalThucAnThua, scaleTA) + "</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(SupTotalBST, scaleTTY) + "</td><td align='right'>" + Config.ToXVal2(SupTotalChuyenDen, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalChuyenDi, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalChet, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalLoaiThai, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalGietMo, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalBan, 0) + "</td><td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                sb.Append(Config.Footer(UserInfo.Profile.City, UserInfo.FirstName + " " + UserInfo.LastName));
                sb.Append("</body></html>");
                Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string[] aLoaiCa = Config.GetSelectedTexts(lstLoaiCa).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string tieude = "";
                string strSQL = "QLCS_BCTK_NhatKy_TheoLoaiCa_new_KhuChuong";
                if (ddlChuan.SelectedValue == "1")
                {
                    strSQL = "QLCS_BCTK_NhatKy_TheoLoaiCa_new_KhuChuong_ChuanCu";
                }
                SqlParameter[] param = new SqlParameter[9];
                if (txtTuNgay.Text == "")
                {
                    txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtDenNgay.Text == "")
                {
                    txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@StrLoaiCa", Config.GetSelectedValues_At(lstLoaiCa));
                param[1] = new SqlParameter("@StrKhuChuong", Config.GetSelectedValues_At(lstKhuChuong));
                param[2] = new SqlParameter("@TuNgay", DateTime.Parse(txtTuNgay.Text, ci));
                param[3] = new SqlParameter("@DenNgay", DateTime.Parse(txtDenNgay.Text, ci));
                param[4] = new SqlParameter("@OrderBy", ddlOrderBy.SelectedValue);
                param[5] = new SqlParameter("@ScaleTA", int.Parse(ddlScaleTA.SelectedValue));
                param[6] = new SqlParameter("@ScaleTTY", int.Parse(ddlScaleTTY.SelectedValue));
                param[7] = new SqlParameter("@PhanLoai", int.Parse(ddlPhanLoai.SelectedValue));
                param[8] = new SqlParameter("@ShowAll", chkShowAll.Checked);
                tieude += "<b>NHẬT KÝ CHĂN NUÔI THEO LOẠI CÁ - TỪ NGÀY " + txtTuNgay.Text + "  ĐẾN NGÀY " + txtDenNgay.Text + "</b>";

                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt;

                StringBuilder sb = new StringBuilder();

                if (ddlOrderBy.SelectedValue == "Ngay")
                {
                    decimal SupTotalThucAn = 0; decimal SupTotalThucAnThua = 0; decimal SupTotalBST = 0;
                    int SupTotalChuyenDen = 0; int SupTotalChuyenDi = 0; int SupTotalChet = 0; int SupTotalLoaiThai = 0; int SupTotalGietMo = 0; int SupTotalBan = 0;
                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr><th rowspan=2 align='center'>Ngày</th><th rowspan=2 align='center'>Chuồng</th><th rowspan=2 align='center'>Số cá<br/>đầu kỳ</th><th rowspan='2' align='center'>Thức ăn</th><th rowspan='2' align='center'>Thức ăn<br/>thừa</th><th rowspan=2 align='center'>Thuốc</th><th colspan='6' align='center'>Biến động đàn</th><th rowspan=2 align='center'>Ghi chú</th></tr>
                        <tr><th align='center'>Nhập<br/>mới</th><th align='center'>Chuyển<br/>chuồng</th><th align='center'>Chết</th><th align='center'>Loại<br/>thải</th><th align='center'>Giết<br/>mổ</th><th align='center'>Bán</th></tr></thead><tbody>");
                    for (int j = 0; j < aLoaiCa.Length; j++)
                    {
                        DateTime currThoiDiem = DateTime.MinValue;
                        int TongSoChuong = 0; int TongSoCa = 0; decimal TongThucAn = 0; decimal TongThucAnThua = 0; decimal TongBST = 0; int TongChuyenDen = 0; int TongChuyenDi = 0; int TongChet = 0; int TongLoaiThai = 0; int TongGietMo = 0; int TongBan = 0;
                        decimal TotalThucAn = 0; decimal TotalThucAnThua = 0; decimal TotalBST = 0;
                        int TotalChuyenDen = 0; int TotalChuyenDi = 0; int TotalChet = 0; int TotalLoaiThai = 0; int TotalGietMo = 0; int TotalBan = 0;
                        dt = ds.Tables[j];
                        sb.Append("<tr><td align='center'><b><span style='color:#f00;'>" + aLoaiCa[j] + "</span></b></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow r = dt.Rows[i];
                            if (currThoiDiem != Convert.ToDateTime(r["ThoiDiem"]) && currThoiDiem != DateTime.MinValue)
                            {
                                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoChuong, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongSoCa, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongThucAn, scaleTA) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongThucAnThua, scaleTA) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBST, scaleTTY) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "</b></td><td></td></tr>");

                                TotalThucAn += TongThucAn;
                                TotalThucAnThua += TongThucAnThua;
                                TotalBST += TongBST;
                                TotalChuyenDen += TongChuyenDen;
                                TotalChuyenDi += TongChuyenDi;
                                TotalChet += TongChet;
                                TotalLoaiThai += TongLoaiThai;
                                TotalGietMo += TongGietMo;
                                TotalBan += TongBan;

                                TongSoChuong = 0;
                                TongSoCa = 0;
                                TongThucAn = 0;
                                TongThucAnThua = 0;
                                TongBST = 0;
                                TongChuyenDen = 0;
                                TongChuyenDi = 0;
                                TongChet = 0;
                                TongLoaiThai = 0;
                                TongGietMo = 0;
                                TongBan = 0;
                            }
                            currThoiDiem = Convert.ToDateTime(r["ThoiDiem"]);
                            sb.Append("<tr><td align='center'>" + Convert.ToDateTime(r["ThoiDiem"]).ToString("dd/MM/yyyy") + "</td><td>" + r["TenChuong"].ToString() + "</td><td align='right'>" + r["SoCa"].ToString() + "</td><td align='left'>" + r["ThucAn"].ToString() + "</td><td align='right'>" + Config.ToXVal2(r["Thua"], scaleTA) + "</td><td align='left'>" + Config.Split(r["BST"].ToString(), 4) + "</td><td align='right'>" + r["ChuyenDen"].ToString() + "</td><td align='right'>" + r["ChuyenDi"].ToString() + "</td><td align='right'>" + r["Chet"].ToString() + "</td><td align='right'>" + r["LoaiThai"].ToString() + "</td><td align='right'>" + r["GietMo"].ToString() + "</td><td align='right'>" + r["Ban"].ToString() + "</td><td align='left'>" + r["Note"].ToString());
                            if (r["LyDoChet"].ToString() != "") sb.Append(" (Chết/loại thải: " + r["LyDoChet"].ToString() + ")");
                            sb.Append("</td></tr>");
                            TongSoChuong++;
                            TongSoCa += Convert.ToInt32(r["SoCa"]);
                            TongThucAn += r["TongThucAn"] != DBNull.Value ? Convert.ToDecimal(r["TongThucAn"]) : 0;
                            TongThucAnThua += r["Thua"] != DBNull.Value ? Convert.ToDecimal(r["Thua"]) : 0;
                            TongBST += r["TongBST"] != DBNull.Value ? Convert.ToDecimal(r["TongBST"]) : 0;
                            TongChuyenDen += r["ChuyenDen"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDen"]) : 0;
                            TongChuyenDi += r["ChuyenDi"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDi"]) : 0;
                            TongChet += r["Chet"] != DBNull.Value ? Convert.ToInt32(r["Chet"]) : 0;
                            TongLoaiThai += r["LoaiThai"] != DBNull.Value ? Convert.ToInt32(r["LoaiThai"]) : 0;
                            TongGietMo += r["GietMo"] != DBNull.Value ? Convert.ToInt32(r["GietMo"]) : 0;
                            TongBan += r["Ban"] != DBNull.Value ? Convert.ToInt32(r["Ban"]) : 0;
                        }
                        if (currThoiDiem != DateTime.MinValue)
                        {
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoChuong, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongSoCa, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongThucAn, scaleTA) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongThucAnThua, scaleTA) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBST, scaleTTY) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "</b></td><td></td></tr>");
                            TotalThucAn += TongThucAn;
                            TotalThucAnThua += TongThucAnThua;
                            TotalBST += TongBST;
                            TotalChuyenDen += TongChuyenDen;
                            TotalChuyenDi += TongChuyenDi;
                            TotalChet += TongChet;
                            TotalLoaiThai += TongLoaiThai;
                            TotalGietMo += TongGietMo;
                            TotalBan += TongBan;
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'><b>Tổng cộng</b></td><td></td><td></td><td align='right'><b>" + Config.ToXVal2(TotalThucAn, scaleTA) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalThucAnThua, scaleTA) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalBST, scaleTTY) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDen, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDi, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalBan, 0) + "</b></td><td></td></tr>");
                            SupTotalThucAn += Math.Round(TotalThucAn, scaleTA);
                            SupTotalThucAnThua += Math.Round(TotalThucAnThua, scaleTA);
                            SupTotalBST += Math.Round(TotalBST, scaleTTY);
                            SupTotalChuyenDen += TotalChuyenDen;
                            SupTotalChuyenDi += TotalChuyenDi;
                            SupTotalChet += TotalChet;
                            SupTotalLoaiThai += TotalLoaiThai;
                            SupTotalGietMo += TotalGietMo;
                            SupTotalBan += TotalBan;
                        }
                    }
                    sb.Append("<tr style='color:#F00;font-weight:bold;'><td align='center'>TỔNG CÁC LOẠI CÁ</td><td></td><td></td><td align='right'>" + Config.ToXVal2(SupTotalThucAn, scaleTA) + "</td><td align='right'>" + Config.ToXVal2(SupTotalThucAnThua, scaleTA) + "</td><td align='right'>" + Config.ToXVal2(SupTotalBST, scaleTTY) + "</td><td align='right'>" + Config.ToXVal2(SupTotalChuyenDen, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalChuyenDi, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalChet, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalLoaiThai, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalGietMo, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalBan, 0) + "</td><td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlOrderBy.SelectedValue == "Chuong")
                {
                    decimal SupTotalThucAn = 0; decimal SupTotalThucAnThua = 0; decimal SupTotalBST = 0;
                    int SupTotalChuyenDen = 0; int SupTotalChuyenDi = 0; int SupTotalChet = 0; int SupTotalLoaiThai = 0; int SupTotalGietMo = 0; int SupTotalBan = 0;
                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr><th rowspan=2 align='center'>Chuồng</th><th rowspan=2 align='center'>Ngày</th><th rowspan=2 align='center'>Số cá<br/>đầu kỳ</th><th rowspan='2' align='center'>Thức ăn</th><th rowspan='2' align='center'>Thức ăn<br/>thừa</th><th rowspan=2 align='center'>Thuốc</th><th colspan='6' align='center'>Biến động đàn</th><th rowspan=2 align='center'>Ghi chú</th></tr>
                        <tr><th align='center'>Nhập<br/>mới</th><th align='center'>Chuyển<br/>chuồng</th><th align='center'>Chết</th><th align='center'>Loại<br/>thải</th><th align='center'>Giết<br/>mổ</th><th align='center'>Bán</th></tr></thead><tbody>");
                    for (int j = 0; j < aLoaiCa.Length; j++)
                    {
                        string currTenChuong = "";
                        int TongSoNgay = 0; decimal TongThucAn = 0; decimal TongThucAnThua = 0; decimal TongBST = 0; int TongChuyenDen = 0; int TongChuyenDi = 0; int TongChet = 0; int TongLoaiThai = 0; int TongGietMo = 0; int TongBan = 0;
                        decimal TotalThucAn = 0; decimal TotalThucAnThua = 0; decimal TotalBST = 0;
                        int TotalChuyenDen = 0; int TotalChuyenDi = 0; int TotalChet = 0; int TotalLoaiThai = 0; int TotalGietMo = 0; int TotalBan = 0;
                        int TotalSoChuong = 0;
                        dt = ds.Tables[j];
                        sb.Append("<tr><td align='center'><b><span style='color:#f00;'>" + aLoaiCa[j] + "</span></b></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow r = dt.Rows[i];
                            if (currTenChuong != r["TenChuong"].ToString() && currTenChuong != "")
                            {
                                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoNgay, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongThucAn, scaleTA) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongThucAnThua, scaleTA) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBST, scaleTTY) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "</b></td><td></td></tr>");

                                TotalThucAn += TongThucAn;
                                TotalThucAnThua += TongThucAnThua;
                                TotalBST += TongBST;
                                TotalChuyenDen += TongChuyenDen;
                                TotalChuyenDi += TongChuyenDi;
                                TotalChet += TongChet;
                                TotalLoaiThai += TongLoaiThai;
                                TotalGietMo += TongGietMo;
                                TotalBan += TongBan;
                                TotalSoChuong++;

                                TongSoNgay = 0;
                                TongThucAn = 0;
                                TongThucAnThua = 0;
                                TongBST = 0;
                                TongChuyenDen = 0;
                                TongChuyenDi = 0;
                                TongChet = 0;
                                TongLoaiThai = 0;
                                TongGietMo = 0;
                                TongBan = 0;
                            }
                            currTenChuong = r["TenChuong"].ToString();
                            sb.Append("<tr><td>" + r["TenChuong"].ToString() + "</td><td align='center'>" + Convert.ToDateTime(r["ThoiDiem"]).ToString("dd/MM/yyyy") + "</td><td align='right'>" + r["SoCa"].ToString() + "</td><td align='left'>" + r["ThucAn"].ToString() + "</td><td align='right'>" + Config.ToXVal2(r["Thua"], scaleTA) + "</td><td align='left'>" + Config.Split(r["BST"].ToString(), 4) + "</td><td align='right'>" + r["ChuyenDen"].ToString() + "</td><td align='right'>" + r["ChuyenDi"].ToString() + "</td><td align='right'>" + r["Chet"].ToString() + "</td><td align='right'>" + r["LoaiThai"].ToString() + "</td><td align='right'>" + r["GietMo"].ToString() + "</td><td align='right'>" + r["Ban"].ToString() + "</td><td align='left'>" + r["Note"].ToString());
                            if (r["LyDoChet"].ToString() != "") sb.Append(" (Chết/loại thải: " + r["LyDoChet"].ToString() + ")");
                            sb.Append("</td></tr>");
                            TongSoNgay++;
                            TongThucAn += r["TongThucAn"] != DBNull.Value ? Convert.ToDecimal(r["TongThucAn"]) : 0;
                            TongThucAnThua += r["Thua"] != DBNull.Value ? Convert.ToDecimal(r["Thua"]) : 0;
                            TongBST += r["TongBST"] != DBNull.Value ? Convert.ToDecimal(r["TongBST"]) : 0;
                            TongChuyenDen += r["ChuyenDen"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDen"]) : 0;
                            TongChuyenDi += r["ChuyenDi"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDi"]) : 0;
                            TongChet += r["Chet"] != DBNull.Value ? Convert.ToInt32(r["Chet"]) : 0;
                            TongLoaiThai += r["LoaiThai"] != DBNull.Value ? Convert.ToInt32(r["LoaiThai"]) : 0;
                            TongGietMo += r["GietMo"] != DBNull.Value ? Convert.ToInt32(r["GietMo"]) : 0;
                            TongBan += r["Ban"] != DBNull.Value ? Convert.ToInt32(r["Ban"]) : 0;
                        }
                        if (currTenChuong != "")
                        {
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoNgay, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongThucAn, scaleTA) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongThucAnThua, scaleTA) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBST, scaleTTY) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "</b></td><td></td></tr>");
                            TotalThucAn += TongThucAn;
                            TotalThucAnThua += TongThucAnThua;
                            TotalBST += TongBST;
                            TotalChuyenDen += TongChuyenDen;
                            TotalChuyenDi += TongChuyenDi;
                            TotalChet += TongChet;
                            TotalLoaiThai += TongLoaiThai;
                            TotalGietMo += TongGietMo;
                            TotalBan += TongBan;
                            TotalSoChuong++;
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'><b>Tổng cộng<br/>(" + Config.ToXVal2(TotalSoChuong, 0) + " chuồng)</b></td><td></td><td align='left'></td><td align='right'><b>" + Config.ToXVal2(TotalThucAn, scaleTA) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalThucAnThua, scaleTA) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalBST, scaleTTY) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDen, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDi, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalBan, 0) + "</b></td><td></td></tr>");
                            SupTotalThucAn += Math.Round(TotalThucAn, scaleTA);
                            SupTotalThucAnThua += Math.Round(TotalThucAnThua, scaleTA);
                            SupTotalBST += Math.Round(TotalBST, scaleTTY);
                            SupTotalChuyenDen += TotalChuyenDen;
                            SupTotalChuyenDi += TotalChuyenDi;
                            SupTotalChet += TotalChet;
                            SupTotalLoaiThai += TotalLoaiThai;
                            SupTotalGietMo += TotalGietMo;
                            SupTotalBan += TotalBan;
                        }
                    }
                    sb.Append("<tr style='color:#F00;font-weight:bold;'><td align='center'>TỔNG CÁC LOẠI CÁ</td><td></td><td></td><td align='right'>" + Config.ToXVal2(SupTotalThucAn, scaleTA) + "</td><td align='right'>" + Config.ToXVal2(SupTotalThucAnThua, scaleTA) + "</td><td align='right'>" + Config.ToXVal2(SupTotalBST, scaleTTY) + "</td><td align='right'>" + Config.ToXVal2(SupTotalChuyenDen, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalChuyenDi, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalChet, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalLoaiThai, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalGietMo, 0) + "</td><td align='right'>" + Config.ToXVal2(SupTotalBan, 0) + "</td><td></td></tr>");
                    sb.Append("</tbody></table>");
                }

                lt.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}