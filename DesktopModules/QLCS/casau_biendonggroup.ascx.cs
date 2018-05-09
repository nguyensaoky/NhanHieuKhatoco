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
    public partial class casau_biendonggroup : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
        int CaSauBienDongPage;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
                if (!IsPostBack)
                {
                    txtToDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    txtFromDate.Text = DateTime.Now.AddDays(-60).ToString("dd/MM/yyyy");
                    btnLoad_Click(null, null);

                    if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                    {
                        grvDanhSach.Columns[5].Visible = false;
                        grvDanhSach.Columns[6].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
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
            string strWhereClause = "bdg.ThoiDiemBienDong >= (select NgayKhoaSo from dnn_QLCS_Config)";
            if (txtFromDate.Text != "")
            {
                strWhereClause += " and bdg.ThoiDiemBienDong >= '" + DateTime.Parse(txtFromDate.Text, ci).ToString("yyyyMMdd") + "'";
            }
            if (txtToDate.Text != "")
            {
                strWhereClause += " and bdg.ThoiDiemBienDong < '" + DateTime.Parse(txtToDate.Text, ci).AddDays(1).ToString("yyyyMMdd") + "'";
            }
            if (ddlLoaiBienDong.SelectedValue != "0")
            {
                strWhereClause += " and bdg.LoaiBienDong = " + ddlLoaiBienDong.SelectedValue;
            }
            return strWhereClause;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                grvDanhSach.DataSourceID = "odsDanhSach";
                grvDanhSach.PageSize = int.Parse(ddlPageSize.SelectedValue);
                string strWhereClause = LoadODSParameters();
                if (strWhereClause != "")
                {
                    odsDanhSach.SelectParameters["WhereClause"].DefaultValue = strWhereClause;
                    lblTongSo.Text = CaSauDataObject.CountBienDongGroup(strWhereClause).ToString();
                    grvDanhSach.Visible = true;
                }
                else
                {
                    grvDanhSach.Visible = false;
                    lblTongSo.Text = "0";
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
                Label lblNote = (Label)(e.Row.FindControl("lblNote"));
                Label lblSoCa = (Label)(e.Row.FindControl("lblSoCa"));
                Label lblSoCaThucTe = (Label)(e.Row.FindControl("lblSoCaThucTe"));
                Label lblSoCaThucTe_Origin = (Label)(e.Row.FindControl("lblSoCaThucTe_Origin"));
                Button btnDeleteBienDong = (Button)(e.Row.FindControl("btnDeleteBienDong"));
                Button btnEditBienDong = (Button)(e.Row.FindControl("btnEditBienDong"));
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
                    btnDeleteBienDong.Visible = true;
                    btnEditBienDong.Visible = true;
                    btnDeleteBienDong.CommandArgument = r["ID"].ToString();
                    btnEditBienDong.CommandArgument = r["ID"].ToString() + "," +  Convert.ToDateTime(r["ThoiDiemBienDong"]).ToString("dd/MM/yyyy HH:mm:ss") + "," + r["Note"].ToString();
                }
                if (r["IDBienDongTo"] == DBNull.Value)
                {
                    e.Row.Visible = false;
                    return;
                }
                int SoCa = Convert.ToInt32(r["IDBienDongTo"]) - Convert.ToInt32(r["IDBienDongFrom"]) + 1;
                int SoLuongBienDongThat = Convert.ToInt32(r["SoLuongBienDongThat"]);
                int SoLuongBienDongThat_Origin = Convert.ToInt32(r["SoLuongBienDongThat_Origin"]);
                lblSoCa.Text = SoCa.ToString();
                lblSoCaThucTe.Text = SoLuongBienDongThat.ToString();
                lblSoCaThucTe_Origin.Text = SoLuongBienDongThat_Origin.ToString();
                if (lblSoCa.Text != lblSoCaThucTe.Text || lblSoCaThucTe_Origin.Text != lblSoCaThucTe.Text) e.Row.BackColor = System.Drawing.Color.LightGray;
                if (r["LoaiBienDong"].ToString() == "1")
                {
                    DataTable tbl = csCont.LoadChuongByID(int.Parse(r["Note"].ToString()));
                    if (tbl != null && tbl.Rows.Count == 1)
                    { 
                        lblNote.Text = tbl.Rows[0]["Chuong"].ToString();
                    }
                    btnEditBienDong.CommandName = "editchuyenchuonggroup";
                }
                else if (r["LoaiBienDong"].ToString() == "2")
                {
                    if(r["Note"].ToString() == "1") lblNote.Text = "Đực";
                    else if (r["Note"].ToString() == "-1") lblNote.Text = "CXĐ";
                    else if (r["Note"].ToString() == "0") lblNote.Text = "Cái";
                    btnEditBienDong.CommandName = "editchuyengioitinhgroup";
                }
                else if (r["LoaiBienDong"].ToString() == "4")
                {
                    DataTable tbl = csCont.LoadLoaiCaByID(int.Parse(r["Note"].ToString()));
                    if (tbl != null && tbl.Rows.Count == 1)
                    {
                        lblNote.Text = tbl.Rows[0]["TenLoaiCa"].ToString();
                    }
                    btnEditBienDong.CommandName = "editchuyenloaicagroup";
                }
                else if (r["LoaiBienDong"].ToString() == "5")
                {
                    lblNote.Text = r["Note"].ToString();
                    btnEditBienDong.CommandName = "editchuyenmasogroup";
                }
                else if (r["LoaiBienDong"].ToString() == "6")
                {
                    if (r["Note"].ToString() == "0") lblNote.Text = "BT";
                    else if (r["Note"].ToString() == "1") lblNote.Text = "Bệnh";
                    else if (r["Note"].ToString() == "-4") lblNote.Text = "Loại thải";
                    else if (r["Note"].ToString() == "-1") lblNote.Text = "Chết";
                    else if (r["Note"].ToString() == "-2") lblNote.Text = "Giết mổ";
                    else if (r["Note"].ToString() == "-3") lblNote.Text = "Bán";
                    else { 
                        lblNote.Text = r["Note"].ToString();
                        btnDeleteBienDong.Visible = false;
                        btnEditBienDong.Visible = false;
                    }
                    btnEditBienDong.CommandName = "editchuyentrangthaigroup";
                }
                else if (r["LoaiBienDong"].ToString() == "7")
                {
                    if (r["Note"].ToString() == "1") lblNote.Text = "Giống";
                    else lblNote.Text = "Tăng trọng";
                    btnEditBienDong.CommandName = "editchuyengionggroup";
                }
                else
                {
                    lblNote.Text = r["Note"].ToString();
                    btnDeleteBienDong.Visible = false;
                    btnEditBienDong.Visible = false;
                }
            }
        }

        protected void btnDeleteBienDong_Click(object sender, EventArgs e)
        {
            string res = csCont.DeleteBienDongCaSauGroup(int.Parse(((Button)sender).CommandArgument), UserId);
            if(res != "") lblMessage.Text = "Cá không xóa được biến động này:<br/>" + res;
            btnLoad_Click(null, null);
        }

        protected void btnEditBienDong_Click(object sender, EventArgs e)
        {
            CaSauBienDongPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauBienDongPage"], PortalId).TabID;
            Session["EditBienDongCaSauGroupParam"] = ((Button)sender).CommandArgument;
            Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/" + ((Button)sender).CommandName) + "','',800,600);</script>", false);
        }
    }
}