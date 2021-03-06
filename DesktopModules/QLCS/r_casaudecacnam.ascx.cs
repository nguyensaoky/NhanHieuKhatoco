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
    public partial class r_casaudecacnam : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        private void BindControls()
        {
            int fromYear = DateTime.Now.Year - 1;
            txtFromYear.Text = fromYear.ToString();
            txtYear.Text = DateTime.Now.Year.ToString();

            DataTable dtLoaiCa = csCont.LoadLoaiCaLon();
            ddlLoaiCa.DataSource = dtLoaiCa;
            ddlLoaiCa.DataTextField = "TenLoaiCa";
            ddlLoaiCa.DataValueField = "IDLoaiCa";
            ddlLoaiCa.DataBind();
            //ddlLoaiCa.Items.Insert(0, new ListItem("", "0"));
        }
        string tieude = "";
        DataTable dt = null;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        ArrayList lstNam = new ArrayList();

        protected void Page_Load(object sender, EventArgs e)
        {
            ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
            try
            {
                if (!Page.IsPostBack)
                {
                    BindControls();
                }
                DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
                hdListBienDongPage.Value = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauListBienDongPage"], PortalId).TabID.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        public void prepareExcelFile(string name, System.Web.HttpResponse resp)
        {
            resp.ClearContent();
            resp.ClearHeaders();
            resp.AppendHeader("Content-Disposition", "attachment; filename=" + name + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
            resp.ContentType = "application/vnd.ms-excel";
        }

        public void prepareData()
        {
            string strSQL = "QLCS_BCTK_CaSauDeCacNam";
            SqlParameter[] param = new SqlParameter[3];
            string StrLoaiCa = "";
            if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
            {
                StrLoaiCa = "@" + Config.GetSelectedValues(ddlLoaiCa).Replace(", ", "@@").Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 1);
            }
            param[0] = new SqlParameter("@year", int.Parse(txtYear.Text) + 1);
            param[1] = new SqlParameter("@fromyear", int.Parse(txtFromYear.Text));
            param[2] = new SqlParameter("@StrLoaiCa", StrLoaiCa);
            dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

            lstNam = new ArrayList();
            for (int i = int.Parse(txtFromYear.Text); i < int.Parse(txtYear.Text) + 1; i++)
            {
                lstNam.Add(i.ToString());
            }
        }

        public void prepareTitleExcel()
        {
            tieude = "<b>DANH SÁCH CÁ SẤU CÁI VÀ LỊCH SỬ ĐẺ TỪ NĂM " + txtFromYear.Text + " ĐẾN NĂM " + txtYear.Text + "</b>";
            sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
        }

        public void prepareTitleView()
        {
            tieude = "<b>DANH SÁCH CÁ SẤU CÁI VÀ LỊCH SỬ ĐẺ TỪ NĂM " + txtFromYear.Text + " ĐẾN NĂM " + txtYear.Text + "</b>";
            sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
        }

        public void prepareFooterExcel()
        {
            sb.Append("<br/>");
            sb.Append(Config.Footer(UserInfo.Profile.City, UserInfo.FirstName + " " + UserInfo.LastName));
            sb.Append("</body></html>");
        }

        public void createTableHeader(bool Excel)
        {
            if (Excel)
            {
                sb.Append("<table border='1' id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
            }
            else
            {
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
            }

            sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;text-align:center;'>
                        <td rowspan='3'>STT</td><td rowspan='3'>LOẠI CÁ</td><td rowspan='3'>VI CẮT</td><td rowspan='3'>MÃ</td><td rowspan='3'>Ô CHUỒNG</td><td rowspan='3'>NGUỒN GỐC</td><td rowspan='3'>LỊCH SỬ ĐẺ</td>");
            foreach (string col in lstNam)
            {
                sb.Append("<td colspan='10'>NĂM " + col + "</td>");
            }
            sb.Append("</tr><tr style='text-align:center;'>");
            foreach (string col in lstNam)
            {
                sb.Append("<td rowspan='2'>Loại cá</td><td colspan='5'>Định mức</td><td rowspan='2'>Trứng đẻ</td><td rowspan='2'>Trứng ấp</td><td rowspan='2'>Có phôi</td><td rowspan='2'>Nở</td>");
            }
            sb.Append("</tr><tr style='text-align:center;'>");
            foreach (string col in lstNam)
            {
                sb.Append("<td>Số trứng đẻ/mái/năm</td><td>Tỷ lệ trứng ấp/trứng đẻ</td><td>Tỷ lệ có phôi/trứng ấp</td><td>Tỷ lệ nở/có phôi</td><td>Số con nở/mái/năm</td>");
            }
            sb.Append("</tr>");
            sb.Append("</thead><tbody>");
        }

        public void CreateContent()
        {
            int tongCa = 0;
            int totalCa = 0;
            int[] arrTong = new int[lstNam.Count];
            int[] arrTotal = new int[lstNam.Count];
            int[] arrDeTong = new int[lstNam.Count];
            int[] arrDeTotal = new int[lstNam.Count];
            int[] arrApTong = new int[lstNam.Count];
            int[] arrApTotal = new int[lstNam.Count];
            int[] arrCoPhoiTong = new int[lstNam.Count];
            int[] arrCoPhoiTotal = new int[lstNam.Count];
            int[] arrNoTong = new int[lstNam.Count];
            int[] arrNoTotal = new int[lstNam.Count];
            for (int i = 0; i < lstNam.Count; i++)
            {
                arrTong[i] = 0;
                arrTotal[i] = 0;
                arrDeTong[i] = 0;
                arrDeTotal[i] = 0;
                arrApTong[i] = 0;
                arrApTotal[i] = 0;
                arrCoPhoiTong[i] = 0;
                arrCoPhoiTotal[i] = 0;
                arrNoTong[i] = 0;
                arrNoTotal[i] = 0;
            }
            string currLoaiCa = "";
            int index;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow r = dt.Rows[i];
                if (currLoaiCa != r["TenLoaiCa"].ToString() && currLoaiCa != "")
                {
                    //Viết dòng tổng
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td style='text-align:center;'>T.C</td><td></td><td align='right'>" + Config.ToXVal2(tongCa, 0) + @"</td><td></td><td></td><td></td><td></td>");
                    for (int k = 0; k < arrTong.Length; k++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrTong[k], 0) + "</td>");
                        sb.Append("<td></td><td></td><td></td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrDeTong[k], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrApTong[k], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrCoPhoiTong[k], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrNoTong[k], 0) + "</td>");
                        //Cộng tổng vào total
                        arrTotal[k] += arrTong[k];
                        arrDeTotal[k] += arrDeTong[k];
                        arrApTotal[k] += arrApTong[k];
                        arrCoPhoiTotal[k] += arrCoPhoiTong[k];
                        arrNoTotal[k] += arrNoTong[k];
                        //Reset tổng
                        arrTong[k] = 0;
                        arrDeTong[k] = 0;
                        arrApTong[k] = 0;
                        arrCoPhoiTong[k] = 0;
                        arrNoTong[k] = 0;
                    }
                    sb.Append("</tr>");
                    //Cộng tổng cá vào total cá
                    totalCa += tongCa;
                    //Reset tổng cá
                    tongCa = 0;
                }
                currLoaiCa = r["TenLoaiCa"].ToString();
                index = i + 1;
                sb.Append("<tr><td>" + index.ToString() + "</td><td style='text-align:left;'>" + r["TenLoaiCa"].ToString() + "</td><td style='text-align:left;'>" + r["MaSoGoc"].ToString() + "</td><td style='text-align:left;'>" + r["MaSo"].ToString() + "</td><td style='text-align:left;'>" + r["TenChuong"].ToString() + "</td><td style='text-align:left;'>" + r["TenNguonGoc"].ToString() + @"</td><td align='center'><a href='" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdListBienDongPage.Value), "", "IDCaSau/" + r["IDCaSau"].ToString()) + @"' style='cursor:pointer;font-weight:bold;'>Xem</a></td>");
                int j = 0;
                foreach (string col in lstNam)
                {
                    string sCol = r[col].ToString();
                    if (sCol != "")
                    {
                        string[] aCol = sCol.Split(new char[] { '/' });
                        if (aCol.Length == 10)
                        {
                            sb.Append("<td style='text-align:right;'>" + aCol[5] + "</td>");
                            for (int k = 0; k < 5; k++)
                            {
                                sb.Append("<td style='text-align:center;'>" + aCol[k].Split(new char[] { ':' })[1] + "</td>");
                            }
                            for (int k = 6; k < aCol.Length; k++)
                            {
                                sb.Append("<td style='text-align:right;'>" + aCol[k].Split(new char[] { ':' })[1] + "</td>");
                            }
                            arrTong[j]++;
                            arrDeTong[j] += int.Parse(aCol[6].Split(new char[] { ':' })[1]);
                            arrApTong[j] += int.Parse(aCol[7].Split(new char[] { ':' })[1]);
                            arrCoPhoiTong[j] += int.Parse(aCol[8].Split(new char[] { ':' })[1]);
                            arrNoTong[j] += int.Parse(aCol[9].Split(new char[] { ':' })[1]);
                        }
                        else
                        {
                            sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
                        }
                    }
                    else sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
                    j++;
                }
                sb.Append("</tr>");
                tongCa++;
            }
            //Viết dòng tổng
            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td style='text-align:center;'>T.C</td><td></td><td align='right'>" + Config.ToXVal2(tongCa, 0) + @"</td><td></td><td></td><td></td><td></td>");
            for (int k = 0; k < arrTong.Length; k++)
            {
                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrTong[k], 0) + "</td>");
                sb.Append("<td></td><td></td><td></td><td></td><td></td>");
                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrDeTong[k], 0) + "</td>");
                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrApTong[k], 0) + "</td>");
                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrCoPhoiTong[k], 0) + "</td>");
                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrNoTong[k], 0) + "</td>");
                //Cộng tổng vào total
                arrTotal[k] += arrTong[k];
                arrDeTotal[k] += arrDeTong[k];
                arrApTotal[k] += arrApTong[k];
                arrCoPhoiTotal[k] += arrCoPhoiTong[k];
                arrNoTotal[k] += arrNoTong[k];
            }
            sb.Append("</tr>");
            //Cộng tổng cá vào total cá
            totalCa += tongCa;
            //Viết dòng total
            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td style='text-align:center;'>Tổng cộng</td><td></td><td align='right'>" + Config.ToXVal2(totalCa, 0) + @"</td><td></td><td></td><td></td><td></td>");
            for (int k = 0; k < arrTotal.Length; k++)
            {
                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrTotal[k], 0) + "</td>");
                sb.Append("<td></td><td></td><td></td><td></td><td></td>");
                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrDeTotal[k], 0) + "</td>");
                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrApTotal[k], 0) + "</td>");
                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrCoPhoiTotal[k], 0) + "</td>");
                sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrNoTotal[k], 0) + "</td>");
            }
            sb.Append("</tr>");
            sb.Append("</table>");
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                prepareExcelFile("TongHopCaDeCacNam", Response);

                prepareData();

                prepareTitleExcel();

                createTableHeader(true);

                CreateContent();

                prepareFooterExcel();

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
                prepareData();

                prepareTitleView();

                createTableHeader(false);

                CreateContent();

                lt.Text = sb.ToString();
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}