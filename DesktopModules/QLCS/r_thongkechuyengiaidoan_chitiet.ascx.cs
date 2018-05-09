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

namespace DotNetNuke.Modules.QLCS
{
    public partial class r_thongkechuyengiaidoan_chitiet : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        private void BindControls()
        {
            for (int i = 2011; i < DateTime.Now.Year + 1; i++)
            {
                lstYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == DateTime.Now.Year) lstYear.Items[lstYear.Items.Count - 1].Selected = true;
            }
            DataTable tblLoaiCa = new DataTable();
            tblLoaiCa = csCont.LoadLoaiCa(1);
            DataTable tblLoaiCa1 = tblLoaiCa.Clone();
            for (int i = tblLoaiCa.Rows.Count - 1; i > -1; i--)
            {
                tblLoaiCa1.Rows.Add(tblLoaiCa.Rows[i].ItemArray);
            }
            lstLoaiCa.DataSource = tblLoaiCa1;
            lstLoaiCa.DataTextField = "TenLoaiCa";
            lstLoaiCa.DataValueField = "IDLoaiCa";
            lstLoaiCa.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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
                string filename = "CaChuyenGiaiDoan.xls";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                string tieude = "";
                tieude += "<b>THỐNG KÊ CHI TIẾT CÁ CHUYỂN GIAI ĐOẠN NĂM " + Config.GetSelectedTexts(lstYear) + " - LOẠI CÁ " + Config.GetSelectedTexts(lstLoaiCa) + "</b>";

                string strSQL = "QLCS_BCTK_ChuyenLoaiCa_ChiTiet";
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@StrYear", Config.GetSelectedValues_At(lstYear));
                param[1] = new SqlParameter("@StrLoaiCa", Config.GetSelectedValues_At(lstLoaiCa));
                param[2] = new SqlParameter("@PhanLoai", int.Parse(ddlPhanLoai.SelectedValue));
                param[3] = new SqlParameter("@HienThiDuLieuSinhSan", chkDuLieuSinhSan.Checked);
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt;
                
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body style='text-align:center;font-family:Times New Roman;'><br/>
                    <div style='text-align:center;font-weight:bold;font-size:14pt;'>" + tieude + "</div><br/>");
                sb.Append("<table border='1'>");
                if (chkDuLieuSinhSan.Checked)
                {
                    int TDe = 0;
                    int TVo = 0;
                    int TTLoai = 0;
                    int THuy = 0;
                    int TKoP = 0;
                    int TCoP = 0;
                    int TCP1 = 0;
                    int TCL = 0;
                    int TCP2 = 0;
                    int TNo = 0;
                    int TongTrungAp = 0;

                    int currTDe = 0;
                    int currTVo = 0;
                    int currTTLoai = 0;
                    int currTHuy = 0;
                    int currTKoP = 0;
                    int currTCoP = 0;
                    int currTCP1 = 0;
                    int currTCL = 0;
                    int currTCP2 = 0;
                    int currTNo = 0;
                    int currTongTrungAp = 0;

                    int TrungHuy = 0;
                    int TrungAp = 0;
                    int CoPhoi = 0;
                    int ConLaiSauChetPhoi1 = 0;
                    int ConNo = 0;
                    int TongNgayAp = 0;

                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><td>STT</td><td>Loại cá</td><td>Ô chuồng</td><td>Vi cắt</td><td>Mã</td><td>Giới tính</td><td>Giống</td><td>Nguồn gốc</td><td>Trứng đẻ</td><td>Trứng ấp</td><td>Tỷ lệ trứng ấp/đẻ (%)</td><td>Có phôi</td><td>Tỷ lệ có phôi/ấp (%)</td><td>Con nở</td><td>Tỷ lệ nở/phôi (%)</td></tr>");
                    string TenLoaiCa = "";
                    string TenLoaiCaMoi = "";
                    string TenGioiTinh = "";
                    int currIndex = 1;
                    System.Text.StringBuilder s;
                    int countCa = 0;
                    for (int k = 0; k < ds.Tables.Count; k++)
                    {
                        dt = ds.Tables[k];
                        countCa += dt.Rows.Count;
                        string[] arrNam = Config.GetSelectedValue(lstYear, k);
                        sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>Năm " + arrNam[1] + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        for (int i = 1; i <= dt.Rows.Count; i++)
                        {
                            s = new System.Text.StringBuilder();
                            DataRow r = dt.Rows[i - 1];
                            if (TenLoaiCa != r["TenLoaiCa"].ToString() || TenLoaiCaMoi != r["TenLoaiCaMoi"].ToString() || TenGioiTinh != r["TenGioiTinh"].ToString())
                            {
                                currIndex = 1;
                                TenLoaiCa = r["TenLoaiCa"].ToString();
                                TenLoaiCaMoi = r["TenLoaiCaMoi"].ToString();
                                TenGioiTinh = r["TenGioiTinh"].ToString();
                                s.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td style='text-align:left;'>" + TenLoaiCa + " - " + TenGioiTinh + " --> " + r["TenLoaiCaMoi"].ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                            }
                            s.Append("<tr style='vertical-align:middle;'>");
                            s.Append("<td>" + currIndex.ToString() + "</td>");
                            currIndex++;
                            s.Append("<td style='text-align:left;'>" + TenLoaiCa + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["TenChuong"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["MaSoGoc"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["MaSo"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + TenGioiTinh + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["TenGiong"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["TenNguonGoc"].ToString() + "</td>");

                            if (r["TrungDe"] != DBNull.Value && Convert.ToInt32(r["TrungDe"]) != 0)
                            {
                                TrungHuy = Convert.ToInt32(r["TrungVo"]) + Convert.ToInt32(r["TrungThaiLoai"]);
                                TrungAp = Convert.ToInt32(r["TrungDe"]) - TrungHuy;
                                CoPhoi = TrungAp - Convert.ToInt32(r["TrungKhongPhoi"]);
                                ConNo = CoPhoi - Convert.ToInt32(r["TrungChetPhoi1"]) - Convert.ToInt32(r["TrungChetPhoi2"]);
                                s.Append("<td style='text-align:right;'>" + r["TrungDe"].ToString() + "</td>");
                                s.Append("<td style='text-align:right;'>" + TrungAp.ToString() + "</td>");
                                s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(100 * TrungAp / Convert.ToDecimal(r["TrungDe"]), 1) + "</td>");
                                s.Append("<td style='text-align:right;'>" + CoPhoi.ToString() + "</td>");
                                if (TrungAp != 0)
                                {
                                    s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(100 * CoPhoi / Convert.ToDecimal(TrungAp), 1) + "</td>");
                                }
                                else
                                {
                                    s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>0</td>");
                                }
                                s.Append("<td style='text-align:right;'>" + ConNo.ToString() + "</td>");
                                if (CoPhoi != 0)
                                {
                                    s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(100 * ConNo / Convert.ToDecimal(CoPhoi), 1) + "</td></tr>");
                                }
                                else
                                {
                                    s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>0</td></tr>");
                                }
                            }
                            else
                            {
                                s.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                            }
                            sb.Append(s.ToString());
                        }
                        sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>T.C</td><td align='right'><b>" + Config.ToXVal2(dt.Rows.Count, 0) + "</b></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    }
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>Tổng cộng</td><td align='right'><b>" + Config.ToXVal2(countCa, 0) + "</b></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                }
                else
                {
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><td>STT</td><td>Loại cá</td><td>Ô chuồng</td><td>Vi cắt</td><td>Mã</td><td>Giới tính</td><td>Giống</td><td>Nguồn gốc</td></tr>");
                    string TenLoaiCa = "";
                    string TenLoaiCaMoi = "";
                    string TenGioiTinh = "";
                    int currIndex = 1;
                    System.Text.StringBuilder s;
                    int countCa = 0;
                    for (int k = 0; k < ds.Tables.Count; k++)
                    {
                        dt = ds.Tables[k];
                        countCa += dt.Rows.Count;
                        string[] arrNam = Config.GetSelectedValue(lstYear, k);
                        sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>Năm " + arrNam[1] + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        for (int i = 1; i <= dt.Rows.Count; i++)
                        {
                            s = new System.Text.StringBuilder();
                            DataRow r = dt.Rows[i - 1];
                            if (TenLoaiCa != r["TenLoaiCa"].ToString() || TenLoaiCaMoi != r["TenLoaiCaMoi"].ToString() || TenGioiTinh != r["TenGioiTinh"].ToString())
                            {
                                currIndex = 1;
                                TenLoaiCa = r["TenLoaiCa"].ToString();
                                TenLoaiCaMoi = r["TenLoaiCaMoi"].ToString();
                                TenGioiTinh = r["TenGioiTinh"].ToString();
                                s.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td style='text-align:left;'>" + TenLoaiCa + " - " + TenGioiTinh + " --> " + r["TenLoaiCaMoi"].ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                            }
                            s.Append("<tr style='vertical-align:middle;'>");
                            s.Append("<td>" + currIndex.ToString() + "</td>");
                            currIndex++;
                            s.Append("<td style='text-align:left;'>" + TenLoaiCa + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["TenChuong"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["MaSoGoc"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["MaSo"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + TenGioiTinh + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["TenGiong"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["TenNguonGoc"].ToString() + "</td></tr>");
                            sb.Append(s.ToString());
                        }
                        sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>T.C</td><td align='right'><b>" + Config.ToXVal2(dt.Rows.Count, 0) + "</b></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    }
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>Tổng cộng</td><td align='right'><b>" + Config.ToXVal2(countCa, 0) + "</b></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                }
                sb.Append("</table>");
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
                string tieude = "";
                tieude += "<b>THỐNG KÊ CHI TIẾT CÁ CHUYỂN GIAI ĐOẠN NĂM " + Config.GetSelectedTexts(lstYear) + "<br/>LOẠI CÁ " + Config.GetSelectedTexts(lstLoaiCa) + "</b>";

                string strSQL = "QLCS_BCTK_ChuyenLoaiCa_ChiTiet";
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@StrYear", Config.GetSelectedValues_At(lstYear));
                param[1] = new SqlParameter("@StrLoaiCa", Config.GetSelectedValues_At(lstLoaiCa));
                param[2] = new SqlParameter("@PhanLoai", int.Parse(ddlPhanLoai.SelectedValue));
                param[3] = new SqlParameter("@HienThiDuLieuSinhSan", chkDuLieuSinhSan.Checked);
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<div style='text-align:center;font-weight:bold;font-size:14pt;'>" + tieude + "</div><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                if (chkDuLieuSinhSan.Checked)
                {
                    int TDe = 0;
                    int TVo = 0;
                    int TTLoai = 0;
                    int THuy = 0;
                    int TKoP = 0;
                    int TCoP = 0;
                    int TCP1 = 0;
                    int TCL = 0;
                    int TCP2 = 0;
                    int TNo = 0;
                    int TongTrungAp = 0;

                    int currTDe = 0;
                    int currTVo = 0;
                    int currTTLoai = 0;
                    int currTHuy = 0;
                    int currTKoP = 0;
                    int currTCoP = 0;
                    int currTCP1 = 0;
                    int currTCL = 0;
                    int currTCP2 = 0;
                    int currTNo = 0;
                    int currTongTrungAp = 0;

                    int TrungHuy = 0;
                    int TrungAp = 0;
                    int CoPhoi = 0;
                    int ConLaiSauChetPhoi1 = 0;
                    int ConNo = 0;
                    int TongNgayAp = 0;

                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><td>STT</td><td>Loại cá</td><td>Ô chuồng</td><td>Vi cắt</td><td>Mã</td><td>Giới tính</td><td>Giống</td><td>Nguồn gốc</td><td>Trứng đẻ</td><td>Trứng ấp</td><td>Tỷ lệ trứng ấp/đẻ (%)</td><td>Có phôi</td><td>Tỷ lệ có phôi/ấp (%)</td><td>Con nở</td><td>Tỷ lệ nở/phôi (%)</td></tr></thead><tbody>");
                    string TenLoaiCa = "";
                    string TenLoaiCaMoi = "";
                    string TenGioiTinh = "";
                    int currIndex = 1;
                    System.Text.StringBuilder s;
                    int countCa = 0;
                    for (int k = 0; k < ds.Tables.Count; k++)
                    {
                        dt = ds.Tables[k];
                        countCa += dt.Rows.Count;
                        string[] arrNam = Config.GetSelectedValue(lstYear, k);
                        sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>Năm " + arrNam[1] + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        for (int i = 1; i <= dt.Rows.Count; i++)
                        {
                            s = new System.Text.StringBuilder();
                            DataRow r = dt.Rows[i - 1];
                            if (TenLoaiCa != r["TenLoaiCa"].ToString() || TenLoaiCaMoi != r["TenLoaiCaMoi"].ToString() || TenGioiTinh != r["TenGioiTinh"].ToString())
                            {
                                currIndex = 1;
                                TenLoaiCa = r["TenLoaiCa"].ToString();
                                TenLoaiCaMoi = r["TenLoaiCaMoi"].ToString();
                                TenGioiTinh = r["TenGioiTinh"].ToString();
                                s.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td style='text-align:left;'>" + TenLoaiCa + " - " + TenGioiTinh + " --> " + r["TenLoaiCaMoi"].ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                            }
                            s.Append("<tr style='vertical-align:middle;'>");
                            s.Append("<td>" + currIndex.ToString() + "</td>");
                            currIndex++;
                            s.Append("<td style='text-align:left;'>" + TenLoaiCa + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["TenChuong"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["MaSoGoc"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["MaSo"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + TenGioiTinh + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["TenGiong"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["TenNguonGoc"].ToString() + "</td>");

                            if (r["TrungDe"] != DBNull.Value && Convert.ToInt32(r["TrungDe"]) != 0)
                            {
                                TrungHuy = Convert.ToInt32(r["TrungVo"]) + Convert.ToInt32(r["TrungThaiLoai"]);
                                TrungAp = Convert.ToInt32(r["TrungDe"]) - TrungHuy;
                                CoPhoi = TrungAp - Convert.ToInt32(r["TrungKhongPhoi"]);
                                ConNo = CoPhoi - Convert.ToInt32(r["TrungChetPhoi1"]) - Convert.ToInt32(r["TrungChetPhoi2"]);
                                s.Append("<td style='text-align:right;'>" + r["TrungDe"].ToString() + "</td>");
                                s.Append("<td style='text-align:right;'>" + TrungAp.ToString() + "</td>");
                                s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(100 * TrungAp / Convert.ToDecimal(r["TrungDe"]), 1) + "</td>");
                                s.Append("<td style='text-align:right;'>" + CoPhoi.ToString() + "</td>");
                                if (TrungAp != 0)
                                {
                                    s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(100 * CoPhoi / Convert.ToDecimal(TrungAp), 1) + "</td>");
                                }
                                else
                                {
                                    s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>0</td>");
                                }
                                s.Append("<td style='text-align:right;'>" + ConNo.ToString() + "</td>");
                                if (CoPhoi != 0)
                                {
                                    s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(100 * ConNo / Convert.ToDecimal(CoPhoi), 1) + "</td></tr>");
                                }
                                else
                                {
                                    s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>0</td></tr>");
                                }
                            }
                            else
                            {
                                s.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                            }
                            sb.Append(s.ToString());
                        }
                        sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>T.C</td><td align='right'><b>" + Config.ToXVal2(dt.Rows.Count, 0) + "</b></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    }
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>Tổng cộng</td><td align='right'><b>" + Config.ToXVal2(countCa, 0) + "</b></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                }
                else
                {
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><td>STT</td><td>Loại cá</td><td>Ô chuồng</td><td>Vi cắt</td><td>Mã</td><td>Giới tính</td><td>Giống</td><td>Nguồn gốc</td></tr></thead><tbody>");
                    string TenLoaiCa = "";
                    string TenLoaiCaMoi = "";
                    string TenGioiTinh = "";
                    int currIndex = 1;
                    System.Text.StringBuilder s;
                    int countCa = 0;
                    for (int k = 0; k < ds.Tables.Count; k++)
                    {
                        dt = ds.Tables[k];
                        countCa += dt.Rows.Count;
                        string[] arrNam = Config.GetSelectedValue(lstYear, k);
                        sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>Năm " + arrNam[1] + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        for (int i = 1; i <= dt.Rows.Count; i++)
                        {
                            s = new System.Text.StringBuilder();
                            DataRow r = dt.Rows[i - 1];
                            if (TenLoaiCa != r["TenLoaiCa"].ToString() || TenLoaiCaMoi != r["TenLoaiCaMoi"].ToString() || TenGioiTinh != r["TenGioiTinh"].ToString())
                            {
                                currIndex = 1;
                                TenLoaiCa = r["TenLoaiCa"].ToString();
                                TenLoaiCaMoi = r["TenLoaiCaMoi"].ToString();
                                TenGioiTinh = r["TenGioiTinh"].ToString();
                                s.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td style='text-align:left;'>" + TenLoaiCa + " - " + TenGioiTinh + " --> " + r["TenLoaiCaMoi"].ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                            }
                            s.Append("<tr style='vertical-align:middle;'>");
                            s.Append("<td>" + currIndex.ToString() + "</td>");
                            currIndex++;
                            s.Append("<td style='text-align:left;'>" + TenLoaiCa + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["TenChuong"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["MaSoGoc"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["MaSo"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + TenGioiTinh + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["TenGiong"].ToString() + "</td>");
                            s.Append("<td style='text-align:left;'>" + r["TenNguonGoc"].ToString() + "</td></tr>");
                            sb.Append(s.ToString());
                        }
                        sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>T.C</td><td align='right'><b>" + Config.ToXVal2(dt.Rows.Count, 0) + "</b></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    }
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>Tổng cộng</td><td align='right'><b>" + Config.ToXVal2(countCa, 0) + "</b></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                }
                sb.Append("</tbody></table>");
                lt.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}