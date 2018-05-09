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
    public partial class r_gietmocatheovicat : DotNetNuke.Entities.Modules.PortalModuleBase
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
            for (int i = DateTime.Now.Year - 2 ; i > 2009; i--)
            {
                ddlNamNo.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ddlNamNo.Items.Insert(0, new ListItem("","0"));

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
            for (int l = 0; l < arrVatTu.Count; l++)
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
                System.Globalization.CultureInfo cic = System.Globalization.CultureInfo.CreateSpecificCulture(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                string filename = "GietMoCaTheoViCat";
                string tieude = "";
                string strSQL = "QLCS_BCTK_GietMo_ChiTietCa_new";
                if (ddlChuan.SelectedValue == "1")
                {
                    strSQL = "QLCS_BCTK_GietMo_ChiTietCa_new_ChuanCu";
                }
                string strSQLSub = "QLCS_BCTK_GietMo_ChiTietSanPham_new";
                string strSQLSPGM = "QLCS_VatTu_GetDanhMuc_OrderByMoTa";
                SqlParameter[] param = new SqlParameter[3];
                SqlParameter p = new SqlParameter("@LoaiVatTu", "SPGM");
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
                param[2] = new SqlParameter("@NamNo", int.Parse(ddlNamNo.SelectedValue));
                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                filename = filename.Replace("/", "_");
                tieude += "<b>SẢN PHẨM THU HỒI TỪ GIẾT MỔ CÁ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                SqlParameter[] paramSub = new SqlParameter[3];
                paramSub[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                paramSub[1] = new SqlParameter("@dTo", txtToDate.Text);
                paramSub[2] = new SqlParameter("@ZeroCol", "");
                paramSub[2].Direction = ParameterDirection.Output;
                paramSub[2].Size = 4000;
                DataTable dtSub = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLSub, paramSub);
                string ZeroCol = paramSub[2].Value.ToString();
                DataTable dtSPGM = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLSPGM, p);
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                string strSPGM = "";
                int idx = 14;
                foreach (DataRow spgm in dtSPGM.Rows)
                {
                    if (!ZeroCol.Contains("@" + spgm["IDVatTu"].ToString() + "@"))
                    {
                        strSPGM += "<td rowspan=2>" + spgm["TenVatTu"].ToString() + "</td>";
                        idx++;
                    }
                }

                int soluongVatTu = 0;
                string CotVatTu = "";
                ArrayList arrVatTu = new ArrayList();
                foreach (ListItem item in lstVatTu.Items)
                {
                    if (item.Selected)
                    {
                        soluongVatTu++;
                        CotVatTu += "<td rowspan=2>" + item.Text + "</td>";
                        arrVatTu.Add(item.Value);
                    }
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table border='1'>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <td rowspan=2>STT</td><td rowspan=2>Vi cắt</td><td rowspan=2>Mã</td><td rowspan=2>Chuồng</td><td rowspan=2>Ngày nhập chuồng</td><td rowspan=2>Số biên bản</td><td rowspan=2>Ngày xuống chuồng</td><td rowspan=2>Ngày Mổ</td><td rowspan=2>Tuổi cá</td><td rowspan=2>Loại cá</td><td rowspan=2>Phân loại</td><td rowspan=2>Người mổ</td><td rowspan=2>Khối lượng hơi (kg)</td><td rowspan=2>Khối lượng móc hàm (kg)</td><td colspan=2>SP.DA</td><td rowspan=2>Loại da</td><td rowspan=2>Phương pháp mổ</td>" + CotVatTu + strSPGM + @"<td rowspan=2>Tỷ lệ móc hàm (%)</td><td rowspan=2>Ghi chú</td></tr>
                        <tr style='font-weight:bold; vertical-align:middle;'><td>Khối lượng (kg)</td><td>Kích thước da (cm)</td></tr>");
                bool recalc = true;
                decimal tongTLDa = 0;
                decimal totalTLDa = 0;
                decimal tongTLeMocHam = 0;
                int tongSoLuong = 0;
                decimal[] tongSoVatTu = new decimal[soluongVatTu];
                int q = 0;
                for (q = 0; q < soluongVatTu; q++)
                {
                    tongSoVatTu[q] = 0;
                }
                int i = 0;
                int iSub = 0;
                int stt = 0;
                decimal[] Sup1TongSoVatTu = new decimal[soluongVatTu];
                for (q = 0; q < soluongVatTu; q++)
                {
                    Sup1TongSoVatTu[q] = 0;
                }
                DateTime currDate = DateTime.MinValue;
                decimal currHoi = 0; decimal currMocHam = 0;
                decimal totalHoi = 0; decimal totalMocHam = 0;
                decimal[] arrCurrKhoiLuongSanPham = new decimal[idx - 14];
                decimal[] arrTotalKhoiLuongSanPham = new decimal[idx - 14];
                decimal[] arrTCKhoiLuongSanPham = new decimal[idx - 14];
                for (int n = 0; n < idx - 14; n++)
                {
                    arrTotalKhoiLuongSanPham[n] = 0;
                    arrTCKhoiLuongSanPham[n] = 0;
                }
                if (dt.Rows.Count > 0)
                {
                    currDate = Convert.ToDateTime(dt.Rows[0]["NgayMo"]);
                }
                while (i <= dt.Rows.Count)
                {
                    DataRow r = null;
                    DataRow rSub = null;
                    DateTime date;
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
                            tongTLeMocHam = currMocHam / currHoi * 100;
                            int index = 0;
                            while (iSub < dtSub.Rows.Count)
                            {
                                rSub = dtSub.Rows[iSub];
                                if (Convert.ToDateTime(rSub["NgayMo"]) == currDate)
                                {
                                    arrCurrKhoiLuongSanPham[index] = Convert.ToDecimal(rSub["KhoiLuong"]);
                                    arrTotalKhoiLuongSanPham[index] += Convert.ToDecimal(rSub["KhoiLuong"]);
                                    index++;
                                    iSub++;
                                }
                                else break;
                            }

                            recalc = false;
                        }
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
                        sb.Append("<tr><td>" + stt.ToString() + "</td><td style='text-align:left;'>" + r["MaSoGoc"].ToString() + "</td><td style='text-align:left;'>" + r["MaSo"].ToString() + "</td><td style='text-align:left;'>" + r["Chuong"].ToString() + @"</td><td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayNhapChuong"]).ToString("dd/MM/yyyy") + "</td><td style='text-align:left;'>" + r["BienBan"].ToString() + @"</td><td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayXuongChuong"]).ToString("dd/MM/yyyy") + @"</td><td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + currDate.ToString("dd/MM/yyyy") + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + TuoiCa + "</td>");
                        sb.Append("<td style='text-align:left;'>" + r["TenLoaiCa"].ToString() + @"</td><td style='text-align:left;'>" + r["PhanLoai"].ToString() + @"</td><td align='left'>" + r["TenNguoiMo"].ToString() + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(r["TrongLuongHoi"], 1) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(r["TrongLuongMocHam"], 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["Da_TrongLuong"], 0) + "</td><td style='text-align:right;'>" + r["Da_Bung"].ToString() + "</td><td>" + r["Da_PhanLoai"].ToString() + "</td><td style='text-align:left;'>" + r["PhuongPhapMo"].ToString() + "</td>" + sTD);
                        if (currHoi == 0)
                        {
                            foreach (decimal d in arrCurrKhoiLuongSanPham)
                            {
                                sb.Append("<td></td>");
                            }
                        }
                        else
                        {
                            if (i + 1 < dt.Rows.Count && Convert.ToDateTime(dt.Rows[i + 1]["NgayMo"]) == date)
                            {
                                int l = 0;
                                foreach (decimal d in arrCurrKhoiLuongSanPham)
                                {
                                    sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(d * Convert.ToDecimal(r["TrongLuongHoi"]) / currHoi, scale + 1) + "</td>");
                                    arrTCKhoiLuongSanPham[l] += Decimal.Parse(Config.ToXVal2(d * Convert.ToDecimal(r["TrongLuongHoi"]) / currHoi, scale + 1), cic);
                                    l++;
                                }
                            }
                            else
                            {
                                int l = 0;
                                foreach (decimal d in arrCurrKhoiLuongSanPham)
                                {
                                    decimal last = d - arrTCKhoiLuongSanPham[l];
                                    sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(last, scale + 1) + "</td>");
                                    arrTCKhoiLuongSanPham[l] = 0;
                                    l++;
                                }
                            }
                        }
                        sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(((decimal)r["TrongLuongMocHam"]) / ((decimal)r["TrongLuongHoi"]) * 100, 2) + "</td><td></td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + tongSoLuong.ToString() + @"</td><td></td><td></td><td></td><td></td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(currHoi, 1) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(currMocHam, 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongTLDa, 0) + "</td><td></td><td></td><td></td>" + tds(tongSoVatTu));
                        foreach (decimal d in arrCurrKhoiLuongSanPham)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(d, scale + 1) + "</td>");
                        }
                        sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tongTLeMocHam, 2) + "</td><td></td></tr>");
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>SPGM/<br/>KLHơi (%)</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>" + tds(soluongVatTu));
                        if (currHoi != 0)
                        {
                            foreach (decimal d in arrCurrKhoiLuongSanPham)
                            {
                                sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(d / currHoi * 100, 2) + "</td>");
                            }
                        }
                        else
                        {
                            foreach (decimal d in arrCurrKhoiLuongSanPham)
                            {
                                sb.Append("<td style='text-align:right;'>0</td>");
                            }
                        }
                        sb.Append("<td></td><td></td></tr>");
                        totalHoi += currHoi;
                        totalMocHam += currMocHam;
                        totalTLDa += tongTLDa;
                        if (date == DateTime.MaxValue) break;
                        tongSoLuong = 0;
                        for (q = 0; q < soluongVatTu; q++)
                        {
                            tongSoVatTu[q] = 0;
                        }
                        tongTLDa = 0;
                        tongTLeMocHam = 0;
                        i--;
                        currDate = date;
                        recalc = true;
                    }
                    i++;
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>Tổng cộng</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(stt, 0) + "</td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(totalHoi, 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(totalMocHam, 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(totalTLDa, 0) + "</td><td></td><td></td><td></td>" + tds(Sup1TongSoVatTu));
                foreach (decimal d in arrTotalKhoiLuongSanPham)
                {
                    sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(d, scale) + "</td>");
                }
                sb.Append("<td></td><td></td></tr>");
                sb.Append("</table><br/><br/>");
                //Ca loai thai
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
                System.Globalization.CultureInfo cic = System.Globalization.CultureInfo.CreateSpecificCulture(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                string tieude = "";
                string strSQL = "QLCS_BCTK_GietMo_ChiTietCa_new";
                if (ddlChuan.SelectedValue == "1")
                {
                    strSQL = "QLCS_BCTK_GietMo_ChiTietCa_new_ChuanCu";
                }
                string strSQLSub = "QLCS_BCTK_GietMo_ChiTietSanPham_new";
                string strSQLSPGM = "QLCS_VatTu_GetDanhMuc_OrderByMoTa";
                SqlParameter[] param = new SqlParameter[3];
                SqlParameter p = new SqlParameter("@LoaiVatTu", "SPGM");
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
                param[2] = new SqlParameter("@NamNo", int.Parse(ddlNamNo.SelectedValue));
                tieude += "<b>SẢN PHẨM THU HỒI TỪ GIẾT MỔ CÁ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                SqlParameter[] paramSub = new SqlParameter[3];
                paramSub[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                paramSub[1] = new SqlParameter("@dTo", txtToDate.Text);
                paramSub[2] = new SqlParameter("@ZeroCol", "");
                paramSub[2].Direction = ParameterDirection.Output;
                paramSub[2].Size = 4000;
                DataTable dtSub = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLSub, paramSub);
                string ZeroCol = paramSub[2].Value.ToString();
                DataTable dtSPGM = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLSPGM, p);
                string strSPGM = "";
                int idx = 14;
                foreach (DataRow spgm in dtSPGM.Rows)
                {
                    if (!ZeroCol.Contains("@" + spgm["IDVatTu"].ToString() + "@"))
                    {
                        strSPGM += "<td rowspan=2 align='center'>" + spgm["TenVatTu"].ToString() + "</td>";
                        idx++;
                    }
                }

                int soluongVatTu = 0;
                string CotVatTu = "";
                ArrayList arrVatTu = new ArrayList();
                foreach (ListItem item in lstVatTu.Items)
                {
                    if (item.Selected)
                    {
                        soluongVatTu++;
                        CotVatTu += "<th rowspan=2>" + item.Text + "</th>";
                        arrVatTu.Add(item.Value);
                    }
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <th rowspan=2>STT</th><th rowspan=2>Vi cắt</th><th rowspan=2>Mã</th><th rowspan=2>Chuồng</th><th rowspan=2>Ngày nhập chuồng</th><th rowspan=2>Số biên bản</th><th rowspan=2>Ngày xuống chuồng</th><th rowspan=2>Ngày Mổ</th><th rowspan=2>Tuổi cá</th><th rowspan=2>Loại cá</th><th rowspan=2>Phân loại</th><th rowspan=2>Người mổ</th><th rowspan=2>Khối lượng hơi (kg)</th><th rowspan=2>Khối lượng móc hàm (kg)</th><th colspan='2'>SP.DA</th><th rowspan=2>Loại da</th><th rowspan=2>Phương pháp mổ</th>" + CotVatTu + strSPGM + @"<th rowspan=2>Tỷ lệ móc hàm (%)</th><th rowspan=2>Ghi chú</th></tr>
                        <tr style='font-weight:bold; vertical-align:middle;'><th>Khối lượng (kg)</th><th>Kích thước da (cm)</th></tr></thead><tbody>");
                bool recalc = true;
                decimal tongTLDa = 0;
                decimal totalTLDa = 0;
                decimal tongTLeMocHam = 0;
                int tongSoLuong = 0;
                decimal[] tongSoVatTu = new decimal[soluongVatTu];
                int q = 0;
                for (q = 0; q < soluongVatTu; q++)
                {
                    tongSoVatTu[q] = 0;
                }
                int i = 0;
                int iSub = 0;
                int stt = 0;
                decimal[] Sup1TongSoVatTu = new decimal[soluongVatTu];
                for (q = 0; q < soluongVatTu; q++)
                {
                    Sup1TongSoVatTu[q] = 0;
                }
                DateTime currDate = DateTime.MinValue;
                decimal currHoi = 0; decimal currMocHam = 0;
                decimal totalHoi = 0; decimal totalMocHam = 0;
                decimal[] arrCurrKhoiLuongSanPham = new decimal[idx - 14];
                decimal[] arrTotalKhoiLuongSanPham = new decimal[idx - 14];
                decimal[] arrTCKhoiLuongSanPham = new decimal[idx - 14];
                for (int n = 0; n < idx - 14; n++)
                {
                    arrTotalKhoiLuongSanPham[n] = 0;
                    arrTCKhoiLuongSanPham[n] = 0;
                }
                if (dt.Rows.Count > 0)
                {
                    currDate = Convert.ToDateTime(dt.Rows[0]["NgayMo"]);
                }
                while (i <= dt.Rows.Count)
                {
                    DataRow r = null;
                    DataRow rSub = null;
                    DateTime date;
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
                            tongTLeMocHam = currMocHam / currHoi * 100;
                            int index = 0;
                            while (iSub < dtSub.Rows.Count)
                            {
                                rSub = dtSub.Rows[iSub];
                                if (Convert.ToDateTime(rSub["NgayMo"]) == currDate)
                                {
                                    arrCurrKhoiLuongSanPham[index] = Convert.ToDecimal(rSub["KhoiLuong"]);
                                    arrTotalKhoiLuongSanPham[index] += Convert.ToDecimal(rSub["KhoiLuong"]);
                                    index++;
                                    iSub++;
                                }
                                else break;
                            }

                            recalc = false;
                        }
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

                        sb.Append("<tr><td style='text-align:center;'>" + stt.ToString() + "</td><td>" + r["MaSoGoc"].ToString() + "</td><td>" + r["MaSo"].ToString() + "</td><td>" + r["Chuong"].ToString() + @"</td><td style='text-align:center;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayNhapChuong"]).ToString("dd/MM/yyyy") + "</td><td>" + r["BienBan"].ToString() + @"</td><td style='text-align:center;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayXuongChuong"]).ToString("dd/MM/yyyy") + @"</td><td style='text-align:center;mso-number-format:""dd\\/mm\\/yyyy"";'>" + currDate.ToString("dd/MM/yyyy") + "</td><td style='text-align:right;'>" + TuoiCa + "</td>");
                        sb.Append("<td>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["PhanLoai"].ToString() + "</td><td align='left'>" + r["TenNguoiMo"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["TrongLuongHoi"], 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["TrongLuongMocHam"], 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["Da_TrongLuong"], 0) + "</td><td style='text-align:right;'>" + r["Da_Bung"].ToString() + "</td><td style='text-align:center;'>" + r["Da_PhanLoai"].ToString() + "</td><td>" + r["PhuongPhapMo"].ToString() + "</td>" + sTD);
                        if (currHoi == 0)
                        {
                            foreach (decimal d in arrCurrKhoiLuongSanPham)
                            {
                                sb.Append("<td></td>");
                            }
                        }
                        else
                        {
                            if (i + 1 < dt.Rows.Count && Convert.ToDateTime(dt.Rows[i + 1]["NgayMo"]) == date)
                            {
                                int l = 0;
                                foreach (decimal d in arrCurrKhoiLuongSanPham)
                                {
                                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(d * Convert.ToDecimal(r["TrongLuongHoi"]) / currHoi, scale + 1) + "</td>");
                                    arrTCKhoiLuongSanPham[l] += Decimal.Parse(Config.ToXVal2(d * Convert.ToDecimal(r["TrongLuongHoi"]) / currHoi, scale + 1), cic);
                                    l++;
                                }
                            }
                            else
                            {
                                int l = 0;
                                foreach (decimal d in arrCurrKhoiLuongSanPham)
                                {
                                    decimal last = d - arrTCKhoiLuongSanPham[l];
                                    sb.Append(@"<td style='text-align:right'>" + Config.ToXVal2(last, scale + 1) + "</td>");
                                    arrTCKhoiLuongSanPham[l] = 0;
                                    l++;
                                }
                            }
                        }
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(((decimal)r["TrongLuongMocHam"]) / ((decimal)r["TrongLuongHoi"]) * 100, 2) + "</td><td></td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + tongSoLuong.ToString() + "</td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(currHoi, 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currMocHam, 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongTLDa, 0) + "</td><td></td><td></td><td></td>" + tds(tongSoVatTu));
                        foreach (decimal d in arrCurrKhoiLuongSanPham)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(d, scale + 1) + "</td>");
                        }
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongTLeMocHam, 2) + "</td><td></td></tr>");
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>SPGM/<br/>KLHơi (%)</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>" + tds(soluongVatTu));
                        if (currHoi != 0)
                        {
                            foreach (decimal d in arrCurrKhoiLuongSanPham)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(d / currHoi * 100, 2) + "</td>");
                            }
                        }
                        else
                        {
                            foreach (decimal d in arrCurrKhoiLuongSanPham)
                            {
                                sb.Append("<td style='text-align:right;'>0</td>");
                            }
                        }
                        sb.Append("<td></td><td></td></tr>");
                        totalHoi += currHoi;
                        totalMocHam += currMocHam;
                        totalTLDa += tongTLDa;
                        if (date == DateTime.MaxValue) break;
                        tongSoLuong = 0;
                        for (q = 0; q < soluongVatTu; q++)
                        {
                            tongSoVatTu[q] = 0;
                        }
                        tongTLDa = 0;
                        tongTLeMocHam = 0;
                        i--;
                        currDate = date;
                        recalc = true;
                    }
                    i++;
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>Tổng cộng</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(stt, 0) + "</td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(totalHoi, 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(totalMocHam, 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(totalTLDa, 0) + "</td><td></td><td></td><td></td>" + tds(Sup1TongSoVatTu));
                foreach (decimal d in arrTotalKhoiLuongSanPham)
                {
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(d, scale) + "</td>");
                }
                sb.Append("<td></td><td></td></tr>");
                sb.Append("</tbody></table><br/><br/>");

                //Ca loai thai
                sb.Append(CaLoaiThai(false));
                lt.Text = sb.ToString();
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        //Ca loai thai
        protected string CaLoaiThai(bool Excel)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                System.Globalization.CultureInfo cic = System.Globalization.CultureInfo.CreateSpecificCulture(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                string tieude = "";
                string strSQL = "QLCS_BCTK_CaLoaiThai_ChiTiet";
                SqlParameter[] param = new SqlParameter[3];
                SqlParameter p = new SqlParameter("@LoaiVatTu", "SPGM");
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
                param[2] = new SqlParameter("@NamNo", int.Parse(ddlNamNo.SelectedValue));
                tieude += "<b>SẢN PHẨM THU HỒI TỪ CÁ LOẠI THẢI TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                int idx = 14;
                int soluongVatTu = 0;
                string CotVatTu = "";
                ArrayList arrVatTu = new ArrayList();
                foreach (ListItem item in lstVatTu.Items)
                {
                    if (item.Selected)
                    {
                        soluongVatTu++;
                        CotVatTu += "<th rowspan=2>" + item.Text + "</th>";
                        arrVatTu.Add(item.Value);
                    }
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                if(Excel)
                    sb.Append("<table border='1'><thead>");
                else
                    sb.Append("<table id='thongke1' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <th rowspan=2>STT</th><th rowspan=2>Vi cắt</th><th rowspan=2>Mã</th><th rowspan=2>Chuồng</th><th rowspan=2>Ngày nhập chuồng</th><th rowspan=2>Số biên bản</th><th rowspan=2>Ngày xuống chuồng</th><th rowspan=2>Ngày chết</th><th rowspan=2>Tuổi cá</th><th rowspan=2>Loại cá</th><th rowspan=2>Phân loại</th><th rowspan=2>Khối lượng khi chết</th><th rowspan=2>Lý do chết</th><th rowspan=2>Ngày nở</th><th colspan='2'>SP.DA</th><th rowspan=2>Loại da</th><th rowspan=2>Phương pháp mổ</th>" + CotVatTu + @"</tr>
                        <tr style='font-weight:bold; vertical-align:middle;'><th>Khối lượng (kg)</th><th>Kích thước da (cm)</th></tr></thead><tbody>");
                int tongSoLuong = 0;
                decimal[] tongSoVatTu = new decimal[soluongVatTu];
                int q = 0;
                for (q = 0; q < soluongVatTu; q++)
                {
                    tongSoVatTu[q] = 0;
                }
                int i = 0;
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

                        sb.Append("<tr><td style='text-align:center;'>" + stt.ToString() + "</td><td>" + r["MaSoGoc"].ToString() + "</td><td>" + r["MaSo"].ToString() + "</td><td>" + r["Chuong"].ToString() + @"</td><td style='text-align:center;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayNhapChuong"]).ToString("dd/MM/yyyy") + "</td><td>" + r["BienBan"].ToString() + @"</td><td style='text-align:center;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayXuongChuong"]).ToString("dd/MM/yyyy") + @"</td><td style='text-align:center;mso-number-format:""dd\\/mm\\/yyyy"";'>" + currDate.ToString("dd/MM/yyyy") + "</td><td style='text-align:right;'>" + TuoiCa + "</td>");
                        sb.Append("<td>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["PhanLoai"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["KhoiLuong"], 2) + "</td><td align='left'>" + r["LyDoChet"].ToString() + @"</td><td style='text-align:center;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayNo"]).ToString("dd/MM/yyyy") + "</td><td></td><td style='text-align:right;'>" + r["Da_Bung"].ToString() + "</td><td style='text-align:center;'>" + r["Da_PhanLoai"].ToString() + "</td><td>" + r["PhuongPhapMo"].ToString() + "</td>" + sTD + "</tr>");
                    }
                    else
                    {
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + tongSoLuong.ToString() + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>" + tds(tongSoVatTu) + "</tr>");
                        if (date == DateTime.MaxValue) break;
                        tongSoLuong = 0;
                        for (q = 0; q < soluongVatTu; q++)
                        {
                            tongSoVatTu[q] = 0;
                        }
                        i--;
                        currDate = date;
                    }
                    i++;
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>Tổng cộng</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(stt, 0) + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>" + tds(Sup1TongSoVatTu) + "</tr>");
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