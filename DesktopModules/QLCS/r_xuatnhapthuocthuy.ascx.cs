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
    public partial class r_xuatnhapthuocthuy : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text =DateTime.Now.ToString("dd/MM/yyyy");

            DataTable tblTTY = csCont.VatTu_GetTTY_Order_CoDonViTinh();
            ddlThuoc.DataSource = tblTTY;
            ddlThuoc.DataValueField = "IDVatTu";
            ddlThuoc.DataTextField = "TenVatTu";
            ddlThuoc.DataBind();
            //ddlThuoc.Items.Insert(0, new ListItem("", "0"));
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
            if (ddlSapXep.SelectedValue == "Doc") ExcelDoc();
            else ExcelNgang();
        }

        protected void ExcelNgang()
        {
            try
            {
                int c = 3; if (txtC.Text != "") c = int.Parse(txtC.Text);
                string d = "";
                for (int f = 0; f < c; f++)
                {
                    d += "0";
                }
                string filename = "TTY";
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
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_FilterThuoc";
                        param = new SqlParameter[5];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY";
                        param = new SqlParameter[3];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    ArrayList tong1 = new ArrayList();
                    for (int l = 0; l < countTTY * 3; l++) tong.Add(new decimal(0));
                    for (int l1 = 0; l1 < countTTY; l1++) tong1.Add(new decimal(0));
                    int tonIndex1 = countTTY * 2;
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        tong1[tonIndex1 - (countTTY * 2)] = tong[tonIndex1];
                        tonIndex1++;
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            if (r[rTTY["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rTTY["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0 && ((decimal)tong[k + countTTY + countTTY]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTTY + countTTY);
                            tong.RemoveAt(k + countTTY);
                            tong.RemoveAt(k);
                            tong1.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Nhập</th>
                              <th colspan=" + countTTY.ToString() + @">Xuất</th>
                              <th colspan=" + countTTY.ToString() + @">Tồn</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (DataRow r in tblTTY.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>ĐK</td>");
                    for (int i = 0; i < countTTY * 2; i++)
                    {
                        sb.Append("<td></td>");
                    }
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()],c) + "</td>");
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
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(r["Nhap" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(r["Xuat" + rTTY["IDVatTu"].ToString()],c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            if (r[rTTY["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(tong1[j],c) + "</td>");
                            }
                            else
                            {
                                sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(r[rTTY["IDVatTu"].ToString()],c) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rTTY["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY * 3; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(tong[i],c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "Nhap")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY * 2; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTTY);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Nhập</th>
                              <th colspan=" + countTTY.ToString() + @">Nhập điều chỉnh</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    for (int i = 0; i < 2; i++)
                    {
                        foreach (DataRow r in tblTTY.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(r["Nhap" + rTTY["IDVatTu"].ToString()],c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(r["NhapDC" + rTTY["IDVatTu"].ToString()],c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY * 2; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(tong[i],c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "NhapBT")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Nhập</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    for (int i = 0; i < 2; i++)
                    {
                        foreach (DataRow r in tblTTY.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(r["Nhap" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(tong[i], c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "NhapDC")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Nhập điều chỉnh</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    for (int i = 0; i < 2; i++)
                    {
                        foreach (DataRow r in tblTTY.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(r["NhapDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(tong[i], c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "Xuat")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY * 3; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0 && ((decimal)tong[k + countTTY + countTTY]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTTY + countTTY);
                            tong.RemoveAt(k + countTTY);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Xuất</th>
                              <th colspan=" + countTTY.ToString() + @">Xuất điều chỉnh</th>
                              <th colspan=" + countTTY.ToString() + @">Hủy</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (DataRow r in tblTTY.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(r["Xuat" + rTTY["IDVatTu"].ToString()],c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(r["XuatDC" + rTTY["IDVatTu"].ToString()],c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(r["XuatH" + rTTY["IDVatTu"].ToString()],c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY * 3; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(tong[i],c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatBT")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Xuất</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(r["Xuat" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(tong[i], c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatDC")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Xuất điều chỉnh</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(r["XuatDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(tong[i], c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatH")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI HỦY THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Hủy</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(r["XuatH" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY; i++)
                    {
                        sb.Append(@"<td style='text-align:right;mso-number-format:""\#\,\#\#0\." + d + @"""'>" + Config.ToXVal2(tong[i], c) + "</td>");
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
                int c = 3; if (txtC.Text != "") c = int.Parse(txtC.Text);
                string d = "";
                for (int f = 0; f < c; f++)
                {
                    d += "0";
                }
                string filename = "TTY";
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
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_FilterThuoc";
                        param = new SqlParameter[5];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY";
                        param = new SqlParameter[3];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    ArrayList tong1 = new ArrayList();
                    for (int l = 0; l < countTTY * 3; l++) tong.Add(new decimal(0));
                    for (int l1 = 0; l1 < countTTY; l1++) tong1.Add(new decimal(0));
                    int tonIndex1 = countTTY * 2;
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        tong1[tonIndex1 - (countTTY * 2)] = tong[tonIndex1];
                        tonIndex1++;
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            if (r[rTTY["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rTTY["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0 && ((decimal)tong[k + countTTY + countTTY]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTTY + countTTY);
                            tong.RemoveAt(k + countTTY);
                            tong.RemoveAt(k);
                            tong1.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                              <th>Nhập</th>
                              <th>Xuất</th>
                              <th>Tồn</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    bool DaCoDongDau = false;
                    foreach (DataRow r in tblTTY.Rows)
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
                        sb.Append(@"<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()], c) + "</td>");
                        sb.Append("<td></td></tr>");
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co xuat nhap thi viet 1 dong
                            if (r["Nhap" + rTTY["IDVatTu"].ToString()] == DBNull.Value && r["Xuat" + rTTY["IDVatTu"].ToString()] == DBNull.Value)
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(r["Nhap" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(r["Xuat" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            if (r[rTTY["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong1[j], c) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(r[rTTY["IDVatTu"].ToString()], c) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rTTY["IDVatTu"].ToString()]);
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
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong[jE + countTTY], c) + "</td>");
                            if (rE[rTTY["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong1[jE], c) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(rE[rTTY["IDVatTu"].ToString()], c) + "</td>");
                                tong1[jE] = Config.ToDecimal(rE[rTTY["IDVatTu"].ToString()]);
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
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongNhap += (decimal)tong[i];
                        tongXuat += (decimal)tong[i + countTTY];
                        tongTon += (decimal)tong1[i];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tongNhap, c) + "</td>");
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tongXuat, c) + "</td>");
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tongTon, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "Nhap")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY * 2; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTTY);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                              <th>Nhập</th>
                              <th>Nhập điều chỉnh</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rTTY["IDVatTu"].ToString()] == DBNull.Value && r["NhapDC" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(r["Nhap" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(r["NhapDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong[jE + countTTY], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    decimal tongNhapDC = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongNhap += (decimal)tong[i];
                        tongNhapDC += (decimal)tong[i + countTTY];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tongNhap, c) + "</td>");
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tongNhapDC, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "NhapBT")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                              <th>Nhập</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(r["Nhap" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongNhap += (decimal)tong[i];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tongNhap, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "NhapDC")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                              <th>Nhập điều chỉnh</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["NhapDC" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(r["NhapDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhapDC = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongNhapDC += (decimal)tong[i];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tongNhapDC, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "Xuat")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY * 3; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0 && ((decimal)tong[k + countTTY + countTTY]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTTY + countTTY);
                            tong.RemoveAt(k + countTTY);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                              <th>Xuất</th>
                                <th>Xuất điều chỉnh</th>
                                <th>Hủy</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rTTY["IDVatTu"].ToString()] == DBNull.Value && r["XuatDC" + rTTY["IDVatTu"].ToString()] == DBNull.Value && r["XuatH" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(r["Xuat" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(r["XuatDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(r["XuatH" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong[jE + countTTY], c) + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong[jE + countTTY + countTTY], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuat = 0;
                    decimal tongXuatDC = 0;
                    decimal tongXuatH = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongXuat += (decimal)tong[i];
                        tongXuatDC += (decimal)tong[i + countTTY];
                        tongXuatH += (decimal)tong[i + countTTY + countTTY];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tongXuat, c) + "</td>");
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tongXuatDC, c) + "</td>");
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tongXuatH, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatBT")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                              <th>Xuất</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(r["Xuat" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuat = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongXuat += (decimal)tong[i];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tongXuat, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatDC")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                                <th>Xuất điều chỉnh</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatDC" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(r["XuatDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuatDC = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongXuatDC += (decimal)tong[i];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tongXuatDC, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                }
                else if (ddlLoaiBC.SelectedValue == "XuatH")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI HỦY THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        sb.Append("</body></html>");
                        Response.Write(sb.ToString());
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                                <th>Hủy</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatH" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(r["XuatH" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td align='left'>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuatH = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongXuatH += (decimal)tong[i];
                    }
                    sb.Append("<td align='right' style='mso-number-format:" + Config.ExcelFormat(c) + ";'>" + Config.ToXVal2(tongXuatH, c) + "</td>");
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
                int c = 3; if (txtC.Text != "") c = int.Parse(txtC.Text);
                if (ddlLoaiBC.SelectedValue == "TongHop")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_FilterThuoc";
                        param = new SqlParameter[5];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY";
                        param = new SqlParameter[3];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    ArrayList tong1 = new ArrayList();
                    for (int l = 0; l < countTTY * 3; l++) tong.Add(new decimal(0));
                    for (int l1 = 0; l1 < countTTY; l1++) tong1.Add(new decimal(0));
                    int tonIndex1 = countTTY * 2;
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        tong1[tonIndex1 - (countTTY * 2)] = tong[tonIndex1];
                        tonIndex1++;
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            if (r[rTTY["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rTTY["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0 && ((decimal)tong[k + countTTY + countTTY]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTTY + countTTY);
                            tong.RemoveAt(k + countTTY);
                            tong.RemoveAt(k);
                            tong1.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có xuất nhập tồn đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Nhập</th>
                              <th colspan=" + countTTY.ToString() + @">Xuất</th>
                              <th colspan=" + countTTY.ToString() + @">Tồn</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (DataRow r in tblTTY.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>ĐK</td>");
                    for (int i = 0; i < countTTY * 2; i++)
                    {
                        sb.Append("<td></td>");
                    }
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()], c) + "</td>");
                        //tong[tonIndex] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        //tonIndex++;
                    }
                    sb.Append("<td></td></tr>");
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Nhap" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Xuat" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            if (r[rTTY["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong1[j], c) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r[rTTY["IDVatTu"].ToString()], c) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rTTY["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY * 3; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "Nhap")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY * 2; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTTY);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có nhập đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Nhập</th>
                              <th colspan=" + countTTY.ToString() + @">Nhập điều chỉnh</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    for (int i = 0; i < 2; i++)
                    {
                        foreach (DataRow r in tblTTY.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Nhap" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["NhapDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY * 2; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "NhapBT")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có nhập đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Nhập</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Nhap" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "NhapDC")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có nhập điều chỉnh đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Nhập điều chỉnh</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["NhapDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "Xuat")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY * 3; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0 && ((decimal)tong[k + countTTY + countTTY]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTTY + countTTY);
                            tong.RemoveAt(k + countTTY);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có xuất đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Xuất</th>
                              <th colspan=" + countTTY.ToString() + @">Xuất điều chỉnh</th>
                              <th colspan=" + countTTY.ToString() + @">Hủy</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (DataRow r in tblTTY.Rows)
                        {
                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                        }
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Xuat" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["XuatDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["XuatH" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY * 3; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "XuatBT")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có xuất đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Xuất</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["Xuat" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "XuatDC")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có xuất điều chỉnh đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Xuất điều chỉnh</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["XuatDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], c) + "</td>");
                    }
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "XuatH")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI HỦY THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có hủy đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th>
                              <th colspan=" + countTTY.ToString() + @">Hủy</th>
                              <th>Ghi chú</th>
                             </tr>
                             <tr><th></th>");
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                    sb.Append("<th></th></tr></thead><tbody>");
                    //decimal[] tong = new decimal[countTTY * 3];
                    //int tonIndex = countTTY * 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        sb.Append("<tr>");
                        sb.Append("<td>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>");
                        //int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["XuatH" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            //j++;
                        }
                        //int j = countTTY * 2;
                        sb.Append("<td></td></tr>");
                    }

                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                    for (int i = 0; i < countTTY; i++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong[i], c) + "</td>");
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
                int c = 3; if (txtC.Text != "") c = int.Parse(txtC.Text);
                if (ddlLoaiBC.SelectedValue == "TongHop")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_FilterThuoc";
                        param = new SqlParameter[5];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY";
                        param = new SqlParameter[3];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    ArrayList tong1 = new ArrayList();
                    for (int l = 0; l < countTTY * 3; l++) tong.Add(new decimal(0));
                    for (int l1 = 0; l1 < countTTY; l1++) tong1.Add(new decimal(0));
                    int tonIndex1 = countTTY * 2;
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                        tong1[tonIndex1 - (countTTY * 2)] = tong[tonIndex1];
                        tonIndex1++;
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            if (r[rTTY["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rTTY["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0 && ((decimal)tong[k + countTTY + countTTY]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTTY + countTTY);
                            tong.RemoveAt(k + countTTY);
                            tong.RemoveAt(k);
                            tong1.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có xuất nhập tồn đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                              <th>Nhập</th>
                              <th>Xuất</th>
                              <th>Tồn</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    bool DaCoDongDau = false;
                    foreach (DataRow r in tblTTY.Rows)
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
                        sb.Append("<td align='right'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()], c) + "</td>");
                        sb.Append("<td></td></tr>");
                    }
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co xuat nhap thi viet 1 dong
                            if (r["Nhap" + rTTY["IDVatTu"].ToString()] == DBNull.Value && r["Xuat" + rTTY["IDVatTu"].ToString()] == DBNull.Value)
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            if (r[rTTY["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(tong1[j], c) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(r[rTTY["IDVatTu"].ToString()], c) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rTTY["IDVatTu"].ToString()]);
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
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countTTY], c) + "</td>");
                            if (rE[rTTY["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(tong1[jE], c) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(rE[rTTY["IDVatTu"].ToString()], c) + "</td>");
                                tong1[jE] = Config.ToDecimal(rE[rTTY["IDVatTu"].ToString()]);
                            }
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Tính tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    decimal tongTon = 0;
                    decimal tongXuat = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongNhap += (decimal)tong[i];
                        tongXuat += (decimal)tong[i + countTTY];
                        tongTon += (decimal)tong1[i];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, c) + "</td>");
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, c) + "</td>");
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongTon, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "Nhap")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY * 2; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTTY);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có nhập đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                              <th>Nhập</th>
                              <th>Nhập điều chỉnh</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rTTY["IDVatTu"].ToString()] == DBNull.Value && r["NhapDC" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countTTY], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    decimal tongNhapDC = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongNhap += (decimal)tong[i];
                        tongNhapDC += (decimal)tong[i + countTTY];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, c) + "</td>");
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhapDC, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "NhapBT")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có nhập đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                              <th>Nhập</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhap = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongNhap += (decimal)tong[i];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "NhapDC")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có nhập điều chỉnh đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                              <th>Nhập điều chỉnh</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["NhapDC" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongNhapDC = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongNhapDC += (decimal)tong[i];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhapDC, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "Xuat")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY * 3; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0 && ((decimal)tong[k + countTTY + countTTY]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k + countTTY + countTTY);
                            tong.RemoveAt(k + countTTY);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có xuất đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                              <th>Xuất</th>
                                <th>Xuất điều chỉnh</th>
                                <th>Hủy</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rTTY["IDVatTu"].ToString()] == DBNull.Value && r["XuatDC" + rTTY["IDVatTu"].ToString()] == DBNull.Value && r["XuatH" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countTTY], c) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countTTY + countTTY], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuat = 0;
                    decimal tongXuatDC = 0;
                    decimal tongXuatH = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongXuat += (decimal)tong[i];
                        tongXuatDC += (decimal)tong[i + countTTY];
                        tongXuatH += (decimal)tong[i + countTTY + countTTY];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, c) + "</td>");
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatDC, c) + "</td>");
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatH, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "XuatBT")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có xuất đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                              <th>Xuất</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuat = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongXuat += (decimal)tong[i];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "XuatDC")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có xuất điều chỉnh đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                                <th>Xuất điều chỉnh</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatDC" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuatDC = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongXuatDC += (decimal)tong[i];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatDC, c) + "</td>");
                    sb.Append("<td></td></tr>");
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                    lblMessage.Text = "";
                }
                else if (ddlLoaiBC.SelectedValue == "XuatH")
                {
                    lblMessage.Text = "";
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                    string tieude = "";
                    string strThuoc = "";
                    string colsNhap = "";
                    string strSQL = "";
                    SqlParameter[] param;
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    tieude += "<b>BẢNG THEO DÕI HỦY THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt;
                    DataTable tblTTY;
                    int countTTY;
                    string strThuocChon = Config.GetSelectedValues(ddlThuoc);
                    if (strThuocChon.StartsWith("0, ")) strThuocChon = strThuocChon.Substring(3); strThuocChon = strThuocChon.Replace(", 0", "");
                    if (strThuocChon != "" && strThuocChon != "0, ")
                    {
                        tblTTY = new DataTable();
                        tblTTY.Columns.Add("IDVatTu", typeof(Int32));
                        tblTTY.Columns.Add("TenVatTu", typeof(String));
                        string[] arrThuoc = strThuocChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrThuoc.Length; j++)
                        {
                            strThuoc += "@" + arrThuoc[j] + "@";
                            colsNhap += ",[Nhap" + arrThuoc[j] + "]";
                            DataRow r = tblTTY.NewRow();
                            r["IDVatTu"] = int.Parse(arrThuoc[j]);
                            r["TenVatTu"] = Config.GetSelectedValue(ddlThuoc, j)[1];
                            tblTTY.Rows.Add(r);
                        }
                        if (colsNhap == "") return;
                        countTTY = tblTTY.Rows.Count;
                        colsNhap = colsNhap.Substring(1);
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet_FilterThuoc";
                        param = new SqlParameter[6];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@StrThuoc", strThuoc);
                        param[4] = new SqlParameter("@colsNhap", colsNhap);
                        param[5] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    }
                    else
                    {
                        strSQL = "QLCS_BCTK_XuatNhapThuocThuY_ChiTiet";
                        param = new SqlParameter[4];
                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                        tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select IDVatTu, (TenVatTu + ' (' + DonViTinh + ')') as TenVatTu from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by MoTa Asc");
                        countTTY = tblTTY.Rows.Count;
                    }

                    //decimal[] tong = new decimal[countTTY * 3];
                    ArrayList tong = new ArrayList();
                    for (int l = 0; l < countTTY; l++) tong.Add(new decimal(0));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        int j = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rTTY["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                    for (int k = countTTY - 1; k >= 0; k--)
                    {
                        if (((decimal)tong[k]) == 0)
                        {
                            tblTTY.Rows.RemoveAt(k);
                            tong.RemoveAt(k);
                            countTTY--;
                        }
                    }
                    if (countTTY == 0)
                    {
                        lblMessage.Text = "Không có hủy đối với thuốc được chọn!";
                        return;
                    }

                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Ngày</th><th>Thuốc</th>
                                <th>Hủy</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow r = dt.Rows[i];
                        bool DaCoDongDau = false;
                        foreach (DataRow rTTY in tblTTY.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatH" + rTTY["IDVatTu"].ToString()] == DBNull.Value) continue;
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rTTY["IDVatTu"].ToString()], c) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                    //Viet dong cuoi day du
                    if (dt.Rows.Count > 1)
                    {
                        bool DaCoDongDau = false;
                        int jE = 0;
                        foreach (DataRow rTTY in tblTTY.Rows)
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
                            sb.Append("<td>" + rTTY["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], c) + "</td>");
                            jE++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                    decimal tongXuatH = 0;
                    for (int i = 0; i < countTTY; i++)
                    {
                        tongXuatH += (decimal)tong[i];
                    }
                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatH, c) + "</td>");
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