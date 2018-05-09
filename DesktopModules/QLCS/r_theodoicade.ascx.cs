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
    public partial class r_theodoicade : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable dtLoaiCa = csCont.LoadLoaiCaLon();
            ddlLoaiCa.DataSource = dtLoaiCa;
            ddlLoaiCa.DataTextField = "TenLoaiCa";
            ddlLoaiCa.DataValueField = "IDLoaiCa";
            ddlLoaiCa.DataBind();
            //ddlLoaiCa.Items.Insert(0, new ListItem("", "0"));
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
                int ConLaiSauChetPhoi2 = 0;
                int TongNgayAp = 0;
                string filename = "CaDe";
                string tieude = "";
                string strSQL = "QLCS_BCTK_CaDe_LoaiCa";
                SqlParameter[] param = new SqlParameter[4];
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                string StrLoaiCa = "";
                if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
                {
                    StrLoaiCa = "@" + Config.GetSelectedValues(ddlLoaiCa).Replace(", ", "@@").Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 1);
                }
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                param[2] = new SqlParameter("@StrLoaiCa", StrLoaiCa);
                param[3] = new SqlParameter("@OrderBy", ddlOrderBy.SelectedValue);
                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                filename = filename.Replace("/", "_");
                tieude += "<b>BẢNG THEO DÕI CÁ ĐẺ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body style='text-align:center;font-family:Times New Roman;'><br/>
                    <div style='text-align:center;font-weight:bold;font-size:14pt;'>" + tieude + "</div><br/>");
                sb.Append("<table border='1'>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><td rowspan='2'>STT</td><td rowspan='2'>Khay ấp</td><td rowspan='2'>Phòng ấp</td><td rowspan='2'>Ngày vào ấp</td><td rowspan='2'>Loại cá đẻ</td><td rowspan='2'>Chuồng</td><td rowspan='2'>Vi cắt</td><td rowspan='2'>Mã</td><td rowspan='2'>Khối lượng trứng<br/>bình quân (g)</td><td rowspan='2'>Tổng trứng đẻ</td><td colspan='3'>Trứng hủy</td><td rowspan='2'>Trứng ấp</td><td rowspan='2'>Tổng trứng ấp</td><td rowspan='2'>Không phôi</td><td rowspan='2'>Có phôi</td><td rowspan='2'>Chết phôi 1</td><td rowspan='2'>Còn lại</td><td rowspan='2'>Chết phôi 2</td><td colspan='2'>Theo dõi nở</td><td rowspan='2'>Tổng ngày ấp</td><td rowspan='2'>Ghi chú</td><td colspan='4'>Theo dõi úm</td><td rowspan='2'>Tỷ lệ khối lượng con nở/<br/>khối lượng trứng bình quân(%)</td></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><td>Vỡ</td><td>Thải loại</td><td>Tổng cộng</td><td>Ngày nở</td><td>Số lượng</td><td>Khay úm</td><td>Khối lượng con<br/>bình quân (g)</td><td>Chiều dài<br/>bình quân (cm)</td><td>Vòng bụng<br/>bình quân (cm)</td></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><td>1</td><td>2</td><td>3</td><td>4</td><td>5</td><td>6</td><td>7</td><td>8</td><td>9</td><td>10</td><td>11</td><td>12</td><td>13=11+12</td><td>14=10-13</td><td>15</td><td>16</td><td>17=14-16</td><td>18</td><td>19=17-18</td><td>20</td><td>21</td><td>22=19-20</td><td>23=21-4</td><td>24</td><td></td><td></td><td></td><td></td><td></td></tr>");
                string TenLoaiCa = "";
                int currIndex = 1;
                System.Text.StringBuilder s;
                if (ddlOrderBy.SelectedValue == "LoaiCa")
                {
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        s = new System.Text.StringBuilder();
                        DataRow r = dt.Rows[i - 1];
                        string[] ss = r["Properties"].ToString().Split(new char[] { ',' });
                        if (TenLoaiCa != ss[2])
                        {
                            currIndex = 1;
                            if (TenLoaiCa != "")
                            {
                                s.Append("<tr style='font-weight:bold;color:#FF0000; vertical-align:middle;'><td></td><td></td><td>T.C</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(currTDe, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTVo, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTTLoai, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTHuy, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTongTrungAp, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(currTKoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCP1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCL, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCP2, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(currTNo, 0) + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                                currTDe = 0;
                                currTVo = 0;
                                currTTLoai = 0;
                                currTHuy = 0;
                                currTKoP = 0;
                                currTCoP = 0;
                                currTCP1 = 0;
                                currTCL = 0;
                                currTCP2 = 0;
                                currTNo = 0;
                                currTongTrungAp = 0;
                            }
                            TenLoaiCa = ss[2];
                            s.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td colspan='2' style='text-align:left;'>" + TenLoaiCa + "</td><td colspan='26'></td></tr>");
                        }
                        s.Append("<tr style='vertical-align:middle;'>");
                        s.Append("<td>" + currIndex.ToString() + "</td>");
                        currIndex++;
                        s.Append("<td style='text-align:left;'>" + r["TenKhayAp"].ToString() + "</td>");
                        s.Append("<td style='text-align:left;'>" + r["TenPhongAp"].ToString() + "</td>");
                        s.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayVaoAp"]).ToString("dd/MM/yyyy") + "</td>");
                        s.Append("<td style='text-align:left;'>" + ss[2] + "</td>");
                        s.Append("<td style='text-align:left;'>" + ss[3] + "</td>");
                        s.Append("<td style='text-align:left;'>" + ss[4] + "</td>");
                        s.Append("<td style='text-align:left;'>" + ss[1] + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrongLuongTrungBQ"].ToString() + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrungDe"].ToString() + "</td>");
                        TDe += Convert.ToInt32(r["TrungDe"]);
                        currTDe += Convert.ToInt32(r["TrungDe"]);
                        s.Append("<td style='text-align:right;'>" + r["TrungVo"].ToString() + "</td>");
                        TVo += Convert.ToInt32(r["TrungVo"]);
                        currTVo += Convert.ToInt32(r["TrungVo"]);
                        s.Append("<td style='text-align:right;'>" + r["TrungThaiLoai"].ToString() + "</td>");
                        TTLoai += Convert.ToInt32(r["TrungThaiLoai"]);
                        currTTLoai += Convert.ToInt32(r["TrungThaiLoai"]);
                        TrungHuy = Convert.ToInt32(r["TrungVo"]) + Convert.ToInt32(r["TrungThaiLoai"]);
                        s.Append("<td style='text-align:right;'>" + TrungHuy.ToString() + "</td>");
                        THuy += TrungHuy;
                        currTHuy += TrungHuy;
                        TrungAp = Convert.ToInt32(r["TrungDe"]) - TrungHuy;
                        s.Append("<td style='text-align:right;'>" + TrungAp.ToString() + "</td>");
                        TongTrungAp += TrungAp;
                        currTongTrungAp += TrungAp;
                        s.Append("<td style='text-align:right;'>" + Config.ToXVal2(TongTrungAp, 0) + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrungKhongPhoi"].ToString() + "</td>");
                        TKoP += Convert.ToInt32(r["TrungKhongPhoi"]);
                        currTKoP += Convert.ToInt32(r["TrungKhongPhoi"]);
                        CoPhoi = TrungAp - Convert.ToInt32(r["TrungKhongPhoi"]);
                        s.Append("<td style='text-align:right;'>" + CoPhoi.ToString() + "</td>");
                        TCoP += CoPhoi;
                        currTCoP += CoPhoi;
                        s.Append("<td style='text-align:right;'>" + r["TrungChetPhoi1"].ToString() + "</td>");
                        TCP1 += Convert.ToInt32(r["TrungChetPhoi1"]);
                        currTCP1 += Convert.ToInt32(r["TrungChetPhoi1"]);
                        ConLaiSauChetPhoi1 = CoPhoi - Convert.ToInt32(r["TrungChetPhoi1"]);
                        s.Append("<td style='text-align:right;'>" + ConLaiSauChetPhoi1.ToString() + "</td>");
                        TCL += ConLaiSauChetPhoi1;
                        currTCL += ConLaiSauChetPhoi1;
                        s.Append("<td style='text-align:right;'>" + r["TrungChetPhoi2"].ToString() + "</td>");
                        TCP2 += Convert.ToInt32(r["TrungChetPhoi2"]);
                        currTCP2 += Convert.ToInt32(r["TrungChetPhoi2"]);
                        ConLaiSauChetPhoi2 = ConLaiSauChetPhoi1 - Convert.ToInt32(r["TrungChetPhoi2"]);
                        if (r["NgayNo"] != DBNull.Value)
                        {
                            s.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayNo"]).ToString("dd/MM/yyyy") + "</td>");
                            TimeSpan ts = ((DateTime)r["NgayNo"]).Subtract((DateTime)r["NgayVaoAp"]);
                            TongNgayAp = ts.Days;
                            s.Append("<td style='text-align:right;'>" + ConLaiSauChetPhoi2.ToString() + "</td>");
                            TNo += ConLaiSauChetPhoi2;
                            currTNo += ConLaiSauChetPhoi2;
                        }
                        else
                        {
                            s.Append("<td></td>");
                            TongNgayAp = 0;
                            if(ConLaiSauChetPhoi2 == 0)
                                s.Append("<td style='text-align:right;'>" + ConLaiSauChetPhoi2.ToString() + "</td>");
                            else
                                s.Append("<td style='text-align:right;'></td>");
                        }
                        
                        
                        s.Append("<td style='text-align:right;'>" + TongNgayAp.ToString() + "</td>");

                        string status = r["Status"].ToString();
                        if (status != "Đã nở")
                        {
                            if (ConLaiSauChetPhoi2 == 0)
                                status = "Không nở";
                            else
                                status = "Chưa nở";
                        }
                        s.Append("<td style='text-align:left;'>" + status + "</td>");

                        s.Append("<td style='text-align:left;'>" + r["TenKhayUm"].ToString() + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrongLuongConBQ"].ToString() + "</td>");
                        s.Append("<td style='text-align:right;'>" + Config.ToXVal(r["ChieuDaiBQ"]) + "</td>");
                        s.Append("<td style='text-align:right;'>" + Config.ToXVal(r["VongBungBQ"]) + "</td>");
                        if (r["TrongLuongConBQ"] == DBNull.Value || r["TrongLuongTrungBQ"] == DBNull.Value)
                        {
                            s.Append("<td></td></tr>");
                        }
                        else if (Convert.ToDecimal(r["TrongLuongTrungBQ"]) != 0)
                        {
                            s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(100 * Convert.ToDecimal(r["TrongLuongConBQ"]) / Convert.ToDecimal(r["TrongLuongTrungBQ"]), 1) + "</td></tr>");
                        }
                        else
                        {
                            s.Append("<td></td></tr>");
                        }
                        sb.Append(s.ToString());
                    }
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td></td><td>T.C</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(currTDe, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTVo, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTTLoai, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTHuy, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTongTrungAp, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(currTKoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCP1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCL, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCP2, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(currTNo, 0) + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td></td><td>Tổng cộng</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TDe, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TVo, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TTLoai, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(THuy, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TongTrungAp, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TKoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCP1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCL, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCP2, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TNo, 0) + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    sb.Append("</table>");
                    sb.Append(Config.Footer(UserInfo.Profile.City, UserInfo.FirstName + " " + UserInfo.LastName));
                    sb.Append("</body></html>");
                    Response.Write(sb.ToString());
                }
                else if(ddlOrderBy.SelectedValue == "KhayAp")
                {
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        s = new System.Text.StringBuilder();
                        DataRow r = dt.Rows[i - 1];
                        string[] ss = r["Properties"].ToString().Split(new char[] { ',' });
                        s.Append("<tr style='vertical-align:middle;'>");
                        s.Append("<td>" + currIndex.ToString() + "</td>");
                        currIndex++;
                        s.Append("<td style='text-align:left;'>" + r["TenKhayAp"].ToString() + "</td>");
                        s.Append("<td style='text-align:left;'>" + r["TenPhongAp"].ToString() + "</td>");
                        s.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayVaoAp"]).ToString("dd/MM/yyyy") + "</td>");
                        s.Append("<td style='text-align:left;'>" + ss[2] + "</td>");
                        s.Append("<td style='text-align:left;'>" + ss[3] + "</td>");
                        s.Append("<td style='text-align:left;'>" + ss[4] + "</td>");
                        s.Append("<td style='text-align:left;'>" + ss[1] + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrongLuongTrungBQ"].ToString() + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrungDe"].ToString() + "</td>");
                        TDe += Convert.ToInt32(r["TrungDe"]);
                        s.Append("<td style='text-align:right;'>" + r["TrungVo"].ToString() + "</td>");
                        TVo += Convert.ToInt32(r["TrungVo"]);
                        s.Append("<td style='text-align:right;'>" + r["TrungThaiLoai"].ToString() + "</td>");
                        TTLoai += Convert.ToInt32(r["TrungThaiLoai"]);
                        TrungHuy = Convert.ToInt32(r["TrungVo"]) + Convert.ToInt32(r["TrungThaiLoai"]);
                        s.Append("<td style='text-align:right;'>" + TrungHuy.ToString() + "</td>");
                        THuy += TrungHuy;
                        TrungAp = Convert.ToInt32(r["TrungDe"]) - TrungHuy;
                        s.Append("<td style='text-align:right;'>" + TrungAp.ToString() + "</td>");
                        TongTrungAp += TrungAp;
                        s.Append("<td style='text-align:right;'>" + Config.ToXVal2(TongTrungAp, 0) + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrungKhongPhoi"].ToString() + "</td>");
                        TKoP += Convert.ToInt32(r["TrungKhongPhoi"]);
                        CoPhoi = TrungAp - Convert.ToInt32(r["TrungKhongPhoi"]);
                        s.Append("<td style='text-align:right;'>" + CoPhoi.ToString() + "</td>");
                        TCoP += CoPhoi;
                        s.Append("<td style='text-align:right;'>" + r["TrungChetPhoi1"].ToString() + "</td>");
                        TCP1 += Convert.ToInt32(r["TrungChetPhoi1"]);
                        ConLaiSauChetPhoi1 = CoPhoi - Convert.ToInt32(r["TrungChetPhoi1"]);
                        s.Append("<td style='text-align:right;'>" + ConLaiSauChetPhoi1.ToString() + "</td>");
                        TCL += ConLaiSauChetPhoi1;
                        s.Append("<td style='text-align:right;'>" + r["TrungChetPhoi2"].ToString() + "</td>");
                        TCP2 += Convert.ToInt32(r["TrungChetPhoi2"]);
                        ConLaiSauChetPhoi2 = ConLaiSauChetPhoi1 - Convert.ToInt32(r["TrungChetPhoi2"]);
                        if (r["NgayNo"] != DBNull.Value)
                        {
                            s.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayNo"]).ToString("dd/MM/yyyy") + "</td>");
                            TimeSpan ts = ((DateTime)r["NgayNo"]).Subtract((DateTime)r["NgayVaoAp"]);
                            TongNgayAp = ts.Days;
                            s.Append("<td style='text-align:right;'>" + ConLaiSauChetPhoi2.ToString() + "</td>");
                            TNo += ConLaiSauChetPhoi2;
                        }
                        else
                        {
                            s.Append("<td></td>");
                            TongNgayAp = 0;
                            if (ConLaiSauChetPhoi2 == 0)
                                s.Append("<td style='text-align:right;'>" + ConLaiSauChetPhoi2.ToString() + "</td>");
                            else
                                s.Append("<td style='text-align:right;'></td>");
                        }
                        
                        
                        s.Append("<td style='text-align:right;'>" + TongNgayAp.ToString() + "</td>");

                        string status = r["Status"].ToString();
                        if (status != "Đã nở")
                        {
                            if (ConLaiSauChetPhoi2 == 0)
                                status = "Không nở";
                            else
                                status = "Chưa nở";
                        }
                        s.Append("<td style='text-align:left;'>" + status + "</td>");

                        s.Append("<td style='text-align:left;'>" + r["TenKhayUm"].ToString() + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrongLuongConBQ"].ToString() + "</td>");
                        s.Append("<td style='text-align:right;'>" + Config.ToXVal(r["ChieuDaiBQ"]) + "</td>");
                        s.Append("<td style='text-align:right;'>" + Config.ToXVal(r["VongBungBQ"]) + "</td>");
                        if (r["TrongLuongConBQ"] == DBNull.Value || r["TrongLuongTrungBQ"] == DBNull.Value)
                        {
                            s.Append("<td></td></tr>");
                        }
                        else if (Convert.ToDecimal(r["TrongLuongTrungBQ"]) != 0)
                        {
                            s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(100 * Convert.ToDecimal(r["TrongLuongConBQ"]) / Convert.ToDecimal(r["TrongLuongTrungBQ"]), 1) + "</td></tr>");
                        }
                        else
                        {
                            s.Append("<td></td></tr>");
                        }
                        sb.Append(s.ToString());
                    }
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td></td><td>Tổng cộng</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TDe, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TVo, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TTLoai, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(THuy, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TongTrungAp, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TKoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCP1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCL, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCP2, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TNo, 0) + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    sb.Append("</table>");
                    sb.Append(Config.Footer(UserInfo.Profile.City, UserInfo.FirstName + " " + UserInfo.LastName));
                    sb.Append("</body></html>");
                    Response.Write(sb.ToString());
                }
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
                int ConLaiSauChetPhoi2 = 0;
                int TongNgayAp = 0;
                string tieude = "";
                string strSQL = "QLCS_BCTK_CaDe_LoaiCa";
                SqlParameter[] param = new SqlParameter[4];
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                string StrLoaiCa = "";
                if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
                {
                    StrLoaiCa = "@" + Config.GetSelectedValues(ddlLoaiCa).Replace(", ", "@@").Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 1);
                }
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                param[2] = new SqlParameter("@StrLoaiCa", StrLoaiCa);
                param[3] = new SqlParameter("@OrderBy", ddlOrderBy.SelectedValue);
                tieude += "<b>BẢNG THEO DÕI CÁ ĐẺ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<div style='text-align:center;font-weight:bold;font-size:14pt;'>" + tieude + "</div><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><th rowspan='2'>STT</th><th rowspan='2'>Khay ấp</th><th rowspan='2'>Phòng ấp</th><th rowspan='2'>Ngày vào ấp</th><th rowspan='2'>Loại cá đẻ</th><th rowspan='2'>Chuồng</th><th rowspan='2'>Vi cắt</th><th rowspan='2'>Mã</th><th rowspan='2'>Khối lượng trứng<br/>bình quân (g)</th><th rowspan='2'>Tổng trứng đẻ</th><th colspan='3'>Trứng hủy</th><th rowspan='2'>Trứng ấp</th><th rowspan='2'>Tổng trứng ấp</th><th rowspan='2'>Không phôi</th><th rowspan='2'>Có phôi</th><th rowspan='2'>Chết phôi 1</th><th rowspan='2'>Còn lại</th><th rowspan='2'>Chết phôi 2</th><th colspan='2'>Theo dõi nở</th><th rowspan='2'>Tổng ngày ấp</th><th rowspan='2'>Ghi chú</th><th colspan='4'>Theo dõi úm</th><th rowspan='2'>Tỷ lệ khối lượng con nở/<br/>khối lượng trứng bình quân(%)</th></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><th>Vỡ</th><th>Thải loại</th><th>Tổng cộng</th><th>Ngày nở</th><th>Số lượng</th><th>Khay úm</th><th>Khối lượng con<br/>bình quân (g)</th><th>Chiều dài<br/>bình quân (cm)</th><th>Vòng bụng<br/>bình quân (cm)</th></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><th>1</th><th>2</th><th>3</th><th>4</th><th>5</th><th>6</th><th>7</th><th>8</th><th>9</th><th>10</th><th>11</th><th>12</th><th>13=11+12</th><th>14=10-13</th><th>15</th><th>16</th><th>17=14-16</th><th>18</th><th>19=17-18</th><th>20</th><th>21</th><th>22=19-20</th><th>23=21-4</th><th>24</th><th></th><th></th><th></th><th></th><th></th></tr></thead><tbody>");
                string TenLoaiCa = "";
                int currIndex = 1;
                System.Text.StringBuilder s;
                if (ddlOrderBy.SelectedValue == "LoaiCa")
                {
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        s = new System.Text.StringBuilder();
                        DataRow r = dt.Rows[i - 1];
                        string[] ss = r["Properties"].ToString().Split(new char[] { ',' });
                        if (TenLoaiCa != ss[2])
                        {
                            currIndex = 1;
                            if (TenLoaiCa != "")
                            {
                                s.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td></td><td>T.C</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(currTDe, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTVo, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTTLoai, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTHuy, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTongTrungAp, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(currTKoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCP1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCL, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCP2, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(currTNo, 0) + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                                currTDe = 0;
                                currTVo = 0;
                                currTTLoai = 0;
                                currTHuy = 0;
                                currTKoP = 0;
                                currTCoP = 0;
                                currTCP1 = 0;
                                currTCL = 0;
                                currTCP2 = 0;
                                currTNo = 0;
                                currTongTrungAp = 0;
                            }
                            TenLoaiCa = ss[2];
                            s.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td>" + TenLoaiCa + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        }
                        s.Append("<tr style='vertical-align:middle;'>");
                        s.Append("<td style='text-align:center;'>" + currIndex.ToString() + "</td>");
                        currIndex++;
                        s.Append("<td>" + r["TenKhayAp"].ToString() + "</td>");
                        s.Append("<td>" + r["TenPhongAp"].ToString() + "</td>");
                        s.Append("<td style='text-align:center;'>" + ((DateTime)r["NgayVaoAp"]).ToString("dd/MM/yyyy") + "</td>");
                        s.Append("<td>" + ss[2] + "</td>");
                        s.Append("<td>" + ss[3] + "</td>");
                        s.Append("<td>" + ss[4] + "</td>");
                        s.Append("<td>" + ss[1] + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrongLuongTrungBQ"].ToString() + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrungDe"].ToString() + "</td>");
                        TDe += Convert.ToInt32(r["TrungDe"]);
                        currTDe += Convert.ToInt32(r["TrungDe"]);
                        s.Append("<td style='text-align:right;'>" + r["TrungVo"].ToString() + "</td>");
                        TVo += Convert.ToInt32(r["TrungVo"]);
                        currTVo += Convert.ToInt32(r["TrungVo"]);
                        s.Append("<td style='text-align:right;'>" + r["TrungThaiLoai"].ToString() + "</td>");
                        TTLoai += Convert.ToInt32(r["TrungThaiLoai"]);
                        currTTLoai += Convert.ToInt32(r["TrungThaiLoai"]);
                        TrungHuy = Convert.ToInt32(r["TrungVo"]) + Convert.ToInt32(r["TrungThaiLoai"]);
                        s.Append("<td style='text-align:right;'>" + TrungHuy.ToString() + "</td>");
                        THuy += TrungHuy;
                        currTHuy += TrungHuy;
                        TrungAp = Convert.ToInt32(r["TrungDe"]) - TrungHuy;
                        s.Append("<td style='text-align:right;'>" + TrungAp.ToString() + "</td>");
                        TongTrungAp += TrungAp;
                        currTongTrungAp += TrungAp;
                        s.Append("<td style='text-align:right;'>" + Config.ToXVal2(TongTrungAp, 0) + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrungKhongPhoi"].ToString() + "</td>");
                        TKoP += Convert.ToInt32(r["TrungKhongPhoi"]);
                        currTKoP += Convert.ToInt32(r["TrungKhongPhoi"]);
                        CoPhoi = TrungAp - Convert.ToInt32(r["TrungKhongPhoi"]);
                        s.Append("<td style='text-align:right;'>" + CoPhoi.ToString() + "</td>");
                        TCoP += CoPhoi;
                        currTCoP += CoPhoi;
                        s.Append("<td style='text-align:right;'>" + r["TrungChetPhoi1"].ToString() + "</td>");
                        TCP1 += Convert.ToInt32(r["TrungChetPhoi1"]);
                        currTCP1 += Convert.ToInt32(r["TrungChetPhoi1"]);
                        ConLaiSauChetPhoi1 = CoPhoi - Convert.ToInt32(r["TrungChetPhoi1"]);
                        s.Append("<td style='text-align:right;'>" + ConLaiSauChetPhoi1.ToString() + "</td>");
                        TCL += ConLaiSauChetPhoi1;
                        currTCL += ConLaiSauChetPhoi1;
                        s.Append("<td style='text-align:right;'>" + r["TrungChetPhoi2"].ToString() + "</td>");
                        TCP2 += Convert.ToInt32(r["TrungChetPhoi2"]);
                        currTCP2 += Convert.ToInt32(r["TrungChetPhoi2"]);
                        ConLaiSauChetPhoi2 = ConLaiSauChetPhoi1 - Convert.ToInt32(r["TrungChetPhoi2"]);
                        if (r["NgayNo"] != DBNull.Value)
                        {
                            s.Append("<td style='text-align:center;'>" + ((DateTime)r["NgayNo"]).ToString("dd/MM/yyyy") + "</td>");
                            TimeSpan ts = ((DateTime)r["NgayNo"]).Subtract((DateTime)r["NgayVaoAp"]);
                            TongNgayAp = ts.Days;
                            s.Append("<td style='text-align:right;'>" + ConLaiSauChetPhoi2.ToString() + "</td>");
                            TNo += ConLaiSauChetPhoi2;
                            currTNo += ConLaiSauChetPhoi2;
                        }
                        else
                        {
                            s.Append("<td></td>");
                            TongNgayAp = 0;
                            if(ConLaiSauChetPhoi2 == 0)
                                s.Append("<td style='text-align:right;'>" + ConLaiSauChetPhoi2.ToString() + "</td>");
                            else
                                s.Append("<td style='text-align:right;'></td>");
                        }
                        
                        
                        s.Append("<td style='text-align:right;'>" + TongNgayAp.ToString() + "</td>");

                        string status = r["Status"].ToString();
                        if (status != "Đã nở")
                        {
                            if (ConLaiSauChetPhoi2 == 0)
                                status = "Không nở";
                            else
                                status = "Chưa nở";
                        }
                        s.Append("<td style='text-align:left;'>" + status + "</td>");

                        s.Append("<td>" + r["TenKhayUm"].ToString() + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrongLuongConBQ"].ToString() + "</td>");
                        s.Append("<td style='text-align:right;'>" + Config.ToXVal(r["ChieuDaiBQ"]) + "</td>");
                        s.Append("<td style='text-align:right;'>" + Config.ToXVal(r["VongBungBQ"]) + "</td>");
                        if (r["TrongLuongConBQ"] == DBNull.Value || r["TrongLuongTrungBQ"] == DBNull.Value)
                        {
                            s.Append("<td></td></tr>");
                        }
                        else if (Convert.ToDecimal(r["TrongLuongTrungBQ"]) != 0)
                        {
                            s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(100* Convert.ToDecimal(r["TrongLuongConBQ"]) / Convert.ToDecimal(r["TrongLuongTrungBQ"]), 1) + "</td></tr>");
                        }
                        else
                        {
                            s.Append("<td></td></tr>");
                        }
                        sb.Append(s.ToString());
                    }
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td></td><td>T.C</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(currTDe, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTVo, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTTLoai, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTHuy, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTongTrungAp, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(currTKoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCP1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCL, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(currTCP2, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(currTNo, 0) + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td></td><td>Tổng cộng</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TDe, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TVo, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TTLoai, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(THuy, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TongTrungAp, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TKoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCP1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCL, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCP2, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TNo, 0) + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                }
                else if(ddlOrderBy.SelectedValue == "KhayAp")
                {
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        s = new System.Text.StringBuilder();
                        DataRow r = dt.Rows[i - 1];
                        string[] ss = r["Properties"].ToString().Split(new char[] { ',' });
                        s.Append("<tr style='vertical-align:middle;'>");
                        s.Append("<td style='text-align:center;'>" + currIndex.ToString() + "</td>");
                        currIndex++;
                        s.Append("<td>" + r["TenKhayAp"].ToString() + "</td>");
                        s.Append("<td>" + r["TenPhongAp"].ToString() + "</td>");
                        s.Append("<td style='text-align:center;'>" + ((DateTime)r["NgayVaoAp"]).ToString("dd/MM/yyyy") + "</td>");
                        s.Append("<td>" + ss[2] + "</td>");
                        s.Append("<td>" + ss[3] + "</td>");
                        s.Append("<td>" + ss[4] + "</td>");
                        s.Append("<td>" + ss[1] + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrongLuongTrungBQ"].ToString() + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrungDe"].ToString() + "</td>");
                        TDe += Convert.ToInt32(r["TrungDe"]);
                        s.Append("<td style='text-align:right;'>" + r["TrungVo"].ToString() + "</td>");
                        TVo += Convert.ToInt32(r["TrungVo"]);
                        s.Append("<td style='text-align:right;'>" + r["TrungThaiLoai"].ToString() + "</td>");
                        TTLoai += Convert.ToInt32(r["TrungThaiLoai"]);
                        TrungHuy = Convert.ToInt32(r["TrungVo"]) + Convert.ToInt32(r["TrungThaiLoai"]);
                        s.Append("<td style='text-align:right;'>" + TrungHuy.ToString() + "</td>");
                        THuy += TrungHuy;
                        TrungAp = Convert.ToInt32(r["TrungDe"]) - TrungHuy;
                        s.Append("<td style='text-align:right;'>" + TrungAp.ToString() + "</td>");
                        TongTrungAp += TrungAp;
                        s.Append("<td style='text-align:right;'>" + Config.ToXVal2(TongTrungAp, 0) + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrungKhongPhoi"].ToString() + "</td>");
                        TKoP += Convert.ToInt32(r["TrungKhongPhoi"]);
                        CoPhoi = TrungAp - Convert.ToInt32(r["TrungKhongPhoi"]);
                        s.Append("<td style='text-align:right;'>" + CoPhoi.ToString() + "</td>");
                        TCoP += CoPhoi;
                        s.Append("<td style='text-align:right;'>" + r["TrungChetPhoi1"].ToString() + "</td>");
                        TCP1 += Convert.ToInt32(r["TrungChetPhoi1"]);
                        ConLaiSauChetPhoi1 = CoPhoi - Convert.ToInt32(r["TrungChetPhoi1"]);
                        s.Append("<td style='text-align:right;'>" + ConLaiSauChetPhoi1.ToString() + "</td>");
                        TCL += ConLaiSauChetPhoi1;
                        s.Append("<td style='text-align:right;'>" + r["TrungChetPhoi2"].ToString() + "</td>");
                        TCP2 += Convert.ToInt32(r["TrungChetPhoi2"]);
                        ConLaiSauChetPhoi2 = ConLaiSauChetPhoi1 - Convert.ToInt32(r["TrungChetPhoi2"]);
                        if (r["NgayNo"] != DBNull.Value)
                        {
                            s.Append("<td style='text-align:center;'>" + ((DateTime)r["NgayNo"]).ToString("dd/MM/yyyy") + "</td>");
                            TimeSpan ts = ((DateTime)r["NgayNo"]).Subtract((DateTime)r["NgayVaoAp"]);
                            TongNgayAp = ts.Days;
                            s.Append("<td style='text-align:right;'>" + ConLaiSauChetPhoi2.ToString() + "</td>");
                            TNo += ConLaiSauChetPhoi2;
                        }
                        else
                        {
                            s.Append("<td></td>");
                            TongNgayAp = 0;
                            if(ConLaiSauChetPhoi2 == 0)
                                s.Append("<td style='text-align:right;'>" + ConLaiSauChetPhoi2.ToString() + "</td>");
                            else
                                s.Append("<td style='text-align:right;'></td>");
                        }
                        
                        
                        s.Append("<td style='text-align:right;'>" + TongNgayAp.ToString() + "</td>");

                        string status = r["Status"].ToString();
                        if (status != "Đã nở")
                        {
                            if (ConLaiSauChetPhoi2 == 0)
                                status = "Không nở";
                            else
                                status = "Chưa nở";
                        }
                        s.Append("<td style='text-align:left;'>" + status + "</td>");

                        s.Append("<td>" + r["TenKhayUm"].ToString() + "</td>");
                        s.Append("<td style='text-align:right;'>" + r["TrongLuongConBQ"].ToString() + "</td>");
                        s.Append("<td style='text-align:right;'>" + Config.ToXVal(r["ChieuDaiBQ"]) + "</td>");
                        s.Append("<td style='text-align:right;'>" + Config.ToXVal(r["VongBungBQ"]) + "</td>");
                        if (r["TrongLuongConBQ"] == DBNull.Value || r["TrongLuongTrungBQ"] == DBNull.Value)
                        {
                            s.Append("<td></td></tr>");
                        }
                        else if (Convert.ToDecimal(r["TrongLuongTrungBQ"]) != 0)
                        {
                            s.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\.0""'>" + Config.ToXVal2(100 * Convert.ToDecimal(r["TrongLuongConBQ"]) / Convert.ToDecimal(r["TrongLuongTrungBQ"]), 1) + "</td></tr>");
                        }
                        else
                        {
                            s.Append("<td></td></tr>");
                        }
                        sb.Append(s.ToString());
                    }
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td></td><td></td><td>Tổng cộng</td><td></td><td></td><td></td><td></td><td></td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TDe, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TVo, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TTLoai, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(THuy, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TongTrungAp, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TKoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCoP, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCP1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCL, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TCP2, 0) + "</td><td></td><td style='text-align:right;'>" + Config.ToXVal2(TNo, 0) + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}