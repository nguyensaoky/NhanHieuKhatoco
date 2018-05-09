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
    public partial class r_xuatnhapspgmtheoloai : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string filename = "DCS";
                string tieude = "";
                string strSQL = "QLCS_BCTK_XuatNhapVatTu";
                SqlParameter[] param = new SqlParameter[3];
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                param[2] = new SqlParameter("@LoaiVatTu", "SPGM");
                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                filename = filename.Replace("/", "_");
                tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                string s = @"<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body style='text-align:center;font-family:Times New Roman;'><br/>
                    <div style='text-align:center;font-weight:bold;font-size:14pt;'>" + tieude + "</div><br/>";
                s += "<table border='1'>";
                s += @"<tr style='font-weight:bold; vertical-align:middle;'><td>STT</td><td width=200>Nội dung</td><td width=100>ĐVT</td><td width=100>Tồn ĐK</td><td width=100>Nhập</td><td width=100>Xuất</td><td width=100>Tồn CK</td><td width=200>Ghi chú</td></tr>";
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i - 1];
                    if (Convert.ToDecimal(r["DK"]) == 0 && Convert.ToDecimal(r["N"]) == 0 && Convert.ToDecimal(r["X"]) == 0 && Convert.ToDecimal(r["CK"]) == 0) continue;
                    s += "<tr style='vertical-align:middle;'>";
                    s += "<td>" + i.ToString() + "</td>";
                    s += "<td>" + r["TenVatTu"].ToString() + "</td>";
                    s += "<td>" + r["DonViTinh"].ToString() + "</td>";
                    s += "<td>" + Config.ToXVal1(r["DK"]) + "</td>";
                    s += "<td>" + Config.ToXVal(r["N"]) + "</td>";
                    s += "<td>" + Config.ToXVal(r["X"]) + "</td>";
                    s += "<td>" + Config.ToXVal1(r["CK"]) + "</td><td></td></tr>";
                }
                s += "</table>";
                s += "<br/><div style='text-align:right;font-style:italic;'>Ninh Ích, ngày&nbsp;&nbsp;&nbsp;&nbsp;tháng&nbsp;&nbsp;&nbsp;&nbsp;năm&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>";
                s += "<br/><div style='text-align:right;font-weight:bold;'>Lập bảng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>";
                s += "</body></html>";
                Response.Write(s);
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
                string strSQL = "QLCS_BCTK_XuatNhapVatTu";
                SqlParameter[] param = new SqlParameter[3];
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                param[2] = new SqlParameter("@LoaiVatTu", "SPGM");
                tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP SẢN PHẨM GIẾT MỔ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                string s = @"<div style='text-align:center;font-weight:bold;font-size:14pt;'>" + tieude + "</div><br/>";
                s += "<table border='1' class='mGrid'>";
                s += @"<tr style='font-weight:bold; vertical-align:middle;'><td>STT</td><td width=200>Nội dung</td><td width=100>ĐVT</td><td width=100>Tồn ĐK</td><td width=100>Nhập</td><td width=100>Xuất</td><td width=100>Tồn CK</td><td width=200>Ghi chú</td></tr>";
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i - 1];
                    if (Convert.ToDecimal(r["DK"]) == 0 && Convert.ToDecimal(r["N"]) == 0 && Convert.ToDecimal(r["X"]) == 0 && Convert.ToDecimal(r["CK"]) == 0) continue;
                    s += "<tr style='vertical-align:middle;'>";
                    s += "<td>" + i.ToString() + "</td>";
                    s += "<td>" + r["TenVatTu"].ToString() + "</td>";
                    s += "<td>" + r["DonViTinh"].ToString() + "</td>";
                    s += "<td>" + Config.ToXVal1(r["DK"]) + "</td>";
                    s += "<td>" + Config.ToXVal(r["N"]) + "</td>";
                    s += "<td>" + Config.ToXVal(r["X"]) + "</td>";
                    s += "<td>" + Config.ToXVal1(r["CK"]) + "</td><td></td></tr>";
                }
                s += "</table>";
                lt.Text = s;
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}