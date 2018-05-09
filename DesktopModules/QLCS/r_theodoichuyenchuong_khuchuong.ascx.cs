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
    public partial class r_theodoichuyenchuong_khuchuong : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");

        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");

            txtTuNgay.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");

            Config.LoadKhuChuong(lstKhuChuong);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
            try
            {
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
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string filename = "NKCC";
                string tieude = "";
                string strSQL = "QLCS_BCTK_NhatKy_Chuong_ChuyenChuong_TheoKhuChuong_new";
                if (ddlChuan.SelectedValue == "1")
                {
                    strSQL = "QLCS_BCTK_NhatKy_Chuong_ChuyenChuong_TheoKhuChuong_new_ChuanCu";
                }
                SqlParameter[] param = new SqlParameter[5];
                if (txtTuNgay.Text == "")
                {
                    txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtDenNgay.Text == "")
                {
                    txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@KhuChuong", Config.GetSelectedValues_At(lstKhuChuong));
                param[1] = new SqlParameter("@TuNgay", DateTime.Parse(txtTuNgay.Text, ci));
                param[2] = new SqlParameter("@DenNgay", DateTime.Parse(txtDenNgay.Text, ci));
                param[3] = new SqlParameter("@OrderBy", ddlOrderBy.SelectedValue);
                param[4] = new SqlParameter("@PhanLoai", int.Parse(ddlPhanLoai.SelectedValue));
                filename += txtTuNgay.Text + "__" + txtDenNgay.Text + ".xls";
                filename = filename.Replace("/", "_").Replace(" ", "");
                tieude += "<b>THEO DÕI BIẾN ĐỘNG Ô CHUỒNG THEO KHU CHUỒNG " + Config.GetSelectedTexts(lstKhuChuong) + " - TỪ NGÀY " + txtTuNgay.Text + "  ĐẾN NGÀY " + txtDenNgay.Text + "</b>";

                DataTable dt = Config.SelectSP(strSQL, param);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                StringBuilder sb = new StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table border='1'>");

                if (ddlOrderBy.SelectedValue == "Ngay")
                {
                    DateTime currThoiDiem = DateTime.MinValue;
                    sb.Append(@"<tr><th align='center'>Ngày</th><th align='center'>Chuồng</th><th align='center'>Số cá đầu kỳ</th><th align='center'>Nhập mới</th><th align='center'>Nhập mới<br/>chi tiết</th><th align='center'>Chuyển chuồng</th><th align='center'>Chuyển chuồng<br/>chi tiết</th><th align='center'>Chết</th><th align='center'>Loại thải</th><th align='center'>Giết mổ</th><th align='center'>Bán</th><th align='center'>Cuối kỳ</th></tr></thead><tbody>");
                    int TongSoChuong = 0; int TongSoCa = 0; int TongChuyenDen = 0; int TongChuyenDi = 0; int TongChet = 0; int TongLoaiThai = 0; int TongGietMo = 0; int TongBan = 0; int TongSoCaCK = 0;
                    int TotalChuyenDen = 0; int TotalChuyenDi = 0; int TotalChet = 0; int TotalLoaiThai = 0; int TotalGietMo = 0; int TotalBan = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        if (currThoiDiem != Convert.ToDateTime(r["ThoiDiem"]) && currThoiDiem != DateTime.MinValue)
                        {
                            sb.Append("<tr><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoChuong, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongSoCa, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "<b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "<b></td><td align='right'><b>" + Config.ToXVal2(TongSoCaCK, 0) + "</b></td></tr>");

                            TotalChuyenDen += TongChuyenDen;
                            TotalChuyenDi += TongChuyenDi;
                            TotalChet += TongChet;
                            TotalLoaiThai += TongLoaiThai;
                            TotalGietMo += TongGietMo;
                            TotalBan += TongBan;

                            TongSoChuong = 0;
                            TongSoCa = 0;
                            TongChuyenDen = 0;
                            TongChuyenDi = 0;
                            TongChet = 0;
                            TongLoaiThai = 0;
                            TongGietMo = 0;
                            TongBan = 0;
                            TongSoCaCK = 0;
                        }
                        currThoiDiem = Convert.ToDateTime(r["ThoiDiem"]);

                        int dk = r["SoCa"] != DBNull.Value ? Convert.ToInt32(r["SoCa"]) : 0;
                        int cde = r["ChuyenDen"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDen"]) : 0;
                        int cdi = r["ChuyenDi"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDi"]) : 0;
                        int ch = r["Chet"] != DBNull.Value ? Convert.ToInt32(r["Chet"]) : 0;
                        int lt = r["LoaiThai"] != DBNull.Value ? Convert.ToInt32(r["LoaiThai"]) : 0;
                        int gm = r["GietMo"] != DBNull.Value ? Convert.ToInt32(r["GietMo"]) : 0;
                        int bn = r["Ban"] != DBNull.Value ? Convert.ToInt32(r["Ban"]) : 0;
                        int ck = dk + cde - cdi - ch - lt - gm - bn;

                        sb.Append(@"<tr><td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["ThoiDiem"]).ToString("dd/MM/yyyy") + "</td><td>" + r["TenChuong"].ToString() + "</td><td align='right'>" + dk.ToString() + "</td><td align='right'>" + cde.ToString() + "</td><td align='left'>" + HttpUtility.HtmlDecode(r["ChuyenDenChiTiet"].ToString()) + "</td><td align='right'>" + cdi.ToString() + "</td><td align='left'>" + HttpUtility.HtmlDecode(r["ChuyenDiChiTiet"].ToString()) + "</td><td align='right'>" + ch.ToString() + "</td><td align='right'>" + lt.ToString() + "</td><td align='right'>" + gm.ToString() + "</td><td align='right'>" + bn.ToString() + "</td><td align='right'>" + ck.ToString() + "</td></tr>");
                        TongSoChuong++;
                        TongSoCa += dk;
                        TongChuyenDen += cde;
                        TongChuyenDi += cdi;
                        TongChet += ch;
                        TongLoaiThai += lt;
                        TongGietMo += gm;
                        TongBan += bn;
                        TongSoCaCK += ck;
                    }
                    if (currThoiDiem != DateTime.MinValue)
                    {
                        sb.Append("<tr><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoChuong, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongSoCa, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "<b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "<b></td><td align='right'><b>" + Config.ToXVal2(TongSoCaCK, 0) + "</b></td></tr>");
                        TotalChuyenDen += TongChuyenDen;
                        TotalChuyenDi += TongChuyenDi;
                        TotalChet += TongChet;
                        TotalLoaiThai += TongLoaiThai;
                        TotalGietMo += TongGietMo;
                        TotalBan += TongBan;
                        sb.Append("<tr><td align='center'><b>Tổng cộng</b></td><td></td><td></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDen, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDi, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TotalChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalBan, 0) + "</b></td><td></td></tr>");
                    }
                    sb.Append("</tbody></table>");
                }
                else if (ddlOrderBy.SelectedValue == "Chuong")
                {
                    string currTenChuong = "";
                    sb.Append(@"<tr><th align='center'>Chuồng</th><th align='center'>Ngày</th><th align='center'>Số cá đầu kỳ</th><th align='center'>Nhập mới</th><th align='center'>Nhập mới<br/>chi tiết</th><th align='center'>Chuyển chuồng</th><th align='center'>Chuyển chuồng<br/>chi tiết</th><th align='center'>Chết</th><th align='center'>Loại thải</th><th align='center'>Giết mổ</th><th align='center'>Bán</th><th align='center'>Cuối kỳ</th></tr></thead><tbody>");
                    int TongSoNgay = 0; int TongChuyenDen = 0; int TongChuyenDi = 0; int TongChet = 0; int TongLoaiThai = 0; int TongGietMo = 0; int TongBan = 0;
                    int TotalChuyenDen = 0; int TotalChuyenDi = 0; int TotalChet = 0; int TotalLoaiThai = 0; int TotalGietMo = 0; int TotalBan = 0;
                    int TotalSoChuong = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        if (currTenChuong != r["TenChuong"].ToString() && currTenChuong != "")
                        {
                            sb.Append("<tr><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoNgay, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "</b></td><td></td></tr>");

                            TotalChuyenDen += TongChuyenDen;
                            TotalChuyenDi += TongChuyenDi;
                            TotalChet += TongChet;
                            TotalLoaiThai += TongLoaiThai;
                            TotalGietMo += TongGietMo;
                            TotalBan += TongBan;
                            TotalSoChuong++;

                            TongSoNgay = 0;
                            TongChuyenDen = 0;
                            TongChuyenDi = 0;
                            TongChet = 0;
                            TongLoaiThai = 0;
                            TongGietMo = 0;
                            TongBan = 0;
                        }
                        currTenChuong = r["TenChuong"].ToString();

                        int dk = r["SoCa"] != DBNull.Value ? Convert.ToInt32(r["SoCa"]) : 0;
                        int cde = r["ChuyenDen"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDen"]) : 0;
                        int cdi = r["ChuyenDi"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDi"]) : 0;
                        int ch = r["Chet"] != DBNull.Value ? Convert.ToInt32(r["Chet"]) : 0;
                        int lt = r["LoaiThai"] != DBNull.Value ? Convert.ToInt32(r["LoaiThai"]) : 0;
                        int gm = r["GietMo"] != DBNull.Value ? Convert.ToInt32(r["GietMo"]) : 0;
                        int bn = r["Ban"] != DBNull.Value ? Convert.ToInt32(r["Ban"]) : 0;
                        int ck = dk + cde - cdi - ch - lt - gm - bn;

                        sb.Append("<tr><td>" + r["TenChuong"].ToString() + @"</td><td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["ThoiDiem"]).ToString("dd/MM/yyyy") + "</td><td align='right'>" + dk.ToString() + "</td><td align='right'>" + cde.ToString() + "</td><td align='left'>" + HttpUtility.HtmlDecode(r["ChuyenDenChiTiet"].ToString()) + "</td><td align='right'>" + cdi.ToString() + "</td><td align='left'>" + HttpUtility.HtmlDecode(r["ChuyenDiChiTiet"].ToString()) + "</td><td align='right'>" + ch.ToString() + "</td><td align='right'>" + lt.ToString() + "</td><td align='right'>" + gm.ToString() + "</td><td align='right'>" + bn.ToString() + "</td><td align='right'>" + ck.ToString() + "</td></tr>");
                        TongSoNgay++;
                        TongChuyenDen += cde;
                        TongChuyenDi += cdi;
                        TongChet += ch;
                        TongLoaiThai += lt;
                        TongGietMo += gm;
                        TongBan += bn;
                    }
                    if (currTenChuong != "")
                    {
                        sb.Append("<tr><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoNgay, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "</b></td><td></td></tr>");
                        TotalChuyenDen += TongChuyenDen;
                        TotalChuyenDi += TongChuyenDi;
                        TotalChet += TongChet;
                        TotalLoaiThai += TongLoaiThai;
                        TotalGietMo += TongGietMo;
                        TotalBan += TongBan;
                        TotalSoChuong++;
                        sb.Append("<tr><td align='center'><b>Tổng cộng<br/>(" + Config.ToXVal2(TotalSoChuong, 0) + " chuồng)</b></td><td></td><td></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDen, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDi, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TotalChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalBan, 0) + "</b></td><td></td></tr>");
                    }
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
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string tieude = "";
                string strSQL = "QLCS_BCTK_NhatKy_Chuong_ChuyenChuong_TheoKhuChuong_new";
                if (ddlChuan.SelectedValue == "1")
                {
                    strSQL = "QLCS_BCTK_NhatKy_Chuong_ChuyenChuong_TheoKhuChuong_new_ChuanCu";
                }
                SqlParameter[] param = new SqlParameter[5];
                if (txtTuNgay.Text == "")
                {
                    txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtDenNgay.Text == "")
                {
                    txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@KhuChuong", Config.GetSelectedValues_At(lstKhuChuong));
                param[1] = new SqlParameter("@TuNgay", DateTime.Parse(txtTuNgay.Text, ci));
                param[2] = new SqlParameter("@DenNgay", DateTime.Parse(txtDenNgay.Text, ci));
                param[3] = new SqlParameter("@OrderBy", ddlOrderBy.SelectedValue);
                param[4] = new SqlParameter("@PhanLoai", int.Parse(ddlPhanLoai.SelectedValue));
                tieude += "<b>THEO DÕI BIẾN ĐỘNG Ô CHUỒNG THEO KHU CHUỒNG - TỪ NGÀY " + txtTuNgay.Text + "  ĐẾN NGÀY " + txtDenNgay.Text + "</b>";

                DataTable dt = Config.SelectSP(strSQL, param);

                StringBuilder sb = new StringBuilder();

                if (ddlOrderBy.SelectedValue == "Ngay")
                {
                    DateTime currThoiDiem = DateTime.MinValue;
                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr><th align='center'>Ngày</th><th align='center'>Chuồng</th><th align='center'>Số cá đầu kỳ</th><th align='center'>Nhập mới</th><th align='center'>Nhập mới<br/>chi tiết</th><th align='center'>Chuyển chuồng</th><th align='center'>Chuyển chuồng<br/>chi tiết</th><th align='center'>Chết</th><th align='center'>Loại thải</th><th align='center'>Giết mổ</th><th align='center'>Bán</th><th align='center'>Cuối kỳ</th></tr></thead><tbody>");
                    int TongSoChuong = 0; int TongSoCa = 0; int TongChuyenDen = 0; int TongChuyenDi = 0; int TongChet = 0; int TongLoaiThai = 0; int TongGietMo = 0; int TongBan = 0; int TongSoCaCK = 0;
                    int TotalChuyenDen = 0; int TotalChuyenDi = 0; int TotalChet = 0; int TotalLoaiThai = 0; int TotalGietMo = 0; int TotalBan = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        if (currThoiDiem != Convert.ToDateTime(r["ThoiDiem"]) && currThoiDiem != DateTime.MinValue)
                        {
                            sb.Append("<tr><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoChuong, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongSoCa, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "<b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "<b></td><td align='right'><b>" + Config.ToXVal2(TongSoCaCK, 0) + "</b></td></tr>");

                            TotalChuyenDen += TongChuyenDen;
                            TotalChuyenDi += TongChuyenDi;
                            TotalChet += TongChet;
                            TotalLoaiThai += TongLoaiThai;
                            TotalGietMo += TongGietMo;
                            TotalBan += TongBan;

                            TongSoChuong = 0;
                            TongSoCa = 0;
                            TongChuyenDen = 0;
                            TongChuyenDi = 0;
                            TongChet = 0;
                            TongLoaiThai = 0;
                            TongGietMo = 0;
                            TongBan = 0;
                            TongSoCaCK = 0;
                        }
                        currThoiDiem = Convert.ToDateTime(r["ThoiDiem"]);

                        int dk = r["SoCa"] != DBNull.Value ? Convert.ToInt32(r["SoCa"]) : 0;
                        int cde = r["ChuyenDen"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDen"]) : 0;
                        int cdi = r["ChuyenDi"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDi"]) : 0;
                        int ch = r["Chet"] != DBNull.Value ? Convert.ToInt32(r["Chet"]) : 0;
                        int lt = r["LoaiThai"] != DBNull.Value ? Convert.ToInt32(r["LoaiThai"]) : 0;
                        int gm = r["GietMo"] != DBNull.Value ? Convert.ToInt32(r["GietMo"]) : 0;
                        int bn = r["Ban"] != DBNull.Value ? Convert.ToInt32(r["Ban"]) : 0;
                        int ck = dk + cde - cdi - ch - lt - gm - bn;

                        sb.Append("<tr><td align='center'>" + Convert.ToDateTime(r["ThoiDiem"]).ToString("dd/MM/yyyy") + "</td><td>" + r["TenChuong"].ToString() + "</td><td align='right'>" + dk.ToString() + "</td><td align='right'>" + cde.ToString() + "</td><td align='left'>" + HttpUtility.HtmlDecode(r["ChuyenDenChiTiet"].ToString()) + "</td><td align='right'>" + cdi.ToString() + "</td><td align='left'>" + HttpUtility.HtmlDecode(r["ChuyenDiChiTiet"].ToString()) + "</td><td align='right'>" + ch.ToString() + "</td><td align='right'>" + lt.ToString() + "</td><td align='right'>" + gm.ToString() + "</td><td align='right'>" + bn.ToString() + "</td><td align='right'>" + ck.ToString() + "</td></tr>");
                        TongSoChuong++;
                        TongSoCa += dk;
                        TongChuyenDen += cde;
                        TongChuyenDi += cdi;
                        TongChet += ch;
                        TongLoaiThai += lt;
                        TongGietMo += gm;
                        TongBan += bn;
                        TongSoCaCK += ck;
                    }
                    if (currThoiDiem != DateTime.MinValue)
                    {
                        sb.Append("<tr><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoChuong, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongSoCa, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "<b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "<b></td><td align='right'><b>" + Config.ToXVal2(TongSoCaCK, 0) + "</b></td></tr>");
                        TotalChuyenDen += TongChuyenDen;
                        TotalChuyenDi += TongChuyenDi;
                        TotalChet += TongChet;
                        TotalLoaiThai += TongLoaiThai;
                        TotalGietMo += TongGietMo;
                        TotalBan += TongBan;
                        sb.Append("<tr><td align='center'><b>Tổng cộng</b></td><td></td><td></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDen, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDi, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TotalChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalBan, 0) + "</b></td><td></td></tr>");
                    }
                    sb.Append("</tbody></table>");
                }
                else if (ddlOrderBy.SelectedValue == "Chuong")
                {
                    string currTenChuong = "";
                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr><th align='center'>Chuồng</th><th align='center'>Ngày</th><th align='center'>Số cá đầu kỳ</th><th align='center'>Nhập mới</th><th align='center'>Nhập mới<br/>chi tiết</th><th align='center'>Chuyển chuồng</th><th align='center'>Chuyển chuồng<br/>chi tiết</th><th align='center'>Chết</th><th align='center'>Loại thải</th><th align='center'>Giết mổ</th><th align='center'>Bán</th><th align='center'>Cuối kỳ</th></tr></thead><tbody>");
                    int TongSoNgay = 0; int TongChuyenDen = 0; int TongChuyenDi = 0; int TongChet = 0; int TongLoaiThai = 0; int TongGietMo = 0; int TongBan = 0;
                    int TotalChuyenDen = 0; int TotalChuyenDi = 0; int TotalChet = 0; int TotalLoaiThai = 0; int TotalGietMo = 0; int TotalBan = 0;
                    int TotalSoChuong = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        if (currTenChuong != r["TenChuong"].ToString() && currTenChuong != "")
                        {
                            sb.Append("<tr><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoNgay, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "</b></td><td></td></tr>");

                            TotalChuyenDen += TongChuyenDen;
                            TotalChuyenDi += TongChuyenDi;
                            TotalChet += TongChet;
                            TotalLoaiThai += TongLoaiThai;
                            TotalGietMo += TongGietMo;
                            TotalBan += TongBan;
                            TotalSoChuong++;

                            TongSoNgay = 0;
                            TongChuyenDen = 0;
                            TongChuyenDi = 0;
                            TongChet = 0;
                            TongLoaiThai = 0;
                            TongGietMo = 0;
                            TongBan = 0;
                        }
                        currTenChuong = r["TenChuong"].ToString();

                        int dk = r["SoCa"] != DBNull.Value ? Convert.ToInt32(r["SoCa"]) : 0;
                        int cde = r["ChuyenDen"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDen"]) : 0;
                        int cdi = r["ChuyenDi"] != DBNull.Value ? Convert.ToInt32(r["ChuyenDi"]) : 0;
                        int ch = r["Chet"] != DBNull.Value ? Convert.ToInt32(r["Chet"]) : 0;
                        int lt = r["LoaiThai"] != DBNull.Value ? Convert.ToInt32(r["LoaiThai"]) : 0;
                        int gm = r["GietMo"] != DBNull.Value ? Convert.ToInt32(r["GietMo"]) : 0;
                        int bn = r["Ban"] != DBNull.Value ? Convert.ToInt32(r["Ban"]) : 0;
                        int ck = dk + cde - cdi - ch - lt - gm - bn;

                        sb.Append("<tr><td>" + r["TenChuong"].ToString() + "</td><td align='center'>" + Convert.ToDateTime(r["ThoiDiem"]).ToString("dd/MM/yyyy") + "</td><td align='right'>" + dk.ToString() + "</td><td align='right'>" + cde.ToString() + "</td><td align='left'>" + HttpUtility.HtmlDecode(r["ChuyenDenChiTiet"].ToString()) + "</td><td align='right'>" + cdi.ToString() + "</td><td align='left'>" + HttpUtility.HtmlDecode(r["ChuyenDiChiTiet"].ToString()) + "</td><td align='right'>" + ch.ToString() + "</td><td align='right'>" + lt.ToString() + "</td><td align='right'>" + gm.ToString() + "</td><td align='right'>" + bn.ToString() + "</td><td align='right'>" + ck.ToString() + "</td></tr>");
                        TongSoNgay++;
                        TongChuyenDen += cde;
                        TongChuyenDi += cdi;
                        TongChet += ch;
                        TongLoaiThai += lt;
                        TongGietMo += gm;
                        TongBan += bn;
                    }
                    if (currTenChuong != "")
                    {
                        sb.Append("<tr><td align='center'><b>T.C</b></td><td align='right'><b>" + Config.ToXVal2(TongSoNgay, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDen, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChuyenDi, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TongChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TongBan, 0) + "</b></td><td></td></tr>");
                        TotalChuyenDen += TongChuyenDen;
                        TotalChuyenDi += TongChuyenDi;
                        TotalChet += TongChet;
                        TotalLoaiThai += TongLoaiThai;
                        TotalGietMo += TongGietMo;
                        TotalBan += TongBan;
                        TotalSoChuong++;
                        sb.Append("<tr><td align='center'><b>Tổng cộng<br/>(" + Config.ToXVal2(TotalSoChuong, 0) + " chuồng)</b></td><td></td><td align='left'></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDen, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TotalChuyenDi, 0) + "</b></td><td></td><td align='right'><b>" + Config.ToXVal2(TotalChet, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalLoaiThai, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalGietMo, 0) + "</b></td><td align='right'><b>" + Config.ToXVal2(TotalBan, 0) + "</b></td><td></td></tr>");
                    }
                    sb.Append("</tbody></table>");
                }
                ltt.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}