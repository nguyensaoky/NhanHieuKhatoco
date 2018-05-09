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
using System.Text;
using System.IO;
using System.Data.SqlClient;
using DotNetNuke.Entities.Tabs;

namespace DotNetNuke.Modules.QLCS
{
    public partial class biendongcasau_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        private TabController t = new TabController();
        public string dataString = "";
        int CaSauBienDongPage;
        int CaSauBienDongLichSuPage;
        int ThuHoiDaLichSuPage;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
                Page.ClientScript.RegisterClientScriptInclude("ac_actb", ResolveUrl("~/js/autocomplete/actb.js"));
                Page.ClientScript.RegisterClientScriptInclude("ac_common", ResolveUrl("~/js/autocomplete/common.js"));
                CaSauBienDongLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauBienDongLichSuPage"], PortalId).TabID;
                CaSauBienDongPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauBienDongPage"], PortalId).TabID;
                ThuHoiDaLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_ThuHoiDaLichSuPage"], PortalId).TabID;
                InitGiaTriMoi();
                if (!IsPostBack)
                {
                    DataTable dtLoaiBienDong = csCont.LoadLoaiBienDong();
                    ddlLoaiBienDong.DataSource = dtLoaiBienDong;
                    ddlLoaiBienDong.DataTextField = "TenLoaiBienDong";
                    ddlLoaiBienDong.DataValueField = "IDLoaiBienDong";
                    ddlLoaiBienDong.DataBind();
                    ddlLoaiBienDong.Items.Insert(0, new ListItem("", "0"));

                    DataTable dtLoaiCa = csCont.LoadLoaiCa(1);
                    ddlLoaiCa.DataSource = dtLoaiCa;
                    ddlLoaiCa.DataTextField = "TenLoaiCa";
                    ddlLoaiCa.DataValueField = "IDLoaiCa";
                    ddlLoaiCa.DataBind();
                    ddlLoaiCa.Items.Insert(0, new ListItem("", "0"));

                    DataTable dtChuong = csCont.LoadChuong(1);
                    ddlChuong.DataSource = dtChuong;
                    ddlChuong.DataTextField = "Chuong";
                    ddlChuong.DataValueField = "IDChuong";
                    ddlChuong.DataBind();
                    ddlChuong.Items.Insert(0, new ListItem("","0"));

                    DataTable dtCaMe = csCont.LoadCaSauMe_AllTrangThai();
                    ddlCaMe.DataSource = dtCaMe;
                    ddlCaMe.DataTextField = "CaMe";
                    ddlCaMe.DataValueField = "IDCaSau";
                    ddlCaMe.DataBind();
                    ddlCaMe.Items.Insert(0, new ListItem("","0"));

                    DataTable dtStatus = new DataTable("Status");
                    DataRow dr = null;
                    dtStatus.Columns.AddRange(new DataColumn[] { new DataColumn("ID"), new DataColumn("Description") });
                    dr = dtStatus.NewRow();
                    dr["ID"] = 0;
                    dr["Description"] = "Bình thường";
                    dtStatus.Rows.Add(dr);
                    dr = dtStatus.NewRow();
                    dr["ID"] = 1;
                    dr["Description"] = "Bệnh";
                    dtStatus.Rows.Add(dr);
                    dr = dtStatus.NewRow();
                    dr["ID"] = -4;
                    dr["Description"] = "Loại thải";
                    dtStatus.Rows.Add(dr);
                    dr = dtStatus.NewRow();
                    dr["ID"] = -3;
                    dr["Description"] = "Đã bán";
                    dtStatus.Rows.Add(dr);
                    dr = dtStatus.NewRow();
                    dr["ID"] = -2;
                    dr["Description"] = "Giết mổ";
                    dtStatus.Rows.Add(dr);
                    dr = dtStatus.NewRow();
                    dr["ID"] = -1;
                    dr["Description"] = "Chết";
                    dtStatus.Rows.Add(dr);
                    ddlStatus.DataSource = dtStatus;
                    ddlStatus.DataValueField = "ID";
                    ddlStatus.DataTextField = "Description";
                    ddlStatus.DataBind();

                    if (Session["AutoDisplay_BDCS"] != null && Convert.ToBoolean(Session["AutoDisplay_BDCS"]))
                    {
                        txtID.Text = Session["BDCS_ID"].ToString();
                        txtMaSo.Text = Session["BDCS_MaSo"].ToString();
                        ddlGioiTinh.SelectedValue = Session["BDCS_GioiTinh"].ToString();
                        Config.SetSelectedValues(ddlLoaiCa, Session["BDCS_LoaiCa"].ToString());
                        Config.SetSelectedValues(ddlChuong, Session["BDCS_Chuong"].ToString());
                        Config.SetSelectedValues(ddlCaMe, Session["BDCS_CaMe"].ToString());
                        if (Session["BDCS_TrangThai"] != null && Session["BDCS_TrangThai"].ToString() != "")
                        {
                            Config.SetSelectedValues(ddlStatus, Session["BDCS_TrangThai"].ToString());
                        }
                        else
                        {
                            Config.SetSelectedValues(ddlStatus, "0, 1, 2, ");
                        }
                        btnLoad_Click(null, null);
                    }
                    else
                    {
                        Config.SetSelectedValues(ddlStatus, "0, 1, 2, ");
                    }
                    if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "KhoaCS"))
                    {
                        btnKhoa.Visible = false;
                        btnMoKhoa.Visible = false;
                    }
                    hdListBienDongPage.Value = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauListBienDongPage"], PortalId).TabID.ToString();
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnKhoa_Click(object sender, EventArgs e)
        {
            string strBienDong = "";
            foreach (GridViewRow row in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                if (chkChon.Checked)
                {
                    strBienDong += "@" + chkChon.Value + "@";
                }
            }
            csCont.Lock_BienDongCaSau(strBienDong, true);
            btnLoad_Click(null, null);
        }

        protected void btnMoKhoa_Click(object sender, EventArgs e)
        {
            string strBienDong = "";
            foreach (GridViewRow row in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                if (chkChon.Checked)
                {
                    strBienDong += "@" + chkChon.Value + "@";
                }
            }
            csCont.Lock_BienDongCaSau(strBienDong, false);
            btnLoad_Click(null, null);
        }

        private void InitGiaTriMoi()
        {
            dataString = "Đực/1;Cái/0;CXĐ/-1;BT/0;Chết/-1;Giết mổ/-2;Bán/-3;Bệnh/1;Loại thải/-4;Giống/1;Tăng trọng/0;";
            DataTable tblChuong = csCont.LoadChuong(-1);
            DataTable tblLoaiCa = csCont.LoadLoaiCa(-1);
            foreach (DataRow r in tblChuong.Rows)
            {
                dataString += r["Chuong"].ToString() + "/" + r["IDChuong"].ToString() + ";";
            }
            foreach (DataRow r in tblLoaiCa.Rows)
            {
                dataString += r["TenLoaiCa"].ToString() + "/" + r["IDLoaiCa"].ToString() + ";";
            }
            dataString = dataString.Remove(dataString.Length - 1);
            Page.ClientScript.RegisterStartupScript(typeof(string), "initGiaTriMoi", "<script language=javascript>initGiaTriMoi();</script>", false);
        }

        protected void grvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(e.Row.FindControl("chkChon"));
                Label lblStatus = (Label)(e.Row.FindControl("lblStatus"));
                Label lblNote = (Label)(e.Row.FindControl("lblNote"));
                HyperLink lnkIDCaSau = (HyperLink)(e.Row.FindControl("lnkIDCaSau"));
                HyperLink lnkCaMe= (HyperLink)(e.Row.FindControl("lnkCaMe"));
                lnkIDCaSau.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdListBienDongPage.Value), "", "IDCaSau/" + r["IDCaSau"].ToString()) + "','',800,600);";
                lnkIDCaSau.Attributes["style"] = "cursor:pointer;font-weight:bold;";
                lnkCaMe.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdListBienDongPage.Value), "", "IDCaSau/" + r["CaMe"].ToString()) + "','',800,600);";
                lnkCaMe.Attributes["style"] = "cursor:pointer;font-weight:bold;";
                chkChon.Attributes["name"] = r["ID"].ToString();
                chkChon.Attributes["onclick"] = "chon_click(event, this);";
                int tt = Convert.ToInt32(r["Status"]);
                if (tt < 0) lblStatus.Text = r["TrangThai"].ToString() + " (" + Convert.ToDateTime(r["LatestChange"]).ToString("dd/MM/yyyy") + ")";
                else 
                { 
                    lblStatus.Text = r["TrangThai"].ToString();
                    if (Convert.ToInt32(r["EndStatus"]) < 0) lblStatus.Text += " (" + Convert.ToDateTime(r["LatestChange"]).ToString("dd/MM/yyyy") + ")";
                }
                Button btnDeleteBienDong = (Button)(e.Row.FindControl("btnDeleteBienDong"));
                Button btnEditBienDong = (Button)(e.Row.FindControl("btnEditBienDong"));
                Button btnXemThayDoi = (Button)(e.Row.FindControl("btnXemThayDoi"));
                Button btnXemSPThuHoi = (Button)(e.Row.FindControl("btnXemSPThuHoi"));
                btnXemThayDoi.CommandArgument = r["ID"].ToString();
                chkChon.Value = r["ID"].ToString();
                btnXemSPThuHoi.Visible = false;
                if (Convert.ToDateTime(r["ThoiDiemBienDong"]) < Config.NgayKhoaSo())
                {
                    btnDeleteBienDong.Enabled = false;
                    btnDeleteBienDong.CssClass = "buttondisable";
                    btnEditBienDong.Enabled = false;
                    btnEditBienDong.CssClass = "buttondisable";
                }
                else
                {
                    if (Convert.ToBoolean(r["Lock"]))
                    {
                        btnDeleteBienDong.Visible = false;
                        btnEditBienDong.Visible = false;
                    }
                    else
                    {
                        if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                        {
                            btnDeleteBienDong.Visible = false;
                            btnEditBienDong.Visible = false;
                        }
                        else
                        {
                            btnDeleteBienDong.Visible = true;
                            btnEditBienDong.Visible = true;
                            btnDeleteBienDong.CommandArgument = r["ID"].ToString();
                            btnEditBienDong.CommandArgument = r["ID"].ToString() + "," + r["IDCaSau"].ToString() + "," + Convert.ToDateTime(r["ThoiDiemBienDong"]).ToString("dd/MM/yyyy HH:mm:ss") + "," + r["Note"].ToString();
                        }
                    }
                }

                if (r["LoaiBienDong"].ToString() == "1")
                {
                    DataTable tbl = csCont.LoadChuongByID(int.Parse(r["Note"].ToString()));
                    if (tbl != null && tbl.Rows.Count == 1)
                    {
                        lblNote.Text = tbl.Rows[0]["Chuong"].ToString();
                    }
                    btnEditBienDong.CommandName = "editchuyenchuong";
                    btnXemThayDoi.CommandName = "chuyenchuong";
                }
                else if (r["LoaiBienDong"].ToString() == "2")
                {
                    if (r["Note"].ToString() == "1") lblNote.Text = "Đực";
                    else if (r["Note"].ToString() == "-1") lblNote.Text = "CXĐ";
                    else if (r["Note"].ToString() == "0") lblNote.Text = "Cái";

                    btnEditBienDong.CommandName = "editchuyengioitinh";
                    btnXemThayDoi.CommandName = "chuyengioitinh";
                }
                else if (r["LoaiBienDong"].ToString() == "3")
                {
                    DataTable tbl = csCont.LoadNguonGocByID(int.Parse(r["Note"].ToString()));
                    if (tbl != null && tbl.Rows.Count == 1)
                    {
                        lblNote.Text = tbl.Rows[0]["TenNguonGoc"].ToString();
                    }

                    btnEditBienDong.CommandName = "editchuyennguongoc";
                    btnXemThayDoi.CommandName = "chuyennguongoc";
                }
                else if (r["LoaiBienDong"].ToString() == "4")
                {
                    DataTable tbl = csCont.LoadLoaiCaByID(int.Parse(r["Note"].ToString()));
                    if (tbl != null && tbl.Rows.Count == 1)
                    {
                        lblNote.Text = tbl.Rows[0]["TenLoaiCa"].ToString();
                    }

                    btnEditBienDong.CommandName = "editchuyenloaica";
                    btnXemThayDoi.CommandName = "chuyenloaica";
                }
                else if (r["LoaiBienDong"].ToString() == "5")
                {
                    lblNote.Text = r["Note"].ToString();
                    btnEditBienDong.CommandName = "editchuyenmaso";
                    btnXemThayDoi.CommandName = "chuyenmaso";
                }
                else if (r["LoaiBienDong"].ToString() == "6")
                {
                    if (r["Note"].ToString() == "0") lblNote.Text = "BT";
                    else if (r["Note"].ToString() == "1") lblNote.Text = "Bệnh";
                    else if (r["Note"].ToString() == "-4") lblNote.Text = "Loại thải";
                    else if (r["Note"].ToString() == "-1") lblNote.Text = "Chết";
                    else if (r["Note"].ToString() == "-2") lblNote.Text = "Giết mổ";
                    else if (r["Note"].ToString() == "-3") lblNote.Text = "Bán";
                    else lblNote.Text = "--";

                    int Note = int.Parse(r["Note"].ToString());
                    if ( Note == -1 || Note == -2 || Note == -4)
                    {
                        btnDeleteBienDong.Visible = false;
                        btnEditBienDong.Visible = false;
                    }
                    btnEditBienDong.CommandName = "editchuyentrangthai";
                    btnXemThayDoi.CommandName = "chuyentrangthai";
                    if (lblNote.Text == "Chết" || lblNote.Text == "Loại thải")
                    {
                        btnXemSPThuHoi.Visible = true;
                        btnXemSPThuHoi.CommandName = "chet";
                    }
                }
                else if (r["LoaiBienDong"].ToString() == "7")
                {
                    if (r["Note"].ToString() == "1") lblNote.Text = "Giống";
                    else lblNote.Text = "Tăng trọng";

                    btnEditBienDong.CommandName = "editchuyengiong";
                    btnXemThayDoi.CommandName = "chuyengiong";
                }
                else if (r["LoaiBienDong"].ToString() == "8")
                {
                    lblNote.Text = "";
                    btnDeleteBienDong.Visible = false;
                    btnEditBienDong.Visible = false;
                    btnXemThayDoi.Visible = false;
                }
                else
                {
                    lblNote.Text = r["Note"].ToString();
                    btnDeleteBienDong.Visible = false;
                    btnEditBienDong.Visible = false;
                    btnXemThayDoi.Visible = false;
                }

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

                //btnEditBienDong.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/" + btnEditBienDong.CommandName) + "','',600,400);";
                btnXemThayDoi.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongLichSuPage, "", "type/" + btnXemThayDoi.CommandName, "status/1", "IDBienDong/" + btnXemThayDoi.CommandArgument) + "','',800,400);";
                btnXemSPThuHoi.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(ThuHoiDaLichSuPage, "", "status/1", "IDThuHoiDa/" + csCont.ThuHoiDa_GetIDByCa(Convert.ToInt32(r["IDCaSau"]), 1)) + "','',800,400);";
            }
        }

        protected void grvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }

        protected void grvDanhSach_Sorting(object sender, GridViewSortEventArgs e)
        {
        }

        private string LoadODSParameters()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            string strWhereClause = "";
            
            string[] arr = Config.GetSelectedValues(ddlStatus).Split(new string[] {", "}, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length == 6) strWhereClause += "0=0";
            else strWhereClause += "Status in (" + Config.GetSelectedValues(ddlStatus).Remove(Config.GetSelectedValues(ddlStatus).Length - 2) + ")";
            
            if (txtID.Text.Trim() != "")
            {
                strWhereClause += " and IDCaSau = '" + txtID.Text + "'";
            }
            if (txtMaSo.Text.Trim() != "")
            {
                strWhereClause += " and MaSo = '" + txtMaSo.Text + "'";
            }
            if (ddlGioiTinh.SelectedValue != "-2")
            {
                strWhereClause += " and GioiTinh = " + ddlGioiTinh.SelectedValue;
            }
            if (ddlGiong.SelectedValue != "-1")
            {
                strWhereClause += " and Giong = " + ddlGiong.SelectedValue;
            }
            if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
            {
                strWhereClause += " and LoaiCa in (" + Config.GetSelectedValues(ddlLoaiCa).Remove(Config.GetSelectedValues(ddlLoaiCa).Length-2) + ")";
            }
            if (Config.GetSelectedValues(ddlChuong) != "0, " && Config.GetSelectedValues(ddlChuong) != "")
            {
                strWhereClause += " and Chuong in (" + Config.GetSelectedValues(ddlChuong).Remove(Config.GetSelectedValues(ddlChuong).Length - 2) + ")";
            }
            if (Config.GetSelectedValues(ddlCaMe) != "0, " && Config.GetSelectedValues(ddlCaMe) != "")
            {
                strWhereClause += " and CaMe in (" + Config.GetSelectedValues(ddlCaMe).Remove(Config.GetSelectedValues(ddlCaMe).Length - 2) + ")";
            }
            if (txtFromDate.Text != "")
            {
                strWhereClause += " and NgayXuongChuong >= '" + DateTime.Parse(txtFromDate.Text, ci).ToString("yyyyMMdd") + "'";
            }
            if (txtToDate.Text != "")
            {
                strWhereClause += " and NgayXuongChuong < '" + DateTime.Parse(txtToDate.Text, ci).AddDays(1).ToString("yyyyMMdd") + "'";
            }
            bool Die = false;
            if (txtDieFrom.Text != "")
            {
                strWhereClause += " and LatestChange >= '" + DateTime.Parse(txtDieFrom.Text, ci).ToString("yyyyMMdd") + "' and Status < 0";
                Die = true;
            }
            if (txtDieTo.Text != "")
            {
                strWhereClause += " and LatestChange < '" + DateTime.Parse(txtDieTo.Text, ci).AddDays(1).ToString("yyyyMMdd") + "'";
                if (!Die) strWhereClause += " and Status < 0";
            }
            if (txtBienDongFrom.Text != "")
            {
                strWhereClause += " and ThoiDiemBienDong >= '" + DateTime.Parse(txtBienDongFrom.Text, ci).ToString("yyyyMMdd") + "'";
            }
            if (txtBienDongTo.Text != "")
            {
                strWhereClause += " and ThoiDiemBienDong < '" + DateTime.Parse(txtBienDongTo.Text, ci).AddDays(1).ToString("yyyyMMdd") + "'";
            }
            if (ddlLoaiBienDong.SelectedValue != "0")
            {
                strWhereClause += " and LoaiBienDong = " + ddlLoaiBienDong.SelectedValue;
            }
            if (txtGiaTriMoi.Text.Trim() != "")
            {
                strWhereClause += " and Note = '" + txtGiaTriMoi.Text.Trim().Substring(txtGiaTriMoi.Text.Trim().IndexOf("/")+1) + "'";
            }
            return strWhereClause;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            Session["AutoDisplay_BDCS"] = true;
            Session["BDCS_ID"] = txtID.Text;
            Session["BDCS_MaSo"] = txtMaSo.Text;
            Session["BDCS_GioiTinh"] = ddlGioiTinh.SelectedValue;
            Session["BDCS_LoaiCa"] = Config.GetSelectedValues(ddlLoaiCa);
            Session["BDCS_Chuong"] = Config.GetSelectedValues(ddlChuong);
            Session["BDCS_CaMe"] = Config.GetSelectedValues(ddlCaMe);
            Session["BDCS_TrangThai"] = Config.GetSelectedValues(ddlStatus);

            grvDanhSach.DataSourceID = "odsDanhSach";
            grvDanhSach.PageSize = int.Parse(ddlPageSize.SelectedValue);
            string strWhereClause = LoadODSParameters();
            if (strWhereClause != "")
            {
                odsDanhSach.SelectParameters["WhereClause"].DefaultValue = strWhereClause;
                lblTongSo.Text = CaSauDataObject.CountBienDong(strWhereClause).ToString();
                grvDanhSach.Visible = true;
            }
            else
            {
                grvDanhSach.Visible = false;
                lblTongSo.Text = "0";
            }
        }

        protected void btnDeleteBienDong_Click(object sender, EventArgs e)
        {
            csCont.DeleteBienDongCaSau(int.Parse(((Button)sender).CommandArgument), UserId);
            btnLoad_Click(null, null);
        }

        protected void btnEditBienDong_Click(object sender, EventArgs e)
        {
            Session["EditBienDongCaSauParam"] = ((Button)sender).CommandArgument;
            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "openwindow", "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/" + ((Button)sender).CommandName) + "','',600,400);", true);
        }
    }
}