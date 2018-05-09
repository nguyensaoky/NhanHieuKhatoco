using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DotNetNuke.Modules.QLCS
{
    public partial class casaude_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    DataTable dtLoaiCa = csCont.LoadLoaiCaLon();
                    ddlLoaiCa.DataSource = dtLoaiCa;
                    ddlLoaiCa.DataTextField = "TenLoaiCa";
                    ddlLoaiCa.DataValueField = "IDLoaiCa";
                    ddlLoaiCa.DataBind();
                    ddlLoaiCa.Items.Insert(0, new ListItem("", "0"));

                    DataTable dtChuong = csCont.LoadChuongByTenChuong("@KSS@@TP@");
                    ddlChuong.DataSource = dtChuong;
                    ddlChuong.DataTextField = "Chuong";
                    ddlChuong.DataValueField = "IDChuong";
                    ddlChuong.DataBind();
                    ddlChuong.Items.Insert(0, new ListItem("", "0"));

                    DataTable dtCaMe = csCont.LoadCaSauMe_AllTrangThai();
                    ddlCaMe.DataSource = dtCaMe;
                    ddlCaMe.DataValueField = "IDCaSau";
                    ddlCaMe.DataTextField = "CaMe";
                    ddlCaMe.DataBind();
                    ddlCaMe.Items.Insert(0, new ListItem("", "0"));

                    DataTable dtPhongAp = csCont.LoadPhongAp(1);
                    ddlPhongAp.DataSource = dtPhongAp;
                    ddlPhongAp.DataValueField = "IDPhongAp";
                    ddlPhongAp.DataTextField = "TenPhongAp";
                    ddlPhongAp.DataBind();
                    ddlPhongAp.Items.Insert(0, new ListItem("", "0"));

                    DataTable dtNhanVien = csCont.LoadNhanVien(1);
                    ddlNhanVien.DataSource = dtNhanVien;
                    ddlNhanVien.DataTextField = "TenNhanVien";
                    ddlNhanVien.DataValueField = "IDNhanVien";
                    ddlNhanVien.DataBind();
                    ddlNhanVien.Items.Insert(0, new ListItem("", "0"));

                    lnkAddCaSauDe.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "casaude_edit", "mid/" + this.ModuleId.ToString());

                    if (Session["AutoDisplay_CSD"] != null && Convert.ToBoolean(Session["AutoDisplay_CSD"]))
                    {
                        txtNgayApTo.Text = Session["CSDe_NgayApDen"].ToString();
                        txtNgayApFrom.Text = Session["CSDe_NgayApTu"].ToString();
                        ddlRowStatus.SelectedValue = Session["CSDe_RowStatus"].ToString();
                        ddlStatus.SelectedValue = Session["CSDe_TrangThai"].ToString();
                        Config.SetSelectedValues(ddlLoaiCa, Session["CSDe_LoaiCa"].ToString());
                        Config.SetSelectedValues(ddlChuong, Session["CSDe_Chuong"].ToString());
                        Config.SetSelectedValues(ddlPhongAp, Session["CSDe_PhongAp"].ToString());
                        Config.SetSelectedValues(ddlNhanVien, Session["CSDe_NhanVien"].ToString());
                        btnLoad_Click(null, null);
                    }
                }
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    lnkAddCaSauDe.Visible = false;
                }
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "KhoaCS"))
                {
                    btnKhoa.Visible = false;
                    btnMoKhoa.Visible = false;
                }
                else
                {
                    btnKhoa.Visible = true;
                    btnMoKhoa.Visible = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnKhoa_Click(object sender, EventArgs e)
        {
            string strTheoDoiDe = "";
            foreach (GridViewRow row in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                if (chkChon.Checked)
                {
                    strTheoDoiDe += "@" + chkChon.Value + "@";
                }
            }
            csCont.Lock_TheoDoiDe(strTheoDoiDe, true);
            btnLoad_Click(null, null);
        }

        protected void btnMoKhoa_Click(object sender, EventArgs e)
        {
            string strTheoDoiDe = "";
            foreach (GridViewRow row in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                if (chkChon.Checked)
                {
                    strTheoDoiDe += "@" + chkChon.Value + "@";
                }
            }
            csCont.Lock_TheoDoiDe(strTheoDoiDe, false);
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
                btnXemThayDoi.CommandName = ddlRowStatus.SelectedValue;
                Label lblCaMe = (Label)(e.Row.FindControl("lblCaMe"));
                lblCaMe.Text = Server.HtmlDecode(r["CaMe1"].ToString());
                HyperLink lnkCaMe = (HyperLink)(e.Row.FindControl("lnkCaMe"));
                Label lblSLNo = (Label)(e.Row.FindControl("lblSLNo"));
                Label lblStatus = (Label)(e.Row.FindControl("lblStatus"));
                Label lblSoNgayAp = (Label)(e.Row.FindControl("lblSoNgayAp"));
                Button btnDelete = (Button)(e.Row.FindControl("btnDelete"));
                Label lblChieuDaiBQ = (Label)(e.Row.FindControl("lblChieuDaiBQ"));
                Label lblVongBungBQ = (Label)(e.Row.FindControl("lblVongBungBQ"));
                Label lblNhanVien = (Label)(e.Row.FindControl("lblNhanVien"));
                lnkCaMe.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "casaude_edit", "tddid/" + r["ID"].ToString(), "mid/" + this.ModuleId.ToString());
                if (r["NgayNo"] != DBNull.Value)
                {
                    lblSLNo.Text = r["TrungNo"].ToString();
                    if (r["NgayNo"] != DBNull.Value && Convert.ToDateTime(r["NgayNo"]) < Config.NgayKhoaSo())
                    {
                        //btnDelete.Visible = false;
                        btnDelete.Enabled = false;
                        btnDelete.CssClass = "buttondisable";
                    }
                    TimeSpan ts = Convert.ToDateTime(r["NgayNo"]).Subtract(Convert.ToDateTime(r["NgayVaoAp"]));
                    lblSoNgayAp.Text = ts.Days.ToString();
                }
                else
                {
                    lblSLNo.Text = "0";
                }
                btnDelete.CommandArgument = r["ID"].ToString();
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    btnDelete.Visible = false;
                }
                if (Convert.ToBoolean(r["Lock"])) btnDelete.Visible = false;
                if (r["Status"].ToString() == "1")
                {
                    lblStatus.Text = "Đã nở";
                }
                else if (r["Status"].ToString() == "0")
                {
                    if (r["TrungNo"] == DBNull.Value || Convert.ToInt32(r["TrungNo"]) == 0)
                        lblStatus.Text = "Không nở";
                    else
                        lblStatus.Text = "Chưa nở";
                }
                //else if (r["Status"].ToString() == "-1")
                //{
                //    lblStatus.Text = "Không ấp";
                //}
                if (r["ChieuDaiBQ"] != DBNull.Value) lblChieuDaiBQ.Text = Convert.ToDecimal(r["ChieuDaiBQ"]).ToString("0.#####");
                if (r["VongBungBQ"] != DBNull.Value) lblVongBungBQ.Text = Convert.ToDecimal(r["VongBungBQ"]).ToString("0.#####");
                if (r["NhanVien"] != DBNull.Value && r["NhanVien"].ToString() != "") lblNhanVien.Text = r["NhanVien"].ToString().Substring(1, r["NhanVien"].ToString().Length - 2).Replace("@@", ", ");
                
                if (Convert.ToBoolean(r["Lock"]))
                {
                    e.Row.CssClass = "GrayRow";
                    btnDelete.Visible = false;
                }
                else
                {
                    e.Row.CssClass = "NormalRow";
                }

                if (ddlRowStatus.SelectedValue == "0")
                {
                    btnDelete.Visible = false;
                    lnkCaMe.Visible = false;
                }
                btnXemThayDoi.Attributes["style"] = "background-image:url('" + ModulePath + "images/log.gif');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;width:16px;";
                e.Row.Attributes["id"] = "row_" + btnXemThayDoi.ClientID;
                e.Row.Attributes["onclick"] = "setSelectedRow(this,'" + btnXemThayDoi.ClientID + "')";
            }
        }

        protected void grvHidden_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                Label lblSLNo = (Label)(e.Row.FindControl("lblSLNo"));
                Label lblStatus = (Label)(e.Row.FindControl("lblStatus"));
                Label lblSoNgayAp = (Label)(e.Row.FindControl("lblSoNgayAp"));
                Label lblChieuDaiBQ = (Label)(e.Row.FindControl("lblChieuDaiBQ"));
                Label lblVongBungBQ = (Label)(e.Row.FindControl("lblVongBungBQ"));
                Label lblNhanVien = (Label)(e.Row.FindControl("lblNhanVien"));
                if (r["NgayNo"] != DBNull.Value)
                {
                    lblSLNo.Text = r["TrungNo"].ToString();
                    TimeSpan ts = Convert.ToDateTime(r["NgayNo"]).Subtract(Convert.ToDateTime(r["NgayVaoAp"]));
                    lblSoNgayAp.Text = ts.Days.ToString();
                }
                else
                {
                    lblSLNo.Text = "0";
                }
                if (r["Status"].ToString() == "1")
                {
                    lblStatus.Text = "Đã nở";
                }
                else if (r["Status"].ToString() == "0")
                {
                    if (r["TrungNo"] == DBNull.Value || Convert.ToInt32(r["TrungNo"]) == 0)
                        lblStatus.Text = "Không nở";
                    else
                        lblStatus.Text = "Chưa nở";
                }
                if (r["ChieuDaiBQ"] != DBNull.Value) lblChieuDaiBQ.Text = Convert.ToDecimal(r["ChieuDaiBQ"]).ToString("0.#####");
                if (r["VongBungBQ"] != DBNull.Value) lblVongBungBQ.Text = Convert.ToDecimal(r["VongBungBQ"]).ToString("0.#####");
                if (r["NhanVien"] != DBNull.Value && r["NhanVien"].ToString() != "") lblNhanVien.Text = r["NhanVien"].ToString().Substring(1, r["NhanVien"].ToString().Length - 2).Replace("@@", ", ");
            }
        }

        protected void grvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }

        protected void grvDanhSach_Sorting(object sender, GridViewSortEventArgs e)
        {
            hdOrderBy.Value = e.SortExpression;
            Session["CSDe_SortExpression"] = e.SortExpression;
            if (e.SortDirection == SortDirection.Ascending) Session["CSDe_SortDirection"] = "ASC";
            else Session["CSDe_SortDirection"] = "DESC";
        }

        private string LoadODSParameters()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture("vi-VN");
            string strWhereClause = "";
            if (Config.GetSelectedValues(ddlCaMe) != "0, " && Config.GetSelectedValues(ddlCaMe) != "")
            {
                if (strWhereClause != "")
                {
                    strWhereClause += " and CaMe in (" + Config.GetSelectedValues(ddlCaMe).Remove(Config.GetSelectedValues(ddlCaMe).Length - 2) + ")";
                }
                else
                {
                    strWhereClause += "CaMe in (" + Config.GetSelectedValues(ddlCaMe).Remove(Config.GetSelectedValues(ddlCaMe).Length - 2) + ")";
                }
            }
            else
            {
                if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
                {
                    if (strWhereClause != "")
                    {
                        strWhereClause += " and LoaiCa in (" + Config.GetSelectedValues(ddlLoaiCa).Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 2) + ")";
                    }
                    else
                    {
                        strWhereClause += "LoaiCa in (" + Config.GetSelectedValues(ddlLoaiCa).Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 2) + ")";
                    }
                }
                if (Config.GetSelectedValues(ddlChuong) != "0, " && Config.GetSelectedValues(ddlChuong) != "")
                {
                    if (strWhereClause != "")
                    {
                        strWhereClause += " and Chuong in (" + Config.GetSelectedValues(ddlChuong).Remove(Config.GetSelectedValues(ddlChuong).Length - 2) + ")";
                    }
                    else
                    {
                        strWhereClause += "Chuong in (" + Config.GetSelectedValues(ddlChuong).Remove(Config.GetSelectedValues(ddlChuong).Length - 2) + ")";
                    }
                }
            }
            if (txtNgayApFrom.Text != "")
            {
                if (strWhereClause != "")
                {
                    strWhereClause += " and NgayVaoAp>='" + DateTime.Parse(txtNgayApFrom.Text, ci).ToString("MM/dd/yyyy") + "'";
                }
                else
                {
                    strWhereClause += "NgayVaoAp>='" + DateTime.Parse(txtNgayApFrom.Text, ci).ToString("MM/dd/yyyy") + "'";
                }
            }
            if (txtNgayApTo.Text != "")
            {
                if (strWhereClause != "")
                {
                    strWhereClause += " and NgayVaoAp<'" + DateTime.Parse(txtNgayApTo.Text, ci).AddDays(1).ToString("MM/dd/yyyy") + "'";
                }
                else
                {
                    strWhereClause += "NgayVaoAp<'" + DateTime.Parse(txtNgayApTo.Text, ci).AddDays(1).ToString("MM/dd/yyyy") + "'";
                }
            }
            if (ddlStatus.SelectedValue != "-1")
            {
                if (strWhereClause != "")
                {
                    if (ddlStatus.SelectedValue == "2")
                    {
                        strWhereClause += " and Status = 0 and (TrungDe - TrungVo - TrungThaiLoai - TrungKhongPhoi - TrungChetPhoi1 - TrungChetPhoi2 > 0)";
                    }
                    else if (ddlStatus.SelectedValue == "3")
                    {
                        strWhereClause += " and Status = 0 and (TrungDe - TrungVo - TrungThaiLoai - TrungKhongPhoi - TrungChetPhoi1 - TrungChetPhoi2 = 0)";
                    }
                    else
                    {
                        strWhereClause += " and Status = " + ddlStatus.SelectedValue;
                    }
                }
                else
                {
                    if (ddlStatus.SelectedValue == "2")
                    {
                        strWhereClause += "Status = 0 and (TrungDe - TrungVo - TrungThaiLoai - TrungKhongPhoi - TrungChetPhoi1 - TrungChetPhoi2 > 0)";
                    }
                    else if (ddlStatus.SelectedValue == "3")
                    {
                        strWhereClause += "Status = 0 and (TrungDe - TrungVo - TrungThaiLoai - TrungKhongPhoi - TrungChetPhoi1 - TrungChetPhoi2 = 0)";
                    }
                    else
                    {
                        strWhereClause += "Status = " + ddlStatus.SelectedValue;
                    }
                }
            }
            if (Config.GetSelectedValues(ddlPhongAp) != "0, " && Config.GetSelectedValues(ddlPhongAp) != "")
            {
                if (strWhereClause != "")
                {
                    strWhereClause += " and PhongAp in (" + Config.GetSelectedValues(ddlPhongAp).Remove(Config.GetSelectedValues(ddlPhongAp).Length - 2) + ")";
                }
                else
                {
                    strWhereClause += "PhongAp in (" + Config.GetSelectedValues(ddlPhongAp).Remove(Config.GetSelectedValues(ddlPhongAp).Length - 2) + ")";
                }
            }
            if (Config.GetSelectedValues(ddlNhanVien) != "0, " && Config.GetSelectedValues(ddlNhanVien) != "")
            {
                if (strWhereClause != "")
                {
                    strWhereClause += " and NhanVien in (" + Config.GetSelectedValues(ddlNhanVien).Remove(Config.GetSelectedValues(ddlNhanVien).Length - 2) + ")";
                }
                else
                {
                    strWhereClause += "NhanVien in (" + Config.GetSelectedValues(ddlNhanVien).Remove(Config.GetSelectedValues(ddlNhanVien).Length - 2) + ")";
                }
            }
            return strWhereClause;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            Session["AutoDisplay_CSD"] = true;
            Session["CSDe_RowStatus"] = ddlRowStatus.SelectedValue;
            Session["CSDe_LoaiCa"] = Config.GetSelectedValues(ddlLoaiCa);
            Session["CSDe_Chuong"] = Config.GetSelectedValues(ddlChuong);
            Session["CSDe_NgayApTu"] = txtNgayApFrom.Text;
            Session["CSDe_NgayApDen"] = txtNgayApTo.Text;
            Session["CSDe_TrangThai"] = ddlStatus.SelectedValue;
            Session["CSDe_PhongAp"] = Config.GetSelectedValues(ddlPhongAp);
            Session["CSDe_NhanVien"] = Config.GetSelectedValues(ddlNhanVien);

            grvDanhSach.PageSize = int.Parse(ddlPageSize.SelectedValue);
            grvDanhSach.PageIndex = 0;
            string strWhereClause = LoadODSParameters();

            if (ddlRowStatus.SelectedValue == "1")
            {
                grvDanhSach.DataSourceID = "odsDanhSach";
                odsDanhSach.SelectParameters["WhereClause"].DefaultValue = strWhereClause;
                lblTongSo.Text = CaSauDataObject.CountCaSauDe(strWhereClause).ToString();
            }
            else
            {
                grvDanhSach.DataSourceID = "odsDanhSachDelete";
                odsDanhSachDelete.SelectParameters["WhereClause"].DefaultValue = strWhereClause;
                lblTongSo.Text = CaSauDataObject.CountCaSauDeDelete(strWhereClause).ToString();
                btnKhoa.Visible = false;
                btnMoKhoa.Visible = false;
            }

            if (Session["CSDe_SortExpression"] != null && Session["CSDe_SortDirection"] != null)
            {
                SortDirection sd = SortDirection.Descending;
                if (Session["CSDe_SortDirection"].ToString() == "ASC") sd = SortDirection.Ascending;
                grvDanhSach.Sort(Session["CSDe_SortExpression"].ToString(), sd);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int IDTDD = int.Parse(((Button)sender).CommandArgument);
            int res = csCont.TheoDoiDe_Delete(IDTDD, UserId);
            if (res == 1)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "dadelete", "alert('Đã xóa!');", true);
                btnLoad_Click(null, null);
            }
            else Page.ClientScript.RegisterStartupScript(typeof(string), "chuadelete", "alert('Xóa không được do đã có cá con chết hoặc giết mổ hoặc bán!');", true);
        }

        protected void btnXemThayDoi_Click(object sender, EventArgs e)
        {
            DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
            if (ddlRowStatus.SelectedValue != "1") {btnKhoa.Visible = false; btnMoKhoa.Visible = false;}
            int CaSauDeLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauDeLichSuPage"], PortalId).TabID;
            Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauDeLichSuPage, "", "status/" + ((Button)sender).CommandName, "IDTheoDoiDe/" + ((Button)sender).CommandArgument) + "','',1000,600);</script>", false);
        }

        //protected void btnExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        //        System.Globalization.CultureInfo cii = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
        //        string filename = "CaSauDe_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xls";
        //        string strSQL = "QLCS_GetCaSauDe";
        //        SqlParameter[] param = new SqlParameter[2];
        //        param[0] = new SqlParameter("@WhereClause", LoadODSParameters());
        //        param[1] = new SqlParameter("@OrderBy", hdOrderBy.Value);
        //        DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
        //        Response.ContentType = "application/vnd.ms-excel";
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'></center><br/><table border='1'><tr>");
        //        DataColumn col = null;
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            col = dt.Columns[i];
        //            if (col.Caption != "SoCaDaRaChuong" && col.Caption != "Chet1_30Ngay")
        //            {
        //                sb.Append("<td style='background-color:#CCC;'>" + col.Caption + "</td>");
        //            }
        //        }
        //        sb.Append("</tr>");
        //        DataRow row = null;
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            row = dt.Rows[i];
        //            sb.Append("<tr>");
        //            for (int j = 0; j < dt.Columns.Count; j++)
        //            {
        //                if (j == 3 || j == 12)
        //                {
        //                    if (row[j] != DBNull.Value)
        //                    {
        //                        sb.Append("<td>" + Convert.ToDateTime(row[j]).ToString("dd/MM/yyyy") + "</td>");
        //                    }
        //                    else
        //                    {
        //                        sb.Append("<td></td>");
        //                    }
        //                }
        //                else if (j > 14 && j < 18)
        //                {
        //                    if (row[j] != DBNull.Value)
        //                    {
        //                        sb.Append("<td>" + Convert.ToDecimal(row[j]).ToString("0.##", cii) + "</td>");
        //                    }
        //                    else
        //                    {
        //                        sb.Append("<td></td>");
        //                    }
        //                }
        //                else if (j == 19)
        //                {
        //                    if (row[j].ToString() == "1")
        //                    {
        //                        sb.Append("<td>Đã nở</td>");
        //                    }
        //                    else if (row[j].ToString() == "0")
        //                    {
        //                        sb.Append("<td>Chưa nở</td>");
        //                    }
        //                    else if (row[j].ToString() == "-1")
        //                    {
        //                        sb.Append("<td>Không ấp</td>");
        //                    }
        //                }
        //                else if (j == 18 || j == 20)
        //                {
        //                }
        //                else
        //                {
        //                    sb.Append("<td>" + row[j].ToString() + "</td>");
        //                }
        //            }
        //            sb.Append("</tr>");
        //        }
        //        sb.Append("</table></body></html>");
        //        Response.Write(sb.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ex.Message);
        //    }
        //}

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                System.Globalization.CultureInfo cii = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
                string filename = "CaSauDe_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xls";
                string strSQL = "QLCS_GetCaSauDe";
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", LoadODSParameters());
                param[1] = new SqlParameter("@OrderBy", hdOrderBy.Value);
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                grvHidden.DataSource = dt;
                grvHidden.DataBind();

                System.IO.StringWriter sw = new System.IO.StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                grvHidden.RenderControl(hw);
                grvHidden.DataSource = null;
                grvHidden.DataBind();
                Response.Output.Write(sw.ToString().Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                Response.End();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}