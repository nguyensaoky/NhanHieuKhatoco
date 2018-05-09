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
    public partial class r_tonghopcadetheoloai : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            txtYear.Text = DateTime.Now.Year.ToString();
            txtDate.Text = "30/9";
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
                
                string filename = "TongHopCaDeTheoLoai";
                string tieude = "";
                string strSQL = "QLCS_BCTK_CaDe_TongHopTheoLoai_Year";
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@d", txtDate.Text + "/" + txtYear.Text);
                param[1] = new SqlParameter("@year", int.Parse(txtYear.Text));
                filename += txtYear.Text + ".xls";
                tieude += "<b>BẢNG TỔNG HỢP CÁ ĐẺ NĂM " + txtYear.Text + "</b>";
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body style='text-align:center;font-family:Times New Roman;'><br/>
                    <div style='text-align:center;font-weight:bold;font-size:14pt;'>" + tieude + "</div><br/>");
                sb.Append("<table border='1'>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><td rowspan='2'>STT</td><td rowspan='2'>Loại cá đẻ</td><td rowspan='2'>Số lượng cá mẹ</td><td rowspan='2'>Số cá mẹ đẻ</td><td rowspan='2'>Tỷ lệ cá đẻ/toàn đàn (%)</td><td rowspan='2'>Tổng số trứng đẻ</td><td rowspan='2'>Số trứng đẻ bình quân/con</td><td rowspan='2'>Số lượng trứng hủy</td><td rowspan='2'>Số lượng trứng ấp</td><td rowspan='2'>Tỷ lệ ấp/trứng đẻ (%)</td><td colspan='6'>Theo dõi ấp</td><td rowspan='2'>Số lượng nở</td><td rowspan='2'>Tỷ lệ nở/phôi (%)</td><td rowspan='2'>Chết 1/30 ngày</td><td rowspan='2'>Tỷ lệ chết/số lượng nở (%)</td></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><td>Không Phôi</td><td>Tỷ lệ Không Phôi/Trứng ấp (%)</td><td>Có Phôi</td><td>Tỷ lệ Có Phôi/Trứng ấp (%)</td><td>Chết Phôi 1,2</td><td>Tỷ lệ Chết Phôi/Trứng ấp (%)</td></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><td>1</td><td>2</td><td>5</td><td>6</td><td>7=6/5</td><td>8</td><td>9=8/6</td><td>10</td><td>11=8-10</td><td>12=11/8</td><td>13</td><td>14=13/11</td><td>15</td><td>16=15/11</td><td>17</td><td>18=17/11</td><td>19</td><td>20=19/15</td><td>21</td><td>22=21/19</td></tr>");
                int TongSoCaMe = 0;
                int SoCaMeDe = 0;
                int TrungDe = 0;
                int TrungHuy = 0;
                int TrungAp = 0;
                int TrungKhongPhoi = 0;
                int TrungCoPhoi = 0;
                int TrungChetPhoi = 0;
                int SoLuongNo = 0;
                int Chet1_30 = 0;
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i - 1];
                    int tongsocame = Convert.ToInt32(r["TongSoCaMe"]);
                    int socamede = Convert.ToInt32(r["SoCaMeDe"]);
                    decimal tylecade = 0;
                    if (tongsocame != 0) tylecade = (decimal)socamede / (decimal)tongsocame * 100;
                    int trunghuy = Convert.ToInt32(r["TrungVo"]) + Convert.ToInt32(r["TrungThaiLoai"]);
                    int trungde = Convert.ToInt32(r["TrungDe"]);
                    decimal trungdebinhquan = 0;
                    if(socamede != 0) trungdebinhquan = (decimal)trungde / (decimal)socamede;
                    int trungap = trungde - trunghuy;
                    decimal tyleap = 0;
                    if(trungde != 0) tyleap = (decimal)trungap / (decimal)trungde * 100;
                    int trungkhongphoi = Convert.ToInt32(r["TrungKhongPhoi"]);
                    int trungchetphoi = Convert.ToInt32(r["TrungChetPhoi1"]) + Convert.ToInt32(r["TrungChetPhoi2"]);
                    int chet1_30 = r["Chet1_30"] != DBNull.Value ? Convert.ToInt32(r["Chet1_30"]) : 0;
                    decimal tylekhongphoi = 0;
                    if(trungap != 0) tylekhongphoi = (decimal)trungkhongphoi / (decimal)trungap * 100;
                    int trungcophoi = trungap - trungkhongphoi;
                    decimal tylecophoi = 0;
                    if (trungap != 0) tylecophoi = (decimal)trungcophoi / (decimal)trungap * 100;
                    decimal tylechetphoi = 0;
                    if (trungap != 0) tylechetphoi = (decimal)trungchetphoi / (decimal)trungap * 100;
                    int soluongno = Convert.ToInt32(r["TrungNo"]);
                    decimal tyleno = 0;
                    if (trungcophoi != 0) tyleno = (decimal)soluongno / (decimal)trungcophoi * 100;
                    decimal tylechet = 0;
                    if (soluongno != 0) tylechet = (decimal)chet1_30 / (decimal)soluongno * 100;
                    sb.Append("<tr style='vertical-align:middle;'>");
                    sb.Append("<td>" + i.ToString() + "</td>");
                    sb.Append("<td style='text-align:left;'>" + r["TenLoaiCa"].ToString() + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["TongSoCaMe"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["SoCaMeDe"], 0) + "</td>");
                    sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tylecade, 2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["TrungDe"], 0) + "</td>");
                    sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(trungdebinhquan, 2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(trunghuy, 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(trungap, 0) + "</td>");
                    sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tyleap, 2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(trungkhongphoi, 0) + "</td>");
                    sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tylekhongphoi, 2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(trungcophoi, 0) + "</td>");
                    sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tylecophoi, 2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(trungchetphoi, 0) + "</td>");
                    sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tylechetphoi, 2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(soluongno, 0) + "</td>");
                    sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tyleno, 2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(chet1_30, 0) + "</td>");
                    sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tylechet, 2) + "</td>");

                    TongSoCaMe += tongsocame;
                    SoCaMeDe += socamede;
                    TrungDe += trungde;
                    TrungHuy += trunghuy;
                    TrungAp += trungap;
                    TrungKhongPhoi += trungkhongphoi;
                    TrungCoPhoi += trungcophoi;
                    TrungChetPhoi += trungchetphoi;
                    SoLuongNo += soluongno;
                    Chet1_30 += chet1_30;
                    
                    sb.Append("</tr>");
                }
                sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td colspan='2'>Tổng cộng</td><td style='text-align:right;'>" + Config.ToXVal2(TongSoCaMe, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(SoCaMeDe, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TrungDe, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TrungHuy, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TrungAp, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TrungKhongPhoi, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TrungCoPhoi, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TrungChetPhoi, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(SoLuongNo, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(Chet1_30, 0) + "</td><td></td></tr>");
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
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string tieude = "";
                string strSQL = "QLCS_BCTK_CaDe_TongHopTheoLoai_Year";
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@d", txtDate.Text + "/" + txtYear.Text);
                param[1] = new SqlParameter("@year", int.Parse(txtYear.Text));
                tieude += "<b>BẢNG TỔNG HỢP CÁ ĐẺ NĂM " + txtYear.Text + "</b>";
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<div style='text-align:center;font-weight:bold;font-size:14pt;'>" + tieude + "</div><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><th rowspan='2'>STT</th><th rowspan='2'>Loại cá đẻ</th><th rowspan='2'>Số lượng cá mẹ</th><th rowspan='2'>Số cá mẹ đẻ</th><th rowspan='2'>Tỷ lệ cá đẻ/toàn đàn (%)</th><th rowspan='2'>Tổng số trứng đẻ</th><th rowspan='2'>Số trứng đẻ bình quân/con</th><th rowspan='2'>Số lượng trứng hủy</th><th rowspan='2'>Số lượng trứng ấp</th><th rowspan='2'>Tỷ lệ ấp/trứng đẻ (%)</th><th colspan='6'>Theo dõi ấp</th><th rowspan='2'>Số lượng nở</th><th rowspan='2'>Tỷ lệ nở/phôi (%)</th><th rowspan='2'>Chết 1/30 ngày</th><th rowspan='2'>Tỷ lệ chết/số lượng nở (%)</th></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><th>Không Phôi</th><th>Tỷ lệ Không Phôi/Trứng ấp (%)</th><th>Có Phôi</th><th>Tỷ lệ Có Phôi/Trứng ấp (%)</th><th>Chết Phôi 1,2</th><th>Tỷ lệ Chết Phôi/Trứng ấp (%)</th></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><th>1</th><th>2</th><th>5</th><th>6</th><th>7=6/5</th><th>8</th><th>9=8/6</th><th>10</th><th>11=8-10</th><th>12=11/8</th><th>13</th><th>14=13/11</th><th>15</th><th>16=15/11</th><th>17</th><th>18=17/11</th><th>19</th><th>20=19/15</th><th>21</th><th>22=21/19</th></tr></thead><tbody>");
                int TongSoCaMe = 0;
                int SoCaMeDe = 0;
                int TrungDe = 0;
                int TrungHuy = 0;
                int TrungAp = 0;
                int TrungKhongPhoi = 0;
                int TrungCoPhoi = 0;
                int TrungChetPhoi = 0;
                int SoLuongNo = 0;
                int Chet1_30 = 0;
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i - 1];
                    int tongsocame = Convert.ToInt32(r["TongSoCaMe"]);
                    int socamede = Convert.ToInt32(r["SoCaMeDe"]);
                    decimal tylecade = 0;
                    if (tongsocame != 0) tylecade = (decimal)socamede / (decimal)tongsocame * 100;
                    int trunghuy = Convert.ToInt32(r["TrungVo"]) + Convert.ToInt32(r["TrungThaiLoai"]);
                    int trungde = Convert.ToInt32(r["TrungDe"]);
                    decimal trungdebinhquan = 0;
                    if (socamede != 0) trungdebinhquan = (decimal)trungde / (decimal)socamede;
                    int trungap = trungde - trunghuy;
                    decimal tyleap = 0;
                    if (trungde != 0) tyleap = (decimal)trungap / (decimal)trungde * 100;
                    int trungkhongphoi = Convert.ToInt32(r["TrungKhongPhoi"]);
                    int trungchetphoi = Convert.ToInt32(r["TrungChetPhoi1"]) + Convert.ToInt32(r["TrungChetPhoi2"]);
                    int chet1_30 = r["Chet1_30"] != DBNull.Value ? Convert.ToInt32(r["Chet1_30"]) : 0;
                    decimal tylekhongphoi = 0;
                    if (trungap != 0) tylekhongphoi = (decimal)trungkhongphoi / (decimal)trungap * 100;
                    int trungcophoi = trungap - trungkhongphoi;
                    decimal tylecophoi = 0;
                    if (trungap != 0) tylecophoi = (decimal)trungcophoi / (decimal)trungap * 100;
                    decimal tylechetphoi = 0;
                    if (trungap != 0) tylechetphoi = (decimal)trungchetphoi / (decimal)trungap * 100;
                    int soluongno = Convert.ToInt32(r["TrungNo"]);
                    decimal tyleno = 0;
                    if (trungcophoi != 0) tyleno = (decimal)soluongno / (decimal)trungcophoi * 100;
                    decimal tylechet = 0;
                    if (soluongno != 0) tylechet = (decimal)chet1_30 / (decimal)soluongno * 100;
                    sb.Append("<tr style='vertical-align:middle;'>");
                    sb.Append("<td style='text-align:center;'>" + i.ToString() + "</td>");
                    sb.Append("<td>" + r["TenLoaiCa"].ToString() + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["TongSoCaMe"],0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["SoCaMeDe"],0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tylecade,2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["TrungDe"],0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(trungdebinhquan,2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(trunghuy,0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(trungap,0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tyleap,2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(trungkhongphoi,0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tylekhongphoi,2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(trungcophoi,0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tylecophoi,2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(trungchetphoi,0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tylechetphoi,2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(soluongno,0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tyleno,2) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(chet1_30,0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tylechet,2) + "</td>");

                    TongSoCaMe += tongsocame;
                    SoCaMeDe += socamede;
                    TrungDe += trungde;
                    TrungHuy += trunghuy;
                    TrungAp += trungap;
                    TrungKhongPhoi += trungkhongphoi;
                    TrungCoPhoi += trungcophoi;
                    TrungChetPhoi += trungchetphoi;
                    SoLuongNo += soluongno;
                    Chet1_30 += chet1_30;

                    sb.Append("</tr>");
                }
                sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td>Tổng cộng</td><td style='text-align:right;'>" + Config.ToXVal2(TongSoCaMe,0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(SoCaMeDe,0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TrungDe,0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TrungHuy,0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TrungAp,0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TrungKhongPhoi,0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TrungCoPhoi,0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TrungChetPhoi,0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(SoLuongNo,0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(Chet1_30,0) + "</td><td></td></tr>");
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