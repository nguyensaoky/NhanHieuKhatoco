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
    public partial class r_tyleduccaisinhsan : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        int stt = 1;
        int tongca = 0;
        private CaSauController csCont = new CaSauController();
        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
                string filename = "ThongKeTyLeDucCaiSinhSan";
                filename += txtDate.Text + ".xls";
                filename = filename.Replace("/", "_");

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                string strSQL = "QLCS_BCTK_TyLeDucCaiSinhSan";
                SqlParameter[] param = new SqlParameter[1];
                if (txtDate.Text == "")
                {
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@d", txtDate.Text);
                string tieude = "<b>BẢNG THỐNG KÊ TỶ LỆ ĐỰC/CÁI ĐÀN CÁ SẤU SINH SẢN ĐẾN TRƯỚC NGÀY " + txtDate.Text + "</b>";

                DataTable dt = Config.SelectSP(strSQL, param);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");
                sb.Append("<table border='1'><tr><td colspan='16'><center style='font-weight:bold;font-size:14pt;'>" + tieude + "</center><br/></td></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <td rowspan='2'>CHUỒNG</td>
                          <td colspan='3'>SINH SẢN 1</td>
                          <td colspan='3'>SINH SẢN 2</td>
                          <td colspan='3'>TSCĐ</td>
                          <td colspan='3'>NƯỚC MẶN</td>
                          <td colspan='3'>TỔNG CỘNG</td>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'><td>Đực</td><td>Cái</td><td>Tỷ lệ<br/>Đực/cái (%)</td><td>Đực</td><td>Cái</td><td>Tỷ lệ<br/>Đực/cái (%)</td><td>Đực</td><td>Cái</td><td>Tỷ lệ<br/>Đực/cái (%)</td><td>Đực</td><td>Cái</td><td>Tỷ lệ<br/>Đực/cái (%)</td><td>Đực</td><td>Cái</td><td>Tỷ lệ<br/>Đực/cái (%)</td></tr>"
                    );
                decimal tyletongSS1 = 0;
                decimal tongDucSS1 = 0;
                decimal tongCaiSS1 = 0;
                decimal tyletongSS2 = 0;
                decimal tongDucSS2 = 0;
                decimal tongCaiSS2 = 0;
                decimal tyletongTSCD = 0;
                decimal tongDucTSCD = 0;
                decimal tongCaiTSCD = 0;
                decimal tyletongNM = 0;
                decimal tongDucNM = 0;
                decimal tongCaiNM = 0;
                decimal tyleTotal = 0;
                decimal totalDuc = 0;
                decimal totalCai = 0;

                foreach (DataRow r in dt.Rows)
                {
                    decimal tongDuc = 0;
                    decimal tongCai = 0;
                    sb.Append("<tr><td style='text-align:left;'>" + r["TenChuong"].ToString() + "</td>");
                    
                    decimal tyleSS1 = 0;
                    decimal DucSS1 = 0; if (r[1] != DBNull.Value) DucSS1 = Convert.ToDecimal(r[1]); tongDuc += DucSS1; tongDucSS1 += DucSS1;
                    decimal CaiSS1 = 0; if (r[2] != DBNull.Value) CaiSS1 = Convert.ToDecimal(r[2]); tongCai += CaiSS1; tongCaiSS1 += CaiSS1;
                    if (CaiSS1 != 0) tyleSS1 = DucSS1 / CaiSS1 * 100;
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(DucSS1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(CaiSS1, 0) + "</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(tyleSS1, 2) + "</td>");
                    
                    decimal tyleSS2 = 0;
                    decimal DucSS2 = 0; if (r[3] != DBNull.Value) DucSS2 = Convert.ToDecimal(r[3]); tongDuc += DucSS2; tongDucSS2 += DucSS2;
                    decimal CaiSS2 = 0; if (r[4] != DBNull.Value) CaiSS2 = Convert.ToDecimal(r[4]); tongCai += CaiSS2; tongCaiSS2 += CaiSS2;
                    if (CaiSS2 != 0) tyleSS2 = DucSS2 / CaiSS2 * 100;
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(DucSS2, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(CaiSS2, 0) + "</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(tyleSS2, 2) + "</td>");

                    decimal tyleTSCD = 0;
                    decimal DucTSCD = 0; if (r[5] != DBNull.Value) DucTSCD = Convert.ToDecimal(r[5]); tongDuc += DucTSCD; tongDucTSCD += DucTSCD;
                    decimal CaiTSCD = 0; if (r[6] != DBNull.Value) CaiTSCD = Convert.ToDecimal(r[6]); tongCai += CaiTSCD; tongCaiTSCD += CaiTSCD;
                    if (CaiTSCD != 0) tyleTSCD = DucTSCD / CaiTSCD * 100;
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(DucTSCD, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(CaiTSCD, 0) + "</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(tyleTSCD, 2) + "</td>");

                    decimal tyleNM = 0;
                    decimal DucNM = 0; if (r[7] != DBNull.Value) DucNM = Convert.ToDecimal(r[7]); tongDuc += DucNM; tongDucNM += DucNM;
                    decimal CaiNM = 0; if (r[8] != DBNull.Value) CaiNM = Convert.ToDecimal(r[8]); tongCai += CaiNM; tongCaiNM += CaiNM;
                    if (CaiNM != 0) tyleNM = DucNM / CaiNM * 100;
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(DucNM, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(CaiNM, 0) + "</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(tyleNM, 2) + "</td>");

                    decimal tyleTong = 0;
                    if (tongCai != 0) tyleTong = tongDuc / tongCai * 100;
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongDuc, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongCai, 0) + "</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(tyleTong, 2) + "</td></tr>");
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td style='font-weight:bold;'>T.C</td>");
                if (tongCaiSS1 != 0) tyletongSS1 = tongDucSS1 / tongCaiSS1 * 100;
                if (tongCaiSS2 != 0) tyletongSS2 = tongDucSS2 / tongCaiSS2 * 100;
                if (tongCaiTSCD != 0) tyletongTSCD = tongDucTSCD / tongCaiTSCD * 100;
                if (tongCaiNM != 0) tyletongNM = tongDucNM / tongCaiNM * 100;
                totalDuc = tongDucSS1 + tongDucSS2 + tongDucTSCD + tongDucNM;
                totalCai = tongCaiSS1 + tongCaiSS2 + tongCaiTSCD + tongCaiNM;
                if (totalCai != 0) tyleTotal = totalDuc / totalCai * 100;
                sb.Append(@"<td style='text-align:right;'>" + Config.ToXVal2(tongDucSS1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongCaiSS1, 0) + "</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(tyletongSS1, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongDucSS2, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongCaiSS2, 0) + "</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(tyletongSS2, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongDucTSCD, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongCaiTSCD, 0) + "</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(tyletongTSCD, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongDucNM, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongCaiNM, 0) + "</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(tyletongNM, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(totalDuc, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(totalCai, 0) + "</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(tyleTotal, 2) + "</td></tr>");
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
                string strSQL = "QLCS_BCTK_TyLeDucCaiSinhSan";
                SqlParameter[] param = new SqlParameter[1];
                if (txtDate.Text == "")
                {
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@d", txtDate.Text);
                string tieude = "<b>BẢNG THỐNG KÊ TỶ LỆ ĐỰC/CÁI ĐÀN CÁ SẤU SINH SẢN ĐẾN TRƯỚC NGÀY " + txtDate.Text + "</b>";

                DataTable dt = Config.SelectSP(strSQL, param);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th>CHUỒNG</th>
                          <th colspan='3'>SINH SẢN 1</th>
                          <th colspan='3'>SINH SẢN 2</th>
                          <th colspan='3'>TSCĐ</th>
                          <th colspan='3'>NƯỚC MẶN</th>
                          <th colspan='3'>TỔNG CỘNG</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'><th></th><th>Đực</th><th>Cái</th><th>Tỷ lệ<br/>Đực/cái (%)</th><th>Đực</th><th>Cái</th><th>Tỷ lệ<br/>Đực/cái (%)</th><th>Đực</th><th>Cái</th><th>Tỷ lệ<br/>Đực/cái (%)</th><th>Đực</th><th>Cái</th><th>Tỷ lệ<br/>Đực/cái (%)</th><th>Đực</th><th>Cái</th><th>Tỷ lệ<br/>Đực/cái (%)</th></tr></thead><tbody>"
                    );
                decimal tyletongSS1 = 0;
                decimal tongDucSS1 = 0;
                decimal tongCaiSS1 = 0;
                decimal tyletongSS2 = 0;
                decimal tongDucSS2 = 0;
                decimal tongCaiSS2 = 0;
                decimal tyletongTSCD = 0;
                decimal tongDucTSCD = 0;
                decimal tongCaiTSCD = 0;
                decimal tyletongNM = 0;
                decimal tongDucNM = 0;
                decimal tongCaiNM = 0;
                decimal tyleTotal = 0;
                decimal totalDuc = 0;
                decimal totalCai = 0;

                foreach (DataRow r in dt.Rows)
                {
                    decimal tongDuc = 0;
                    decimal tongCai = 0;
                    sb.Append("<tr><td style='text-align:left;'>" + r["TenChuong"].ToString() + "</td>");

                    decimal tyleSS1 = 0;
                    decimal DucSS1 = 0; if (r[1] != DBNull.Value) DucSS1 = Convert.ToDecimal(r[1]); tongDuc += DucSS1; tongDucSS1 += DucSS1;
                    decimal CaiSS1 = 0; if (r[2] != DBNull.Value) CaiSS1 = Convert.ToDecimal(r[2]); tongCai += CaiSS1; tongCaiSS1 += CaiSS1;
                    if (CaiSS1 != 0) tyleSS1 = DucSS1 / CaiSS1 * 100;
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(DucSS1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(CaiSS1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tyleSS1, 2) + "</td>");

                    decimal tyleSS2 = 0;
                    decimal DucSS2 = 0; if (r[3] != DBNull.Value) DucSS2 = Convert.ToDecimal(r[3]); tongDuc += DucSS2; tongDucSS2 += DucSS2;
                    decimal CaiSS2 = 0; if (r[4] != DBNull.Value) CaiSS2 = Convert.ToDecimal(r[4]); tongCai += CaiSS2; tongCaiSS2 += CaiSS2;
                    if (CaiSS2 != 0) tyleSS2 = DucSS2 / CaiSS2 * 100;
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(DucSS2, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(CaiSS2, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tyleSS2, 2) + "</td>");

                    decimal tyleTSCD = 0;
                    decimal DucTSCD = 0; if (r[5] != DBNull.Value) DucTSCD = Convert.ToDecimal(r[5]); tongDuc += DucTSCD; tongDucTSCD += DucTSCD;
                    decimal CaiTSCD = 0; if (r[6] != DBNull.Value) CaiTSCD = Convert.ToDecimal(r[6]); tongCai += CaiTSCD; tongCaiTSCD += CaiTSCD;
                    if (CaiTSCD != 0) tyleTSCD = DucTSCD / CaiTSCD * 100;
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(DucTSCD, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(CaiTSCD, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tyleTSCD, 2) + "</td>");

                    decimal tyleNM = 0;
                    decimal DucNM = 0; if (r[7] != DBNull.Value) DucNM = Convert.ToDecimal(r[7]); tongDuc += DucNM; tongDucNM += DucNM;
                    decimal CaiNM = 0; if (r[8] != DBNull.Value) CaiNM = Convert.ToDecimal(r[8]); tongCai += CaiNM; tongCaiNM += CaiNM;
                    if (CaiNM != 0) tyleNM = DucNM / CaiNM * 100;
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(DucNM, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(CaiNM, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tyleNM, 2) + "</td>");

                    decimal tyleTong = 0;
                    if (tongCai != 0) tyleTong = tongDuc / tongCai * 100;
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongDuc, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongCai, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tyleTong, 2) + "</td></tr>");
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td style='font-weight:bold;'>T.C</td>");
                if (tongCaiSS1 != 0) tyletongSS1 = tongDucSS1 / tongCaiSS1 * 100;
                if (tongCaiSS2 != 0) tyletongSS2 = tongDucSS2 / tongCaiSS2 * 100;
                if (tongCaiTSCD != 0) tyletongTSCD = tongDucTSCD / tongCaiTSCD * 100;
                if (tongCaiNM != 0) tyletongNM = tongDucNM / tongCaiNM * 100;
                totalDuc = tongDucSS1 + tongDucSS2 + tongDucTSCD + tongDucNM;
                totalCai = tongCaiSS1 + tongCaiSS2 + tongCaiTSCD + tongCaiNM;
                if (totalCai != 0) tyleTotal = totalDuc / totalCai * 100;
                sb.Append(@"<td style='text-align:right;'>" + Config.ToXVal2(tongDucSS1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongCaiSS1, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tyletongSS1, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongDucSS2, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongCaiSS2, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tyletongSS2, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongDucTSCD, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongCaiTSCD, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tyletongTSCD, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongDucNM, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tongCaiNM, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tyletongNM, 2) + "</td><td style='text-align:right;'>" + Config.ToXVal2(totalDuc, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(totalCai, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tyleTotal, 2) + "</td></tr></tbody></table>");

                lt.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}