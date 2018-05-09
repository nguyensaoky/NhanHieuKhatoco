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
    public partial class casau_chuyentrangthai : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        DataTable tblLyDoChet = null;
        string sVatTuGietMo = ConfigurationManager.AppSettings["QLCS_VatTuGietMo"];
        DataTable tblVatTu = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] != "chuyentrangthai" && Request.QueryString["type"] != "editchuyentrangthai" && Request.QueryString["type"] != "editchuyentrangthaigroup") { wrapper.Visible = false; return; }
            tblVatTu = csCont.VatTu_GetByString(sVatTuGietMo);
            if (!IsPostBack)
            {
                txtThoiDiemChuyen.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                if (Request.QueryString["type"] == "editchuyentrangthai")
                {
                    string[] arg = Session["EditBienDongCaSauParam"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    txtThoiDiemChuyen.Text = arg[2];
                    hdIDBienDong.Value = arg[0];
                    hdIDCaSau.Value = arg[1];

                    ddlTrangThai.SelectedValue = arg[3];
                    ListItem i = ddlTrangThai.Items.FindByValue("-1");
                    ddlTrangThai.Items.Remove(i);
                    ListItem i1 = ddlTrangThai.Items.FindByValue("-4");
                    ddlTrangThai.Items.Remove(i1);
                }
                else if (Request.QueryString["type"] == "editchuyentrangthaigroup")
                {
                    string[] arg = Session["EditBienDongCaSauGroupParam"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    txtThoiDiemChuyen.Text = arg[1];
                    hdIDBienDongGroup.Value = arg[0];
                    ddlTrangThai.Text = arg[2];

                    btnChuyen.Text = "Cập nhật toàn bộ biến động";
                    ListItem i = ddlTrangThai.Items.FindByValue("-1");
                    ddlTrangThai.Items.Remove(i);
                    ListItem i1 = ddlTrangThai.Items.FindByValue("-4");
                    ddlTrangThai.Items.Remove(i1);
                    btnChuyenChon.Visible = true;

                    grvDanhSach.Visible = true;
                    DataTable tblCa = csCont.GetDanhSachCaSauBienDongByBienDongGroup(int.Parse(hdIDBienDongGroup.Value));
                    grvDanhSach.DataSource = tblCa;
                    grvDanhSach.DataBind();
                }
                ddlTrangThai_SelectedIndexChanged(null, null);
            }
            else
            {
                foreach (GridViewRow r in grvSPTH.Rows)
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

        protected void grvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(e.Row.FindControl("chkChon"));
                Label lblStatus = (Label)(e.Row.FindControl("lblStatus"));
                HyperLink lnkIDCaSau = (HyperLink)(e.Row.FindControl("lnkIDCaSau"));
                Label lblSTT = (Label)(e.Row.FindControl("lblSTT"));
                DotNetNuke.Entities.Tabs.TabController tabCont = new DotNetNuke.Entities.Tabs.TabController();
                lnkIDCaSau.ToolTip = r["ID"].ToString();
                lnkIDCaSau.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(tabCont.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauListBienDongPage"], PortalId).TabID, "", "IDCaSau/" + r["IDCaSau"].ToString()) + "','',800,600);";
                lnkIDCaSau.Attributes["style"] = "cursor:pointer;font-weight:bold;";
                chkChon.Attributes["name"] = r["IDCaSau"].ToString();
                chkChon.Attributes["onclick"] = "chon_click(event, this);";
                lblStatus.Text = r["TrangThai"].ToString();
                int t = e.Row.RowIndex + 1;
                lblSTT.Text = t.ToString();
            }
        }

        protected void grvSPTH_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                DropDownList ddlLyDoChet = (DropDownList)(e.Row.FindControl("ddlLyDoChet"));
                ddlLyDoChet.DataSource = tblLyDoChet;
                ddlLyDoChet.DataTextField = "TenLyDoChet";
                ddlLyDoChet.DataValueField = "IDLyDoChet";
                ddlLyDoChet.DataBind();
                ddlLyDoChet.Items.Insert(0, new ListItem("---", "0"));

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
            }
        }

        protected void grvDanhSach_Sorting(object sender, GridViewSortEventArgs e)
        {
        }

        private string CreateSVatTu(PlaceHolder dsVatTu, bool full)
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

        protected void btnChuyen_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] == "chuyentrangthai")
            {
                if (Session["DSCaSauChuyenTrangThai"] != null && Session["DSCaSauChuyenTrangThai"].ToString() != "")
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                    if (DateTime.Parse(txtThoiDiemChuyen.Text, culture) < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "chuyenfail", "<script language=javascript>alert('Ngày chuyển không được trước ngày khóa sổ');</script>", false);
                        return;
                    }
                    if (ddlTrangThai.SelectedValue == "-3")
                    {
                        string res = csCont.ChuyenTrangThai_CaBan(Session["DSCaSauChuyenTrangThai"].ToString(), DateTime.Parse(txtThoiDiemChuyen.Text, culture), UserId);
                        if (res != "") Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Một số cá không chuyển trạng thái được có ID: " + res.Substring(1, res.Length - 2).Replace("@@", ", ") + "');</script>", false);
                        else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                    }
                    else if (ddlTrangThai.SelectedValue == "-1" || ddlTrangThai.SelectedValue == "-4")
                    {
                        string StrDaBung = "";
                        string StrDaPhanLoai = "";
                        string StrDau = "";
                        string StrPPM = "";
                        string StrLDC = "";
                        string StrKL = "";
                        string LyDo = "";
                        string StrVatTu = "";
                        foreach (GridViewRow r in grvSPTH.Rows)
                        {
                            CheckBox chkDau = (CheckBox)(r.FindControl("chkDau"));
                            TextBox txtDaBung = (TextBox)(r.FindControl("txtDaBung"));
                            DropDownList ddlDaPhanLoai = (DropDownList)(r.FindControl("ddlDaPhanLoai"));
                            DropDownList ddlPPM = (DropDownList)(r.FindControl("ddlPPM"));
                            DropDownList ddlLyDoChet = (DropDownList)(r.FindControl("ddlLyDoChet"));
                            TextBox txtKhoiLuong = (TextBox)(r.FindControl("txtKhoiLuong"));
                            if (txtDaBung.Text == "")
                            {
                                txtDaBung.Text = "0";
                            }
                            StrDaBung += "@" + txtDaBung.Text + "@";
                            StrDaPhanLoai += "@" + ddlDaPhanLoai.SelectedValue + "@";
                            StrDau += "@" + Convert.ToInt32(chkDau.Checked).ToString() + "@";
                            StrPPM += "@" + ddlPPM.SelectedValue + "@";
                            StrLDC += "@" + ddlLyDoChet.SelectedValue + "@";
                            StrKL += "@" + txtKhoiLuong.Text.Replace(",", ".") + "@";
                            PlaceHolder dsVatTu = (PlaceHolder)(r.FindControl("dsVatTu"));
                            string sVatTu = CreateSVatTu(dsVatTu, false);
                            StrVatTu += "*" + sVatTu + "*";
                        }
                        string res = csCont.CaSauChet(Session["DSCaSauChuyenTrangThai"].ToString(), DateTime.Parse(txtThoiDiemChuyen.Text, culture), UserId, StrDaBung, StrDaPhanLoai, StrDau, StrPPM, StrLDC, StrKL, txtBienBan.Text.Trim(), ddlTrangThai.SelectedValue, StrVatTu);

                        if (res != "") Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Một số cá không chuyển trạng thái được có ID: " + res.Substring(1, res.Length - 2).Replace("@@", ", ") + "');</script>", false);
                        else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                    }
                    else
                    {
                        string res = csCont.ChuyenTrangThai_CaSong(Session["DSCaSauChuyenTrangThai"].ToString(), int.Parse(ddlTrangThai.SelectedValue), DateTime.Parse(txtThoiDiemChuyen.Text, culture), UserId);
                        if (res != "") Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Một số cá không chuyển trạng thái được có ID: " + res.Substring(1, res.Length - 2).Replace("@@", ", ") + "');</script>", false);
                        else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                    }
                }
            }
            else if (Request.QueryString["type"] == "editchuyentrangthai")
            {
                if (hdIDCaSau.Value != "0")
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                    if (DateTime.Parse(txtThoiDiemChuyen.Text, culture) < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "chuyenfail", "<script language=javascript>alert('Ngày chuyển không được trước ngày khóa sổ');</script>", false);
                        return;
                    }
                    if (ddlTrangThai.SelectedValue == "-3")
                    {
                        int res = csCont.EditChuyenTrangThai_CaBan(int.Parse(hdIDBienDong.Value), int.Parse(hdIDCaSau.Value), DateTime.Parse(txtThoiDiemChuyen.Text, culture), UserId);
                        if (res == 0) Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Không chuyển trạng thái được!');</script>", false);
                        else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                    }
                    else if (ddlTrangThai.SelectedValue == "-1" || ddlTrangThai.SelectedValue == "-4")
                    {
                        int RealDaBung = 0;
                        int RealDaPhanLoai = 0;
                        int RealDau = 0;
                        string RealPPM = "";
                        int RealLDC = 0;
                        decimal RealKL = 0;
                        string LyDo = "";
                        string sVatTu = "";
                        foreach (GridViewRow r in grvSPTH.Rows)
                        {
                            CheckBox chkDau = (CheckBox)(r.FindControl("chkDau"));
                            TextBox txtDaBung = (TextBox)(r.FindControl("txtDaBung"));
                            DropDownList ddlDaPhanLoai = (DropDownList)(r.FindControl("ddlDaPhanLoai"));
                            DropDownList ddlPPM = (DropDownList)(r.FindControl("ddlPPM"));
                            DropDownList ddlLyDoChet = (DropDownList)(r.FindControl("ddlLyDoChet"));
                            TextBox txtKhoiLuong = (TextBox)(r.FindControl("txtKhoiLuong"));
                            if (txtDaBung.Text == "")
                            {
                                txtDaBung.Text = "0";
                            }
                            RealDaBung = int.Parse(txtDaBung.Text);
                            RealDaPhanLoai = int.Parse(ddlDaPhanLoai.SelectedValue);
                            RealDau = Convert.ToInt32(chkDau.Checked);
                            RealPPM = ddlPPM.SelectedValue;
                            RealLDC = int.Parse(ddlLyDoChet.SelectedValue);
                            RealKL = decimal.Parse(txtKhoiLuong.Text.Trim());
                            PlaceHolder dsVatTu = (PlaceHolder)(r.FindControl("dsVatTu"));
                            sVatTu = CreateSVatTu(dsVatTu, false);
                        }
                        int res = csCont.EditCaSauChet(int.Parse(hdIDBienDong.Value), int.Parse(hdIDCaSau.Value), DateTime.Parse(txtThoiDiemChuyen.Text, culture), RealDaBung, RealDaPhanLoai, RealDau, RealPPM, RealLDC, RealKL, UserId, txtBienBan.Text.Trim(), ddlTrangThai.SelectedValue, sVatTu);
                        if (res == 0) Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Không chuyển trạng thái được!');</script>", false);
                        else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                    }
                    else
                    {
                        int res = csCont.EditChuyenTrangThai_CaSong(int.Parse(hdIDBienDong.Value), int.Parse(hdIDCaSau.Value), int.Parse(ddlTrangThai.SelectedValue), DateTime.Parse(txtThoiDiemChuyen.Text, culture), UserId);
                        if (res == 0) Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Không chuyển trạng thái được!');</script>", false);
                        else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                    }
                }
            }
            else if (Request.QueryString["type"] == "editchuyentrangthaigroup")
            {
                if (hdIDBienDongGroup.Value != "0")
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                    if (DateTime.Parse(txtThoiDiemChuyen.Text, culture) < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "chuyenfail", "<script language=javascript>alert('Ngày chuyển không được trước ngày khóa sổ');</script>", false);
                        return;
                    }
                    string res = csCont.UpdateBienDongGroup(int.Parse(hdIDBienDongGroup.Value), DateTime.Parse(txtThoiDiemChuyen.Text, culture), ddlTrangThai.SelectedValue, UserId);
                    if (res != "") Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Một số cá không chuyển được:" + res.Substring(1, res.Length - 2).Replace("@@", ", ") + "');</script>", false);
                    else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                }
            }
        }

        protected void ddlTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTrangThai.SelectedValue == "-1" || ddlTrangThai.SelectedValue == "-4")
            {
                CaChet.Visible = true;
                tblLyDoChet = csCont.LoadLyDoChet(1);
                if (Request.QueryString["type"] == "chuyentrangthai")
                {
                    if (Session["DSCaSauChuyenTrangThai"] != null && Session["DSCaSauChuyenTrangThai"].ToString() != "")
                    {
                        grvSPTH.DataSource = csCont.CaSau_GetCaSauFromArray(Session["DSCaSauChuyenTrangThai"].ToString());
                        grvSPTH.DataBind();
                    }
                }
                else if (Request.QueryString["type"] == "editchuyentrangthai")
                {
                    if (hdIDCaSau.Value != "0")
                    {
                        grvSPTH.DataSource = csCont.CaSau_GetCaSauFromArray("@" + hdIDCaSau.Value + "@");
                        grvSPTH.DataBind();
                    }
                }
            }
            else CaChet.Visible = false;
        }

        protected void btnChuyenChon_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] == "editchuyentrangthaigroup")
            {
                if (hdIDBienDongGroup.Value != "0")
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                    if (DateTime.Parse(txtThoiDiemChuyen.Text, culture) < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "chuyenfail", "<script language=javascript>alert('Ngày chuyển không được trước ngày khóa sổ');</script>", false);
                        return;
                    }
                    string StrIDBienDong = "";
                    foreach (GridViewRow row in grvDanhSach.Rows)
                    {
                        HyperLink lnkIDCaSau = (HyperLink)(row.FindControl("lnkIDCaSau"));
                        HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                        if (chkChon.Checked)
                        {
                            StrIDBienDong += "@" + lnkIDCaSau.ToolTip + "@";
                        }
                    }
                    string res = csCont.UpdateBienDongList(StrIDBienDong, DateTime.Parse(txtThoiDiemChuyen.Text, culture), ddlTrangThai.SelectedValue, UserId);
                    if (res != "") Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Một số cá không chuyển được:" + res.Substring(1, res.Length - 2).Replace("@@", ", ") + "');</script>", false);
                    else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                }
            }
        }
    }
}