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
    public partial class r_casauantheophanloai : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable dtLoaiCa = csCont.LoadLoaiCa(1);
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

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string tieude = "CÁ ĂN THEO GIỐNG - TĂNG TRỌNG TỪ " + txtFromDate.Text + " ĐẾN " + txtToDate.Text;
                DataTable tbl = null;
                string strSQL = "QLCS_BCTK_CaSauAnTheoPhanLoai";
                SqlParameter[] param = new SqlParameter[3];
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                }
                param[1] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[2] = new SqlParameter("@dTo", txtToDate.Text);
                param[0] = new SqlParameter("@LoaiCa", 0);
                string strLoaiCa = Config.GetSelectedValues(ddlLoaiCa);
                if (strLoaiCa != "" && strLoaiCa != "0, ")
                {
                    string[] arrLoaiCa = strLoaiCa.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string i in arrLoaiCa)
                    {
                        param[0].Value = int.Parse(i);
                        DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        if (tbl == null) tbl = dt.Clone();
                        foreach (DataRow r in dt.Rows)
                        {
                            DataRow row = tbl.NewRow();
                            for (int j = 0; j < tbl.Columns.Count; j++)
                            {
                                if (j > 0) r[j] = Math.Round(Convert.ToDecimal(r[j], ci), 2);
                                row[j] = r[j];
                            }
                            tbl.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    DataTable dtLoaiCa = csCont.LoadLoaiCa(1);
                    foreach (DataRow rr in dtLoaiCa.Rows)
                    {
                        int i = Convert.ToInt32(rr["IDLoaiCa"]);
                        param[0].Value = i;
                        DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        if (tbl == null) tbl = dt.Clone();
                        foreach (DataRow r in dt.Rows)
                        {
                            DataRow row = tbl.NewRow();
                            for (int j = 0; j < tbl.Columns.Count; j++)
                            {
                                if (j > 0) r[j] = Math.Round(Convert.ToDecimal(r[j], ci), 2);
                                row[j] = r[j];
                            }
                            tbl.Rows.Add(row);
                        }
                    }
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<div style='text-align:center;font-weight:bold;font-size:14pt;'>" + tieude + "</div><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th>Loại cá</th>
                          <th>Tiêu tốn thức ăn trung bình (kg/con)</th>
                          <th>Giống (kg/con)</th>
                          <th>Tăng trọng (kg/con)</th>
                          </tr></thead><tbody>");
                foreach (DataRow r in tbl.Rows)
                {
                    sb.Append("<tr><td style='text-align:left;'>" + r["TenLoaiCa"].ToString() + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(r["TieuTonThucAnToanGiaiDoan"], 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(r["TieuTonThucAnToanGiaiDoanGiong"], 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(r["TieuTonThucAnToanGiaiDoanTT"], 2) + "</td></tr>");
                }
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
                string filename = "CaAnGiongTangTrong.xls";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                string tieude = "CÁ ĂN THEO GIỐNG - TĂNG TRỌNG TỪ " + txtFromDate.Text + " ĐẾN " + txtToDate.Text;
                DataTable tbl = null;
                string strSQL = "QLCS_BCTK_CaSauAnTheoPhanLoai";
                SqlParameter[] param = new SqlParameter[3];
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                }
                param[1] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[2] = new SqlParameter("@dTo", txtToDate.Text);
                param[0] = new SqlParameter("@LoaiCa", 0);
                string strLoaiCa = Config.GetSelectedValues(ddlLoaiCa);
                if (strLoaiCa != "" && strLoaiCa != "0, ")
                {
                    string[] arrLoaiCa = strLoaiCa.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string i in arrLoaiCa)
                    {
                        param[0].Value = int.Parse(i);
                        DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        if (tbl == null) tbl = dt.Clone();
                        foreach (DataRow r in dt.Rows)
                        {
                            DataRow row = tbl.NewRow();
                            for (int j = 0; j < tbl.Columns.Count; j++)
                            {
                                if (j > 0) r[j] = Math.Round(Convert.ToDecimal(r[j], ci), 2);
                                row[j] = r[j];
                            }
                            tbl.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    DataTable dtLoaiCa = csCont.LoadLoaiCa(1);
                    foreach (DataRow rr in dtLoaiCa.Rows)
                    {
                        int i = Convert.ToInt32(rr["IDLoaiCa"]);
                        param[0].Value = i;
                        DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        if (tbl == null) tbl = dt.Clone();
                        foreach (DataRow r in dt.Rows)
                        {
                            DataRow row = tbl.NewRow();
                            for (int j = 0; j < tbl.Columns.Count; j++)
                            {
                                if (j > 0) r[j] = Math.Round(Convert.ToDecimal(r[j], ci), 2);
                                row[j] = r[j];
                            }
                            tbl.Rows.Add(row);
                        }
                    }
                }
                
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");
                sb.Append("<table border='1'><tr><td valign='middle' colspan='6'><center style='font-weight:bold;font-size:14pt;'>" + tieude + "</center><br/></td></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <td colspan='3'>Loại cá</td>
                          <td>Tiêu tốn thức ăn trung bình (kg/con)</td>
                          <td>Giống (kg/con)</td>
                          <td>Tăng trọng (kg/con)</td>
                         </tr>");
                foreach (DataRow r in tbl.Rows)
                {
                    sb.Append("<tr><td colspan='3' style='text-align:left;'>" + r["TenLoaiCa"].ToString() + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(r["TieuTonThucAnToanGiaiDoan"], 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(r["TieuTonThucAnToanGiaiDoanGiong"], 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(r["TieuTonThucAnToanGiaiDoanTT"], 2) + "</td></tr>");
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
    }
}