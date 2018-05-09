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
    public partial class casau_chuyengioitinh : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] != "chuyengioitinh" && Request.QueryString["type"] != "editchuyengioitinh" && Request.QueryString["type"] != "editchuyengioitinhgroup") { wrapper.Visible = false; return; }
            if (!IsPostBack)
            {
                txtThoiDiemChuyen.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                if (Request.QueryString["type"] == "editchuyengioitinh")
                {
                    string[] arg = Session["EditBienDongCaSauParam"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    txtThoiDiemChuyen.Text = arg[2];
                    hdIDBienDong.Value = arg[0];
                    hdIDCaSau.Value = arg[1];

                    ddlGioiTinh.SelectedValue = arg[3];
                }
                else if (Request.QueryString["type"] == "editchuyengioitinhgroup")
                {
                    string[] arg = Session["EditBienDongCaSauGroupParam"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    txtThoiDiemChuyen.Text = arg[1];
                    hdIDBienDongGroup.Value = arg[0];
                    ddlGioiTinh.SelectedValue = arg[2];

                    btnChuyen.Text = "Cập nhật toàn bộ biến động";
                    btnChuyenChon.Visible = true;

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

        protected void btnChuyen_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] == "chuyengioitinh")
            {
                if (Session["DSCaSauChuyenGioiTinh"] != null && Session["DSCaSauChuyenGioiTinh"].ToString() != "")
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                    if (DateTime.Parse(txtThoiDiemChuyen.Text, culture) < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "chuyenfail", "<script language=javascript>alert('Ngày chuyển không được trước ngày khóa sổ');</script>", false);
                        return;
                    }
                    string res = csCont.ChuyenGioiTinh(Session["DSCaSauChuyenGioiTinh"].ToString(), int.Parse(ddlGioiTinh.SelectedValue), DateTime.Parse(txtThoiDiemChuyen.Text, culture), UserId);
                    if (res != "") Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Một số cá không chuyển trạng thái được có ID: " + res.Substring(1, res.Length - 2).Replace("@@", ", ") + "');</script>", false);
                    else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                }
            }
            else if (Request.QueryString["type"] == "editchuyengioitinh")
            {
                if (hdIDCaSau.Value != "0")
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                    if (DateTime.Parse(txtThoiDiemChuyen.Text, culture) < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "chuyenfail", "<script language=javascript>alert('Ngày chuyển không được trước ngày khóa sổ');</script>", false);
                        return;
                    }
                    int res = csCont.EditChuyenGioiTinh(int.Parse(hdIDBienDong.Value), int.Parse(hdIDCaSau.Value), int.Parse(ddlGioiTinh.SelectedValue), DateTime.Parse(txtThoiDiemChuyen.Text, culture), UserId);
                    if (res == 0) Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Không chuyển trạng thái được!');</script>", false);
                    else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                }
            }
            else if (Request.QueryString["type"] == "editchuyengioitinhgroup")
            {
                if (hdIDBienDongGroup.Value != "0")
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                    if (DateTime.Parse(txtThoiDiemChuyen.Text, culture) < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "chuyenfail", "<script language=javascript>alert('Ngày chuyển không được trước ngày khóa sổ');</script>", false);
                        return;
                    }
                    string res = csCont.UpdateBienDongGroup(int.Parse(hdIDBienDongGroup.Value), DateTime.Parse(txtThoiDiemChuyen.Text, culture), ddlGioiTinh.SelectedValue, UserId);
                    if (res != "") Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Một số cá không chuyển được:" + res.Substring(1, res.Length - 2).Replace("@@", ", ") + "');</script>", false);
                    else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                }
            }
        }

        protected void btnChuyenChon_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] == "editchuyengioitinhgroup")
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
                    string res = csCont.UpdateBienDongList(StrIDBienDong, DateTime.Parse(txtThoiDiemChuyen.Text, culture), ddlGioiTinh.SelectedValue, UserId);
                    if (res != "") Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>alert('Một số cá không chuyển được:" + res.Substring(1, res.Length - 2).Replace("@@", ", ") + "');</script>", false);
                    else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong!');window.opener.finishEdit();self.close();</script>", false);
                }
            }
        }
    }
}