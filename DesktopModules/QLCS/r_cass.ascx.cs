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
    public partial class r_cass : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        private void BindControls()
        {
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
                string filename = "ThongKeTyLeDucCaiSinhSan";
                filename += txtDate.Text + ".xls";
                filename = filename.Replace("/", "_");

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string tieude = "";
                tieude += "<b>THỐNG KÊ CÁ SINH SẢN NGÀY " + txtDate.Text + "</b>";

                string strSQL = "QLCS_BCTK_BienDongDan_CaSS";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@date", DateTime.Parse(txtDate.Text, ci));
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dtLoaiCa = ds.Tables[1];
                DataTable dtTong = ds.Tables[2];
                Dictionary<int, string> dicLoaiCa = new Dictionary<int, string>();
                foreach (DataRow rLoaiCa in dtLoaiCa.Rows)
                {
                    dicLoaiCa.Add(int.Parse(rLoaiCa["IDLoaiCa"].ToString()), rLoaiCa["TenLoaiCa"].ToString());
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");
                sb.Append("<table border='1'><tr><td colspan='21'><center style='font-weight:bold;font-size:14pt;'>" + tieude + "</center></td></tr><tr><td colspan='21'></td></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;text-align:center;'><td rowspan='2'>Ô chuồng</td>");
                foreach (DataRow rLoaiCa in dtLoaiCa.Rows)
                {
                    sb.Append(@"<td style='text-align:center;' colspan='4'>" + rLoaiCa["TenLoaiCa"].ToString() + "</td>");
                }
                sb.Append(@"<td colspan='4'>Tổng</td></tr><tr>");
                for (int i = 0; i < dtLoaiCa.Rows.Count; i++)
                {
                    sb.Append(@"<td style='text-align:center;'>Đực</td><td style='text-align:center;'>Cái</td><td style='text-align:center;'>Tỷ lệ</td><td style='text-align:center;'>Tổng</td>");
                }
                sb.Append(@"<td style='text-align:center;'>Đực</td><td style='text-align:center;'>Cái</td><td style='text-align:center;'>Tỷ lệ</td><td style='text-align:center;'>Tổng</td>");
                sb.Append(@"</tr>");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    sb.Append("<tr style='vertical-align:middle;'>");
                    sb.Append("<td>" + r["TenChuong"].ToString() + "</td>");
                    foreach (DataRow rLoaiCa in dtLoaiCa.Rows)
                    {
                        string sIDLoaiCa = rLoaiCa["IDLoaiCa"].ToString();
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r[sIDLoaiCa + "-1"], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r[sIDLoaiCa + "-0"], 0) + "</td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(r["TL" + sIDLoaiCa], 2) + "%</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["T" + sIDLoaiCa], 0) + "</td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["D"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["C"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(r["TL"], 2) + "%</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["T"], 0) + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>Tổng cộng</td>");
                foreach (DataRow rTong in dtTong.Rows)
                {
                    foreach (DataRow rLoaiCa in dtLoaiCa.Rows)
                    {
                        string sIDLoaiCa = rLoaiCa["IDLoaiCa"].ToString();
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong[sIDLoaiCa + "-1"], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong[sIDLoaiCa + "-0"], 0) + "</td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(rTong["TL" + sIDLoaiCa], 2) + "%</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong["T" + sIDLoaiCa], 0) + "</td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong["D"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong["C"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(rTong["TL"], 2) + "%</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong["T"], 0) + "</td>");
                    sb.Append("</tr>");
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
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string tieude = "";
                tieude += "<b>THỐNG KÊ CÁ SINH SẢN NGÀY " + txtDate.Text + "</b>";

                string strSQL = "QLCS_BCTK_BienDongDan_CaSS";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@date", DateTime.Parse(txtDate.Text, ci));
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dtLoaiCa = ds.Tables[1];
                DataTable dtTong = ds.Tables[2];
                Dictionary<int, string> dicLoaiCa = new Dictionary<int, string>();
                foreach (DataRow rLoaiCa in dtLoaiCa.Rows)
                {
                    dicLoaiCa.Add(int.Parse(rLoaiCa["IDLoaiCa"].ToString()), rLoaiCa["TenLoaiCa"].ToString());
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<div style='text-align:center;font-weight:bold;font-size:14pt;'>" + tieude + "</div><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;text-align:center;'><td rowspan='2'>Ô chuồng</td>");
                foreach (DataRow rLoaiCa in dtLoaiCa.Rows)
                {
                    sb.Append(@"<td style='text-align:center;' colspan='4'>" + rLoaiCa["TenLoaiCa"].ToString() + "</td>");
                }
                sb.Append(@"<td colspan='4'>Tổng</td></tr><tr>");
                for (int i = 0; i < dtLoaiCa.Rows.Count; i++)
                {
                    sb.Append(@"<td style='text-align:center;'>Đực</td><td style='text-align:center;'>Cái</td><td style='text-align:center;'>Tỷ lệ</td><td style='text-align:center;'>Tổng</td>");
                }
                sb.Append(@"<td style='text-align:center;'>Đực</td><td style='text-align:center;'>Cái</td><td style='text-align:center;'>Tỷ lệ</td><td style='text-align:center;'>Tổng</td>");
                sb.Append(@"</tr></thead><tbody>");
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    sb.Append("<tr style='vertical-align:middle;'>");
                    sb.Append("<td>" + r["TenChuong"].ToString() + "</td>");
                    foreach (DataRow rLoaiCa in dtLoaiCa.Rows)
                    {
                        string sIDLoaiCa = rLoaiCa["IDLoaiCa"].ToString();
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r[sIDLoaiCa + "-1"], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r[sIDLoaiCa + "-0"], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["TL" + sIDLoaiCa], 2) + "%</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["T" + sIDLoaiCa], 0) + "</td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["D"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["C"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["TL"], 2) + "%</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["T"], 0) + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>Tổng cộng</td>");
                foreach (DataRow rTong in dtTong.Rows)
                {
                    foreach (DataRow rLoaiCa in dtLoaiCa.Rows)
                    {
                        string sIDLoaiCa = rLoaiCa["IDLoaiCa"].ToString();
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong[sIDLoaiCa + "-1"], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong[sIDLoaiCa + "-0"], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong["TL" + sIDLoaiCa], 2) + "%</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong["T" + sIDLoaiCa], 0) + "</td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong["D"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong["C"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong["TL"], 2) + "%</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(rTong["T"], 0) + "</td>");
                    sb.Append("</tr>");
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