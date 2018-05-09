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

namespace DotNetNuke.Modules.QLCS
{
    public partial class gietmoca_chonca : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
        IFormatProvider ci = new System.Globalization.CultureInfo("vi-VN", true);
        CaSauController csCont = new CaSauController();
        bool allowEdit = false;
        string sVatTuGietMo = ConfigurationManager.AppSettings["QLCS_VatTuGietMo"];
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
                DataTable tblVatTu = csCont.VatTu_GetByString(sVatTuGietMo);
                foreach (DataRow r in tblVatTu.Rows)
                {
                    Label lblIDVatTu = new Label();
                    lblIDVatTu.ID = "lblIDVatTu" + r["IDVatTu"].ToString();
                    lblIDVatTu.Text = "<div style='width:200px;float:left;text-align:left;'>" + r["TenVatTu"].ToString() + "</div>";
                    dsVatTu.Controls.Add(lblIDVatTu);
                    TextBox txt = new TextBox();
                    txt.ID = r["IDVatTu"].ToString();
                    txt.Attributes["style"] = "float:left;text-align:right;width:70px;";
                    txt.Text = "0";
                    dsVatTu.Controls.Add(txt);

                    Label lblDVT = new Label();
                    lblDVT.Text = "<div style='text-align:left;float:left;width:40px;'>&nbsp;" + r["DonViTinh"].ToString() + "</div>";
                    lblDVT.ID = "lblDVT" + r["IDVatTu"].ToString();
                    dsVatTu.Controls.Add(lblDVT);

                    Label lblXuongDong = new Label();
                    lblXuongDong.Text = "<div style='clear:both;'></div>";
                    dsVatTu.Controls.Add(lblXuongDong);
                }
                if (!IsPostBack)
                {
                    if (Request.QueryString["gietmoca"] != null)
                    {
                        lblGMC.Text = Request.QueryString["gietmoca"];
                    }
                    DataTable dtLoaiCa = csCont.LoadLoaiCa(1);
                    ddlLoaiCa.DataSource = dtLoaiCa;
                    ddlLoaiCa.DataTextField = "TenLoaiCa";
                    ddlLoaiCa.DataValueField = "IDLoaiCa";
                    ddlLoaiCa.DataBind();
                    ddlLoaiCa.Items.Insert(0, new ListItem("-----", "0"));

                    DataTable dtChuong = csCont.LoadChuong(1);
                    ddlChuong.DataSource = dtChuong;
                    ddlChuong.DataTextField = "Chuong";
                    ddlChuong.DataValueField = "IDChuong";
                    ddlChuong.DataBind();
                    ddlChuong.Items.Insert(0, new ListItem("-----", "0"));

                    DataTable dtCaMe = csCont.LoadCaSauMe_AllTrangThai();
                    ddlCaMe.DataSource = dtCaMe;
                    ddlCaMe.DataTextField = "CaMe";
                    ddlCaMe.DataValueField = "IDCaSau";
                    ddlCaMe.DataBind();
                    ddlCaMe.Items.Insert(0, new ListItem("-----", "0"));

                    DataTable dtNguoiMo = csCont.LoadNhanVien(1);
                    ddlNguoiMo.DataSource = dtNguoiMo;
                    ddlNguoiMo.DataTextField = "TenNhanVien";
                    ddlNguoiMo.DataValueField = "IDNhanVien";
                    ddlNguoiMo.DataBind();

                    //Xem xet co cho phep save va delete hay khong
                    DataTable tblGMC = csCont.GietMoCa_GetByID(int.Parse(lblGMC.Text));
                    if (Convert.ToDateTime(tblGMC.Rows[0]["NgayMo"]) < Config.NgayKhoaSo())
                    {
                        allowEdit = false;
                    }
                    else
                    {
                        if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                        {
                            allowEdit = false;
                        }
                        else
                        {
                            if (Convert.ToBoolean(tblGMC.Rows[0]["Lock"]))
                            {
                                allowEdit = false;
                            }
                            else
                            {
                                allowEdit = true;
                            }
                        }
                    }

                    if (Request.QueryString["gmcct"] != null)
                    {
                        SearchCa.Visible = false;
                        btnDelete.Visible = true;
                        lblGMCCT.Text = Request.QueryString["gmcct"];
                        DataTable dtGMCCT = csCont.GietMoCa_GetChiTietByChiTietID(int.Parse(lblGMCCT.Text), 1);
                        if (dtGMCCT.Rows.Count == 1)
                        {
                            DataRow r = dtGMCCT.Rows[0];
                            //lblGMC.Text = r["GietMoCa"].ToString();
                            txtIDCa.Text = r["Ca"].ToString();
                            txtDa_TrongLuong.Text = ((decimal)r["Da_TrongLuong"]).ToString("0.#####");
                            txtDa_Bung.Text = r["Da_Bung"].ToString();
                            ddlDa_PhanLoai.SelectedValue = r["Da_PhanLoai"].ToString();
                            chkDau.Checked = Convert.ToBoolean(r["Dau"]);
                            txtTrongLuongHoi.Text = ((decimal)r["TrongLuongHoi"]).ToString("0.#####");
                            txtTrongLuongMocHam.Text = ((decimal)r["TrongLuongMocHam"]).ToString("0.#####");
                            ddlPhuongPhapMo.SelectedValue = r["PhuongPhapMo"].ToString();
							ddlNguoiMo.SelectedValue = r["NguoiMo"].ToString();
                            LoadStringVatTuToControls(r["VatTu"].ToString(), dsVatTu);
                            if (Convert.ToInt32(r["Status"]) == -4)
                                chkDiTat.Checked = true;

                            if (allowEdit)
                            {
                                btnSave.Visible = true;
                                btnDelete.Visible = true;
                            }
                            else
                            {
                                btnSave.Visible = false;
                                btnDelete.Visible = false;
                            }
                        }
                        else
                        {
                            btnSave.Visible = false;
                            btnDelete.Visible = false;
                        }
                    }
                    else
                    {
                        btnDelete.Visible = false;
                        if (allowEdit)
                        {
                            btnSave.Visible = true;
                        }
                        else
                        {
                            btnSave.Visible = false;
                        }
                    }
                }
                hdListBienDongPage.Value = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauListBienDongPage"], PortalId).TabID.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void LoadStringVatTuToControls(string sVatTu, PlaceHolder dsVatTu)
        { 
            if(sVatTu != "")
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
                HyperLink lnkCaMe = (HyperLink)(e.Row.FindControl("lnkCaMe"));
                lnkCaMe.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdListBienDongPage.Value), "", "IDCaSau/" + r["CaMe"].ToString()) + "','',800,600);";
                lnkCaMe.Attributes["style"] = "cursor:pointer;font-weight:bold;";
                HyperLink btnChon = (HyperLink)(e.Row.FindControl("btnChon"));
                if (Request.QueryString["gmcct"] == null)
                {
                    btnChon.Attributes["onclick"] = "chon_click(" + r["IDCaSau"].ToString() + ");";
                    btnChon.Attributes["style"] = "cursor:pointer;";
                }
                else
                {
                    btnChon.Visible = false;
                }
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
            string strWhereClause = "(";
            int count = 0;
            foreach (ListItem i in cblStatus.Items)
            {
                if (i.Selected)
                {
                    if (strWhereClause == "(") strWhereClause += "Status = " + i.Value;
                    else strWhereClause += " or Status = " + i.Value;
                    count++;
                }
            }
            strWhereClause += ")";
            if (strWhereClause == "()")
            {
                return "";
            }
            if (count == cblStatus.Items.Count)
            {
                strWhereClause = "Status >= 0";
            }
            DataTable tblGMC = csCont.GietMoCa_GetByID(int.Parse(lblGMC.Text));
            string ngayMo = Convert.ToDateTime(tblGMC.Rows[0]["NgayMo"]).ToString("yyyy/MM/dd HH:mm:ss");
            strWhereClause += " and LatestChange < '" + ngayMo + "'";
            if (txtMaSo.Text.Trim() != "")
            {
                strWhereClause += " and MaSo = '" + txtMaSo.Text + "'";
            }
            if (ddlGiong.SelectedValue != "-1")
            {
                strWhereClause += " and Giong = " + ddlGiong.SelectedValue;
            }
            if (ddlGioiTinh.SelectedValue != "-2")
            {
                strWhereClause += " and GioiTinh = " + ddlGioiTinh.SelectedValue;
            }
            if (ddlLoaiCa.SelectedValue != "0")
            {
                strWhereClause += " and LoaiCa = " + ddlLoaiCa.SelectedValue;
            }
            if (ddlChuong.SelectedValue != "0")
            {
                strWhereClause += " and Chuong = " + ddlChuong.SelectedValue;
            }
            if (ddlCaMe.SelectedValue != "0")
            {
                strWhereClause += " and CaMe = " + ddlCaMe.SelectedValue;
            }
            if (txtFromDate.Text != "")
            {
                strWhereClause += " and NgayXuongChuong >= '" + DateTime.Parse(txtFromDate.Text, ci).ToString("yyyyMMdd") + "'";
            }
            if (txtToDate.Text != "")
            {
                strWhereClause += " and NgayXuongChuong < '" + DateTime.Parse(txtToDate.Text, ci).AddDays(1).ToString("yyyyMMdd") + "'";
            }
            if (txtNhapChuongFrom.Text != "")
            {
                strWhereClause += " and NgayNhapChuong >= '" + DateTime.Parse(txtNhapChuongFrom.Text, ci).ToString("yyyyMMdd") + "'";
            }
            if (txtNhapChuongTo.Text != "")
            {
                strWhereClause += " and NgayNhapChuong < '" + DateTime.Parse(txtNhapChuongTo.Text, ci).AddDays(1).ToString("yyyyMMdd") + "'";
            }
            return strWhereClause;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            grvDanhSach.DataSourceID = "odsDanhSach";
            grvDanhSach.PageSize = int.Parse(ddlPageSize.SelectedValue);
            string strWhereClause = LoadODSParameters();
            if (strWhereClause != "")
            {
                odsDanhSach.SelectParameters["WhereClause"].DefaultValue = strWhereClause;
                lblTongSo.Text = CaSauDataObject.Count_HienTai(strWhereClause).ToString();
                grvDanhSach.Visible = true;
            }
            else
            {
                grvDanhSach.Visible = false;
                lblTongSo.Text = "0";
            }
        }

        private string CreateSVatTu(bool full)
        {
            string sRes = "";
            int temp = 0;
            foreach (Control c in dsVatTu.Controls)
            {
                if (c.GetType().Equals(typeof(TextBox)))
                {
                    if (full)
                    {
                        sRes += "@" + c.ID + "/" + ((TextBox)c).Text + "@";
                    }
                    else
                    {
                        if (int.TryParse(((TextBox)c).Text, out temp))
                        {
                            if (temp > 0) sRes += "@" + c.ID + "/" + ((TextBox)c).Text + "@";
                        }
                    }
                }
            }
            return sRes;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string sVatTu = "";
                if (lblGMCCT.Text == "0")
                {
                    sVatTu = CreateSVatTu(false);
                    int res = csCont.GietMoCa_InsertChiTiet(int.Parse(lblGMC.Text), int.Parse(txtIDCa.Text), decimal.Parse(txtDa_TrongLuong.Text), int.Parse(txtDa_Bung.Text), int.Parse(ddlDa_PhanLoai.SelectedValue), Convert.ToInt32(chkDau.Checked), int.Parse(ddlNguoiMo.SelectedValue), decimal.Parse(txtTrongLuongHoi.Text), decimal.Parse(txtTrongLuongMocHam.Text), UserId, ddlPhuongPhapMo.SelectedValue, chkDiTat.Checked, sVatTu);
                    if (res == 1)
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "inserted", "<script language=javascript>alert('Đã thêm xong');window.opener.finishEdit();</script>", false);
                        btnLoad_Click(null, null);
                        txtIDCa.Text = "";    
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "inserted", "<script language=javascript>alert('Không thêm được! Thời điểm cập nhật da cá sấu không hợp lệ!');</script>", false);
                    }
                }
                else
                {
                    sVatTu = CreateSVatTu(true);
                    int res = csCont.GietMoCa_UpdateChiTiet(int.Parse(lblGMCCT.Text), int.Parse(lblGMC.Text), decimal.Parse(txtDa_TrongLuong.Text), int.Parse(txtDa_Bung.Text), int.Parse(ddlDa_PhanLoai.SelectedValue), Convert.ToInt32(chkDau.Checked), int.Parse(ddlNguoiMo.SelectedValue), decimal.Parse(txtTrongLuongHoi.Text), decimal.Parse(txtTrongLuongMocHam.Text), UserId, ddlPhuongPhapMo.SelectedValue, chkDiTat.Checked, sVatTu);
                    if (res == 1)
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã cập nhật xong');window.opener.finishEdit();self.close();</script>", false);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "notupdated", "<script language=javascript>alert('Không cập nhật được! Có thể số liệu không hợp lệ!');</script>", false);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int res = csCont.GietMoCa_DeleteChiTiet(int.Parse(lblGMCCT.Text), int.Parse(lblGMC.Text), int.Parse(txtIDCa.Text), UserId);
                if (res == 1)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã xóa xong');window.opener.finishEdit();self.close();</script>", false);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "notupdated", "<script language=javascript>alert('Không xóa được! Có thể số liệu vật tư da cá sấu không hợp lệ!');</script>", false);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}