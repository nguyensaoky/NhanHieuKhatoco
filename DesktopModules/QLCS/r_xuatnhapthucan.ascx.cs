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
    public partial class r_xuatnhapthucan : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        int scale = 0;
        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable tblTA = csCont.VatTu_GetTA_Order();
            ddlTA.DataSource = tblTA;
            ddlTA.DataValueField = "IDVatTu";
            ddlTA.DataTextField = "TenVatTu";
            ddlTA.DataBind();
            //ddlTA.Items.Insert(0, new ListItem("", "0"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
            try
            {
                scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_TA_Scale"]);
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
            if (ddlSapXep.SelectedValue == "Doc") ExcelDoc();
            else ExcelNgang();
        }

        protected void ExcelNgang()
        {
            try
            {
                string filename = "TA";
                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                filename = filename.Replace("/", "_");
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");

                if (ddlLoaiBC.SelectedValue == "TongHop")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT NHẬP THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_FilterThucAn";
                        param = new SqlParameter[5];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn";
                        param = new SqlParameter[3];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTA * 3];
                    ArrayList tong = new ArrayList();
                    ArrayList tong1 = new ArrayList();
                    for (int l = 0; l < countTA * 3; l++) tong.Add(new decimal(0));
                    for (int l1 = 0; l1 < countTA; l1++) tong1.Add(new decimal(0));
                    int tonIndex1 = countTA * 2;
                    foreach (DataRow r in tblTA.Rows)
                    {
                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        tong1[tonIndex1 - (countTA * 2)] = tong[tonIndex1];
                        tonIndex1++;
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            if (r[rTA["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rTA["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTA]) == 0 && ((decimal)tong[k + countTA + countTA]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTA + countTA);
                            tong.RemoveAt(k + countTA);
                            tong.RemoveAt(k);
                            tong1.RemoveAt(k);
                            countTA--;
                        }
                    }

                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Nhập (kg)</th>
                              <th colspan=" + countTA.ToString() + @">Xuất (kg)</th>
                              <th colspan=" + countTA.ToString() + @">Tồn (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (DataRow r in tblTA.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("</tr></thead><tbody>");
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>ĐK</td>");
                    for (int i = 0; i < countTA * 2; i++)
                    {
                        sb.Append("<td></td>");
                    }
                    //decimal[] tong = new decimal[countTA * 3];
                    //int tonIndex = countTA * 2;
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()],scale) + "</td>");
                        //tong[tonIndex] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        //tonIndex++;
                    }
                    sb.Append("<td></td></tr>");
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Nhap" + rTA["IDVatTu"].ToString()],scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Xuat" + rTA["IDVatTu"].ToString()],scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTA * 2;
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            if (r[rTA["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong1[j],scale) + "</td>");
                            }
                            else
                            {
                                sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r[rTA["IDVatTu"].ToString()],scale) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rTA["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA * 3; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[i],scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "Nhap")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA * 2; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTA]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTA);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }

                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Nhập (kg)</th>
                              <th colspan=" + countTA.ToString() + @">Nhập điều chỉnh (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    for (int i = 0; i < 2; i++)
                    {
                        foreach (DataRow r in tblTA.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Nhap" + rTA["IDVatTu"].ToString()],scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["NhapDC" + rTA["IDVatTu"].ToString()],scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA * 2; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[i],scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "NhapBT")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }

                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Nhập (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Nhap" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "NhapDC")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }

                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Nhập điều chỉnh (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["NhapDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "Xuat")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA * 3; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTA]) == 0 && ((decimal)tong[k + countTA + countTA]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTA + countTA);
                            tong.RemoveAt(k + countTA);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Xuất (kg)</th>
                              <th colspan=" + countTA.ToString() + @">Xuất điều chỉnh (kg)</th>
                              <th colspan=" + countTA.ToString() + @">Hủy (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (DataRow r in tblTA.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Xuat" + rTA["IDVatTu"].ToString()],scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatDC" + rTA["IDVatTu"].ToString()],scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatH" + rTA["IDVatTu"].ToString()],scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA * 3; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[i],scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatBT")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Xuất (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Xuat" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatDC")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Xuất điều chỉnh (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatH")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI HỦY THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Hủy (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatH" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }

                sb.Append(Config.Footer(UserInfo.Profile.City, UserInfo.FirstName + " " + UserInfo.LastName));
                sb.Append("</body></html>");
                Response.Write(sb.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void ExcelDoc()
        {
            try
            {
                string filename = "TA";
                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                filename = filename.Replace("/", "_");
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");

                if (ddlLoaiBC.SelectedValue == "TongHop")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT NHẬP THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_FilterThucAn";
                        param = new SqlParameter[5];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn";
                        param = new SqlParameter[3];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTA * 3];
                    ArrayList tong = new ArrayList();
                    ArrayList tong1 = new ArrayList();
                    for (int l = 0; l < countTA * 3; l++) tong.Add(new decimal(0));
                    for (int l1 = 0; l1 < countTA; l1++) tong1.Add(new decimal(0));
                    int tonIndex1 = countTA * 2;
                    foreach (DataRow r in tblTA.Rows)
                    {
                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        tong1[tonIndex1 - (countTA * 2)] = tong[tonIndex1];
                        tonIndex1++;
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            if (r[rTA["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rTA["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTA]) == 0 && ((decimal)tong[k + countTA + countTA]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTA + countTA);
                            tong.RemoveAt(k + countTA);
                            tong.RemoveAt(k);
                            tong1.RemoveAt(k);
                            countTA--;
                        }
                    }

                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                              <th>Nhập (kg)</th>
                              <th>Xuất (kg)</th>
                              <th>Tồn (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    bool DaCoDongDau = false;
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append("<td align='center'>ĐK</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td align='left'>" + r["TenVatTu"].ToString() + "</td><td></td><td></td>");
                        sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()], scale) + "</td>");
                        sb.Append("<td></td></tr>");
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co xuat nhap thi viet 1 dong
                            if (r["Nhap" + rTA["IDVatTu"].ToString()] == DBNull.Value && r["Xuat" + rTA["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                j++;
                                continue;
                            }
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Nhap" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Xuat" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            if (r[rTA["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong1[j], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r[rTA["IDVatTu"].ToString()], scale) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rTA["IDVatTu"].ToString()]);
                            }
                            j++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        DaCoDongDau = false;
                        DataRow rE = dt.Rows[dt.Rows.Count - 1];
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE + countTA], scale) + "</td>");
                            if (rE[rTA["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong1[jE], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(rE[rTA["IDVatTu"].ToString()], scale) + "</td>");
                                tong1[jE] = Config.ToDecimal(rE[rTA["IDVatTu"].ToString()]);
                            }
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    decimal tongXuat = 0;
                    decimal tongTon = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongNhap += (decimal)tong[i];
                        tongXuat += (decimal)tong[i + countTA];
                        tongTon += (decimal)tong1[i];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTon, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "Nhap")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA * 2; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTA]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTA);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }

                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                              <th>Nhập (kg)</th>
                              <th>Nhập điều chỉnh (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rTA["IDVatTu"].ToString()] == DBNull.Value && r["NhapDC" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Nhap" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["NhapDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE + countTA], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    decimal tongNhapDC = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongNhap += (decimal)tong[i];
                        tongNhapDC += (decimal)tong[i + countTA];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongNhapDC, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "NhapBT")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }

                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                              <th>Nhập (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Nhap" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongNhap += (decimal)tong[i];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "NhapDC")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }

                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                              <th>Nhập điều chỉnh (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["NhapDC" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["NhapDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhapDC = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongNhapDC += (decimal)tong[i];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongNhapDC, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "Xuat")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA * 3; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTA]) == 0 && ((decimal)tong[k + countTA + countTA]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTA + countTA);
                            tong.RemoveAt(k + countTA);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                              <th>Xuất (kg)</th>
                                <th>Xuất điều chỉnh (kg)</th>
                                <th>Hủy (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rTA["IDVatTu"].ToString()] == DBNull.Value && r["XuatDC" + rTA["IDVatTu"].ToString()] == DBNull.Value && r["XuatH" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Xuat" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatH" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE + countTA], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE + countTA + countTA], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuat = 0;
                    decimal tongXuatDC = 0;
                    decimal tongXuatH = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongXuat += (decimal)tong[i];
                        tongXuatDC += (decimal)tong[i + countTA];
                        tongXuatH += (decimal)tong[i + countTA + countTA];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongXuatDC, scale) + "</td>");
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongXuatH, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatBT")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                              <th>Xuất (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Xuat" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuat = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongXuat += (decimal)tong[i];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatDC")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                                <th>Xuất điều chỉnh (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatDC" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuatDC = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongXuatDC += (decimal)tong[i];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongXuatDC, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatH")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI HỦY THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                                <th>Hủy (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatH" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatH" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td align='left'>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuatH = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongXuatH += (decimal)tong[i];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongXuatH, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }

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
            if (ddlSapXep.SelectedValue == "Doc") ViewDoc();
            else ViewNgang();
        }

        protected void ViewNgang()
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (ddlLoaiBC.SelectedValue == "TongHop")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT NHẬP THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_FilterThucAn";
                        param = new SqlParameter[5];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn";
                        param = new SqlParameter[3];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTA * 3];
                    ArrayList tong = new ArrayList();
                    ArrayList tong1 = new ArrayList();
                    for (int l = 0; l < countTA * 3; l++) tong.Add(new decimal(0));
                    for (int l1 = 0; l1 < countTA; l1++) tong1.Add(new decimal(0));
                    int tonIndex1 = countTA * 2;
                    foreach (DataRow r in tblTA.Rows)
                    {
                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        tong1[tonIndex1 - (countTA * 2)] = tong[tonIndex1];
                        tonIndex1++;
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            if (r[rTA["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rTA["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTA]) == 0 && ((decimal)tong[k + countTA + countTA]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTA + countTA);
                            tong.RemoveAt(k + countTA);
                            tong.RemoveAt(k);
                            tong1.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có xuất nhập tồn đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Nhập (kg)</th>
                              <th colspan=" + countTA.ToString() + @">Xuất (kg)</th>
                              <th colspan=" + countTA.ToString() + @">Tồn (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (DataRow r in tblTA.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("</tr></thead><tbody>");
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>ĐK</td>");
                    for (int i = 0; i < countTA * 2; i++)
                    {
                        sb.Append("<td></td>");
                    }
                    //decimal[] tong = new decimal[countTA * 3];
                    //int tonIndex = countTA * 2;
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()], scale) + "</td>");
                        //tong[tonIndex] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        //tonIndex++;
                    }
                    sb.Append("<td></td></tr>");
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Nhap" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Xuat" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTA * 2;
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            if (r[rTA["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong1[j], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r[rTA["IDVatTu"].ToString()], scale) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rTA["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA * 3; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "Nhap")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA * 2; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTA]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTA);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có nhập đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Nhập (kg)</th>
                              <th colspan=" + countTA.ToString() + @">Nhập điều chỉnh (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    for (int i = 0; i < 2; i++)
                    {
                        foreach (DataRow r in tblTA.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Nhap" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["NhapDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA * 2; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "NhapBT")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có nhập đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Nhập (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Nhap" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "NhapDC")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có nhập điều chỉnh đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Nhập điều chỉnh (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["NhapDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "Xuat")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA * 3; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTA]) == 0 && ((decimal)tong[k + countTA + countTA]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTA + countTA);
                            tong.RemoveAt(k + countTA);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có xuất đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Xuất (kg)</th>
                              <th colspan=" + countTA.ToString() + @">Xuất điều chỉnh (kg)</th>
                              <th colspan=" + countTA.ToString() + @">Hủy (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (DataRow r in tblTA.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Xuat" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["XuatDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["XuatH" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA * 3; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "XuatBT")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có xuất đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Xuất (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Xuat" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "XuatDC")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có xuất điều chỉnh đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Xuất điều chỉnh (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["XuatDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "XuatH")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI HỦY THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có hủy đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countTA.ToString() + @">Hủy (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["XuatH" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            //j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTA; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void ViewDoc()
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (ddlLoaiBC.SelectedValue == "TongHop")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT NHẬP THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_FilterThucAn";
                        param = new SqlParameter[5];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn";
                        param = new SqlParameter[3];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTA * 3];
                    ArrayList tong = new ArrayList();
                    ArrayList tong1 = new ArrayList();
                    for (int l = 0; l < countTA * 3; l++) tong.Add(new decimal(0));
                    for (int l1 = 0; l1 < countTA; l1++) tong1.Add(new decimal(0));
                    int tonIndex1 = countTA * 2;
                    foreach (DataRow r in tblTA.Rows)
                    {
                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        tong1[tonIndex1 - (countTA * 2)] = tong[tonIndex1];
                        tonIndex1++;
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            if (r[rTA["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rTA["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTA]) == 0 && ((decimal)tong[k + countTA + countTA]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTA + countTA);
                            tong.RemoveAt(k + countTA);
                            tong.RemoveAt(k);
                            tong1.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có xuất nhập tồn đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                              <th>Nhập (kg)</th>
                              <th>Xuất (kg)</th>
                              <th>Tồn (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    bool DaCoDongDau = false;
                    foreach (DataRow r in tblTA.Rows)
                    {
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append("<td align='center'>ĐK</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "</td><td></td><td></td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()], scale) + "</td>");
                        sb.Append("<td></td></tr>");
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co xuat nhap thi viet 1 dong
                            if (r["Nhap" + rTA["IDVatTu"].ToString()] == DBNull.Value && r["Xuat" + rTA["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                j++;
                                continue;
                            }
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            if (r[rTA["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(tong1[j], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(r[rTA["IDVatTu"].ToString()], scale) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rTA["IDVatTu"].ToString()]);
                            }
                            j++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        DaCoDongDau = false;
                        DataRow rE = dt.Rows[dt.Rows.Count - 1];
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countTA], scale) + "</td>");
                            if (rE[rTA["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(tong1[jE], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(rE[rTA["IDVatTu"].ToString()], scale) + "</td>");
                                tong1[jE] = Config.ToDecimal(rE[rTA["IDVatTu"].ToString()]);
                            }
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    decimal tongXuat = 0;
                    decimal tongTon = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongNhap += (decimal)tong[i];
                        tongXuat += (decimal)tong[i + countTA];
                        tongTon += (decimal)tong1[i];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongTon, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "Nhap")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA * 2; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTA]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTA);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có nhập đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                              <th>Nhập (kg)</th>
                              <th>Nhập điều chỉnh (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rTA["IDVatTu"].ToString()] == DBNull.Value && r["NhapDC" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countTA], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    decimal tongNhapDC = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongNhap += (decimal)tong[i];
                        tongNhapDC += (decimal)tong[i + countTA];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhapDC, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "NhapBT")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có nhập đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                              <th>Nhập (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongNhap += (decimal)tong[i];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "NhapDC")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có nhập điều chỉnh đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                              <th>Nhập điều chỉnh (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["NhapDC" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhapDC = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongNhapDC += (decimal)tong[i];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhapDC, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "Xuat")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA * 3; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTA]) == 0 && ((decimal)tong[k + countTA + countTA]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTA + countTA);
                            tong.RemoveAt(k + countTA);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có xuất đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                              <th>Xuất (kg)</th>
                                <th>Xuất điều chỉnh (kg)</th>
                                <th>Hủy (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rTA["IDVatTu"].ToString()] == DBNull.Value && r["XuatDC" + rTA["IDVatTu"].ToString()] == DBNull.Value && r["XuatH" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countTA], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countTA + countTA], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuat = 0;
                    decimal tongXuatDC = 0;
                    decimal tongXuatH = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongXuat += (decimal)tong[i];
                        tongXuatDC += (decimal)tong[i + countTA];
                        tongXuatH += (decimal)tong[i + countTA + countTA];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatDC, scale) + "</td>");
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatH, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "XuatBT")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có xuất đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                              <th>Xuất (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuat = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongXuat += (decimal)tong[i];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "XuatDC")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có xuất điều chỉnh đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                                <th>Xuất điều chỉnh (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatDC" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuatDC = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongXuatDC += (decimal)tong[i];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatDC, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "XuatH")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strTA = "";
                    string colsNhap = "";

                    string strSQL = "";
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI HỦY THỨC ĂN TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblTA;
                    int countTA;
                    string strTAChon = Config.GetSelectedValues(ddlTA);
                    if (strTAChon.StartsWith("0, ")) strTAChon = strTAChon.Substring(3); strTAChon = strTAChon.Replace(", 0", "");
                    if (strTAChon != "" && strTAChon != "0, ")
                    {
                        tblTA = new DataTable();
                        tblTA.Columns.Add("IDVatTu", typeof(Int32));
                        tblTA.Columns.Add("TenVatTu", typeof(String));
                        string[] arrTA = strTAChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrTA.Length; j++)
                        {
                            strTA += "@" + arrTA[j] + "@";
                            colsNhap += ",[Nhap" + arrTA[j] + "]";
                            DataRow r = tblTA.NewRow();
                            r["IDVatTu"] = int.Parse(arrTA[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlTA, j)[1];
                            tblTA.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTA = tblTA.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet_FilterThucAn";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThucAn", strTA);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThucAn_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTA = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TA' and Active=1 order by MoTa Asc");
                        countTA = tblTA.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTA; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTA["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTA - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTA.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTA--;
                        }
                    }
                    if (countTA == 0)
                    {
                        lblMessage.Text = "Không có hủy đối với thức ăn được chọn!";
                        lt.Text = "";
                        return;
                    }
                    //int countTA = tblTA.Rows.Count;

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Thức ăn</th>
                                <th>Hủy (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatH" + rTA["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rTA["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTA in tblTA.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rTA["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuatH = 0;
                    for (int i = 0; i < countTA; i++)
                    {
                        tongXuatH += (decimal)tong[i];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatH, scale) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}