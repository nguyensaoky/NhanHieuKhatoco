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
    public partial class casau_chuyenchuong : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
            if (Request.QueryString["type"] != "chuyenchuong" && Request.QueryString["type"] != "editchuyenchuong" && Request.QueryString["type"] != "editchuyenchuonggroup") { wrapper.Visible = false; return; }
            if (!IsPostBack)
            {
                DataTable dtChuong = csCont.LoadChuong(1);
                ddlChuong.DataSource = dtChuong;
                ddlChuong.DataTextField = "Chuong";
                ddlChuong.DataValueField = "IDChuong";
                ddlChuong.DataBind();

                txtThoiDiemChuyen.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                if (Request.QueryString["type"] == "editchuyenchuong")
                {
                    string[] arg = Session["EditBienDongCaSauParam"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    txtThoiDiemChuyen.Text = arg[2];
                    hdIDBienDong.Value = arg[0];
                    hdIDCaSau.Value = arg[1];

                    ddlChuong.SelectedValue = arg[3];

                    btnChuyenChuong.Text = "Chuyển";
                    btnChuyenMotPhan.Visible = false;
                }
                else if (Request.QueryString["type"] == "editchuyenchuonggroup")
                {
                    string[] arg = Session["EditBienDongCaSauGroupParam"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    txtThoiDiemChuyen.Text = arg[1];
                    hdIDBienDongGroup.Value = arg[0];
                    ddlChuong.SelectedValue = arg[2];

                    btnChuyenChuong.Text = "Cập nhật toàn bộ biến động";
                    btnChuyenChon.Visible = true;
                    btnChuyenMotPhan.Visible = false;
                    btnKiemTra.Visible = false;

                    grvDanhSach.Visible = true;
                    DataTable tblCa = csCont.GetDanhSachCaSauBienDongByBienDongGroup(int.Parse(hdIDBienDongGroup.Value));
                    grvDanhSach.DataSource = tblCa;
                    grvDanhSach.DataBind();
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

        protected void grvDanhSach_Sorting(object sender, GridViewSortEventArgs e)
        {
        }

        protected void btnKiemTra_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] == "chuyenchuong")
            {
                if (Session["DSCaSauChuyenChuong"] != null && Session["DSCaSauChuyenChuong"].ToString() != "")
                {
                    lblCSLoai.Text = "";
                    string strCS = Session["DSCaSauChuyenChuong"].ToString();
                    string[] arrCS = strCS.Substring(1, strCS.Length - 2).Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < arrCS.Length; i++)
                    {
                        if (csCont.CoTheNhapChuong(int.Parse(arrCS[i]), int.Parse(ddlChuong.SelectedValue), 3) == 0)
                        {
                            lblCSLoai.Text += arrCS[i] + ";";
                        }
                    }
                    if (lblCSLoai.Text == "")
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "tot", "<script language=javascript>alert('Tất cả đều hợp lệ');</script>", false);
                    }
                    else
                    {
                        btnChuyenMotPhan.Visible = true;
                        Page.ClientScript.RegisterStartupScript(typeof(string), "khonghople", "<script language=javascript>alert('Những con sau đây không hợp lệ: " + lblCSLoai.Text.Remove(lblCSLoai.Text.Length - 1) + "');</script>", false);
                    }
                }
            }
            else if (Request.QueryString["type"] == "editchuyenchuong")
            {
                if (hdIDCaSau.Value != "0")
                {
                    lblCSLoai.Text = "";
                    if (csCont.CoTheNhapChuong(int.Parse(hdIDCaSau.Value), int.Parse(ddlChuong.SelectedValue), 3) == 0)
                    {
                        lblCSLoai.Text += hdIDCaSau.Value + ";";
                    }
                    if (lblCSLoai.Text == "")
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "tot", "<script language=javascript>alert('Hợp lệ');</script>", false);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "khonghople", "<script language=javascript>alert('Không hợp lệ');</script>", false);
                    }
                }
            }
        }

        protected void btnChuyenMotPhan_Click(object sender, EventArgs e)
        {
            if (Session["DSCaSauChuyenChuong"] != null && Session["DSCaSauChuyenChuong"].ToString() != "")
            {
                IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                if (DateTime.Parse(txtThoiDiemChuyen.Text, culture) < Config.NgayKhoaSo())
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "chuyenfail", "<script language=javascript>alert('Ngày chuyển chuồng không được trước ngày khóa sổ');</script>", false);
                    return;
                }
                string strCSHopLe = Session["DSCaSauChuyenChuong"].ToString();
                string[] arrCSLoai = lblCSLoai.Text.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < arrCSLoai.Length; i++)
                {
                    strCSHopLe = strCSHopLe.Replace("@" + arrCSLoai[i] + "@", "");
                }

                string res = csCont.ChuyenChuong(strCSHopLe, int.Parse(ddlChuong.SelectedValue), DateTime.Parse(txtThoiDiemChuyen.Text, culture), UserId);
                if (res != "") Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Một số cá không chuyển trạng thái được có ID: " + res.Substring(1, res.Length - 2).Replace("@@", ", ") + "');</script>", false);
                else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
            }
        }

        protected void btnChuyenChuong_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] == "chuyenchuong")
            {
                if (Session["DSCaSauChuyenChuong"] != null && Session["DSCaSauChuyenChuong"].ToString() != "")
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                    if (DateTime.Parse(txtThoiDiemChuyen.Text, culture) < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "chuyenfail", "<script language=javascript>alert('Ngày chuyển chuồng không được trước ngày khóa sổ');</script>", false);
                        return;
                    }
                    string res = csCont.ChuyenChuong(Session["DSCaSauChuyenChuong"].ToString(), int.Parse(ddlChuong.SelectedValue), DateTime.Parse(txtThoiDiemChuyen.Text, culture), UserId);
                    if (res != "") Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Một số cá không chuyển trạng thái được có ID: " + res.Substring(1, res.Length - 2).Replace("@@", ", ") + "');</script>", false);
                    else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                }
            }
            else if (Request.QueryString["type"] == "editchuyenchuong")
            {
                if (hdIDCaSau.Value != "0")
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                    if (DateTime.Parse(txtThoiDiemChuyen.Text, culture) < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "chuyenfail", "<script language=javascript>alert('Ngày chuyển chuồng không được trước ngày khóa sổ');</script>", false);
                        return;
                    }
                    int res = csCont.EditChuyenChuong(int.Parse(hdIDBienDong.Value), int.Parse(hdIDCaSau.Value), int.Parse(ddlChuong.SelectedValue), DateTime.Parse(txtThoiDiemChuyen.Text, culture), UserId);
                    if (res == 0) Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Không chuyển trạng thái được!');</script>", false);
                    else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                }
            }
            else if (Request.QueryString["type"] == "editchuyenchuonggroup")
            {
                if (hdIDBienDongGroup.Value != "0")
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                    if (DateTime.Parse(txtThoiDiemChuyen.Text, culture) < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "chuyenfail", "<script language=javascript>alert('Ngày chuyển chuồng không được trước ngày khóa sổ');</script>", false);
                        return;
                    }
                    string res = csCont.UpdateBienDongGroup(int.Parse(hdIDBienDongGroup.Value), DateTime.Parse(txtThoiDiemChuyen.Text, culture),ddlChuong.SelectedValue, UserId);
                    if (res != "") Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Một số cá không chuyển được:" + res.Substring(1, res.Length - 2).Replace("@@", ", ") + "');</script>", false);
                    else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                }
            }
        }

        protected void btnChuyenChon_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] == "editchuyenchuonggroup")
            {
                if (hdIDBienDongGroup.Value != "0")
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                    if (DateTime.Parse(txtThoiDiemChuyen.Text, culture) < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "chuyenfail", "<script language=javascript>alert('Ngày chuyển chuồng không được trước ngày khóa sổ');</script>", false);
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
                    string res = csCont.UpdateBienDongList(StrIDBienDong, DateTime.Parse(txtThoiDiemChuyen.Text, culture), ddlChuong.SelectedValue, UserId);
                    if (res != "") Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Một số cá không chuyển được:" + res.Substring(1, res.Length - 2).Replace("@@", ", ") + "');</script>", false);
                    else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                }
            }
        }
    }
}