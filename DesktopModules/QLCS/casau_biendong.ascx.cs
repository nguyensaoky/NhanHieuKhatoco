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

namespace DotNetNuke.Modules.QLCS
{
    public partial class casau_biendong : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
        private CaSauController csCont = new CaSauController();
        int CaSauBienDongPage;
        int CaSauBienDongLichSuPage;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
                if (!IsPostBack)
                {
                    if(Request.QueryString["IDCaSau"] != null)
                    {
                        int IDCaSau = int.Parse(Request.QueryString["IDCaSau"]);
                        DataTable tblChiTiet = csCont.LoadCaSauHienTaiByID(IDCaSau);
                        DataTable tblBienDong = csCont.LoadCaSauBienDong(IDCaSau);
                        DataTable tblBienDongDelete = csCont.LoadCaSauBienDong_Delete(IDCaSau);
                        DataTable tblSinhSan = csCont.LoadTheoDoiDeByIDCaSau(IDCaSau);
                        grvChiTiet.DataSource = tblChiTiet;
                        grvChiTiet.DataBind();
                        grvDanhSach.DataSource = tblBienDong;
                        grvDanhSach.DataBind();
                        if (tblBienDongDelete.Rows.Count > 0)
                        {
                            lblBienDongXoa.Text = "BIẾN ĐỘNG ĐÃ XÓA";
                            lblBienDongXoa.Visible = true;
                            grvDanhSachXoa.DataSource = tblBienDongDelete;
                            grvDanhSachXoa.DataBind();
                        }
                        if (tblSinhSan.Rows.Count > 0)
                        {
                            lblSinhSan.Text = "THỐNG KÊ SINH SẢN";
                            lblSinhSan.Visible = true;
                            grvSinhSan.DataSource = tblSinhSan;
                            grvSinhSan.DataBind();
                        }

                        if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "KhoaCS"))
                        {
                            btnKhoa.Visible = false;
                            btnMoKhoa.Visible = false;
                            grvDanhSach.Columns[5].Visible = false;
                        }

                        if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                        {
                            grvDanhSach.Columns[6].Visible = false;
                            grvDanhSach.Columns[7].Visible = false;
                        }
                        if(tblChiTiet.Rows.Count > 0)
                            txtGhiChu.Text = tblChiTiet.Rows[0]["GhiChu"].ToString();
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
            int IDCaSau = int.Parse(Request.QueryString["IDCaSau"]);
            DataTable tblBienDong = csCont.LoadCaSauBienDong(IDCaSau);
            grvDanhSach.DataSource = tblBienDong;
            grvDanhSach.DataBind();
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
            int IDCaSau = int.Parse(Request.QueryString["IDCaSau"]);
            DataTable tblBienDong = csCont.LoadCaSauBienDong(IDCaSau);
            grvDanhSach.DataSource = tblBienDong;
            grvDanhSach.DataBind();
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                int IDCaSau = int.Parse(Request.QueryString["IDCaSau"]);
                DataTable tblChiTiet = csCont.LoadCaSauHienTaiByID(IDCaSau);
                DataTable tblBienDong = csCont.LoadCaSauBienDong(IDCaSau);
                DataTable tblSinhSan = csCont.LoadTheoDoiDeByIDCaSau(IDCaSau);
                grvChiTiet.DataSource = tblChiTiet;
                grvChiTiet.DataBind();
                grvDanhSach.DataSource = tblBienDong;
                grvDanhSach.DataBind();
                if (tblSinhSan.Rows.Count > 0)
                {
                    lblSinhSan.Text = "THỐNG KÊ SINH SẢN";
                    lblSinhSan.Visible = true;
                    grvSinhSan.DataSource = tblSinhSan;
                    grvSinhSan.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void grvChiTiet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                HyperLink lnkCaMe = (HyperLink)(e.Row.FindControl("lnkCaMe"));
                lnkCaMe.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdListBienDongPage.Value), "", "IDCaSau/" + r["CaMe"].ToString()) + "','',800,600);";
                lnkCaMe.Attributes["style"] = "cursor:pointer;font-weight:bold;";
                Label lblStatus = (Label)(e.Row.FindControl("lblStatus"));
                if (r["Status"].ToString() == "0") lblStatus.Text = "BT";
                else if (r["Status"].ToString() == "1") lblStatus.Text = "Bệnh";
                else if (r["Status"].ToString() == "-4") lblStatus.Text = "Loại thải";
                else if (r["Status"].ToString() == "-1") lblStatus.Text = "Chết";
                else if (r["Status"].ToString() == "-2") lblStatus.Text = "Giết mổ";
                else if (r["Status"].ToString() == "-3") lblStatus.Text = "Bán";
                else lblStatus.Text = "--";
            }
        }

        protected void grvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(e.Row.FindControl("chkChon"));
                Label lblNote = (Label)(e.Row.FindControl("lblNote"));
                Label lblOldValue = (Label)(e.Row.FindControl("lblOldValue"));
                Button btnDeleteBienDong = (Button)(e.Row.FindControl("btnDeleteBienDong"));
                Button btnEditBienDong = (Button)(e.Row.FindControl("btnEditBienDong"));
                Button btnXemThayDoi = (Button)(e.Row.FindControl("btnXemThayDoi"));
                Button btnXemSPThuHoi = (Button)(e.Row.FindControl("btnXemSPThuHoi"));
                btnXemThayDoi.CommandArgument = r["ID"].ToString();
                chkChon.Value = r["ID"].ToString();
                btnXemSPThuHoi.Visible = false;
                if (Convert.ToDateTime(r["ThoiDiemBienDong"]) < Config.NgayKhoaSo())
                {
                    //btnDeleteBienDong.Visible = false;
                    //btnEditBienDong.Visible = false;
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
                            btnEditBienDong.CommandArgument = r["ID"].ToString() + "," + Request.QueryString["IDCaSau"] + "," + Convert.ToDateTime(r["ThoiDiemBienDong"]).ToString("dd/MM/yyyy HH:mm:ss") + "," + r["Note"].ToString();
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
                    DataTable tblOld = csCont.LoadChuongByID(int.Parse(r["Chuong"].ToString()));
                    if (tblOld != null && tblOld.Rows.Count == 1)
                    {
                        lblOldValue.Text = tblOld.Rows[0]["Chuong"].ToString();
                    }
                    btnEditBienDong.CommandName = "editchuyenchuong";
                    btnXemThayDoi.CommandName = "chuyenchuong";
                }
                else if (r["LoaiBienDong"].ToString() == "2")
                {
                    if(r["Note"].ToString() == "1") lblNote.Text = "Đực";
                    else if (r["Note"].ToString() == "-1") lblNote.Text = "CXĐ";
                    else if (r["Note"].ToString() == "0") lblNote.Text = "Cái";

                    if (r["GioiTinh"].ToString() == "1") lblOldValue.Text = "Đực";
                    else if (r["GioiTinh"].ToString() == "-1") lblOldValue.Text = "CXĐ";
                    else if (r["GioiTinh"].ToString() == "0") lblOldValue.Text = "Cái";
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

                    DataTable tblOld = csCont.LoadNguonGocByID(int.Parse(r["NguonGoc"].ToString()));
                    if (tblOld != null && tblOld.Rows.Count == 1)
                    {
                        lblOldValue.Text = tblOld.Rows[0]["TenNguonGoc"].ToString();
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

                    DataTable tblOld = csCont.LoadLoaiCaByID(int.Parse(r["LoaiCa"].ToString()));
                    if (tblOld != null && tblOld.Rows.Count == 1)
                    {
                        lblOldValue.Text = tblOld.Rows[0]["TenLoaiCa"].ToString();
                    }
                    btnEditBienDong.CommandName = "editchuyenloaica";
                    btnXemThayDoi.CommandName = "chuyenloaica";
                }
                else if (r["LoaiBienDong"].ToString() == "5")
                {
                    lblNote.Text = r["Note"].ToString();
                    lblOldValue.Text = r["MaSo"].ToString();
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

                    if (r["Status"].ToString() == "0") lblOldValue.Text = "BT";
                    else if (r["Status"].ToString() == "1") lblOldValue.Text = "Bệnh";
                    else if (r["Status"].ToString() == "-4") lblOldValue.Text = "Loại thải";
                    else if (r["Status"].ToString() == "-1") lblOldValue.Text = "Chết";
                    else if (r["Status"].ToString() == "-2") lblOldValue.Text = "Giết mổ";
                    else if (r["Status"].ToString() == "-3") lblOldValue.Text = "Bán";
                    else lblOldValue.Text = "--";

                    if (int.Parse(r["Note"].ToString()) < 0 && int.Parse(r["Note"].ToString()) > -3)
                    {
                        btnDeleteBienDong.Visible = false;
                        btnEditBienDong.Visible = false;
                    }
                    btnEditBienDong.CommandName = "editchuyentrangthai";
                    btnXemThayDoi.CommandName = "chuyentrangthai";
                    if(lblNote.Text == "Chết")
                    {
                        btnXemSPThuHoi.Visible = true;
                        btnXemSPThuHoi.CommandName = "chet";
                    }
                }
                else if (r["LoaiBienDong"].ToString() == "7")
                {
                    if (r["Note"].ToString() == "1") lblNote.Text = "Giống";
                    else lblNote.Text = "Tăng trọng";

                    if (Convert.ToBoolean(r["Giong"])) lblOldValue.Text = "Giống";
                    else lblOldValue.Text = "Tăng trọng";
                    btnEditBienDong.CommandName = "editchuyengiong";
                    btnXemThayDoi.CommandName = "chuyengiong";
                }
                else if (r["LoaiBienDong"].ToString() == "8")
                {
                    lblNote.Text = "";
                    lblOldValue.Text = "";
                    btnDeleteBienDong.Visible = false;
                    btnEditBienDong.Visible = false;
                    btnXemThayDoi.Visible = false;
                }
                else
                {
                    lblNote.Text = r["Note"].ToString();
                    lblOldValue.Text = "";
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
                btnXemThayDoi.Attributes["style"] = "background-image:url('" + ModulePath + "images/log.gif');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;";
                e.Row.Attributes["id"] = "row_" + btnXemThayDoi.ClientID;
                e.Row.Attributes["onclick"] = "setSelectedRow(this,'" + btnXemThayDoi.ClientID + "')";
            }
        }

        protected void grvDanhSachXoa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                Label lblNote = (Label)(e.Row.FindControl("lblNote"));
                Label lblOldValue = (Label)(e.Row.FindControl("lblOldValue"));
                Button btnXemThayDoi = (Button)(e.Row.FindControl("btnXemThayDoiXoa"));
                Button btnXemSPThuHoiXoa = (Button)(e.Row.FindControl("btnXemSPThuHoiXoa"));
                btnXemSPThuHoiXoa.Visible = false;
                btnXemThayDoi.CommandArgument = r["IDBienDong"].ToString();
                if (r["LoaiBienDong"].ToString() == "1")
                {
                    DataTable tbl = csCont.LoadChuongByID(int.Parse(r["Note"].ToString()));
                    if (tbl != null && tbl.Rows.Count == 1)
                    {
                        lblNote.Text = tbl.Rows[0]["Chuong"].ToString();
                    }
                    DataTable tblOld = csCont.LoadChuongByID(int.Parse(r["Chuong"].ToString()));
                    if (tblOld != null && tblOld.Rows.Count == 1)
                    {
                        lblOldValue.Text = tblOld.Rows[0]["Chuong"].ToString();
                    }
                    btnXemThayDoi.CommandName = "chuyenchuong";
                }
                else if (r["LoaiBienDong"].ToString() == "2")
                {
                    if (r["Note"].ToString() == "1") lblNote.Text = "Đực";
                    else if (r["Note"].ToString() == "-1") lblNote.Text = "CXĐ";
                    else if (r["Note"].ToString() == "0") lblNote.Text = "Cái";

                    if (r["GioiTinh"].ToString() == "1") lblOldValue.Text = "Đực";
                    else if (r["GioiTinh"].ToString() == "-1") lblOldValue.Text = "CXĐ";
                    else if (r["GioiTinh"].ToString() == "0") lblOldValue.Text = "Cái";
                    btnXemThayDoi.CommandName = "chuyengioitinh";
                }
                else if (r["LoaiBienDong"].ToString() == "3")
                {
                    DataTable tbl = csCont.LoadNguonGocByID(int.Parse(r["Note"].ToString()));
                    if (tbl != null && tbl.Rows.Count == 1)
                    {
                        lblNote.Text = tbl.Rows[0]["TenNguonGoc"].ToString();
                    }

                    DataTable tblOld = csCont.LoadNguonGocByID(int.Parse(r["NguonGoc"].ToString()));
                    if (tblOld != null && tblOld.Rows.Count == 1)
                    {
                        lblOldValue.Text = tblOld.Rows[0]["TenNguonGoc"].ToString();
                    }
                    btnXemThayDoi.CommandName = "chuyennguongoc";
                }
                else if (r["LoaiBienDong"].ToString() == "4")
                {
                    DataTable tbl = csCont.LoadLoaiCaByID(int.Parse(r["Note"].ToString()));
                    if (tbl != null && tbl.Rows.Count == 1)
                    {
                        lblNote.Text = tbl.Rows[0]["TenLoaiCa"].ToString();
                    }

                    DataTable tblOld = csCont.LoadLoaiCaByID(int.Parse(r["LoaiCa"].ToString()));
                    if (tblOld != null && tblOld.Rows.Count == 1)
                    {
                        lblOldValue.Text = tblOld.Rows[0]["TenLoaiCa"].ToString();
                    }
                    btnXemThayDoi.CommandName = "chuyenloaica";
                }
                else if (r["LoaiBienDong"].ToString() == "5")
                {
                    lblNote.Text = r["Note"].ToString();
                    lblOldValue.Text = r["MaSo"].ToString();
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

                    if (r["Status"].ToString() == "0") lblOldValue.Text = "BT";
                    else if (r["Status"].ToString() == "1") lblOldValue.Text = "Bệnh";
                    else if (r["Status"].ToString() == "-4") lblOldValue.Text = "Loại thải";
                    else if (r["Status"].ToString() == "-1") lblOldValue.Text = "Chết";
                    else if (r["Status"].ToString() == "-2") lblOldValue.Text = "Giết mổ";
                    else if (r["Status"].ToString() == "-3") lblOldValue.Text = "Bán";
                    else lblOldValue.Text = "--";
                    btnXemThayDoi.CommandName = "chuyentrangthai";
                    if (lblNote.Text == "Chết")
                    {
                        btnXemSPThuHoiXoa.Visible = true;
                        btnXemSPThuHoiXoa.CommandName = "chet";
                    }
                }
                else if (r["LoaiBienDong"].ToString() == "7")
                {
                    if (r["Note"].ToString() == "1") lblNote.Text = "Giống";
                    else lblNote.Text = "Tăng trọng";

                    if (Convert.ToBoolean(r["Giong"])) lblOldValue.Text = "Giống";
                    else lblOldValue.Text = "Tăng trọng";
                    btnXemThayDoi.CommandName = "chuyengiong";
                }
                else if (r["LoaiBienDong"].ToString() == "8")
                {
                    lblNote.Text = "";
                    lblOldValue.Text = "";
                    btnXemThayDoi.Visible = false;
                }
                else
                {
                    lblNote.Text = r["Note"].ToString();
                    lblOldValue.Text = "";
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
                btnXemThayDoi.Attributes["style"] = "background-image:url('" + ModulePath + "images/log.gif');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;";
                e.Row.Attributes["id"] = "row_" + btnXemThayDoi.ClientID;
                e.Row.Attributes["onclick"] = "setSelectedRow(this,'" + btnXemThayDoi.ClientID + "')";
            }
        }

        protected void grvSinhSan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                Label lblTrongLuongConBQ = (Label)(e.Row.FindControl("lblTrongLuongConBQ"));
                Label lblChieuDaiBQ = (Label)(e.Row.FindControl("lblChieuDaiBQ"));
                Label lblVongBungBQ = (Label)(e.Row.FindControl("lblVongBungBQ"));
                lblTrongLuongConBQ.Text = Config.ToXVal2(r["TrongLuongConBQ"], 0);
                lblChieuDaiBQ.Text = Config.ToXVal2(r["ChieuDaiBQ"], 0);
                lblVongBungBQ.Text = Config.ToXVal2(r["VongBungBQ"], 0);
            }
        }

        protected void btnDeleteBienDong_Click(object sender, EventArgs e)
        {
            csCont.DeleteBienDongCaSau(int.Parse(((Button)sender).CommandArgument), UserId);
            int IDCaSau = int.Parse(Request.QueryString["IDCaSau"]);
            DataTable tblChiTiet = csCont.LoadCaSauHienTaiByID(IDCaSau);
            DataTable tblBienDong = csCont.LoadCaSauBienDong(IDCaSau);
            DataTable tblBienDongDelete = csCont.LoadCaSauBienDong_Delete(IDCaSau);
            grvChiTiet.DataSource = tblChiTiet;
            grvChiTiet.DataBind();
            grvDanhSach.DataSource = tblBienDong;
            grvDanhSach.DataBind();
            grvDanhSachXoa.DataSource = tblBienDongDelete;
            grvDanhSachXoa.DataBind();
            lblBienDongXoa.Text = "BIẾN ĐỘNG ĐÃ XÓA";
            lblBienDongXoa.Visible = true;
        }

        protected void btnEditBienDong_Click(object sender, EventArgs e)
        {
            CaSauBienDongPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauBienDongPage"], PortalId).TabID;
            Session["EditBienDongCaSauParam"] = ((Button)sender).CommandArgument;
            Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/" + ((Button)sender).CommandName) + "','',600,400);</script>", false);
        }

        protected void btnXemThayDoi_Click(object sender, EventArgs e)
        {
            CaSauBienDongLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauBienDongLichSuPage"], PortalId).TabID;
            Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongLichSuPage, "", "type/" + ((Button)sender).CommandName, "status/1", "IDBienDong/" + ((Button)sender).CommandArgument) + "','',800,400);</script>", false);
        }

        protected void btnXemThayDoiXoa_Click(object sender, EventArgs e)
        {
            CaSauBienDongLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauBienDongLichSuPage"], PortalId).TabID;
            Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongLichSuPage, "", "type/" + ((Button)sender).CommandName, "status/0", "IDBienDong/" + ((Button)sender).CommandArgument) + "','',800,400);</script>", false);
        }

        protected void btnXemSPThuHoi_Click(object sender, EventArgs e)
        {
            if(((Button)sender).CommandName == "chet")
            {
                DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
                int ThuHoiDaLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_ThuHoiDaLichSuPage"], PortalId).TabID;
                Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(ThuHoiDaLichSuPage, "", "status/1", "IDThuHoiDa/" + csCont.ThuHoiDa_GetIDByCa(int.Parse(Request.QueryString["IDCaSau"]),1)) + "','',800,400);</script>", false);
            }
        }

        protected void btnXemSPThuHoiXoa_Click(object sender, EventArgs e)
        {
            if (((Button)sender).CommandName == "chet")
            {
                DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
                int ThuHoiDaLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_ThuHoiDaLichSuPage"], PortalId).TabID;
                Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(ThuHoiDaLichSuPage, "", "status/0", "IDThuHoiDa/" + csCont.ThuHoiDa_GetIDByCa(int.Parse(Request.QueryString["IDCaSau"]),0)) + "','',800,400);</script>", false);
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = "ChiTietCa_ID" + Request.QueryString["IDCaSau"] + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xls";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                string s = @"<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body style='text-align:center;font-family:Times New Roman;'><br/><br/>";
                s += "<table border='1'>";
                s += "<tr><td colspan='" + grvChiTiet.Columns.Count.ToString() + "'><b>THÔNG TIN HIỆN TẠI</b></td></tr>";
                s += "<tr>";
                DataControlField col = null;
                for (int i = 0; i < grvChiTiet.Columns.Count; i++)
                {
                    col = grvChiTiet.Columns[i];
                    s += "<td style='background-color:#CCC;'>" + col.HeaderText + "</td>";
                }
                s += "</tr>";
                GridViewRow row = null;
                for (int i = 0; i < grvChiTiet.Rows.Count; i++)
                {
                    row = grvChiTiet.Rows[i];
                    s += "<tr>";
                    TableCell cell = null;
                    int j = 0;
                    for (j = 0; j < row.Cells.Count-1; j++)
                    {
                        cell = row.Cells[j];
                        s += "<td>" + cell.Text + "</td>";
                    }
                    Label l = (Label)(row.Cells[j].FindControl("lblStatus"));
                    s += "<td>" + l.Text + "</td>";
                    s += "</tr>";
                }
                s += "</table><br/><br/>";
                //-------------------------------------------------
                s += "<table border='1'>";
                int cols = grvDanhSach.Columns.Count - 4;
                s += "<tr><td colspan='" + cols.ToString() + "'><b>CHI TIẾT BIẾN ĐỘNG</b></td></tr>";
                s += "<tr>";
                for (int i = 0; i < grvDanhSach.Columns.Count-4; i++)
                {
                    col = grvDanhSach.Columns[i];
                    s += "<td style='background-color:#CCC;'>" + col.HeaderText + "</td>";
                }
                s += "</tr>";
                for (int i = 0; i < grvDanhSach.Rows.Count; i++)
                {
                    row = grvDanhSach.Rows[i];
                    s += "<tr>";
                    TableCell cell = null;
                    for (int j = 0; j < row.Cells.Count-4; j++)
                    {
                        cell = row.Cells[j];
                        if (j == 2)
                        {
                            Label l = (Label)(cell.FindControl("lblOldValue"));
                            s += "<td>" + l.Text + "</td>";
                        }
                        else if (j == 3)
                        {
                            Label l = (Label)(cell.FindControl("lblNote"));
                            s += "<td>" + l.Text + "</td>";
                        }
                        else
                        {
                            s += "<td>" + cell.Text + "</td>";
                        }
                    }
                    s += "</tr>";
                }
                s += "</table><br/><br/>";
                //-------------------------------------------------
                if (grvSinhSan.Rows.Count > 0)
                {
                    s += "<table border='1'>";
                    s += "<tr><td colspan='" + grvSinhSan.Columns.Count.ToString() + "'><b>THỐNG KÊ SINH SẢN</b></td></tr>";
                    s += "<tr>";
                    for (int i = 0; i < grvSinhSan.Columns.Count; i++)
                    {
                        col = grvSinhSan.Columns[i];
                        s += "<td style='background-color:#CCC;'>" + col.HeaderText + "</td>";
                    }
                    s += "</tr>";
                    for (int i = 0; i < grvSinhSan.Rows.Count; i++)
                    {
                        row = grvSinhSan.Rows[i];
                        s += "<tr>";
                        TableCell cell = null;
                        for (int j = 0; j < row.Cells.Count; j++)
                        {
                            cell = row.Cells[j];
                            s += "<td>" + cell.Text + "</td>";
                        }
                        s += "</tr>";
                    }
                    s += "</table>";
                }
                //-------------------------------------------------
                s += "</body></html>";
                Response.Write(s);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnSaveGhiChu_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["IDCaSau"] != null)
                csCont.CaSau_UpdateGhiChu(int.Parse(Request.QueryString["IDCaSau"]), Server.HtmlDecode(txtGhiChu.Text.Trim()));
        }
    }
}