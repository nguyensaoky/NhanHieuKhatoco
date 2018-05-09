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
    public partial class r_tonghopdagietmo : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        int scale = 0;
        private void BindControls()
        {
            DataTable dtLoaiCa = csCont.LoadLoaiCa(1);
            ddlLoaiCa.DataSource = dtLoaiCa;
            ddlLoaiCa.DataTextField = "TenLoaiCa";
            ddlLoaiCa.DataValueField = "IDLoaiCa";
            ddlLoaiCa.DataBind();
            ddlLoaiCa.Items.Insert(0, new ListItem("", "0"));

            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable tblDaCBMoi = csCont.VatTu_GetDCS_Order_Type("DCS_CBM");
            ddlDaCBMoi.DataSource = tblDaCBMoi;
            ddlDaCBMoi.DataValueField = "IDVatTu";
            ddlDaCBMoi.DataTextField = "TenVatTu";
            ddlDaCBMoi.DataBind();

            DataTable tblDaCLMoi = csCont.VatTu_GetDCS_Order_Type("DCS_CLM");
            ddlDaCLMoi.DataSource = tblDaCLMoi;
            ddlDaCLMoi.DataValueField = "IDVatTu";
            ddlDaCLMoi.DataTextField = "TenVatTu";
            ddlDaCLMoi.DataBind();

            DataTable tblDaMDL = csCont.VatTu_GetDCS_Order_Type("DCS_MDL");
            ddlDaMDL.DataSource = tblDaMDL;
            ddlDaMDL.DataValueField = "IDVatTu";
            ddlDaMDL.DataTextField = "TenVatTu";
            ddlDaMDL.DataBind();

            DataTable tblDa = csCont.VatTu_GetDCS_Order_Type("");
            ddlDa.DataSource = tblDa;
            ddlDa.DataValueField = "IDVatTu";
            ddlDa.DataTextField = "TenVatTu";
            ddlDa.DataBind();

            DataTable tblDaCB = csCont.VatTu_GetDCS_Order_Type("CB");
            ddlDaCB.DataSource = tblDaCB;
            ddlDaCB.DataValueField = "IDVatTu";
            ddlDaCB.DataTextField = "TenVatTu";
            ddlDaCB.DataBind();

            DataTable tblDaCL = csCont.VatTu_GetDCS_Order_Type("CL");
            ddlDaCL.DataSource = tblDaCL;
            ddlDaCL.DataValueField = "IDVatTu";
            ddlDaCL.DataTextField = "TenVatTu";
            ddlDaCL.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
            try
            {
                scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_DCS_Scale"]);
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
            if (ddlHinhThuc.SelectedValue == "0")
                Excel_GietMoChet();
            else if (ddlHinhThuc.SelectedValue == "1")
                Excel_GietMo();
            else if (ddlHinhThuc.SelectedValue == "2")
                Excel_Chet();
        }

        protected void Excel_GietMoChet()
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string filename = "TongHopDaGietMoChet";
                string tieude = "";
                string strSQL = "QLCS_BCTK_TongHopDa_GietMoChet";
                tieude = "<b>BÁO CÁO DA THU HỒI TỪ GIẾT MỔ CÁ VÀ CÁ CHẾT TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + " - LOẠI CÁ " + ddlLoaiCa.SelectedItem.Text + "</b>";
                SqlParameter[] param = new SqlParameter[5];
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
                string sChuong = "";
                for (int i = 0; i < int.Parse(hdNumListChuong.Value); i++)
                {
                    sChuong += Config.GetSelectedValues_At((ListBox)(phChuong.FindControl("lstChuong" + i.ToString())));
                }
                param[2] = new SqlParameter("@sChuong", sChuong);
                string sLoaiDa = Config.GetSelectedValues_At(ddlDaCBMoi) + Config.GetSelectedValues_At(ddlDaCLMoi) + Config.GetSelectedValues_At(ddlDaMDL) + Config.GetSelectedValues_At(ddlDa) + Config.GetSelectedValues_At(ddlDaCB) + Config.GetSelectedValues_At(ddlDaCL);
                param[3] = new SqlParameter("@sLoaiDa", sLoaiDa);
                param[4] = new SqlParameter("@LoaiCa", int.Parse(ddlLoaiCa.SelectedValue));
                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                filename = filename.Replace("/", "_");

                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");
                sb.Append("<table border='1'><tr><td colspan='10'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/></td></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <td>Loại cá</td><td>Da thu hồi</td><td colspan='4'>Số lượng (tấm/cái)</td><td colspan='4'>Tỷ lệ (%)</td></tr>
                        <tr><td rowspan='2'></td><td rowspan='2'></td><td colspan='2'>Cá giết mổ</td><td rowspan='2'>Chết</td><td rowspan='2'>Cộng</td><td colspan='2'>Cá giết mổ</td><td rowspan='2'>Chết</td><td rowspan='2'>Cộng</td></tr>
                        <tr><td>Con TP</td><td>Con thải loại</td><td>Con TP</td><td>Con thải loại</td></tr>
                        ");

                string TenLoaiCa = "";
                int currNumVatTu = 0;
                int currSoLuongTong = 0;
                decimal currSoLuong_1 = 0;
                decimal currSoLuong_0 = 0;
                decimal currSoLuong_Chet = 0;
                int SoLuongTong = 0;
                int SoLoaiCa = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (r["TenLoaiCa"].ToString() != TenLoaiCa)
                    {
                        //them tong hop start
                        if (TenLoaiCa != "")
                        {
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>T.C</b></td><td align='right'><b>" + currNumVatTu.ToString() + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_1, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_0, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_Chet, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuongTong, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_1 / currSoLuongTong * 100, 2) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_0 / currSoLuongTong * 100, 2) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_Chet / currSoLuongTong * 100, 2) + "</b></td><td></td></tr>");
                            currNumVatTu = 0;
                            currSoLuong_1 = 0;
                            currSoLuong_0 = 0;
                            currSoLuong_Chet = 0;
                        }
                        //them tong hop end
                        TenLoaiCa = r["TenLoaiCa"].ToString();
                        decimal tong = Convert.ToDecimal(r["1"]) + Convert.ToDecimal(r["0"]) + Convert.ToDecimal(r["SoLuong"]); 
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_1 = Convert.ToDecimal(r["1"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_0 = Convert.ToDecimal(r["0"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_Chet = Convert.ToDecimal(r["SoLuong"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<tr><td style='vertical-align:middle;'>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["1"], scale) + @"</td><td align='right'>" + Config.ToXVal2(r["0"], scale) + @"</td><td align='right'>" + Config.ToXVal2(r["SoLuong"], scale) + @"</td><td align='right'>" + Config.ToXVal2(tong, scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_1, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_0, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_Chet, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl, 2) + "</td>");
                        sb.Append("</tr>");
                        currSoLuongTong = Convert.ToInt32(r["SoLuongTong"]);
                        SoLuongTong += currSoLuongTong;
                        SoLoaiCa++;
                    }
                    else
                    {
                        decimal tong = Convert.ToDecimal(r["1"]) + Convert.ToDecimal(r["0"]) + Convert.ToDecimal(r["SoLuong"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_1 = Convert.ToDecimal(r["1"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_0 = Convert.ToDecimal(r["0"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_Chet = Convert.ToDecimal(r["SoLuong"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<tr><td></td><td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["1"], scale) + @"</td><td align='right'>" + Config.ToXVal2(r["0"], scale) + @"</td><td align='right'>" + Config.ToXVal2(r["SoLuong"], scale) + @"</td><td align='right'>" + Config.ToXVal2(tong, scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_1, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_0, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_Chet, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl, 2) + "</td>");
                        sb.Append("</tr>");
                    }
                    currSoLuong_1 += Convert.ToDecimal(r["1"]);
                    currSoLuong_0 += Convert.ToDecimal(r["0"]);
                    currSoLuong_Chet += Convert.ToDecimal(r["SoLuong"]);
                    currNumVatTu++;
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>T.C</b></td><td align='right'><b>" + currNumVatTu.ToString() + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_1, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_0, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_Chet, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuongTong, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_1 / currSoLuongTong * 100, 2) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_0 / currSoLuongTong * 100, 2) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_Chet / currSoLuongTong * 100, 2) + "</b></td><td></td></tr>");

                decimal Tong_1 = 0;
                decimal Tong_0 = 0;
                decimal Tong_Chet = 0;
                if (ddlLoaiCa.SelectedValue == "0" && SoLoaiCa > 1)
                {
                    bool DaCoDongDau = false;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow r = dt1.Rows[i];
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>Theo từng loại da</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        Tong_1 += Convert.ToDecimal(r["1"]);
                        Tong_0 += Convert.ToDecimal(r["0"]);
                        Tong_Chet += Convert.ToDecimal(r["SoLuong"]);
                        decimal tong = Convert.ToDecimal(r["1"]) + Convert.ToDecimal(r["0"]) + Convert.ToDecimal(r["SoLuong"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_1 = Convert.ToDecimal(r["1"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_0 = Convert.ToDecimal(r["0"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_Chet = Convert.ToDecimal(r["SoLuong"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["1"], scale) + "</td><td align='right'>" + Config.ToXVal2(r["0"], scale) + "</td><td align='right'>" + Config.ToXVal2(r["SoLuong"], scale) + "</td><td align='right'>" + Config.ToXVal2(tong, scale) + "</td><td align='right'>" + Config.ToXVal2(tl_1, 2) + "</td><td align='right'>" + Config.ToXVal2(tl_0, 2) + "</td><td align='right'>" + Config.ToXVal2(tl_Chet, 2) + "</td><td align='right'>" + Config.ToXVal2(tl, 2) + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>Tổng cộng</b></td><td></td><td align='right'><b>" + Config.ToXVal2(Tong_1, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(Tong_0, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(Tong_Chet, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(SoLuongTong, scale) + @"</b></td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2((decimal)Tong_1 / SoLuongTong * 100, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2((decimal)Tong_0 / SoLuongTong * 100, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2((decimal)Tong_Chet / SoLuongTong * 100, 2) + @"</td><td></td></tr>");
                }
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

        protected void Excel_GietMo()
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string filename = "TongHopDaGietMo";
                string tieude = "";
                string strSQL = "QLCS_BCTK_TongHopDa_GietMo";
                tieude = "<b>BÁO CÁO DA THU HỒI TỪ GIẾT MỔ CÁ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + " - LOẠI CÁ " + ddlLoaiCa.SelectedItem.Text + "</b>";
                SqlParameter[] param = new SqlParameter[5];
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
                string sChuong = "";
                for (int i = 0; i < int.Parse(hdNumListChuong.Value); i++)
                {
                    sChuong += Config.GetSelectedValues_At((ListBox)(phChuong.FindControl("lstChuong" + i.ToString())));
                }
                param[2] = new SqlParameter("@sChuong", sChuong);
                string sLoaiDa = Config.GetSelectedValues_At(ddlDaCBMoi) + Config.GetSelectedValues_At(ddlDaCLMoi) + Config.GetSelectedValues_At(ddlDaMDL) + Config.GetSelectedValues_At(ddlDa) + Config.GetSelectedValues_At(ddlDaCB) + Config.GetSelectedValues_At(ddlDaCL);
                param[3] = new SqlParameter("@sLoaiDa", sLoaiDa);
                param[4] = new SqlParameter("@LoaiCa", int.Parse(ddlLoaiCa.SelectedValue));
                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                filename = filename.Replace("/", "_");

                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");
                sb.Append("<table border='1'><tr><td colspan='8'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/></td></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <td>Loại cá</td><td>Da thu hồi</td><td colspan='3'>Số lượng (tấm/cái)</td><td colspan='3'>Tỷ lệ (%)</td></tr>
                        <tr><td></td><td></td><td>Con TP</td><td>Con thải loại</td><td>Cộng</td><td>Con TP</td><td>Con thải loại</td><td>Cộng</td></tr>
                        ");

                string TenLoaiCa = "";
                int currNumVatTu = 0;
                int currSoLuongTong = 0;
                decimal currSoLuong_1 = 0;
                decimal currSoLuong_0 = 0;
                int SoLuongTong = 0;
                int SoLoaiCa = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (r["TenLoaiCa"].ToString() != TenLoaiCa)
                    {
                        //them tong hop start
                        if (TenLoaiCa != "")
                        {
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>T.C</b></td><td align='right'><b>" + currNumVatTu.ToString() + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_1, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_0, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuongTong, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_1 / currSoLuongTong * 100, 2) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_0 / currSoLuongTong * 100, 2) + "</b></td><td></td></tr>");
                            currNumVatTu = 0;
                            currSoLuong_1 = 0;
                            currSoLuong_0 = 0;
                        }
                        //them tong hop end
                        TenLoaiCa = r["TenLoaiCa"].ToString();
                        decimal tong = Convert.ToDecimal(r["1"]) + Convert.ToDecimal(r["0"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_1 = Convert.ToDecimal(r["1"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_0 = Convert.ToDecimal(r["0"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<tr><td style='vertical-align:middle;'>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["1"], scale) + @"</td><td align='right'>" + Config.ToXVal2(r["0"], scale) + @"</td><td align='right'>" + Config.ToXVal2(tong, scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_1, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_0, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl, 2) + "</td>");
                        sb.Append("</tr>");
                        currSoLuongTong = Convert.ToInt32(r["SoLuongTong"]);
                        SoLuongTong += currSoLuongTong;
                        SoLoaiCa++;
                    }
                    else
                    {
                        decimal tong = Convert.ToDecimal(r["1"]) + Convert.ToDecimal(r["0"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_1 = Convert.ToDecimal(r["1"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_0 = Convert.ToDecimal(r["0"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<tr><td></td><td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["1"], scale) + @"</td><td align='right'>" + Config.ToXVal2(r["0"], scale) + @"</td><td align='right'>" + Config.ToXVal2(tong, scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_1, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_0, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl, 2) + "</td>");
                        sb.Append("</tr>");
                    }
                    currSoLuong_1 += Convert.ToDecimal(r["1"]);
                    currSoLuong_0 += Convert.ToDecimal(r["0"]);
                    currNumVatTu++;
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>T.C</b></td><td align='right'><b>" + currNumVatTu.ToString() + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_1, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_0, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuongTong, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_1 / currSoLuongTong * 100, 2) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_0 / currSoLuongTong * 100, 2) + "</b></td><td></td></tr>");

                decimal Tong_1 = 0;
                decimal Tong_0 = 0;
                if (ddlLoaiCa.SelectedValue == "0" && SoLoaiCa > 1)
                {
                    bool DaCoDongDau = false;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow r = dt1.Rows[i];
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>Theo từng loại da</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        Tong_1 += Convert.ToDecimal(r["1"]);
                        Tong_0 += Convert.ToDecimal(r["0"]);
                        decimal tong = Convert.ToDecimal(r["1"]) + Convert.ToDecimal(r["0"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_1 = Convert.ToDecimal(r["1"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_0 = Convert.ToDecimal(r["0"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["1"], scale) + "</td><td align='right'>" + Config.ToXVal2(r["0"], scale) + "</td><td align='right'>" + Config.ToXVal2(tong, scale) + "</td><td align='right'>" + Config.ToXVal2(tl_1, 2) + "</td><td align='right'>" + Config.ToXVal2(tl_0, 2) + "</td><td align='right'>" + Config.ToXVal2(tl, 2) + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>Tổng cộng</b></td><td></td><td align='right'><b>" + Config.ToXVal2(Tong_1, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(Tong_0, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(SoLuongTong, scale) + @"</b></td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2((decimal)Tong_1 / SoLuongTong * 100, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2((decimal)Tong_0 / SoLuongTong * 100, 2) + @"</td><td></td></tr>");
                }
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

        protected void Excel_Chet()
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string filename = "TongHopDaChet";
                string tieude = "";
                string strSQL = "QLCS_BCTK_TongHopDa_Chet";
                tieude = "<b>BÁO CÁO DA THU HỒI TỪ CÁ CHẾT TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + " - LOẠI CÁ " + ddlLoaiCa.SelectedItem.Text + "</b>";
                SqlParameter[] param = new SqlParameter[5];
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
                string sChuong = "";
                for (int i = 0; i < int.Parse(hdNumListChuong.Value); i++)
                {
                    sChuong += Config.GetSelectedValues_At((ListBox)(phChuong.FindControl("lstChuong" + i.ToString())));
                }
                param[2] = new SqlParameter("@sChuong", sChuong);
                string sLoaiDa = Config.GetSelectedValues_At(ddlDaCBMoi) + Config.GetSelectedValues_At(ddlDaCLMoi) + Config.GetSelectedValues_At(ddlDaMDL) + Config.GetSelectedValues_At(ddlDa) + Config.GetSelectedValues_At(ddlDaCB) + Config.GetSelectedValues_At(ddlDaCL);
                param[3] = new SqlParameter("@sLoaiDa", sLoaiDa);
                param[4] = new SqlParameter("@LoaiCa", int.Parse(ddlLoaiCa.SelectedValue));
                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                filename = filename.Replace("/", "_");

                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");
                sb.Append("<table border='1'><tr><td colspan='4'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/></td></tr>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <td>Loại cá</td><td>Da thu hồi</td><td>Số lượng (tấm/cái)</td><td>Tỷ lệ (%)</td></tr>
                        ");

                string TenLoaiCa = "";
                int currNumVatTu = 0;
                int currSoLuongTong = 0;
                decimal currSoLuong_Chet = 0;
                int SoLuongTong = 0;
                int SoLoaiCa = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (r["TenLoaiCa"].ToString() != TenLoaiCa)
                    {
                        //them tong hop start
                        if (TenLoaiCa != "")
                        {
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>T.C</b></td><td align='right'><b>" + currNumVatTu.ToString() + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_Chet, scale) + "</b></td><td></td></tr>");
                            currNumVatTu = 0;
                            currSoLuong_Chet = 0;
                        }
                        //them tong hop end
                        TenLoaiCa = r["TenLoaiCa"].ToString();
                        decimal tong = Convert.ToDecimal(r["SoLuong"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_Chet = Convert.ToDecimal(r["SoLuong"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<tr><td style='vertical-align:middle;'>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["SoLuong"], scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_Chet, 2) + @"</td>");
                        sb.Append("</tr>");
                        currSoLuongTong = Convert.ToInt32(r["SoLuongTong"]);
                        SoLuongTong += currSoLuongTong;
                        SoLoaiCa++;
                    }
                    else
                    {
                        decimal tong = Convert.ToDecimal(r["SoLuong"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_Chet = Convert.ToDecimal(r["SoLuong"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<tr><td></td><td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["SoLuong"], scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_Chet, 2) + @"</td>");
                        sb.Append("</tr>");
                    }
                    currSoLuong_Chet += Convert.ToDecimal(r["SoLuong"]);
                    currNumVatTu++;
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>T.C</b></td><td align='right'><b>" + currNumVatTu.ToString() + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_Chet, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_Chet / currSoLuongTong * 100, 2) + "</b></td></tr>");

                decimal Tong_Chet = 0;
                if (ddlLoaiCa.SelectedValue == "0" && SoLoaiCa > 1)
                {
                    bool DaCoDongDau = false;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow r = dt1.Rows[i];
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>Theo từng loại da</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        Tong_Chet += Convert.ToDecimal(r["SoLuong"]);
                        decimal tong = Convert.ToDecimal(r["SoLuong"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_Chet = Convert.ToDecimal(r["SoLuong"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["SoLuong"], scale) + "</td><td align='right'>" + Config.ToXVal2(tl_Chet, 2) + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>Tổng cộng</b></td><td></td><td align='right'><b>" + Config.ToXVal2(Tong_Chet, scale) + @"</b></td><td></td></tr>");
                }
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
            if (ddlHinhThuc.SelectedValue == "0")
                View_GietMoChet();
            else if (ddlHinhThuc.SelectedValue == "1")
                View_GietMo();
            else if (ddlHinhThuc.SelectedValue == "2")
                View_Chet();
        }

        protected void View_GietMoChet()
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string tieude = "";
                string strSQL = "QLCS_BCTK_TongHopDa_GietMoChet";
                tieude = "<b>BÁO CÁO DA THU HỒI TỪ GIẾT MỔ CÁ VÀ CÁ CHẾT TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + " - LOẠI CÁ " + ddlLoaiCa.SelectedItem.Text + "</b>";
                SqlParameter[] param = new SqlParameter[5];
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
                string sChuong = "";
                for (int i = 0; i < int.Parse(hdNumListChuong.Value); i++)
                {
                    sChuong += Config.GetSelectedValues_At((ListBox)(phChuong.FindControl("lstChuong" + i.ToString())));
                }
                param[2] = new SqlParameter("@sChuong", sChuong);
                string sLoaiDa = Config.GetSelectedValues_At(ddlDaCBMoi) + Config.GetSelectedValues_At(ddlDaCLMoi) + Config.GetSelectedValues_At(ddlDaMDL) + Config.GetSelectedValues_At(ddlDa) + Config.GetSelectedValues_At(ddlDaCB) + Config.GetSelectedValues_At(ddlDaCL);
                param[3] = new SqlParameter("@sLoaiDa", sLoaiDa);
                param[4] = new SqlParameter("@LoaiCa", int.Parse(ddlLoaiCa.SelectedValue));

                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <th>Loại cá</th><th>Da thu hồi</th><th colspan='4'>Số lượng (tấm/cái)</th><th colspan='4'>Tỷ lệ (%)</th></tr>
                        <tr><th rowspan='2'></th><th rowspan='2'></th><th colspan='2'>Cá giết mổ</th><th rowspan='2'>Chết</th><th rowspan='2'>Cộng</th><th colspan='2'>Cá giết mổ</th><th rowspan='2'>Chết</th><th rowspan='2'>Cộng</th></tr>
                        <tr><th>Con TP</th><th>Con thải loại</th><th>Con TP</th><th>Con thải loại</th></tr></thead></tbody>
                        ");

                string TenLoaiCa = "";
                int currNumVatTu = 0;
                int currSoLuongTong = 0;
                decimal currSoLuong_1 = 0;
                decimal currSoLuong_0 = 0;
                decimal currSoLuong_Chet = 0;
                int SoLuongTong = 0;
                int SoLoaiCa = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (r["TenLoaiCa"].ToString() != TenLoaiCa)
                    {
                        //them tong hop start
                        if (TenLoaiCa != "")
                        {
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>T.C</b></td><td align='right'><b>" + currNumVatTu.ToString() + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_1, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_0, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_Chet, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuongTong, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_1 / currSoLuongTong * 100, 2) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_0 / currSoLuongTong * 100, 2) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_Chet / currSoLuongTong * 100, 2) + "</b></td><td></td></tr>");
                            currNumVatTu = 0;
                            currSoLuong_1 = 0;
                            currSoLuong_0 = 0;
                            currSoLuong_Chet = 0;
                        }
                        //them tong hop end
                        TenLoaiCa = r["TenLoaiCa"].ToString();
                        decimal tong = Convert.ToDecimal(r["1"]) + Convert.ToDecimal(r["0"]) + Convert.ToDecimal(r["SoLuong"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_1 = Convert.ToDecimal(r["1"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_0 = Convert.ToDecimal(r["0"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_Chet = Convert.ToDecimal(r["SoLuong"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<tr><td style='vertical-align:middle;'>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["1"], scale) + @"</td><td align='right'>" + Config.ToXVal2(r["0"], scale) + @"</td><td align='right'>" + Config.ToXVal2(r["SoLuong"], scale) + @"</td><td align='right'>" + Config.ToXVal2(tong, scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_1, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_0, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_Chet, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl, 2) + "</td>");
                        sb.Append("</tr>");
                        currSoLuongTong = Convert.ToInt32(r["SoLuongTong"]);
                        SoLuongTong += currSoLuongTong;
                        SoLoaiCa++;
                    }
                    else
                    {
                        decimal tong = Convert.ToDecimal(r["1"]) + Convert.ToDecimal(r["0"]) + Convert.ToDecimal(r["SoLuong"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_1 = Convert.ToDecimal(r["1"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_0 = Convert.ToDecimal(r["0"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_Chet = Convert.ToDecimal(r["SoLuong"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<tr><td></td><td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["1"], scale) + @"</td><td align='right'>" + Config.ToXVal2(r["0"], scale) + @"</td><td align='right'>" + Config.ToXVal2(r["SoLuong"], scale) + @"</td><td align='right'>" + Config.ToXVal2(tong, scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_1, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_0, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_Chet, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl, 2) + "</td>");
                        sb.Append("</tr>");
                    }
                    currSoLuong_1 += Convert.ToDecimal(r["1"]);
                    currSoLuong_0 += Convert.ToDecimal(r["0"]);
                    currSoLuong_Chet += Convert.ToDecimal(r["SoLuong"]);
                    currNumVatTu++;
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>T.C</b></td><td align='right'><b>" + currNumVatTu.ToString() + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_1, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_0, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_Chet, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuongTong, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_1 / currSoLuongTong * 100, 2) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_0 / currSoLuongTong * 100, 2) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_Chet / currSoLuongTong * 100, 2) + "</b></td><td></td></tr>");

                decimal Tong_1 = 0;
                decimal Tong_0 = 0;
                decimal Tong_Chet = 0;
                if (ddlLoaiCa.SelectedValue == "0" && SoLoaiCa > 1)
                {
                    bool DaCoDongDau = false;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow r = dt1.Rows[i];
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>Theo từng loại da</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        Tong_1 += Convert.ToDecimal(r["1"]);
                        Tong_0 += Convert.ToDecimal(r["0"]);
                        Tong_Chet += Convert.ToDecimal(r["SoLuong"]);
                        decimal tong = Convert.ToDecimal(r["1"]) + Convert.ToDecimal(r["0"]) + Convert.ToDecimal(r["SoLuong"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_1 = Convert.ToDecimal(r["1"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_0 = Convert.ToDecimal(r["0"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_Chet = Convert.ToDecimal(r["SoLuong"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["1"], scale) + "</td><td align='right'>" + Config.ToXVal2(r["0"], scale) + "</td><td align='right'>" + Config.ToXVal2(r["SoLuong"], scale) + "</td><td align='right'>" + Config.ToXVal2(tong, scale) + "</td><td align='right'>" + Config.ToXVal2(tl_1, 2) + "</td><td align='right'>" + Config.ToXVal2(tl_0, 2) + "</td><td align='right'>" + Config.ToXVal2(tl_Chet, 2) + "</td><td align='right'>" + Config.ToXVal2(tl, 2) + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>Tổng cộng</b></td><td></td><td align='right'><b>" + Config.ToXVal2(Tong_1, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(Tong_0, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(Tong_Chet, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(SoLuongTong, scale) + @"</b></td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2((decimal)Tong_1 / SoLuongTong * 100, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2((decimal)Tong_0 / SoLuongTong * 100, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2((decimal)Tong_Chet / SoLuongTong * 100, 2) + @"</td><td></td></tr>");
                }
                sb.Append("</tbody></table>");
                lt.Text = sb.ToString();
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void View_GietMo()
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string tieude = "";
                string strSQL = "QLCS_BCTK_TongHopDa_GietMo";
                tieude = "<b>BÁO CÁO DA THU HỒI TỪ GIẾT MỔ CÁ TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + " - LOẠI CÁ " + ddlLoaiCa.SelectedItem.Text + "</b>";
                SqlParameter[] param = new SqlParameter[5];
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
                string sChuong = "";
                for (int i = 0; i < int.Parse(hdNumListChuong.Value); i++)
                {
                    sChuong += Config.GetSelectedValues_At((ListBox)(phChuong.FindControl("lstChuong" + i.ToString())));
                }
                param[2] = new SqlParameter("@sChuong", sChuong);
                string sLoaiDa = Config.GetSelectedValues_At(ddlDaCBMoi) + Config.GetSelectedValues_At(ddlDaCLMoi) + Config.GetSelectedValues_At(ddlDaMDL) + Config.GetSelectedValues_At(ddlDa) + Config.GetSelectedValues_At(ddlDaCB) + Config.GetSelectedValues_At(ddlDaCL);
                param[3] = new SqlParameter("@sLoaiDa", sLoaiDa);
                param[4] = new SqlParameter("@LoaiCa", int.Parse(ddlLoaiCa.SelectedValue));

                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <th>Loại cá</th><th>Da thu hồi</th><th colspan='3'>Số lượng (tấm/cái)</th><th colspan='3'>Tỷ lệ (%)</th></tr>
                        <tr><th></th><th></th><th>Con TP</th><th>Con thải loại</th><th>Cộng</th><th>Con TP</th><th>Con thải loại</th><th>Cộng</th></tr></thead></tbody>
                        ");

                string TenLoaiCa = "";
                int currNumVatTu = 0;
                int currSoLuongTong = 0;
                decimal currSoLuong_1 = 0;
                decimal currSoLuong_0 = 0;
                int SoLuongTong = 0;
                int SoLoaiCa = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (r["TenLoaiCa"].ToString() != TenLoaiCa)
                    {
                        //them tong hop start
                        if (TenLoaiCa != "")
                        {
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>T.C</b></td><td align='right'><b>" + currNumVatTu.ToString() + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_1, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_0, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuongTong, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_1 / currSoLuongTong * 100, 2) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_0 / currSoLuongTong * 100, 2) + "</b></td><td></td></tr>");
                            currNumVatTu = 0;
                            currSoLuong_1 = 0;
                            currSoLuong_0 = 0;
                        }
                        //them tong hop end
                        TenLoaiCa = r["TenLoaiCa"].ToString();
                        decimal tong = Convert.ToDecimal(r["1"]) + Convert.ToDecimal(r["0"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_1 = Convert.ToDecimal(r["1"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_0 = Convert.ToDecimal(r["0"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<tr><td style='vertical-align:middle;'>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["1"], scale) + @"</td><td align='right'>" + Config.ToXVal2(r["0"], scale) + @"</td><td align='right'>" + Config.ToXVal2(tong, scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_1, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_0, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl, 2) + "</td>");
                        sb.Append("</tr>");
                        currSoLuongTong = Convert.ToInt32(r["SoLuongTong"]);
                        SoLuongTong += currSoLuongTong;
                        SoLoaiCa++;
                    }
                    else
                    {
                        decimal tong = Convert.ToDecimal(r["1"]) + Convert.ToDecimal(r["0"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_1 = Convert.ToDecimal(r["1"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_0 = Convert.ToDecimal(r["0"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<tr><td></td><td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["1"], scale) + @"</td><td align='right'>" + Config.ToXVal2(r["0"], scale) + @"</td><td align='right'>" + Config.ToXVal2(tong, scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_1, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_0, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl, 2) + "</td>");
                        sb.Append("</tr>");
                    }
                    currSoLuong_1 += Convert.ToDecimal(r["1"]);
                    currSoLuong_0 += Convert.ToDecimal(r["0"]);
                    currNumVatTu++;
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>T.C</b></td><td align='right'><b>" + currNumVatTu.ToString() + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_1, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_0, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuongTong, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_1 / currSoLuongTong * 100, 2) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_0 / currSoLuongTong * 100, 2) + "</b></td><td></td></tr>");

                decimal Tong_1 = 0;
                decimal Tong_0 = 0;
                if (ddlLoaiCa.SelectedValue == "0" && SoLoaiCa > 1)
                {
                    bool DaCoDongDau = false;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow r = dt1.Rows[i];
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>Theo từng loại da</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        Tong_1 += Convert.ToDecimal(r["1"]);
                        Tong_0 += Convert.ToDecimal(r["0"]);
                        decimal tong = Convert.ToDecimal(r["1"]) + Convert.ToDecimal(r["0"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_1 = Convert.ToDecimal(r["1"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_0 = Convert.ToDecimal(r["0"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["1"], scale) + "</td><td align='right'>" + Config.ToXVal2(r["0"], scale) + "</td><td align='right'>" + Config.ToXVal2(tong, scale) + "</td><td align='right'>" + Config.ToXVal2(tl_1, 2) + "</td><td align='right'>" + Config.ToXVal2(tl_0, 2) + "</td><td align='right'>" + Config.ToXVal2(tl, 2) + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>Tổng cộng</b></td><td></td><td align='right'><b>" + Config.ToXVal2(Tong_1, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(Tong_0, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2(SoLuongTong, scale) + @"</b></td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2((decimal)Tong_1 / SoLuongTong * 100, 2) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2((decimal)Tong_0 / SoLuongTong * 100, 2) + @"</td><td></td></tr>");
                }
                sb.Append("</tbody></table>");
                lt.Text = sb.ToString();
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void View_Chet()
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string tieude = "";
                string strSQL = "QLCS_BCTK_TongHopDa_Chet";
                tieude = "<b>BÁO CÁO DA THU HỒI TỪ CÁ CHẾT TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + " - LOẠI CÁ " + ddlLoaiCa.SelectedItem.Text + "</b>";
                SqlParameter[] param = new SqlParameter[5];
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
                string sChuong = "";
                for (int i = 0; i < int.Parse(hdNumListChuong.Value); i++)
                {
                    sChuong += Config.GetSelectedValues_At((ListBox)(phChuong.FindControl("lstChuong" + i.ToString())));
                }
                param[2] = new SqlParameter("@sChuong", sChuong);
                string sLoaiDa = Config.GetSelectedValues_At(ddlDaCBMoi) + Config.GetSelectedValues_At(ddlDaCLMoi) + Config.GetSelectedValues_At(ddlDaMDL) + Config.GetSelectedValues_At(ddlDa) + Config.GetSelectedValues_At(ddlDaCB) + Config.GetSelectedValues_At(ddlDaCL);
                param[3] = new SqlParameter("@sLoaiDa", sLoaiDa);
                param[4] = new SqlParameter("@LoaiCa", int.Parse(ddlLoaiCa.SelectedValue));

                DataSet ds = Config.SelectSPs(strSQL, param);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                        <th>Loại cá</th><th>Da thu hồi</th><th>Số lượng (tấm/cái)</th><th>Tỷ lệ (%)</th></tr></thead></tbody>
                        ");

                string TenLoaiCa = "";
                int currNumVatTu = 0;
                int currSoLuongTong = 0;
                decimal currSoLuong_Chet = 0;
                int SoLuongTong = 0;
                int SoLoaiCa = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (r["TenLoaiCa"].ToString() != TenLoaiCa)
                    {
                        //them tong hop start
                        if (TenLoaiCa != "")
                        {
                            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>T.C</b></td><td align='right'><b>" + currNumVatTu.ToString() + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_Chet, scale) + "</b></td><td></td></tr>");
                            currNumVatTu = 0;
                            currSoLuong_Chet = 0;
                        }
                        //them tong hop end
                        TenLoaiCa = r["TenLoaiCa"].ToString();
                        decimal tong = Convert.ToDecimal(r["SoLuong"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_Chet = Convert.ToDecimal(r["SoLuong"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<tr><td style='vertical-align:middle;'>" + r["TenLoaiCa"].ToString() + "</td><td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["SoLuong"], scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_Chet, 2) + @"</td>");
                        sb.Append("</tr>");
                        currSoLuongTong = Convert.ToInt32(r["SoLuongTong"]);
                        SoLuongTong += currSoLuongTong;
                        SoLoaiCa++;
                    }
                    else
                    {
                        decimal tong = Convert.ToDecimal(r["SoLuong"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_Chet = Convert.ToDecimal(r["SoLuong"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<tr><td></td><td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["SoLuong"], scale) + @"</td><td style='text-align:right;mso-number-format:""\#\,\#\#0\.00""'>" + Config.ToXVal2(tl_Chet, 2) + @"</td>");
                        sb.Append("</tr>");
                    }
                    currSoLuong_Chet += Convert.ToDecimal(r["SoLuong"]);
                    currNumVatTu++;
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>T.C</b></td><td align='right'><b>" + currNumVatTu.ToString() + "</b></td><td align='right'><b>" + Config.ToXVal2(currSoLuong_Chet, scale) + "</b></td><td align='right'><b>" + Config.ToXVal2((decimal)currSoLuong_Chet / currSoLuongTong * 100, 2) + "</b></td></tr>");

                decimal Tong_Chet = 0;
                if (ddlLoaiCa.SelectedValue == "0" && SoLoaiCa > 1)
                {
                    bool DaCoDongDau = false;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow r = dt1.Rows[i];
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>Theo từng loại da</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        Tong_Chet += Convert.ToDecimal(r["SoLuong"]);
                        decimal tong = Convert.ToDecimal(r["SoLuong"]);
                        decimal tl = tong / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        decimal tl_Chet = Convert.ToDecimal(r["SoLuong"]) / Convert.ToDecimal(r["SoLuongTong"]) * 100;
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(r["SoLuong"], scale) + "</td><td align='right'>" + Config.ToXVal2(tl_Chet, 2) + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td><b>Tổng cộng</b></td><td></td><td align='right'><b>" + Config.ToXVal2(Tong_Chet, scale) + @"</b></td><td></td></tr>");
                }
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