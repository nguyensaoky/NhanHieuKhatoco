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
    public partial class r_gietmocatheongay : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        int scale = 0;
        CaSauController csCont = new CaSauController();
        string sVatTuGietMo = ConfigurationManager.AppSettings["QLCS_VatTuGietMo"];
        DataTable tblVatTu = null;

        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable tblLoaiCa = new DataTable();
            tblLoaiCa = csCont.LoadLoaiCa(1);
            lstLoaiCa.DataSource = tblLoaiCa;
            lstLoaiCa.DataTextField = "TenLoaiCa";
            lstLoaiCa.DataValueField = "IDLoaiCa";
            lstLoaiCa.DataBind();

            lstVatTu.DataSource = tblVatTu;
            lstVatTu.DataTextField = "TenFull";
            lstVatTu.DataValueField = "IDVatTu";
            lstVatTu.DataBind();
            foreach (ListItem i in lstVatTu.Items)
	        {
                i.Selected = true;
        	} 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
            try
            {
                tblVatTu = csCont.VatTu_GetByString(sVatTuGietMo);
                scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_SPGM_Scale"]);
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

        private string tds(int num)
        { 
            string res = "";
            for (int i = 0; i < num; i++)
            {
                res += "<td></td>";
            }
            return res;
        }

        private string tds(decimal[] a)
        {
            string res = "";
            for (int i = 0; i < a.Length; i++)
            {
                res += "<td style='text-align:right;'>" + Config.ToXVal2(a[i], 0) + "</td>";
            }
            return res;
        }

        private string value(string sVatTu, ArrayList arrVatTu, out decimal[] SoLuong)
        {
            SoLuong = new decimal[arrVatTu.Count];
            string res = "";
            for (int l = 0; l < arrVatTu.Count; l++ )
            {
                string i = arrVatTu[l].ToString();
                int j = sVatTu.IndexOf("@" + i + "/");
                if (j >= 0)
                {
                    int k = sVatTu.IndexOf('@', j + 1);
                    decimal t = decimal.Parse(sVatTu.Substring(j + i.Length + 2, k - (j + i.Length + 2)));
                    res += "<td style='text-align:right;'>" + Config.ToXVal2(t, 0) + "</td>";
                    SoLuong[l] = t;
                }
                else
                {
                    res += "<td style='text-align:right;'>0</td>";
                    SoLuong[l] = 0;
                }
            }
            return res;
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string filename = "GietMoCaTheoNgay";
                string tieude = "";
                string strSQL = "QLCS_BCTK_GietMo_ChiTietCa_LoaiCa";
                if (ddlChuan.SelectedValue == "1")
                {
                    strSQL = "QLCS_BCTK_GietMo_ChiTietCa_LoaiCa_ChuanCu";
                }
                string strSQLSub = "QLCS_BCTK_GietMo_ChiTietSanPham_LoaiCa";
                SqlParameter[] param = new SqlParameter[3];
                SqlParameter[] paramSub = new SqlParameter[3];
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                param[2] = new SqlParameter("@StrLoaiCa", Config.GetSelectedValues_At(lstLoaiCa));
                paramSub[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                paramSub[1] = new SqlParameter("@dTo", txtToDate.Text);
                paramSub[2] = new SqlParameter("@StrLoaiCa", Config.GetSelectedValues_At(lstLoaiCa));
                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                filename = filename.Replace("/", "_");
                tieude += "<b>SẢN PHẨM THU HỒI TỪ GIẾT MỔ CÁ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                //DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                DataSet ds = Config.SelectSPs(strSQL, param);
                //DataTable dtSub = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLSub, param);
                DataSet dsSub = Config.SelectSPs(strSQLSub, paramSub);

                int soluongVatTu = 0;
                string CotVatTu = "";
                ArrayList arrVatTu = new ArrayList();
                foreach (ListItem item in lstVatTu.Items)
                {
                    if (item.Selected)
                    {
                        soluongVatTu++;
                        CotVatTu += "<td>" + item.Text + "</td>";
                        arrVatTu.Add(item.Value);
                    }
                }
                int soluongCot = 4 + soluongVatTu;

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table border='1'>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <td rowspan=2>STT</td><td rowspan=2>Số biên bản</td><td rowspan=2>Ngày Mổ</td><td rowspan=2>Tuổi cá</td><td rowspan=2>Loại cá</td><td rowspan=2>Chuồng</td><td rowspan=2>Ngày nhập chuồng</td><td rowspan=2>Phân loại</td><td colspan='" + soluongCot.ToString() + @"'>SP.DA</td><td rowspan=2>Người mổ</td><td rowspan=2>Khối lượng hơi (kg)</td><td rowspan=2>Khối lượng móc hàm (kg)</td><td rowspan=2>Tỷ lệ móc hàm (%)</td><td rowspan=2>Sản phẩm sau fillet</td><td rowspan=2>Tổng cộng (kg)</td><td rowspan=2>Tỷ lệ / Khối lượng móc hàm (%)</td><td rowspan=2>Tỷ lệ / Khối lượng hơi</td>
                        </tr>
                        <tr style='font-weight:bold; vertical-align:middle;'><td>Khối lượng (kg)</td><td>Kích thước<br/>da (cm)</td><td>Loại da</td><td>Da (CB/CL/MDL)</td>" + CotVatTu + "</tr>");
                string[] aLoaiCa = Config.GetSelectedTexts(lstLoaiCa).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                int Supstt = 0;
                decimal[] SupTongSoVatTu = new decimal[soluongVatTu];
                int q = 0;
                for (q = 0; q < soluongVatTu; q++)
                {
                    SupTongSoVatTu[q] = 0;
                }
                decimal SuptotalTLDa = 0;
                decimal SuptotalTLSP = 0;
                decimal SuptotalHoi = 0;
                decimal SuptotalMocHam = 0;
                for (int u = 0; u < ds.Tables.Count; u++ )
                {
                    DataTable dt = ds.Tables[u];
                    DataTable dtSub = dsSub.Tables[u];
                    if (aLoaiCa.Length > 0)
                        sb.Append("<tr style='font-weight:bold;color:#00F;'><td style='text-align:left;'>Loại cá</td><td style='text-align:left'>" + aLoaiCa[u] + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>" + tds(soluongVatTu) + "</tr>");
                    bool recalc = true;
                    decimal tongTLDa = 0;
                    decimal tongTLSP = 0;
                    decimal tongTLeMocHam = 0;
                    decimal tongTLeTLHoi = 0;
                    int tongSoLuong = 0;
                    decimal[] tongSoVatTu = new decimal[soluongVatTu];
                    for (q = 0; q < soluongVatTu; q++)
                    {
                        tongSoVatTu[q] = 0;
                    }
                    decimal totalTLDa = 0;
                    decimal totalTLSP = 0;
                    decimal totalHoi = 0;
                    decimal totalMocHam = 0;
                    decimal kl;
                    int i = 0;
                    int iSub = 0;
                    int stt = 0;
                    decimal[] Sup1TongSoVatTu = new decimal[soluongVatTu];
                    for (q = 0; q < soluongVatTu; q++)
                    {
                        Sup1TongSoVatTu[q] = 0;
                    }
                    DateTime currDate = DateTime.MinValue;
                    DateTime currDateSub = DateTime.MinValue;
                    decimal currHoi = 0; decimal currMocHam = 0;
                    if (dt.Rows.Count > 0)
                    {
                        currDate = Convert.ToDateTime(dt.Rows[0]["NgayMo"]);
                        currDateSub = Convert.ToDateTime(dtSub.Rows[0]["NgayMo"]);
                    }
                    while (i <= dt.Rows.Count && iSub <= dtSub.Rows.Count)
                    {
                        DataRow r = null;
                        DataRow rSub = null;
                        DateTime date;
                        DateTime dateSub;
                        if (i == dt.Rows.Count)
                        {
                            date = DateTime.MaxValue;
                        }
                        else
                        {
                            r = dt.Rows[i];
                            date = Convert.ToDateTime(r["NgayMo"]);
                            if (recalc)
                            {
                                currHoi = Convert.ToDecimal(r["TongTrongLuongHoi"]);
                                currMocHam = Convert.ToDecimal(r["TongTrongLuongMocHam"]);
                                recalc = false;
                            }
                        }
                        if (iSub == dtSub.Rows.Count)
                            dateSub = DateTime.MaxValue;
                        else
                        {
                            rSub = dtSub.Rows[iSub];
                            dateSub = Convert.ToDateTime(rSub["NgayMo"]);
                        }
                        if (date == currDate)
                        {
                            string TuoiCa = "";
                            //string PhanLoai = "Thương phẩm";
                            if (r["NgayNo"] != DBNull.Value)
                            {
                                int SoNgay = Config.Days360(Convert.ToDateTime(r["NgayMo"]), Convert.ToDateTime(r["NgayNo"]));
                                double dTuoiCa = (double)SoNgay / 30;
                                //if (dTuoiCa < 18 && !r["TenLoaiCa"].ToString().Contains("2,3,4")) PhanLoai = "Loại thải";
                                //TuoiCa = Math.Round(dTuoiCa, 2).ToString();
                                TuoiCa = Config.ToXVal2(dTuoiCa, 2);
                            }
                            else
                            {
                                TuoiCa = "";
                            }

                            //if (Convert.ToDecimal(r["TrongLuongHoi"]) < 15 && !r["TenLoaiCa"].ToString().Contains("2,3,4")) PhanLoai = "Loại thải";

                            tongSoLuong++;
                            stt++;
                            tongTLDa += (decimal)r["Da_TrongLuong"];
                            decimal[] soVatTuTemp = null;
                            string sTD = value(r["VatTu"].ToString(), arrVatTu, out soVatTuTemp);
                            for (q = 0; q < soluongVatTu; q++)
                            {
                                tongSoVatTu[q] += soVatTuTemp[q];
                                Sup1TongSoVatTu[q] += soVatTuTemp[q];
                            }
                            sb.Append("<tr><td>" + stt.ToString() + "</td><td style='text-align:left;'>" + r["BienBan"].ToString() + @"</td><td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + currDate.ToString("dd/MM/yyyy") + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + TuoiCa + "</td>");
                            if (((decimal)r["TrongLuongHoi"])!=0)
                                sb.Append("<td style='text-align:left;'>" + r["TenLoaiCa"].ToString() + "</td><td style='text-align:left;'>" + r["Chuong"].ToString() + @"</td><td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayNhapChuong"]).ToString("dd/MM/yyyy") + "</td><td style='text-align:left;'>" + r["PhanLoai"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["Da_TrongLuong"], 0) + "</td><td style='text-align:right;'>" + r["Da_Bung"].ToString() + "</td><td style='text-align:center;'>" + r["Da_PhanLoai"].ToString() + @"<td style='text-align:left;'>" + r["PhuongPhapMo"].ToString() + @"</td>" + sTD + "<td align='left'>" + r["TenNguoiMo"].ToString() + "</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["TrongLuongHoi"], scale) + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["TrongLuongMocHam"], scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(((decimal)r["TrongLuongMocHam"]) / ((decimal)r["TrongLuongHoi"]) * 100, 2) + "</td>");
                            else
                                sb.Append("<td style='text-align:left;'>" + r["TenLoaiCa"].ToString() + "</td><td style='text-align:left;'>" + r["Chuong"].ToString() + @"</td><td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayNhapChuong"]).ToString("dd/MM/yyyy") + "</td><td style='text-align:left;'>" + r["PhanLoai"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["Da_TrongLuong"], 0) + "</td><td style='text-align:right;'>" + r["Da_Bung"].ToString() + "</td><td style='text-align:center;'>" + r["Da_PhanLoai"].ToString() + @"<td style='text-align:left;'>" + r["PhuongPhapMo"].ToString() + @"</td>" + sTD + "<td align='left'>" + r["TenNguoiMo"].ToString() + "</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["TrongLuongHoi"], scale) + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["TrongLuongMocHam"], scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(0, 2) + "</td>");
                            if (dateSub == currDateSub && dateSub == currDate)
                            {
                                if (rSub["KhoiLuong"] == DBNull.Value)
                                {
                                    kl = 0;
                                }
                                else
                                {
                                    kl = (decimal)rSub["KhoiLuong"];
                                }
                                tongTLSP += kl;
                                if (currMocHam != 0)
                                {
                                    tongTLeMocHam += kl / currMocHam * 100;
                                    tongTLeTLHoi += kl / currHoi * 100;
                                    sb.Append("<td style='text-align:left;'>" + rSub["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(rSub["KhoiLuong"], scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(kl / currMocHam * 100, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(kl / currHoi * 100, 2) + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td style='text-align:left;'>" + rSub["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(rSub["KhoiLuong"], scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(0, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(0, 2) + "</td>");
                                }
                            }
                            else
                            {
                                sb.Append("<td></td><td></td><td></td><td></td>");
                                iSub--;
                            }
                            sb.Append("</tr>");
                        }
                        else
                        {
                            if (dateSub == currDateSub && dateSub == currDate)
                            {
                                //sb.Append("<tr><td></td><td style='text-align:left;'>" + rSub["BienBan"].ToString() + "</td><td style='text-align:center;'>" + currDateSub.ToString("dd/MM/yyyy") + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
                                sb.Append("<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>" + tds(soluongVatTu));
                                if (rSub["KhoiLuong"] == DBNull.Value)
                                {
                                    kl = 0;
                                }
                                else
                                {
                                    kl = (decimal)rSub["KhoiLuong"];
                                }
                                tongTLSP += kl;
                                if (currMocHam != 0)
                                {
                                    tongTLeMocHam += kl / currMocHam * 100;
                                    tongTLeTLHoi += kl / currHoi * 100;
                                    sb.Append("<td style='text-align:left;'>" + rSub["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(rSub["KhoiLuong"], scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(kl / currMocHam * 100, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(kl / currHoi * 100, 2) + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td style='text-align:left;'>" + rSub["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(rSub["KhoiLuong"], scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(0, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(0, 2) + "</td>");
                                }
                                i--;
                                sb.Append("</tr>");
                            }
                            else
                            {
                                if (currHoi == 0) sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td><td style='text-align:right;'>" + tongSoLuong.ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(tongTLDa, 0) + @"</td><td></td><td></td><td></td>" + tds(tongSoVatTu) + "<td></td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(currHoi, scale) + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(currMocHam, scale) + @"</td><td style='text-align:right;'>0</td><td></td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTLSP, scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tongTLeMocHam, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tongTLeTLHoi, 2) + "</td>");
                                else sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td><td style='text-align:right;'>" + tongSoLuong.ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(tongTLDa, 0) + @"</td><td></td><td></td><td></td>" + tds(tongSoVatTu) + "<td></td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(currHoi, scale) + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(currMocHam, scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(currMocHam / currHoi * 100, 2) + @"</td><td></td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTLSP, scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tongTLeMocHam, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tongTLeTLHoi, 2) + "</td>");
                                sb.Append("</tr>");
                                totalTLDa += tongTLDa;
                                totalTLSP += tongTLSP;
                                totalHoi += currHoi;
                                totalMocHam += currMocHam;
                                if (date == DateTime.MaxValue && dateSub == DateTime.MaxValue) break;
                                tongSoLuong = 0;
                                for (q = 0; q < soluongVatTu; q++)
                                {
                                    tongSoVatTu[q] = 0;
                                }
                                tongTLSP = 0;
                                tongTLDa = 0;
                                tongTLeMocHam = 0;
                                tongTLeTLHoi = 0;
                                i--;
                                iSub--;
                                if (date != DateTime.MaxValue) currDate = date;
                                if (dateSub != DateTime.MaxValue) currDateSub = dateSub;
                                recalc = true;
                            }
                        }
                        i++;
                        iSub++;
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>Tổng cộng</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(stt, 0) + "</td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(totalTLDa, 0) + @"</td><td></td><td></td><td></td>" + tds(Sup1TongSoVatTu) + "<td></td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalHoi, scale) + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalMocHam, scale) + @"</td><td></td><td></td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalTLSP, scale) + @"</td><td></td><td></td></tr>");
                    Supstt += stt;
                    for (q = 0; q < soluongVatTu; q++)
                    {
                        SupTongSoVatTu[q] += Sup1TongSoVatTu[q];
                    }
                    SuptotalTLDa += totalTLDa;
                    SuptotalHoi += totalHoi;
                    SuptotalMocHam += totalMocHam;
                    SuptotalTLSP += totalTLSP;
                }
                if (ds.Tables.Count > 1)
                {
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>TỔNG CÁC LOẠI CÁ</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(Supstt, 0) + "</td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(SuptotalTLDa, 0) + @"</td><td></td><td></td><td></td>" + tds(SupTongSoVatTu) + "<td></td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(SuptotalHoi, scale) + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(SuptotalMocHam, scale) + @"</td><td></td><td></td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(SuptotalTLSP, scale) + @"</td><td></td><td></td></tr>");
                }
                sb.Append("</table><br/><br/>");

                // Ca loai thai
                sb.Append(CaLoaiThai(true));

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
                string strSQL = "QLCS_BCTK_GietMo_ChiTietCa_LoaiCa";
                if (ddlChuan.SelectedValue == "1")
                {
                    strSQL = "QLCS_BCTK_GietMo_ChiTietCa_LoaiCa_ChuanCu";
                }
                string strSQLSub = "QLCS_BCTK_GietMo_ChiTietSanPham_LoaiCa";
                SqlParameter[] param = new SqlParameter[3];
                SqlParameter[] paramSub = new SqlParameter[3];
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                param[2] = new SqlParameter("@StrLoaiCa", Config.GetSelectedValues_At(lstLoaiCa));
                paramSub[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                paramSub[1] = new SqlParameter("@dTo", txtToDate.Text);
                paramSub[2] = new SqlParameter("@StrLoaiCa", Config.GetSelectedValues_At(lstLoaiCa));
                tieude += "<b>SẢN PHẨM THU HỒI TỪ GIẾT MỔ CÁ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                //DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                DataSet ds = Config.SelectSPs(strSQL, param);
                //DataTable dtSub = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLSub, param);
                DataSet dsSub = Config.SelectSPs(strSQLSub, paramSub);

                int soluongVatTu = 0;
                string CotVatTu = "";
                ArrayList arrVatTu = new ArrayList();
                foreach (ListItem item in lstVatTu.Items)
                {
                    if (item.Selected)
                    {
                        soluongVatTu++;
                        CotVatTu += "<th>" + item.Text + "</th>";
                        arrVatTu.Add(item.Value);
                    }
                }
                int soluongCot = 4 + soluongVatTu;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <th rowspan=2>STT</th><th rowspan=2>Số biên bản</th><th rowspan=2>Ngày Mổ</th><th rowspan=2>Tuổi cá</th><th rowspan=2>Loại cá</th><th rowspan=2>Chuồng</th><th rowspan=2>Ngày nhập chuồng</th><th rowspan=2>Phân loại</th><th colspan='" + soluongCot.ToString() + @"'>SP.DA</th><th rowspan=2>Người mổ</th><th rowspan=2>Khối lượng hơi (kg)</th><th rowspan=2>Khối lượng móc hàm (kg)</th><th rowspan=2>Tỷ lệ móc hàm (%)</th><th rowspan=2>Sản phẩm sau fillet</th><th rowspan=2>Tổng cộng (kg)</th><th rowspan=2>Tỷ lệ / <br/>Khối lượng móc hàm (%)</th><th rowspan=2>Tỷ lệ / <br/>Khối lượng hơi</th></tr>
                        <tr style='font-weight:bold; vertical-align:middle;'><th>Khối lượng (kg)</th><th>Kích thước da (cm)</th><th>Loại da</th><th>Da (CB/CL/MDL)</th>" + CotVatTu + "</tr></thead><tbody>");
                string[] aLoaiCa = Config.GetSelectedTexts(lstLoaiCa).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                int Supstt = 0;
                decimal[] SupTongSoVatTu = new decimal[soluongVatTu];
                int q = 0;
                for (q = 0; q < soluongVatTu; q++)
                {
                    SupTongSoVatTu[q] = 0;
                }
                decimal SuptotalTLDa = 0;
                decimal SuptotalTLSP = 0;
                decimal SuptotalHoi = 0;
                decimal SuptotalMocHam = 0;
                for (int u = 0; u < ds.Tables.Count; u++)
                {
                    DataTable dt = ds.Tables[u];
                    DataTable dtSub = dsSub.Tables[u];
                    if(aLoaiCa.Length > 0)
                        sb.Append("<tr style='font-weight:bold;color:#00F;'><td style='text-align:left;'>Loại cá</td><td style='text-align:left'>" + aLoaiCa[u] + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>" + tds(soluongVatTu) + "</tr>");
                    bool recalc = true;
                    decimal tongTLDa = 0;
                    decimal tongTLSP = 0;
                    decimal tongTLeMocHam = 0;
                    decimal tongTLeTLHoi = 0;
                    int tongSoLuong = 0;
                    decimal[] tongSoVatTu = new decimal[soluongVatTu];
                    for (q = 0; q < soluongVatTu; q++)
                    {
                        tongSoVatTu[q] = 0;
                    }
                    decimal totalTLDa = 0;
                    decimal totalTLSP = 0;
                    decimal totalHoi = 0;
                    decimal totalMocHam = 0;
                    decimal kl;
                    int i = 0;
                    int iSub = 0;
                    int stt = 0;
                    decimal[] Sup1TongSoVatTu = new decimal[soluongVatTu];
                    for (q = 0; q < soluongVatTu; q++)
                    {
                        Sup1TongSoVatTu[q] = 0;
                    }
                    DateTime currDate = DateTime.MinValue;
                    DateTime currDateSub = DateTime.MinValue;
                    decimal currHoi = 0; decimal currMocHam = 0;
                    if (dt.Rows.Count > 0)
                    {
                        currDate = Convert.ToDateTime(dt.Rows[0]["NgayMo"]);
                        currDateSub = Convert.ToDateTime(dtSub.Rows[0]["NgayMo"]);
                    }
                    while (i <= dt.Rows.Count && iSub <= dtSub.Rows.Count)
                    {
                        DataRow r = null;
                        DataRow rSub = null;
                        DateTime date;
                        DateTime dateSub;
                        if (i == dt.Rows.Count)
                        {
                            date = DateTime.MaxValue;
                        }
                        else
                        {
                            r = dt.Rows[i];
                            date = Convert.ToDateTime(r["NgayMo"]);
                            if (recalc)
                            {
                                currHoi = Convert.ToDecimal(r["TongTrongLuongHoi"]);
                                currMocHam = Convert.ToDecimal(r["TongTrongLuongMocHam"]);
                                recalc = false;
                            }
                        }
                        if (iSub == dtSub.Rows.Count)
                            dateSub = DateTime.MaxValue;
                        else
                        {
                            rSub = dtSub.Rows[iSub];
                            dateSub = Convert.ToDateTime(rSub["NgayMo"]);
                        }
                        if (date == currDate)
                        {
                            string TuoiCa = "";
                            //string PhanLoai = "Thương phẩm";
                            if (r["NgayNo"] != DBNull.Value)
                            {
                                int SoNgay = Config.Days360(Convert.ToDateTime(r["NgayMo"]), Convert.ToDateTime(r["NgayNo"]));
                                double dTuoiCa = (double)SoNgay / 30;
                                //TuoiCa = Math.Round(dTuoiCa, 2).ToString();
                                //if (dTuoiCa < 18 && !r["TenLoaiCa"].ToString().Contains("2,3,4")) PhanLoai = "Loại thải";
                                TuoiCa = Config.ToXVal2(dTuoiCa, 2);
                            }
                            else
                            {
                                TuoiCa = "";
                            }

                            //if (Convert.ToDecimal(r["TrongLuongHoi"]) < 15 && !r["TenLoaiCa"].ToString().Contains("2,3,4")) PhanLoai = "Loại thải";

                            tongSoLuong++;
                            stt++;
                            tongTLDa += (decimal)r["Da_TrongLuong"];
                            decimal[] soVatTuTemp = null;
                            string sTD = value(r["VatTu"].ToString(), arrVatTu, out soVatTuTemp);
                            for (q = 0; q < soluongVatTu; q++)
                            {
                                tongSoVatTu[q] += soVatTuTemp[q];
                                Sup1TongSoVatTu[q] += soVatTuTemp[q];
                            }
                            sb.Append("<tr><td style='text-align:center;'>" + stt.ToString() + "</td><td>" + r["BienBan"].ToString() + @"</td><td style='text-align:center;'>" + currDate.ToString("dd/MM/yyyy") + "</td><td style='text-align:right;'>" + TuoiCa + "</td>");
                            if (((decimal)r["TrongLuongHoi"]) != 0)
                                sb.Append("<td>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["Chuong"].ToString() + "</td><td>" + Convert.ToDateTime(r["NgayNhapChuong"]).ToString("dd/MM/yyyy") + "</td><td>" + r["PhanLoai"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["Da_TrongLuong"], 0) + "</td><td style='text-align:right;'>" + r["Da_Bung"].ToString() + "</td><td style='text-align:center;'>" + r["Da_PhanLoai"].ToString() + "</td><td>" + r["PhuongPhapMo"].ToString() + "</td>" + sTD + "<td style='text-align:left;'>" + r["TenNguoiMo"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["TrongLuongHoi"], 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["TrongLuongMocHam"], 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(((decimal)r["TrongLuongMocHam"]) / ((decimal)r["TrongLuongHoi"]) * 100, 2) + "</td>");
                            else
                                sb.Append("<td>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["Chuong"].ToString() + "</td><td>" + Convert.ToDateTime(r["NgayNhapChuong"]).ToString("dd/MM/yyyy") + "</td><td>" + r["PhanLoai"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["Da_TrongLuong"], 0) + "</td><td style='text-align:right;'>" + r["Da_Bung"].ToString() + "</td><td style='text-align:center;'>" + r["Da_PhanLoai"].ToString() + "</td><td>" + r["PhuongPhapMo"].ToString() + "</td>" + sTD + "<td style='text-align:left;'>" + r["TenNguoiMo"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["TrongLuongHoi"], 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["TrongLuongMocHam"], 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(0, 2) + "</td>");
                            if (dateSub == currDateSub && dateSub == currDate)
                            {
                                if (rSub["KhoiLuong"] == DBNull.Value)
                                {
                                    kl = 0;
                                }
                                else
                                {
                                    kl = (decimal)rSub["KhoiLuong"];
                                }
                                tongTLSP += kl;
                                if (currMocHam != 0)
                                {
                                    tongTLeMocHam += kl / currMocHam * 100;
                                    tongTLeTLHoi += kl / currHoi * 100;
                                    sb.Append("<td>" + rSub["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rSub["KhoiLuong"], scale) + "</td><td style='text-align:right;'>" + Config.ToXVal2(kl / currMocHam * 100, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(kl / currHoi * 100, 2) + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td>" + rSub["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rSub["KhoiLuong"], scale) + "</td><td style='text-align:right;'>" + Config.ToXVal2(0, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(0, 2) + "</td>");
                                }
                            }
                            else
                            {
                                sb.Append("<td></td><td></td><td></td><td></td>");
                                iSub--;
                            }
                            sb.Append("</tr>");
                        }
                        else
                        {
                            if (dateSub == currDateSub && dateSub == currDate)
                            {
                                //sb.Append("<tr><td></td><td>" + rSub["BienBan"].ToString() + "</td><td style='text-align:center;'>" + currDateSub.ToString("dd/MM/yyyy") + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
                                sb.Append("<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>" + tds(soluongVatTu));
                                if (rSub["KhoiLuong"] == DBNull.Value)
                                {
                                    kl = 0;
                                }
                                else
                                {
                                    kl = (decimal)rSub["KhoiLuong"];
                                }
                                tongTLSP += kl;
                                if (currMocHam != 0)
                                {
                                    tongTLeMocHam += kl / currMocHam * 100;
                                    tongTLeTLHoi += kl / currHoi * 100;
                                    sb.Append("<td>" + rSub["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rSub["KhoiLuong"], scale) + "</td><td style='text-align:right;'>" + Config.ToXVal2(kl / currMocHam * 100, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(kl / currHoi * 100, 2) + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td>" + rSub["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rSub["KhoiLuong"], scale) + "</td><td style='text-align:right;'>" + Config.ToXVal2(0, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(0, 2) + "</td>");
                                }
                                i--;
                                sb.Append("</tr>");
                            }
                            else
                            {
                                if (currHoi == 0) sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td><td style='text-align:right;'>" + tongSoLuong.ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(tongTLDa, 0) + "</td><td></td><td></td><td></td>" + tds(tongSoVatTu) + "<td></td><td style='text-align:right;'>" + Config.ToXVal2(currHoi, 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currMocHam, 1) + "</td><td style='text-align:right;'>0</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(tongTLSP, scale) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongTLeMocHam, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongTLeTLHoi, 2) + "</td>");
                                else sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td><td style='text-align:right;'>" + tongSoLuong.ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(tongTLDa, 0) + "</td><td></td><td></td><td></td>" + tds(tongSoVatTu) + "<td></td><td style='text-align:right;'>" + Config.ToXVal2(currHoi, 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currMocHam, 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currMocHam / currHoi * 100, 2) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(tongTLSP, scale) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongTLeMocHam, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongTLeTLHoi, 2) + "</td>");
                                sb.Append("</tr>");
                                totalTLDa += tongTLDa;
                                totalTLSP += tongTLSP;
                                totalHoi += currHoi;
                                totalMocHam += currMocHam;
                                if (date == DateTime.MaxValue && dateSub == DateTime.MaxValue) break;
                                tongSoLuong = 0;
                                for (q = 0; q < soluongVatTu; q++)
                                {
                                    tongSoVatTu[q] = 0;
                                }
                                tongTLSP = 0;
                                tongTLDa = 0;
                                tongTLeMocHam = 0;
                                tongTLeTLHoi = 0;
                                i--;
                                iSub--;
                                if (date != DateTime.MaxValue) currDate = date;
                                if (dateSub != DateTime.MaxValue) currDateSub = dateSub;
                                recalc = true;
                            }
                        }
                        i++;
                        iSub++;
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>Tổng cộng</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(stt, 0) + "</td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(totalTLDa, 0) + @"</td><td></td><td></td><td></td>" + tds(Sup1TongSoVatTu) + "<td></td><td style='text-align:right;'>" + Config.ToXVal2(totalHoi, scale) + @"</td><td style='text-align:right;'>" + Config.ToXVal2(totalMocHam, scale) + @"</td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(totalTLSP, scale) + @"</td><td></td><td></td></tr>");
                    Supstt += stt;
                    for (q = 0; q < soluongVatTu; q++)
                    {
                        SupTongSoVatTu[q] += Sup1TongSoVatTu[q];
                    }
                    SuptotalTLDa += totalTLDa;
                    SuptotalHoi += totalHoi;
                    SuptotalMocHam += totalMocHam;
                    SuptotalTLSP += totalTLSP;
                }
                if (ds.Tables.Count > 1)
                {
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>TỔNG CÁC LOẠI CÁ</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(Supstt, 0) + "</td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(SuptotalTLDa, 0) + @"</td><td></td><td></td><td></td>" + tds(SupTongSoVatTu) + "<td></td><td style='text-align:right;'>" + Config.ToXVal2(SuptotalHoi, scale) + @"</td><td style='text-align:right;'>" + Config.ToXVal2(SuptotalMocHam, scale) + @"</td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(SuptotalTLSP, scale) + @"</td><td></td><td></td></tr>");
                }
                sb.Append("</tbody></table><br/><br/>");
                
                // Ca loai thai
                sb.Append(CaLoaiThai(false));
                
                lt.Text = sb.ToString();
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected string CaLoaiThai(bool Excel)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string tieude = "";
                string strSQL = "QLCS_BCTK_CaLoaiThai_ChiTiet_LoaiCa";
                SqlParameter[] param = new SqlParameter[3];
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                param[2] = new SqlParameter("@StrLoaiCa", Config.GetSelectedValues_At(lstLoaiCa));
                tieude += "<b>SẢN PHẨM THU HỒI TỪ CÁ LOẠI THẢI TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataSet ds = Config.SelectSPs(strSQL, param);

                int soluongVatTu = 0;
                string CotVatTu = "";
                ArrayList arrVatTu = new ArrayList();
                foreach (ListItem item in lstVatTu.Items)
                {
                    if (item.Selected)
                    {
                        soluongVatTu++;
                        CotVatTu += "<th>" + item.Text + "</th>";
                        arrVatTu.Add(item.Value);
                    }
                }
                int soluongCot = 4 + soluongVatTu;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                if(Excel)
                    sb.Append("<table border='1'><thead>");
                else
                    sb.Append("<table id='thongke1' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                                <th rowspan=2>STT</th>
                                <th rowspan=2>Số biên bản</th>
                                <th rowspan=2>Ngày Chết</th>
                                <th rowspan=2>Tuổi cá</th>
                                <th rowspan=2>Loại cá</th>
                                <th rowspan=2>Chuồng</th>
                                <th rowspan=2>Ngày nhập chuồng</th>
                                <th rowspan=2>Phân loại</th>
                                <th colspan='" + soluongCot.ToString() + @"'>SP.DA</th>
                                <th rowspan=2>Khối lượng</th>
                                <th rowspan=2>Lý do thải loại</th>
                            </tr>
                            <tr style='font-weight:bold; vertical-align:middle;'><th>Khối lượng (kg)</th><th>Kích thước da (cm)</th><th>Loại da</th><th>Da (CB/CL/MDL)</th>" + CotVatTu + "</tr></thead><tbody>");
                string[] aLoaiCa = Config.GetSelectedTexts(lstLoaiCa).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                int Supstt = 0;
                decimal[] SupTongSoVatTu = new decimal[soluongVatTu];
                int q = 0;
                for (q = 0; q < soluongVatTu; q++)
                {
                    SupTongSoVatTu[q] = 0;
                }
                for (int u = 0; u < ds.Tables.Count; u++)
                {
                    DataTable dt = ds.Tables[u];
                    if (aLoaiCa.Length > 0)
                        sb.Append("<tr style='font-weight:bold;color:#00F;'><td style='text-align:left;'>Loại cá</td><td style='text-align:left'>" + aLoaiCa[u] + "</td>" + tds(12 + soluongVatTu) + "</tr>");
                    int tongSoLuong = 0;
                    decimal[] tongSoVatTu = new decimal[soluongVatTu];
                    for (q = 0; q < soluongVatTu; q++)
                    {
                        tongSoVatTu[q] = 0;
                    }
                    decimal kl;
                    int i = 0;
                    int iSub = 0;
                    int stt = 0;
                    decimal[] Sup1TongSoVatTu = new decimal[soluongVatTu];
                    for (q = 0; q < soluongVatTu; q++)
                    {
                        Sup1TongSoVatTu[q] = 0;
                    }
                    DateTime currDate = DateTime.MinValue;
                    if (dt.Rows.Count > 0)
                    {
                        currDate = Convert.ToDateTime(dt.Rows[0]["Ngay"]);
                    }
                    while (i <= dt.Rows.Count)
                    {
                        DataRow r = null;
                        DateTime date;
                        if (i == dt.Rows.Count)
                        {
                            date = DateTime.MaxValue;
                        }
                        else
                        {
                            r = dt.Rows[i];
                            date = Convert.ToDateTime(r["Ngay"]);
                        }
                        if (date == currDate)
                        {
                            string TuoiCa = "";
                            if (r["NgayNo"] != DBNull.Value)
                            {
                                int SoNgay = Config.Days360(Convert.ToDateTime(r["Ngay"]), Convert.ToDateTime(r["NgayNo"]));
                                double dTuoiCa = (double)SoNgay / 30;
                                TuoiCa = Config.ToXVal2(dTuoiCa, 2);
                            }
                            else
                            {
                                TuoiCa = "";
                            }

                            tongSoLuong++;
                            stt++;
                            decimal[] soVatTuTemp = null;
                            string sTD = value(r["VatTu"].ToString(), arrVatTu, out soVatTuTemp);
                            for (q = 0; q < soluongVatTu; q++)
                            {
                                tongSoVatTu[q] += soVatTuTemp[q];
                                Sup1TongSoVatTu[q] += soVatTuTemp[q];
                            }
                            sb.Append("<tr><td style='text-align:center;'>" + stt.ToString() + "</td><td>" + r["BienBan"].ToString() + @"</td><td style='text-align:center;'>" + currDate.ToString("dd/MM/yyyy") + "</td><td style='text-align:right;'>" + TuoiCa + "</td>");
                            sb.Append("<td>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["Chuong"].ToString() + "</td><td>" + Convert.ToDateTime(r["NgayNhapChuong"]).ToString("dd/MM/yyyy") + "</td><td>" + r["PhanLoai"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(0, 0) + "</td><td style='text-align:right;'>" + r["Da_Bung"].ToString() + "</td><td style='text-align:center;'>" + r["Da_PhanLoai"].ToString() + "</td><td>" + r["PhuongPhapMo"].ToString() + "</td>" + sTD + "<td style='text-align:right;'>" + Config.ToXVal2(r["KhoiLuong"], 2) + "</td><td style='text-align:left;'>" + r["LyDoChet"].ToString() + "</td>");
                            sb.Append("</tr>");
                        }
                        else
                        {
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td><td style='text-align:right;'>" + tongSoLuong.ToString() + "</td>" + tds(9) + tds(tongSoVatTu) + "<td></td><td></td></tr>");
                            if (date == DateTime.MaxValue) break;
                            tongSoLuong = 0;
                            for (q = 0; q < soluongVatTu; q++)
                            {
                                tongSoVatTu[q] = 0;
                            }
                            i--;
                            if (date != DateTime.MaxValue) currDate = date;
                        }
                        i++;
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>Tổng cộng</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(stt, 0) + "</td>" + tds(9) + tds(Sup1TongSoVatTu) + "<td></td><td></td></tr>");
                    Supstt += stt;
                    for (q = 0; q < soluongVatTu; q++)
                    {
                        SupTongSoVatTu[q] += Sup1TongSoVatTu[q];
                    }
                }
                if (ds.Tables.Count > 1)
                {
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>TỔNG CÁC LOẠI CÁ</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(Supstt, 0) + "</td>" + tds(9) + tds(SupTongSoVatTu) + "<td></td><td></td></tr>");
                }
                sb.Append("</tbody></table><br/><br/>");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}