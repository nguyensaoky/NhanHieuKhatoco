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
    public partial class casauan_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        int scale = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_TA_Scale"]);
                if (!IsPostBack)
                {
                    DataTable dtLoaiCa = csCont.LoadLoaiCa(1);
                    ddlLoaiCa.DataSource = dtLoaiCa;
                    ddlLoaiCa.DataTextField = "TenLoaiCa";
                    ddlLoaiCa.DataValueField = "IDLoaiCa";
                    ddlLoaiCa.DataBind();
                    ddlLoaiCa.Items.Insert(0, new ListItem("", "0"));

                    DataTable dtNhanVien = csCont.LoadNhanVien(1);
                    ddlNguoiChoAn.DataSource = dtNhanVien;
                    ddlNguoiChoAn.DataTextField = "TenNhanVien";
                    ddlNguoiChoAn.DataValueField = "IDNhanVien";
                    ddlNguoiChoAn.DataBind();
                    ddlNguoiChoAn.Items.Insert(0, new ListItem("", "0"));

                    txtNgayChoAnFrom.Text = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
                    txtNgayChoAnTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    lnkAddCaSauAn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "casauan_edit_thucan", "mid/" + this.ModuleId.ToString());

                    if (Session["AutoDisplay_CSA"] != null && Convert.ToBoolean(Session["AutoDisplay_CSA"]))
                    {
                        txtNgayChoAnFrom.Text = Session["CSAn_NgayChoAnFrom"].ToString();
                        txtNgayChoAnTo.Text = Session["CSAn_NgayChoAnTo"].ToString();
                        Config.SetSelectedValues(ddlLoaiCa, Session["CSAn_LoaiCa"].ToString());
                        Config.SetSelectedValues(ddlNguoiChoAn, Session["CSAn_NguoiChoAn"].ToString());
                        btnLoad_Click(null, null);
                    }
                }
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    lnkAddCaSauAn.Visible = false;
                }
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "KhoaCS"))
                {
                    btnKhoa.Visible = false;
                    btnMoKhoa.Visible = false;
                    grvDanhSach.Columns[5].Visible = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnKhoa_Click(object sender, EventArgs e)
        {
            string strCaSauAn = "";
            foreach (GridViewRow row in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                if (chkChon.Checked)
                {
                    strCaSauAn += "@" + chkChon.Value + "@";
                }
            }
            csCont.Lock_CaSauAn(strCaSauAn, true);
            btnLoad_Click(null, null);
        }

        protected void btnMoKhoa_Click(object sender, EventArgs e)
        {
            string strCaSauAn = "";
            foreach (GridViewRow row in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                if (chkChon.Checked)
                {
                    strCaSauAn += "@" + chkChon.Value + "@";
                }
            }
            csCont.Lock_CaSauAn(strCaSauAn, false);
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
                HyperLink lnkEdit = (HyperLink)(e.Row.FindControl("lnkEdit"));
                lnkEdit.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "casauan_edit_thucan", "csaid/" + r["ID"].ToString(), "mid/" + this.ModuleId.ToString());
                Label lblTongThucAn = (Label)(e.Row.FindControl("lblTongThucAn"));
                lblTongThucAn.Text = Config.ToXVal2(r["TongThucAn"], scale);
                Label lblNguoiChoAn = (Label)(e.Row.FindControl("lblNguoiChoAn"));
                Label lblLoaiCaAn = (Label)(e.Row.FindControl("lblLoaiCaAn"));
                if (r["NhanVien"] != DBNull.Value && r["NhanVien"].ToString() != "") lblNguoiChoAn.Text = r["NhanVien"].ToString().Substring(1, r["NhanVien"].ToString().Length - 2).Replace("@@", ", ");
                if (r["LoaiCa"] != DBNull.Value && r["LoaiCa"].ToString() != "") lblLoaiCaAn.Text = r["LoaiCa"].ToString().Substring(1, r["LoaiCa"].ToString().Length - 2).Replace("@@", ", ");

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
                Label lblThuoc = (Label)(e.Row.FindControl("lblThuoc"));
                if (r["Thuoc"] != DBNull.Value)
                {
                    int Thuoc = Convert.ToInt32(r["Thuoc"]);
                    if (Thuoc == 0) lblThuoc.Text = "";
                    else lblThuoc.Text = Thuoc.ToString();
                }
            }
        }

        protected void grvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //udpDanhSach.Update();
        }

        protected void grvDanhSach_Sorting(object sender, GridViewSortEventArgs e)
        {
            //udpDanhSach.Update();
            Session["CSAn_SortExpression"] = e.SortExpression;
            if (e.SortDirection == SortDirection.Ascending) Session["CSAn_SortDirection"] = "ASC";
            else Session["CSAn_SortDirection"] = "DESC";
        }

        private string LoadODSParameters()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture("vi-VN");
            string strWhereClause = "";
            strWhereClause += "csa.Status = ''";
            if (txtNgayChoAnFrom.Text != "")
            {
                strWhereClause += " and csa.ThoiDiem>='" + DateTime.Parse(txtNgayChoAnFrom.Text, ci).ToString("MM/dd/yyyy") + "'";
            }
            if (txtNgayChoAnTo.Text != "")
            {
                strWhereClause += " and csa.ThoiDiem<'" + DateTime.Parse(txtNgayChoAnTo.Text, ci).AddDays(1).ToString("MM/dd/yyyy") + "'";
            }
            if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
            {
                strWhereClause += " and csata.LoaiCa in (" + Config.GetSelectedValues(ddlLoaiCa).Remove(Config.GetSelectedValues(ddlLoaiCa).Length - 2) + ")";
            }
            if (Config.GetSelectedValues(ddlNguoiChoAn) != "0, " && Config.GetSelectedValues(ddlNguoiChoAn) != "")
            {
                strWhereClause += " and csatanv.NhanVien in (" + Config.GetSelectedValues(ddlNguoiChoAn).Remove(Config.GetSelectedValues(ddlNguoiChoAn).Length - 2) + ")";
            }
            return strWhereClause;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            Session["AutoDisplay_CSA"] = true;
            Session["CSAn_NgayChoAnFrom"] = txtNgayChoAnFrom.Text;
            Session["CSAn_NgayChoAnTo"] = txtNgayChoAnTo.Text;
            Session["CSAn_LoaiCa"] = Config.GetSelectedValues(ddlLoaiCa);
            Session["CSAn_NguoiChoAn"] = Config.GetSelectedValues(ddlNguoiChoAn);

            grvDanhSach.DataSourceID = "odsDanhSach";
            grvDanhSach.PageSize = int.Parse(ddlPageSize.SelectedValue);
            string strWhereClause = LoadODSParameters();
            odsDanhSach.SelectParameters["WhereClause"].DefaultValue = strWhereClause;
            lblTongSo.Text = CaSauDataObject.CountCaSauAn(strWhereClause).ToString();
            //udpDanhSach.Update();

            if (Session["CSAn_SortExpression"] != null && Session["CSAn_SortDirection"] != null)
            {
                SortDirection sd = SortDirection.Descending;
                if(Session["CSAn_SortDirection"].ToString() == "ASC") sd = SortDirection.Ascending;
                grvDanhSach.Sort(Session["CSAn_SortExpression"].ToString(), sd);
            }
        }

        protected void btnXemThayDoi_Click(object sender, EventArgs e)
        {
            DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
            int CaSauAnLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauAnLichSuPage"], PortalId).TabID;
            Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauAnLichSuPage, "", "IDCaSauAn/" + ((Button)sender).CommandArgument) + "','',1000,600);</script>", false);
        }
    }
}