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
    public partial class r_xuatnhapspgm : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        int scale = 0;
        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable tblSPGM = csCont.VatTu_GetSPGM_Order();
            ddlSPGM.DataSource = tblSPGM;
            ddlSPGM.DataValueField = "IDVatTu";
            ddlSPGM.DataTextField = "TenVatTu";
            ddlSPGM.DataBind();
            //ddlSPGM.Items.Insert(0, new ListItem("", "0"));
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

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (ddlSapXep.SelectedValue == "Doc") ExcelDoc();
            else ExcelNgang();
        }

        protected void ExcelNgang()
        {
            try
            {
                string filename = "SPGM";
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
                    decimal TongTLH = 0;
                    decimal TongTLMH = 0;
                    int TongSL = 0;
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_FilterSPGM";
                        param = new SqlParameter[5];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM";
                        param = new SqlParameter[3];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    ArrayList tong1 = new ArrayList();
                    for (int l = 0; l < countSPGM * 3; l++) tong.Add(new decimal(0));
                    for (int l1 = 0; l1 < countSPGM; l1++) tong1.Add(new decimal(0));
                    int tonIndex1 = countSPGM * 2;
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        tong1[tonIndex1 - (countSPGM * 2)] = tong[tonIndex1];
                        tonIndex1++;
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            if (r[rSPGM["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rSPGM["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countSPGM]) == 0 && ((decimal)tong[k + countSPGM + countSPGM]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countSPGM + countSPGM);
                            tong.RemoveAt(k + countSPGM);
                            tong.RemoveAt(k);
                            tong1.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th><th rowspan=2>Biên bản<br/>giết mổ</th><th rowspan=2>Khối lượng<br/>hơi (kg)</th><th rowspan=2>Khối lượng<br/>móc hàm (kg)</th><th rowspan=2>Số lượng<br/>cá giết mổ</th>
                              <th colspan=" + countSPGM.ToString() + @">Nhập (kg)</th>
                              <th colspan=" + countSPGM.ToString() + @">Xuất (kg)</th>
                              <th colspan=" + countSPGM.ToString() + @">Tồn (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (DataRow r in tblSPGM.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("</tr></thead><tbody>");
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>ĐK</td><td></td><td></td><td></td><td></td>");
                    for (int i = 0; i < countSPGM * 2; i++)
                    {
                        sb.Append("<td></td>");
                    }
                    //decimal[] tong = new decimal[countSPGM * 3];
                    //int tonIndex = countSPGM * 2;
                    foreach (DataRow r in tblSPGM.Rows)
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
                        sb.Append("<td align='left'>" + r["BienBan"].ToString() + "</td>");
                        string tlh = r["TrongLuongHoi"] == DBNull.Value ? "" : Config.ToXVal2(r["TrongLuongHoi"],1);
                        TongTLH += r["TrongLuongHoi"] == DBNull.Value ? 0 : Convert.ToDecimal(r["TrongLuongHoi"]);
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + tlh + "</td>");
                        string tlmh = r["TrongLuongMocHam"] == DBNull.Value ? "" : Config.ToXVal2(r["TrongLuongMocHam"],1);
                        TongTLMH += r["TrongLuongMocHam"] == DBNull.Value ? 0 : Convert.ToDecimal(r["TrongLuongMocHam"]);
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + tlmh + "</td>");
                        string slm = r["SoLuongMo"] == DBNull.Value ? "" : r["SoLuongMo"].ToString();
                        TongSL += r["SoLuongMo"] == DBNull.Value ? 0 : Convert.ToInt32(r["SoLuongMo"]);
                        sb.Append(@"<td style='text-align:right;'>" + slm + "</td>");
                        //int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Nhap" + rSPGM["IDVatTu"].ToString()],scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Xuat" + rSPGM["IDVatTu"].ToString()],scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            //j++;
                        }
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            if (r[rSPGM["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong1[j],scale) + "</td>");
                            }
                            else
                            {
                                sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r[rSPGM["IDVatTu"].ToString()],scale) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rSPGM["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append(@"<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(TongTLH, 1) + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(TongTLMH, 1) + "</td><td align='right'>" + TongSL.ToString() + "</td>");
                    for (int i = 0; i < countSPGM * 3; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM * 2; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countSPGM]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countSPGM);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Nhập (kg)</th>
                              <th colspan=" + countSPGM.ToString() + @">Nhập điều chỉnh (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    for (int i = 0; i < 2; i++)
                    {
                        foreach (DataRow r in tblSPGM.Rows)
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
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Nhap" + rSPGM["IDVatTu"].ToString()],scale) + "</td>");
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["NhapDC" + rSPGM["IDVatTu"].ToString()],scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM * 2; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Nhập (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Nhap" + rSPGM["IDVatTu"].ToString()],scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[i],scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "NhapDC")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Nhập điều chỉnh (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["NhapDC" + rSPGM["IDVatTu"].ToString()],scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[i],scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "Xuat")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM * 3; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countSPGM]) == 0 && ((decimal)tong[k + countSPGM + countSPGM]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countSPGM + countSPGM);
                            tong.RemoveAt(k + countSPGM);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Xuất (kg)</th>
                              <th colspan=" + countSPGM.ToString() + @">Xuất điều chỉnh (kg)</th>
                              <th colspan=" + countSPGM.ToString() + @">Hủy (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (DataRow r in tblSPGM.Rows)
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
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Xuat" + rSPGM["IDVatTu"].ToString()],scale) + "</td>");
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatDC" + rSPGM["IDVatTu"].ToString()],scale) + "</td>");
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatH" + rSPGM["IDVatTu"].ToString()],scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM * 3; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Xuất (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Xuat" + rSPGM["IDVatTu"].ToString()],scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[i],scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatDC")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Xuất điều chỉnh (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatDC" + rSPGM["IDVatTu"].ToString()],scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[i],scale) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatH")
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI HỦY SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Hủy (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatH" + rSPGM["IDVatTu"].ToString()],scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[i],scale) + "</td>");
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
                string filename = "SPGM";
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
                    decimal TongTLH = 0;
                    decimal TongTLMH = 0;
                    int TongSL = 0;
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_FilterSPGM";
                        param = new SqlParameter[5];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM";
                        param = new SqlParameter[3];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    ArrayList tong1 = new ArrayList();
                    for (int l = 0; l < countSPGM * 3; l++) tong.Add(new decimal(0));
                    for (int l1 = 0; l1 < countSPGM; l1++) tong1.Add(new decimal(0));
                    int tonIndex1 = countSPGM * 2;
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        tong1[tonIndex1 - (countSPGM * 2)] = tong[tonIndex1];
                        tonIndex1++;
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            if (r[rSPGM["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rSPGM["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countSPGM]) == 0 && ((decimal)tong[k + countSPGM + countSPGM]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countSPGM + countSPGM);
                            tong.RemoveAt(k + countSPGM);
                            tong.RemoveAt(k);
                            tong1.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Biên bản<br/>giết mổ</th><th>Khối lượng<br/>hơi (kg)</th><th>Khối lượng<br/>móc hàm (kg)</th><th>Số lượng<br/>cá giết mổ</th><th>SPGM</th>
                              <th>Nhập (kg)</th>
                              <th>Xuất (kg)</th>
                              <th>Tồn (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    bool DaCoDongDau = false;
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append("<td align='center'>ĐK</td><td></td><td></td><td></td><td></td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td><td></td><td></td><td></td><td></td>");
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
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co xuat nhap thi viet 1 dong
                            if (r["Nhap" + rSPGM["IDVatTu"].ToString()] == DBNull.Value && r["Xuat" + rSPGM["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                j++;
                                continue;
                            }
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                sb.Append("<td align='left'>" + r["BienBan"].ToString() + "</td>");
                                string tlh = r["TrongLuongHoi"] == DBNull.Value ? "" : Config.ToXVal2(r["TrongLuongHoi"], 1);
                                TongTLH += r["TrongLuongHoi"] == DBNull.Value ? 0 : Convert.ToDecimal(r["TrongLuongHoi"]);
                                sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(1) + ";'>" + tlh + "</td>");
                                string tlmh = r["TrongLuongMocHam"] == DBNull.Value ? "" : Config.ToXVal2(r["TrongLuongMocHam"], 1);
                                TongTLMH += r["TrongLuongMocHam"] == DBNull.Value ? 0 : Convert.ToDecimal(r["TrongLuongMocHam"]);
                                sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(1) + ";'>" + tlmh + "</td>");
                                string slm = r["SoLuongMo"] == DBNull.Value ? "" : r["SoLuongMo"].ToString();
                                TongSL += r["SoLuongMo"] == DBNull.Value ? 0 : Convert.ToInt32(r["SoLuongMo"]);
                                sb.Append("<td style='text-align:right;'>" + slm + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td><td></td><td></td><td></td><td></td>");
                            }
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Nhap" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Xuat" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            if (r[rSPGM["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong1[j], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r[rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rSPGM["IDVatTu"].ToString()]);
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
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td><td></td><td></td><td></td><td></td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td><td></td><td></td><td></td><td></td>");
                            }
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE + countSPGM], scale) + "</td>");
                            if (rE[rSPGM["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong1[jE], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(rE[rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                                tong1[jE] = Config.ToDecimal(rE[rSPGM["IDVatTu"].ToString()]);
                            }
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(1) + ";'>" + Config.ToXVal2(TongTLH, 1) + "</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(1) + ";'>" + Config.ToXVal2(TongTLMH, 1) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TongSL, 0) + "</td><td></td>");
                    decimal tongNhap = 0;
                    decimal tongXuat = 0;
                    decimal tongTon = 0;
                    for (int i = 0; i < countSPGM; i++)
                    {
                        tongNhap += (decimal)tong[i];
                        tongXuat += (decimal)tong[i + countSPGM];
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM * 2; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countSPGM]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countSPGM);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>SPGM</th>
                              <th>Nhập (kg)</th>
                              <th>Nhập điều chỉnh (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rSPGM["IDVatTu"].ToString()] == DBNull.Value && r["NhapDC" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Nhap" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["NhapDC" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE + countSPGM], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    decimal tongNhapDC = 0;
                    for (int i = 0; i < countSPGM; i++)
                    {
                        tongNhap += (decimal)tong[i];
                        tongNhapDC += (decimal)tong[i + countSPGM];
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>SPGM</th>
                              <th>Nhập (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Nhap" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    for (int i = 0; i < countSPGM; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>SPGM</th>
                              <th>Nhập điều chỉnh (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["NhapDC" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["NhapDC" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhapDC = 0;
                    for (int i = 0; i < countSPGM; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM * 3; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countSPGM]) == 0 && ((decimal)tong[k + countSPGM + countSPGM]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countSPGM + countSPGM);
                            tong.RemoveAt(k + countSPGM);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>SPGM</th>
                              <th>Xuất (kg)</th>
                                <th>Xuất điều chỉnh (kg)</th>
                                <th>Hủy (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rSPGM["IDVatTu"].ToString()] == DBNull.Value && r["XuatDC" + rSPGM["IDVatTu"].ToString()] == DBNull.Value && r["XuatH" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Xuat" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatDC" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatH" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE + countSPGM], scale) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE + countSPGM + countSPGM], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuat = 0;
                    decimal tongXuatDC = 0;
                    decimal tongXuatH = 0;
                    for (int i = 0; i < countSPGM; i++)
                    {
                        tongXuat += (decimal)tong[i];
                        tongXuatDC += (decimal)tong[i + countSPGM];
                        tongXuatH += (decimal)tong[i + countSPGM + countSPGM];
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>SPGM</th>
                              <th>Xuất (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["Xuat" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuat = 0;
                    for (int i = 0; i < countSPGM; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>SPGM</th>
                                <th>Xuất điều chỉnh (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatDC" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatDC" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuatDC = 0;
                    for (int i = 0; i < countSPGM; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI HỦY SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>SPGM</th>
                                <th>Hủy (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatH" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(r["XuatH" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td align='left'>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuatH = 0;
                    for (int i = 0; i < countSPGM; i++)
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
                    decimal TongTLH = 0;
                    decimal TongTLMH = 0;
                    int TongSL = 0;
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_FilterSPGM";
                        param = new SqlParameter[5];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM";
                        param = new SqlParameter[3];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    ArrayList tong1 = new ArrayList();
                    for (int l = 0; l < countSPGM * 3; l++) tong.Add(new decimal(0));
                    for (int l1 = 0; l1 < countSPGM; l1++) tong1.Add(new decimal(0));
                    int tonIndex1 = countSPGM * 2;
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        tong1[tonIndex1 - (countSPGM * 2)] = tong[tonIndex1];
                        tonIndex1++;
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            if (r[rSPGM["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rSPGM["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countSPGM]) == 0 && ((decimal)tong[k + countSPGM + countSPGM]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countSPGM + countSPGM);
                            tong.RemoveAt(k + countSPGM);
                            tong.RemoveAt(k);
                            tong1.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có xuất nhập tồn đối với SPGM được chọn!";
                        lt.Text = "";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th><th rowspan=2>Biên bản<br/>giết mổ</th><th rowspan=2>Khối lượng<br/>hơi (kg)</th><th rowspan=2>Khối lượng<br/>móc hàm (kg)</th><th rowspan=2>Số lượng<br/>cá giết mổ</th>
                              <th colspan=" + countSPGM.ToString() + @">Nhập (kg)</th>
                              <th colspan=" + countSPGM.ToString() + @">Xuất (kg)</th>
                              <th colspan=" + countSPGM.ToString() + @">Tồn (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (DataRow r in tblSPGM.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("</tr></thead><tbody>");
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>ĐK</td><td></td><td></td><td></td><td></td>");
                    for (int i = 0; i < countSPGM * 2; i++)
                    {
                        sb.Append("<td></td>");
                    }
                    //decimal[] tong = new decimal[countSPGM * 3];
                    //int tonIndex = countSPGM * 2;
                    foreach (DataRow r in tblSPGM.Rows)
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
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("<td>" + r["BienBan"].ToString() + "</td>");
                        string tlh = r["TrongLuongHoi"] == DBNull.Value ? "" : Config.ToXVal2(r["TrongLuongHoi"], 1);
                        TongTLH += r["TrongLuongHoi"] == DBNull.Value ? 0 : Convert.ToDecimal(r["TrongLuongHoi"]);
                        sb.Append("<td style='text-align:right;'>" + tlh + "</td>");
                        string tlmh = r["TrongLuongMocHam"] == DBNull.Value ? "" : Config.ToXVal2(r["TrongLuongMocHam"], 1);
                        TongTLMH += r["TrongLuongMocHam"] == DBNull.Value ? 0 : Convert.ToDecimal(r["TrongLuongMocHam"]);
                        sb.Append("<td style='text-align:right;'>" + tlmh + "</td>");
                        string slm = r["SoLuongMo"] == DBNull.Value ? "" : r["SoLuongMo"].ToString();
                        TongSL += r["SoLuongMo"] == DBNull.Value ? 0 : Convert.ToInt32(r["SoLuongMo"]);
                        sb.Append("<td style='text-align:right;'>" + slm + "</td>");
                        //int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Nhap" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Xuat" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            //j++;
                        }
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            if (r[rSPGM["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong1[j], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r[rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rSPGM["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td><td></td><td align='right'>" + Config.ToXVal2(TongTLH, 1) + "</td><td align='right'>" + Config.ToXVal2(TongTLMH, 1) + "</td><td align='right'>" + Config.ToXVal2(TongSL, 0) + "</td>");
                    for (int i = 0; i < countSPGM * 3; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM * 2; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countSPGM]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countSPGM);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có nhập đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Nhập (kg)</th>
                              <th colspan=" + countSPGM.ToString() + @">Nhập điều chỉnh (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    for (int i = 0; i < 2; i++)
                    {
                        foreach (DataRow r in tblSPGM.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Nhap" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["NhapDC" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM * 2; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có nhập đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Nhập (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Nhap" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có nhập điều chỉnh đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Nhập điều chỉnh (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["NhapDC" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM * 3; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countSPGM]) == 0 && ((decimal)tong[k + countSPGM + countSPGM]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countSPGM + countSPGM);
                            tong.RemoveAt(k + countSPGM);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có xuất đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Xuất (kg)</th>
                              <th colspan=" + countSPGM.ToString() + @">Xuất điều chỉnh (kg)</th>
                              <th colspan=" + countSPGM.ToString() + @">Hủy (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (DataRow r in tblSPGM.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Xuat" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["XuatDC" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["XuatH" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM * 3; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có xuất đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Xuất (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Xuat" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có xuất điều chỉnh đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Xuất điều chỉnh (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["XuatDC" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI HỦY SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có hủy đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countSPGM.ToString() + @">Hủy (kg)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["XuatH" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countSPGM; i++)
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
                    decimal TongTLH = 0;
                    decimal TongTLMH = 0;
                    int TongSL = 0;
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_FilterSPGM";
                        param = new SqlParameter[5];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM";
                        param = new SqlParameter[3];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    ArrayList tong1 = new ArrayList();
                    for (int l = 0; l < countSPGM * 3; l++) tong.Add(new decimal(0));
                    for (int l1 = 0; l1 < countSPGM; l1++) tong1.Add(new decimal(0));
                    int tonIndex1 = countSPGM * 2;
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        tong1[tonIndex1 - (countSPGM * 2)] = tong[tonIndex1];
                        tonIndex1++;
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            if (r[rSPGM["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rSPGM["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countSPGM]) == 0 && ((decimal)tong[k + countSPGM + countSPGM]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countSPGM + countSPGM);
                            tong.RemoveAt(k + countSPGM);
                            tong.RemoveAt(k);
                            tong1.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có xuất nhập tồn đối với SPGM được chọn!";
                        lt.Text = "";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Biên bản<br/>giết mổ</th><th>Khối lượng<br/>hơi (kg)</th><th>Khối lượng<br/>móc hàm (kg)</th><th>Số lượng<br/>cá giết mổ</th><th>SPGM</th>
                              <th>Nhập (kg)</th>
                              <th>Xuất (kg)</th>
                              <th>Tồn (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    bool DaCoDongDau = false;
                    foreach (DataRow r in tblSPGM.Rows)
                    {
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append("<td align='center'>ĐK</td><td></td><td></td><td></td><td></td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td><td></td><td></td><td></td><td></td>");
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
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co xuat nhap thi viet 1 dong
                            if (r["Nhap" + rSPGM["IDVatTu"].ToString()] == DBNull.Value && r["Xuat" + rSPGM["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                j++;
                                continue;
                            }
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                                sb.Append("<td>" + r["BienBan"].ToString() + "</td>");
                                string tlh = r["TrongLuongHoi"] == DBNull.Value ? "" : Config.ToXVal2(r["TrongLuongHoi"], 1);
                                TongTLH += r["TrongLuongHoi"] == DBNull.Value ? 0 : Convert.ToDecimal(r["TrongLuongHoi"]);
                                sb.Append("<td style='text-align:right;'>" + tlh + "</td>");
                                string tlmh = r["TrongLuongMocHam"] == DBNull.Value ? "" : Config.ToXVal2(r["TrongLuongMocHam"], 1);
                                TongTLMH += r["TrongLuongMocHam"] == DBNull.Value ? 0 : Convert.ToDecimal(r["TrongLuongMocHam"]);
                                sb.Append("<td style='text-align:right;'>" + tlmh + "</td>");
                                string slm = r["SoLuongMo"] == DBNull.Value ? "" : r["SoLuongMo"].ToString();
                                TongSL += r["SoLuongMo"] == DBNull.Value ? 0 : Convert.ToInt32(r["SoLuongMo"]);
                                sb.Append("<td style='text-align:right;'>" + slm + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td><td></td><td></td><td></td><td></td>");
                            }
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            if (r[rSPGM["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(tong1[j], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(r[rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rSPGM["IDVatTu"].ToString()]);
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
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Viet 1 dong
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td><td></td><td></td><td></td><td></td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td><td></td><td></td><td></td><td></td>");
                            }
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countSPGM], scale) + "</td>");
                            if (rE[rSPGM["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(tong1[jE], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(rE[rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                                tong1[jE] = Config.ToDecimal(rE[rSPGM["IDVatTu"].ToString()]);
                            }
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td><td align='right'>" + Config.ToXVal2(TongTLH, 1) + "</td><td align='right'>" + Config.ToXVal2(TongTLMH, 1) + "</td><td align='right'>" + Config.ToXVal2(TongSL, 0) + "</td><td></td>");
                    decimal tongNhap = 0;
                    decimal tongXuat = 0;
                    decimal tongTon = 0;
                    for (int i = 0; i < countSPGM; i++)
                    {
                        tongNhap += (decimal)tong[i];
                        tongXuat += (decimal)tong[i + countSPGM];
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM * 2; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countSPGM]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countSPGM);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có nhập đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                                <th>Ngày</th><th>SPGM</th>
                              <th>Nhập (kg)</th>
                              <th>Nhập điều chỉnh (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rSPGM["IDVatTu"].ToString()] == DBNull.Value && r["NhapDC" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countSPGM], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    decimal tongNhapDC = 0;
                    for (int i = 0; i < countSPGM; i++)
                    {
                        tongNhap += (decimal)tong[i];
                        tongNhapDC += (decimal)tong[i + countSPGM];
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có nhập đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>SPGM</th>
                              <th>Nhập (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    for (int i = 0; i < countSPGM; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có nhập điều chỉnh đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>SPGM</th>
                              <th>Nhập điều chỉnh (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["NhapDC" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhapDC = 0;
                    for (int i = 0; i < countSPGM; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM * 3; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countSPGM]) == 0 && ((decimal)tong[k + countSPGM + countSPGM]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countSPGM + countSPGM);
                            tong.RemoveAt(k + countSPGM);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có xuất đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>SPGM</th>
                              <th>Xuất (kg)</th>
                                <th>Xuất điều chỉnh (kg)</th>
                                <th>Hủy (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rSPGM["IDVatTu"].ToString()] == DBNull.Value && r["XuatDC" + rSPGM["IDVatTu"].ToString()] == DBNull.Value && r["XuatH" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countSPGM], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countSPGM + countSPGM], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuat = 0;
                    decimal tongXuatDC = 0;
                    decimal tongXuatH = 0;
                    for (int i = 0; i < countSPGM; i++)
                    {
                        tongXuat += (decimal)tong[i];
                        tongXuatDC += (decimal)tong[i + countSPGM];
                        tongXuatH += (decimal)tong[i + countSPGM + countSPGM];
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có xuất đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>SPGM</th>
                              <th>Xuất (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuat = 0;
                    for (int i = 0; i < countSPGM; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có xuất điều chỉnh đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>SPGM</th>
                                <th>Xuất điều chỉnh (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatDC" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuatDC = 0;
                    for (int i = 0; i < countSPGM; i++)
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
                    string strSPGM = "";
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
                    tieude += "<b>BẢNG THEO DÕI HỦY SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    SqlParameter[] param;
                    DataTable dt;
                    DataTable tblSPGM;
                    int countSPGM;
                    string strSPGMChon = Config.GetSelectedValues(ddlSPGM);
                    if (strSPGMChon.StartsWith("0, ")) strSPGMChon = strSPGMChon.Substring(3); strSPGMChon = strSPGMChon.Replace(", 0", "");
                    if (strSPGMChon != "" && strSPGMChon != "0, ")
                    {
                        tblSPGM = new DataTable();
                        tblSPGM.Columns.Add("IDVatTu", typeof(Int32));
                        tblSPGM.Columns.Add("TenVatTu", typeof(String));
                        string[] arrSPGM = strSPGMChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrSPGM.Length; j++)
                        {
                            strSPGM += "@" + arrSPGM[j] + "@";
                            colsNhap += ",[Nhap" + arrSPGM[j] + "]";
                            DataRow r = tblSPGM.NewRow();
                            r["IDVatTu"] = int.Parse(arrSPGM[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlSPGM, j)[1];
                            tblSPGM.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countSPGM = tblSPGM.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet_FilterSPGM";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrSPGM", strSPGM);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapSPGM_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblSPGM = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='SPGM' and Active=1 order by MoTa Asc");
                        countSPGM = tblSPGM.Rows.Count;
                    }

                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countSPGM; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rSPGM["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countSPGM - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblSPGM.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countSPGM--;
                        }
                    }
                    if (countSPGM == 0)
                    {
                        lblMessage.Text = "Không có hủy đối với SPGM được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>SPGM</th>
                                <th>Hủy (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatH" + rSPGM["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rSPGM["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rSPGM in tblSPGM.Rows)
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
                            sb.Append("<td>" + rSPGM["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuatH = 0;
                    for (int i = 0; i < countSPGM; i++)
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