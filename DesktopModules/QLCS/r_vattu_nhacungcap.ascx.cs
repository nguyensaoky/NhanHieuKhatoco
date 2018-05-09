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
    public partial class r_vattu_nhacungcap : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        /*Must have for baocao coding template*/
        //DataTable dt = null;
        DataSet ds = null;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        /*End Must have for baocao coding template*/

        private void BindControls()
        {
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            int i = 2017;
            for (i = 2017; i <= DateTime.Now.Year; i++)
            {
                ddlNamFrom.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlNamTo.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ddlNamFrom.SelectedValue = i.ToString();
            ddlNamTo.SelectedValue = i.ToString();
            ddlThangTo.SelectedValue = DateTime.Now.Month.ToString();
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

        /*Must have for baocao coding template: fill data to dt and return title*/
        public string createDataAndTieuDe()
        {
            string tieude = "";
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
            tieude = "<b>BẢNG THEO DÕI NHẬP " + ddlLoaiVatTu.SelectedItem.Text.ToUpper() + " TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
            strSQL = "QLCS_BCTK_VatTuNhaCungCap";
            param = new SqlParameter[3];
            param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
            param[1] = new SqlParameter("@dTo", txtToDate.Text);
            param[2] = new SqlParameter("@LoaiVatTu", ddlLoaiVatTu.SelectedValue);
            ds = Config.SelectSPs(strSQL, param);
            return tieude;
        }

        /*Must have for baocao coding template: create table header (just the tr and th of the table header)*/
        public string createTableHeader()
        {
            if (ds.Tables.Count == 2)
            {
                DataTable tblVatTu = ds.Tables[0];
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                s.Append(@"<tr style='font-weight:bold; vertical-align:middle;'><th><b>Nhà cung cấp</b></th>");
                foreach (DataRow r in tblVatTu.Rows)
                {
                    s.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                }
                s.Append("<th><b>Tổng cộng</b></th></tr>");
                return s.ToString();
            }
            lblMessage.Text = "Không có nhập đối với vật tư được chọn!";
            return "";
        }

        /*Must have for baocao coding template: create content of the table (just the tr and td of the table)*/
        public void createContent(System.Text.StringBuilder sb)
        {
            if (ds.Tables.Count == 1)
            {
                return;
            }
            int countVatTu = ds.Tables[0].Rows.Count;
            decimal[] tongcot = new decimal[countVatTu];
            decimal Tong = 0;
            for (int k = 0; k < countVatTu; k++)
            {
                tongcot[k] = 0;
            }
            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                DataRow r = ds.Tables[1].Rows[i];
                decimal tonghang = 0;
                sb.Append("<tr>");
                sb.Append("<td align='left'>" + r["NCC"].ToString() + "</td>");
                for (int j = 0; j < countVatTu; j++)
			    {
                    decimal d = 0;
                    if (r[2 + j] != DBNull.Value) d = Convert.ToDecimal(r[2 + j]);
			        sb.Append("<td align='right'>" + Config.ToXVal2(d, 0) + "</td>");
                    tonghang += d;
                    tongcot[j] += d;
			    }
                sb.Append("<td align='right'>" + Config.ToXVal2(tonghang, 0) + "</td>");
                sb.Append("</tr>");
                Tong += tonghang;
            }
            //Tính tổng
            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>Tổng</td>");
            for (int i = 0; i < countVatTu; i++)
            {
                sb.Append("<td align='right'>" + Config.ToXVal2(tongcot[i], 0) + "</td>");
            }
            sb.Append("<td align='right'>" + Config.ToXVal2(Tong, 0) + "</td>");
            sb.Append("</tr>");
            lblMessage.Text = "";
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //Just call this
                Config.exportExcel(Response, sb, "VatTuNhaCungCap", createDataAndTieuDe, createContent, createTableHeader);
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
                //Just call this
                Config.exportView(Response, sb, createDataAndTieuDe, createContent, createTableHeader);
                lt.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        public string createDataAndTieuDeThang()
        {
            if (int.Parse(ddlThangFrom.SelectedValue) + (int.Parse(ddlNamFrom.SelectedValue) * 12) > int.Parse(ddlThangTo.SelectedValue) + (int.Parse(ddlNamTo.SelectedValue) * 12))
            {
                lblMessage.Text = "Thời gian chọn không hợp lý!";
                return "";
            }
            string tieude = "";
            string strSQL = "";
            SqlParameter[] param;
            tieude = "<b>BẢNG THEO DÕI NHẬP " + ddlLoaiVatTu.SelectedItem.Text.ToUpper() + " TỪ THÁNG " + ddlThangFrom.SelectedValue + " NĂM " + ddlNamFrom.SelectedValue + " ĐẾN THÁNG " + ddlThangTo.SelectedValue + " NĂM " + ddlNamTo.SelectedValue + "</b>";
            strSQL = "QLCS_BCTK_VatTuNhaCungCapThang";
            param = new SqlParameter[5];
            param[0] = new SqlParameter("@mFrom", int.Parse(ddlThangFrom.SelectedValue));
            param[1] = new SqlParameter("@yFrom", int.Parse(ddlNamFrom.SelectedValue));
            param[2] = new SqlParameter("@mTo", int.Parse(ddlThangTo.SelectedValue));
            param[3] = new SqlParameter("@yTo", int.Parse(ddlNamTo.SelectedValue));
            param[4] = new SqlParameter("@LoaiVatTu", ddlLoaiVatTu1.SelectedValue);
            ds = Config.SelectSPs(strSQL, param);
            return tieude;
        }

        protected void btnExcelThang_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = "VatTuNhaCungCapThang.xls";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                string tieude = createDataAndTieuDeThang();
                if(tieude == "") return;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
                //sb.Append("<table border='1'><tr><td><br/><span style='font-weight:bold;font-size:15pt;'>" + tieude + "</span><br/></td></tr></table>");

                System.Text.StringBuilder sTieuDe = new System.Text.StringBuilder();
                System.Text.StringBuilder sHeader = new System.Text.StringBuilder();
                System.Text.StringBuilder sNoiDung = new System.Text.StringBuilder();
                int countVatTu = 0;
                int l = 0;
                while (l < ds.Tables.Count)
			    {
			        DataTable dt = ds.Tables[l];
                    if(dt.Columns.Count == 2)
                    {
                        sTieuDe = new System.Text.StringBuilder("<table border='1' id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead><tr><th colspan='@x'><center><b>");
                        sTieuDe.Append("THÁNG " + dt.Rows[0]["Thang"].ToString() + "/" + dt.Rows[0]["Nam"].ToString() + "</b></center></th></tr>");
                    }
                    else if(dt.Columns.Count == 1)
                    {
                        if(dt.Rows.Count == 0)
                        {
                            sTieuDe.Replace("@x","3").Append("</table><br/>");
                            sb.Append(sTieuDe.ToString());
                        }
                        else
	                    {
                            countVatTu = dt.Rows.Count;
                            int socot = 2 + countVatTu;
                            sTieuDe.Replace("@x", socot.ToString());
                            //Header
                            sHeader = new System.Text.StringBuilder(@"<tr style='font-weight:bold; vertical-align:middle;'><th><b>Nhà cung cấp</b></th>");
                            foreach (DataRow r in dt.Rows)
                            {
                                sHeader.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                            }
                            sHeader.Append("<th><b>Tổng cộng</b></th></tr>");
                            sb.Append(sTieuDe.ToString()).Append(sHeader.ToString());
	                    }
                    } 
                    else
                    {
                        decimal[] tongcot = new decimal[countVatTu];
                        decimal Tong = 0;
                        for (int k = 0; k < countVatTu; k++)
                        {
                            tongcot[k] = 0;
                        }
                        sNoiDung = new System.Text.StringBuilder();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow r = dt.Rows[i];
                            decimal tonghang = 0;
                            sNoiDung.Append("<tr>");
                            sNoiDung.Append("<td align='left'>" + r["NCC"].ToString() + "</td>");
                            for (int j = 0; j < countVatTu; j++)
			                {
                                decimal d = 0;
                                if (r[2 + j] != DBNull.Value) d = Convert.ToDecimal(r[2 + j]);
			                    sNoiDung.Append("<td align='right'>" + Config.ToXVal2(d, 0) + "</td>");
                                tonghang += d;
                                tongcot[j] += d;
			                }
                            sNoiDung.Append("<td align='right'>" + Config.ToXVal2(tonghang, 0) + "</td>");
                            sNoiDung.Append("</tr>");
                            Tong += tonghang;
                        }
                        //Tính tổng
                        sNoiDung.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>Tổng</td>");
                        for (int i = 0; i < countVatTu; i++)
                        {
                            sNoiDung.Append("<td align='right'>" + Config.ToXVal2(tongcot[i], 0) + "</td>");
                        }
                        sNoiDung.Append("<td align='right'>" + Config.ToXVal2(Tong, 0) + "</td>");
                        sNoiDung.Append("</tr></table><br/>");
                        sb.Append(sNoiDung.ToString());
                    }
                    l++;
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
    }
}