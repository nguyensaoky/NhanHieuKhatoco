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
    public partial class vattu_lichsubiendong : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        int scale = 0;
        DataTable tblNhaCungCap = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["loaivattu"] != null)
                {
                    string loaivattu = Request.QueryString["loaivattu"];
                    if (loaivattu == "TA") scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_TA_Scale"]);
                    else if (loaivattu == "TTY") scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_TTY_Scale"]);
                    else if (loaivattu == "DCS") scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_DCS_Scale"]);
                    else if (loaivattu == "DCS_") scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_DCS__Scale"]);
                    else if (loaivattu == "DCS_CB") scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_DCS_CB_Scale"]);
                    else if (loaivattu == "DCS_CL") scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_DCS_CL_Scale"]);
                    else if (loaivattu == "DCS_MDL") scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_DCS_MDL_Scale"]);
                    else if (loaivattu == "SPGM") scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_SPGM_Scale"]);
                }
                if (!IsPostBack)
                {
                    lnkCancel.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "");
                    if(Request.QueryString["IDVatTu"] != null)
                    {
                        int IDVatTu = int.Parse(Request.QueryString["IDVatTu"]);
                        DataTable tblChiTiet = csCont.LoadVatTu_HienTaiByID(IDVatTu);
                        if (tblChiTiet.Rows.Count > 0)
                        {
                            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
                            lblVatTu.Text = tblChiTiet.Rows[0]["TenVatTu"].ToString() + " (Tồn hiện tại: " + Config.ToXVal2(tblChiTiet.Rows[0]["SoLuong"],scale) + " " + tblChiTiet.Rows[0]["DonViTinh"].ToString() + ")";
                            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                            grvDanhSach.Columns[1].HeaderText += " (" + tblChiTiet.Rows[0]["DonViTinh"].ToString() + ")";
                            grvDanhSach.Columns[3].HeaderText += " (" + tblChiTiet.Rows[0]["DonViTinh"].ToString() + ")";
                            grvDanhSach.Columns[4].HeaderText += " (" + tblChiTiet.Rows[0]["DonViTinh"].ToString() + ")";
                            btnLoad_Click(null, null);
                        }
                        else
                        {
                            btnLoad.Visible = false;
                        }
                        if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "KhoaCS"))
                        {
                            btnKhoa.Visible = false;
                            btnMoKhoa.Visible = false;
                        }
                        if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                        {
                            grvDanhSach.Columns[6].Visible = false;
                        }
                    }
                    else
                    {
                        btnLoad.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            if (txtFromDate.Text == "" || txtToDate.Text == "")
            {
                lblMessage.Text = "Bạn chưa nhập ngày";
                lblMessage.Visible = true;
            }
            else
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                tblNhaCungCap = csCont.LoadNhaCungCapByVatTu(int.Parse(Request.QueryString["IDVatTu"]));
                DataTable tblBienDong = csCont.VatTu_GetVatTuBienDongByIDVatTu(int.Parse(Request.QueryString["IDVatTu"]), int.Parse(ddlRowStatus.SelectedValue), DateTime.Parse(txtFromDate.Text, ci), DateTime.Parse(txtToDate.Text, ci));
                grvDanhSach.DataSource = tblBienDong;
                grvDanhSach.DataBind();
            }
        }

        protected void btnKhoa_Click(object sender, EventArgs e)
        {
            string strBienDongVatTu = "";
            foreach (GridViewRow row in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                if (chkChon.Checked)
                {
                    strBienDongVatTu += "@" + chkChon.Value + "@";
                }
            }
            csCont.Lock_VatTu_BienDong(strBienDongVatTu, true);
            btnLoad_Click(null, null);
        }

        protected void btnMoKhoa_Click(object sender, EventArgs e)
        {
            string strBienDongVatTu = "";
            foreach (GridViewRow row in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                if (chkChon.Checked)
                {
                    strBienDongVatTu += "@" + chkChon.Value + "@";
                }
            }
            csCont.Lock_VatTu_BienDong(strBienDongVatTu, false);
            btnLoad_Click(null, null);
        }

        protected void grvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(e.Row.FindControl("chkChon"));
                chkChon.Value = r["IDBienDong"].ToString();
                Button btnXemThayDoi = (Button)(e.Row.FindControl("btnXemThayDoi"));
                btnXemThayDoi.CommandArgument = r["IDBienDong"].ToString();
                btnXemThayDoi.CommandName = ddlRowStatus.SelectedValue;
                TextBox txtSoLuongBienDong = (TextBox)(e.Row.FindControl("txtSoLuongBienDong"));
                txtSoLuongBienDong.Text = Convert.ToDecimal(r["SoLuongBienDong"]).ToString("0.#####");
                DropDownList ddlNhaCungCap = (DropDownList)(e.Row.FindControl("ddlNhaCungCap"));
                Button btnSave = (Button)(e.Row.FindControl("btnSave"));
                Label lblSoLuongTruocDo = (Label)(e.Row.FindControl("lblSoLuongTruocDo"));
                Label lblSoLuongBienDong = (Label)(e.Row.FindControl("lblSoLuongBienDong"));
                Label lblSoLuongSauDo = (Label)(e.Row.FindControl("lblSoLuongSauDo"));
                Label lblNguon = (Label)(e.Row.FindControl("lblNguon"));
                if (r["Ref1"] != DBNull.Value) lblNguon.Text = r["Ref1"].ToString().Substring(0, r["Ref1"].ToString().IndexOf('_'));
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    btnSave.Visible = false;
                    txtSoLuongBienDong.Visible = false;
                    ddlNhaCungCap.Visible = false;
                }
                else
                {
                    if (Convert.ToDateTime(r["NgayBienDong"]) < Config.NgayKhoaSo())
                    {
                        //btnSave.Visible = false;
                        //txtSoLuongBienDong.Visible = false;
                        btnSave.Enabled = false;
                        btnSave.CssClass = "buttondisable";
                        txtSoLuongBienDong.Enabled = false;
                        ddlNhaCungCap.Visible = false;
                    }
                    else
                    {
                        if (r["Ref1"].ToString() == "")
                        {
                            btnSave.CommandArgument = e.Row.RowIndex.ToString() + ";" + r["IDBienDong"].ToString();
                        }
                        else
                        {
                            btnSave.Visible = false;
                            txtSoLuongBienDong.Visible = false;
                            ddlNhaCungCap.Visible = false;
                        }
                    }
                }
                decimal slbd = Convert.ToDecimal(r["SoLuongBienDong"]);
                decimal slsd = Convert.ToDecimal(r["SoLuongSauDo"]);
                decimal sltd = 0;
                //lblSoLuongBienDong.Text = slbd.ToString("0.#####");
                //lblSoLuongSauDo.Text = slsd.ToString("0.#####");
                lblSoLuongBienDong.Text = Config.ToXVal2(slbd,scale);
                lblSoLuongSauDo.Text = Config.ToXVal2(slsd,scale);
                if(r["Tang_Giam"].ToString() == "1")
                {
                    sltd = slsd - slbd;
                }
                else
                {
                    sltd = slsd + slbd;
                }
                //lblSoLuongTruocDo.Text = sltd.ToString("0.#####");
                lblSoLuongTruocDo.Text = Config.ToXVal2(sltd,scale);
                if (Convert.ToBoolean(r["Lock"]))
                {
                    e.Row.CssClass = "GrayRow";
                    txtSoLuongBienDong.Visible = false;
                    ddlNhaCungCap.Visible = false;
                    btnSave.Visible = false;
                }
                else
                {
                    e.Row.CssClass = "NormalRow";
                    if (ddlRowStatus.SelectedValue == "0")
                    {
                        txtSoLuongBienDong.Visible = false;
                        ddlNhaCungCap.Visible = false;
                        btnSave.Visible = false;
                    }
                }
                if (r["Tang_Giam"].ToString() == "1" && tblNhaCungCap.Rows.Count > 0)
                {
                    ddlNhaCungCap.DataSource = tblNhaCungCap;
                    ddlNhaCungCap.DataTextField = "NhaCungCap";
                    ddlNhaCungCap.DataValueField = "ID";
                    ddlNhaCungCap.DataBind();
                    ddlNhaCungCap.Items.Insert(0, new ListItem("", "0"));
                    ddlNhaCungCap.SelectedValue = r["IDNhaCungCap"] == DBNull.Value ? "0" : r["IDNhaCungCap"].ToString();
                }
                else
                {
                    ddlNhaCungCap.Visible = false;
                }
                btnXemThayDoi.Attributes["style"] = "background-image:url('" + ModulePath + "images/log.gif');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;width:16px;";
                e.Row.Attributes["id"] = "row_" + btnXemThayDoi.ClientID;
                e.Row.Attributes["onclick"] = "setSelectedRow(this,'" + btnXemThayDoi.ClientID + "')";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Button btnSave = (Button)sender;
            string[] a = btnSave.CommandArgument.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            int rowIndex = int.Parse(a[0]);
            int IDBienDong = int.Parse(a[1]);
            TextBox txtSoLuongBienDong = (TextBox)(grvDanhSach.Rows[rowIndex].FindControl("txtSoLuongBienDong"));
            DropDownList ddlNhaCungCap = (DropDownList)(grvDanhSach.Rows[rowIndex].FindControl("ddlNhaCungCap"));
            int res;
            if(ddlNhaCungCap.Visible)
                res = csCont.VatTu_ThayDoiSoLuongBienDong(IDBienDong, decimal.Parse(txtSoLuongBienDong.Text), UserId, int.Parse(ddlNhaCungCap.SelectedValue));
            else
                res = csCont.VatTu_ThayDoiSoLuongBienDong(IDBienDong, decimal.Parse(txtSoLuongBienDong.Text), UserId, 0);
            if (res == 0)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "savefail", "alert('Số liệu không hợp lệ!');", true);
            }
            else
            {
                int IDVatTu = int.Parse(Request.QueryString["IDVatTu"]);
                DataTable tblChiTiet = csCont.LoadVatTu_HienTaiByID(IDVatTu);
                if (tblChiTiet.Rows.Count > 0)
                {
                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
                    lblVatTu.Text = tblChiTiet.Rows[0]["TenVatTu"].ToString() + "(Tồn hiện tại: " + Config.ToXVal2(tblChiTiet.Rows[0]["SoLuong"], scale) + " " + tblChiTiet.Rows[0]["DonViTinh"].ToString() + ")";
                }
                Page.ClientScript.RegisterStartupScript(typeof(string), "savesuccess", "alert('Lưu thành công!');", true);
                btnLoad_Click(null, null);
            }
        }

        protected void btnXemThayDoi_Click(object sender, EventArgs e)
        {
            DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
            int VatTuBienDongLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_VatTuBienDongLichSuPage"], PortalId).TabID;
            Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(VatTuBienDongLichSuPage, "", "IDVatTuBienDong/" + ((Button)sender).CommandArgument, "status/" + ((Button)sender).CommandName) + "','',1000,600);</script>", false);
        }
    }
}