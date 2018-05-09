using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace DotNetNuke.Modules.QLCS
{
    public partial class gietmoca_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtNgayGietMoFrom.Text = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
                    txtNgayGietMoTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    DataTable dtLoaiCa = csCont.LoadLoaiCa(1);
                    ddlLoaiCa.DataSource = dtLoaiCa;
                    ddlLoaiCa.DataTextField = "TenLoaiCa";
                    ddlLoaiCa.DataValueField = "IDLoaiCa";
                    ddlLoaiCa.DataBind();
                    ddlLoaiCa.Items.Insert(0, new ListItem("", "0"));

                    lnkAddGietMoCa.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "gietmoca_add", "mid/" + this.ModuleId.ToString());
                    if (Session["AutoDisplay_GMC"] != null && Convert.ToBoolean(Session["AutoDisplay_GMC"]))
                    {
                        txtNgayGietMoFrom.Text = Session["GMC_TuNgay"].ToString();
                        txtNgayGietMoTo.Text = Session["GMC_DenNgay"].ToString();
                        txtBienBan.Text = Session["GMC_BB"].ToString();
                        Config.SetSelectedValues(ddlLoaiCa, Session["GMC_LoaiCa"].ToString());
                        btnLoad_Click(null, null);
                    }
                }
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    lnkAddGietMoCa.Visible = false;
                }
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "KhoaCS"))
                {
                    btnKhoa.Visible = false;
                    btnMoKhoa.Visible = false;
                    grvDanhSach.Columns[5].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnKhoa_Click(object sender, EventArgs e)
        {
            string strGietMoCa = "";
            foreach (GridViewRow row in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                if (chkChon.Checked)
                {
                    strGietMoCa += "@" + chkChon.Value + "@";
                }
            }
            csCont.Lock_GietMoCa(strGietMoCa, true);
            btnLoad_Click(null, null);
        }

        protected void btnMoKhoa_Click(object sender, EventArgs e)
        {
            string strGietMoCa = "";
            foreach (GridViewRow row in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                if (chkChon.Checked)
                {
                    strGietMoCa += "@" + chkChon.Value + "@";
                }
            }
            csCont.Lock_GietMoCa(strGietMoCa, false);
            btnLoad_Click(null, null);
        }

        protected void grvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(e.Row.FindControl("chkChon"));
                chkChon.Value = r["ID"].ToString();
                Button btnXemThayDoi = (Button)(e.Row.FindControl("btnXemThayDoi"));
                btnXemThayDoi.CommandArgument = r["ID"].ToString();
                HyperLink lnkEdit = (HyperLink)(e.Row.FindControl("lnkEdit"));
                Button btnBBMoKham = (Button)(e.Row.FindControl("btnBBMoKham"));
                Button btnBBSPThuHoi = (Button)(e.Row.FindControl("btnBBSPThuHoi"));
                Label lblTrongLuongHoi = (Label)(e.Row.FindControl("lblTrongLuongHoi"));
                Label lblTrongLuongMocHam = (Label)(e.Row.FindControl("lblTrongLuongMocHam"));
                btnBBMoKham.CommandArgument = r["ID"].ToString();
                btnBBSPThuHoi.CommandArgument = r["ID"].ToString();
                lnkEdit.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "gietmoca_edit", "gmcid/" + r["ID"].ToString(), "mid/" + this.ModuleId.ToString());
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    lnkEdit.Text = "Xem thông tin";
                }
                else if (Convert.ToDateTime(r["NgayMo"]) < Config.NgayKhoaSo())
                {
                    lnkEdit.Text = "Xem thông tin";
                }
                if (r["TrongLuongHoi"] == DBNull.Value) lblTrongLuongHoi.Text = "0";
                //else lblTrongLuongHoi.Text = Convert.ToDecimal(r["TrongLuongHoi"]).ToString("0.0");
                else lblTrongLuongHoi.Text = Config.ToXVal2(r["TrongLuongHoi"],1);
                if (r["TrongLuongMocHam"] == DBNull.Value) lblTrongLuongMocHam.Text = "0"; 
                //else lblTrongLuongMocHam.Text = Convert.ToDecimal(r["TrongLuongMocHam"]).ToString("0.0");
                else lblTrongLuongMocHam.Text = Config.ToXVal2(r["TrongLuongMocHam"],1);
                Label lblLoaiCa = (Label)(e.Row.FindControl("lblLoaiCa"));
                if (r["LoaiCa"] != DBNull.Value && r["LoaiCa"].ToString() != "") lblLoaiCa.Text = r["LoaiCa"].ToString().Substring(1, r["LoaiCa"].ToString().Length - 2).Replace("@@", ", ");
                if (Convert.ToBoolean(r["Lock"]))
                {
                    e.Row.CssClass = "GrayRow";
                }
                else
                {
                    e.Row.CssClass = "NormalRow";
                }
                btnXemThayDoi.Attributes["style"] = "background-image:url('" + ModulePath + "images/log.gif');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;width:16px;";
                e.Row.Attributes["id"] = "row_" + btnXemThayDoi.ClientID;
                e.Row.Attributes["onclick"] = "setSelectedRow(this,'" + btnXemThayDoi.ClientID + "')";
            }
        }

        protected void grvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }

        protected void grvDanhSach_Sorting(object sender, GridViewSortEventArgs e)
        {
            Session["GMC_SortExpression"] = e.SortExpression;
            if (e.SortDirection == SortDirection.Ascending) Session["GMC_SortDirection"] = "ASC";
            else Session["GMC_SortDirection"] = "DESC";
        }

        private string LoadODSParameters()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture("vi-VN");
            string strWhereClause = "";
            strWhereClause += "1=1";
            if (txtNgayGietMoFrom.Text != "")
            {
                strWhereClause += " and gmc.NgayMo>='" + DateTime.Parse(txtNgayGietMoFrom.Text, ci).ToString("MM/dd/yyyy") + "'";
            }
            if (txtNgayGietMoTo.Text != "")
            {
                strWhereClause += " and gmc.NgayMo<'" + DateTime.Parse(txtNgayGietMoTo.Text, ci).AddDays(1).ToString("MM/dd/yyyy") + "'";
            }
            if (txtBienBan.Text != "")
            {
                strWhereClause += " and gmc.BienBan like '%" + txtBienBan.Text + "%'";
            }
            if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
            {
                strWhereClause += " and cs.LoaiCa in (" + Config.GetSelectedValues(ddlLoaiCa).Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 2) + ")";
            }
            return strWhereClause;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            Session["AutoDisplay_GMC"] = true;
            Session["GMC_TuNgay"] = txtNgayGietMoFrom.Text;
            Session["GMC_DenNgay"] = txtNgayGietMoTo.Text;
            Session["GMC_BB"] = txtBienBan.Text;
            Session["GMC_LoaiCa"] = Config.GetSelectedValues(ddlLoaiCa);

            grvDanhSach.DataSourceID = "odsDanhSach";
            grvDanhSach.PageSize = int.Parse(ddlPageSize.SelectedValue);
            string strWhereClause = LoadODSParameters();
            odsDanhSach.SelectParameters["WhereClause"].DefaultValue = strWhereClause;
            lblTongSo.Text = CaSauDataObject.CountGietMoCa(strWhereClause).ToString();

            if (Session["GMC_SortExpression"] != null && Session["GMC_SortDirection"] != null)
            {
                SortDirection sd = SortDirection.Descending;
                if (Session["GMC_SortDirection"].ToString() == "ASC") sd = SortDirection.Ascending;
                grvDanhSach.Sort(Session["GMC_SortExpression"].ToString(), sd);
            }
        }

        protected void btnBBMoKham_Click(object sender, EventArgs e)
        {
            int GMC = int.Parse(((Button)sender).CommandArgument);
            DataTable tblGMC = csCont.GietMoCa_GetByID(GMC);
            DataTable tblGMCCT = csCont.GietMoCa_GetChiTiet(GMC,1);
            string filename = "BB_MoKham_" + ((DateTime)tblGMC.Rows[0]["NgayMo"]).ToString("dd_MM_yyyy") + ".doc";
            string tieude = "<b>BIÊN BẢN MỔ KHÁM - GIẾT MỔ CÁ SẤU</b>";
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.ContentType = "application/msword";
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
            sb.Append("Hôm nay, vào lúc:......giờ, ngày " + ((DateTime)tblGMC.Rows[0]["NgayMo"]).ToString("dd/MM/yyyy") + ", tại Trại Nhà giết mổ Ninh Hòa, chúng tôi gồm có:<br/>");
            sb.Append("Ông/Bà:..................................................../Chức vụ:....................................................<br/>");
            sb.Append("Ông/Bà:..................................................../Chức vụ:....................................................<br/>");
            sb.Append("Ông/Bà:..................................................../Chức vụ:....................................................<br/>");
            sb.Append("Ông/Bà:..................................................../Chức vụ:....................................................<br/>");
            sb.Append("Ông/Bà:..................................................../Chức vụ:....................................................<br/>");
            sb.Append("Cùng tiến hành mổ khám cá sấu&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cá nuôi&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Giống&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tăng trọng, Số lượng: " + tblGMCCT.Rows.Count.ToString() + " con, Đàn: .......<br/>");
            sb.Append("<table cellpadding='5' border='1' style='width: 100%;border-collapse:collapse;'>");
            sb.Append("<tr style='text-align:center;font-weight:bold;background-color:#eee;'><td rowspan='2'>STT</td><td rowspan='2'>Mã số/Ô chuồng</td><td rowspan='2'>Giới tính</td><td rowspan='2'>Khối lượng hơi (Kg)</td><td rowspan='2'>Bệnh tích</td><td rowspan='2'>Kết luận/Đề nghị</td><td colspan='3'>Sản phẩm thu hồi</td></tr><tr style='font-weight:bold;background-color:#eee;'><td>Kích thước da (cm)</td><td>Thịt móc hàm(kg)</td><td>Khác</td></tr>");
            int i = 1;
            foreach (DataRow r in tblGMCCT.Rows)
            {
                sb.Append("<tr><td align='center'>" + i.ToString() + "</td><td>" + r["TenCa"].ToString() + "</td><td>");
                string GioiTinh = "CXĐ";
                if (r["GioiTinh"].ToString() == "1") GioiTinh = "Đực";
                else if (r["GioiTinh"].ToString() == "0") GioiTinh = "Cái";
                sb.Append(GioiTinh + "</td><td align='right'>" + Config.ToXVal2(r["TrongLuongHoi"], 1) + "</td><td></td><td></td><td align='right'>" + r["Da_Bung"].ToString() + "</td><td align='right'>" + Config.ToXVal2(r["TrongLuongMocHam"], 1) + "</td><td align='left'>" + r["PhuongPhapMo"].ToString() + "</td></tr>");
                i++;
            }
            sb.Append("</table><br/>Biên bản hoàn thành vào hồi......giờ cùng ngày, mọi người nhất trí ký vào biên bản<br/>Lãnh đạo trại&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Thống kê&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Thủ kho&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Người mổ khám&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Đại diện tổ chăn nuôi<br/>");
            sb.Append("</body></html>");
            Response.Write(sb.ToString());
        }

        protected void btnBBSPThuHoi_KhongCoQuyCach_Click(object sender, EventArgs e)
        {
            int GMC = int.Parse(((Button)sender).CommandArgument);
            DataTable tblGMC = csCont.GietMoCa_GetByID(GMC);
            DataTable tblGMCCT = csCont.GietMoCa_GetChiTiet(GMC,1);
            DataTable tblGMCSP = csCont.GietMoCa_GetSanPhamByID(GMC);
            string filename = "BB_SPThuHoi_" + ((DateTime)tblGMC.Rows[0]["NgayMo"]).ToString("dd_MM_yyyy") + ".doc";
            string tieude = "<b>SẢN PHẨM THU HỒI SAU GIẾT MỔ</b>";
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.ContentType = "application/msword";
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
            sb.Append("Hiện trạng các sản phẩm thu hồi:<br/><br/><b><u>A. Sản phẩm da:</u></b><br/><br/>");
            if (tblGMCCT.Rows.Count == 1)
            {
                sb.Append("<table cellpadding='5' border='1' style='width: 100%;border-collapse:collapse;'>");
                DataRow r = tblGMCCT.Rows[0];
                sb.Append("<tr style='text-align:center;font-weight:bold;background-color:#eee;'><td>STT</td><td>Phân loại da</td><td>Kích thước da (cm)</td><td>Da (CB/CL/MDL)</td></tr>");
                sb.Append("<tr><td align='center'>1</td><td align='center'>" + r["Da_PhanLoai"].ToString() + "</td><td align='right'>" + r["Da_Bung"].ToString() + "</td><td align='left'>" + r["PhuongPhapMo"].ToString() + "</td></tr><tr><td colspan='4'>Tổng cộng: 1 tấm</td></tr>");
                sb.Append("</table>");
            }
            else if (tblGMCCT.Rows.Count > 1)
            {
                sb.Append("<table cellpadding='5' border='1' style='width: 100%;border-collapse:collapse;'>");
                sb.Append("<tr style='text-align:center;font-weight:bold;background-color:#eee;'><td>STT</td><td>Phân loại da</td><td>Kích thước da (cm)</td><td>Da (CB/CL/MDL)</td><td>STT</td><td>Phân loại da</td><td>Kích thước da (cm)</td><td>Da (CB/CL/MDL)</td></tr>");
                int GioiHanGiua = 0;
                if (tblGMCCT.Rows.Count % 2 == 1) GioiHanGiua = tblGMCCT.Rows.Count / 2 + 1;
                else GioiHanGiua = tblGMCCT.Rows.Count / 2;
                for (int i = 1; i <= GioiHanGiua; i++)
                {
                    DataRow r = tblGMCCT.Rows[i-1];
                    sb.Append("<tr><td align='center'>" + i.ToString() + "</td><td align='center'>" + r["Da_PhanLoai"].ToString() + "</td><td align='right'>" + r["Da_Bung"].ToString() + "</td><td align='left'>" + r["PhuongPhapMo"].ToString() + "</td>");
                    int col2Index = i + GioiHanGiua;
                    if (col2Index <= tblGMCCT.Rows.Count)
                    {
                        DataRow r1 = tblGMCCT.Rows[col2Index-1];
                        sb.Append("<td align='center'>" + col2Index.ToString() + "</td><td align='center'>" + r1["Da_PhanLoai"].ToString() + "</td><td align='right'>" + r1["Da_Bung"].ToString() + "</td><td align='left'>" + r["PhuongPhapMo"].ToString() + "</td></tr>");
                    }
                    else
                    {
                        sb.Append("<td></td><td></td><td></td><td></td></tr>");
                    }
                }
                sb.Append("<tr><td colspan='8'>Tổng cộng: " + tblGMCCT.Rows.Count.ToString() + " tấm</td></tr>");
                sb.Append("</table>");
            }

            sb.Append("<br/><br/><b><u>B. Sản phẩm thịt thu hồi:</u></b><br/><br/>");

            //Cách hiển thị cũ
            //if (tblGMCSP.Rows.Count > 0)
            //{
            //    s += "<table cellpadding='5' border='1' style='width: 100%;border-collapse:collapse;'>";
            //    s += "<tr style='text-align:center;font-weight:bold;background-color:#eee;'><td rowspan='2'>Tên thành phẩm</td><td colspan='4'>Quy cách/số lượng (gói)</td><td rowspan='2'>Tổng cộng</td><td rowspan='2'>Ghi chú</td></tr><tr style='text-align:center;font-weight:bold;background-color:#eee;'><td>01 kg</td><td>0,5 kg</td><td>0,2kg</td><td></td></tr>";
            //    decimal tong = 0;
            //    foreach (DataRow r in tblGMCSP.Rows)
            //    {
            //        s += "<tr><td>" + r["TenVatTu"].ToString() + "</td><td></td><td></td><td></td><td></td><td>" + Config.ToXVal1(r["KhoiLuong"]) + "</td><td></td></tr>";
            //        if(r["KhoiLuong"] != DBNull.Value)
            //            tong += Convert.ToDecimal(r["KhoiLuong"]);
            //    }
            //    s += "<tr><td>Tổng cộng</td><td></td><td></td><td></td><td></td><td>" + Config.ToXVal1(tong) + "</td><td></td></tr>";
            //    s += "</table>";
            //}

            //Cách hiển thị mới
            if (tblGMCSP.Rows.Count > 0)
            {
                sb.Append("<table cellpadding='5' border='1' style='width: 100%;border-collapse:collapse;'>");
                sb.Append("<tr style='text-align:center;font-weight:bold;background-color:#eee;'><td>Tên thành phẩm</td><td>Tổng cộng</td><td>Ghi chú</td></tr>");
                decimal tong = 0;
                foreach (DataRow r in tblGMCSP.Rows)
                {
                    sb.Append("<tr><td>" + r["TenVatTu"].ToString() + "</td><td align='right'>" + Config.ToXVal2(r["KhoiLuong"],1) + "</td><td></td></tr>");
                    if (r["KhoiLuong"] != DBNull.Value)
                        tong += Convert.ToDecimal(r["KhoiLuong"]);
                }
                sb.Append("<tr><td>Tổng cộng</td><td align='right'>" + Config.ToXVal2(tong,1) + "</td><td></td></tr>");
                sb.Append("</table>");
            }
            sb.Append("<br/>Lãnh đạo trại&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Thống kê&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Thủ kho&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Đại diện tổ giết mổ<br/>");
            sb.Append("</body></html>");
            Response.Write(sb.ToString());
        }

        protected void btnBBSPThuHoi_Click(object sender, EventArgs e)
        {
            int GMC = int.Parse(((Button)sender).CommandArgument);
            DataTable tblGMC = csCont.GietMoCa_GetByID(GMC);
            DataTable tblGMCCT = csCont.GietMoCa_GetChiTiet(GMC, 1);
            DataTable tblGMCSP = csCont.GietMoCa_GetSanPhamByID(GMC);
            string filename = "BB_SPThuHoi_" + ((DateTime)tblGMC.Rows[0]["NgayMo"]).ToString("dd_MM_yyyy") + ".doc";
            string tieude = "<b>SẢN PHẨM THU HỒI SAU GIẾT MỔ</b>";
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.ContentType = "application/msword";
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
            sb.Append("Hiện trạng các sản phẩm thu hồi:<br/><br/><b><u>A. Sản phẩm da:</u></b><br/><br/>");
            if (tblGMCCT.Rows.Count == 1)
            {
                sb.Append("<table cellpadding='5' border='1' style='width: 100%;border-collapse:collapse;'>");
                DataRow r = tblGMCCT.Rows[0];
                sb.Append("<tr style='text-align:center;font-weight:bold;background-color:#eee;'><td>STT</td><td>Phân loại da</td><td>Kích thước da (cm)</td><td>Da (CB/CL/MDL)</td></tr>");
                sb.Append("<tr><td align='center'>1</td><td align='center'>" + r["Da_PhanLoai"].ToString() + "</td><td align='right'>" + r["Da_Bung"].ToString() + "</td><td align='left'>" + r["PhuongPhapMo"].ToString() + "</td></tr><tr><td colspan='4'>Tổng cộng: 1 tấm</td></tr>");
                sb.Append("</table>");
            }
            else if (tblGMCCT.Rows.Count > 1)
            {
                sb.Append("<table cellpadding='5' border='1' style='width: 100%;border-collapse:collapse;'>");
                sb.Append("<tr style='text-align:center;font-weight:bold;background-color:#eee;'><td>STT</td><td>Phân loại da</td><td>Kích thước da (cm)</td><td>Da (CB/CL/MDL)</td><td>STT</td><td>Phân loại da</td><td>Kích thước da (cm)</td><td>Da (CB/CL/MDL)</td></tr>");
                int GioiHanGiua = 0;
                if (tblGMCCT.Rows.Count % 2 == 1) GioiHanGiua = tblGMCCT.Rows.Count / 2 + 1;
                else GioiHanGiua = tblGMCCT.Rows.Count / 2;
                for (int i = 1; i <= GioiHanGiua; i++)
                {
                    DataRow r = tblGMCCT.Rows[i - 1];
                    sb.Append("<tr><td align='center'>" + i.ToString() + "</td><td align='center'>" + r["Da_PhanLoai"].ToString() + "</td><td align='right'>" + r["Da_Bung"].ToString() + "</td><td align='left'>" + r["PhuongPhapMo"].ToString() + "</td>");
                    int col2Index = i + GioiHanGiua;
                    if (col2Index <= tblGMCCT.Rows.Count)
                    {
                        DataRow r1 = tblGMCCT.Rows[col2Index - 1];
                        sb.Append("<td align='center'>" + col2Index.ToString() + "</td><td align='center'>" + r1["Da_PhanLoai"].ToString() + "</td><td align='right'>" + r1["Da_Bung"].ToString() + "</td><td align='left'>" + r["PhuongPhapMo"].ToString() + "</td></tr>");
                    }
                    else
                    {
                        sb.Append("<td></td><td></td><td></td><td></td></tr>");
                    }
                }
                sb.Append("<tr><td colspan='8'>Tổng cộng: " + tblGMCCT.Rows.Count.ToString() + " tấm</td></tr>");
                sb.Append("</table>");
            }

            sb.Append("<br/><br/><b><u>B. Sản phẩm thịt thu hồi:</u></b><br/><br/>");

            if (tblGMCSP.Rows.Count > 0)
            {
                sb.Append("<table cellpadding='5' border='1' style='width: 100%;border-collapse:collapse;'>");
                sb.Append("<tr style='text-align:center;font-weight:bold;background-color:#eee;'><td rowspan='2'>Tên thành phẩm</td><td colspan='4'>Quy cách/số lượng (gói)</td><td rowspan='2'>Tổng cộng</td><td rowspan='2'>Ghi chú</td></tr><tr style='text-align:center;font-weight:bold;background-color:#eee;'><td>01 kg</td><td>0,5 kg</td><td>0,2kg</td><td></td></tr>");
                sb.Append("<tr><td>Thịt Fillet</td><td style='text-align:right;'>@351_1@</td><td style='text-align:right;'>@352_2@</td><td style='text-align:right;'>@353_3@</td><td></td><td style='text-align:right;'>$351,352,353.41$</td><td></td></tr>");
                sb.Append("<tr><td>Thịt X</td><td style='text-align:right;'>@354_1@</td><td style='text-align:right;'>@355_2@</td><td style='text-align:right;'></td><td></td><td style='text-align:right;'>$354,355.42$</td><td></td></tr>");
                sb.Append("<tr><td>Tay</td><td style='text-align:right;'>@356_1@</td><td style='text-align:right;'>@357_2@</td><td style='text-align:right;'></td><td></td><td style='text-align:right;'>$356,357.43$</td><td></td></tr>");
                sb.Append("<tr><td>Lòng</td><td style='text-align:right;'>@358_1@</td><td style='text-align:right;'>@359_2@</td><td style='text-align:right;'></td><td></td><td style='text-align:right;'>$358,359.44$</td><td></td></tr>");
                sb.Append("<tr><td>Lưỡi</td><td style='text-align:right;'>@360_1@</td><td style='text-align:right;'>@361_2@</td><td style='text-align:right;'></td><td></td><td style='text-align:right;'>$360,361.45$</td><td></td></tr>");
                sb.Append("<tr><td>Pín</td><td style='text-align:right;'></td><td style='text-align:right;'></td><td style='text-align:right;'></td><td></td><td style='text-align:right;'>$46$</td><td></td></tr>");
                sb.Append("<tr><td>Mỡ</td><td style='text-align:right;'></td><td style='text-align:right;'></td><td style='text-align:right;'></td><td></td><td style='text-align:right;'>$47$</td><td></td></tr>");
                sb.Append("<tr><td>Xương</td><td style='text-align:right;'></td><td style='text-align:right;'></td><td style='text-align:right;'></td><td></td><td style='text-align:right;'>$48$</td><td></td></tr>");
                sb.Append("<tr><td>Da</td><td style='text-align:right;'></td><td style='text-align:right;'></td><td style='text-align:right;'></td><td></td><td style='text-align:right;'>$120$</td><td></td></tr>");
                sb.Append("<tr><td>Sườn</td><td style='text-align:right;'></td><td style='text-align:right;'></td><td style='text-align:right;'></td><td></td><td style='text-align:right;'>$86$</td><td></td></tr>");
                sb.Append("<tr><td>Khớp</td><td style='text-align:right;'></td><td style='text-align:right;'></td><td style='text-align:right;'></td><td></td><td style='text-align:right;'>$87$</td><td></td></tr>");
                sb.Append("<tr><td>Đuôi</td><td style='text-align:right;'></td><td style='text-align:right;'></td><td style='text-align:right;'></td><td></td><td style='text-align:right;'>$88$</td><td></td></tr>");
                sb.Append("<tr><td>Tổng cộng</td><td style='text-align:right;'>@351_1,354_1,356_1,358_1,360_1@</td><td style='text-align:right;'>@352_2,355_2,357_2,359_2,361_2@</td><td style='text-align:right;'>@353_3@</td><td></td><td style='text-align:right;'>Tong</td><td></td></tr>");
                decimal tong = 0;
                
                string ss = (string)(sb.ToString().Clone());
                int i = ss.IndexOf("$");
                int ii = 0;
                string strID = "";
                while (i > 0)
                {
                    ss = ss.Substring(i + 1);
                    ii = ss.IndexOf("$");
                    strID = ss.Substring(0, ii);
                    decimal res = GetKhoiLuongOr(tblGMCSP, strID);
                    sb = sb.Replace("$" + strID + "$", Config.ToXVal2(res, 1));
                    ss = ss.Substring(ii + 1);
                    i = ss.IndexOf("$");
                }

                ss = (string)(sb.ToString().Clone());
                i = ss.IndexOf("@");
                ii = 0;
                strID = "";
                while (i > 0)
                {
                    ss = ss.Substring(i + 1);
                    ii = ss.IndexOf("@");
                    strID = ss.Substring(0, ii);
                    decimal res = GetKhoiLuongAnd_1(tblGMCSP, strID);
                    sb = sb.Replace("@" + strID + "@", Config.ToXVal2(res, 0));
                    ss = ss.Substring(ii + 1);
                    i = ss.IndexOf("@");
                }

                foreach (DataRow r in tblGMCSP.Rows)
                {
                    if (r["KhoiLuong"] != DBNull.Value)
                        tong += Convert.ToDecimal(r["KhoiLuong"]);
                }
                sb = sb.Replace("Tong", Config.ToXVal2(tong, 1));
                sb.Append("</table>");
            }

            sb.Append("<br/>Lãnh đạo trại&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Thống kê&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Thủ kho&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Đại diện tổ giết mổ<br/>");
            sb.Append("</body></html>");
            Response.Write(sb.ToString());
        }

        private decimal GetKhoiLuong(DataTable dt, string ID)
        {
            decimal res = 0;
            foreach (DataRow r in dt.Rows)
	        {
                if (r["IDVatTu"].ToString() == ID)
                {
                    if (r["KhoiLuong"] != DBNull.Value)
                        res = Convert.ToDecimal(r["KhoiLuong"]);
                    break;
                }
	        }
            return res;
        }

        private decimal GetKhoiLuong_1(DataTable dt, string ID)
        {
            decimal res = 0;
            string[] part = ID.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries); 
            foreach (DataRow r in dt.Rows)
            {
                if (r["IDVatTu"].ToString() == part[0])
                {
                    if (r["KhoiLuong"] != DBNull.Value)
                        res = Convert.ToDecimal(r["KhoiLuong"]) * int.Parse(part[1]);
                    break;
                }
            }
            return res;
        }

        private decimal GetKhoiLuongAnd(DataTable dt, string strID)
        {
            decimal res = 0;
            string[] arrID = strID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ID in arrID)
            {
                res += GetKhoiLuong(dt, ID);
            }
            return res;
        }

        private decimal GetKhoiLuongOr(DataTable dt, string strID)
        {
            decimal res = 0;
            string[] arrID = strID.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ID in arrID)
            {
                if (GetKhoiLuongAnd(dt, ID) > res) res = GetKhoiLuongAnd(dt, ID);
            }
            return res;
        }

        private decimal GetKhoiLuongAnd_1(DataTable dt, string strID)
        {
            decimal res = 0;
            string[] arrID = strID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ID in arrID)
            {
                res += GetKhoiLuong_1(dt, ID);
            }
            return res;
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnXemThayDoi_Click(object sender, EventArgs e)
        {
            DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
            int GietMoCaLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_GietMoCaLichSuPage"], PortalId).TabID;
            Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(GietMoCaLichSuPage, "", "IDGietMoCa/" + ((Button)sender).CommandArgument) + "','',1000,600);</script>", false);
        }
    }
}