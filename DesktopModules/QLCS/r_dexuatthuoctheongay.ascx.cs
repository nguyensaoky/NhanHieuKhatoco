﻿using System;
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
    public partial class r_dexuatthuoctheongay : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo cin = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        int scaleTA = 0;
        int scaleTTY = 0;
        private void BindControls()
        {
            txtTuNgay.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            
            DataTable tblLoaiCa = new DataTable();
            tblLoaiCa = csCont.LoadLoaiCa(1);
            lstLoaiCa.DataSource = tblLoaiCa;
            lstLoaiCa.DataTextField = "TenLoaiCa";
            lstLoaiCa.DataValueField = "IDLoaiCa";
            lstLoaiCa.DataBind();

            Config.LoadKhuChuong(lstKhuChuong);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                scaleTA = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_TA_Scale"]);
                scaleTTY = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_TTY_Scale"]);
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
                string filename = "DeXuatThuocTheoNgay";
                filename += txtTuNgay.Text + "___" + txtDenNgay.Text + ".xls";
                filename = filename.Replace("/", "_");
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                if (Config.GetSelectedValues_At(lstKhuChuong) == "@@" || Config.GetSelectedValues_At(lstKhuChuong) == "")
                {
                    DateTime currngayxuat = DateTime.MinValue;
                    string tieude = "ĐỀ XUẤT THUỐC THEO NGÀY";
                    string strSQL = "QLCS_BCTK_DeXuatThuocTheoNgay";
                    SqlParameter[] param = new SqlParameter[3];
                    if (txtTuNgay.Text == "")
                    {
                        txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtDenNgay.Text == "")
                    {
                        txtDenNgay.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    }
                    param[0] = new SqlParameter("@dFrom", txtTuNgay.Text);
                    param[1] = new SqlParameter("@dTo", txtDenNgay.Text);
                    param[2] = new SqlParameter("@StrLoaiCa", Config.GetSelectedValues_At(lstLoaiCa));
                    DataTable dt = Config.SelectSP(strSQL, param);

                    string tenLoaiCa = "";
                    int STT = 1;
                    decimal KLG = 0;
                    decimal KLTT = 0;
                    decimal KLGTA = 0;
                    decimal KLTTTA = 0;
                    int currLoaiCa = 0;
                    decimal TKLG = 0;
                    decimal TKLTT = 0;
                    decimal TKLGTA = 0;
                    decimal TKLTTTA = 0;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><tr><th colspan='8'><b>GIỐNG</b></th><th></th><th colspan='8'><b>TĂNG TRỌNG</b></th></tr>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <td>STT</td>
                              <td>Ngày</td>
                              <td>Số con</td>
                              <td>Tên<br/>NVL</td>
                              <td>ĐVT</td>
                              <td>Thực xuất</td>
                              <td>Thức ăn<br/>xuất (kg)</td>
                              <td>Ghi chú</td>
                              <td>&nbsp;</td>
                              <td>STT</td>
                              <td>Ngày</td>
                              <td>Số con</td>
                              <td>Tên<br/>NVL</td>
                              <td>ĐVT</td>
                              <td>Thực xuất</td>
                              <td>Thức ăn<br/>xuất (kg)</td>
                              <td>Ghi chú</td>
                             </tr>");
                    foreach (DataRow r in dt.Rows)
                    {
                        if (currLoaiCa != Convert.ToInt32(r["LoaiCa"]))
                        {
                            tenLoaiCa = Config.GetTextByValue(lstLoaiCa, Convert.ToString(r["LoaiCa"]));
                            if (currLoaiCa != 0)
                            {
                                //Viết dòng tổng
                                sb.Append("<tr style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>");
                                sb.Append("<td></td><td style='vertical-align:middle;'>T.C</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(KLG, scaleTTY) + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(KLGTA, scaleTA) + "</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("<td></td><td style='vertical-align:middle;'>T.C</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(KLTT, scaleTTY) + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(KLTTTA, scaleTA) + "</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("</tr>");
                            }
                            //Viết dòng đầu mới: Tên loại cá
                            sb.Append("<tr><td></td><td style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>" + tenLoaiCa + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='font-weight:bold;text-align:center;color:#F00;'>" + tenLoaiCa + "</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                            //Cộng tổng cuối
                            TKLG += KLG;
                            TKLTT += KLTT;
                            TKLGTA += KLGTA;
                            TKLTTTA += KLTTTA;
                            //Reset
                            KLG = 0;
                            KLTT = 0;
                            KLGTA = 0;
                            KLTTTA = 0;
                            STT = 1;
                            currLoaiCa = Convert.ToInt32(r["LoaiCa"]);
                            currngayxuat = DateTime.MinValue;
                        }
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
                        else { klg = Math.Round(klg, 5); kltt = kl - klg; }

                        decimal klta = 0;
                        decimal klgta = 0;
                        decimal klttta = 0;
                        if (r["KhoiLuongThucAnTT"] != DBNull.Value) klttta = Convert.ToDecimal(r["KhoiLuongThucAnTT"]);
                        if (r["KhoiLuongThucAn"] != DBNull.Value) klta = Convert.ToDecimal(r["KhoiLuongThucAn"]);
                        if (slg == 0) { klgta = 0; klttta = klta; }
                        else if (sltt == 0) { klgta = klta; klttta = 0; }
                        else { klttta = Math.Round(klttta, 5); klgta = klta - klttta; }

                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>" + STT.ToString() + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(klg, scaleTTY) + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(klgta, scaleTA) + "</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>" + STT.ToString() + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(kltt, scaleTTY) + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(klttta, scaleTA) + "</td>");//
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("</tr>");
                        
                        STT++;
                        KLG += Math.Round(klg, scaleTTY);
                        KLTT += Math.Round(kltt, scaleTTY);

                        if (currngayxuat != Convert.ToDateTime(r["NgayXuat"]))
                        {
                            KLGTA += Math.Round(klgta, scaleTA);
                            KLTTTA += Math.Round(klttta, scaleTA);
                            currngayxuat = Convert.ToDateTime(r["NgayXuat"]);
                        }
                    }
                    if (currLoaiCa != 0)
                    {
                        //Viết dòng tổng
                        sb.Append("<tr style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>");
                        sb.Append("<td></td><td style='vertical-align:middle;'>T.C</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(KLG, scaleTTY) + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(KLGTA, scaleTA) + "</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td></td><td style='vertical-align:middle;'>T.C</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(KLTT, scaleTTY) + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(KLTTTA, scaleTA) + "</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("</tr>");
                        //Cộng tổng cuối
                        TKLG += KLG;
                        TKLTT += KLTT;
                        TKLGTA += KLGTA;
                        TKLTTTA += KLTTTA;
                        //Viết dòng Tổng cuối
                        sb.Append("<tr style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>");
                        sb.Append("<td></td><td style='vertical-align:middle;'>Tổng cộng</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(TKLG, scaleTTY) + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(TKLGTA, scaleTA) + "</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td></td><td style='vertical-align:middle;'>Tổng cộng</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(TKLTT, scaleTTY) + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(TKLTTTA, scaleTA) + "</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                    sb.Append(Config.Footer(UserInfo.Profile.City, UserInfo.FirstName + " " + UserInfo.LastName));
                    sb.Append("</body></html>");
                    Response.Write(sb.ToString());
                }
                else
                {
                    DateTime currngayxuat = DateTime.MinValue;
                    string tieude = "ĐỀ XUẤT THUỐC THEO NGÀY";
                    string strSQL = "QLCS_BCTK_DeXuatThuocTheoNgay_KhuChuong";
                    SqlParameter[] param = new SqlParameter[4];
                    if (txtTuNgay.Text == "")
                    {
                        txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtDenNgay.Text == "")
                    {
                        txtDenNgay.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    }
                    param[0] = new SqlParameter("@dFrom", txtTuNgay.Text);
                    param[1] = new SqlParameter("@dTo", txtDenNgay.Text);
                    param[2] = new SqlParameter("@StrLoaiCa", Config.GetSelectedValues_At(lstLoaiCa));
                    string sKhuChuong = Config.GetSelectedValues_At(lstKhuChuong).StartsWith("@@") ? Config.GetSelectedValues_At(lstKhuChuong).Substring(2) : Config.GetSelectedValues_At(lstKhuChuong);
                    param[3] = new SqlParameter("@StrKhuChuong", sKhuChuong);
                    DataSet ds = Config.SelectSPs(strSQL, param);
                    DataTable dt;

                    string tenLoaiCa = "";
                    int STT = 1;
                    decimal KLG = 0;
                    decimal KLTT = 0;
                    decimal KLGTA = 0;
                    decimal KLTTTA = 0;
                    int currLoaiCa = 0;
                    decimal TKLG = 0;
                    decimal TKLTT = 0;
                    decimal TKLGTA = 0;
                    decimal TKLTTTA = 0;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append("<table border='1'><tr><th colspan='8'><b>GIỐNG</b></th><th></th><th colspan='8'><b>TĂNG TRỌNG</b></th></tr>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <td>STT</td>
                              <td>Ngày</td>
                              <td>Số con</td>
                              <td>Tên<br/>NVL</td>
                              <td>ĐVT</td>
                              <td>Thực xuất</td>
                              <td>Thức ăn<br/>xuất (kg)</td>
                              <td>Ghi chú</td>
                              <td>&nbsp;</td>
                              <td>STT</td>
                              <td>Ngày</td>
                              <td>Số con</td>
                              <td>Tên<br/>NVL</td>
                              <td>ĐVT</td>
                              <td>Thực xuất</td>
                              <td>Thức ăn<br/>xuất (kg)</td>
                              <td>Ghi chú</td>
                             </tr>");
                    string[] aKhuChuong = Config.GetSelectedTexts(lstKhuChuong).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < aKhuChuong.Length; k++)
                    {
                        TKLG = 0;
                        TKLTT = 0;
                        TKLGTA = 0;
                        TKLTTTA = 0;
                        KLG = 0;
                        KLTT = 0;
                        KLGTA = 0;
                        KLTTTA = 0;
                        STT = 1;
                        currLoaiCa = 0;
                        currngayxuat = DateTime.MinValue;
                        dt = ds.Tables[k];
                        //Viết dòng đầu mới: Tên khu chuồng
                        sb.Append("<tr><td style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>Khu chuồng<br/>" + aKhuChuong[k] + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='font-weight:bold;text-align:center;color:#F00;'>Khu chuồng<br/>" + aKhuChuong[k] + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        foreach (DataRow r in dt.Rows)
                        {
                            if (currLoaiCa != Convert.ToInt32(r["LoaiCa"]))
                            {
                                tenLoaiCa = Config.GetTextByValue(lstLoaiCa, Convert.ToString(r["LoaiCa"]));
                                if (currLoaiCa != 0)
                                {
                                    //Viết dòng tổng
                                    sb.Append("<tr style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>");
                                    sb.Append("<td></td><td style='vertical-align:middle;'>T.C</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(KLG, scaleTTY) + "</td>");
                                    sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(KLGTA, scaleTA) + "</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("<td></td><td style='vertical-align:middle;'>T.C</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(KLTT, scaleTTY) + "</td>");
                                    sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(KLTTTA, scaleTA) + "</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("</tr>");
                                }
                                //Viết dòng đầu mới: Tên loại cá
                                sb.Append("<tr><td></td><td style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>" + tenLoaiCa + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='font-weight:bold;text-align:center;color:#F00;'>" + tenLoaiCa + "</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                                //Cộng tổng cuối
                                TKLG += KLG;
                                TKLTT += KLTT;
                                TKLGTA += KLGTA;
                                TKLTTTA += KLTTTA;
                                //Reset
                                KLG = 0;
                                KLTT = 0;
                                KLGTA = 0;
                                KLTTTA = 0;
                                STT = 1;
                                currLoaiCa = Convert.ToInt32(r["LoaiCa"]);
                                currngayxuat = DateTime.MinValue;
                            }
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

                            decimal klta = 0;
                            decimal klgta = 0;
                            decimal klttta = 0;
                            if (r["KhoiLuongThucAnTT"] != DBNull.Value) klttta = Convert.ToDecimal(r["KhoiLuongThucAnTT"]);
                            if (r["KhoiLuongThucAn"] != DBNull.Value) klta = Convert.ToDecimal(r["KhoiLuongThucAn"]);
                            if (slg == 0) { klgta = 0; klttta = klta; }
                            else if (sltt == 0) { klgta = klta; klttta = 0; }
                            else { klttta = Math.Round(klttta, 5); klgta = klta - klttta; }

                            sb.Append("<tr>");
                            sb.Append("<td style='vertical-align:middle;'>" + STT.ToString() + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(klg, scaleTTY) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(klgta, scaleTA) + "</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>" + STT.ToString() + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;mso-number-format:""dd\\/mm\\/yyyy"";'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(kltt, scaleTTY) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(klttta, scaleTA) + "</td>");//
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("</tr>");

                            STT++;
                            KLG += Math.Round(klg, scaleTTY);
                            KLTT += Math.Round(kltt, scaleTTY);
                            if (currngayxuat != Convert.ToDateTime(r["NgayXuat"]))
                            {
                                KLGTA += Math.Round(klgta, scaleTA);
                                KLTTTA += Math.Round(klttta, scaleTA);
                                currngayxuat = Convert.ToDateTime(r["NgayXuat"]);
                            }
                        }
                        if (currLoaiCa != 0)
                        {
                            //Viết dòng tổng
                            sb.Append("<tr style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>");
                            sb.Append("<td></td><td style='vertical-align:middle;'>T.C</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(KLG, scaleTTY) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(KLGTA, scaleTA) + "</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td></td><td style='vertical-align:middle;'>T.C</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(KLTT, scaleTTY) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(KLTTTA, scaleTA) + "</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("</tr>");
                            //Cộng tổng cuối
                            TKLG += KLG;
                            TKLTT += KLTT;
                            TKLGTA += KLGTA;
                            TKLTTTA += KLTTTA;
                            //Viết dòng Tổng cuối
                            sb.Append("<tr style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>");
                            sb.Append("<td></td><td style='vertical-align:middle;'>Tổng cộng</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(TKLG, scaleTTY) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(TKLGTA, scaleTA) + "</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td></td><td style='vertical-align:middle;'>Tổng cộng</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTTY) + ";'>" + Config.ToXVal2(TKLTT, scaleTTY) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;mso-number-format:" + Config.ExcelFormat(scaleTA) + ";'>" + Config.ToXVal2(TKLTTTA, scaleTA) + "</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("</tr>");
                        }
                    }
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
                    DateTime currngayxuat = DateTime.MinValue;
                    string tieude = "ĐỀ XUẤT THUỐC THEO NGÀY";
                    string strSQL = "QLCS_BCTK_DeXuatThuocTheoNgay";
                    SqlParameter[] param = new SqlParameter[3];
                    if (txtTuNgay.Text == "")
                    {
                        txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtDenNgay.Text == "")
                    {
                        txtDenNgay.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    }
                    param[0] = new SqlParameter("@dFrom", txtTuNgay.Text);
                    param[1] = new SqlParameter("@dTo", txtDenNgay.Text);
                    param[2] = new SqlParameter("@StrLoaiCa", Config.GetSelectedValues_At(lstLoaiCa));
                    DataTable dt = Config.SelectSP(strSQL, param);

                    string tenLoaiCa = "";
                    int STT = 1;
                    decimal KLG = 0;
                    decimal KLTT = 0;
                    decimal KLGTA = 0;
                    decimal KLTTTA = 0;
                    int currLoaiCa = 0;
                    decimal TKLG = 0;
                    decimal TKLTT = 0;
                    decimal TKLGTA = 0;
                    decimal TKLTTTA = 0;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append(@"<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>
                        <tr><th colspan='8'><b>GIỐNG</b></th><th></th><th colspan='8'><b>TĂNG TRỌNG</b></th></tr>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>STT</th>
                              <th>Ngày</th>
                              <th>Số con</th>
                              <th>Tên<br/>NVL</th>
                              <th>ĐVT</th>
                              <th>Thực xuất</th>
                              <th>Thức ăn<br/>xuất (kg)</th>
                              <th>Ghi chú</th>
                              <th>&nbsp;</th>
                              <th>STT</th>
                              <th>Ngày</th>
                              <th>Số con</th>
                              <th>Tên<br/>NVL</th>
                              <th>ĐVT</th>
                              <th>Thực xuất</th>
                              <th>Thức ăn<br/>xuất (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    foreach (DataRow r in dt.Rows)
                    {
                        if (currLoaiCa != Convert.ToInt32(r["LoaiCa"]))
                        {
                            tenLoaiCa = Config.GetTextByValue(lstLoaiCa, Convert.ToString(r["LoaiCa"]));
                            if (currLoaiCa != 0)
                            {
                                //Viết dòng tổng
                                sb.Append("<tr>");
                                sb.Append("<td></td><td style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>T.C</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLG, scaleTTY) + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLGTA, scaleTA) + "</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("<td></td><td style='vertical-align:middle;font-weight:bold;color:#F00;text-align:center;'>T.C</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLTT, scaleTTY) + "</td>");
                                sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLTTTA, scaleTA) + "</td>");
                                sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                sb.Append("</tr>");
                            }
                            //Viết dòng đầu mới: Tên loại cá
                            sb.Append("<tr><td></td><td style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>" + tenLoaiCa + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='font-weight:bold;text-align:center;color:#F00;'>" + tenLoaiCa + "</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                            //Cộng tổng cuối
                            TKLG += KLG;
                            TKLTT += KLTT;
                            TKLGTA += KLGTA;
                            TKLTTTA += KLTTTA;
                            //Reset
                            KLG = 0;
                            KLTT = 0;
                            KLGTA = 0;
                            KLTTTA = 0;
                            STT = 1;
                            currLoaiCa = Convert.ToInt32(r["LoaiCa"]);
                            currngayxuat = DateTime.MinValue;
                        }
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
                        else { klg = Math.Round(klg, 5); kltt = kl - klg; }

                        decimal klta = 0;
                        decimal klgta = 0;
                        decimal klttta = 0;
                        if (r["KhoiLuongThucAn"] != DBNull.Value) klta = Convert.ToDecimal(r["KhoiLuongThucAn"]);
                        if (r["KhoiLuongThucAnTT"] != DBNull.Value) klttta = Convert.ToDecimal(r["KhoiLuongThucAnTT"]);
                        if (slg == 0) { klgta = 0; klttta = klta; }
                        else if (sltt == 0) { klgta = klta; klttta = 0; }
                        else klttta = Math.Round(klttta, 5); klgta = klta - klttta;

                        sb.Append("<tr>");
                        sb.Append("<td style='vertical-align:middle;'>" + STT.ToString() + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:center;'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(klg, scaleTTY) + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(klgta, scaleTA) + "</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>" + STT.ToString() + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:center;'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                        sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(kltt, scaleTTY) + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(klttta, scaleTA) + "</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("</tr>");

                        STT++;
                        KLG += Math.Round(klg, scaleTTY);
                        KLTT += Math.Round(kltt, scaleTTY);

                        if (currngayxuat != Convert.ToDateTime(r["NgayXuat"]))
                        {
                            KLGTA += Math.Round(klgta, scaleTA);
                            KLTTTA += Math.Round(klttta, scaleTA);
                            currngayxuat = Convert.ToDateTime(r["NgayXuat"]);
                        }
                    }
                    if (currLoaiCa != 0)
                    {
                        //Viết dòng tổng
                        sb.Append("<tr>");
                        sb.Append("<td></td><td style='vertical-align:middle;font-weight:bold;color:#F00;text-align:center;'>T.C</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLG, scaleTTY) + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLGTA, scaleTA) + "</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td></td><td style='vertical-align:middle;font-weight:bold;color:#F00;text-align:center;'>T.C</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLTT, scaleTTY) + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLTTTA, scaleTA) + "</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("</tr>");
                        //Cộng tổng cuối
                        TKLG += KLG;
                        TKLTT += KLTT;
                        TKLGTA += KLGTA;
                        TKLTTTA += KLTTTA;
                        //Viết dòng Tổng cuối
                        sb.Append("<tr>");
                        sb.Append("<td></td><td style='vertical-align:middle;font-weight:bold;color:#F00;text-align:center;'>Tổng cộng</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(TKLG, scaleTTY) + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(TKLGTA, scaleTA) + "</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td></td><td style='vertical-align:middle;font-weight:bold;color:#F00;text-align:center;'>Tổng cộng</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(TKLTT, scaleTTY) + "</td>");
                        sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(TKLTTTA, scaleTA) + "</td>");
                        sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</tbody></table>");
                    lt.Text = sb.ToString();
                }
                else
                {
                    DateTime currngayxuat = DateTime.MinValue;
                    string tieude = "ĐỀ XUẤT THUỐC THEO NGÀY";
                    string strSQL = "QLCS_BCTK_DeXuatThuocTheoNgay_KhuChuong";
                    SqlParameter[] param = new SqlParameter[4];
                    if (txtTuNgay.Text == "")
                    {
                        txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (txtDenNgay.Text == "")
                    {
                        txtDenNgay.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    }
                    param[0] = new SqlParameter("@dFrom", txtTuNgay.Text);
                    param[1] = new SqlParameter("@dTo", txtDenNgay.Text);
                    param[2] = new SqlParameter("@StrLoaiCa", Config.GetSelectedValues_At(lstLoaiCa));
                    string sKhuChuong = Config.GetSelectedValues_At(lstKhuChuong).StartsWith("@@") ? Config.GetSelectedValues_At(lstKhuChuong).Substring(2) : Config.GetSelectedValues_At(lstKhuChuong);
                    param[3] = new SqlParameter("@StrKhuChuong", sKhuChuong);
                    DataSet ds = Config.SelectSPs(strSQL, param);
                    DataTable dt;

                    string tenLoaiCa = "";
                    int STT = 1;
                    decimal KLG = 0;
                    decimal KLTT = 0;
                    decimal KLGTA = 0;
                    decimal KLTTTA = 0;
                    int currLoaiCa = 0;
                    decimal TKLG = 0;
                    decimal TKLTT = 0;
                    decimal TKLGTA = 0;
                    decimal TKLTTTA = 0;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                    sb.Append(@"<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>
                        <tr><th colspan='8'><b>GIỐNG</b></th><th></th><th colspan='8'><b>TĂNG TRỌNG</b></th></tr>");
                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>STT</th>
                              <th>Ngày</th>
                              <th>Số con</th>
                              <th>Tên<br/>NVL</th>
                              <th>ĐVT</th>
                              <th>Thực xuất</th>
                              <th>Thức ăn<br/>xuất (kg)</th>
                              <th>Ghi chú</th>
                              <th>&nbsp;</th>
                              <th>STT</th>
                              <th>Ngày</th>
                              <th>Số con</th>
                              <th>Tên<br/>NVL</th>
                              <th>ĐVT</th>
                              <th>Thực xuất</th>
                              <th>Thức ăn<br/>xuất (kg)</th>
                              <th>Ghi chú</th>
                             </tr></thead><tbody>");
                    string[] aKhuChuong = Config.GetSelectedTexts(lstKhuChuong).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < aKhuChuong.Length; k++)
                    {
                        TKLG = 0;
                        TKLTT = 0;
                        TKLGTA = 0;
                        TKLTTTA = 0;
                        KLG = 0;
                        KLTT = 0;
                        KLGTA = 0;
                        KLTTTA = 0;
                        STT = 1;
                        currLoaiCa = 0;
                        currngayxuat = DateTime.MinValue;
                        dt = ds.Tables[k];
                        //Viết dòng đầu mới: Tên khu chuồng
                        sb.Append("<tr><td style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>Khu chuồng<br/>" + aKhuChuong[k] + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='font-weight:bold;text-align:center;color:#F00;'>Khu chuồng<br/>" + aKhuChuong[k] + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                        foreach (DataRow r in dt.Rows)
                        {
                            if (currLoaiCa != Convert.ToInt32(r["LoaiCa"]))
                            {
                                tenLoaiCa = Config.GetTextByValue(lstLoaiCa, Convert.ToString(r["LoaiCa"]));
                                if (currLoaiCa != 0)
                                {
                                    //Viết dòng tổng
                                    sb.Append("<tr>");
                                    sb.Append("<td></td><td style='vertical-align:middle;font-weight:bold;color:#F00;text-align:center;'>T.C</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLG, scaleTTY) + "</td>");
                                    sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLGTA, scaleTA) + "</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("<td></td><td style='vertical-align:middle;font-weight:bold;color:#F00;text-align:center;'>T.C</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLTT, scaleTTY) + "</td>");
                                    sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLTTTA, scaleTA) + "</td>");
                                    sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                                    sb.Append("</tr>");
                                }
                                //Viết dòng đầu mới: Tên loại cá
                                sb.Append("<tr><td></td><td style='vertical-align:middle;font-weight:bold;text-align:center;color:#F00;'>" + tenLoaiCa + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='font-weight:bold;text-align:center;color:#F00;'>" + tenLoaiCa + "</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                                //Cộng tổng cuối
                                TKLG += KLG;
                                TKLTT += KLTT;
                                TKLGTA += KLGTA;
                                TKLTTTA += KLTTTA;
                                //Reset
                                KLG = 0;
                                KLTT = 0;
                                KLGTA = 0;
                                KLTTTA = 0;
                                STT = 1;
                                currLoaiCa = Convert.ToInt32(r["LoaiCa"]);
                                currngayxuat = DateTime.MinValue;
                            }
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

                            decimal klta = 0;
                            decimal klgta = 0;
                            decimal klttta = 0;
                            if (r["KhoiLuongThucAnTT"] != DBNull.Value) klttta = Convert.ToDecimal(r["KhoiLuongThucAnTT"]);
                            if (r["KhoiLuongThucAn"] != DBNull.Value) klta = Convert.ToDecimal(r["KhoiLuongThucAn"]);
                            if (slg == 0) { klgta = 0; klttta = klta; }
                            else if (sltt == 0) { klgta = klta; klttta = 0; }
                            else { klttta = Math.Round(klttta, 5); klgta = klta - klttta; }

                            sb.Append("<tr>");
                            sb.Append("<td style='vertical-align:middle;'>" + STT.ToString() + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:center;'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(slg, 0) + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(klg, scaleTTY) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(klgta, scaleTA) + "</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>" + STT.ToString() + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:center;'>" + Convert.ToDateTime(r["NgayXuat"]).ToString("dd/MM/yyyy") + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(sltt, 0) + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["LoaiThuoc"].ToString() + "</td>");
                            sb.Append("<td style='vertical-align:middle;text-align:left;'>" + r["DonViTinh"].ToString() + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(kltt, scaleTTY) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;'>" + Config.ToXVal2(klttta, scaleTA) + "</td>");//
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("</tr>");

                            STT++;
                            KLG += Math.Round(klg, scaleTTY);
                            KLTT += Math.Round(kltt, scaleTTY);
                            if (currngayxuat != Convert.ToDateTime(r["NgayXuat"]))
                            {
                                KLGTA += Math.Round(klgta, scaleTA);
                                KLTTTA += Math.Round(klttta, scaleTA);
                                currngayxuat = Convert.ToDateTime(r["NgayXuat"]);
                            }
                        }
                        if (currLoaiCa != 0)
                        {
                            //Viết dòng tổng
                            sb.Append("<tr>");
                            sb.Append("<td></td><td style='vertical-align:middle;font-weight:bold;color:#F00;text-align:center;'>T.C</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLG, scaleTTY) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLGTA, scaleTA) + "</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td></td><td style='vertical-align:middle;font-weight:bold;color:#F00;text-align:center;'>T.C</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLTT, scaleTTY) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(KLTTTA, scaleTA) + "</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("</tr>");
                            //Cộng tổng cuối
                            TKLG += KLG;
                            TKLTT += KLTT;
                            TKLGTA += KLGTA;
                            TKLTTTA += KLTTTA;
                            //Viết dòng Tổng cuối
                            sb.Append("<tr>");
                            sb.Append("<td></td><td style='vertical-align:middle;font-weight:bold;color:#F00;text-align:center;'>Tổng cộng</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(TKLG, scaleTTY) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(TKLGTA, scaleTA) + "</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td></td><td style='vertical-align:middle;font-weight:bold;color:#F00;text-align:center;'>Tổng cộng</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(TKLTT, scaleTTY) + "</td>");
                            sb.Append(@"<td style='vertical-align:middle;text-align:right;font-weight:bold;color:#F00;'>" + Config.ToXVal2(TKLTTTA, scaleTA) + "</td>");
                            sb.Append("<td style='vertical-align:middle;'>&nbsp;</td>");
                            sb.Append("</tr>");
                        }
                    }
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