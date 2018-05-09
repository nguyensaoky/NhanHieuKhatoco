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
    public partial class r_casaucaichuade : DotNetNuke.Entities.Modules.PortalModuleBase
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

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string filename = "TongHopCaSauCaiChuaDe";
                string tieude = "";
                string strSQL = "QLCS_BCTK_CaSauCaiChuaDe";
                SqlParameter[] param = new SqlParameter[3];
                string StrLoaiCa = "";
                if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
                {
                    StrLoaiCa = "@" + Config.GetSelectedValues(ddlLoaiCa).Replace(", ", "@@").Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 1);
                }
                param[0] = new SqlParameter("@year", int.Parse(txtYear.Text));
                param[1] = new SqlParameter("@fromyear", int.Parse(txtFromYear.Text));
                param[2] = new SqlParameter("@StrLoaiCa", StrLoaiCa);
                filename += txtYear.Text + "_" + txtFromYear.Text + ".xls";
                tieude += "<b>DANH SÁCH CÁ SẤU CÁI CHƯA ĐẺ NĂM " + txtYear.Text + "</b>";
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                ArrayList lstNam = new ArrayList();
                for (int i = int.Parse(txtFromYear.Text); i < int.Parse(txtYear.Text); i++)
                { 
                    lstNam.Add(i.ToString());
                }
                int tongCa = 0;
                int totalCa = 0;
                int[] arrTong = new int[lstNam.Count];
                int[] arrTotal = new int[lstNam.Count];
                for (int i = 0; i < lstNam.Count; i++)
                {
                    arrTong[i] = 0;
                    arrTotal[i] = 0;
                }
                string currLoaiCa = "";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table border='1'>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <td>STT</td><td>LOẠI CÁ</td><td>VI CẮT</td><td>MÃ</td><td>Ô CHUỒNG</td><td>NGUỒN GỐC</td><td>LỊCH SỬ ĐẺ</td>");
                foreach (string col in lstNam)
	            {
            	    sb.Append("<td>NĂM " + col + "</td>");
	            }
                sb.Append("</tr>");
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
                            //Cộng tổng vào total
                            arrTotal[k] += arrTong[k];
                            //Reset tổng
                            arrTong[k] = 0;
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
                        if (r[col].ToString() != "")
                        {
                            sb.Append("<td style='text-align:center;'>" + r[col].ToString() + "</td>");
                            arrTong[j]++;
                        }
                        else sb.Append("<td></td>");
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
                    //Cộng tổng vào total
                    arrTotal[k] += arrTong[k];
                }
                sb.Append("</tr>");
                //Cộng tổng cá vào total cá
                totalCa += tongCa;
                //Viết dòng total
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td style='text-align:center;'>Tổng cộng</td><td></td><td align='right'>" + Config.ToXVal2(totalCa, 0) + @"</td><td></td><td></td><td></td><td></td>");
                for (int k = 0; k < arrTotal.Length; k++)
                {
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrTotal[k], 0) + "</td>");
                }
                sb.Append("</tr>");
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
                string strSQL = "QLCS_BCTK_CaSauCaiChuaDe";
                SqlParameter[] param = new SqlParameter[3];
                string StrLoaiCa = "";
                if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
                {
                    StrLoaiCa = "@" + Config.GetSelectedValues(ddlLoaiCa).Replace(", ", "@@").Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 1);
                }
                param[0] = new SqlParameter("@year", int.Parse(txtYear.Text));
                param[1] = new SqlParameter("@fromyear", int.Parse(txtFromYear.Text));
                param[2] = new SqlParameter("@StrLoaiCa", StrLoaiCa);
                tieude += "<b>DANH SÁCH CÁ SẤU CÁI CHƯA ĐẺ NĂM " + txtYear.Text + "</b>";
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                ArrayList lstNam = new ArrayList();
                for (int i = int.Parse(txtFromYear.Text); i < int.Parse(txtYear.Text); i++)
                {
                    lstNam.Add(i.ToString());
                }
                int tongCa = 0;
                int totalCa = 0;
                int[] arrTong = new int[lstNam.Count];
                int[] arrTotal = new int[lstNam.Count];
                for (int i = 0; i < lstNam.Count; i++)
                {
                    arrTong[i] = 0;
                    arrTotal[i] = 0;
                }
                string currLoaiCa = "";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr><th>STT</th><th>LOẠI CÁ</th><th>VI CẮT</th><th>MÃ</th><th>Ô CHUỒNG</th><th>NGUỒN GỐC</th><th>LỊCH SỬ ĐẺ</th>");
                foreach (string col in lstNam)
                {
                    sb.Append("<th>NĂM " + col + "</th>");
                }
                sb.Append("</tr></thead><tbody>");
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
                            //Cộng tổng vào total
                            arrTotal[k] += arrTong[k];
                            //Reset tổng
                            arrTong[k] = 0;
                        }
                        sb.Append("</tr>");
                        //Cộng tổng cá vào total cá
                        totalCa += tongCa;
                        //Reset tổng cá
                        tongCa = 0;
                    }
                    currLoaiCa = r["TenLoaiCa"].ToString();
                    index = i + 1;
                    sb.Append("<tr><td style='text-align:center;'>" + index.ToString() + "</td><td>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["MaSoGoc"].ToString() + "</td><td>" + r["MaSo"].ToString() + "</td><td>" + r["TenChuong"].ToString() + "</td><td>" + r["TenNguonGoc"].ToString() + @"</td><td align='center'><a onclick='openwindow(""" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdListBienDongPage.Value), "", "IDCaSau/" + r["IDCaSau"].ToString()) + @""","""",800,600);' style='cursor:pointer;font-weight:bold;'>Xem</a></td>");
                    int j = 0;
                    foreach (string col in lstNam)
                    {
                        if (r[col].ToString() != "")
                        {
                            sb.Append("<td style='text-align:center;'>" + r[col].ToString() + "</td>");
                            arrTong[j]++;
                        }
                        else sb.Append("<td></td>");
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
                    //Cộng tổng vào total
                    arrTotal[k] += arrTong[k];
                }
                sb.Append("</tr>");
                //Cộng tổng cá vào total cá
                totalCa += tongCa;
                //Viết dòng total
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td style='text-align:center;'>Tổng cộng</td><td></td><td align='right'>" + Config.ToXVal2(totalCa, 0) + @"</td><td></td><td></td><td></td><td></td>");
                for (int k = 0; k < arrTotal.Length; k++)
                {
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(arrTotal[k], 0) + "</td>");
                }
                sb.Append("</tr>");
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