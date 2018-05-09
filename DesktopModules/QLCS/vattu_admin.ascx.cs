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
    public partial class vattu_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        int scale = 0;

        public static string GetStandardSQLOrder(string sortBy)
        {
            if (sortBy == "")
                return "";
            string[] arrSortBy = sortBy.Split(new char[] { ' ' });
            if (arrSortBy.Length <= 2)
            {
                return sortBy;
            }
            else if (arrSortBy.Length == 3)
            {
                if (((string)(arrSortBy[1])).ToUpper() == "DESC")
                {
                    return arrSortBy[0] + " ASC";
                }
                else
                {
                    return arrSortBy[0] + " DESC";
                }
            }
            return sortBy;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
                    DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
                    hdVatTuBienDongPage.Value = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_VatTuBienDongPage"], PortalId).TabID.ToString();

                    if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                    {
                        lnkAddNew.Visible = false;
                    }
                    if(Settings["LoaiVatTu"] != null)
                        ddlLoaiVatTu.SelectedValue = Settings["LoaiVatTu"].ToString();
                    lnkAddNew.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "vattu_addnew", "mid/" + this.ModuleId.ToString(), "loaivattu/" + ddlLoaiVatTu.SelectedValue);
                    scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_" + ddlLoaiVatTu.SelectedValue + "_Scale"]);
                    btnLoad_Click(null, null);
                }
                else
                {
                    scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_" + ddlLoaiVatTu.SelectedValue + "_Scale"]);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        
        protected void grvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                Button btnBienDong = (Button)(e.Row.FindControl("btnBienDong"));
                HyperLink lnkLichSuBienDong = (HyperLink)(e.Row.FindControl("lnkLichSuBienDong"));
                HyperLink lnkEdit = (HyperLink)(e.Row.FindControl("lnkEdit"));
                Label lblSoLuong = (Label)(e.Row.FindControl("lblSoLuong"));
                btnBienDong.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdVatTuBienDongPage.Value), "", "VatTu/" + r["VatTu"].ToString()) + "','',400,250);";
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    btnBienDong.Visible = false;
                    lnkEdit.Visible = false;
                }
                lnkLichSuBienDong.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(TabId, "vattu_lichsubiendong", "mid/" + ModuleId.ToString() ,"IDVatTu/" + r["VatTu"].ToString(), "loaivattu/" + ddlLoaiVatTu.SelectedValue);
                lnkEdit.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(TabId, "vattu_edit", "mid/" + ModuleId.ToString(), "IDVatTu/" + r["VatTu"].ToString());
                lblSoLuong.Text = Config.ToXVal2(r["SoLuong"], scale);
            }
        }

        protected void grvHidden_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                Label lblSoLuong = (Label)(e.Row.FindControl("lblSoLuong"));
                lblSoLuong.Text = Config.ToXVal2(r["SoLuong"], scale);
            }
        }

        protected void grvDanhSach_Sorting(object sender, GridViewSortEventArgs e)
        {
            Session["VT_SortExpression"] = e.SortExpression;
            if (e.SortDirection == SortDirection.Ascending) Session["VT_SortDirection"] = "ASC";
            else Session["VT_SortDirection"] = "DESC";
            udpDanhSach.Update();
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            //DataTable tblVatTu = csCont.LoadVatTu_HienTai(ddlLoaiVatTu.SelectedValue, "MoTa");
            //grvDanhSach.DataSource = tblVatTu;
            //grvDanhSach.DataBind();
            //udpDanhSach.Update();

            grvDanhSach.DataSourceID = "odsDanhSach";
            odsDanhSach.SelectParameters["LVT"].DefaultValue = ddlLoaiVatTu.SelectedValue;

            if (Session["VT_SortExpression"] != null && Session["VT_SortDirection"] != null)
            {
                SortDirection sd = SortDirection.Descending;
                if (Session["VT_SortDirection"].ToString() == "ASC") sd = SortDirection.Ascending;
                grvDanhSach.Sort(Session["VT_SortExpression"].ToString(), sd);
            }
        }

        protected void btnBienDong_Click(object sender, EventArgs e)
        {
            Session["UserID"] = UserId;
        }

        //protected void btnExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string filename = "VatTu_" + ddlLoaiVatTu.SelectedValue + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xls";
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
        //        Response.ContentType = "application/vnd.ms-excel";
        //        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //        sb.Append(@"<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body style='text-align:center;font-family:Times New Roman;'><br/><br/>");
        //        sb.Append("<table border='1'>");
        //        sb.Append("<tr>");
        //        DataControlField col = null;
        //        for (int i = 0; i < grvDanhSach.Columns.Count - 1; i++)
        //        {
        //            col = grvDanhSach.Columns[i];
        //            sb.Append("<td style='background-color:#CCC;'>" + col.HeaderText + "</td>");
        //        }
        //        sb.Append("</tr>");
        //        GridViewRow row = null;
        //        for (int i = 0; i < grvDanhSach.Rows.Count; i++)
        //        {
        //            row = grvDanhSach.Rows[i];
        //            sb.Append("<tr>");
        //            TableCell cell = null;
        //            for (int j = 0; j < row.Cells.Count - 1; j++)
        //            {
        //                cell = row.Cells[j];
        //                if (j != 1)
        //                {
        //                    sb.Append("<td>" + cell.Text + "</td>");
        //                }
        //                else
        //                {
        //                    Label l = (Label)(cell.FindControl("lblSoLuong"));
        //                    sb.Append("<td>" + l.Text + "</td>");
        //                }
        //            }
        //            sb.Append("</tr>");
        //        }
        //        sb.Append("</table>");
        //        sb.Append("</body></html>");
        //        Response.Write(sb.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ex.Message);
        //    }
        //}

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = "VatTu_" + ddlLoaiVatTu.SelectedValue + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xls";
                DataTable tblVatTu = csCont.LoadVatTu_HienTai(ddlLoaiVatTu.SelectedValue, "MoTa");

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                grvHidden.DataSource = tblVatTu;
                grvHidden.DataBind();

                System.IO.StringWriter sw = new System.IO.StringWriter();
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

        protected void ddlLoaiVatTu_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnLoad_Click(null, null);
        }
    }
}