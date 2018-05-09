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
    public partial class r_thongkecachet : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        private void BindControls()
        {
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
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

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string strSQL = "QLCS_BCTK_CaChet";
                string tieude = "";
                tieude += "<b>BẢNG THỐNG KÊ CÁ @KieuBC@ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                if (ddlKieuBC.SelectedValue == "Chet") { strSQL = "QLCS_BCTK_CaChet"; tieude = tieude.Replace("@KieuBC@", "CHẾT"); }
                else if (ddlKieuBC.SelectedValue == "LoaiThai") { strSQL = "QLCS_BCTK_CaLoaiThai"; tieude = tieude.Replace("@KieuBC@", "LOẠI THẢI"); }
                else if (ddlKieuBC.SelectedValue == "Ban") { strSQL = "QLCS_BCTK_CaBan"; tieude = tieude.Replace("@KieuBC@", "BÁN"); }
                else if (ddlKieuBC.SelectedValue == "GietMo") { strSQL = "QLCS_BCTK_CaGietMo"; tieude = tieude.Replace("@KieuBC@", "GIẾT MỔ"); }
                else if (ddlKieuBC.SelectedValue == "Nhap") { strSQL = "QLCS_BCTK_CaNhap"; tieude = tieude.Replace("@KieuBC@", "NHẬP"); }

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
                param[2] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr>
                      <th>STT</th>
                      <th>");
                if (ddlLoaiBC.SelectedValue == "TheoChuong") sb.Append("Chuồng");
                else if (ddlLoaiBC.SelectedValue == "TheoDan") sb.Append("Loại cá");
                sb.Append(@"</th>
                      <th>Số lượng cá</th><th>Chi tiết</th><th>Giống</th><th>Chi tiết giống</th><th>Tăng trọng</th><th>Chi tiết tăng trọng</th>
                     </tr></thead><tbody>");
                int i = 1;
                int tongca = 0;
                int tongcagiong = 0;
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("<tr><td>" + i.ToString() + "</td><td style='text-align:left;'>" + r["TieuChi"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["TongCa"], 0) + "</td><td style='text-align:left;'>" + Config.Split(r["ChiTiet"].ToString(),3) + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["TongCaGiong"], 0) + "</td><td style='text-align:left;'>" + Config.Split(r["ChiTietGiong"].ToString(),3) + "</td><td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(r["TongCa"]) - Convert.ToInt32(r["TongCaGiong"]), 0) + "</td><td style='text-align:left;'>" + Config.Split(r["ChiTietTT"].ToString(),3) + "</td></tr>");
                    i++;
                    tongca += Convert.ToInt32(r["TongCa"]);
                    tongcagiong += Convert.ToInt32(r["TongCaGiong"]);
                }
                sb.Append("<tr><td style='text-align:center;font-weight:bold;'>Tổng cộng</td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tongca, 0) + "</td><td></td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tongcagiong, 0) + "</td><td></td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tongca-tongcagiong, 0) + "</td><td></td></tr>");
                sb.Append("</tbody></table>");
                lt.Text = sb.ToString();
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
                string filename = "CaChet.xls";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                string strSQL = "QLCS_BCTK_CaChet";
                string tieude = "";
                tieude += "<b>BẢNG THỐNG KÊ CÁ @KieuBC@ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                if (ddlKieuBC.SelectedValue == "Chet") { strSQL = "QLCS_BCTK_CaChet"; tieude = tieude.Replace("@KieuBC@", "CHẾT"); filename = "CaChet.xls"; }
                else if (ddlKieuBC.SelectedValue == "LoaiThai") { strSQL = "QLCS_BCTK_CaLoaiThai"; tieude = tieude.Replace("@KieuBC@", "LOẠI THẢI"); filename = "CaLoaiThai.xls"; }
                else if (ddlKieuBC.SelectedValue == "Ban") { strSQL = "QLCS_BCTK_CaBan"; tieude = tieude.Replace("@KieuBC@", "BÁN"); filename = "CaBan.xls"; }
                else if (ddlKieuBC.SelectedValue == "GietMo") { strSQL = "QLCS_BCTK_CaGietMo"; tieude = tieude.Replace("@KieuBC@", "GIẾT MỔ"); filename = "CaGietMo.xls"; }
                else if (ddlKieuBC.SelectedValue == "Nhap") { strSQL = "QLCS_BCTK_CaNhap"; tieude = tieude.Replace("@KieuBC@", "NHẬP"); filename = "CaNhap.xls"; }

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
                param[2] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");
                sb.Append("<table border='1'><tr><td colspan='8'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/></td></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                      <td>STT</td>
                      <td>");
                if (ddlLoaiBC.SelectedValue == "TheoChuong") sb.Append("Chuồng");
                else if (ddlLoaiBC.SelectedValue == "TheoDan") sb.Append("Loại cá");
                sb.Append(@"</td>
                      <td>Số lượng cá</td><td>Chi tiết</td><td>Giống</td><td>Chi tiết giống</td><td>Tăng trọng</td><td>Chi tiết tăng trọng</td>
                     </tr>");
                int i = 1;
                int tongca = 0;
                int tongcagiong = 0;
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("<tr><td>" + i.ToString() + "</td><td style='text-align:left;'>" + r["TieuChi"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["TongCa"], 0) + "</td><td style='text-align:left;'>" + r["ChiTiet"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["TongCaGiong"], 0) + "</td><td style='text-align:left;'>" + r["ChiTietGiong"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(r["TongCa"]) - Convert.ToInt32(r["TongCaGiong"]), 0) + "</td><td style='text-align:left;'>" + r["ChiTietTT"].ToString() + "</td></tr>");
                    i++;
                    tongca += Convert.ToInt32(r["TongCa"]);
                    tongcagiong += Convert.ToInt32(r["TongCaGiong"]);
                }
                sb.Append("<tr><td style='font-weight:bold;'>Tổng cộng</td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tongca, 0) + "</td><td></td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tongcagiong, 0) + "</td><td></td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tongca-tongcagiong, 0) + "</td><td></td></tr>");
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
    }
}