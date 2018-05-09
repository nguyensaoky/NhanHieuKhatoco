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
    public partial class r_thongkechuyengiaidoan : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        private void BindControls()
        {
            for (int i = 2011; i < DateTime.Now.Year + 1; i++)
            {
                lstYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == DateTime.Now.Year) lstYear.Items[lstYear.Items.Count - 1].Selected = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string tieude = "";
                tieude += "<b>BẢNG THỐNG KÊ CÁ CHUYỂN GIAI ĐOẠN NĂM " + Config.GetSelectedTexts(lstYear) + "</b>";
                string strSQL = "QLCS_BCTK_ChuyenLoaiCa";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@StrYear", Config.GetSelectedValues_At(lstYear));
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<div style='text-align:center;font-weight:bold;font-size:14pt;'>" + tieude + "</div><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><th>Giai đoạn</th><th>Số cá được chuyển</th><th>Số cá giống được chuyển</th><th>Số cá tăng trọng được chuyển</th></tr></thead><tbody>");
                int Total = 0;
                int TotalGiong = 0;
                for (int k = 0; k < ds.Tables.Count; k++)
                {
                    dt = ds.Tables[k];
                    string[] arrNam = Config.GetSelectedValue(lstYear, k);
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>Năm " + arrNam[1] + "</td><td></td><td></td><td></td></tr>");
                    int tong = 0;
                    int tonggiong = 0;
                    foreach (DataRow r in dt.Rows)
                    {
                        sb.Append("<tr><td style='text-align:left;'>" + r["LoaiCaFrom"].ToString() + " --> " + r["LoaiCaTo"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["SoLuong"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["SoLuongGiong"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(r["SoLuong"]) - Convert.ToInt32(r["SoLuongGiong"]), 0) + "</td></tr>");
                        tong += Convert.ToInt32(r["SoLuong"]);
                        tonggiong += Convert.ToInt32(r["SoLuongGiong"]);
                    }
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td style='text-align:center;font-weight:bold; vertical-align:middle;color:#FF0000;'>T.C</td><td style='text-align:right;'>" + Config.ToXVal2(tong, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tonggiong, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tong - tonggiong, 0) + "</td></tr>");
                    Total += tong;
                    TotalGiong += tonggiong;
                }
                sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td style='text-align:center;font-weight:bold; vertical-align:middle;color:#FF0000;'>Tổng cộng</td><td style='text-align:right;'>" + Config.ToXVal2(Total, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TotalGiong, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(Total - TotalGiong, 0) + "</td></tr>");
                sb.Append("</tbody></table>");
                lt.Text = sb.ToString();
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
                string filename = "ThongKeCaChuyenGiaiDoan.xls";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                
                string tieude = "";
                tieude += "<b>BẢNG THỐNG KÊ CÁ CHUYỂN GIAI ĐOẠN NĂM " + Config.GetSelectedTexts(lstYear) + "</b>";
                string strSQL = "QLCS_BCTK_ChuyenLoaiCa";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@StrYear", Config.GetSelectedValues_At(lstYear));
                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt;
                
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");
                sb.Append("<table border='1'><tr><td colspan='4'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/></td></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><td>Giai đoạn</td><td>Số cá được chuyển</td><td>Số cá giống được chuyển</td><td>Số cá tăng trọng được chuyển</td></tr>");
                int Total = 0;
                int TotalGiong = 0;
                for (int k = 0; k < ds.Tables.Count; k++)
                {
                    dt = ds.Tables[k];
                    string[] arrNam = Config.GetSelectedValue(lstYear, k);
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td>Năm " + arrNam[1] + "</td><td></td><td></td><td></td></tr>");
                    int tong = 0;
                    int tonggiong = 0;
                    foreach (DataRow r in dt.Rows)
                    {
                        sb.Append("<tr><td style='text-align:left;'>" + r["LoaiCaFrom"].ToString() + " --> " + r["LoaiCaTo"].ToString() + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["SoLuong"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(r["SoLuongGiong"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(r["SoLuong"]) - Convert.ToInt32(r["SoLuongGiong"]), 0) + "</td></tr>");
                        tong += Convert.ToInt32(r["SoLuong"]);
                        tonggiong += Convert.ToInt32(r["SoLuongGiong"]);
                    }
                    sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td style='text-align:center;font-weight:bold; vertical-align:middle;color:#FF0000;'>T.C</td><td style='text-align:right;'>" + Config.ToXVal2(tong, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tonggiong, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(tong - tonggiong, 0) + "</td></tr>");
                    Total += tong;
                    TotalGiong += tonggiong;
                }
                sb.Append("<tr style='font-weight:bold; vertical-align:middle;color:#FF0000;'><td style='text-align:center;font-weight:bold; vertical-align:middle;color:#FF0000;'>Tổng cộng</td><td style='text-align:right;'>" + Config.ToXVal2(Total, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(TotalGiong, 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(Total - TotalGiong, 0) + "</td></tr>");
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
    }
}