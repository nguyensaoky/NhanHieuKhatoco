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
using System.Data.SqlClient;
using System.Text;
using System.IO;

namespace DotNetNuke.Modules.QLCS
{
    public partial class casau_chinhsuacachet : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        DataTable tblLDC = null;
        string sVatTuGietMo = ConfigurationManager.AppSettings["QLCS_VatTuGietMo"];
        DataTable tblVatTu = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tblVatTu = csCont.VatTu_GetByString(sVatTuGietMo);
                if (!IsPostBack)
                {
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
                    ddlChuong.Items.Insert(0, new ListItem("", "0"));

                    DataTable dtCaMe = csCont.LoadCaSauMe_AllTrangThai();
                    ddlCaMe.DataSource = dtCaMe;
                    ddlCaMe.DataTextField = "CaMe";
                    ddlCaMe.DataValueField = "IDCaSau";
                    ddlCaMe.DataBind();
                    ddlCaMe.Items.Insert(0, new ListItem("", "0"));

                    DataTable dtBienBan = csCont.GetBienBanCaChet();
                    lstBienBan.DataSource = dtBienBan;
                    lstBienBan.DataTextField = "BienBan";
                    lstBienBan.DataValueField = "BienBan";
                    lstBienBan.DataBind();
                    lstBienBan.Items.Insert(0, new ListItem("", "0"));

                    if (Session["AutoDisplay_CSChet"] != null && Convert.ToBoolean(Session["AutoDisplay_CSChet"]))
                    {
                        txtID.Text = Session["CSChet_ID"].ToString();
                        txtMaSo.Text = Session["CSChet_MaSo"].ToString();
                        ddlGioiTinh.SelectedValue = Session["CSChet_GioiTinh"].ToString();
                        Config.SetSelectedValues(ddlLoaiCa, Session["CSChet_LoaiCa"].ToString());
                        Config.SetSelectedValues(ddlChuong, Session["CSChet_Chuong"].ToString());
                        Config.SetSelectedValues(ddlCaMe, Session["CSChet_CaMe"].ToString());
                        Config.SetSelectedValues(lstBienBan, Session["CSChet_BienBan"].ToString());
                        btnLoad_Click(null, null);
                    }

                    if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "KhoaCS"))
                    {
                        btnKhoa.Visible = false;
                        btnMoKhoa.Visible = false;
                        grvDanhSach.Columns[0].Visible = false;
                    }

                    if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                    {
                        hdCanEdit.Value = "0";
                        grvDanhSach.Columns[20].Visible = false;
                        grvDanhSach.Columns[22].Visible = false;
                        grvDanhSach.Columns[23].Visible = false;
                    }
                    else
	                {
                        hdCanEdit.Value = "1";
	                }
                    DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
                    hdListBienDongPage.Value = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauListBienDongPage"], PortalId).TabID.ToString();
                }
                else
                {
                    foreach (GridViewRow r in grvDanhSach.Rows)
                    {
                        PlaceHolder dsVatTu = (PlaceHolder)(r.FindControl("dsVatTu"));
                        foreach (DataRow rr in tblVatTu.Rows)
                        {
                            Label lblIDVatTu = new Label();
                            lblIDVatTu.ID = "lblIDVatTu" + rr["IDVatTu"].ToString();
                            lblIDVatTu.Text = "<div style='float:left;text-align:left;'>" + rr["TenVatTu"].ToString() + "</div>";
                            dsVatTu.Controls.Add(lblIDVatTu);
                            TextBox txt = new TextBox();
                            txt.ID = rr["IDVatTu"].ToString();
                            txt.Attributes["style"] = "float:left;text-align:right;width:30px;margin-left:3px;";
                            txt.Text = "0";
                            dsVatTu.Controls.Add(txt);

                            Label lblXuongDong = new Label();
                            lblXuongDong.Text = "<br/";
                            dsVatTu.Controls.Add(lblXuongDong);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void LoadStringVatTuToControls(string sVatTu, PlaceHolder dsVatTu)
        {
            if (sVatTu != "")
            {
                string[] aVatTu = sVatTu.Substring(1, sVatTu.Length - 2).Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string VatTuGroup in aVatTu)
                {
                    string[] aVatTuGroup = VatTuGroup.Split(new char[] { '/' });
                    Control txtVatTu = dsVatTu.FindControl(aVatTuGroup[0]);
                    if (txtVatTu != null)
                        ((TextBox)txtVatTu).Text = aVatTuGroup[1];
                }
            }
        }

        protected void grvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(e.Row.FindControl("chkChon"));
                Button btnXemThayDoi = (Button)(e.Row.FindControl("btnXemThayDoi"));
                HyperLink lnkIDCaSau = (HyperLink)(e.Row.FindControl("lnkIDCaSau"));
                TextBox txtNgayChet = (TextBox)(e.Row.FindControl("txtNgayChet"));
                txtNgayChet.Text = Convert.ToDateTime(r["LatestChange"]).ToString("dd/MM/yyyy HH:mm:ss");
                TextBox txtBienBan = (TextBox)(e.Row.FindControl("txtBienBan"));
                txtBienBan.Text = r["BienBan"].ToString();
                lnkIDCaSau.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdListBienDongPage.Value), "", "IDCaSau/" + r["IDCaSau"].ToString()) + "','',800,600);";
                lnkIDCaSau.Attributes["style"] = "cursor:pointer;font-weight:bold;";
                HyperLink lnkCaMe = (HyperLink)(e.Row.FindControl("lnkCaMe"));
                lnkCaMe.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdListBienDongPage.Value), "", "IDCaSau/" + r["CaMe"].ToString()) + "','',800,600);";
                lnkCaMe.Attributes["style"] = "cursor:pointer;font-weight:bold;";
                TextBox txtDa_Bung = (TextBox)(e.Row.FindControl("txtDa_Bung"));
                DropDownList ddlDa_PhanLoai = (DropDownList)(e.Row.FindControl("ddlDa_PhanLoai"));
                CheckBox chkDau = (CheckBox)(e.Row.FindControl("chkDau"));
                Button btnSave = (Button)(e.Row.FindControl("btnSave"));
                Button btnSaveNgayChet = (Button)(e.Row.FindControl("btnSaveNgayChet"));
                Button btnPhucHoi = (Button)(e.Row.FindControl("btnPhucHoi"));
                DropDownList ddlPPM = (DropDownList)(e.Row.FindControl("ddlPPM"));
                DropDownList ddlHinhThucChet = (DropDownList)(e.Row.FindControl("ddlHinhThucChet"));
                DropDownList ddlLyDoChet = (DropDownList)(e.Row.FindControl("ddlLyDoChet"));
                ddlLyDoChet.DataSource = tblLDC;
                ddlLyDoChet.DataTextField = "TenLyDoChet";
                ddlLyDoChet.DataValueField = "IDLyDoChet";
                ddlLyDoChet.DataBind();
                ddlLyDoChet.Items.Insert(0, new ListItem("---","0"));
                TextBox txtKhoiLuong = (TextBox)(e.Row.FindControl("txtKhoiLuong"));
                txtDa_Bung.Text = r["Da_Bung"].ToString();
                ddlDa_PhanLoai.SelectedValue = r["Da_PhanLoai"].ToString();
                ddlPPM.SelectedValue = r["Note"].ToString();
                ddlHinhThucChet.SelectedValue = r["Status"].ToString();
                ddlLyDoChet.SelectedValue = r["LyDoChet"].ToString();
                txtKhoiLuong.Text = r["KhoiLuong"].ToString();
                chkDau.Checked = Convert.ToBoolean(r["Dau"]);
                btnSave.CommandArgument = e.Row.RowIndex.ToString() + ";" + r["IDCaSau"].ToString();
                btnSaveNgayChet.CommandArgument = e.Row.RowIndex.ToString() + ";" + r["IDCaSau"].ToString() + ";" + r["IDThuHoiDa"];
                btnPhucHoi.CommandArgument = r["IDCaSau"].ToString();
                if (hdCanEdit.Value == "1")
                {
                    if (Convert.ToDateTime(r["LatestChange"]) < Config.NgayKhoaSo())
                    {
                        //btnSave.Visible = false;
                        //btnPhucHoi.Visible = false;
                        btnSave.Enabled = false;
                        btnSave.CssClass = "buttondisable";
                        btnSaveNgayChet.Enabled = false;
                        btnSaveNgayChet.CssClass = "buttondisable";
                        btnPhucHoi.Enabled = false;
                        btnPhucHoi.CssClass = "buttondisable";
                    }
                    else
                    {
                        if (Convert.ToBoolean(r["Lock"]))
                        {
                            btnSave.Visible = false;
                            btnSaveNgayChet.Visible = false;
                            btnPhucHoi.Visible = false;
                        }
                        else
                        {
                            btnSave.Visible = true;
                            btnSaveNgayChet.Visible = true;
                            btnPhucHoi.Visible = true;
                        }
                    }
                }
                else
                {
                    btnSave.Visible = false;
                    btnSaveNgayChet.Visible = false;
                    btnPhucHoi.Visible = false;
                }
                btnXemThayDoi.CommandArgument = r["IDThuHoiDa"].ToString();
                chkChon.Value = r["IDThuHoiDa"].ToString();

                if (Convert.ToBoolean(r["Lock"]))
                {
                    e.Row.CssClass = "GrayRow";
                }
                else
                {
                    e.Row.CssClass = "NormalRow";
                }
                btnXemThayDoi.Attributes["style"] = "background-image:url('" + ModulePath + "images/log.gif');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;width:16px;";
                btnSave.Attributes["style"] = "background-image:url('" + ModulePath + "images/save.gif');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;width:16px;";
                btnSaveNgayChet.Attributes["style"] = "background-image:url('" + ModulePath + "images/savedate.gif');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;width:16px;";
                btnPhucHoi.Attributes["style"] = "background-image:url('" + ModulePath + "images/restore.png');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;width:16px;";
                e.Row.Attributes["id"] = "row_" + btnXemThayDoi.ClientID;
                e.Row.Attributes["onclick"] = "setSelectedRow(this,'" + btnXemThayDoi.ClientID + "')";
                if (ddlRowStatus.SelectedValue == "0")
                {
                    btnPhucHoi.Visible = false;
                    btnSave.Visible = false;
                    btnSaveNgayChet.Visible = false;
                }

                PlaceHolder dsVatTu = (PlaceHolder)(e.Row.FindControl("dsVatTu"));
                foreach (DataRow rr in tblVatTu.Rows)
                {
                    Label lblIDVatTu = new Label();
                    lblIDVatTu.ID = "lblIDVatTu" + rr["IDVatTu"].ToString();
                    lblIDVatTu.Text = "<div style='float:left;text-align:left;'>" + rr["TenVatTu"].ToString() + "</div>";
                    dsVatTu.Controls.Add(lblIDVatTu);
                    TextBox txt = new TextBox();
                    txt.ID = rr["IDVatTu"].ToString();
                    txt.Attributes["style"] = "float:left;text-align:right;width:30px;margin-left:3px;";
                    txt.Text = "0";
                    dsVatTu.Controls.Add(txt);

                    Label lblXuongDong = new Label();
                    lblXuongDong.Text = "<br/";
                    dsVatTu.Controls.Add(lblXuongDong);
                }
                LoadStringVatTuToControls(r["VatTu"].ToString(), dsVatTu);
            }
        }

        protected void grvHidden_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                Label lblDa_PhanLoai = (Label)(e.Row.FindControl("lblDa_PhanLoai"));
                Label lblHinhThucChet = (Label)(e.Row.FindControl("lblHinhThucChet"));
                lblDa_PhanLoai.Text = r["Da_PhanLoai"].ToString() == "6" ? "CXĐ" : r["Da_PhanLoai"].ToString();
                if(r["Status"].ToString() == "-1")
                    lblHinhThucChet.Text = "Chết";
                else if(r["Status"].ToString() == "-4")
                    lblHinhThucChet.Text = "Loại thải";
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
            if (strBienDong != "")
            {
                csCont.Lock_ThuHoiDa(strBienDong, true);
                btnLoad_Click(null, null);
            }
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
            if (strBienDong != "")
            {
                csCont.Lock_ThuHoiDa(strBienDong, false);
                btnLoad_Click(null, null);
            }
        }

        protected void grvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            tblLDC = csCont.LoadLyDoChet(1);
        }

        protected void grvDanhSach_Sorting(object sender, GridViewSortEventArgs e)
        {
            tblLDC = csCont.LoadLyDoChet(1);
            hdOrderBy.Value = e.SortExpression;
        }

        private string LoadODSParameters()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            string strWhereClause = "1=1";
            if (ddlHinhThucChet.SelectedValue != "0")
            {
                strWhereClause += " and cs.Status = " + ddlHinhThucChet.SelectedValue;
            }
            if (txtID.Text.Trim() != "")
            {
                strWhereClause += " and cs.IDCaSau = '" + txtID.Text + "'";
            }
            if (txtMaSo.Text.Trim() != "")
            {
                strWhereClause += " and cs.MaSo = '" + txtMaSo.Text + "'";
            }
            if (ddlGioiTinh.SelectedValue != "-2")
            {
                strWhereClause += " and cs.GioiTinh = " + ddlGioiTinh.SelectedValue;
            }
            if (ddlGiong.SelectedValue != "-1")
            {
                strWhereClause += " and cs.Giong = " + ddlGiong.SelectedValue;
            }
            if (Config.GetSelectedValues(lstBienBan) != "0, " && Config.GetSelectedValues(lstBienBan) != "")
            {
                strWhereClause += " and thd.BienBan in (" + Config.GetSelectedValuesSQL(lstBienBan).Remove(Config.GetSelectedValuesSQL(lstBienBan).Length - 2) + ")";
            }
            if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
            {
                strWhereClause += " and cs.LoaiCa in (" + Config.GetSelectedValues(ddlLoaiCa).Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 2) + ")";
            }
            if (Config.GetSelectedValues(ddlChuong) != "0, " && Config.GetSelectedValues(ddlChuong) != "")
            {
                strWhereClause += " and cs.Chuong in (" + Config.GetSelectedValues(ddlChuong).Remove(Config.GetSelectedValues(ddlChuong).Length - 2) + ")";
            }
            if (Config.GetSelectedValues(ddlCaMe) != "0, " && Config.GetSelectedValues(ddlCaMe) != "")
            {
                strWhereClause += " and cs.CaMe in (" + Config.GetSelectedValues(ddlCaMe).Remove(Config.GetSelectedValues(ddlCaMe).Length - 2) + ")";
            }
            if (txtFromDate.Text != "")
            {
                strWhereClause += " and cs.NgayXuongChuong >= '" + DateTime.Parse(txtFromDate.Text, ci).ToString("yyyyMMdd") + "'";
            }
            if (txtToDate.Text != "")
            {
                strWhereClause += " and cs.NgayXuongChuong < '" + DateTime.Parse(txtToDate.Text, ci).AddDays(1).ToString("yyyyMMdd") + "'";
            }
            if (txtNgayChetFrom.Text != "")
            {
                strWhereClause += " and cs.LatestChange >= '" + DateTime.Parse(txtNgayChetFrom.Text, ci).ToString("yyyyMMdd") + "'";
            }
            if (txtNgayChetTo.Text != "")
            {
                strWhereClause += " and cs.LatestChange < '" + DateTime.Parse(txtNgayChetTo.Text, ci).AddDays(1).ToString("yyyyMMdd") + "'";
            }
            return strWhereClause;
        }

        private string LoadODSParametersDelete()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            string strWhereClause = "1=1";
            if (ddlHinhThucChet.SelectedValue != "0")
            {
                strWhereClause += " and c.Status = '" + ddlHinhThucChet.SelectedValue + "'";
            }
            if (txtID.Text.Trim() != "")
            {
                strWhereClause += " and cs.IDCaSau = '" + txtID.Text + "'";
            }
            if (txtMaSo.Text.Trim() != "")
            {
                strWhereClause += " and cs.MaSo = '" + txtMaSo.Text + "'";
            }
            if (ddlGioiTinh.SelectedValue != "-2")
            {
                strWhereClause += " and cs.GioiTinh = " + ddlGioiTinh.SelectedValue;
            }
            if (ddlGiong.SelectedValue != "-1")
            {
                strWhereClause += " and cs.Giong = " + ddlGiong.SelectedValue;
            }
            if (Config.GetSelectedValues(lstBienBan) != "0, " && Config.GetSelectedValues(lstBienBan) != "")
            {
                strWhereClause += " and thd.BienBan in (" + Config.GetSelectedValuesSQL(lstBienBan).Remove(Config.GetSelectedValuesSQL(lstBienBan).Length - 2) + ")";
            }
            if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
            {
                strWhereClause += " and cs.LoaiCa in (" + Config.GetSelectedValues(ddlLoaiCa).Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 2) + ")";
            }
            if (Config.GetSelectedValues(ddlChuong) != "0, " && Config.GetSelectedValues(ddlChuong) != "")
            {
                strWhereClause += " and cs.Chuong in (" + Config.GetSelectedValues(ddlChuong).Remove(Config.GetSelectedValues(ddlChuong).Length - 2) + ")";
            }
            if (Config.GetSelectedValues(ddlCaMe) != "0, " && Config.GetSelectedValues(ddlCaMe) != "")
            {
                strWhereClause += " and cs.CaMe in (" + Config.GetSelectedValues(ddlCaMe).Remove(Config.GetSelectedValues(ddlCaMe).Length - 2) + ")";
            }
            if (txtFromDate.Text != "")
            {
                strWhereClause += " and cs.NgayXuongChuong >= '" + DateTime.Parse(txtFromDate.Text, ci).ToString("yyyyMMdd") + "'";
            }
            if (txtToDate.Text != "")
            {
                strWhereClause += " and cs.NgayXuongChuong < '" + DateTime.Parse(txtToDate.Text, ci).AddDays(1).ToString("yyyyMMdd") + "'";
            }
            if (txtNgayChetFrom.Text != "")
            {
                strWhereClause += " and c.Ngay >= '" + DateTime.Parse(txtNgayChetFrom.Text, ci).ToString("yyyyMMdd") + "'";
            }
            if (txtNgayChetTo.Text != "")
            {
                strWhereClause += " and c.Ngay < '" + DateTime.Parse(txtNgayChetTo.Text, ci).AddDays(1).ToString("yyyyMMdd") + "'";
            }
            return strWhereClause;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            tblLDC = csCont.LoadLyDoChet(1);
            Session["AutoDisplay_CSChet"] = true;
            Session["CSChet_ID"] = txtID.Text;
            Session["CSChet_MaSo"] = txtMaSo.Text;
            Session["CSChet_GioiTinh"] = ddlGioiTinh.SelectedValue;
            Session["CSChet_LoaiCa"] = Config.GetSelectedValues(ddlLoaiCa);
            Session["CSChet_Chuong"] = Config.GetSelectedValues(ddlChuong);
            Session["CSChet_CaMe"] = Config.GetSelectedValues(ddlCaMe);
            Session["CSChet_BienBan"] = Config.GetSelectedValues(lstBienBan);

            grvDanhSach.PageSize = int.Parse(ddlPageSize.SelectedValue);
            if (ddlRowStatus.SelectedValue == "1")
            {
                grvDanhSach.Columns[17].SortExpression = "[Status] DESC";
                grvDanhSach.Columns[21].SortExpression = "[LatestChange] DESC";
                string strWhereClause = LoadODSParameters();
                grvDanhSach.DataSourceID = "odsDanhSach";
                odsDanhSach.SelectParameters["WhereClause"].DefaultValue = strWhereClause;
                lblTongSo.Text = CaSauDataObject.CountCaChet_HienTai(strWhereClause).ToString();
            }
            else
            {
                grvDanhSach.Columns[17].SortExpression = "[c.Status] DESC";
                grvDanhSach.Columns[21].SortExpression = "[c.Ngay] DESC";
                string strWhereClause = LoadODSParametersDelete();
                grvDanhSach.DataSourceID = "odsDanhSachDelete";
                odsDanhSachDelete.SelectParameters["WhereClause"].DefaultValue = strWhereClause;
                lblTongSo.Text = CaSauDataObject.CountCaChetDelete(strWhereClause).ToString();
                btnKhoa.Visible = false;
                btnMoKhoa.Visible = false;
            }
        }

        private string CreateSVatTu(PlaceHolder dsVatTu)
        {
            string sRes = "";
            foreach (Control c in dsVatTu.Controls)
            {
                if (c.GetType().Equals(typeof(TextBox)))
                {
                    sRes += "@" + c.ID + "/" + ((TextBox)c).Text + "@";
                }
            }
            return sRes;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string[] s = ((Button)sender).CommandArgument.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            int i = int.Parse(s[0]);
            int idcasau = int.Parse(s[1]);
            GridViewRow r = grvDanhSach.Rows[i];
            TextBox txtDa_Bung = (TextBox)r.FindControl("txtDa_Bung");
            DropDownList ddlDa_PhanLoai = (DropDownList)r.FindControl("ddlDa_PhanLoai");
            DropDownList ddlPPM = (DropDownList)r.FindControl("ddlPPM");
            DropDownList ddlLyDoChet = (DropDownList)(r.FindControl("ddlLyDoChet"));
            TextBox txtKhoiLuong = (TextBox)(r.FindControl("txtKhoiLuong"));
            TextBox txtBienBan = (TextBox)(r.FindControl("txtBienBan"));
            if (txtKhoiLuong.Text == "") txtKhoiLuong.Text = "0";
            CheckBox chkDau = (CheckBox)r.FindControl("chkDau");
            PlaceHolder dsVatTu = (PlaceHolder)r.FindControl("dsVatTu");
            string sVatTu = CreateSVatTu(dsVatTu);
            int res = csCont.CaChet_UpdateThongSo(idcasau, int.Parse(txtDa_Bung.Text), int.Parse(ddlDa_PhanLoai.SelectedValue), Convert.ToInt32(chkDau.Checked), ddlPPM.SelectedValue, int.Parse(ddlLyDoChet.SelectedValue), decimal.Parse(txtKhoiLuong.Text.Trim()), txtBienBan.Text, UserId, sVatTu);
            if (res == 1) Page.ClientScript.RegisterStartupScript(typeof(string), "savesuccess", "alert('Lưu thành công');", true);
            else Page.ClientScript.RegisterStartupScript(typeof(string), "savefail", "alert('Lưu không thành công');", true);
        }

        protected void btnSaveNgayChet_Click(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            string[] s = ((Button)sender).CommandArgument.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            int i = int.Parse(s[0]);
            int idcasau = int.Parse(s[1]);
            int thuhoida = int.Parse(s[2]);
            GridViewRow r = grvDanhSach.Rows[i];
            TextBox txtNgayChet = (TextBox)(r.FindControl("txtNgayChet"));
            DropDownList ddlHinhThucChet = (DropDownList)(r.FindControl("ddlHinhThucChet"));
            int res = csCont.CaChet_UpdateNgayChet(idcasau, thuhoida, DateTime.Parse(txtNgayChet.Text, ci), UserId, ddlHinhThucChet.SelectedValue);
            if (res == 1) Page.ClientScript.RegisterStartupScript(typeof(string), "savesuccess", "alert('Lưu thành công');", true);
            else Page.ClientScript.RegisterStartupScript(typeof(string), "savefail", "alert('Lưu không thành công," + res.ToString() + "');", true);
        }

        protected void btnPhucHoi_Click(object sender, EventArgs e)
        {
            int idcasau = int.Parse(((Button)sender).CommandArgument);
            int res = csCont.CaChet_PhucHoi(idcasau, UserId);
            if (res == 1)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "savesuccess", "alert('Phục hồi thành công');", true);
                btnLoad_Click(null, null);
            }
            else Page.ClientScript.RegisterStartupScript(typeof(string), "savefail", "alert('Phục hồi không thành công');", true);
        }

        protected void btnXemThayDoi_Click(object sender, EventArgs e)
        {
            DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
            int ThuHoiDaLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_ThuHoiDaLichSuPage"], PortalId).TabID;
            //Session["EditBienDongCaSauParam"] = ((Button)sender).CommandArgument;
            Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(ThuHoiDaLichSuPage, "", "status/" + ddlRowStatus.SelectedValue, "IDThuHoiDa/" + ((Button)sender).CommandArgument) + "','',800,400);</script>", false);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");

                string strSQL = "QLCS_GetCaSauChet_HienTai";
                SqlParameter[] param = new SqlParameter[2];
                string filename = "CaChet_HienTai.xls";
                param[0] = new SqlParameter("@WhereClause", LoadODSParameters());
                param[1] = new SqlParameter("@OrderBy", hdOrderBy.Value);
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                grvHidden.DataSource = dt;
                grvHidden.DataBind();

                StringWriter sw = new StringWriter();
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