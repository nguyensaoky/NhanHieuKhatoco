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
    public partial class r_thongkecatheochuong : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        int stt = 1;
        int tongca = 0;
        private CaSauController csCont = new CaSauController();
        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable tblLoaiCa = new DataTable();
            tblLoaiCa = csCont.LoadLoaiCa(1);
            lstLoaiCa.DataSource = tblLoaiCa;
            lstLoaiCa.DataTextField = "TenLoaiCa";
            lstLoaiCa.DataValueField = "IDLoaiCa";
            lstLoaiCa.DataBind();
            //lstLoaiCa.Items.Insert(0, new ListItem("", "0"));
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
                DataSet ds = Config.SelectSPs("QLCS_DanhMuc_GetAllChuong_TenChuong", new SqlParameter[] { });
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    ListBox lstChuong = new ListBox();
                    lstChuong.ID = "lstChuong" + i.ToString();
                    lstChuong.DataSource = ds.Tables[i];
                    lstChuong.DataTextField = "Chuong";
                    lstChuong.DataValueField = "IDChuong";
                    lstChuong.DataBind();
                    //lstChuong.Items.Insert(0, new ListItem("", "0"));
                    lstChuong.Rows = 6;
                    lstChuong.SelectionMode = ListSelectionMode.Multiple;
                    lstChuong.EnableViewState = true;
                    phChuong.Controls.Add(lstChuong);
                }
                hdNumListChuong.Value = ds.Tables.Count.ToString();
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
                string filename = "ThongKeTheoChuong";
                filename += txtDate.Text + ".xls";
                filename = filename.Replace("/", "_");

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                string strSQL = "QLCS_BCTK_CaTheoChuong";
                SqlParameter[] param = new SqlParameter[3];
                if (txtDate.Text == "")
                {
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@d", txtDate.Text);
                string sChuong = "";
                for (int j = 0; j < int.Parse(hdNumListChuong.Value); j++)
                {
                    sChuong += Config.GetSelectedValues_At((ListBox)(phChuong.FindControl("lstChuong" + j.ToString()))).Replace("@0@","");
                }
                param[1] = new SqlParameter("@StrChuong", sChuong);
                string sLoaiCa = Config.GetSelectedValues_At(lstLoaiCa).Replace("@0@", "");
                param[2] = new SqlParameter("@StrLoaiCa", sLoaiCa);

                string tieude = "<b>BẢNG THỐNG KÊ ĐÀN CÁ SẤU THEO CHUỒNG ĐẾN TRƯỚC NGÀY " + txtDate.Text + "</b>";
                if (sLoaiCa != "") tieude += "<br/>LOẠI CÁ: " + Config.GetSelectedTexts(lstLoaiCa);

                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt;

                string[] aLoaiCa = Config.GetSelectedTexts(lstLoaiCa).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");
                sb.Append("<table border='1'><tr><td colspan='5'><center style='font-weight:bold;font-size:14pt;'>" + tieude + "</center><br/></td></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <td>STT</td>
                          <td>CHUỒNG</td>
                          <td>SỐ LƯỢNG CÁ</td>
                          <td>GIỐNG</td>
                          <td>TĂNG TRỌNG</td>
                         </tr>");
                int i = 1;
                int tong = 0;
                int tonggiong = 0;
                int tongtt = 0;

                if (aLoaiCa.Length == 0)
                {
                    dt = ds.Tables[0];
                    foreach (DataRow r in dt.Rows)
                    {
                        sb.Append("<tr><td>" + i.ToString() + "</td><td style='text-align:left;'>" + r["Chuong"].ToString() + "</td><td style='text-align:right;'>" + r["SoLuongTong"].ToString() + "</td><td style='text-align:right;'>" + r["SoLuongGiong"].ToString() + "</td><td style='text-align:right;'>" + r["SoLuongTT"].ToString() + "</td></tr>");
                        i++;
                        tong += Convert.ToInt32(r["SoLuongTong"]);
                        tonggiong += Convert.ToInt32(r["SoLuongGiong"]);
                        tongtt += Convert.ToInt32(r["SoLuongTT"]);
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td style='font-weight:bold;'>T.C</td><td></td>");
                    sb.Append(@"<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tong, 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tonggiong, 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tongtt, 0) + "</td></tr>");
                    sb.Append("</table>");
                    sb.Append(Config.Footer(UserInfo.Profile.City, UserInfo.FirstName + " " + UserInfo.LastName));
                    sb.Append("</body></html>");
                }
                else
                {
                    int Total = 0;
                    int TotalGiong = 0;
                    int TotalTT = 0;
                    for (int j = 0; j < aLoaiCa.Length; j++)
                    {
                        i = 1;
                        tong = 0;
                        tonggiong = 0;
                        tongtt = 0;
                        dt = ds.Tables[j];
                        sb.Append("<tr><td align='center'><b><span style='color:#f00;'>" + aLoaiCa[j] + "</span></b></td><td></td><td></td><td></td><td></td></tr>");
                        foreach (DataRow r in dt.Rows)
                        {
                            sb.Append("<tr><td>" + i.ToString() + "</td><td style='text-align:left;'>" + r["Chuong"].ToString() + "</td><td style='text-align:right;'>" + r["SoLuongTong"].ToString() + "</td><td style='text-align:right;'>" + r["SoLuongGiong"].ToString() + "</td><td style='text-align:right;'>" + r["SoLuongTT"].ToString() + "</td></tr>");
                            i++;
                            tong += Convert.ToInt32(r["SoLuongTong"]);
                            tonggiong += Convert.ToInt32(r["SoLuongGiong"]);
                            tongtt += Convert.ToInt32(r["SoLuongTT"]);
                        }
                        Total += tong;
                        TotalGiong += tonggiong;
                        TotalTT += tongtt;
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td style='font-weight:bold;'>T.C</td><td></td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tong, 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tonggiong, 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tongtt, 0) + "</td></tr>");
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td style='font-weight:bold;'>Tổng cộng</td><td></td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Total, 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TotalGiong, 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TotalTT, 0) + "</td></tr>");
                    sb.Append("</table>");
                    sb.Append(Config.Footer(UserInfo.Profile.City, UserInfo.FirstName + " " + UserInfo.LastName));
                    sb.Append("</body></html>");
                }
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
                string strSQL = "QLCS_BCTK_CaTheoChuong";
                SqlParameter[] param = new SqlParameter[3];
                if (txtDate.Text == "")
                {
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@d", txtDate.Text);
                string sChuong = "";
                for (int j = 0; j < int.Parse(hdNumListChuong.Value); j++)
                {
                    sChuong += Config.GetSelectedValues_At((ListBox)(phChuong.FindControl("lstChuong" + j.ToString()))).Replace("@0@", "");
                }
                param[1] = new SqlParameter("@StrChuong", sChuong);
                string sLoaiCa = Config.GetSelectedValues_At(lstLoaiCa).Replace("@0@", "");
                param[2] = new SqlParameter("@StrLoaiCa", sLoaiCa);

                string tieude = "<b>BẢNG THỐNG KÊ ĐÀN CÁ SẤU THEO CHUỒNG ĐẾN TRƯỚC NGÀY " + txtDate.Text + "</b>";
                if (sLoaiCa != "") tieude += "<br/>LOẠI CÁ: " + Config.GetSelectedTexts(lstLoaiCa);

                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt;

                string[] aLoaiCa = Config.GetSelectedTexts(lstLoaiCa).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr><th align='center'>STT</th><th align='center'>Chuồng</th><th align='center'>Số lượng cá</th><th align='center'>SỐ lượng cá giống</th><th align='center'>Số lượng cá tăng trọng</th></tr></thead><tbody>");
                int i = 1;
                int tong = 0;
                int tonggiong = 0;
                int tongtt = 0;

                if (aLoaiCa.Length == 0)
                {
                    dt = ds.Tables[0];
                    foreach (DataRow r in dt.Rows)
                    {
                        sb.Append("<tr><td>" + i.ToString() + "</td><td style='text-align:left;'>" + r["Chuong"].ToString() + "</td><td style='text-align:right;'>" + r["SoLuongTong"].ToString() + "</td><td style='text-align:right;'>" + r["SoLuongGiong"].ToString() + "</td><td style='text-align:right;'>" + r["SoLuongTT"].ToString() + "</td></tr>");
                        i++;
                        tong += Convert.ToInt32(r["SoLuongTong"]);
                        tonggiong += Convert.ToInt32(r["SoLuongGiong"]);
                        tongtt += Convert.ToInt32(r["SoLuongTT"]);
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td style='font-weight:bold;text-align:center;'>T.C</td><td></td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tong, 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tonggiong, 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tongtt, 0) + "</td></tr>");
                    sb.Append("</tbody></table>");
                }
                else
                {
                    int Total = 0;
                    int TotalGiong = 0;
                    int TotalTT = 0;
                    for (int j = 0; j < aLoaiCa.Length; j++)
                    {
                        i = 1;
                        tong = 0;
                        tonggiong = 0;
                        tongtt = 0;
                        dt = ds.Tables[j];
                        sb.Append("<tr><td align='center'><b><span style='color:#f00;'>" + aLoaiCa[j] + "</span></b></td><td></td><td></td><td></td><td></td></tr>");
                        foreach (DataRow r in dt.Rows)
                        {
                            sb.Append("<tr><td>" + i.ToString() + "</td><td style='text-align:left;'>" + r["Chuong"].ToString() + "</td><td style='text-align:right;'>" + r["SoLuongTong"].ToString() + "</td><td style='text-align:right;'>" + r["SoLuongGiong"].ToString() + "</td><td style='text-align:right;'>" + r["SoLuongTT"].ToString() + "</td></tr>");
                            i++;
                            tong += Convert.ToInt32(r["SoLuongTong"]);
                            tonggiong += Convert.ToInt32(r["SoLuongGiong"]);
                            tongtt += Convert.ToInt32(r["SoLuongTT"]);
                        }
                        Total += tong;
                        TotalGiong += tonggiong;
                        TotalTT += tongtt;
                        sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td style='font-weight:bold;text-align:center;'>T.C</td><td></td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tong, 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tonggiong, 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(tongtt, 0) + "</td></tr>");
                    }
                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td style='font-weight:bold;text-align:center;'>Tổng cộng</td><td></td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Total, 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TotalGiong, 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TotalTT, 0) + "</td></tr>");
                    sb.Append("</tbody></table>");
                }
                lt.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}