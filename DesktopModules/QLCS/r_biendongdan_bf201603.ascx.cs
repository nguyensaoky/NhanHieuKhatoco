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
using Microsoft.ApplicationBlocks.Data;
using DotNetNuke.Framework.Providers;

namespace DotNetNuke.Modules.QLCS
{
    public partial class r_biendongdan_bf201603 : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private const string TABLE_NAME = "THEPAGE";
        private const string ProviderType = "data";
        int scale = 0;
        private string strConn = DotNetNuke.Common.Utilities.Config.GetConnectionString();
        private string objectQualifier;
        private string databaseOwner;

        private string GetFullyQualifiedName(string name)
        {
            ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
            Provider objProvider = (Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider];
            objectQualifier = objProvider.Attributes["objectQualifier"];
            if (!String.IsNullOrEmpty(objectQualifier) && objectQualifier.EndsWith("_") == false)
            {
                objectQualifier += "_";
            }
            databaseOwner = objProvider.Attributes["databaseOwner"];
            if (!String.IsNullOrEmpty(databaseOwner) && databaseOwner.EndsWith(".") == false)
            {
                databaseOwner += ".";
            }
            return databaseOwner + objectQualifier + name;
        }

        private string getConnectionString()
        {
            return DotNetNuke.Common.Utilities.Config.GetConnectionString();
        }

        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        int[] IDLoaiCa = new int[] {1,2,3,4,5,-1,6,7,8,9,11,10 };
        private void BindControls()
        {
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
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

        private decimal GetKhoiLuongFromTable(DataTable dt, int ThucAn)
        {
            foreach (DataRow r in dt.Rows)
            {
                if (Convert.ToInt32(r["ThucAn"]) == ThucAn) return Convert.ToDecimal(r["KhoiLuong"]);
            }
            return 0;
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (txtFromDate.Text == "")
            {
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            DateTime dtFrom = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"));
            if (dtFrom.Day == 1 && dtFrom.Month == 10) btnExcel_FromFirstOct_Click();
            else btnExcel_Others_Click();
        }

        protected void btnExcel_FromFirstOct_Click()
        {
            try
            {
                string filename = "BDTD";
                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                filename = filename.Replace("/", "_");
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                int iThucAn = 0;
                string tieude = "";
                string strSQL = "QLCS_BCTK_BienDongDan_JoinMauBC";
                string strSQL_PLGM = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_oldway";
                string strSubSQL = "QLCS_BCTK_CaAn";
                string strMaxSQL = "QLCS_BCTK_MaxSoLoaiThucAn";
                string strSQLTD = "QLCS_BCTK_BienDongDan_CaTanDung";
                string strSQLTDAn = "QLCS_BCTK_CaKhongMeAn";
                SqlParameter[] param = new SqlParameter[2];
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
                tieude += "<b>BẢNG THEO DÕI BIẾN ĐỘNG TỔNG ĐÀN CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                SqlParameter[] paramPLGM = new SqlParameter[2];
                paramPLGM[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                paramPLGM[1] = new SqlParameter("@dTo", txtToDate.Text);
                DataSet dsPLGM = Config.SelectSPs(strSQL_PLGM, paramPLGM);
                DataTable dtPLGM = dsPLGM.Tables[0];
                DataTable dtPLGM1 = dsPLGM.Tables[1];

                SqlParameter[] paramSub = new SqlParameter[2];
                paramSub[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                paramSub[1] = new SqlParameter("@dTo", txtToDate.Text);
                DataSet dsSub = Config.SelectSPs(strSubSQL, paramSub);
                DataTable dtSub = dsSub.Tables[0];
                DataTable dtSub1 = dsSub.Tables[1];
                DataTable dtSub2 = dsSub.Tables[2];
                SqlParameter[] param2 = new SqlParameter[2];
                param2[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param2[1] = new SqlParameter("@dTo", txtToDate.Text);
                DataTable dtMax = DotNetNuke.NewsProvider.DataProvider.SelectSP(strMaxSQL, param2);
                DataTable dtTD = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTD, param2);
                SqlParameter[] param1 = new SqlParameter[4];
                param1[0] = new SqlParameter("@LoaiCa", 1);
                param1[1] = new SqlParameter("@dFrom", txtFromDate.Text);
                param1[2] = new SqlParameter("@dTo", txtToDate.Text);
                param1[3] = new SqlParameter("@ListThucAn", "");
                param1[3].Direction = ParameterDirection.Output;
                param1[3].Size = 4000;
                DataTable dtTDAn = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTDAn, param1);
                string ListThucAnTD = param1[3].Value.ToString();
                int SoCotThucAn = 2;
                if (dtMax != null && dtMax.Rows.Count == 1 && dtMax.Rows[0]["MaxSoLoaiThucAn"] != DBNull.Value) SoCotThucAn = Convert.ToInt32(dtMax.Rows[0]["MaxSoLoaiThucAn"]) * 2;
                if (Convert.ToInt32(dtSub2.Rows[0]["MaxSoLoaiThucAnHauBi"]) * 2 > SoCotThucAn) SoCotThucAn = Convert.ToInt32(dtSub2.Rows[0]["MaxSoLoaiThucAnHauBi"]) * 2;
                if (SoCotThucAn < 2) SoCotThucAn = 2;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table border='1'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=3>STT</th>
                          <th rowspan=3>Nội dung</th>
                          <th colspan=3>Tồn đầu</th>
                          <th colspan=3>Nhập</th>
                          <th colspan=3>Xuất</th>
                          <th colspan=3>Chết</th>
                          <th colspan=3>Loại thải</th>
                          <th colspan=5>Giết mổ</th>
                          <th colspan=3>Tồn cuối</th>
                          <th colspan=" + SoCotThucAn.ToString() + @">Tiêu tốn thức ăn</th>
                          <th rowspan=3>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=4>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>");
                for (int ii = 0; ii < SoCotThucAn / 2; ii++)
                {
                    sb.Append(@"<th rowspan=2>Loại</th>
                          <th rowspan=2>SL (kg)</th>");
                }

                sb.Append(@"</tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>TP</th>
                          <th>LT</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>");
                sb.Append("</tr></thead><tbody>");
                int i = 0;
                //Cá loại 1 (cá con)
                DataRow[] r1 = new DataRow[6];
                for (int jj = 0; jj < 6; jj++)
                {
                    r1[jj] = dt.Rows[i * 6 + jj];
                }
                sb.Append("<tr>");
                sb.Append("<td style='vertical-align:middle;'>1.1</td>");

                int[,] GiaTri1 = new int[3, 7];
                int t1;

                //-------------------------------------------

                //GiaTri1[1, 0] - Giong Ton Dau
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 0] += Convert.ToInt32(r1[t1]["TonDau"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
                }
                //GiaTri1[2, 0] - Tang Trong Ton Dau
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 0] += Convert.ToInt32(r1[t1]["TonDau"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
                }
                //GiaTri1[0, 0] - Tong Ton Dau
                GiaTri1[0, 0] = GiaTri1[1, 0] + GiaTri1[2, 0];

                //-------------------------------------------

                //GiaTri1[1, 1] - Giong Nhap
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]) - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
                }
                //GiaTri1[2, 1] - Tang Trong Nhap
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]) - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
                }
                //GiaTri1[0, 1] - Tong Nhap
                GiaTri1[0, 1] = GiaTri1[1, 1] + GiaTri1[2, 1];

                //-------------------------------------------

                //GiaTri1[1, 2] - Giong Xuat
                for (t1 = 0; t1 < 3; t1++)
                {
                    //GiaTri1[1, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]) - Convert.ToInt32(r1[t1]["Nhap_CGT"]) - Convert.ToInt32(r1[t1]["Nhap_CPL"]);
                    GiaTri1[1, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
                }
                //GiaTri1[2, 2] - Tang Trong Xuat
                for (t1 = 3; t1 < 6; t1++)
                {
                    //GiaTri1[2, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]) - Convert.ToInt32(r1[t1]["Nhap_CGT"]) - Convert.ToInt32(r1[t1]["Nhap_CPL"]);
                    GiaTri1[2, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
                }
                //GiaTri1[0, 2] - Tong Xuat
                GiaTri1[0, 2] = GiaTri1[1, 2] + GiaTri1[2, 2];

                //-------------------------------------------

                //GiaTri1[1, 3] - Giong Chet
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 3] += Convert.ToInt32(r1[t1]["Chet"]);
                }
                //GiaTri1[2, 3] - Tang Trong Chet
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 3] += Convert.ToInt32(r1[t1]["Chet"]);
                }
                //GiaTri1[0, 3] - Tong Chet
                GiaTri1[0, 3] = GiaTri1[1, 3] + GiaTri1[2, 3];

                //-------------------------------------------

                //GiaTri1[1, 3] - Giong LoaiThai
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 4] += Convert.ToInt32(r1[t1]["LoaiThai"]);
                }
                //GiaTri1[2, 3] - Tang Trong Chet
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 4] += Convert.ToInt32(r1[t1]["LoaiThai"]);
                }
                //GiaTri1[0, 3] - Tong Chet
                GiaTri1[0, 4] = GiaTri1[1, 4] + GiaTri1[2, 4];

                //-------------------------------------------

                //GiaTri1[1, 4] - Giong Giet Mo
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 5] += Convert.ToInt32(r1[t1]["GietMo"]);
                }
                //GiaTri1[2, 4] - Tang Trong Giet Mo
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 5] += Convert.ToInt32(r1[t1]["GietMo"]);
                }
                //GiaTri1[0, 4] - Tong Giet Mo
                GiaTri1[0, 5] = GiaTri1[1, 5] + GiaTri1[2, 5];

                //-------------------------------------------

                //GiaTri1[1, 5] - Giong Ton Cuoi
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 6] += Convert.ToInt32(r1[t1]["TonCuoi"]);
                }
                //GiaTri1[2, 5] - Tang Trong Ton Cuoi
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 6] += Convert.ToInt32(r1[t1]["TonCuoi"]);
                }
                //GiaTri1[0, 5] - Tong Ton Cuoi
                GiaTri1[0, 6] = GiaTri1[1, 6] + GiaTri1[2, 6];

                //-------------------------------------------

                DataRow rTD = dtTD.Rows[0];
                sb.Append("<td align='left'>Cá GĐ úm</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 0] - Convert.ToInt32(rTD["TonDau"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 1] - (Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"])), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 2] - (Convert.ToInt32(rTD["Xuat_Ban"]) + Convert.ToInt32(rTD["Xuat_CLC"])), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 3] - Convert.ToInt32(rTD["Chet"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 4] - Convert.ToInt32(rTD["LoaiThai"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 5] - Convert.ToInt32(rTD["GietMo"]), 0) + "</td><td></td><td>" + Config.ToXVal2(GiaTri1[0, 5] - Convert.ToInt32(rTD["GietMo"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 6] - Convert.ToInt32(rTD["TonCuoi"]), 0) + "</td><td></td><td></td>");

                //ThucAn Start
                int j = 0;
                for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                {
                    DataRow rta = dtSub.Rows[h];
                    if (rta["LoaiCa"].ToString() == "1")
                    {
                        if (ListThucAnTD.Contains("@" + rta["ThucAn"].ToString() + "@"))
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(Convert.ToDecimal(rta["KhoiLuong"]) - GetKhoiLuongFromTable(dtTDAn, Convert.ToInt32(rta["ThucAn"])), scale) + "</td>");
                        else
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                        j++;
                    }
                    else
                    {
                        iThucAn = h;
                        break;
                    }
                }
                for (int t = j; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
                //ThucAn End
                sb.Append("<td></td>");
                sb.Append("</tr>");



                //Cá tận dụng
                sb.Append("<tr>");
                sb.Append("<td style='vertical-align:middle;'>1.2</td>");
                sb.Append("<td align='left'>Cá GĐ úm tận dụng</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonDau"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Xuat_Ban"]) + Convert.ToInt32(rTD["Xuat_CLC"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["Chet"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["LoaiThai"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["GietMo"], 0) + "</td><td></td><td>" + Config.ToXVal2(rTD["GietMo"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonCuoi"], 0) + "</td><td></td><td></td>");
                //ThucAn Start
                if (ListThucAnTD == "")
                {
                    for (int t = 0; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
                }
                else
                {
                    int o = 0;
                    for (o = 0; o < dtTDAn.Rows.Count; o++)
                    {
                        DataRow r = dtTDAn.Rows[o];
                        sb.Append("<td>" + r["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(r["KhoiLuong"], scale) + "</td>");
                    }
                    for (int t = o; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
                }
                //ThucAn End
                sb.Append("<td></td>");
                sb.Append("</tr>");


                //Cá từ loại 2 -> loại 4
                for (i = 1; i < 4; i++)
                {
                    DataRow[] r = new DataRow[6];
                    for (int jj = 0; jj < 6; jj++)
                    {
                        r[jj] = dt.Rows[i * 6 + jj];
                    }
                    sb.Append("<tr>");
                    int idx = i + 1;
                    sb.Append("<td style='vertical-align:middle;'>" + idx.ToString() + "</td>");
                    string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                    int[,] GiaTri = new int[3, 7];
                    int t;

                    //-------------------------------------------

                    //GiaTri[1, 0] - Giong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    //GiaTri[2, 0] - Tang Trong Ton Dau
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    //GiaTri[0, 0] - Tong Ton Dau
                    GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];

                    //-------------------------------------------

                    //GiaTri[1, 1] - Giong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    //GiaTri[2, 1] - Tang Trong Nhap
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    //GiaTri[0, 1] - Tong Nhap
                    GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];

                    //-------------------------------------------

                    //GiaTri[1, 2] - Giong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        //GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                        GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    //GiaTri[2, 2] - Tang Trong Xuat
                    for (t = 3; t < 6; t++)
                    {
                        //GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                        GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    //GiaTri[0, 2] - Tong Xuat
                    GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];

                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 3] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 3] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];

                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 4] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 4] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];

                    //-------------------------------------------

                    //GiaTri[1, 4] - Giong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 5] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    //GiaTri[2, 4] - Tang Trong Giet Mo
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 5] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    //GiaTri[0, 4] - Tong Giet Mo
                    GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];

                    //-------------------------------------------

                    //GiaTri[1, 5] - Giong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 6] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 6] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];

                    //-------------------------------------------

                    int iThucAnGoc = iThucAn;
                    for (t = 0; t < 3; t++)
                    {
                        if (t == 0)
                        {
                            sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 5; y++)
                            {
                                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                            }
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 5], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 3]["GietMo"]), 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 2]["GietMo"]), 0) + "</td><td></td><td></td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6], 0) + "</td><td></td><td></td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                    j++;
                                }
                                else
                                {
                                    iThucAn = h;
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            sb.Append("<td></td>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td>");
                            sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 5; y++)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                            }
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 5], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 2]["GietMo"], 0) + "</td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6], 0) + "</td><td></td><td></td>");
                            //
                            //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAnGoc; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    if (t == 1)
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                    }
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            //
                            sb.Append("<td></td>");
                        }
                        sb.Append("</tr>");
                    }
                }


                //Cá hậu bị từ loại 5, -1
                for (int n = 0; n < 1; n++)
                {
                    DataRow[] r = new DataRow[6];
                    for (int jj = 0; jj < 6; jj++)
                    {
                        r[jj] = dt1.Rows[jj];
                    }
                    sb.Append("<tr>");
                    sb.Append("<td style='vertical-align:middle;'>5</td>");
                    string[] tenLoaiCa = new string[] { "Hậu bị", "Con giống", "Tăng trọng" };
                    int[,] GiaTri = new int[3, 21];
                    int t;

                    //-------------------------------------------

                    //GiaTri[1, 0] - Giong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[1, 1] += Convert.ToInt32(r[2]["TonDau"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) - Convert.ToInt32(r[2]["Xuat_CGTO"]) - Convert.ToInt32(r[2]["Xuat_CPLO"]);
                    GiaTri[1, 2] += Convert.ToInt32(r[1]["TonDau"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) - Convert.ToInt32(r[1]["Xuat_CGTO"]) - Convert.ToInt32(r[1]["Xuat_CPLO"]);
                    //GiaTri[2, 0] - Tang Trong Ton Dau
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[2, 1] += Convert.ToInt32(r[5]["TonDau"]) + Convert.ToInt32(r[5]["Nhap_CGTO"]) + Convert.ToInt32(r[5]["Nhap_CPLO"]) - Convert.ToInt32(r[5]["Xuat_CGTO"]) - Convert.ToInt32(r[5]["Xuat_CPLO"]);
                    GiaTri[2, 2] += Convert.ToInt32(r[4]["TonDau"]) + Convert.ToInt32(r[4]["Nhap_CGTO"]) + Convert.ToInt32(r[4]["Nhap_CPLO"]) - Convert.ToInt32(r[4]["Xuat_CGTO"]) - Convert.ToInt32(r[4]["Xuat_CPLO"]);
                    //GiaTri[0, 0] - Tong Ton Dau
                    GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
                    GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];
                    GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];
                    //-------------------------------------------

                    //GiaTri[1, 1] - Giong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[1, 4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]) - Convert.ToInt32(r[2]["Xuat_CPLI"]) - Convert.ToInt32(r[2]["Xuat_CGTI"]);
                    GiaTri[1, 5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]) - Convert.ToInt32(r[1]["Xuat_CPLI"]) - Convert.ToInt32(r[1]["Xuat_CGTI"]);
                    //GiaTri[2, 1] - Tang Trong Nhap
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[2, 4] += Convert.ToInt32(r[5]["Nhap_NhapChuong"]) + Convert.ToInt32(r[5]["Nhap_CLC"]) + Convert.ToInt32(r[5]["Nhap_CGTI"]) + Convert.ToInt32(r[5]["Nhap_CGT_D"]) + Convert.ToInt32(r[5]["Nhap_CPLI"]) + Convert.ToInt32(r[5]["Nhap_CPL_D"]) + Convert.ToInt32(r[5]["Nhap_CTT"]) - Convert.ToInt32(r[5]["Xuat_CPLI"]) - Convert.ToInt32(r[5]["Xuat_CGTI"]);
                    GiaTri[2, 5] += Convert.ToInt32(r[4]["Nhap_NhapChuong"]) + Convert.ToInt32(r[4]["Nhap_CLC"]) + Convert.ToInt32(r[4]["Nhap_CGTI"]) + Convert.ToInt32(r[4]["Nhap_CGT_D"]) + Convert.ToInt32(r[4]["Nhap_CPLI"]) + Convert.ToInt32(r[4]["Nhap_CPL_D"]) + Convert.ToInt32(r[4]["Nhap_CTT"]) - Convert.ToInt32(r[4]["Xuat_CPLI"]) - Convert.ToInt32(r[4]["Xuat_CGTI"]);
                    //GiaTri[0, 1] - Tong Nhap
                    GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
                    GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];
                    GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];
                    //-------------------------------------------

                    //GiaTri[1, 2] - Giong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[1, 7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);
                    GiaTri[1, 8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                    //GiaTri[2, 2] - Tang Trong Xuat
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[2, 7] += Convert.ToInt32(r[5]["Xuat_Ban"]) + Convert.ToInt32(r[5]["Xuat_CLC"]) + Convert.ToInt32(r[5]["Xuat_CGT_D"]) + Convert.ToInt32(r[5]["Xuat_CPL_D"]);
                    GiaTri[2, 8] += Convert.ToInt32(r[4]["Xuat_Ban"]) + Convert.ToInt32(r[4]["Xuat_CLC"]) + Convert.ToInt32(r[4]["Xuat_CGT_D"]) + Convert.ToInt32(r[4]["Xuat_CPL_D"]);
                    //GiaTri[0, 2] - Tong Xuat
                    GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
                    GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];
                    GiaTri[0, 8] = GiaTri[1, 8] + GiaTri[2, 8];
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[1, 10] += Convert.ToInt32(r[2]["Chet"]);
                    GiaTri[1, 11] += Convert.ToInt32(r[1]["Chet"]);
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[2, 10] += Convert.ToInt32(r[5]["Chet"]);
                    GiaTri[2, 11] += Convert.ToInt32(r[4]["Chet"]);
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
                    GiaTri[0, 10] = GiaTri[1, 10] + GiaTri[2, 10];
                    GiaTri[0, 11] = GiaTri[1, 11] + GiaTri[2, 11];//Ky
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[1, 13] += Convert.ToInt32(r[2]["LoaiThai"]);
                    GiaTri[1, 14] += Convert.ToInt32(r[1]["LoaiThai"]);
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[2, 13] += Convert.ToInt32(r[5]["LoaiThai"]);
                    GiaTri[2, 14] += Convert.ToInt32(r[4]["LoaiThai"]);
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
                    GiaTri[0, 13] = GiaTri[1, 13] + GiaTri[2, 13];
                    GiaTri[0, 14] = GiaTri[1, 14] + GiaTri[2, 14];
                    //-------------------------------------------

                    //GiaTri[1, 4] - Giong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[1, 16] += Convert.ToInt32(r[2]["GietMo"]);
                    GiaTri[1, 17] += Convert.ToInt32(r[1]["GietMo"]);
                    //GiaTri[2, 4] - Tang Trong Giet Mo
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[2, 16] += Convert.ToInt32(r[5]["GietMo"]);
                    GiaTri[2, 17] += Convert.ToInt32(r[4]["GietMo"]);
                    //GiaTri[0, 4] - Tong Giet Mo
                    GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
                    GiaTri[0, 16] = GiaTri[1, 16] + GiaTri[2, 16];
                    GiaTri[0, 17] = GiaTri[1, 17] + GiaTri[2, 17];
                    //-------------------------------------------

                    //GiaTri[1, 5] - Giong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[1, 19] += Convert.ToInt32(r[2]["TonCuoi"]);
                    GiaTri[1, 20] += Convert.ToInt32(r[1]["TonCuoi"]);
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[2, 19] += Convert.ToInt32(r[5]["TonCuoi"]);
                    GiaTri[2, 20] += Convert.ToInt32(r[4]["TonCuoi"]);
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
                    GiaTri[0, 19] = GiaTri[1, 19] + GiaTri[2, 19];
                    GiaTri[0, 20] = GiaTri[1, 20] + GiaTri[2, 20];
                    //-------------------------------------------

                    for (t = 0; t < 3; t++)
                    {
                        if (t == 0)
                        {
                            sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1.Rows[1]["GietMo"]) + Convert.ToInt32(dtPLGM1.Rows[3]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1.Rows[0]["GietMo"]) + Convert.ToInt32(dtPLGM1.Rows[2]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = 0; h < dtSub1.Rows.Count; h++)
                            {
                                DataRow rta = dtSub1.Rows[h];
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                j++;
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            sb.Append("<td></td>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td>");
                            sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM1.Rows[t * 2 - 1]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM1.Rows[t * 2 - 2]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //
                            //ThucAn Start
                            j = 0;
                            for (int h = 0; h < dtSub1.Rows.Count; h++)
                            {
                                DataRow rta = dtSub1.Rows[h];
                                if (t == 1)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                }
                                j++;
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            //
                            sb.Append("<td></td>");
                        }
                        sb.Append("</tr>");
                    }
                }



                //Cá loại 5, -1, 6
                for (i = 4; i < 7; i++)
                {
                    DataRow[] r = new DataRow[6];
                    for (int jj = 0; jj < 6; jj++)
                    {
                        r[jj] = dt.Rows[i * 6 + jj];
                    }
                    sb.Append("<tr>");
                    if (i == 4) sb.Append("<td style='vertical-align:middle;'>5.1</td>");
                    else if (i == 5) sb.Append("<td style='vertical-align:middle;'>5.2</td>");
                    else sb.Append("<td style='vertical-align:middle;'>" + i.ToString() + "</td>");
                    string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                    int[,] GiaTri = new int[3, 21];
                    int t;

                    //-------------------------------------------

                    //GiaTri[1, 0] - Giong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[1, 1] += Convert.ToInt32(r[2]["TonDau"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) - Convert.ToInt32(r[2]["Xuat_CGTO"]) - Convert.ToInt32(r[2]["Xuat_CPLO"]);
                    GiaTri[1, 2] += Convert.ToInt32(r[1]["TonDau"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) - Convert.ToInt32(r[1]["Xuat_CGTO"]) - Convert.ToInt32(r[1]["Xuat_CPLO"]);
                    //GiaTri[2, 0] - Tang Trong Ton Dau
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[2, 1] += Convert.ToInt32(r[5]["TonDau"]) + Convert.ToInt32(r[5]["Nhap_CGTO"]) + Convert.ToInt32(r[5]["Nhap_CPLO"]) - Convert.ToInt32(r[5]["Xuat_CGTO"]) - Convert.ToInt32(r[5]["Xuat_CPLO"]);
                    GiaTri[2, 2] += Convert.ToInt32(r[4]["TonDau"]) + Convert.ToInt32(r[4]["Nhap_CGTO"]) + Convert.ToInt32(r[4]["Nhap_CPLO"]) - Convert.ToInt32(r[4]["Xuat_CGTO"]) - Convert.ToInt32(r[4]["Xuat_CPLO"]);
                    //GiaTri[0, 0] - Tong Ton Dau
                    GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
                    GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];
                    GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];
                    //-------------------------------------------

                    //GiaTri[1, 1] - Giong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[1, 4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]) - Convert.ToInt32(r[2]["Xuat_CPLI"]) - Convert.ToInt32(r[2]["Xuat_CGTI"]);
                    GiaTri[1, 5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]) - Convert.ToInt32(r[1]["Xuat_CPLI"]) - Convert.ToInt32(r[1]["Xuat_CGTI"]);
                    //GiaTri[2, 1] - Tang Trong Nhap
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[2, 4] += Convert.ToInt32(r[5]["Nhap_NhapChuong"]) + Convert.ToInt32(r[5]["Nhap_CLC"]) + Convert.ToInt32(r[5]["Nhap_CGTI"]) + Convert.ToInt32(r[5]["Nhap_CGT_D"]) + Convert.ToInt32(r[5]["Nhap_CPLI"]) + Convert.ToInt32(r[5]["Nhap_CPL_D"]) + Convert.ToInt32(r[5]["Nhap_CTT"]) - Convert.ToInt32(r[5]["Xuat_CPLI"]) - Convert.ToInt32(r[5]["Xuat_CGTI"]);
                    GiaTri[2, 5] += Convert.ToInt32(r[4]["Nhap_NhapChuong"]) + Convert.ToInt32(r[4]["Nhap_CLC"]) + Convert.ToInt32(r[4]["Nhap_CGTI"]) + Convert.ToInt32(r[4]["Nhap_CGT_D"]) + Convert.ToInt32(r[4]["Nhap_CPLI"]) + Convert.ToInt32(r[4]["Nhap_CPL_D"]) + Convert.ToInt32(r[4]["Nhap_CTT"]) - Convert.ToInt32(r[4]["Xuat_CPLI"]) - Convert.ToInt32(r[4]["Xuat_CGTI"]);
                    //GiaTri[0, 1] - Tong Nhap
                    GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
                    GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];
                    GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];
                    //-------------------------------------------

                    //GiaTri[1, 2] - Giong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[1, 7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);
                    GiaTri[1, 8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                    //GiaTri[2, 2] - Tang Trong Xuat
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[2, 7] += Convert.ToInt32(r[5]["Xuat_Ban"]) + Convert.ToInt32(r[5]["Xuat_CLC"]) + Convert.ToInt32(r[5]["Xuat_CGT_D"]) + Convert.ToInt32(r[5]["Xuat_CPL_D"]);
                    GiaTri[2, 8] += Convert.ToInt32(r[4]["Xuat_Ban"]) + Convert.ToInt32(r[4]["Xuat_CLC"]) + Convert.ToInt32(r[4]["Xuat_CGT_D"]) + Convert.ToInt32(r[4]["Xuat_CPL_D"]);
                    //GiaTri[0, 2] - Tong Xuat
                    GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
                    GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];
                    GiaTri[0, 8] = GiaTri[1, 8] + GiaTri[2, 8];
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[1, 10] += Convert.ToInt32(r[2]["Chet"]);
                    GiaTri[1, 11] += Convert.ToInt32(r[1]["Chet"]);
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[2, 10] += Convert.ToInt32(r[5]["Chet"]);
                    GiaTri[2, 11] += Convert.ToInt32(r[4]["Chet"]);
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
                    GiaTri[0, 10] = GiaTri[1, 10] + GiaTri[2, 10];
                    GiaTri[0, 11] = GiaTri[1, 11] + GiaTri[2, 11];//Ky
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[1, 13] += Convert.ToInt32(r[2]["LoaiThai"]);
                    GiaTri[1, 14] += Convert.ToInt32(r[1]["LoaiThai"]);
                    //GiaTri[2, 4] - Tang Trong Giet Mo
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[2, 13] += Convert.ToInt32(r[5]["LoaiThai"]);
                    GiaTri[2, 14] += Convert.ToInt32(r[4]["LoaiThai"]);
                    //GiaTri[0, 4] - Tong Giet Mo
                    GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
                    GiaTri[0, 13] = GiaTri[1, 13] + GiaTri[2, 13];
                    GiaTri[0, 14] = GiaTri[1, 14] + GiaTri[2, 14];
                    //-------------------------------------------

                    //GiaTri[1, 4] - Giong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[1, 16] += Convert.ToInt32(r[2]["GietMo"]);
                    GiaTri[1, 17] += Convert.ToInt32(r[1]["GietMo"]);
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[2, 16] += Convert.ToInt32(r[5]["GietMo"]);
                    GiaTri[2, 17] += Convert.ToInt32(r[4]["GietMo"]);
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
                    GiaTri[0, 16] = GiaTri[1, 16] + GiaTri[2, 16];
                    GiaTri[0, 17] = GiaTri[1, 17] + GiaTri[2, 17];
                    //-------------------------------------------

                    //GiaTri[1, 5] - Giong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[1, 19] += Convert.ToInt32(r[2]["TonCuoi"]);
                    GiaTri[1, 20] += Convert.ToInt32(r[1]["TonCuoi"]);
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[2, 19] += Convert.ToInt32(r[5]["TonCuoi"]);
                    GiaTri[2, 20] += Convert.ToInt32(r[4]["TonCuoi"]);
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
                    GiaTri[0, 19] = GiaTri[1, 19] + GiaTri[2, 19];
                    GiaTri[0, 20] = GiaTri[1, 20] + GiaTri[2, 20];
                    //-------------------------------------------

                    int iThucAnGoc = iThucAn;
                    for (t = 0; t < 3; t++)
                    {
                        if (t == 0)
                        {
                            sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 3]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 2]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                    j++;
                                }
                                else
                                {
                                    iThucAn = h;
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            sb.Append("<td></td>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td>");
                            sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 2]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //
                            //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAnGoc; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    if (t == 1)
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                    }
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            //
                            sb.Append("<td></td>");
                        }
                        sb.Append("</tr>");
                    }
                }

                //Cá từ loại 7 -> loại 11
                for (i = 7; i < 12; i++)
                {
                    DataRow[] r = new DataRow[3];
                    for (int jj = 0; jj < 3; jj++)
                    {
                        r[jj] = dt.Rows[i * 6 + jj];
                    }
                    sb.Append("<tr>");
                    sb.Append("<td>" + i.ToString() + "</td>");
                    sb.Append("<td align='left'>" + r[0]["TenLoaiCa"].ToString() + "</td>");
                    int[] GiaTri = new int[21];
                    int t;

                    //-------------------------------------------

                    //GiaTri[0] - Tong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    //GiaTri[2] - Cai Ton Dau
                    GiaTri[2] += Convert.ToInt32(r[1]["TonDau"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) - Convert.ToInt32(r[1]["Xuat_CGTO"]) - Convert.ToInt32(r[1]["Xuat_CPLO"]);
                    //GiaTri[1] - Duc Ton Dau
                    GiaTri[1] += Convert.ToInt32(r[2]["TonDau"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) - Convert.ToInt32(r[2]["Xuat_CGTO"]) - Convert.ToInt32(r[2]["Xuat_CPLO"]);

                    //-------------------------------------------

                    //GiaTri[3] - Tong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]);
                    }
                    //GiaTri[5] - Cai Nhap
                    GiaTri[5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]) - Convert.ToInt32(r[1]["Xuat_CGTI"]) - Convert.ToInt32(r[1]["Xuat_CPLI"]);
                    //GiaTri[4] - Duc Nhap
                    GiaTri[4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]) - Convert.ToInt32(r[2]["Xuat_CGTI"]) - Convert.ToInt32(r[2]["Xuat_CPLI"]);

                    //-------------------------------------------

                    //GiaTri[6] - Tong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        //GiaTri[6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CGT"]) - Convert.ToInt32(r[t]["Nhap_CPL"]);
                        GiaTri[6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    //GiaTri[8] - Cai Xuat
                    //GiaTri[8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGT"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPL"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]) - Convert.ToInt32(r[1]["Nhap_CGT"]) - Convert.ToInt32(r[1]["Nhap_CPL"]);
                    GiaTri[8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                    //GiaTri[7] - Duc Xuat
                    //GiaTri[7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGT"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPL"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]) - Convert.ToInt32(r[2]["Nhap_CGT"]) - Convert.ToInt32(r[2]["Nhap_CPL"]);
                    GiaTri[7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);

                    //-------------------------------------------

                    //GiaTri[9] - Tong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    //GiaTri[11] - Cai Chet
                    GiaTri[11] += Convert.ToInt32(r[1]["Chet"]);
                    //GiaTri[10] - Duc Chet
                    GiaTri[10] += Convert.ToInt32(r[2]["Chet"]);

                    //-------------------------------------------

                    //GiaTri[9] - Tong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    //GiaTri[14] - Cai Giet Mo
                    GiaTri[14] += Convert.ToInt32(r[1]["LoaiThai"]);
                    //GiaTri[13] - Duc Giet Mo
                    GiaTri[13] += Convert.ToInt32(r[2]["LoaiThai"]);

                    //-------------------------------------------

                    //GiaTri[12] - Tong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    //GiaTri[17] - Cai Ton Cuoi
                    GiaTri[17] += Convert.ToInt32(r[1]["GietMo"]);
                    //GiaTri[16] - Duc Ton Cuoi
                    GiaTri[16] += Convert.ToInt32(r[2]["GietMo"]);

                    //-------------------------------------------

                    //GiaTri[15] - Tong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    //GiaTri[17] - Cai Ton Cuoi
                    GiaTri[20] += Convert.ToInt32(r[1]["TonCuoi"]);
                    //GiaTri[16] - Duc Ton Cuoi
                    GiaTri[19] += Convert.ToInt32(r[2]["TonCuoi"]);

                    //-------------------------------------------

                    for (int y = 0; y < 5; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 1], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 2], 0) + "</td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[15], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + 1]["GietMo"], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4]["GietMo"], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[16], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[17], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[18], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[19], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[20], 0) + "</td>");
                    //ThucAn Start
                    j = 0;
                    for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                    {
                        DataRow rta = dtSub.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                        {
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                            j++;
                        }
                        else
                        {
                            iThucAn = h;
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                }
                //Tính tổng
                int[] Tong = new int[21];
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    Tong[0] += Convert.ToInt32(r["TonDau"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[2] += Convert.ToInt32(r["TonDau"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[1] += Convert.ToInt32(r["TonDau"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                    }

                    Tong[3] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]) - Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[5] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]) - Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[4] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]) - Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                    }

                    //Tong[6] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL"]) + Convert.ToInt32(r["Xuat_CPL_D"]) - Convert.ToInt32(r["Nhap_CGT"]) - Convert.ToInt32(r["Nhap_CPL"]);
                    Tong[6] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        //Tong[8] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL"]) + Convert.ToInt32(r["Xuat_CPL_D"]) - Convert.ToInt32(r["Nhap_CGT"]) - Convert.ToInt32(r["Nhap_CPL"]);
                        Tong[8] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        //Tong[7] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL"]) + Convert.ToInt32(r["Xuat_CPL_D"]) - Convert.ToInt32(r["Nhap_CGT"]) - Convert.ToInt32(r["Nhap_CPL"]);
                        Tong[7] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                    }

                    Tong[9] += Convert.ToInt32(r["Chet"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[11] += Convert.ToInt32(r["Chet"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[10] += Convert.ToInt32(r["Chet"]);
                    }

                    Tong[12] += Convert.ToInt32(r["LoaiThai"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[14] += Convert.ToInt32(r["LoaiThai"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[13] += Convert.ToInt32(r["LoaiThai"]);
                    }

                    Tong[15] += Convert.ToInt32(r["GietMo"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[17] += Convert.ToInt32(r["GietMo"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[16] += Convert.ToInt32(r["GietMo"]);
                    }

                    Tong[18] += Convert.ToInt32(r["TonCuoi"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[20] += Convert.ToInt32(r["TonCuoi"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[19] += Convert.ToInt32(r["TonCuoi"]);
                    }
                }
                //Tính tổng phân loại giêt mô
                int[] TongPLGM = new int[2];
                for (i = 0; i < dtPLGM.Rows.Count; i++)
                {
                    DataRow r = dtPLGM.Rows[i];
                    if (i % 2 == 1)
                    {
                        TongPLGM[1] += Convert.ToInt32(r["GietMo"]);
                    }
                    else
                    {
                        TongPLGM[0] += Convert.ToInt32(r["GietMo"]);
                    }
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td></td><td>Tổng cộng</td>");
                for (i = 0; i < 15; i++)
                {
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[i], 0) + "</td>");
                }
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[15], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TongPLGM[1], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TongPLGM[0], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[16], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[17], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[18], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[19], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[20], 0) + "</td>");

                for (i = 0; i < SoCotThucAn; i++)
                {
                    sb.Append(@"<td></td>");
                }
                sb.Append("<td></td></tr>");
                sb.Append("</tbody>");
                sb.Append("</table><br/><div style='text-align:right;font-style:italic;'>Ninh Ích, ngày&nbsp;&nbsp;&nbsp;tháng&nbsp;&nbsp;&nbsp;&nbsp;năm&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div></tr>");
                sb.Append("<br/><div style='font-weight:bold;text-align:left;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Giám đốc&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Kế toán trưởng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;BP. Theo dõi&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Lãnh đạo trại</div></body></html>");
                Response.Write(sb.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnExcel_Others_Click()
        {
            try
            {
                string filename = "BDTD";
                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                filename = filename.Replace("/", "_");
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                int iThucAn = 0;
                string tieude = "";
                string strSQL = "QLCS_BCTK_BienDongDan_JoinMauBC";
                string strSQL_PLGM = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_oldway";
                string strSubSQL = "QLCS_BCTK_CaAn";
                string strMaxSQL = "QLCS_BCTK_MaxSoLoaiThucAn";
                string strSQLTD = "QLCS_BCTK_BienDongDan_CaTanDung";
                string strSQLTDAn = "QLCS_BCTK_CaKhongMeAn";
                SqlParameter[] param = new SqlParameter[2];
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
                tieude += "<b>BẢNG THEO DÕI BIẾN ĐỘNG TỔNG ĐÀN CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                SqlParameter[] paramPLGM = new SqlParameter[2];
                paramPLGM[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                paramPLGM[1] = new SqlParameter("@dTo", txtToDate.Text);
                DataSet dsPLGM = Config.SelectSPs(strSQL_PLGM, paramPLGM);
                DataTable dtPLGM = dsPLGM.Tables[0];
                DataTable dtPLGM1 = dsPLGM.Tables[1];

                SqlParameter[] paramSub = new SqlParameter[2];
                paramSub[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                paramSub[1] = new SqlParameter("@dTo", txtToDate.Text);
                DataSet dsSub = Config.SelectSPs(strSubSQL, paramSub);
                DataTable dtSub = dsSub.Tables[0];
                DataTable dtSub1 = dsSub.Tables[1];
                DataTable dtSub2 = dsSub.Tables[2];
                SqlParameter[] param2 = new SqlParameter[2];
                param2[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param2[1] = new SqlParameter("@dTo", txtToDate.Text);
                DataTable dtMax = DotNetNuke.NewsProvider.DataProvider.SelectSP(strMaxSQL, param2);
                DataTable dtTD = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTD, param2);
                SqlParameter[] param1 = new SqlParameter[4];
                param1[0] = new SqlParameter("@LoaiCa", 1);
                param1[1] = new SqlParameter("@dFrom", txtFromDate.Text);
                param1[2] = new SqlParameter("@dTo", txtToDate.Text);
                param1[3] = new SqlParameter("@ListThucAn", "");
                param1[3].Direction = ParameterDirection.Output;
                param1[3].Size = 4000;
                DataTable dtTDAn = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTDAn, param1);
                string ListThucAnTD = param1[3].Value.ToString();
                int SoCotThucAn = 2;
                if (dtMax != null && dtMax.Rows.Count == 1 && dtMax.Rows[0]["MaxSoLoaiThucAn"] != DBNull.Value) SoCotThucAn = Convert.ToInt32(dtMax.Rows[0]["MaxSoLoaiThucAn"]) * 2;
                if (Convert.ToInt32(dtSub2.Rows[0]["MaxSoLoaiThucAnHauBi"]) * 2 > SoCotThucAn) SoCotThucAn = Convert.ToInt32(dtSub2.Rows[0]["MaxSoLoaiThucAnHauBi"]) * 2;
                if (SoCotThucAn < 2) SoCotThucAn = 2;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table border='1'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=3>STT</th>
                          <th rowspan=3>Nội dung</th>
                          <th colspan=3>Tồn đầu</th>
                          <th colspan=3>Nhập</th>
                          <th colspan=3>Xuất</th>
                          <th colspan=3>Chết</th>
                          <th colspan=3>Loại thải</th>
                          <th colspan=5>Giết mổ</th>
                          <th colspan=3>Tồn cuối</th>
                          <th colspan=" + SoCotThucAn.ToString() + @">Tiêu tốn thức ăn</th>
                          <th rowspan=3>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=4>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>");
                for (int ii = 0; ii < SoCotThucAn / 2; ii++)
                {
                    sb.Append(@"<th rowspan=2>Loại</th>
                          <th rowspan=2>SL (kg)</th>");
                }

                sb.Append(@"</tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>TP</th>
                          <th>LT</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>");
                sb.Append("</tr></thead><tbody>");
                int i = 0;
                //Cá loại 1 (cá con)
                DataRow[] r1 = new DataRow[6];
                for (int jj = 0; jj < 6; jj++)
                {
                    r1[jj] = dt.Rows[i * 6 + jj];
                }
                sb.Append("<tr>");
                sb.Append("<td style='vertical-align:middle;'>1.1</td>");

                int[,] GiaTri1 = new int[3, 7];
                int t1;

                //-------------------------------------------

                //GiaTri1[1, 0] - Giong Ton Dau
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 0] += Convert.ToInt32(r1[t1]["TonDau"]);// +Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
                }
                //GiaTri1[2, 0] - Tang Trong Ton Dau
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 0] += Convert.ToInt32(r1[t1]["TonDau"]);// +Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
                }
                //GiaTri1[0, 0] - Tong Ton Dau
                GiaTri1[0, 0] = GiaTri1[1, 0] + GiaTri1[2, 0];

                //-------------------------------------------

                //GiaTri1[1, 1] - Giong Nhap
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]);// - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
                }
                //GiaTri1[2, 1] - Tang Trong Nhap
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]);// - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
                }
                //GiaTri1[0, 1] - Tong Nhap
                GiaTri1[0, 1] = GiaTri1[1, 1] + GiaTri1[2, 1];

                //-------------------------------------------

                //GiaTri1[1, 2] - Giong Xuat
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGTO"]) + Convert.ToInt32(r1[t1]["Xuat_CGTI"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPLO"]) + Convert.ToInt32(r1[t1]["Xuat_CPLI"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
                }
                //GiaTri1[2, 2] - Tang Trong Xuat
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGTO"]) + Convert.ToInt32(r1[t1]["Xuat_CGTI"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPLO"]) + Convert.ToInt32(r1[t1]["Xuat_CPLI"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
                }
                //GiaTri1[0, 2] - Tong Xuat
                GiaTri1[0, 2] = GiaTri1[1, 2] + GiaTri1[2, 2];

                //-------------------------------------------

                //GiaTri1[1, 3] - Giong Chet
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 3] += Convert.ToInt32(r1[t1]["Chet"]);
                }
                //GiaTri1[2, 3] - Tang Trong Chet
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 3] += Convert.ToInt32(r1[t1]["Chet"]);
                }
                //GiaTri1[0, 3] - Tong Chet
                GiaTri1[0, 3] = GiaTri1[1, 3] + GiaTri1[2, 3];

                //-------------------------------------------

                //GiaTri1[1, 3] - Giong LoaiThai
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 4] += Convert.ToInt32(r1[t1]["LoaiThai"]);
                }
                //GiaTri1[2, 3] - Tang Trong Chet
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 4] += Convert.ToInt32(r1[t1]["LoaiThai"]);
                }
                //GiaTri1[0, 3] - Tong Chet
                GiaTri1[0, 4] = GiaTri1[1, 4] + GiaTri1[2, 4];

                //-------------------------------------------

                //GiaTri1[1, 4] - Giong Giet Mo
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 5] += Convert.ToInt32(r1[t1]["GietMo"]);
                }
                //GiaTri1[2, 4] - Tang Trong Giet Mo
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 5] += Convert.ToInt32(r1[t1]["GietMo"]);
                }
                //GiaTri1[0, 4] - Tong Giet Mo
                GiaTri1[0, 5] = GiaTri1[1, 5] + GiaTri1[2, 5];

                //-------------------------------------------

                //GiaTri1[1, 5] - Giong Ton Cuoi
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 6] += Convert.ToInt32(r1[t1]["TonCuoi"]);
                }
                //GiaTri1[2, 5] - Tang Trong Ton Cuoi
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 6] += Convert.ToInt32(r1[t1]["TonCuoi"]);
                }
                //GiaTri1[0, 5] - Tong Ton Cuoi
                GiaTri1[0, 6] = GiaTri1[1, 6] + GiaTri1[2, 6];

                //-------------------------------------------

                DataRow rTD = dtTD.Rows[0];
                sb.Append("<td align='left'>Cá GĐ úm</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 0] - Convert.ToInt32(rTD["TonDau"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 1] - (Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"])), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 2] - (Convert.ToInt32(rTD["Xuat_Ban"]) + Convert.ToInt32(rTD["Xuat_CLC"])), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 3] - Convert.ToInt32(rTD["Chet"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 4] - Convert.ToInt32(rTD["LoaiThai"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 5] - Convert.ToInt32(rTD["GietMo"]), 0) + "</td><td></td><td>" + Config.ToXVal2(GiaTri1[0, 5] - Convert.ToInt32(rTD["GietMo"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 6] - Convert.ToInt32(rTD["TonCuoi"]), 0) + "</td><td></td><td></td>");

                //ThucAn Start
                int j = 0;
                for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                {
                    DataRow rta = dtSub.Rows[h];
                    if (rta["LoaiCa"].ToString() == "1")
                    {
                        if (ListThucAnTD.Contains("@" + rta["ThucAn"].ToString() + "@"))
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(Convert.ToDecimal(rta["KhoiLuong"]) - GetKhoiLuongFromTable(dtTDAn, Convert.ToInt32(rta["ThucAn"])), scale) + "</td>");
                        else
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                        j++;
                    }
                    else
                    {
                        iThucAn = h;
                        break;
                    }
                }
                for (int t = j; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
                //ThucAn End
                sb.Append("<td></td>");
                sb.Append("</tr>");



                //Cá tận dụng
                sb.Append("<tr>");
                sb.Append("<td style='vertical-align:middle;'>1.2</td>");
                sb.Append("<td align='left'>Cá GĐ úm tận dụng</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonDau"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Xuat_Ban"]) + Convert.ToInt32(rTD["Xuat_CLC"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["Chet"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["LoaiThai"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["GietMo"], 0) + "</td><td></td><td>" + Config.ToXVal2(rTD["GietMo"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonCuoi"], 0) + "</td><td></td><td></td>");
                //ThucAn Start
                if (ListThucAnTD == "")
                {
                    for (int t = 0; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
                }
                else
                {
                    int o = 0;
                    for (o = 0; o < dtTDAn.Rows.Count; o++)
                    {
                        DataRow r = dtTDAn.Rows[o];
                        sb.Append("<td>" + r["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(r["KhoiLuong"], scale) + "</td>");
                    }
                    for (int t = o; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
                }
                //ThucAn End
                sb.Append("<td></td>");
                sb.Append("</tr>");


                //Cá từ loại 2 -> loại 4
                for (i = 1; i < 4; i++)
                {
                    DataRow[] r = new DataRow[6];
                    for (int jj = 0; jj < 6; jj++)
                    {
                        r[jj] = dt.Rows[i * 6 + jj];
                    }
                    sb.Append("<tr>");
                    int idx = i + 1;
                    sb.Append("<td style='vertical-align:middle;'>" + idx.ToString() + "</td>");
                    string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                    int[,] GiaTri = new int[3, 7];
                    int t;

                    //-------------------------------------------

                    //GiaTri[1, 0] - Giong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    //GiaTri[2, 0] - Tang Trong Ton Dau
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    //GiaTri[0, 0] - Tong Ton Dau
                    GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];

                    //-------------------------------------------

                    //GiaTri[1, 1] - Giong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    //GiaTri[2, 1] - Tang Trong Nhap
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    //GiaTri[0, 1] - Tong Nhap
                    GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];

                    //-------------------------------------------

                    //GiaTri[1, 2] - Giong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    //GiaTri[2, 2] - Tang Trong Xuat
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    //GiaTri[0, 2] - Tong Xuat
                    GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];

                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 3] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 3] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];

                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 4] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 4] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];

                    //-------------------------------------------

                    //GiaTri[1, 4] - Giong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 5] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    //GiaTri[2, 4] - Tang Trong Giet Mo
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 5] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    //GiaTri[0, 4] - Tong Giet Mo
                    GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];

                    //-------------------------------------------

                    //GiaTri[1, 5] - Giong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 6] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 6] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];

                    //-------------------------------------------

                    int iThucAnGoc = iThucAn;
                    for (t = 0; t < 3; t++)
                    {
                        if (t == 0)
                        {
                            sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 5; y++)
                            {
                                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                            }
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 5], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 3]["GietMo"]), 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 2]["GietMo"]), 0) + "</td><td></td><td></td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6], 0) + "</td><td></td><td></td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                    j++;
                                }
                                else
                                {
                                    iThucAn = h;
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            sb.Append("<td></td>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td>");
                            sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 5; y++)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                            }
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 5], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 2]["GietMo"], 0) + "</td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6], 0) + "</td><td></td><td></td>");
                            //
                            //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAnGoc; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    if (t == 1)
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                    }
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            //
                            sb.Append("<td></td>");
                        }
                        sb.Append("</tr>");
                    }
                }


                //Tổng hợp Cá hậu bị từ loại 5, -1
                for (int n = 0; n < 1; n++)
                {
                    DataRow[] r = new DataRow[6];
                    for (int jj = 0; jj < 6; jj++)
                    {
                        r[jj] = dt1.Rows[jj];
                    }
                    sb.Append("<tr>");
                    sb.Append("<td style='vertical-align:middle;'>5</td>");
                    string[] tenLoaiCa = new string[] { "Hậu bị", "Con giống", "Tăng trọng" };
                    int[,] GiaTri = new int[3, 21];
                    int t;

                    //-------------------------------------------

                    //GiaTri[1, 0] - Giong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[1, 1] += Convert.ToInt32(r[2]["TonDau"]);
                    GiaTri[1, 2] += Convert.ToInt32(r[1]["TonDau"]);
                    //GiaTri[2, 0] - Tang Trong Ton Dau
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[2, 1] += Convert.ToInt32(r[5]["TonDau"]);
                    GiaTri[2, 2] += Convert.ToInt32(r[4]["TonDau"]);
                    //GiaTri[0, 0] - Tong Ton Dau
                    GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
                    GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];
                    GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];
                    //-------------------------------------------

                    //GiaTri[1, 1] - Giong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[1, 4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]);
                    GiaTri[1, 5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]);
                    //GiaTri[2, 1] - Tang Trong Nhap
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[2, 4] += Convert.ToInt32(r[5]["Nhap_NhapChuong"]) + Convert.ToInt32(r[5]["Nhap_CLC"]) + Convert.ToInt32(r[5]["Nhap_CGTO"]) + Convert.ToInt32(r[5]["Nhap_CGTI"]) + Convert.ToInt32(r[5]["Nhap_CGT_D"]) + Convert.ToInt32(r[5]["Nhap_CPLO"]) + Convert.ToInt32(r[5]["Nhap_CPLI"]) + Convert.ToInt32(r[5]["Nhap_CPL_D"]) + Convert.ToInt32(r[5]["Nhap_CTT"]);
                    GiaTri[2, 5] += Convert.ToInt32(r[4]["Nhap_NhapChuong"]) + Convert.ToInt32(r[4]["Nhap_CLC"]) + Convert.ToInt32(r[4]["Nhap_CGTO"]) + Convert.ToInt32(r[4]["Nhap_CGTI"]) + Convert.ToInt32(r[4]["Nhap_CGT_D"]) + Convert.ToInt32(r[4]["Nhap_CPLO"]) + Convert.ToInt32(r[4]["Nhap_CPLI"]) + Convert.ToInt32(r[4]["Nhap_CPL_D"]) + Convert.ToInt32(r[4]["Nhap_CTT"]);
                    //GiaTri[0, 1] - Tong Nhap
                    GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
                    GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];
                    GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];
                    //-------------------------------------------

                    //GiaTri[1, 2] - Giong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[1, 7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGTO"]) + Convert.ToInt32(r[2]["Xuat_CGTI"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPLO"]) + Convert.ToInt32(r[2]["Xuat_CPLI"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);
                    GiaTri[1, 8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGTO"]) + Convert.ToInt32(r[1]["Xuat_CGTI"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPLO"]) + Convert.ToInt32(r[1]["Xuat_CPLI"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                    //GiaTri[2, 2] - Tang Trong Xuat
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[2, 7] += Convert.ToInt32(r[5]["Xuat_Ban"]) + Convert.ToInt32(r[5]["Xuat_CLC"]) + Convert.ToInt32(r[5]["Xuat_CGTO"]) + Convert.ToInt32(r[5]["Xuat_CGTI"]) + Convert.ToInt32(r[5]["Xuat_CGT_D"]) + Convert.ToInt32(r[5]["Xuat_CPLO"]) + Convert.ToInt32(r[5]["Xuat_CPLI"]) + Convert.ToInt32(r[5]["Xuat_CPL_D"]);
                    GiaTri[2, 8] += Convert.ToInt32(r[4]["Xuat_Ban"]) + Convert.ToInt32(r[4]["Xuat_CLC"]) + Convert.ToInt32(r[4]["Xuat_CGTO"]) + Convert.ToInt32(r[4]["Xuat_CGTI"]) + Convert.ToInt32(r[4]["Xuat_CGT_D"]) + Convert.ToInt32(r[4]["Xuat_CPLO"]) + Convert.ToInt32(r[4]["Xuat_CPLI"]) + Convert.ToInt32(r[4]["Xuat_CPL_D"]);
                    //GiaTri[0, 2] - Tong Xuat
                    GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
                    GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];
                    GiaTri[0, 8] = GiaTri[1, 8] + GiaTri[2, 8];
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[1, 10] += Convert.ToInt32(r[2]["Chet"]);
                    GiaTri[1, 11] += Convert.ToInt32(r[1]["Chet"]);
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[2, 10] += Convert.ToInt32(r[5]["Chet"]);
                    GiaTri[2, 11] += Convert.ToInt32(r[4]["Chet"]);
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
                    GiaTri[0, 10] = GiaTri[1, 10] + GiaTri[2, 10];
                    GiaTri[0, 11] = GiaTri[1, 11] + GiaTri[2, 11];
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[1, 13] += Convert.ToInt32(r[2]["LoaiThai"]);
                    GiaTri[1, 14] += Convert.ToInt32(r[1]["LoaiThai"]);
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[2, 13] += Convert.ToInt32(r[5]["LoaiThai"]);
                    GiaTri[2, 14] += Convert.ToInt32(r[4]["LoaiThai"]);
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
                    GiaTri[0, 13] = GiaTri[1, 13] + GiaTri[2, 13];
                    GiaTri[0, 14] = GiaTri[1, 14] + GiaTri[2, 14];
                    //-------------------------------------------

                    //GiaTri[1, 4] - Giong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[1, 16] += Convert.ToInt32(r[2]["GietMo"]);
                    GiaTri[1, 17] += Convert.ToInt32(r[1]["GietMo"]);
                    //GiaTri[2, 4] - Tang Trong Giet Mo
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[2, 16] += Convert.ToInt32(r[5]["GietMo"]);
                    GiaTri[2, 17] += Convert.ToInt32(r[4]["GietMo"]);
                    //GiaTri[0, 4] - Tong Giet Mo
                    GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
                    GiaTri[0, 16] = GiaTri[1, 16] + GiaTri[2, 16];
                    GiaTri[0, 17] = GiaTri[1, 17] + GiaTri[2, 17];
                    //-------------------------------------------

                    //GiaTri[1, 5] - Giong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[1, 19] += Convert.ToInt32(r[2]["TonCuoi"]);
                    GiaTri[1, 20] += Convert.ToInt32(r[1]["TonCuoi"]);
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[2, 19] += Convert.ToInt32(r[5]["TonCuoi"]);
                    GiaTri[2, 20] += Convert.ToInt32(r[4]["TonCuoi"]);
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
                    GiaTri[0, 19] = GiaTri[1, 19] + GiaTri[2, 19];
                    GiaTri[0, 20] = GiaTri[1, 20] + GiaTri[2, 20];
                    //-------------------------------------------

                    int iThucAnGoc = iThucAn;
                    for (t = 0; t < 3; t++)
                    {
                        if (t == 0)
                        {
                            sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1.Rows[1]["GietMo"]) + Convert.ToInt32(dtPLGM1.Rows[3]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1.Rows[0]["GietMo"]) + Convert.ToInt32(dtPLGM1.Rows[2]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = 0; h < dtSub1.Rows.Count; h++)
                            {
                                DataRow rta = dtSub1.Rows[h];
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                j++;
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            sb.Append("<td></td>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td>");
                            sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM1.Rows[t * 2 - 1]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM1.Rows[t * 2 - 2]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //
                            //ThucAn Start
                            j = 0;
                            for (int h = 0; h < dtSub1.Rows.Count; h++)
                            {
                                DataRow rta = dtSub1.Rows[h];
                                if (t == 1)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                }
                                j++;
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            //
                            sb.Append("<td></td>");
                        }
                        sb.Append("</tr>");
                    }
                }

                //Cá loại 5, -1, 6
                for (i = 4; i < 7; i++)
                {
                    DataRow[] r = new DataRow[6];
                    for (int jj = 0; jj < 6; jj++)
                    {
                        r[jj] = dt.Rows[i * 6 + jj];
                    }
                    sb.Append("<tr>");
                    if(i==4)
                        sb.Append("<td style='vertical-align:middle;'>5.1</td>");
                    else if(i==5)
                        sb.Append("<td style='vertical-align:middle;'>5.2</td>");
                    else
                        sb.Append("<td style='vertical-align:middle;'>" + i.ToString() + "</td>");
                    string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                    int[,] GiaTri = new int[3, 21];
                    int t;

                    //-------------------------------------------

                    //GiaTri[1, 0] - Giong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[1, 1] += Convert.ToInt32(r[2]["TonDau"]);
                    GiaTri[1, 2] += Convert.ToInt32(r[1]["TonDau"]);
                    //GiaTri[2, 0] - Tang Trong Ton Dau
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[2, 1] += Convert.ToInt32(r[5]["TonDau"]);
                    GiaTri[2, 2] += Convert.ToInt32(r[4]["TonDau"]);
                    //GiaTri[0, 0] - Tong Ton Dau
                    GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
                    GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];
                    GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];
                    //-------------------------------------------

                    //GiaTri[1, 1] - Giong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[1, 4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]);
                    GiaTri[1, 5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]);
                    //GiaTri[2, 1] - Tang Trong Nhap
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[2, 4] += Convert.ToInt32(r[5]["Nhap_NhapChuong"]) + Convert.ToInt32(r[5]["Nhap_CLC"]) + Convert.ToInt32(r[5]["Nhap_CGTO"]) + Convert.ToInt32(r[5]["Nhap_CGTI"]) + Convert.ToInt32(r[5]["Nhap_CGT_D"]) + Convert.ToInt32(r[5]["Nhap_CPLO"]) + Convert.ToInt32(r[5]["Nhap_CPLI"]) + Convert.ToInt32(r[5]["Nhap_CPL_D"]) + Convert.ToInt32(r[5]["Nhap_CTT"]);
                    GiaTri[2, 5] += Convert.ToInt32(r[4]["Nhap_NhapChuong"]) + Convert.ToInt32(r[4]["Nhap_CLC"]) + Convert.ToInt32(r[4]["Nhap_CGTO"]) + Convert.ToInt32(r[4]["Nhap_CGTI"]) + Convert.ToInt32(r[4]["Nhap_CGT_D"]) + Convert.ToInt32(r[4]["Nhap_CPLO"]) + Convert.ToInt32(r[4]["Nhap_CPLI"]) + Convert.ToInt32(r[4]["Nhap_CPL_D"]) + Convert.ToInt32(r[4]["Nhap_CTT"]);
                    //GiaTri[0, 1] - Tong Nhap
                    GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
                    GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];
                    GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];
                    //-------------------------------------------

                    //GiaTri[1, 2] - Giong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[1, 7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGTO"]) + Convert.ToInt32(r[2]["Xuat_CGTI"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPLO"]) + Convert.ToInt32(r[2]["Xuat_CPLI"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);
                    GiaTri[1, 8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGTO"]) + Convert.ToInt32(r[1]["Xuat_CGTI"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPLO"]) + Convert.ToInt32(r[1]["Xuat_CPLI"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                    //GiaTri[2, 2] - Tang Trong Xuat
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[2, 7] += Convert.ToInt32(r[5]["Xuat_Ban"]) + Convert.ToInt32(r[5]["Xuat_CLC"]) + Convert.ToInt32(r[5]["Xuat_CGTO"]) + Convert.ToInt32(r[5]["Xuat_CGTI"]) + Convert.ToInt32(r[5]["Xuat_CGT_D"]) + Convert.ToInt32(r[5]["Xuat_CPLO"]) + Convert.ToInt32(r[5]["Xuat_CPLI"]) + Convert.ToInt32(r[5]["Xuat_CPL_D"]);
                    GiaTri[2, 8] += Convert.ToInt32(r[4]["Xuat_Ban"]) + Convert.ToInt32(r[4]["Xuat_CLC"]) + Convert.ToInt32(r[4]["Xuat_CGTO"]) + Convert.ToInt32(r[4]["Xuat_CGTI"]) + Convert.ToInt32(r[4]["Xuat_CGT_D"]) + Convert.ToInt32(r[4]["Xuat_CPLO"]) + Convert.ToInt32(r[4]["Xuat_CPLI"]) + Convert.ToInt32(r[4]["Xuat_CPL_D"]);
                    //GiaTri[0, 2] - Tong Xuat
                    GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
                    GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];
                    GiaTri[0, 8] = GiaTri[1, 8] + GiaTri[2, 8];
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[1, 10] += Convert.ToInt32(r[2]["Chet"]);
                    GiaTri[1, 11] += Convert.ToInt32(r[1]["Chet"]);
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[2, 10] += Convert.ToInt32(r[5]["Chet"]);
                    GiaTri[2, 11] += Convert.ToInt32(r[4]["Chet"]);
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
                    GiaTri[0, 10] = GiaTri[1, 10] + GiaTri[2, 10];
                    GiaTri[0, 11] = GiaTri[1, 11] + GiaTri[2, 11];
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[1, 13] += Convert.ToInt32(r[2]["LoaiThai"]);
                    GiaTri[1, 14] += Convert.ToInt32(r[1]["LoaiThai"]);
                    //GiaTri[2, 4] - Tang Trong Giet Mo
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[2, 13] += Convert.ToInt32(r[5]["LoaiThai"]);
                    GiaTri[2, 14] += Convert.ToInt32(r[4]["LoaiThai"]);
                    //GiaTri[0, 4] - Tong Giet Mo
                    GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
                    GiaTri[0, 13] = GiaTri[1, 13] + GiaTri[2, 13];
                    GiaTri[0, 14] = GiaTri[1, 14] + GiaTri[2, 14];
                    //-------------------------------------------

                    //GiaTri[1, 4] - Giong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[1, 16] += Convert.ToInt32(r[2]["GietMo"]);
                    GiaTri[1, 17] += Convert.ToInt32(r[1]["GietMo"]);
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[2, 16] += Convert.ToInt32(r[5]["GietMo"]);
                    GiaTri[2, 17] += Convert.ToInt32(r[4]["GietMo"]);
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
                    GiaTri[0, 16] = GiaTri[1, 16] + GiaTri[2, 16];
                    GiaTri[0, 17] = GiaTri[1, 17] + GiaTri[2, 17];
                    //-------------------------------------------

                    //GiaTri[1, 5] - Giong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[1, 19] += Convert.ToInt32(r[2]["TonCuoi"]);
                    GiaTri[1, 20] += Convert.ToInt32(r[1]["TonCuoi"]);
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[2, 19] += Convert.ToInt32(r[5]["TonCuoi"]);
                    GiaTri[2, 20] += Convert.ToInt32(r[4]["TonCuoi"]);
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
                    GiaTri[0, 19] = GiaTri[1, 19] + GiaTri[2, 19];
                    GiaTri[0, 20] = GiaTri[1, 20] + GiaTri[2, 20];
                    //-------------------------------------------

                    int iThucAnGoc = iThucAn;
                    for (t = 0; t < 3; t++)
                    {
                        if (t == 0)
                        {
                            sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 3]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 2]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                    j++;
                                }
                                else
                                {
                                    iThucAn = h;
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            sb.Append("<td></td>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td>");
                            sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 2]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //
                            //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAnGoc; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    if (t == 1)
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                    }
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            //
                            sb.Append("<td></td>");
                        }
                        sb.Append("</tr>");
                    }
                }

                //Cá từ loại 7 -> loại 11
                for (i = 7; i < 12; i++)
                {
                    DataRow[] r = new DataRow[3];
                    for (int jj = 0; jj < 3; jj++)
                    {
                        r[jj] = dt.Rows[i * 6 + jj];
                    }
                    sb.Append("<tr>");
                    sb.Append("<td>" + i.ToString() + "</td>");
                    sb.Append("<td align='left'>" + r[0]["TenLoaiCa"].ToString() + "</td>");
                    int[] GiaTri = new int[21];
                    int t;

                    //-------------------------------------------

                    //GiaTri[0] - Tong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    //GiaTri[2] - Cai Ton Dau
                    GiaTri[2] += Convert.ToInt32(r[1]["TonDau"]);// +Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) - Convert.ToInt32(r[1]["Xuat_CGTO"]) - Convert.ToInt32(r[1]["Xuat_CPLO"]);
                    //GiaTri[1] - Duc Ton Dau
                    GiaTri[1] += Convert.ToInt32(r[2]["TonDau"]);// +Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) - Convert.ToInt32(r[2]["Xuat_CGTO"]) - Convert.ToInt32(r[2]["Xuat_CPLO"]);

                    //-------------------------------------------

                    //GiaTri[3] - Tong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CGTI"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]);
                    }
                    //GiaTri[5] - Cai Nhap
                    GiaTri[5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]);// -Convert.ToInt32(r[1]["Xuat_CGTI"]) - Convert.ToInt32(r[1]["Xuat_CPLI"]);
                    //GiaTri[4] - Duc Nhap
                    GiaTri[4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]);// -Convert.ToInt32(r[2]["Xuat_CGTI"]) - Convert.ToInt32(r[2]["Xuat_CPLI"]);

                    //-------------------------------------------

                    //GiaTri[6] - Tong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    //GiaTri[8] - Cai Xuat
                    GiaTri[8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGTO"]) + Convert.ToInt32(r[1]["Xuat_CGTI"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPLO"]) + Convert.ToInt32(r[1]["Xuat_CPLI"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                    //GiaTri[7] - Duc Xuat
                    GiaTri[7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGTO"]) + Convert.ToInt32(r[2]["Xuat_CGTI"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPLO"]) + Convert.ToInt32(r[2]["Xuat_CPLI"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);

                    //-------------------------------------------

                    //GiaTri[9] - Tong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    //GiaTri[11] - Cai Chet
                    GiaTri[11] += Convert.ToInt32(r[1]["Chet"]);
                    //GiaTri[10] - Duc Chet
                    GiaTri[10] += Convert.ToInt32(r[2]["Chet"]);

                    //-------------------------------------------

                    //GiaTri[9] - Tong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    //GiaTri[14] - Cai Giet Mo
                    GiaTri[14] += Convert.ToInt32(r[1]["LoaiThai"]);
                    //GiaTri[13] - Duc Giet Mo
                    GiaTri[13] += Convert.ToInt32(r[2]["LoaiThai"]);

                    //-------------------------------------------

                    //GiaTri[12] - Tong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    //GiaTri[17] - Cai Ton Cuoi
                    GiaTri[17] += Convert.ToInt32(r[1]["GietMo"]);
                    //GiaTri[16] - Duc Ton Cuoi
                    GiaTri[16] += Convert.ToInt32(r[2]["GietMo"]);

                    //-------------------------------------------

                    //GiaTri[15] - Tong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    //GiaTri[17] - Cai Ton Cuoi
                    GiaTri[20] += Convert.ToInt32(r[1]["TonCuoi"]);
                    //GiaTri[16] - Duc Ton Cuoi
                    GiaTri[19] += Convert.ToInt32(r[2]["TonCuoi"]);

                    //-------------------------------------------
                    for (int y = 0; y < 5; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 1], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 2], 0) + "</td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[15], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + 1]["GietMo"], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4]["GietMo"], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[16], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[17], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[18], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[19], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[20], 0) + "</td>");
                    //ThucAn Start
                    j = 0;
                    for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                    {
                        DataRow rta = dtSub.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                        {
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                            j++;
                        }
                        else
                        {
                            iThucAn = h;
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                }
                //Tính tổng
                int[] Tong = new int[21];
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    Tong[0] += Convert.ToInt32(r["TonDau"]);// +Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[2] += Convert.ToInt32(r["TonDau"]);// +Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[1] += Convert.ToInt32(r["TonDau"]);// +Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                    }

                    Tong[3] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLO"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]);// -Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[5] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLO"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]);// -Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[4] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLO"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]);// -Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                    }

                    Tong[6] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGTO"]) + Convert.ToInt32(r["Xuat_CGTI"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPLO"]) + Convert.ToInt32(r["Xuat_CPLI"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[8] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGTO"]) + Convert.ToInt32(r["Xuat_CGTI"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPLO"]) + Convert.ToInt32(r["Xuat_CPLI"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[7] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGTO"]) + Convert.ToInt32(r["Xuat_CGTI"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPLO"]) + Convert.ToInt32(r["Xuat_CPLI"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                    }

                    Tong[9] += Convert.ToInt32(r["Chet"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[11] += Convert.ToInt32(r["Chet"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[10] += Convert.ToInt32(r["Chet"]);
                    }

                    Tong[12] += Convert.ToInt32(r["LoaiThai"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[14] += Convert.ToInt32(r["LoaiThai"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[13] += Convert.ToInt32(r["LoaiThai"]);
                    }

                    Tong[15] += Convert.ToInt32(r["GietMo"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[17] += Convert.ToInt32(r["GietMo"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[16] += Convert.ToInt32(r["GietMo"]);
                    }

                    Tong[18] += Convert.ToInt32(r["TonCuoi"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[20] += Convert.ToInt32(r["TonCuoi"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[19] += Convert.ToInt32(r["TonCuoi"]);
                    }
                }
                //Tính tổng phân loại giêt mô
                int[] TongPLGM = new int[2];
                for (i = 0; i < dtPLGM.Rows.Count; i++)
                {
                    DataRow r = dtPLGM.Rows[i];
                    if (i % 2 == 1)
                    {
                        TongPLGM[1] += Convert.ToInt32(r["GietMo"]);
                    }
                    else
                    {
                        TongPLGM[0] += Convert.ToInt32(r["GietMo"]);
                    }
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td></td><td>Tổng cộng</td>");
                for (i = 0; i < 15; i++)
                {
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[i], 0) + "</td>");
                }
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[15], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TongPLGM[1], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TongPLGM[0], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[16], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[17], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[18], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[19], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[20], 0) + "</td>");

                for (i = 0; i < SoCotThucAn; i++)
                {
                    sb.Append(@"<td></td>");
                }
                sb.Append("<td></td></tr>");
                sb.Append("</tbody>");
                sb.Append("</table><br/><div style='text-align:right;font-style:italic;'>Ninh Ích, ngày&nbsp;&nbsp;&nbsp;tháng&nbsp;&nbsp;&nbsp;&nbsp;năm&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div></tr>");
                sb.Append("<br/><div style='font-weight:bold;text-align:left;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Giám đốc&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Kế toán trưởng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;BP. Theo dõi&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Lãnh đạo trại</div></body></html>");
                Response.Write(sb.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            if (txtFromDate.Text == "")
            {
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            DateTime dtFrom = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"));
            if (dtFrom.Day == 1 && dtFrom.Month == 10) btnView_FromFirstOct_Click();
            else btnView_Others_Click();
        }

        protected void btnView_FromFirstOct_Click()
        {
            try
            {
                int iThucAn = 0;
                string tieude = "";
                string strSQL = "QLCS_BCTK_BienDongDan_JoinMauBC";
                string strSQL_PLGM = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_oldway";
                string strSubSQL = "QLCS_BCTK_CaAn";
                string strMaxSQL = "QLCS_BCTK_MaxSoLoaiThucAn";
                string strSQLTD = "QLCS_BCTK_BienDongDan_CaTanDung";
                string strSQLTDAn = "QLCS_BCTK_CaKhongMeAn";
                SqlParameter[] param = new SqlParameter[2];
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
                tieude += "<b>BẢNG THEO DÕI BIẾN ĐỘNG TỔNG ĐÀN CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                SqlParameter[] paramPLGM = new SqlParameter[2];
                paramPLGM[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                paramPLGM[1] = new SqlParameter("@dTo", txtToDate.Text);
                DataSet dsPLGM = Config.SelectSPs(strSQL_PLGM, paramPLGM);
                DataTable dtPLGM = dsPLGM.Tables[0];
                DataTable dtPLGM1 = dsPLGM.Tables[1];

                SqlParameter[] paramSub = new SqlParameter[2];
                paramSub[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                paramSub[1] = new SqlParameter("@dTo", txtToDate.Text);
                DataSet dsSub = Config.SelectSPs(strSubSQL, paramSub);
                DataTable dtSub = dsSub.Tables[0];
                DataTable dtSub1 = dsSub.Tables[1];
                DataTable dtSub2 = dsSub.Tables[2];
                SqlParameter[] param2 = new SqlParameter[2];
                param2[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param2[1] = new SqlParameter("@dTo", txtToDate.Text);
                DataTable dtMax = DotNetNuke.NewsProvider.DataProvider.SelectSP(strMaxSQL, param2);
                DataTable dtTD = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTD, param2);
                SqlParameter[] param1 = new SqlParameter[4];
                param1[0] = new SqlParameter("@LoaiCa", 1);
                param1[1] = new SqlParameter("@dFrom", txtFromDate.Text);
                param1[2] = new SqlParameter("@dTo", txtToDate.Text);
                param1[3] = new SqlParameter("@ListThucAn", "");
                param1[3].Direction = ParameterDirection.Output;
                param1[3].Size = 4000;
                DataTable dtTDAn = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTDAn, param1);
                string ListThucAnTD = param1[3].Value.ToString();
                int SoCotThucAn = 2;
                if (dtMax != null && dtMax.Rows.Count == 1 && dtMax.Rows[0]["MaxSoLoaiThucAn"] != DBNull.Value) SoCotThucAn = Convert.ToInt32(dtMax.Rows[0]["MaxSoLoaiThucAn"]) * 2;
                if (Convert.ToInt32(dtSub2.Rows[0]["MaxSoLoaiThucAnHauBi"]) * 2 > SoCotThucAn) SoCotThucAn = Convert.ToInt32(dtSub2.Rows[0]["MaxSoLoaiThucAnHauBi"]) * 2;
                if (SoCotThucAn < 2) SoCotThucAn = 2;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=3>STT</th>
                          <th rowspan=3>Nội dung</th>
                          <th colspan=3>Tồn đầu</th>
                          <th colspan=3>Nhập</th>
                          <th colspan=3>Xuất</th>
                          <th colspan=3>Chết</th>
                          <th colspan=3>Loại thải</th>
                          <th colspan=5>Giết mổ</th>
                          <th colspan=3>Tồn cuối</th>
                          <th colspan=" + SoCotThucAn.ToString() + @">Tiêu tốn thức ăn</th>
                          <th rowspan=3>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=4>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>");
                for (int ii = 0; ii < SoCotThucAn / 2; ii++)
                {
                    sb.Append(@"<th rowspan=2>Loại</th>
                          <th rowspan=2>SL (kg)</th>");
                }

                sb.Append(@"</tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>TP</th>
                          <th>LT</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>");
                sb.Append("</tr></thead><tbody>");
                int i = 0;
                //Cá loại 1 (cá con)
                DataRow[] r1 = new DataRow[6];
                for (int jj = 0; jj < 6; jj++)
                {
                    r1[jj] = dt.Rows[i * 6 + jj];
                }
                sb.Append("<tr>");
                sb.Append("<td style='vertical-align:middle;'>1.1</td>");

                int[,] GiaTri1 = new int[3, 7];
                int t1;

                //-------------------------------------------

                //GiaTri1[1, 0] - Giong Ton Dau
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 0] += Convert.ToInt32(r1[t1]["TonDau"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
                }
                //GiaTri1[2, 0] - Tang Trong Ton Dau
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 0] += Convert.ToInt32(r1[t1]["TonDau"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
                }
                //GiaTri1[0, 0] - Tong Ton Dau
                GiaTri1[0, 0] = GiaTri1[1, 0] + GiaTri1[2, 0];

                //-------------------------------------------

                //GiaTri1[1, 1] - Giong Nhap
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]) - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
                }
                //GiaTri1[2, 1] - Tang Trong Nhap
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]) - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
                }
                //GiaTri1[0, 1] - Tong Nhap
                GiaTri1[0, 1] = GiaTri1[1, 1] + GiaTri1[2, 1];

                //-------------------------------------------

                //GiaTri1[1, 2] - Giong Xuat
                for (t1 = 0; t1 < 3; t1++)
                {
                    //GiaTri1[1, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]) - Convert.ToInt32(r1[t1]["Nhap_CGT"]) - Convert.ToInt32(r1[t1]["Nhap_CPL"]);
                    GiaTri1[1, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
                }
                //GiaTri1[2, 2] - Tang Trong Xuat
                for (t1 = 3; t1 < 6; t1++)
                {
                    //GiaTri1[2, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]) - Convert.ToInt32(r1[t1]["Nhap_CGT"]) - Convert.ToInt32(r1[t1]["Nhap_CPL"]);
                    GiaTri1[2, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
                }
                //GiaTri1[0, 2] - Tong Xuat
                GiaTri1[0, 2] = GiaTri1[1, 2] + GiaTri1[2, 2];

                //-------------------------------------------

                //GiaTri1[1, 3] - Giong Chet
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 3] += Convert.ToInt32(r1[t1]["Chet"]);
                }
                //GiaTri1[2, 3] - Tang Trong Chet
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 3] += Convert.ToInt32(r1[t1]["Chet"]);
                }
                //GiaTri1[0, 3] - Tong Chet
                GiaTri1[0, 3] = GiaTri1[1, 3] + GiaTri1[2, 3];

                //-------------------------------------------

                //GiaTri1[1, 3] - Giong LoaiThai
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 4] += Convert.ToInt32(r1[t1]["LoaiThai"]);
                }
                //GiaTri1[2, 3] - Tang Trong Chet
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 4] += Convert.ToInt32(r1[t1]["LoaiThai"]);
                }
                //GiaTri1[0, 3] - Tong Chet
                GiaTri1[0, 4] = GiaTri1[1, 4] + GiaTri1[2, 4];

                //-------------------------------------------

                //GiaTri1[1, 4] - Giong Giet Mo
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 5] += Convert.ToInt32(r1[t1]["GietMo"]);
                }
                //GiaTri1[2, 4] - Tang Trong Giet Mo
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 5] += Convert.ToInt32(r1[t1]["GietMo"]);
                }
                //GiaTri1[0, 4] - Tong Giet Mo
                GiaTri1[0, 5] = GiaTri1[1, 5] + GiaTri1[2, 5];

                //-------------------------------------------

                //GiaTri1[1, 5] - Giong Ton Cuoi
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 6] += Convert.ToInt32(r1[t1]["TonCuoi"]);
                }
                //GiaTri1[2, 5] - Tang Trong Ton Cuoi
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 6] += Convert.ToInt32(r1[t1]["TonCuoi"]);
                }
                //GiaTri1[0, 5] - Tong Ton Cuoi
                GiaTri1[0, 6] = GiaTri1[1, 6] + GiaTri1[2, 6];

                //-------------------------------------------

                DataRow rTD = dtTD.Rows[0];
                sb.Append("<td align='left'>Cá GĐ úm</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 0] - Convert.ToInt32(rTD["TonDau"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 1] - (Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"])), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 2] - (Convert.ToInt32(rTD["Xuat_Ban"]) + Convert.ToInt32(rTD["Xuat_CLC"])), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 3] - Convert.ToInt32(rTD["Chet"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 4] - Convert.ToInt32(rTD["LoaiThai"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 5] - Convert.ToInt32(rTD["GietMo"]), 0) + "</td><td></td><td>" + Config.ToXVal2(GiaTri1[0, 5] - Convert.ToInt32(rTD["GietMo"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 6] - Convert.ToInt32(rTD["TonCuoi"]), 0) + "</td><td></td><td></td>");

                //ThucAn Start
                int j = 0;
                for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                {
                    DataRow rta = dtSub.Rows[h];
                    if (rta["LoaiCa"].ToString() == "1")
                    {
                        if(ListThucAnTD.Contains("@" + rta["ThucAn"].ToString() + "@"))
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(Convert.ToDecimal(rta["KhoiLuong"]) - GetKhoiLuongFromTable(dtTDAn, Convert.ToInt32(rta["ThucAn"])), scale) + "</td>");
                        else
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                        j++;
                    }
                    else
                    {
                        iThucAn = h;
                        break;
                    }
                }
                for (int t = j; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
                //ThucAn End
                sb.Append("<td></td>");
                sb.Append("</tr>");

                
                
                
                //Cá tận dụng
                sb.Append("<tr>");
                sb.Append("<td style='vertical-align:middle;'>1.2</td>");
                sb.Append("<td align='left'>Cá GĐ úm tận dụng</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonDau"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Xuat_Ban"]) + Convert.ToInt32(rTD["Xuat_CLC"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["Chet"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["LoaiThai"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["GietMo"], 0) + "</td><td></td><td>" + Config.ToXVal2(rTD["GietMo"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonCuoi"], 0) + "</td><td></td><td></td>");
                //ThucAn Start
                if(ListThucAnTD == "")
                {
                    for (int t = 0; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
                }
                else
                {
                    int o = 0;
                    for (o = 0; o < dtTDAn.Rows.Count; o++)
                    {
                        DataRow r = dtTDAn.Rows[o];
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["KhoiLuong"], scale) + "</td>");
                    }
                    for (int t = o; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
                }
                //ThucAn End
                sb.Append("<td></td>");
                sb.Append("</tr>");


                //Cá từ loại 2 -> loại 4
                for (i = 1; i < 4; i++)
                {
                    DataRow[] r = new DataRow[6];
                    for (int jj = 0; jj < 6; jj++)
                    {
                        r[jj] = dt.Rows[i * 6 + jj];
                    }
                    sb.Append("<tr>");
                    int idx = i + 1;
                    sb.Append("<td style='vertical-align:middle;'>" + idx.ToString() + "</td>");
                    string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                    int[,] GiaTri = new int[3, 7];
                    int t;

                    //-------------------------------------------

                    //GiaTri[1, 0] - Giong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    //GiaTri[2, 0] - Tang Trong Ton Dau
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    //GiaTri[0, 0] - Tong Ton Dau
                    GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];

                    //-------------------------------------------

                    //GiaTri[1, 1] - Giong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    //GiaTri[2, 1] - Tang Trong Nhap
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    //GiaTri[0, 1] - Tong Nhap
                    GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];

                    //-------------------------------------------

                    //GiaTri[1, 2] - Giong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        //GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                        GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    //GiaTri[2, 2] - Tang Trong Xuat
                    for (t = 3; t < 6; t++)
                    {
                        //GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                        GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    //GiaTri[0, 2] - Tong Xuat
                    GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];

                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 3] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 3] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];

                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 4] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 4] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];

                    //-------------------------------------------

                    //GiaTri[1, 4] - Giong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 5] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    //GiaTri[2, 4] - Tang Trong Giet Mo
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 5] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    //GiaTri[0, 4] - Tong Giet Mo
                    GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];

                    //-------------------------------------------

                    //GiaTri[1, 5] - Giong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 6] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 6] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];

                    //-------------------------------------------

                    int iThucAnGoc = iThucAn;
                    for (t = 0; t < 3; t++)
                    {
                        if (t == 0)
                        {
                            sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 5; y++)
                            {
                                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                            }
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 5], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 3]["GietMo"]), 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 2]["GietMo"]), 0) + "</td><td></td><td></td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6], 0) + "</td><td></td><td></td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                    j++;
                                }
                                else
                                {
                                    iThucAn = h;
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            sb.Append("<td></td>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td>");
                            sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 5; y++)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                            }
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 5], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 2]["GietMo"], 0) + "</td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6], 0) + "</td><td></td><td></td>");
                            //
                            //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAnGoc; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    if (t == 1)
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                    }
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            //
                            sb.Append("<td></td>");
                        }
                        sb.Append("</tr>");
                    }
                }



                //Tong hop ca hau bi tu loai 5 va -1
                for (int n = 0; n < 1; n++)
                {
                    DataRow[] r = new DataRow[6];
                    for (int jj = 0; jj < 6; jj++)
                    {
                        r[jj] = dt1.Rows[jj];
                    }
                    sb.Append("<tr>");
                    sb.Append("<td style='vertical-align:middle;'>5</td>");
                    string[] tenLoaiCa = new string[] { "Hậu bị", "Con giống", "Tăng trọng" };
                    int[,] GiaTri = new int[3, 21];
                    int t;

                    //-------------------------------------------

                    //GiaTri[1, 0] - Giong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[1, 1] += Convert.ToInt32(r[2]["TonDau"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) - Convert.ToInt32(r[2]["Xuat_CGTO"]) - Convert.ToInt32(r[2]["Xuat_CPLO"]);
                    GiaTri[1, 2] += Convert.ToInt32(r[1]["TonDau"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) - Convert.ToInt32(r[1]["Xuat_CGTO"]) - Convert.ToInt32(r[1]["Xuat_CPLO"]);
                    //GiaTri[2, 0] - Tang Trong Ton Dau
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[2, 1] += Convert.ToInt32(r[5]["TonDau"]) + Convert.ToInt32(r[5]["Nhap_CGTO"]) + Convert.ToInt32(r[5]["Nhap_CPLO"]) - Convert.ToInt32(r[5]["Xuat_CGTO"]) - Convert.ToInt32(r[5]["Xuat_CPLO"]);
                    GiaTri[2, 2] += Convert.ToInt32(r[4]["TonDau"]) + Convert.ToInt32(r[4]["Nhap_CGTO"]) + Convert.ToInt32(r[4]["Nhap_CPLO"]) - Convert.ToInt32(r[4]["Xuat_CGTO"]) - Convert.ToInt32(r[4]["Xuat_CPLO"]);
                    //GiaTri[0, 0] - Tong Ton Dau
                    GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
                    GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];
                    GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];
                    //-------------------------------------------

                    //GiaTri[1, 1] - Giong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[1, 4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]) - Convert.ToInt32(r[2]["Xuat_CPLI"]) - Convert.ToInt32(r[2]["Xuat_CGTI"]);
                    GiaTri[1, 5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]) - Convert.ToInt32(r[1]["Xuat_CPLI"]) - Convert.ToInt32(r[1]["Xuat_CGTI"]);
                    //GiaTri[2, 1] - Tang Trong Nhap
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[2, 4] += Convert.ToInt32(r[5]["Nhap_NhapChuong"]) + Convert.ToInt32(r[5]["Nhap_CLC"]) + Convert.ToInt32(r[5]["Nhap_CGTI"]) + Convert.ToInt32(r[5]["Nhap_CGT_D"]) + Convert.ToInt32(r[5]["Nhap_CPLI"]) + Convert.ToInt32(r[5]["Nhap_CPL_D"]) + Convert.ToInt32(r[5]["Nhap_CTT"]) - Convert.ToInt32(r[5]["Xuat_CPLI"]) - Convert.ToInt32(r[5]["Xuat_CGTI"]);
                    GiaTri[2, 5] += Convert.ToInt32(r[4]["Nhap_NhapChuong"]) + Convert.ToInt32(r[4]["Nhap_CLC"]) + Convert.ToInt32(r[4]["Nhap_CGTI"]) + Convert.ToInt32(r[4]["Nhap_CGT_D"]) + Convert.ToInt32(r[4]["Nhap_CPLI"]) + Convert.ToInt32(r[4]["Nhap_CPL_D"]) + Convert.ToInt32(r[4]["Nhap_CTT"]) - Convert.ToInt32(r[4]["Xuat_CPLI"]) - Convert.ToInt32(r[4]["Xuat_CGTI"]);
                    //GiaTri[0, 1] - Tong Nhap
                    GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
                    GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];
                    GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];
                    //-------------------------------------------

                    //GiaTri[1, 2] - Giong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[1, 7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);
                    GiaTri[1, 8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                    //GiaTri[2, 2] - Tang Trong Xuat
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[2, 7] += Convert.ToInt32(r[5]["Xuat_Ban"]) + Convert.ToInt32(r[5]["Xuat_CLC"]) + Convert.ToInt32(r[5]["Xuat_CGT_D"]) + Convert.ToInt32(r[5]["Xuat_CPL_D"]);
                    GiaTri[2, 8] += Convert.ToInt32(r[4]["Xuat_Ban"]) + Convert.ToInt32(r[4]["Xuat_CLC"]) + Convert.ToInt32(r[4]["Xuat_CGT_D"]) + Convert.ToInt32(r[4]["Xuat_CPL_D"]);
                    //GiaTri[0, 2] - Tong Xuat
                    GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
                    GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];
                    GiaTri[0, 8] = GiaTri[1, 8] + GiaTri[2, 8];
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[1, 10] += Convert.ToInt32(r[2]["Chet"]);
                    GiaTri[1, 11] += Convert.ToInt32(r[1]["Chet"]);
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[2, 10] += Convert.ToInt32(r[5]["Chet"]);
                    GiaTri[2, 11] += Convert.ToInt32(r[4]["Chet"]);
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
                    GiaTri[0, 10] = GiaTri[1, 10] + GiaTri[2, 10];
                    GiaTri[0, 11] = GiaTri[1, 11] + GiaTri[2, 11];//Ky
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[1, 13] += Convert.ToInt32(r[2]["LoaiThai"]);
                    GiaTri[1, 14] += Convert.ToInt32(r[1]["LoaiThai"]);
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[2, 13] += Convert.ToInt32(r[5]["LoaiThai"]);
                    GiaTri[2, 14] += Convert.ToInt32(r[4]["LoaiThai"]);
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
                    GiaTri[0, 13] = GiaTri[1, 13] + GiaTri[2, 13];
                    GiaTri[0, 14] = GiaTri[1, 14] + GiaTri[2, 14];
                    //-------------------------------------------

                    //GiaTri[1, 4] - Giong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[1, 16] += Convert.ToInt32(r[2]["GietMo"]);
                    GiaTri[1, 17] += Convert.ToInt32(r[1]["GietMo"]);
                    //GiaTri[2, 4] - Tang Trong Giet Mo
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[2, 16] += Convert.ToInt32(r[5]["GietMo"]);
                    GiaTri[2, 17] += Convert.ToInt32(r[4]["GietMo"]);
                    //GiaTri[0, 4] - Tong Giet Mo
                    GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
                    GiaTri[0, 16] = GiaTri[1, 16] + GiaTri[2, 16];
                    GiaTri[0, 17] = GiaTri[1, 17] + GiaTri[2, 17];
                    //-------------------------------------------

                    //GiaTri[1, 5] - Giong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[1, 19] += Convert.ToInt32(r[2]["TonCuoi"]);
                    GiaTri[1, 20] += Convert.ToInt32(r[1]["TonCuoi"]);
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[2, 19] += Convert.ToInt32(r[5]["TonCuoi"]);
                    GiaTri[2, 20] += Convert.ToInt32(r[4]["TonCuoi"]);
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
                    GiaTri[0, 19] = GiaTri[1, 19] + GiaTri[2, 19];
                    GiaTri[0, 20] = GiaTri[1, 20] + GiaTri[2, 20];
                    //-------------------------------------------

                    for (t = 0; t < 3; t++)
                    {
                        if (t == 0)
                        {
                            sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1.Rows[1]["GietMo"]) + Convert.ToInt32(dtPLGM1.Rows[3]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1.Rows[0]["GietMo"]) + Convert.ToInt32(dtPLGM1.Rows[2]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = 0; h < dtSub1.Rows.Count; h++)
                            {
                                DataRow rta = dtSub1.Rows[h];
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                j++;
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            sb.Append("<td></td>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td>");
                            sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM1.Rows[t * 2 - 1]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM1.Rows[t * 2 - 2]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = 0; h < dtSub1.Rows.Count; h++)
                            {
                                DataRow rta = dtSub1.Rows[h];
                                if (t == 1)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                }
                                j++;
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            //
                            sb.Append("<td></td>");
                        }
                        sb.Append("</tr>");
                    }
                }


                ////Cá loại 5, -1, 6
                for (i = 4; i < 7; i++)
                {
                    DataRow[] r = new DataRow[6];
                    for (int jj = 0; jj < 6; jj++)
                    {
                        r[jj] = dt.Rows[i * 6 + jj];
                    }
                    sb.Append("<tr>");
                    if (i == 4) sb.Append("<td style='vertical-align:middle;'>5.1</td>");
                    else if (i == 5) sb.Append("<td style='vertical-align:middle;'>5.2</td>");
                    else sb.Append("<td style='vertical-align:middle;'>" + i.ToString() + "</td>");
                    string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                    int[,] GiaTri = new int[3, 21];
                    int t;

                    //-------------------------------------------

                    //GiaTri[1, 0] - Giong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[1, 1] += Convert.ToInt32(r[2]["TonDau"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) - Convert.ToInt32(r[2]["Xuat_CGTO"]) - Convert.ToInt32(r[2]["Xuat_CPLO"]);
                    GiaTri[1, 2] += Convert.ToInt32(r[1]["TonDau"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) - Convert.ToInt32(r[1]["Xuat_CGTO"]) - Convert.ToInt32(r[1]["Xuat_CPLO"]);
                    //GiaTri[2, 0] - Tang Trong Ton Dau
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[2, 1] += Convert.ToInt32(r[5]["TonDau"]) + Convert.ToInt32(r[5]["Nhap_CGTO"]) + Convert.ToInt32(r[5]["Nhap_CPLO"]) - Convert.ToInt32(r[5]["Xuat_CGTO"]) - Convert.ToInt32(r[5]["Xuat_CPLO"]);
                    GiaTri[2, 2] += Convert.ToInt32(r[4]["TonDau"]) + Convert.ToInt32(r[4]["Nhap_CGTO"]) + Convert.ToInt32(r[4]["Nhap_CPLO"]) - Convert.ToInt32(r[4]["Xuat_CGTO"]) - Convert.ToInt32(r[4]["Xuat_CPLO"]);
                    //GiaTri[0, 0] - Tong Ton Dau
                    GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
                    GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];
                    GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];
                    //-------------------------------------------

                    //GiaTri[1, 1] - Giong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[1, 4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]) - Convert.ToInt32(r[2]["Xuat_CPLI"]) - Convert.ToInt32(r[2]["Xuat_CGTI"]);
                    GiaTri[1, 5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]) - Convert.ToInt32(r[1]["Xuat_CPLI"]) - Convert.ToInt32(r[1]["Xuat_CGTI"]);
                    //GiaTri[2, 1] - Tang Trong Nhap
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[2, 4] += Convert.ToInt32(r[5]["Nhap_NhapChuong"]) + Convert.ToInt32(r[5]["Nhap_CLC"]) + Convert.ToInt32(r[5]["Nhap_CGTI"]) + Convert.ToInt32(r[5]["Nhap_CGT_D"]) + Convert.ToInt32(r[5]["Nhap_CPLI"]) + Convert.ToInt32(r[5]["Nhap_CPL_D"]) + Convert.ToInt32(r[5]["Nhap_CTT"]) - Convert.ToInt32(r[5]["Xuat_CPLI"]) - Convert.ToInt32(r[5]["Xuat_CGTI"]);
                    GiaTri[2, 5] += Convert.ToInt32(r[4]["Nhap_NhapChuong"]) + Convert.ToInt32(r[4]["Nhap_CLC"]) + Convert.ToInt32(r[4]["Nhap_CGTI"]) + Convert.ToInt32(r[4]["Nhap_CGT_D"]) + Convert.ToInt32(r[4]["Nhap_CPLI"]) + Convert.ToInt32(r[4]["Nhap_CPL_D"]) + Convert.ToInt32(r[4]["Nhap_CTT"]) - Convert.ToInt32(r[4]["Xuat_CPLI"]) - Convert.ToInt32(r[4]["Xuat_CGTI"]);
                    //GiaTri[0, 1] - Tong Nhap
                    GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
                    GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];
                    GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];
                    //-------------------------------------------

                    //GiaTri[1, 2] - Giong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[1, 7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);
                    GiaTri[1, 8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                    //GiaTri[2, 2] - Tang Trong Xuat
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[2, 7] += Convert.ToInt32(r[5]["Xuat_Ban"]) + Convert.ToInt32(r[5]["Xuat_CLC"]) + Convert.ToInt32(r[5]["Xuat_CGT_D"]) + Convert.ToInt32(r[5]["Xuat_CPL_D"]);
                    GiaTri[2, 8] += Convert.ToInt32(r[4]["Xuat_Ban"]) + Convert.ToInt32(r[4]["Xuat_CLC"]) + Convert.ToInt32(r[4]["Xuat_CGT_D"]) + Convert.ToInt32(r[4]["Xuat_CPL_D"]);
                    //GiaTri[0, 2] - Tong Xuat
                    GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
                    GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];
                    GiaTri[0, 8] = GiaTri[1, 8] + GiaTri[2, 8];
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[1, 10] += Convert.ToInt32(r[2]["Chet"]);
                    GiaTri[1, 11] += Convert.ToInt32(r[1]["Chet"]);
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[2, 10] += Convert.ToInt32(r[5]["Chet"]);
                    GiaTri[2, 11] += Convert.ToInt32(r[4]["Chet"]);
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
                    GiaTri[0, 10] = GiaTri[1, 10] + GiaTri[2, 10];
                    GiaTri[0, 11] = GiaTri[1, 11] + GiaTri[2, 11];//Ky
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[1, 13] += Convert.ToInt32(r[2]["LoaiThai"]);
                    GiaTri[1, 14] += Convert.ToInt32(r[1]["LoaiThai"]);
                    //GiaTri[2, 4] - Tang Trong Giet Mo
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[2, 13] += Convert.ToInt32(r[5]["LoaiThai"]);
                    GiaTri[2, 14] += Convert.ToInt32(r[4]["LoaiThai"]);
                    //GiaTri[0, 4] - Tong Giet Mo
                    GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
                    GiaTri[0, 13] = GiaTri[1, 13] + GiaTri[2, 13];
                    GiaTri[0, 14] = GiaTri[1, 14] + GiaTri[2, 14];
                    //-------------------------------------------

                    //GiaTri[1, 4] - Giong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[1, 16] += Convert.ToInt32(r[2]["GietMo"]);
                    GiaTri[1, 17] += Convert.ToInt32(r[1]["GietMo"]);
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[2, 16] += Convert.ToInt32(r[5]["GietMo"]);
                    GiaTri[2, 17] += Convert.ToInt32(r[4]["GietMo"]);
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
                    GiaTri[0, 16] = GiaTri[1, 16] + GiaTri[2, 16];
                    GiaTri[0, 17] = GiaTri[1, 17] + GiaTri[2, 17];
                    //-------------------------------------------

                    //GiaTri[1, 5] - Giong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[1, 19] += Convert.ToInt32(r[2]["TonCuoi"]);
                    GiaTri[1, 20] += Convert.ToInt32(r[1]["TonCuoi"]);
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[2, 19] += Convert.ToInt32(r[5]["TonCuoi"]);
                    GiaTri[2, 20] += Convert.ToInt32(r[4]["TonCuoi"]);
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
                    GiaTri[0, 19] = GiaTri[1, 19] + GiaTri[2, 19];
                    GiaTri[0, 20] = GiaTri[1, 20] + GiaTri[2, 20];
                    //-------------------------------------------

                    int iThucAnGoc = iThucAn;
                    for (t = 0; t < 3; t++)
                    {
                        if (t == 0)
                        {
                            sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 3]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 2]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                    j++;
                                }
                                else
                                {
                                    iThucAn = h;
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            sb.Append("<td></td>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td>");
                            sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 2]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //
                            //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAnGoc; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    if (t == 1)
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                    }
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            //
                            sb.Append("<td></td>");
                        }
                        sb.Append("</tr>");
                    }
                }

                //Cá từ loại 7 -> loại 11
                for (i = 7; i < 12; i++)
                {
                    DataRow[] r = new DataRow[3];
                    for (int jj = 0; jj < 3; jj++)
                    {
                        r[jj] = dt.Rows[i * 6 + jj];
                    }
                    sb.Append("<tr>");
                    sb.Append("<td>" + i.ToString() + "</td>");
                    sb.Append("<td align='left'>" + r[0]["TenLoaiCa"].ToString() + "</td>");
                    int[] GiaTri = new int[21];
                    int t;

                    //-------------------------------------------

                    //GiaTri[0] - Tong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    //GiaTri[2] - Cai Ton Dau
                    GiaTri[2] += Convert.ToInt32(r[1]["TonDau"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) - Convert.ToInt32(r[1]["Xuat_CGTO"]) - Convert.ToInt32(r[1]["Xuat_CPLO"]);
                    //GiaTri[1] - Duc Ton Dau
                    GiaTri[1] += Convert.ToInt32(r[2]["TonDau"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) - Convert.ToInt32(r[2]["Xuat_CGTO"]) - Convert.ToInt32(r[2]["Xuat_CPLO"]);

                    //-------------------------------------------

                    //GiaTri[3] - Tong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]);
                    }
                    //GiaTri[5] - Cai Nhap
                    GiaTri[5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]) - Convert.ToInt32(r[1]["Xuat_CGTI"]) - Convert.ToInt32(r[1]["Xuat_CPLI"]);
                    //GiaTri[4] - Duc Nhap
                    GiaTri[4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]) - Convert.ToInt32(r[2]["Xuat_CGTI"]) - Convert.ToInt32(r[2]["Xuat_CPLI"]);

                    //-------------------------------------------

                    //GiaTri[6] - Tong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        //GiaTri[6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CGT"]) - Convert.ToInt32(r[t]["Nhap_CPL"]);
                        GiaTri[6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    //GiaTri[8] - Cai Xuat
                    //GiaTri[8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGT"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPL"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]) - Convert.ToInt32(r[1]["Nhap_CGT"]) - Convert.ToInt32(r[1]["Nhap_CPL"]);
                    GiaTri[8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                    //GiaTri[7] - Duc Xuat
                    //GiaTri[7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGT"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPL"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]) - Convert.ToInt32(r[2]["Nhap_CGT"]) - Convert.ToInt32(r[2]["Nhap_CPL"]);
                    GiaTri[7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);

                    //-------------------------------------------

                    //GiaTri[9] - Tong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    //GiaTri[11] - Cai Chet
                    GiaTri[11] += Convert.ToInt32(r[1]["Chet"]);
                    //GiaTri[10] - Duc Chet
                    GiaTri[10] += Convert.ToInt32(r[2]["Chet"]);

                    //-------------------------------------------

                    //GiaTri[9] - Tong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    //GiaTri[14] - Cai Giet Mo
                    GiaTri[14] += Convert.ToInt32(r[1]["LoaiThai"]);
                    //GiaTri[13] - Duc Giet Mo
                    GiaTri[13] += Convert.ToInt32(r[2]["LoaiThai"]);

                    //-------------------------------------------

                    //GiaTri[12] - Tong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    //GiaTri[17] - Cai Ton Cuoi
                    GiaTri[17] += Convert.ToInt32(r[1]["GietMo"]);
                    //GiaTri[16] - Duc Ton Cuoi
                    GiaTri[16] += Convert.ToInt32(r[2]["GietMo"]);

                    //-------------------------------------------

                    //GiaTri[15] - Tong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    //GiaTri[17] - Cai Ton Cuoi
                    GiaTri[20] += Convert.ToInt32(r[1]["TonCuoi"]);
                    //GiaTri[16] - Duc Ton Cuoi
                    GiaTri[19] += Convert.ToInt32(r[2]["TonCuoi"]);

                    //-------------------------------------------
                    for (int y = 0; y < 5; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 1], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 2], 0) + "</td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[15], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + 1]["GietMo"], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4]["GietMo"], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[16], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[17], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[18], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[19], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[20], 0) + "</td>");
                    //ThucAn Start
                    j = 0;
                    for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                    {
                        DataRow rta = dtSub.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                        {
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                            j++;
                        }
                        else
                        {
                            iThucAn = h;
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                }
                //Tính tổng
                int[] Tong = new int[21];
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    Tong[0] += Convert.ToInt32(r["TonDau"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]); ;
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[2] += Convert.ToInt32(r["TonDau"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]); ;
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[1] += Convert.ToInt32(r["TonDau"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]); ;
                    }

                    Tong[3] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]) - Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[5] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]) - Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[4] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]) - Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                    }

                    //Tong[6] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL"]) + Convert.ToInt32(r["Xuat_CPL_D"]) - Convert.ToInt32(r["Nhap_CGT"]) - Convert.ToInt32(r["Nhap_CPL"]);
                    Tong[6] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        //Tong[8] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL"]) + Convert.ToInt32(r["Xuat_CPL_D"]) - Convert.ToInt32(r["Nhap_CGT"]) - Convert.ToInt32(r["Nhap_CPL"]);
                        Tong[8] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        //Tong[7] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL"]) + Convert.ToInt32(r["Xuat_CPL_D"]) - Convert.ToInt32(r["Nhap_CGT"]) - Convert.ToInt32(r["Nhap_CPL"]);
                        Tong[7] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                    }

                    Tong[9] += Convert.ToInt32(r["Chet"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[11] += Convert.ToInt32(r["Chet"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[10] += Convert.ToInt32(r["Chet"]);
                    }

                    Tong[12] += Convert.ToInt32(r["LoaiThai"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[14] += Convert.ToInt32(r["LoaiThai"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[13] += Convert.ToInt32(r["LoaiThai"]);
                    }

                    Tong[15] += Convert.ToInt32(r["GietMo"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[17] += Convert.ToInt32(r["GietMo"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[16] += Convert.ToInt32(r["GietMo"]);
                    }

                    Tong[18] += Convert.ToInt32(r["TonCuoi"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[20] += Convert.ToInt32(r["TonCuoi"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[19] += Convert.ToInt32(r["TonCuoi"]);
                    }
                }
                //Tính tổng phân loại giêt mô
                int[] TongPLGM = new int[2];
                for (i = 0; i < dtPLGM.Rows.Count; i++)
                {
                    DataRow r = dtPLGM.Rows[i];
                    if (i % 2 == 1)
                    {
                        TongPLGM[1] += Convert.ToInt32(r["GietMo"]);
                    }
                    else
                    {
                        TongPLGM[0] += Convert.ToInt32(r["GietMo"]);
                    }
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td></td><td style='text-align:center;font-weight:bold;'>Tổng cộng</td>");
                for (i = 0; i < 15; i++)
                {
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[i], 0) + "</td>");
                }
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[15], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TongPLGM[1], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TongPLGM[0], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[16], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[17], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[18], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[19], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[20], 0) + "</td>");

                for (i = 0; i < SoCotThucAn; i++)
                {
                    sb.Append(@"<td></td>");
                }
                sb.Append("<td></td></tr>");
                sb.Append("</tbody></table>");
                lt.Text = sb.ToString();
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnView_Others_Click()
        {
            try
            {
                int iThucAn = 0;
                string tieude = "";
                string strSQL = "QLCS_BCTK_BienDongDan_JoinMauBC";
                string strSQL_PLGM = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_oldway";
                string strSubSQL = "QLCS_BCTK_CaAn";
                string strMaxSQL = "QLCS_BCTK_MaxSoLoaiThucAn";
                string strSQLTD = "QLCS_BCTK_BienDongDan_CaTanDung";
                string strSQLTDAn = "QLCS_BCTK_CaKhongMeAn";
                SqlParameter[] param = new SqlParameter[2];
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
                tieude += "<b>BẢNG THEO DÕI BIẾN ĐỘNG TỔNG ĐÀN CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                SqlParameter[] paramPLGM = new SqlParameter[2];
                paramPLGM[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                paramPLGM[1] = new SqlParameter("@dTo", txtToDate.Text);
                DataSet dsPLGM = Config.SelectSPs(strSQL_PLGM, paramPLGM);
                DataTable dtPLGM = dsPLGM.Tables[0];
                DataTable dtPLGM1 = dsPLGM.Tables[1];

                SqlParameter[] paramSub = new SqlParameter[2];
                paramSub[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                paramSub[1] = new SqlParameter("@dTo", txtToDate.Text);
                DataSet dsSub = Config.SelectSPs(strSubSQL, paramSub);
                DataTable dtSub = dsSub.Tables[0];
                DataTable dtSub1 = dsSub.Tables[1];
                DataTable dtSub2 = dsSub.Tables[2];
                SqlParameter[] param2 = new SqlParameter[2];
                param2[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param2[1] = new SqlParameter("@dTo", txtToDate.Text);
                DataTable dtMax = DotNetNuke.NewsProvider.DataProvider.SelectSP(strMaxSQL, param2);
                DataTable dtTD = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTD, param2);
                SqlParameter[] param1 = new SqlParameter[4];
                param1[0] = new SqlParameter("@LoaiCa", 1);
                param1[1] = new SqlParameter("@dFrom", txtFromDate.Text);
                param1[2] = new SqlParameter("@dTo", txtToDate.Text);
                param1[3] = new SqlParameter("@ListThucAn", "");
                param1[3].Direction = ParameterDirection.Output;
                param1[3].Size = 4000;
                DataTable dtTDAn = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTDAn, param1);
                string ListThucAnTD = param1[3].Value.ToString();
                int SoCotThucAn = 2;
                if (dtMax != null && dtMax.Rows.Count == 1 && dtMax.Rows[0]["MaxSoLoaiThucAn"] != DBNull.Value) SoCotThucAn = Convert.ToInt32(dtMax.Rows[0]["MaxSoLoaiThucAn"]) * 2;
                if (Convert.ToInt32(dtSub2.Rows[0]["MaxSoLoaiThucAnHauBi"]) * 2 > SoCotThucAn) SoCotThucAn = Convert.ToInt32(dtSub2.Rows[0]["MaxSoLoaiThucAnHauBi"]) * 2;
                if (SoCotThucAn < 2) SoCotThucAn = 2;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=3>STT</th>
                          <th rowspan=3>Nội dung</th>
                          <th colspan=3>Tồn đầu</th>
                          <th colspan=3>Nhập</th>
                          <th colspan=3>Xuất</th>
                          <th colspan=3>Chết</th>
                          <th colspan=3>Loại thải</th>
                          <th colspan=5>Giết mổ</th>
                          <th colspan=3>Tồn cuối</th>
                          <th colspan=" + SoCotThucAn.ToString() + @">Tiêu tốn thức ăn</th>
                          <th rowspan=3>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=4>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>");
                for (int ii = 0; ii < SoCotThucAn / 2; ii++)
                {
                    sb.Append(@"<th rowspan=2>Loại</th>
                          <th rowspan=2>SL (kg)</th>");
                }

                sb.Append(@"</tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>TP</th>
                          <th>LT</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>");
                sb.Append("</tr></thead><tbody>");
                int i = 0;
                //Cá loại 1 (cá con)
                DataRow[] r1 = new DataRow[6];
                for (int jj = 0; jj < 6; jj++)
                {
                    r1[jj] = dt.Rows[i * 6 + jj];
                }
                sb.Append("<tr>");
                sb.Append("<td style='vertical-align:middle;'>1.1</td>");

                int[,] GiaTri1 = new int[3, 7];
                int t1;

                //-------------------------------------------

                //GiaTri1[1, 0] - Giong Ton Dau
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 0] += Convert.ToInt32(r1[t1]["TonDau"]);// +Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
                }
                //GiaTri1[2, 0] - Tang Trong Ton Dau
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 0] += Convert.ToInt32(r1[t1]["TonDau"]);// +Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
                }
                //GiaTri1[0, 0] - Tong Ton Dau
                GiaTri1[0, 0] = GiaTri1[1, 0] + GiaTri1[2, 0];

                //-------------------------------------------

                //GiaTri1[1, 1] - Giong Nhap
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]);// - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
                }
                //GiaTri1[2, 1] - Tang Trong Nhap
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]);// - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
                }
                //GiaTri1[0, 1] - Tong Nhap
                GiaTri1[0, 1] = GiaTri1[1, 1] + GiaTri1[2, 1];

                //-------------------------------------------

                //GiaTri1[1, 2] - Giong Xuat
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGTO"]) + Convert.ToInt32(r1[t1]["Xuat_CGTI"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPLO"]) + Convert.ToInt32(r1[t1]["Xuat_CPLI"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
                }
                //GiaTri1[2, 2] - Tang Trong Xuat
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGTO"]) + Convert.ToInt32(r1[t1]["Xuat_CGTI"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPLO"]) + Convert.ToInt32(r1[t1]["Xuat_CPLI"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
                }
                //GiaTri1[0, 2] - Tong Xuat
                GiaTri1[0, 2] = GiaTri1[1, 2] + GiaTri1[2, 2];

                //-------------------------------------------

                //GiaTri1[1, 3] - Giong Chet
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 3] += Convert.ToInt32(r1[t1]["Chet"]);
                }
                //GiaTri1[2, 3] - Tang Trong Chet
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 3] += Convert.ToInt32(r1[t1]["Chet"]);
                }
                //GiaTri1[0, 3] - Tong Chet
                GiaTri1[0, 3] = GiaTri1[1, 3] + GiaTri1[2, 3];

                //-------------------------------------------

                //GiaTri1[1, 3] - Giong LoaiThai
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 4] += Convert.ToInt32(r1[t1]["LoaiThai"]);
                }
                //GiaTri1[2, 3] - Tang Trong Chet
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 4] += Convert.ToInt32(r1[t1]["LoaiThai"]);
                }
                //GiaTri1[0, 3] - Tong Chet
                GiaTri1[0, 4] = GiaTri1[1, 4] + GiaTri1[2, 4];

                //-------------------------------------------

                //GiaTri1[1, 4] - Giong Giet Mo
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 5] += Convert.ToInt32(r1[t1]["GietMo"]);
                }
                //GiaTri1[2, 4] - Tang Trong Giet Mo
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 5] += Convert.ToInt32(r1[t1]["GietMo"]);
                }
                //GiaTri1[0, 4] - Tong Giet Mo
                GiaTri1[0, 5] = GiaTri1[1, 5] + GiaTri1[2, 5];

                //-------------------------------------------

                //GiaTri1[1, 5] - Giong Ton Cuoi
                for (t1 = 0; t1 < 3; t1++)
                {
                    GiaTri1[1, 6] += Convert.ToInt32(r1[t1]["TonCuoi"]);
                }
                //GiaTri1[2, 5] - Tang Trong Ton Cuoi
                for (t1 = 3; t1 < 6; t1++)
                {
                    GiaTri1[2, 6] += Convert.ToInt32(r1[t1]["TonCuoi"]);
                }
                //GiaTri1[0, 5] - Tong Ton Cuoi
                GiaTri1[0, 6] = GiaTri1[1, 6] + GiaTri1[2, 6];

                //-------------------------------------------

                DataRow rTD = dtTD.Rows[0];
                sb.Append("<td align='left'>Cá GĐ úm</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 0] - Convert.ToInt32(rTD["TonDau"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 1] - (Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"])), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 2] - (Convert.ToInt32(rTD["Xuat_Ban"]) + Convert.ToInt32(rTD["Xuat_CLC"])), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 3] - Convert.ToInt32(rTD["Chet"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 4] - Convert.ToInt32(rTD["LoaiThai"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 5] - Convert.ToInt32(rTD["GietMo"]), 0) + "</td><td></td><td>" + Config.ToXVal2(GiaTri1[0, 5] - Convert.ToInt32(rTD["GietMo"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 6] - Convert.ToInt32(rTD["TonCuoi"]), 0) + "</td><td></td><td></td>");

                //ThucAn Start
                int j = 0;
                for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                {
                    DataRow rta = dtSub.Rows[h];
                    if (rta["LoaiCa"].ToString() == "1")
                    {
                        if (ListThucAnTD.Contains("@" + rta["ThucAn"].ToString() + "@"))
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(Convert.ToDecimal(rta["KhoiLuong"]) - GetKhoiLuongFromTable(dtTDAn, Convert.ToInt32(rta["ThucAn"])), scale) + "</td>");
                        else
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                        j++;
                    }
                    else
                    {
                        iThucAn = h;
                        break;
                    }
                }
                for (int t = j; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
                //ThucAn End
                sb.Append("<td></td>");
                sb.Append("</tr>");




                //Cá tận dụng
                sb.Append("<tr>");
                sb.Append("<td style='vertical-align:middle;'>1.2</td>");
                sb.Append("<td align='left'>Cá GĐ úm tận dụng</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonDau"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Xuat_Ban"]) + Convert.ToInt32(rTD["Xuat_CLC"]), 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["Chet"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["LoaiThai"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["GietMo"], 0) + "</td><td></td><td>" + Config.ToXVal2(rTD["GietMo"], 0) + "</td><td></td><td></td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonCuoi"], 0) + "</td><td></td><td></td>");
                //ThucAn Start
                if (ListThucAnTD == "")
                {
                    for (int t = 0; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
                }
                else
                {
                    int o = 0;
                    for (o = 0; o < dtTDAn.Rows.Count; o++)
                    {
                        DataRow r = dtTDAn.Rows[o];
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["KhoiLuong"], scale) + "</td>");
                    }
                    for (int t = o; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
                }
                //ThucAn End
                sb.Append("<td></td>");
                sb.Append("</tr>");


                //Cá từ loại 2 -> loại 4
                for (i = 1; i < 4; i++)
                {
                    DataRow[] r = new DataRow[6];
                    for (int jj = 0; jj < 6; jj++)
                    {
                        r[jj] = dt.Rows[i * 6 + jj];
                    }
                    sb.Append("<tr>");
                    int idx = i + 1;
                    sb.Append("<td style='vertical-align:middle;'>" + idx.ToString() + "</td>");
                    string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                    int[,] GiaTri = new int[3, 7];
                    int t;

                    //-------------------------------------------

                    //GiaTri[1, 0] - Giong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    //GiaTri[2, 0] - Tang Trong Ton Dau
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    //GiaTri[0, 0] - Tong Ton Dau
                    GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];

                    //-------------------------------------------

                    //GiaTri[1, 1] - Giong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    //GiaTri[2, 1] - Tang Trong Nhap
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    //GiaTri[0, 1] - Tong Nhap
                    GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];

                    //-------------------------------------------

                    //GiaTri[1, 2] - Giong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    //GiaTri[2, 2] - Tang Trong Xuat
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    //GiaTri[0, 2] - Tong Xuat
                    GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];

                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 3] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 3] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];

                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 4] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 4] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];

                    //-------------------------------------------

                    //GiaTri[1, 4] - Giong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 5] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    //GiaTri[2, 4] - Tang Trong Giet Mo
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 5] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    //GiaTri[0, 4] - Tong Giet Mo
                    GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];

                    //-------------------------------------------

                    //GiaTri[1, 5] - Giong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 6] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 6] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];

                    //-------------------------------------------

                    int iThucAnGoc = iThucAn;
                    for (t = 0; t < 3; t++)
                    {
                        if (t == 0)
                        {
                            sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 5; y++)
                            {
                                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                            }
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 5], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 3]["GietMo"]), 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 2]["GietMo"]), 0) + "</td><td></td><td></td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6], 0) + "</td><td></td><td></td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                    j++;
                                }
                                else
                                {
                                    iThucAn = h;
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            sb.Append("<td></td>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td>");
                            sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 5; y++)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                            }
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 5], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 2]["GietMo"], 0) + "</td><td></td><td></td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6], 0) + "</td><td></td><td></td>");
                            //
                            //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAnGoc; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    if (t == 1)
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                    }
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            //
                            sb.Append("<td></td>");
                        }
                        sb.Append("</tr>");
                    }
                }

                //Tong hop Cá hậu bị từ loại 5, -1
                for (int n = 0; n < 1; n++)
                {
                    DataRow[] r = new DataRow[6];
                    for (int jj = 0; jj < 6; jj++)
                    {
                        r[jj] = dt1.Rows[jj];
                    }
                    sb.Append("<tr>");
                    sb.Append("<td style='vertical-align:middle;'>5</td>");
                    string[] tenLoaiCa = new string[] { "Hậu bị", "Con giống", "Tăng trọng" };
                    int[,] GiaTri = new int[3, 21];
                    int t;

                    //-------------------------------------------

                    //GiaTri[1, 0] - Giong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[1, 1] += Convert.ToInt32(r[2]["TonDau"]);
                    GiaTri[1, 2] += Convert.ToInt32(r[1]["TonDau"]);
                    //GiaTri[2, 0] - Tang Trong Ton Dau
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[2, 1] += Convert.ToInt32(r[5]["TonDau"]);
                    GiaTri[2, 2] += Convert.ToInt32(r[4]["TonDau"]);
                    //GiaTri[0, 0] - Tong Ton Dau
                    GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
                    GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];
                    GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];
                    //-------------------------------------------

                    //GiaTri[1, 1] - Giong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[1, 4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]);
                    GiaTri[1, 5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]);
                    //GiaTri[2, 1] - Tang Trong Nhap
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[2, 4] += Convert.ToInt32(r[5]["Nhap_NhapChuong"]) + Convert.ToInt32(r[5]["Nhap_CLC"]) + Convert.ToInt32(r[5]["Nhap_CGTO"]) + Convert.ToInt32(r[5]["Nhap_CGTI"]) + Convert.ToInt32(r[5]["Nhap_CGT_D"]) + Convert.ToInt32(r[5]["Nhap_CPLO"]) + Convert.ToInt32(r[5]["Nhap_CPLI"]) + Convert.ToInt32(r[5]["Nhap_CPL_D"]) + Convert.ToInt32(r[5]["Nhap_CTT"]);
                    GiaTri[2, 5] += Convert.ToInt32(r[4]["Nhap_NhapChuong"]) + Convert.ToInt32(r[4]["Nhap_CLC"]) + Convert.ToInt32(r[4]["Nhap_CGTO"]) + Convert.ToInt32(r[4]["Nhap_CGTI"]) + Convert.ToInt32(r[4]["Nhap_CGT_D"]) + Convert.ToInt32(r[4]["Nhap_CPLO"]) + Convert.ToInt32(r[4]["Nhap_CPLI"]) + Convert.ToInt32(r[4]["Nhap_CPL_D"]) + Convert.ToInt32(r[4]["Nhap_CTT"]);
                    //GiaTri[0, 1] - Tong Nhap
                    GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
                    GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];
                    GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];
                    //-------------------------------------------

                    //GiaTri[1, 2] - Giong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[1, 7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGTO"]) + Convert.ToInt32(r[2]["Xuat_CGTI"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPLO"]) + Convert.ToInt32(r[2]["Xuat_CPLI"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);
                    GiaTri[1, 8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGTO"]) + Convert.ToInt32(r[1]["Xuat_CGTI"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPLO"]) + Convert.ToInt32(r[1]["Xuat_CPLI"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                    //GiaTri[2, 2] - Tang Trong Xuat
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[2, 7] += Convert.ToInt32(r[5]["Xuat_Ban"]) + Convert.ToInt32(r[5]["Xuat_CLC"]) + Convert.ToInt32(r[5]["Xuat_CGTO"]) + Convert.ToInt32(r[5]["Xuat_CGTI"]) + Convert.ToInt32(r[5]["Xuat_CGT_D"]) + Convert.ToInt32(r[5]["Xuat_CPLO"]) + Convert.ToInt32(r[5]["Xuat_CPLI"]) + Convert.ToInt32(r[5]["Xuat_CPL_D"]);
                    GiaTri[2, 8] += Convert.ToInt32(r[4]["Xuat_Ban"]) + Convert.ToInt32(r[4]["Xuat_CLC"]) + Convert.ToInt32(r[4]["Xuat_CGTO"]) + Convert.ToInt32(r[4]["Xuat_CGTI"]) + Convert.ToInt32(r[4]["Xuat_CGT_D"]) + Convert.ToInt32(r[4]["Xuat_CPLO"]) + Convert.ToInt32(r[4]["Xuat_CPLI"]) + Convert.ToInt32(r[4]["Xuat_CPL_D"]);
                    //GiaTri[0, 2] - Tong Xuat
                    GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
                    GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];
                    GiaTri[0, 8] = GiaTri[1, 8] + GiaTri[2, 8];
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[1, 10] += Convert.ToInt32(r[2]["Chet"]);
                    GiaTri[1, 11] += Convert.ToInt32(r[1]["Chet"]);
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[2, 10] += Convert.ToInt32(r[5]["Chet"]);
                    GiaTri[2, 11] += Convert.ToInt32(r[4]["Chet"]);
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
                    GiaTri[0, 10] = GiaTri[1, 10] + GiaTri[2, 10];
                    GiaTri[0, 11] = GiaTri[1, 11] + GiaTri[2, 11];
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[1, 13] += Convert.ToInt32(r[2]["LoaiThai"]);
                    GiaTri[1, 14] += Convert.ToInt32(r[1]["LoaiThai"]);
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[2, 13] += Convert.ToInt32(r[5]["LoaiThai"]);
                    GiaTri[2, 14] += Convert.ToInt32(r[4]["LoaiThai"]);
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
                    GiaTri[0, 13] = GiaTri[1, 13] + GiaTri[2, 13];
                    GiaTri[0, 14] = GiaTri[1, 14] + GiaTri[2, 14];
                    //-------------------------------------------

                    //GiaTri[1, 4] - Giong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[1, 16] += Convert.ToInt32(r[2]["GietMo"]);
                    GiaTri[1, 17] += Convert.ToInt32(r[1]["GietMo"]);
                    //GiaTri[2, 4] - Tang Trong Giet Mo
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[2, 16] += Convert.ToInt32(r[5]["GietMo"]);
                    GiaTri[2, 17] += Convert.ToInt32(r[4]["GietMo"]);
                    //GiaTri[0, 4] - Tong Giet Mo
                    GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
                    GiaTri[0, 16] = GiaTri[1, 16] + GiaTri[2, 16];
                    GiaTri[0, 17] = GiaTri[1, 17] + GiaTri[2, 17];
                    //-------------------------------------------

                    //GiaTri[1, 5] - Giong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[1, 19] += Convert.ToInt32(r[2]["TonCuoi"]);
                    GiaTri[1, 20] += Convert.ToInt32(r[1]["TonCuoi"]);
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[2, 19] += Convert.ToInt32(r[5]["TonCuoi"]);
                    GiaTri[2, 20] += Convert.ToInt32(r[4]["TonCuoi"]);
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
                    GiaTri[0, 19] = GiaTri[1, 19] + GiaTri[2, 19];
                    GiaTri[0, 20] = GiaTri[1, 20] + GiaTri[2, 20];
                    //-------------------------------------------

                    for (t = 0; t < 3; t++)
                    {
                        if (t == 0)
                        {
                            sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1.Rows[1]["GietMo"]) + Convert.ToInt32(dtPLGM1.Rows[3]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1.Rows[0]["GietMo"]) + Convert.ToInt32(dtPLGM1.Rows[2]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = 0; h < dtSub1.Rows.Count; h++)
                            {
                                DataRow rta = dtSub1.Rows[h];
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                j++;
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            sb.Append("<td></td>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td>");
                            sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM1.Rows[t * 2 - 1]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM1.Rows[t * 2 - 2]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //
                            //ThucAn Start
                            j = 0;
                            for (int h = 0; h < dtSub1.Rows.Count; h++)
                            {
                                DataRow rta = dtSub1.Rows[h];
                                if (t == 1)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                }
                                j++;
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            //
                            sb.Append("<td></td>");
                        }
                        sb.Append("</tr>");
                    }
                }

                //Cá loại 5, -1, 6
                for (i = 4; i < 7; i++)
                {
                    DataRow[] r = new DataRow[6];
                    for (int jj = 0; jj < 6; jj++)
                    {
                        r[jj] = dt.Rows[i * 6 + jj];
                    }
                    sb.Append("<tr>");
                    if (i == 4) sb.Append("<td style='vertical-align:middle;'>5.1</td>");
                    else if (i == 5) sb.Append("<td style='vertical-align:middle;'>5.2</td>");
                    else sb.Append("<td style='vertical-align:middle;'>" + i.ToString() + "</td>");
                    string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                    int[,] GiaTri = new int[3, 21];
                    int t;

                    //-------------------------------------------

                    //GiaTri[1, 0] - Giong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[1, 1] += Convert.ToInt32(r[2]["TonDau"]);
                    GiaTri[1, 2] += Convert.ToInt32(r[1]["TonDau"]);
                    //GiaTri[2, 0] - Tang Trong Ton Dau
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    GiaTri[2, 1] += Convert.ToInt32(r[5]["TonDau"]);
                    GiaTri[2, 2] += Convert.ToInt32(r[4]["TonDau"]);
                    //GiaTri[0, 0] - Tong Ton Dau
                    GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
                    GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];
                    GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];
                    //-------------------------------------------

                    //GiaTri[1, 1] - Giong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[1, 4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]);
                    GiaTri[1, 5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]);
                    //GiaTri[2, 1] - Tang Trong Nhap
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                    }
                    GiaTri[2, 4] += Convert.ToInt32(r[5]["Nhap_NhapChuong"]) + Convert.ToInt32(r[5]["Nhap_CLC"]) + Convert.ToInt32(r[5]["Nhap_CGTO"]) + Convert.ToInt32(r[5]["Nhap_CGTI"]) + Convert.ToInt32(r[5]["Nhap_CGT_D"]) + Convert.ToInt32(r[5]["Nhap_CPLO"]) + Convert.ToInt32(r[5]["Nhap_CPLI"]) + Convert.ToInt32(r[5]["Nhap_CPL_D"]) + Convert.ToInt32(r[5]["Nhap_CTT"]);
                    GiaTri[2, 5] += Convert.ToInt32(r[4]["Nhap_NhapChuong"]) + Convert.ToInt32(r[4]["Nhap_CLC"]) + Convert.ToInt32(r[4]["Nhap_CGTO"]) + Convert.ToInt32(r[4]["Nhap_CGTI"]) + Convert.ToInt32(r[4]["Nhap_CGT_D"]) + Convert.ToInt32(r[4]["Nhap_CPLO"]) + Convert.ToInt32(r[4]["Nhap_CPLI"]) + Convert.ToInt32(r[4]["Nhap_CPL_D"]) + Convert.ToInt32(r[4]["Nhap_CTT"]);
                    //GiaTri[0, 1] - Tong Nhap
                    GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
                    GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];
                    GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];
                    //-------------------------------------------

                    //GiaTri[1, 2] - Giong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[1, 7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGTO"]) + Convert.ToInt32(r[2]["Xuat_CGTI"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPLO"]) + Convert.ToInt32(r[2]["Xuat_CPLI"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);
                    GiaTri[1, 8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGTO"]) + Convert.ToInt32(r[1]["Xuat_CGTI"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPLO"]) + Convert.ToInt32(r[1]["Xuat_CPLI"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                    //GiaTri[2, 2] - Tang Trong Xuat
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    GiaTri[2, 7] += Convert.ToInt32(r[5]["Xuat_Ban"]) + Convert.ToInt32(r[5]["Xuat_CLC"]) + Convert.ToInt32(r[5]["Xuat_CGTO"]) + Convert.ToInt32(r[5]["Xuat_CGTI"]) + Convert.ToInt32(r[5]["Xuat_CGT_D"]) + Convert.ToInt32(r[5]["Xuat_CPLO"]) + Convert.ToInt32(r[5]["Xuat_CPLI"]) + Convert.ToInt32(r[5]["Xuat_CPL_D"]);
                    GiaTri[2, 8] += Convert.ToInt32(r[4]["Xuat_Ban"]) + Convert.ToInt32(r[4]["Xuat_CLC"]) + Convert.ToInt32(r[4]["Xuat_CGTO"]) + Convert.ToInt32(r[4]["Xuat_CGTI"]) + Convert.ToInt32(r[4]["Xuat_CGT_D"]) + Convert.ToInt32(r[4]["Xuat_CPLO"]) + Convert.ToInt32(r[4]["Xuat_CPLI"]) + Convert.ToInt32(r[4]["Xuat_CPL_D"]);
                    //GiaTri[0, 2] - Tong Xuat
                    GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
                    GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];
                    GiaTri[0, 8] = GiaTri[1, 8] + GiaTri[2, 8];
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[1, 10] += Convert.ToInt32(r[2]["Chet"]);
                    GiaTri[1, 11] += Convert.ToInt32(r[1]["Chet"]);
                    //GiaTri[2, 3] - Tang Trong Chet
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    GiaTri[2, 10] += Convert.ToInt32(r[5]["Chet"]);
                    GiaTri[2, 11] += Convert.ToInt32(r[4]["Chet"]);
                    //GiaTri[0, 3] - Tong Chet
                    GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
                    GiaTri[0, 10] = GiaTri[1, 10] + GiaTri[2, 10];
                    GiaTri[0, 11] = GiaTri[1, 11] + GiaTri[2, 11];
                    //-------------------------------------------

                    //GiaTri[1, 3] - Giong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[1, 13] += Convert.ToInt32(r[2]["LoaiThai"]);
                    GiaTri[1, 14] += Convert.ToInt32(r[1]["LoaiThai"]);
                    //GiaTri[2, 4] - Tang Trong Giet Mo
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    GiaTri[2, 13] += Convert.ToInt32(r[5]["LoaiThai"]);
                    GiaTri[2, 14] += Convert.ToInt32(r[4]["LoaiThai"]);
                    //GiaTri[0, 4] - Tong Giet Mo
                    GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
                    GiaTri[0, 13] = GiaTri[1, 13] + GiaTri[2, 13];
                    GiaTri[0, 14] = GiaTri[1, 14] + GiaTri[2, 14];
                    //-------------------------------------------

                    //GiaTri[1, 4] - Giong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[1, 16] += Convert.ToInt32(r[2]["GietMo"]);
                    GiaTri[1, 17] += Convert.ToInt32(r[1]["GietMo"]);
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    GiaTri[2, 16] += Convert.ToInt32(r[5]["GietMo"]);
                    GiaTri[2, 17] += Convert.ToInt32(r[4]["GietMo"]);
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
                    GiaTri[0, 16] = GiaTri[1, 16] + GiaTri[2, 16];
                    GiaTri[0, 17] = GiaTri[1, 17] + GiaTri[2, 17];
                    //-------------------------------------------

                    //GiaTri[1, 5] - Giong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[1, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[1, 19] += Convert.ToInt32(r[2]["TonCuoi"]);
                    GiaTri[1, 20] += Convert.ToInt32(r[1]["TonCuoi"]);
                    //GiaTri[2, 5] - Tang Trong Ton Cuoi
                    for (t = 3; t < 6; t++)
                    {
                        GiaTri[2, 18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    GiaTri[2, 19] += Convert.ToInt32(r[5]["TonCuoi"]);
                    GiaTri[2, 20] += Convert.ToInt32(r[4]["TonCuoi"]);
                    //GiaTri[0, 5] - Tong Ton Cuoi
                    GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
                    GiaTri[0, 19] = GiaTri[1, 19] + GiaTri[2, 19];
                    GiaTri[0, 20] = GiaTri[1, 20] + GiaTri[2, 20];
                    //-------------------------------------------

                    int iThucAnGoc = iThucAn;
                    for (t = 0; t < 3; t++)
                    {
                        if (t == 0)
                        {
                            sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 3]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 2]["GietMo"]), 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                    j++;
                                }
                                else
                                {
                                    iThucAn = h;
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            sb.Append("<td></td>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td>");
                            sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                            for (int y = 0; y < 15; y++)
                            {
                                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                            }
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 15], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 2]["GietMo"], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 16], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 17], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19], 0) + "</td>");
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20], 0) + "</td>");
                            //
                            //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn Start
                            j = 0;
                            for (int h = iThucAnGoc; h < dtSub.Rows.Count; h++)
                            {
                                DataRow rta = dtSub.Rows[h];
                                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                                {
                                    if (t == 1)
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                    }
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                            //ThucAn End
                            //
                            sb.Append("<td></td>");
                        }
                        sb.Append("</tr>");
                    }
                }

                //Cá từ loại 7 -> loại 11
                for (i = 7; i < 12; i++)
                {
                    DataRow[] r = new DataRow[3];
                    for (int jj = 0; jj < 3; jj++)
                    {
                        r[jj] = dt.Rows[i * 6 + jj];
                    }
                    sb.Append("<tr>");
                    sb.Append("<td>" + i.ToString() + "</td>");
                    sb.Append("<td align='left'>" + r[0]["TenLoaiCa"].ToString() + "</td>");
                    int[] GiaTri = new int[21];
                    int t;

                    //-------------------------------------------

                    //GiaTri[0] - Tong Ton Dau
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                    }
                    //GiaTri[2] - Cai Ton Dau
                    GiaTri[2] += Convert.ToInt32(r[1]["TonDau"]);// +Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) - Convert.ToInt32(r[1]["Xuat_CGTO"]) - Convert.ToInt32(r[1]["Xuat_CPLO"]);
                    //GiaTri[1] - Duc Ton Dau
                    GiaTri[1] += Convert.ToInt32(r[2]["TonDau"]);// +Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) - Convert.ToInt32(r[2]["Xuat_CGTO"]) - Convert.ToInt32(r[2]["Xuat_CPLO"]);

                    //-------------------------------------------

                    //GiaTri[3] - Tong Nhap
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CGTI"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]);
                    }
                    //GiaTri[5] - Cai Nhap
                    GiaTri[5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]);// -Convert.ToInt32(r[1]["Xuat_CGTI"]) - Convert.ToInt32(r[1]["Xuat_CPLI"]);
                    //GiaTri[4] - Duc Nhap
                    GiaTri[4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]);// -Convert.ToInt32(r[2]["Xuat_CGTI"]) - Convert.ToInt32(r[2]["Xuat_CPLI"]);

                    //-------------------------------------------

                    //GiaTri[6] - Tong Xuat
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                    }
                    //GiaTri[8] - Cai Xuat
                    GiaTri[8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGTO"]) + Convert.ToInt32(r[1]["Xuat_CGTI"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPLO"]) + Convert.ToInt32(r[1]["Xuat_CPLI"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                    //GiaTri[7] - Duc Xuat
                    GiaTri[7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGTO"]) + Convert.ToInt32(r[2]["Xuat_CGTI"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPLO"]) + Convert.ToInt32(r[2]["Xuat_CPLI"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);

                    //-------------------------------------------

                    //GiaTri[9] - Tong Chet
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[9] += Convert.ToInt32(r[t]["Chet"]);
                    }
                    //GiaTri[11] - Cai Chet
                    GiaTri[11] += Convert.ToInt32(r[1]["Chet"]);
                    //GiaTri[10] - Duc Chet
                    GiaTri[10] += Convert.ToInt32(r[2]["Chet"]);

                    //-------------------------------------------

                    //GiaTri[9] - Tong LoaiThai
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[12] += Convert.ToInt32(r[t]["LoaiThai"]);
                    }
                    //GiaTri[14] - Cai Giet Mo
                    GiaTri[14] += Convert.ToInt32(r[1]["LoaiThai"]);
                    //GiaTri[13] - Duc Giet Mo
                    GiaTri[13] += Convert.ToInt32(r[2]["LoaiThai"]);

                    //-------------------------------------------

                    //GiaTri[12] - Tong Giet Mo
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[15] += Convert.ToInt32(r[t]["GietMo"]);
                    }
                    //GiaTri[17] - Cai Ton Cuoi
                    GiaTri[17] += Convert.ToInt32(r[1]["GietMo"]);
                    //GiaTri[16] - Duc Ton Cuoi
                    GiaTri[16] += Convert.ToInt32(r[2]["GietMo"]);

                    //-------------------------------------------

                    //GiaTri[15] - Tong Ton Cuoi
                    for (t = 0; t < 3; t++)
                    {
                        GiaTri[18] += Convert.ToInt32(r[t]["TonCuoi"]);
                    }
                    //GiaTri[17] - Cai Ton Cuoi
                    GiaTri[20] += Convert.ToInt32(r[1]["TonCuoi"]);
                    //GiaTri[16] - Duc Ton Cuoi
                    GiaTri[19] += Convert.ToInt32(r[2]["TonCuoi"]);

                    //-------------------------------------------
                    for (int y = 0; y < 5; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 1], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 2], 0) + "</td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[15], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + 1]["GietMo"], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4]["GietMo"], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[16], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[17], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[18], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[19], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[20], 0) + "</td>");
                    //ThucAn Start
                    j = 0;
                    for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                    {
                        DataRow rta = dtSub.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                        {
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                            j++;
                        }
                        else
                        {
                            iThucAn = h;
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                }
                //Tính tổng
                int[] Tong = new int[21];
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    Tong[0] += Convert.ToInt32(r["TonDau"]);// +Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]); ;
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[2] += Convert.ToInt32(r["TonDau"]);// +Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]); ;
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[1] += Convert.ToInt32(r["TonDau"]);// +Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]); ;
                    }

                    Tong[3] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLO"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]);// -Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[5] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLO"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]);// -Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[4] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLO"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]);// -Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                    }

                    Tong[6] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGTO"]) + Convert.ToInt32(r["Xuat_CGTI"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPLO"]) + Convert.ToInt32(r["Xuat_CPLI"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[8] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGTO"]) + Convert.ToInt32(r["Xuat_CGTI"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPLO"]) + Convert.ToInt32(r["Xuat_CPLI"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[7] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGTO"]) + Convert.ToInt32(r["Xuat_CGTI"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPLO"]) + Convert.ToInt32(r["Xuat_CPLI"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                    }

                    Tong[9] += Convert.ToInt32(r["Chet"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[11] += Convert.ToInt32(r["Chet"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[10] += Convert.ToInt32(r["Chet"]);
                    }

                    Tong[12] += Convert.ToInt32(r["LoaiThai"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[14] += Convert.ToInt32(r["LoaiThai"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[13] += Convert.ToInt32(r["LoaiThai"]);
                    }

                    Tong[15] += Convert.ToInt32(r["GietMo"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[17] += Convert.ToInt32(r["GietMo"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[16] += Convert.ToInt32(r["GietMo"]);
                    }

                    Tong[18] += Convert.ToInt32(r["TonCuoi"]);
                    if (i >= 24 && i % 3 == 1)
                    {
                        Tong[20] += Convert.ToInt32(r["TonCuoi"]);
                    }
                    if (i >= 24 && i % 3 == 2)
                    {
                        Tong[19] += Convert.ToInt32(r["TonCuoi"]);
                    }
                }
                //Tính tổng phân loại giêt mô
                int[] TongPLGM = new int[2];
                for (i = 0; i < dtPLGM.Rows.Count; i++)
                {
                    DataRow r = dtPLGM.Rows[i];
                    if (i % 2 == 1)
                    {
                        TongPLGM[1] += Convert.ToInt32(r["GietMo"]);
                    }
                    else
                    {
                        TongPLGM[0] += Convert.ToInt32(r["GietMo"]);
                    }
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td></td><td style='text-align:center;font-weight:bold;'>Tổng cộng</td>");
                for (i = 0; i < 15; i++)
                {
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[i], 0) + "</td>");
                }
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[15], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TongPLGM[1], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TongPLGM[0], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[16], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[17], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[18], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[19], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[20], 0) + "</td>");

                for (i = 0; i < SoCotThucAn; i++)
                {
                    sb.Append(@"<td></td>");
                }
                sb.Append("<td></td></tr>");
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