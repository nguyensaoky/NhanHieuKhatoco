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
    public partial class r_thuoctheongay : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        System.Globalization.CultureInfo cin = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        int scale = 1;
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

            DataTable dtThuoc = csCont.LoadVatTu("TTY");
            ddlThuoc.DataSource = dtThuoc;
            ddlThuoc.DataTextField = "TenVatTu";
            ddlThuoc.DataValueField = "IDVatTu";
            ddlThuoc.DataBind();
            //ddlThuoc.Items.Insert(0, new ListItem("", "0"));

            Config.LoadKhuChuong(lstKhuChuong);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
            try
            {
                if (!Page.IsPostBack)
                {
                    txtScale.Text = ConfigurationManager.AppSettings["QLCS_VatTu_TTY_Scale"];
                    BindControls();
                }
                scale = int.Parse(txtScale.Text);
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
                if (Config.GetSelectedValues_At(lstKhuChuong) == "@@" || Config.GetSelectedValues_At(lstKhuChuong) == "")
                {
                    string filename = "ThuocTheoNgay";
                    string tieude = "";
                    string strSQL = "QLCS_BCTK_ThuocTheoNgay";
                    SqlParameter[] param = new SqlParameter[5];
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    }
                    string StrLoaiCa = "";
                    if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
                    {
                        StrLoaiCa = "@" + Config.GetSelectedValues(ddlLoaiCa).Replace(", ", "@@").Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 1);
                    }
                    string StrThuoc = "";
                    if (Config.GetSelectedValues(ddlThuoc) != "0, " && Config.GetSelectedValues(ddlThuoc) != "")
                    {
                        StrThuoc = "@" + Config.GetSelectedValues(ddlThuoc).Replace(", ", "@@").Remove(Config.GetSelectedValues(ddlThuoc).Length - 1);
                    }
                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
                    param[2] = new SqlParameter("@StrLoaiCa", StrLoaiCa);
                    param[3] = new SqlParameter("@StrThuoc", StrThuoc);
                    param[4] = new SqlParameter("@OrderBy", ddlOrderBy.SelectedValue);
                    filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                    filename = filename.Replace("/", "_");
                    tieude += "<b>BẢNG THEO DÕI THUỐC TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    Response.ContentType = "application/vnd.ms-excel";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append(@"<table border='1'>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <td>Loại Thuốc thú y</td>
                          <td>Đơn vị tính</td>
                          <td>Ngày xuất</td>
                          <td>Loại cá</td>
                          <td>Số lượng cá</td>
                          <td>Lượng thuốc</td>
                          <td>Số lượng cá Giống</td>
                          <td>Lượng thuốc Giống dùng</td>
                          <td>Số lượng cá TT</td>
                          <td>Lượng thuốc TT dùng</td>
                         </tr>");
                    decimal tong = 0;
                    decimal tongG = 0;
                    decimal tongTT = 0;
                    decimal total = 0;
                    decimal totalG = 0;
                    decimal totalTT = 0;
                    if (ddlOrderBy.SelectedValue == "Ngay")
                    {
                        DateTime currNgay = DateTime.MinValue;
                        foreach (DataRow r in dt.Rows)
                        {
                            if (currNgay != Convert.ToDateTime(r["NgayXuat"]) && currNgay != DateTime.MinValue)
                            {
                                //Viết dòng tổng
                                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                                sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                                sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                                sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                                sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                                sb.Append("</tr>");
                                //Cộng tổng vào total
                                total += tong;
                                totalG += tongG;
                                totalTT += tongTT;
                                //reset tổng
                                tong = 0;
                                tongG = 0;
                                tongTT = 0;
                            }
                            //cập nhật currNgay
                            currNgay = Convert.ToDateTime(r["NgayXuat"]);

                            int slg = 0;
                            int slc = 0;
                            if (r["SoLuongGiong"] != DBNull.Value) slg = Convert.ToInt32(r["SoLuongGiong"]);
                            if (r["SoLuongCa"] != DBNull.Value) slc = Convert.ToInt32(r["SoLuongCa"]);
                            int sltt = slc - slg;

                            decimal kl = 0;
                            decimal klg = 0;
                            decimal kltt = 0;
                            if (r["KhoiLuongGiong"] != DBNull.Value) klg = Convert.ToDecimal(r["KhoiLuongGiong"]);
                            if (r["SoLuong"] != DBNull.Value) kl = Convert.ToDecimal(r["SoLuong"]);
                            if (slg == 0) { klg = 0; kltt = kl; }
                            else if (sltt == 0) { klg = kl; kltt = 0; }
                            else { kltt = kl - klg; }

                            sb.Append("<tr>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiCa"].ToString() + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slc, 0) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(r["SoLuong"], scale) + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(klg, scale) + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(kltt, scale) + "</td>");
                            sb.Append("</tr>");

                            //cộng vào tổng
                            tong += Math.Round(Convert.ToDecimal(r["SoLuong"]), scale);
                            tongG += Math.Round(klg, scale);
                            tongTT += Math.Round(kltt, scale);
                        }
                        //Viết dòng tổng
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                        sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                        sb.Append("</tr>");
                        //Cộng tổng vào total
                        total += tong;
                        totalG += tongG;
                        totalTT += tongTT;
                        //Viết dòng total
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                        sb.Append("<td>Tổng cộng</td><td></td><td></td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(total, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalG, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalTT, scale) + "</td>");
                        sb.Append("</tr>");
                    }
                    else if (ddlOrderBy.SelectedValue == "LoaiCa")
                    {
                        string currLoaiCa = "";
                        foreach (DataRow r in dt.Rows)
                        {
                            if (currLoaiCa != r["LoaiCa"].ToString() && currLoaiCa != "")
                            {
                                //Viết dòng tổng
                                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                                sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                                sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                                sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                                sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                                sb.Append("</tr>");
                                //Cộng tổng vào total
                                total += tong;
                                totalG += tongG;
                                totalTT += tongTT;
                                //reset tổng
                                tong = 0;
                                tongG = 0;
                                tongTT = 0;
                            }
                            //cập nhật currLoaiCa
                            currLoaiCa = r["LoaiCa"].ToString();

                            int slg = 0;
                            int slc = 0;
                            if (r["SoLuongGiong"] != DBNull.Value) slg = Convert.ToInt32(r["SoLuongGiong"]);
                            if (r["SoLuongCa"] != DBNull.Value) slc = Convert.ToInt32(r["SoLuongCa"]);
                            int sltt = slc - slg;

                            decimal kl = 0;
                            decimal klg = 0;
                            decimal kltt = 0;
                            if (r["KhoiLuongGiong"] != DBNull.Value) klg = Convert.ToDecimal(r["KhoiLuongGiong"]);
                            if (r["SoLuong"] != DBNull.Value) kl = Convert.ToDecimal(r["SoLuong"]);
                            if (slg == 0) { klg = 0; kltt = kl; }
                            else if (sltt == 0) { klg = kl; kltt = 0; }
                            else { kltt = kl - klg; }

                            sb.Append("<tr>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiCa"].ToString() + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slc, 0) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(r["SoLuong"], scale) + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(klg, scale) + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(kltt, scale) + "</td>");
                            sb.Append("</tr>");

                            //cộng vào tổng
                            tong += Math.Round(Convert.ToDecimal(r["SoLuong"]), scale);
                            tongG += Math.Round(klg, scale);
                            tongTT += Math.Round(kltt, scale);
                        }
                        //Viết dòng tổng
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                        sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                        sb.Append("</tr>");
                        //Cộng tổng vào total
                        total += tong;
                        totalG += tongG;
                        totalTT += tongTT;
                        //Viết dòng total
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                        sb.Append("<td>Tổng cộng</td><td></td><td></td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(total, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalG, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalTT, scale) + "</td>");
                        sb.Append("</tr>");
                    }
                    else if (ddlOrderBy.SelectedValue == "Thuoc")
                    {
                        string currLoaiThuoc = "";
                        foreach (DataRow r in dt.Rows)
                        {
                            if (currLoaiThuoc != r["LoaiThuoc"].ToString() && currLoaiThuoc != "")
                            {
                                //Viết dòng tổng
                                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                                sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                                sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                                sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                                sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                                sb.Append("</tr>");
                                //Cộng tổng vào total
                                total += tong;
                                totalG += tongG;
                                totalTT += tongTT;
                                //reset tổng
                                tong = 0;
                                tongG = 0;
                                tongTT = 0;
                            }
                            //cập nhật currLoaiThuoc
                            currLoaiThuoc = r["LoaiThuoc"].ToString();

                            int slg = 0;
                            int slc = 0;
                            if (r["SoLuongGiong"] != DBNull.Value) slg = Convert.ToInt32(r["SoLuongGiong"]);
                            if (r["SoLuongCa"] != DBNull.Value) slc = Convert.ToInt32(r["SoLuongCa"]);
                            int sltt = slc - slg;

                            decimal kl = 0;
                            decimal klg = 0;
                            decimal kltt = 0;
                            if (r["KhoiLuongGiong"] != DBNull.Value) klg = Convert.ToDecimal(r["KhoiLuongGiong"]);
                            if (r["SoLuong"] != DBNull.Value) kl = Convert.ToDecimal(r["SoLuong"]);
                            if (slg == 0) { klg = 0; kltt = kl; }
                            else if (sltt == 0) { klg = kl; kltt = 0; }
                            else { kltt = kl - klg; }

                            sb.Append("<tr>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiCa"].ToString() + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slc, 0) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(r["SoLuong"], scale) + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(klg, scale) + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(kltt, scale) + "</td>");
                            sb.Append("</tr>");

                            //cộng vào tổng
                            tong += Math.Round(Convert.ToDecimal(r["SoLuong"]), scale);
                            tongG += Math.Round(klg, scale);
                            tongTT += Math.Round(kltt, scale);
                        }
                        //Viết dòng tổng
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                        sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                        sb.Append("</tr>");
                        //Cộng tổng vào total
                        total += tong;
                        totalG += tongG;
                        totalTT += tongTT;
                        //Viết dòng total
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                        sb.Append("<td>Tổng cộng</td><td></td><td></td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(total, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalG, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalTT, scale) + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("</table>");
                    sb.Append(Config.Footer(UserInfo.Profile.City, UserInfo.FirstName + " " + UserInfo.LastName));
                    sb.Append("</body></html>");
                    Response.Write(sb.ToString());
                }
                else
                {
                    string filename = "ThuocTheoNgay_KhuChuong";
                    string tieude = "";
                    string strSQL = "QLCS_BCTK_ThuocTheoNgay_KhuChuong";
                    SqlParameter[] param = new SqlParameter[6];
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    }
                    string StrLoaiCa = "";
                    if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
                    {
                        StrLoaiCa = "@" + Config.GetSelectedValues(ddlLoaiCa).Replace(", ", "@@").Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 1);
                    }
                    string StrThuoc = "";
                    if (Config.GetSelectedValues(ddlThuoc) != "0, " && Config.GetSelectedValues(ddlThuoc) != "")
                    {
                        StrThuoc = "@" + Config.GetSelectedValues(ddlThuoc).Replace(", ", "@@").Remove(Config.GetSelectedValues(ddlThuoc).Length - 1);
                    }
                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
                    param[2] = new SqlParameter("@StrLoaiCa", StrLoaiCa);
                    param[3] = new SqlParameter("@StrThuoc", StrThuoc);
                    string sKhuChuong = Config.GetSelectedValues_At(lstKhuChuong).StartsWith("@@") ? Config.GetSelectedValues_At(lstKhuChuong).Substring(2) : Config.GetSelectedValues_At(lstKhuChuong);
                    param[4] = new SqlParameter("@StrKhuChuong", sKhuChuong);
                    param[5] = new SqlParameter("@OrderBy", ddlOrderBy.SelectedValue);
                    DataSet ds = Config.SelectSPs(strSQL, param);
                    DataTable dt;
                    filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                    filename = filename.Replace("/", "_");
                    tieude += "<b>BẢNG THEO DÕI THUỐC TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";

                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    Response.ContentType = "application/vnd.ms-excel";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append(@"<table border='1'>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <td>Loại Thuốc thú y</td>
                          <td>Đơn vị tính</td>
                          <td>Ngày xuất</td>
                          <td>Loại cá</td>
                          <td>Số lượng cá</td>
                          <td>Lượng thuốc</td>
                          <td>Số lượng cá Giống</td>
                          <td>Lượng thuốc Giống dùng</td>
                          <td>Số lượng cá TT</td>
                          <td>Lượng thuốc TT dùng</td>
                         </tr>");
                    decimal uTotal = 0;
                    decimal uTotalG = 0;
                    decimal uTotalTT = 0;
                    string[] aKhuChuong = Config.GetSelectedTexts(lstKhuChuong).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < aKhuChuong.Length; k++)
                    {
                        dt = ds.Tables[k];
                        //Viết dòng đầu mới: Tên khu chuồng
                        sb.Append("<tr><td style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>Khu chuồng<br/>" + aKhuChuong[k] + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        decimal tong = 0;
                        decimal tongG = 0;
                        decimal tongTT = 0;
                        decimal total = 0;
                        decimal totalG = 0;
                        decimal totalTT = 0;
                        if (ddlOrderBy.SelectedValue == "Ngay")
                        {
                            DateTime currNgay = DateTime.MinValue;
                            foreach (DataRow r in dt.Rows)
                            {
                                if (currNgay != Convert.ToDateTime(r["NgayXuat"]) && currNgay != DateTime.MinValue)
                                {
                                    //Viết dòng tổng
                                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                                    sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                                    sb.Append("</tr>");
                                    //Cộng tổng vào total
                                    total += tong;
                                    totalG += tongG;
                                    totalTT += tongTT;
                                    //reset tổng
                                    tong = 0;
                                    tongG = 0;
                                    tongTT = 0;
                                }
                                //cập nhật currNgay
                                currNgay = Convert.ToDateTime(r["NgayXuat"]);

                                string[] Group = r["KhoiLuongKhuChuong"].ToString().Split(new char[] { '/' });
                                int slg = 0;
                                int slc = 0;
                                if (r["SoLuongGiong"] != DBNull.Value) slg = Convert.ToInt32(r["SoLuongGiong"]);
                                slc = int.Parse(Group[1]);
                                int sltt = slc - slg;

                                decimal kl = 0;
                                decimal klg = 0;
                                decimal kltt = 0;
                                if (r["KhoiLuongGiong"] != DBNull.Value) klg = Convert.ToDecimal(r["KhoiLuongGiong"]);
                                kl = decimal.Parse(Group[2], cin);
                                if (slg == 0) { klg = 0; kltt = kl; }
                                else if (sltt == 0) { klg = kl; kltt = 0; }
                                else { klg = Math.Round(klg, 5); kltt = kl - klg; }

                                sb.Append("<tr>");
                                sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiCa"].ToString() + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slc, 0) + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(kl, scale) + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(klg, scale) + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(kltt, scale) + "</td>");
                                sb.Append("</tr>");

                                //cộng vào tổng
                                tong += Math.Round(kl, scale);
                                tongG += Math.Round(klg, scale);
                                tongTT += Math.Round(kltt, scale);
                            }
                            //Viết dòng tổng
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                            sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                            sb.Append("</tr>");
                            //Cộng tổng vào total
                            total += tong;
                            totalG += tongG;
                            totalTT += tongTT;
                            //Viết dòng total
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                            sb.Append("<td>Tổng cộng</td><td></td><td></td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(total, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalG, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalTT, scale) + "</td>");
                            sb.Append("</tr>");
                        }
                        else if (ddlOrderBy.SelectedValue == "LoaiCa")
                        {
                            string currLoaiCa = "";
                            foreach (DataRow r in dt.Rows)
                            {
                                if (currLoaiCa != r["LoaiCa"].ToString() && currLoaiCa != "")
                                {
                                    //Viết dòng tổng
                                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                                    sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                                    sb.Append("</tr>");
                                    //Cộng tổng vào total
                                    total += tong;
                                    totalG += tongG;
                                    totalTT += tongTT;
                                    //reset tổng
                                    tong = 0;
                                    tongG = 0;
                                    tongTT = 0;
                                }
                                //cập nhật currLoaiCa
                                currLoaiCa = r["LoaiCa"].ToString();

                                string[] Group = r["KhoiLuongKhuChuong"].ToString().Split(new char[] { '/' });
                                int slg = 0;
                                int slc = 0;
                                if (r["SoLuongGiong"] != DBNull.Value) slg = Convert.ToInt32(r["SoLuongGiong"]);
                                slc = int.Parse(Group[1]);
                                int sltt = slc - slg;

                                decimal kl = 0;
                                decimal klg = 0;
                                decimal kltt = 0;
                                if (r["KhoiLuongGiong"] != DBNull.Value) klg = Convert.ToDecimal(r["KhoiLuongGiong"]);
                                kl = decimal.Parse(Group[2], cin);
                                if (slg == 0) { klg = 0; kltt = kl; }
                                else if (sltt == 0) { klg = kl; kltt = 0; }
                                else { klg = Math.Round(klg, 5); kltt = kl - klg; }

                                sb.Append("<tr>");
                                sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiCa"].ToString() + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slc, 0) + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(kl, scale) + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(klg, scale) + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(kltt, scale) + "</td>");
                                sb.Append("</tr>");

                                //cộng vào tổng
                                tong += Math.Round(kl, scale);
                                tongG += Math.Round(klg, scale);
                                tongTT += Math.Round(kltt, scale);
                            }
                            //Viết dòng tổng
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                            sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                            sb.Append("</tr>");
                            //Cộng tổng vào total
                            total += tong;
                            totalG += tongG;
                            totalTT += tongTT;
                            //Viết dòng total
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                            sb.Append("<td>Tổng cộng</td><td></td><td></td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(total, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalG, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalTT, scale) + "</td>");
                            sb.Append("</tr>");
                        }
                        else if (ddlOrderBy.SelectedValue == "Thuoc")
                        {
                            string currLoaiThuoc = "";
                            foreach (DataRow r in dt.Rows)
                            {
                                if (currLoaiThuoc != r["LoaiThuoc"].ToString() && currLoaiThuoc != "")
                                {
                                    //Viết dòng tổng
                                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                                    sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                                    sb.Append("</tr>");
                                    //Cộng tổng vào total
                                    total += tong;
                                    totalG += tongG;
                                    totalTT += tongTT;
                                    //reset tổng
                                    tong = 0;
                                    tongG = 0;
                                    tongTT = 0;
                                }
                                //cập nhật currLoaiThuoc
                                currLoaiThuoc = r["LoaiThuoc"].ToString();

                                string[] Group = r["KhoiLuongKhuChuong"].ToString().Split(new char[] { '/' });
                                int slg = 0;
                                int slc = 0;
                                if (r["SoLuongGiong"] != DBNull.Value) slg = Convert.ToInt32(r["SoLuongGiong"]);
                                slc = int.Parse(Group[1]);
                                int sltt = slc - slg;

                                decimal kl = 0;
                                decimal klg = 0;
                                decimal kltt = 0;
                                if (r["KhoiLuongGiong"] != DBNull.Value) klg = Convert.ToDecimal(r["KhoiLuongGiong"]);
                                kl = decimal.Parse(Group[2], cin);
                                if (slg == 0) { klg = 0; kltt = kl; }
                                else if (sltt == 0) { klg = kl; kltt = 0; }
                                else { klg = Math.Round(klg, 5); kltt = kl - klg; }

                                sb.Append("<tr>");
                                sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiCa"].ToString() + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slc, 0) + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(kl, scale) + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(klg, scale) + "</td>");
                                sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(kltt, scale) + "</td>");
                                sb.Append("</tr>");

                                //cộng vào tổng
                                tong += Math.Round(kl, scale);
                                tongG += Math.Round(klg, scale);
                                tongTT += Math.Round(kltt, scale);
                            }
                            //Viết dòng tổng
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                            sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                            sb.Append("</tr>");
                            //Cộng tổng vào total
                            total += tong;
                            totalG += tongG;
                            totalTT += tongTT;
                            //Viết dòng total
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                            sb.Append("<td>Tổng cộng</td><td></td><td></td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(total, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalG, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(totalTT, scale) + "</td>");
                            sb.Append("</tr>");
                        }
                        uTotal += total;
                        uTotalG += totalG;
                        uTotalTT += totalTT;
                    }
                    //Viết dòng uTotal
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                    sb.Append("<td>TỔNG CỘNG</td><td></td><td></td><td></td><td></td>");
                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(uTotal, scale) + "</td><td></td>");
                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(uTotalG, scale) + "</td><td></td>");
                    sb.Append("<td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + ";'>" + Config.ToXVal2(uTotalTT, scale) + "</td>");
                    sb.Append("</tr>");

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
                if (Config.GetSelectedValues_At(lstKhuChuong) == "@@" || Config.GetSelectedValues_At(lstKhuChuong) == "")
                {
                    string tieude = "";
                    string strSQL = "QLCS_BCTK_ThuocTheoNgay";
                    SqlParameter[] param = new SqlParameter[5];
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    }
                    string StrLoaiCa = "";
                    if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
                    {
                        StrLoaiCa = "@" + Config.GetSelectedValues(ddlLoaiCa).Replace(", ", "@@").Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 1);
                    }
                    string StrThuoc = "";
                    if (Config.GetSelectedValues(ddlThuoc) != "0, " && Config.GetSelectedValues(ddlThuoc) != "")
                    {
                        StrThuoc = "@" + Config.GetSelectedValues(ddlThuoc).Replace(", ", "@@").Remove(Config.GetSelectedValues(ddlThuoc).Length - 1);
                    }
                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
                    param[2] = new SqlParameter("@StrLoaiCa", StrLoaiCa);
                    param[3] = new SqlParameter("@StrThuoc", StrThuoc);
                    param[4] = new SqlParameter("@OrderBy", ddlOrderBy.SelectedValue);
                    tieude += "<b>BẢNG THEO DÕI THUỐC TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Loại Thuốc thú y</th>
                              <th>Đơn vị tính</th>
                              <th>Ngày xuất</th>
                              <th>Loại cá</th>
                              <th>Số lượng cá</th>
                              <th>Lượng thuốc</th>
                              <th>Số lượng cá Giống</th>
                              <th>Lượng thuốc Giống dùng</th>
                              <th>Số lượng cá TT</th>
                              <th>Lượng thuốc TT dùng</th>
                             </tr></thead><tbody>");
                    decimal tong = 0;
                    decimal tongG = 0;
                    decimal tongTT = 0;
                    decimal total = 0;
                    decimal totalG = 0;
                    decimal totalTT = 0;
                    if (ddlOrderBy.SelectedValue == "Ngay")
                    {
                        DateTime currNgay = DateTime.MinValue;
                        foreach (DataRow r in dt.Rows)
                        {
                            if (currNgay != Convert.ToDateTime(r["NgayXuat"]) && currNgay != DateTime.MinValue)
                            {
                                //Viết dòng tổng
                                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                                sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                                sb.Append("</tr>");
                                //Cộng tổng vào total
                                total += tong;
                                totalG += tongG;
                                totalTT += tongTT;
                                //reset tổng
                                tong = 0;
                                tongG = 0;
                                tongTT = 0;
                            }
                            //cập nhật currNgay
                            currNgay = Convert.ToDateTime(r["NgayXuat"]);

                            int slg = 0;
                            int slc = 0;
                            if (r["SoLuongGiong"] != DBNull.Value) slg = Convert.ToInt32(r["SoLuongGiong"]);
                            if (r["SoLuongCa"] != DBNull.Value) slc = Convert.ToInt32(r["SoLuongCa"]);
                            int sltt = slc - slg;

                            decimal kl = 0;
                            decimal klg = 0;
                            decimal kltt = 0;
                            if (r["KhoiLuongGiong"] != DBNull.Value) klg = Convert.ToDecimal(r["KhoiLuongGiong"]);
                            if (r["SoLuong"] != DBNull.Value) kl = Convert.ToDecimal(r["SoLuong"]);
                            if (slg == 0) { klg = 0; kltt = kl; }
                            else if (sltt == 0) { klg = kl; kltt = 0; }
                            else { kltt = kl - klg; }

                            sb.Append("<tr>");
                            sb.Append("<td>" + r["LoaiThuoc"].ToString() + "</td>");
                            sb.Append("<td>" + r["DonViTinh"].ToString() + "</td>");
                            sb.Append("<td style='text-align:center;'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                            sb.Append("<td>" + r["LoaiCa"].ToString() + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(slc, 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["SoLuong"], scale) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(klg, scale) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(kltt, scale) + "</td>");
                            sb.Append("</tr>");

                            //cộng vào tổng
                            tong += Math.Round(Convert.ToDecimal(r["SoLuong"]), scale);
                            tongG += Math.Round(klg, scale);
                            tongTT += Math.Round(kltt, scale);
                        }
                        //Viết dòng tổng
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                        sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                        sb.Append("</tr>");
                        //Cộng tổng vào total
                        total += tong;
                        totalG += tongG;
                        totalTT += tongTT;
                        //Viết dòng total
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                        sb.Append("<td>Tổng cộng</td><td></td><td></td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(total, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(totalG, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(totalTT, scale) + "</td>");
                        sb.Append("</tr>");
                    }
                    else if (ddlOrderBy.SelectedValue == "LoaiCa")
                    {
                        string currLoaiCa = "";
                        foreach (DataRow r in dt.Rows)
                        {
                            if (currLoaiCa != r["LoaiCa"].ToString() && currLoaiCa != "")
                            {
                                //Viết dòng tổng
                                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                                sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                                sb.Append("</tr>");
                                //Cộng tổng vào total
                                total += tong;
                                totalG += tongG;
                                totalTT += tongTT;
                                //reset tổng
                                tong = 0;
                                tongG = 0;
                                tongTT = 0;
                            }
                            //cập nhật currLoaiCa
                            currLoaiCa = r["LoaiCa"].ToString();

                            int slg = 0;
                            int slc = 0;
                            if (r["SoLuongGiong"] != DBNull.Value) slg = Convert.ToInt32(r["SoLuongGiong"]);
                            if (r["SoLuongCa"] != DBNull.Value) slc = Convert.ToInt32(r["SoLuongCa"]);
                            int sltt = slc - slg;

                            decimal kl = 0;
                            decimal klg = 0;
                            decimal kltt = 0;
                            if (r["KhoiLuongGiong"] != DBNull.Value) klg = Convert.ToDecimal(r["KhoiLuongGiong"]);
                            if (r["SoLuong"] != DBNull.Value) kl = Convert.ToDecimal(r["SoLuong"]);
                            if (slg == 0) { klg = 0; kltt = kl; }
                            else if (sltt == 0) { klg = kl; kltt = 0; }
                            else { kltt = kl - klg; }

                            sb.Append("<tr>");
                            sb.Append("<td>" + r["LoaiThuoc"].ToString() + "</td>");
                            sb.Append("<td>" + r["DonViTinh"].ToString() + "</td>");
                            sb.Append("<td style='text-align:center;'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                            sb.Append("<td>" + r["LoaiCa"].ToString() + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(slc, 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["SoLuong"], scale) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(klg, scale) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(kltt, scale) + "</td>");
                            sb.Append("</tr>");

                            //cộng vào tổng
                            tong += Math.Round(Convert.ToDecimal(r["SoLuong"]), scale);
                            tongG += Math.Round(klg, scale);
                            tongTT += Math.Round(kltt, scale);
                        }
                        //Viết dòng tổng
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                        sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                        sb.Append("</tr>");
                        //Cộng tổng vào total
                        total += tong;
                        totalG += tongG;
                        totalTT += tongTT;
                        //Viết dòng total
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                        sb.Append("<td>Tổng cộng</td><td></td><td></td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(total, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(totalG, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(totalTT, scale) + "</td>");
                        sb.Append("</tr>");
                    }
                    else if (ddlOrderBy.SelectedValue == "Thuoc")
                    {
                        string currThuoc = "";
                        foreach (DataRow r in dt.Rows)
                        {
                            if (currThuoc != r["LoaiThuoc"].ToString() && currThuoc != "")
                            {
                                //Viết dòng tổng
                                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                                sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                                sb.Append("</tr>");
                                //Cộng tổng vào total
                                total += tong;
                                totalG += tongG;
                                totalTT += tongTT;
                                //reset tổng
                                tong = 0;
                                tongG = 0;
                                tongTT = 0;
                            }
                            //cập nhật currThuoc
                            currThuoc = r["LoaiThuoc"].ToString();

                            int slg = 0;
                            int slc = 0;
                            if (r["SoLuongGiong"] != DBNull.Value) slg = Convert.ToInt32(r["SoLuongGiong"]);
                            if (r["SoLuongCa"] != DBNull.Value) slc = Convert.ToInt32(r["SoLuongCa"]);
                            int sltt = slc - slg;

                            decimal kl = 0;
                            decimal klg = 0;
                            decimal kltt = 0;
                            if (r["KhoiLuongGiong"] != DBNull.Value) klg = Convert.ToDecimal(r["KhoiLuongGiong"]);
                            if (r["SoLuong"] != DBNull.Value) kl = Convert.ToDecimal(r["SoLuong"]);
                            if (slg == 0) { klg = 0; kltt = kl; }
                            else if (sltt == 0) { klg = kl; kltt = 0; }
                            else { kltt = kl - klg; }

                            sb.Append("<tr>");
                            sb.Append("<td>" + r["LoaiThuoc"].ToString() + "</td>");
                            sb.Append("<td>" + r["DonViTinh"].ToString() + "</td>");
                            sb.Append("<td style='text-align:center;'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                            sb.Append("<td>" + r["LoaiCa"].ToString() + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(slc, 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(r["SoLuong"], scale) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(klg, scale) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(kltt, scale) + "</td>");
                            sb.Append("</tr>");

                            //cộng vào tổng
                            tong += Math.Round(Convert.ToDecimal(r["SoLuong"]), scale);
                            tongG += Math.Round(klg, scale);
                            tongTT += Math.Round(kltt, scale);
                        }
                        //Viết dòng tổng
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                        sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                        sb.Append("</tr>");
                        //Cộng tổng vào total
                        total += tong;
                        totalG += tongG;
                        totalTT += tongTT;
                        //Viết dòng total
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                        sb.Append("<td>Tổng cộng</td><td></td><td></td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(total, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(totalG, scale) + "</td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(totalTT, scale) + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                }
                else
                {
                    string tieude = "";
                    string strSQL = "QLCS_BCTK_ThuocTheoNgay_KhuChuong";
                    SqlParameter[] param = new SqlParameter[6];
                    if (txtFromDate.Text == "")
                    {
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtToDate.Text == "")
                    {
                        txtToDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    }
                    string StrLoaiCa = "";
                    if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
                    {
                        StrLoaiCa = "@" + Config.GetSelectedValues(ddlLoaiCa).Replace(", ", "@@").Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 1);
                    }
                    string StrThuoc = "";
                    if (Config.GetSelectedValues(ddlThuoc) != "0, " && Config.GetSelectedValues(ddlThuoc) != "")
                    {
                        StrThuoc = "@" + Config.GetSelectedValues(ddlThuoc).Replace(", ", "@@").Remove(Config.GetSelectedValues(ddlThuoc).Length - 1);
                    }
                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
                    param[2] = new SqlParameter("@StrLoaiCa", StrLoaiCa);
                    param[3] = new SqlParameter("@StrThuoc", StrThuoc);
                    string sKhuChuong = Config.GetSelectedValues_At(lstKhuChuong).StartsWith("@@") ? Config.GetSelectedValues_At(lstKhuChuong).Substring(2) : Config.GetSelectedValues_At(lstKhuChuong);
                    param[4] = new SqlParameter("@StrKhuChuong", sKhuChuong);
                    param[5] = new SqlParameter("@OrderBy", ddlOrderBy.SelectedValue);
                    DataSet ds = Config.SelectSPs(strSQL, param);
                    DataTable dt;
                    tieude += "<b>BẢNG THEO DÕI THUỐC TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                    sb.Append(@"<tr>
                              <th>Loại Thuốc thú y</th>
                              <th>Đơn vị tính</th>
                              <th>Ngày xuất</th>
                              <th>Loại cá</th>
                              <th>Số lượng cá</th>
                              <th>Lượng thuốc</th>
                              <th>Số lượng cá Giống</th>
                              <th>Lượng thuốc Giống dùng</th>
                              <th>Số lượng cá TT</th>
                              <th>Lượng thuốc TT dùng</th>
                             </tr></thead><tbody>");
                    decimal uTotal = 0;
                    decimal uTotalG = 0;
                    decimal uTotalTT = 0;
                    string[] aKhuChuong = Config.GetSelectedTexts(lstKhuChuong).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < aKhuChuong.Length; k++)
                    {
                        dt = ds.Tables[k];
                        //Viết dòng đầu mới: Tên khu chuồng
                        sb.Append("<tr><td style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>Khu chuồng<br/>" + aKhuChuong[k] + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        decimal tong = 0;
                        decimal tongG = 0;
                        decimal tongTT = 0;
                        decimal total = 0;
                        decimal totalG = 0;
                        decimal totalTT = 0;
                        if (ddlOrderBy.SelectedValue == "Ngay")
                        {
                            DateTime currNgay = DateTime.MinValue;
                            foreach (DataRow r in dt.Rows)
                            {
                                if (currNgay != Convert.ToDateTime(r["NgayXuat"]) && currNgay != DateTime.MinValue)
                                {
                                    //Viết dòng tổng
                                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                                    sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                                    sb.Append("</tr>");
                                    //Cộng tổng vào total
                                    total += tong;
                                    totalG += tongG;
                                    totalTT += tongTT;
                                    //reset tổng
                                    tong = 0;
                                    tongG = 0;
                                    tongTT = 0;
                                }
                                //cập nhật currNgay
                                currNgay = Convert.ToDateTime(r["NgayXuat"]);

                                string[] Group = r["KhoiLuongKhuChuong"].ToString().Split(new char[] { '/' });
                                int slg = 0;
                                int slc = 0;
                                if (r["SoLuongGiong"] != DBNull.Value) slg = Convert.ToInt32(r["SoLuongGiong"]);
                                slc = int.Parse(Group[1]);
                                int sltt = slc - slg;

                                decimal kl = 0;
                                decimal klg = 0;
                                decimal kltt = 0;
                                if (r["KhoiLuongGiong"] != DBNull.Value) klg = Convert.ToDecimal(r["KhoiLuongGiong"]);
                                kl = decimal.Parse(Group[2], cin);
                                if (slg == 0) { klg = 0; kltt = kl; }
                                else if (sltt == 0) { klg = kl; kltt = 0; }
                                else { klg = Math.Round(klg, 5); kltt = kl - klg; }

                                sb.Append("<tr>");
                                sb.Append("<td>" + r["LoaiThuoc"].ToString() + "</td>");
                                sb.Append("<td>" + r["DonViTinh"].ToString() + "</td>");
                                sb.Append("<td style='text-align:center;'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                                sb.Append("<td>" + r["LoaiCa"].ToString() + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(slc, 0) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(kl, scale) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(klg, scale) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(kltt, scale) + "</td>");
                                sb.Append("</tr>");

                                //cộng vào tổng
                                tong += Math.Round(kl, scale);
                                tongG += Math.Round(klg, scale);
                                tongTT += Math.Round(kltt, scale);
                            }
                            //Viết dòng tổng
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                            sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                            sb.Append("</tr>");
                            //Cộng tổng vào total
                            total += tong;
                            totalG += tongG;
                            totalTT += tongTT;
                            //Viết dòng total
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                            sb.Append("<td>Tổng cộng</td><td></td><td></td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(total, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(totalG, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(totalTT, scale) + "</td>");
                            sb.Append("</tr>");
                        }
                        else if (ddlOrderBy.SelectedValue == "LoaiCa")
                        {
                            string currLoaiCa = "";
                            foreach (DataRow r in dt.Rows)
                            {
                                if (currLoaiCa != r["LoaiCa"].ToString() && currLoaiCa != "")
                                {
                                    //Viết dòng tổng
                                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                                    sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                                    sb.Append("</tr>");
                                    //Cộng tổng vào total
                                    total += tong;
                                    totalG += tongG;
                                    totalTT += tongTT;
                                    //reset tổng
                                    tong = 0;
                                    tongG = 0;
                                    tongTT = 0;
                                }
                                //cập nhật currLoaiCa
                                currLoaiCa = r["LoaiCa"].ToString();

                                string[] Group = r["KhoiLuongKhuChuong"].ToString().Split(new char[] { '/' });
                                int slg = 0;
                                int slc = 0;
                                if (r["SoLuongGiong"] != DBNull.Value) slg = Convert.ToInt32(r["SoLuongGiong"]);
                                slc = int.Parse(Group[1]);
                                int sltt = slc - slg;

                                decimal kl = 0;
                                decimal klg = 0;
                                decimal kltt = 0;
                                if (r["KhoiLuongGiong"] != DBNull.Value) klg = Convert.ToDecimal(r["KhoiLuongGiong"]);
                                kl = decimal.Parse(Group[2], cin);
                                if (slg == 0) { klg = 0; kltt = kl; }
                                else if (sltt == 0) { klg = kl; kltt = 0; }
                                else { klg = Math.Round(klg, 5); kltt = kl - klg; }

                                sb.Append("<tr>");
                                sb.Append("<td>" + r["LoaiThuoc"].ToString() + "</td>");
                                sb.Append("<td>" + r["DonViTinh"].ToString() + "</td>");
                                sb.Append("<td style='text-align:center;'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                                sb.Append("<td>" + r["LoaiCa"].ToString() + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(slc, 0) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(kl, scale) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(klg, scale) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(kltt, scale) + "</td>");
                                sb.Append("</tr>");

                                //cộng vào tổng
                                tong += Math.Round(kl, scale);
                                tongG += Math.Round(klg, scale);
                                tongTT += Math.Round(kltt, scale);
                            }
                            //Viết dòng tổng
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                            sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                            sb.Append("</tr>");
                            //Cộng tổng vào total
                            total += tong;
                            totalG += tongG;
                            totalTT += tongTT;
                            //Viết dòng total
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                            sb.Append("<td>Tổng cộng</td><td></td><td></td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(total, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(totalG, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(totalTT, scale) + "</td>");
                            sb.Append("</tr>");
                        }
                        else if (ddlOrderBy.SelectedValue == "Thuoc")
                        {
                            string currThuoc = "";
                            foreach (DataRow r in dt.Rows)
                            {
                                if (currThuoc != r["LoaiThuoc"].ToString() && currThuoc != "")
                                {
                                    //Viết dòng tổng
                                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                                    sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                                    sb.Append("</tr>");
                                    //Cộng tổng vào total
                                    total += tong;
                                    totalG += tongG;
                                    totalTT += tongTT;
                                    //reset tổng
                                    tong = 0;
                                    tongG = 0;
                                    tongTT = 0;
                                }
                                //cập nhật currThuoc
                                currThuoc = r["LoaiThuoc"].ToString();

                                string[] Group = r["KhoiLuongKhuChuong"].ToString().Split(new char[] { '/' });
                                int slg = 0;
                                int slc = 0;
                                if (r["SoLuongGiong"] != DBNull.Value) slg = Convert.ToInt32(r["SoLuongGiong"]);
                                slc = int.Parse(Group[1]);
                                int sltt = slc - slg;

                                decimal kl = 0;
                                decimal klg = 0;
                                decimal kltt = 0;
                                if (r["KhoiLuongGiong"] != DBNull.Value) klg = Convert.ToDecimal(r["KhoiLuongGiong"]);
                                kl = decimal.Parse(Group[2], cin);
                                if (slg == 0) { klg = 0; kltt = kl; }
                                else if (sltt == 0) { klg = kl; kltt = 0; }
                                else { klg = Math.Round(klg, 5); kltt = kl - klg; }

                                sb.Append("<tr>");
                                sb.Append("<td>" + r["LoaiThuoc"].ToString() + "</td>");
                                sb.Append("<td>" + r["DonViTinh"].ToString() + "</td>");
                                sb.Append("<td style='text-align:center;'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                                sb.Append("<td>" + r["LoaiCa"].ToString() + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(slc, 0) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(kl, scale) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(klg, scale) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(kltt, scale) + "</td>");
                                sb.Append("</tr>");

                                //cộng vào tổng
                                tong += Math.Round(kl, scale);
                                tongG += Math.Round(klg, scale);
                                tongTT += Math.Round(kltt, scale);
                            }
                            //Viết dòng tổng
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                            sb.Append("<td>T.C</td><td></td><td></td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tong, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongG, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(tongTT, scale) + "</td>");
                            sb.Append("</tr>");
                            //Cộng tổng vào total
                            total += tong;
                            totalG += tongG;
                            totalTT += tongTT;
                            //Viết dòng total
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                            sb.Append("<td>Tổng cộng</td><td></td><td></td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(total, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(totalG, scale) + "</td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(totalTT, scale) + "</td>");
                            sb.Append("</tr>");
                        }
                        uTotal += total;
                        uTotalG += totalG;
                        uTotalTT += totalTT;
                    }
                    //Viết dòng uTotal
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'>");
                    sb.Append("<td>TỔNG CỘNG</td><td></td><td></td><td></td><td></td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(uTotal, scale) + "</td><td></td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(uTotalG, scale) + "</td><td></td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(uTotalTT, scale) + "</td>");
                    sb.Append("</tr>");
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