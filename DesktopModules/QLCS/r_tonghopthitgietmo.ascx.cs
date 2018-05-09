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
    public partial class r_tonghopthitgietmo : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        int scale = 0;
        private void BindControls()
        {
            DataTable dtLoaiCa = csCont.LoadLoaiCa(1);

            ddlLoaiCaNgay.DataSource = dtLoaiCa;
            ddlLoaiCaNgay.DataTextField = "TenLoaiCa";
            ddlLoaiCaNgay.DataValueField = "IDLoaiCa";
            ddlLoaiCaNgay.DataBind();
            ddlLoaiCaNgay.Items.Insert(0, new ListItem("", "0"));

            ddlLoaiCaThang.DataSource = dtLoaiCa;
            ddlLoaiCaThang.DataTextField = "TenLoaiCa";
            ddlLoaiCaThang.DataValueField = "IDLoaiCa";
            ddlLoaiCaThang.DataBind();
            ddlLoaiCaThang.Items.Insert(0, new ListItem("", "0"));

            ddlLoaiCaQuy.DataSource = dtLoaiCa;
            ddlLoaiCaQuy.DataTextField = "TenLoaiCa";
            ddlLoaiCaQuy.DataValueField = "IDLoaiCa";
            ddlLoaiCaQuy.DataBind();
            ddlLoaiCaQuy.Items.Insert(0, new ListItem("", "0"));

            txtFromDate.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            ddlFromMonth.SelectedValue = "1";
            txtFromYear.Text = DateTime.Now.Year.ToString();
            ddlToMonth.SelectedValue = DateTime.Now.Month.ToString();
            txtToYear.Text = DateTime.Now.Year.ToString();

            txtYear.Text = DateTime.Now.Year.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
            try
            {
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

        protected void btnExcelThang_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string filename = "TongHopThitGietMoTheoThang";
                string tieude = "";
                string strSQL = "QLCS_BCTK_GietMo_TongHopThit_thang_chinhxac";
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@dFrom", "1/" + ddlFromMonth.SelectedValue + "/" + txtFromYear.Text);

                int toMonth = DateTime.Now.Month;
                int toYear = DateTime.Now.Year;
                if (ddlToMonth.SelectedValue != "12")
                {
                    toMonth = int.Parse(ddlToMonth.SelectedValue) + 1;
                    toYear = int.Parse(txtToYear.Text);
                }
                else
                {
                    toMonth = 1;
                    toYear = int.Parse(txtToYear.Text) + 1;
                }
                param[1] = new SqlParameter("@dTo", "1/" + toMonth.ToString() + "/" + toYear.ToString());
                param[2] = new SqlParameter("@LoaiCa", int.Parse(ddlLoaiCaThang.SelectedValue));
                filename += ddlFromMonth.SelectedValue + "_" + txtFromYear.Text + "___" + toMonth.ToString() + "_" + toYear.ToString() + ".xls";
                tieude += "<b>BÁO CÁO TỔNG HỢP THỊT THU HỒI TỪ CÁ GIẾT MỔ</b>";
                //DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                ArrayList lstThang = new ArrayList();
                int firstColIndex = dt.Columns.IndexOf("TenVatTu");
                int lastColIndex = dt.Columns.IndexOf("LoaiCaH");
                decimal[] lstTong = new decimal[lastColIndex - firstColIndex - 1];
                decimal[] lstTotal = new decimal[lastColIndex - firstColIndex - 1];
                for (int i = firstColIndex + 1; i < lastColIndex; i++)
                { 
                    lstThang.Add(dt.Columns[i].ColumnName);
                    lstTong[i - firstColIndex - 1] = 0;
                    lstTotal[i - firstColIndex - 1] = 0;
                }
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table border='1'>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <td rowspan=2>Loại cá</td><td colspan='2' rowspan=2>Sản phẩm<br/>thu hồi</td>");
                foreach (string col in lstThang)
	            {
            	    sb.Append("<td colspan='3'>Tháng " + col + "</td>");
	            }
                sb.Append("</tr>");
                sb.Append("<tr>");
                for (int i = 0; i< lstThang.Count; i++)
	            {
            	    sb.Append("<td>Số lượng<br/>(kg)</td><td>TL/MH<br/>(%)</td><td>TL/H<br/>(%)</td>");
	            }
                sb.Append("</tr>");

                string TenLoaiCa = "";
                int currNumVatTu = 0;
                decimal val = 0;
                decimal Hval = 0;
                decimal Mval = 0;
                decimal Htyle = 0;
                decimal Mtyle = 0;
                int SoLoaiCa = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                { 
                    DataRow r = dt.Rows[i];
                    if(r["TenLoaiCa"].ToString() != TenLoaiCa)
                    {
                        if (TenLoaiCa != "")
                        {
                            //Viết dòng tổng
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td colspan='2'></td>");
                            int ti1 = 0;
                            foreach (decimal val1 in lstTong)
                            {
                                sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val1, scale) + "</td><td></td><td></td>");
                                lstTotal[ti1] = (decimal)(lstTotal[ti1]) + val1;
                                ti1++;
                            }
                            sb.Append("</tr>");
                            //Reset Tong
                            for (int j = 0; j < lstTong.Length; j++)
                            {
                                lstTong[j] = 0;
                            }
                        }
                        TenLoaiCa = r["TenLoaiCa"].ToString();
                        sb = sb.Replace("@r@", currNumVatTu.ToString());
                        currNumVatTu = 0;
                        sb.Append("<tr><td rowspan='@r@' style='vertical-align:middle;text-align:left;'>" + r["TenLoaiCa"].ToString() + "</td><td colspan='2' style='text-align:left;'>" + r["TenVatTu"].ToString() + "</td>");
                        int li1 = 0;
                        foreach (string col in lstThang)
                        {
                            if (r[col] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r[col]);
                            if (r["H" + col] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["H" + col]);
                            if (r["M" + col] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["M" + col]);
                            if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                            if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                            sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val, scale) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Mtyle, 2) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                            lstTong[li1] = (decimal)(lstTong[li1]) + val;
                            li1++;
                        }
                        sb.Append("</tr>");
                        SoLoaiCa++;
                    }
                    else
                    {
                        sb.Append("<tr><td colspan='2' style='text-align:left;'>" + r["TenVatTu"].ToString() + "</td>");
                        int li2 = 0;
                        foreach (string col in lstThang)
                        {
                            if (r[col] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r[col]);
                            if (r["H" + col] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["H" + col]);
                            if (r["M" + col] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["M" + col]);
                            if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                            if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                            sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val, scale) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Mtyle, 2) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                            lstTong[li2] = (decimal)(lstTong[li2]) + val;
                            li2++;
                        }
                        sb.Append("</tr>");
                    }
                    currNumVatTu++;
                }
                sb = sb.Replace("@r@", currNumVatTu.ToString());
                //Viết dòng tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td colspan='2'></td>");
                int ti2 = 0;
                foreach (decimal val2 in lstTong)
                {
                    sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val2, scale) + "</td><td></td><td></td>");
                    lstTotal[ti2] = (decimal)(lstTotal[ti2]) + val2;
                    ti2++;
                }
                sb.Append("</tr>");
                //Viet dong cuoi day du
                if (ddlLoaiCaThang.SelectedValue == "0" && SoLoaiCa > 1)
                {
                    bool DaCoDongDau = false;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow r = dt1.Rows[i];
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>Theo từng loại SPGM</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td colspan='2' align='left'>" + r["TenVatTu"].ToString() + "</td>");
                        foreach (string col in lstThang)
                        {
                            if (r[col] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r[col]);
                            if (r["H" + col] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["H" + col]);
                            if (r["M" + col] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["M" + col]);
                            if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                            if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                            sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val, scale) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Mtyle, 2) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                        }
                        sb.Append("</tr>");
                    }
                    //Viết dòng total
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>Tổng cộng</b></td><td colspan='2'></td>");
                    foreach (decimal val3 in lstTotal)
                    {
                        sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val3, scale) + "</td><td></td><td></td>");
                    }
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

        protected void btnViewThang_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string tieude = "";
                string strSQL = "QLCS_BCTK_GietMo_TongHopThit_thang_chinhxac";
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@dFrom", "1/" + ddlFromMonth.SelectedValue + "/" + txtFromYear.Text);

                int toMonth = DateTime.Now.Month;
                int toYear = DateTime.Now.Year;
                if (ddlToMonth.SelectedValue != "12")
                {
                    toMonth = int.Parse(ddlToMonth.SelectedValue) + 1;
                    toYear = int.Parse(txtToYear.Text);
                }
                else
                {
                    toMonth = 1;
                    toYear = int.Parse(txtToYear.Text) + 1;
                }
                param[1] = new SqlParameter("@dTo", "1/" + toMonth.ToString() + "/" + toYear.ToString());
                param[2] = new SqlParameter("@LoaiCa", int.Parse(ddlLoaiCaThang.SelectedValue));
                tieude += "<b>BÁO CÁO TỔNG HỢP THỊT THU HỒI TỪ CÁ GIẾT MỔ - LOẠI CÁ " + ddlLoaiCaThang.SelectedItem.Text + "</b>";
                //DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                ArrayList lstThang = new ArrayList();
                int firstColIndex = dt.Columns.IndexOf("TenVatTu");
                int lastColIndex = dt.Columns.IndexOf("LoaiCaH");
                decimal[] lstTong = new decimal[lastColIndex - firstColIndex - 1];
                decimal[] lstTotal = new decimal[lastColIndex - firstColIndex - 1];
                for (int i = firstColIndex + 1; i < lastColIndex; i++)
                {
                    lstThang.Add(dt.Columns[i].ColumnName);
                    lstTong[i - firstColIndex - 1] = 0;
                    lstTotal[i - firstColIndex - 1] = 0;
                }
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <th rowspan=2>Loại cá</th><th rowspan=2>Sản phẩm<br/>thu hồi</th>");
                foreach (string col in lstThang)
                {
                    sb.Append("<th colspan='3'>Tháng " + col + "</th>");
                }
                sb.Append("</tr>");
                sb.Append("<tr>");
                for (int i = 0; i < lstThang.Count; i++)
                {
                    sb.Append("<th>Số lượng<br/>(kg)</th><th>TL/MH<br/>(%)</th><th>TL/H<br/>(%)</th>");
                }
                sb.Append("</tr></thead><tbody>");

                string TenLoaiCa = "";
                decimal val = 0;
                decimal Hval = 0;
                decimal Mval = 0;
                decimal Htyle = 0;
                decimal Mtyle = 0;
                int SoLoaiCa = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (r["TenLoaiCa"].ToString() != TenLoaiCa)
                    {
                        if (TenLoaiCa != "")
                        {
                            //Viết dòng tổng
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td>");
                            int ti1 = 0;
                            foreach (decimal val1 in lstTong)
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(val1, scale) + "</td><td></td><td></td>");
                                lstTotal[ti1] = (decimal)(lstTotal[ti1]) + val1;
                                ti1++;
                            }
                            sb.Append("</tr>");
                            //Reset Tong
                            for (int j = 0; j < lstTong.Length; j++)
                            {
                                lstTong[j] = 0;
                            }
                        }
                        TenLoaiCa = r["TenLoaiCa"].ToString();
                        sb.Append("<tr><td style='vertical-align:middle;'>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["TenVatTu"].ToString() + "</td>");
                        int li1 = 0;
                        foreach (string col in lstThang)
                        {
                            if (r[col] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r[col]);
                            if (r["H" + col] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["H" + col]);
                            if (r["M" + col] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["M" + col]);
                            if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                            if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                            sb.Append("<td align='right'>" + Config.ToXVal2(val, scale) + "</td><td align='right'>" + Config.ToXVal2(Mtyle, 2) + "</td><td align='right'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                            lstTong[li1] = (decimal)(lstTong[li1]) + val;
                            li1++;
                        }
                        sb.Append("</tr>");
                        SoLoaiCa++;
                    }
                    else
                    {
                        sb.Append("<tr><td></td><td>" + r["TenVatTu"].ToString() + "</td>");
                        int li2 = 0;
                        foreach (string col in lstThang)
                        {
                            if (r[col] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r[col]);
                            if (r["H" + col] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["H" + col]);
                            if (r["M" + col] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["M" + col]);
                            if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                            if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                            sb.Append("<td align='right'>" + Config.ToXVal2(val, scale) + "</td><td align='right'>" + Config.ToXVal2(Mtyle, 2) + "</td><td align='right'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                            lstTong[li2] = (decimal)(lstTong[li2]) + val;
                            li2++;
                        }
                        sb.Append("</tr>");
                    }
                }
                //Viết dòng tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td>");
                int ti2 = 0;
                foreach (decimal val2 in lstTong)
                {
                    sb.Append("<td align='right'>" + Config.ToXVal2(val2, scale) + "</td><td></td><td></td>");
                    lstTotal[ti2] = (decimal)(lstTotal[ti2]) + val2;
                    ti2++;
                }
                sb.Append("</tr>");
                //Viet dong cuoi day du
                if (ddlLoaiCaThang.SelectedValue == "0" && SoLoaiCa > 1)
                {
                    bool DaCoDongDau = false;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow r = dt1.Rows[i];
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>Theo từng loại SPGM</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td align='left'>" + r["TenVatTu"].ToString() + "</td>");
                        foreach (string col in lstThang)
                        {
                            if (r[col] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r[col]);
                            if (r["H" + col] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["H" + col]);
                            if (r["M" + col] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["M" + col]);
                            if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                            if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                            sb.Append(@"<td align='right'>" + Config.ToXVal2(val, scale) + @"</td><td align='right'>" + Config.ToXVal2(Mtyle, 2) + @"</td><td align='right'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                        }
                        sb.Append("</tr>");
                    }
                    //Viết dòng total
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>Tổng cộng</td><td></td>");
                    foreach (decimal val3 in lstTotal)
                    {
                        sb.Append("<td align='right'>" + Config.ToXVal2(val3, scale) + "</td><td></td><td></td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table>");
                ltNgay.Text = "";
                ltThang.Text = sb.ToString();
                ltQuy.Text = "";
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnExcelQuy_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string filename = "TongHopThitGietMoTheoQuy" + txtYear.Text + ".xls";
                string tieude = "";
                string strSQL = "QLCS_BCTK_GietMo_TongHopThit_quy_chinhxac";
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@year", int.Parse(txtYear.Text));
                param[1] = new SqlParameter("@LoaiCa", int.Parse(ddlLoaiCaQuy.SelectedValue));
                tieude += "<b>BÁO CÁO TỔNG HỢP THỊT THU HỒI TỪ CÁ GIẾT MỔ</b>";
                //DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table border='1'>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><td rowspan=2>Loại cá</td><td rowspan=2>Sản phẩm<br/>thu hồi</td>");
                sb.Append("<td colspan='3'>Quý 1</td><td colspan='3'>Quý 2</td><td colspan='3'>Quý 3</td><td colspan='3'>Quý 4</td></tr>");
                sb.Append("<tr><td>Số lượng<br/>(kg)</td><td>TL/MH<br/>(%)</td><td>TL/H<br/>(%)</td><td>Số lượng<br/>(kg)</td><td>TL/MH<br/>(%)</td><td>TL/H<br/>(%)</td><td>Số lượng<br/>(kg)</td><td>TL/MH<br/>(%)</td><td>TL/H<br/>(%)</td><td>Số lượng<br/>(kg)</td><td>TL/MH<br/>(%)</td><td>TL/H<br/>(%)</td></tr>");
                decimal[] lstTong = new decimal[4];
                decimal[] lstTotal = new decimal[4];
                lstTong[0] = lstTong[1] = lstTong[2] = lstTong[3] = 0;
                lstTotal[0] = lstTotal[1] = lstTotal[2] = lstTotal[3] = 0;
                string TenLoaiCa = "";
                int currNumVatTu = 0;
                decimal val = 0;
                decimal Hval = 0;
                decimal Mval = 0;
                decimal Htyle = 0;
                decimal Mtyle = 0;
                int SoLoaiCa = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (r["TenLoaiCa"].ToString() != TenLoaiCa)
                    {
                        if (TenLoaiCa != "")
                        {
                            //Viết dòng tổng
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td>");
                            int ti1 = 0;
                            foreach (decimal val1 in lstTong)
                            {
                                sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val1, scale) + "</td><td></td><td></td>");
                                lstTotal[ti1] = (decimal)(lstTotal[ti1]) + val1;
                                ti1++;
                            }
                            sb.Append("</tr>");
                            //Reset Tong
                            for (int j = 0; j < lstTong.Length; j++)
                            {
                                lstTong[j] = 0;
                            }
                        }
                        TenLoaiCa = r["TenLoaiCa"].ToString();
                        sb = sb.Replace("@r@", currNumVatTu.ToString());
                        currNumVatTu = 0;
                        sb.Append("<tr><td rowspan='@r@' style='vertical-align:middle;text-align:left;'>" + r["TenLoaiCa"].ToString() + "</td><td style='text-align:left;'>" + r["TenVatTu"].ToString() + "</td>");
                        int li1 = 0;
                        for(int j = 1; j < 5; j++)
                        {
                            string col = "Q" + j.ToString();
                            if (r[col] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r[col]);
                            if (r["H" + col] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["H" + col]);
                            if (r["M" + col] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["M" + col]);
                            if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                            if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                            sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val, scale) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Mtyle, 2) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                            lstTong[li1] = (decimal)(lstTong[li1]) + val;
                            li1++;
                        }
                        sb.Append("</tr>");
                        SoLoaiCa++;
                    }
                    else
                    {
                        sb.Append("<tr><td style='text-align:left;'>" + r["TenVatTu"].ToString() + "</td>");
                        int li2 = 0;
                        for (int j = 1; j < 5; j++)
                        {
                            string col = "Q" + j.ToString();
                            if (r[col] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r[col]);
                            if (r["H" + col] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["H" + col]);
                            if (r["M" + col] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["M" + col]);
                            if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                            if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                            sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val, scale) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Mtyle, 2) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                            lstTong[li2] = (decimal)(lstTong[li2]) + val;
                            li2++;
                        }
                        sb.Append("</tr>");
                    }
                    currNumVatTu++;
                }
                sb = sb.Replace("@r@", currNumVatTu.ToString());
                //Viết dòng tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td>");
                int ti2 = 0;
                foreach (decimal val2 in lstTong)
                {
                    sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val2, scale) + "</td><td></td><td></td>");
                    lstTotal[ti2] = (decimal)(lstTotal[ti2]) + val2;
                    ti2++;
                }
                sb.Append("</tr>");
                //Viet dong cuoi day du
                if (ddlLoaiCaQuy.SelectedValue == "0" && SoLoaiCa > 1)
                {
                    bool DaCoDongDau = false;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow r = dt1.Rows[i];
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>Theo từng loại SPGM</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td align='left'>" + r["TenVatTu"].ToString() + "</td>");
                        for (int j = 1; j < 5; j++)
                        {
                            string col = "Q" + j.ToString();
                            if (r[col] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r[col]);
                            if (r["H" + col] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["H" + col]);
                            if (r["M" + col] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["M" + col]);
                            if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                            if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                            sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val, scale) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Mtyle, 2) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                        }
                        sb.Append("</tr>");
                    }
                    //Viết dòng total
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>Tổng cộng</b></td><td></td>");
                    foreach (decimal val3 in lstTotal)
                    {
                        sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val3, scale) + "</td><td></td><td></td>");
                    }
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

        protected void btnViewQuy_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string tieude = "";
                string strSQL = "QLCS_BCTK_GietMo_TongHopThit_quy_chinhxac";
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@year", int.Parse(txtYear.Text));
                param[1] = new SqlParameter("@LoaiCa", int.Parse(ddlLoaiCaQuy.SelectedValue));
                tieude += "<b>BÁO CÁO TỔNG HỢP THỊT THU HỒI TỪ CÁ GIẾT MỔ - LOẠI CÁ " + ddlLoaiCaQuy.SelectedItem.Text + "</b>";
                //DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><th rowspan=2>Loại cá</th><th rowspan=2>Sản phẩm<br/>thu hồi</th>");
                sb.Append("<th colspan='3'>Quý 1</th><th colspan='3'>Quý 2</th><th colspan='3'>Quý 3</th><th colspan='3'>Quý 4</th></tr>");
                sb.Append("<tr><th>Số lượng<br/>(kg)</th><th>TL/MH<br/>(%)</th><th>TL/H<br/>(%)</th><th>Số lượng<br/>(kg)</th><th>TL/MH<br/>(%)</th><th>TL/H<br/>(%)</th><th>Số lượng<br/>(kg)</th><th>TL/MH<br/>(%)</th><th>TL/H<br/>(%)</th><th>Số lượng<br/>(kg)</th><th>TL/MH<br/>(%)</th><th>TL/H<br/>(%)</th></tr></thead><tbody>");
                decimal[] lstTong = new decimal[4];
                decimal[] lstTotal = new decimal[4];
                lstTong[0] = lstTong[1] = lstTong[2] = lstTong[3] = 0;
                lstTotal[0] = lstTotal[1] = lstTotal[2] = lstTotal[3] = 0;
                string TenLoaiCa = "";
                decimal val = 0;
                decimal Hval = 0;
                decimal Mval = 0;
                decimal Htyle = 0;
                decimal Mtyle = 0;
                int SoLoaiCa = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (r["TenLoaiCa"].ToString() != TenLoaiCa)
                    {
                        if (TenLoaiCa != "")
                        {
                            //Viết dòng tổng
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td>");
                            int ti1 = 0;
                            foreach (decimal val1 in lstTong)
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(val1, scale) + "</td><td></td><td></td>");
                                lstTotal[ti1] = (decimal)(lstTotal[ti1]) + val1;
                                ti1++;
                            }
                            sb.Append("</tr>");
                            //Reset Tong
                            for (int j = 0; j < lstTong.Length; j++)
                            {
                                lstTong[j] = 0;
                            }
                        }
                        TenLoaiCa = r["TenLoaiCa"].ToString();
                        sb.Append("<tr><td style='vertical-align:middle;'>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["TenVatTu"].ToString() + "</td>");
                        int li1 = 0;
                        for (int j = 1; j < 5; j++)
                        {
                            string col = "Q" + j.ToString();
                            if (r[col] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r[col]);
                            if (r["H" + col] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["H" + col]);
                            if (r["M" + col] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["M" + col]);
                            if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                            if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                            sb.Append("<td align='right'>" + Config.ToXVal2(val, scale) + "</td><td align='right'>" + Config.ToXVal2(Mtyle, 2) + "</td><td align='right'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                            lstTong[li1] = (decimal)(lstTong[li1]) + val;
                            li1++;
                        }
                        sb.Append("</tr>");
                        SoLoaiCa++;
                    }
                    else
                    {
                        sb.Append("<tr><td></td><td>" + r["TenVatTu"].ToString() + "</td>");
                        int li2 = 0;
                        for (int j = 1; j < 5; j++)
                        {
                            string col = "Q" + j.ToString();
                            if (r[col] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r[col]);
                            if (r["H" + col] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["H" + col]);
                            if (r["M" + col] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["M" + col]);
                            if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                            if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                            sb.Append("<td align='right'>" + Config.ToXVal2(val, scale) + "</td><td align='right'>" + Config.ToXVal2(Mtyle, 2) + "</td><td align='right'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                            lstTong[li2] = (decimal)(lstTong[li2]) + val;
                            li2++;
                        }
                        sb.Append("</tr>");
                    }
                }
                //Viết dòng tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td>");
                int ti2 = 0;
                foreach (decimal val2 in lstTong)
                {
                    sb.Append("<td align='right'>" + Config.ToXVal2(val2, scale) + "</td><td></td><td></td>");
                    lstTotal[ti2] = (decimal)(lstTotal[ti2]) + val2;
                    ti2++;
                }
                sb.Append("</tr>");
                //Viet dong cuoi day du
                if (ddlLoaiCaQuy.SelectedValue == "0" && SoLoaiCa > 1)
                {
                    bool DaCoDongDau = false;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow r = dt1.Rows[i];
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>Theo từng loại SPGM</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td align='left'>" + r["TenVatTu"].ToString() + "</td>");
                        for (int j = 1; j < 5; j++)
                        {
                            string col = "Q" + j.ToString();
                            if (r[col] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r[col]);
                            if (r["H" + col] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["H" + col]);
                            if (r["M" + col] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["M" + col]);
                            if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                            if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                            sb.Append(@"<td align='right'>" + Config.ToXVal2(val, scale) + @"</td><td align='right'>" + Config.ToXVal2(Mtyle, 2) + @"</td><td align='right'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                        }
                        sb.Append("</tr>");
                    }
                    //Viết dòng total
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>Tổng cộng</b></td><td></td>");
                    foreach (decimal val3 in lstTotal)
                    {
                        sb.Append("<td align='right'>" + Config.ToXVal2(val3, scale) + "</td><td></td><td></td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table>");
                ltNgay.Text = "";
                ltThang.Text = "";
                ltQuy.Text = sb.ToString();
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnExcelNgay_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string filename = "TongHopThitGietMoTheoNgay.xls";
                string tieude = "";
                string strSQL = "QLCS_BCTK_GietMo_TongHopThit_ngay_chinhxac";
                SqlParameter[] param = new SqlParameter[3];
                if (txtFromDate.Text == "") txtFromDate.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
                if (txtToDate.Text == "") txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                param[2] = new SqlParameter("@LoaiCa", int.Parse(ddlLoaiCaNgay.SelectedValue));
                tieude += "<b>BÁO CÁO TỔNG HỢP THỊT THU HỒI TỪ CÁ GIẾT MỔ</b>";
                //DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table border='1'>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><td rowspan=2>Loại cá</td><td rowspan=2>Sản phẩm<br/>thu hồi</td>");
                sb.Append("<td colspan='3'>Từ ngày " + txtFromDate.Text + " đến ngày " + txtToDate.Text + "</td></tr>");
                sb.Append("<tr><td>Số lượng<br/>(kg)</td><td>TL/MH<br/>(%)</td><td>TL/H<br/>(%)</td></tr>");
                decimal Tong = 0;
                decimal Total = 0;
                string TenLoaiCa = "";
                int currNumVatTu = 0;
                decimal val = 0;
                decimal Hval = 0;
                decimal Mval = 0;
                decimal Htyle = 0;
                decimal Mtyle = 0;
                int SoLoaiCa = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (r["TenLoaiCa"].ToString() != TenLoaiCa)
                    {
                        if (TenLoaiCa != "")
                        {
                            //Viết dòng tổng
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(Tong, scale) + "</td><td></td><td></td>");
                            Total += Tong;
                            sb.Append("</tr>");
                            //Reset Tong
                            Tong = 0;
                        }
                        TenLoaiCa = r["TenLoaiCa"].ToString();
                        sb = sb.Replace("@r@", currNumVatTu.ToString());
                        currNumVatTu = 0;
                        sb.Append("<tr><td rowspan='@r@' style='vertical-align:middle;text-align:left;'>" + r["TenLoaiCa"].ToString() + "</td><td style='text-align:left;'>" + r["TenVatTu"].ToString() + "</td>");

                        if (r["KhoiLuong"] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r["KhoiLuong"]);
                        if (r["TrongLuongHoi"] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["TrongLuongHoi"]);
                        if (r["TrongLuongMocHam"] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["TrongLuongMocHam"]);
                        if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                        if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                        sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val, scale) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Mtyle, 2) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                        Tong += val;

                        sb.Append("</tr>");
                        SoLoaiCa++;
                    }
                    else
                    {
                        sb.Append("<tr><td style='text-align:left;'>" + r["TenVatTu"].ToString() + "</td>");
                        if (r["KhoiLuong"] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r["KhoiLuong"]);
                        if (r["TrongLuongHoi"] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["TrongLuongHoi"]);
                        if (r["TrongLuongMocHam"] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["TrongLuongMocHam"]);
                        if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                        if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                        sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val, scale) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Mtyle, 2) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                        Tong += val;
                        sb.Append("</tr>");
                    }
                    currNumVatTu++;
                }
                sb = sb.Replace("@r@", currNumVatTu.ToString());
                //Viết dòng tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td>");
                sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(Tong, scale) + "</td><td></td><td></td>");
                Total += Tong;
                sb.Append("</tr>");
                //Viet dong cuoi day du
                if (ddlLoaiCaQuy.SelectedValue == "0" && SoLoaiCa > 1)
                {
                    bool DaCoDongDau = false;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow r = dt1.Rows[i];
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>Theo từng loại SPGM</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td align='left'>" + r["TenVatTu"].ToString() + "</td>");
                        if (r["KhoiLuong"] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r["KhoiLuong"]);
                        if (r["TrongLuongHoi"] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["TrongLuongHoi"]);
                        if (r["TrongLuongMocHam"] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["TrongLuongMocHam"]);
                        if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                        if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                        sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(val, scale) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Mtyle, 2) + @"</td><td align='right' style='mso-number-format:" + Config.ExcelFormat(2) + ";'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                        sb.Append("</tr>");
                    }
                    //Viết dòng total
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>Tổng cộng</b></td><td></td>");
                    sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(Total, scale) + "</td><td></td><td></td>");
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

        protected void btnViewNgay_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string tieude = "";
                string strSQL = "QLCS_BCTK_GietMo_TongHopThit_ngay_chinhxac";
                SqlParameter[] param = new SqlParameter[3];
                if (txtFromDate.Text == "") txtFromDate.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
                if (txtToDate.Text == "") txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                param[2] = new SqlParameter("@LoaiCa", int.Parse(ddlLoaiCaNgay.SelectedValue));
                tieude += "<b>BÁO CÁO TỔNG HỢP THỊT THU HỒI TỪ CÁ GIẾT MỔ - LOẠI CÁ " + ddlLoaiCaQuy.SelectedItem.Text + "</b>";
                //DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><th rowspan=2>Loại cá</th><th rowspan=2>Sản phẩm<br/>thu hồi</th>");
                sb.Append("<th colspan='3'>Từ ngày " + txtFromDate.Text + " đến ngày " + txtToDate.Text + "</th></tr>");
                sb.Append("<tr><th>Số lượng<br/>(kg)</th><th>TL/MH<br/>(%)</th><th>TL/H<br/>(%)</th></tr></thead><tbody>");
                decimal Tong = 0;
                decimal Total = 0;
                string TenLoaiCa = "";
                decimal val = 0;
                decimal Hval = 0;
                decimal Mval = 0;
                decimal Htyle = 0;
                decimal Mtyle = 0;
                int SoLoaiCa = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (r["TenLoaiCa"].ToString() != TenLoaiCa)
                    {
                        if (TenLoaiCa != "")
                        {
                            //Viết dòng tổng
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(Tong, scale) + "</td><td></td><td></td>");
                            Total += Tong;
                            sb.Append("</tr>");
                            //Reset Tong
                            Tong = 0;
                        }
                        TenLoaiCa = r["TenLoaiCa"].ToString();
                        sb.Append("<tr><td style='vertical-align:middle;'>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["TenVatTu"].ToString() + "</td>");
                        int li1 = 0;
                        
                        if (r["KhoiLuong"] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r["KhoiLuong"]);
                        if (r["TrongLuongHoi"] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["TrongLuongHoi"]);
                        if (r["TrongLuongMocHam"] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["TrongLuongMocHam"]);
                        if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                        if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                        sb.Append("<td align='right'>" + Config.ToXVal2(val, scale) + "</td><td align='right'>" + Config.ToXVal2(Mtyle, 2) + "</td><td align='right'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                        Tong += val;
                        
                        sb.Append("</tr>");
                        SoLoaiCa++;
                    }
                    else
                    {
                        sb.Append("<tr><td></td><td>" + r["TenVatTu"].ToString() + "</td>");

                        if (r["KhoiLuong"] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r["KhoiLuong"]);
                        if (r["TrongLuongHoi"] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["TrongLuongHoi"]);
                        if (r["TrongLuongMocHam"] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["TrongLuongMocHam"]);
                        if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                        if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                        sb.Append("<td align='right'>" + Config.ToXVal2(val, scale) + "</td><td align='right'>" + Config.ToXVal2(Mtyle, 2) + "</td><td align='right'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                        Tong += val;
                        
                        sb.Append("</tr>");
                    }
                }
                //Viết dòng tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td>");
                sb.Append("<td align='right'>" + Config.ToXVal2(Tong, scale) + "</td><td></td><td></td>");
                Total += Tong;
                sb.Append("</tr>");
                //Viet dong cuoi day du
                if (ddlLoaiCaNgay.SelectedValue == "0" && SoLoaiCa > 1)
                {
                    bool DaCoDongDau = false;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow r = dt1.Rows[i];
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>Theo từng loại SPGM</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td align='left'>" + r["TenVatTu"].ToString() + "</td>");
                        if (r["KhoiLuong"] == DBNull.Value) val = 0; else val = Convert.ToDecimal(r["KhoiLuong"]);
                        if (r["TrongLuongHoi"] == DBNull.Value) Hval = 0; else Hval = Convert.ToDecimal(r["TrongLuongHoi"]);
                        if (r["TrongLuongMocHam"] == DBNull.Value) Mval = 0; else Mval = Convert.ToDecimal(r["TrongLuongMocHam"]);
                        if (Hval == 0) Htyle = 0; else Htyle = val / Hval * 100;
                        if (Mval == 0) Mtyle = 0; else Mtyle = val / Mval * 100;
                        sb.Append(@"<td align='right'>" + Config.ToXVal2(val, scale) + @"</td><td align='right'>" + Config.ToXVal2(Mtyle, 2) + @"</td><td align='right'>" + Config.ToXVal2(Htyle, 2) + "</td>");
                        sb.Append("</tr>");
                    }
                    //Viết dòng total
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>Tổng cộng</b></td><td></td>");
                    sb.Append("<td align='right'>" + Config.ToXVal2(Total, scale) + "</td><td></td><td></td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table>");
                ltNgay.Text = sb.ToString();
                ltThang.Text = "";
                ltQuy.Text = "";
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}