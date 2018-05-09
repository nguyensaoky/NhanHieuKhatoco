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

namespace DotNetNuke.Modules.QLCS
{
    public partial class casau_phuchoicaban : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
                if (!IsPostBack)
                {
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
                    ddlChuong.Items.Insert(0, new ListItem("-----","0"));

                    DataTable dtCaMe = csCont.LoadCaSauMe_AllTrangThai();
                    ddlCaMe.DataSource = dtCaMe;
                    ddlCaMe.DataTextField = "CaMe";
                    ddlCaMe.DataValueField = "IDCaSau";
                    ddlCaMe.DataBind();
                    ddlCaMe.Items.Insert(0, new ListItem("-----", "0"));

                    if (Session["AutoDisplay_CSBan"] != null && Convert.ToBoolean(Session["AutoDisplay_CSBan"]))
                    {
                        txtMaSo.Text = Session["CSBan_MaSo"].ToString();
                        ddlGioiTinh.SelectedValue = Session["CSBan_GioiTinh"].ToString();
                        ddlLoaiCa.SelectedValue = Session["CSBan_LoaiCa"].ToString();
                        ddlChuong.SelectedValue = Session["CSBan_Chuong"].ToString();
                        ddlCaMe.SelectedValue = Session["CSBan_CaMe"].ToString();
                        btnLoad_Click(null, null);
                    }

                    if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                    {
                        btnPhucHoi.Visible = false;
                        tdSub.Visible = false;
                    }
                    DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
                    hdListBienDongPage.Value = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauListBienDongPage"], PortalId).TabID.ToString();
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void grvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(e.Row.FindControl("chkChon"));
                HyperLink lnkIDCaSau = (HyperLink)(e.Row.FindControl("lnkIDCaSau"));
                lnkIDCaSau.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdListBienDongPage.Value), "", "IDCaSau/" + r["IDCaSau"].ToString()) + "','',800,600);";
                lnkIDCaSau.Attributes["style"] = "cursor:pointer;font-weight:bold;";
                chkChon.Attributes["name"] = r["IDCaSau"].ToString();
                chkChon.Attributes["onclick"] = "chon_click(event, this);";
            }
        }

        protected void grvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            udpDanhSach.Update();
        }

        protected void grvDanhSach_Sorting(object sender, GridViewSortEventArgs e)
        {
            udpDanhSach.Update();
        }

        private string LoadODSParameters()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            string strWhereClause = "[Status] = -3";
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
            if (txtNgayBanFrom.Text != "")
            {
                strWhereClause += " and LatestChange >= '" + DateTime.Parse(txtNgayBanFrom.Text, ci).ToString("yyyyMMdd") + "'";
            }
            if (txtNgayBanTo.Text != "")
            {
                strWhereClause += " and LatestChange < '" + DateTime.Parse(txtNgayBanTo.Text, ci).AddDays(1).ToString("yyyyMMdd") + "'";
            }
            return strWhereClause;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            Session["AutoDisplay_CSBan"] = true;
            Session["CSBan_MaSo"] = txtMaSo.Text;
            Session["CSBan_GioiTinh"] = ddlGioiTinh.SelectedValue;
            Session["CSBan_LoaiCa"] = ddlLoaiCa.SelectedValue;
            Session["CSBan_Chuong"] = ddlChuong.SelectedValue;
            Session["CSBan_CaMe"] = ddlCaMe.SelectedValue;

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
            udpDanhSach.Update();
        }

        private bool existInList(string idCaSau, ListBox lstChon)
        {
            foreach (ListItem item in lstChon.Items)
            {
                if (item.Value == idCaSau)
                    return true;
            }
            return false;
        }

        protected void btnChon_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grvDanhSach.Rows)
            {
                HyperLink lnkIDCaSau = (HyperLink)(row.FindControl("lnkIDCaSau"));
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                if (chkChon.Checked && !existInList(lnkIDCaSau.Text, lstChon))
                {
                    lstChon.Items.Add(new ListItem(lnkIDCaSau.Text + "-VC:" + row.Cells[1].Text + "-LC:" + HttpContext.Current.Server.HtmlDecode(row.Cells[6].Text) + "-CH:" + row.Cells[2].Text, lnkIDCaSau.Text));
                    lstChon.Items[lstChon.Items.Count - 1].Selected = true;
                }
            }
            udpDanhSachChon.Update();
        }
        
        protected void btnBo_Click(object sender, EventArgs e)
        {
            for (int i = lstChon.Items.Count-1; i>=0; i--)
            {
                if (lstChon.Items[i].Selected)
                {
                    lstChon.Items.RemoveAt(i);
                }
            }
            udpDanhSachChon.Update();
        }

        protected void btnBoToanBo_Click(object sender, EventArgs e)
        {
            lstChon.Items.Clear();
            udpDanhSachChon.Update();
        }

        protected void btnPhucHoi_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (ListItem i in lstChon.Items)
            {
                if (i.Selected) s += "@" + i.Value + "@";
            }
            csCont.PhucHoiCaBan(s, UserId);
            for (int i = lstChon.Items.Count - 1; i >= 0; i-- )
            {
                if (lstChon.Items[i].Selected) lstChon.Items.RemoveAt(i);
            }
            btnLoad_Click(null, null);
        }
    }
}