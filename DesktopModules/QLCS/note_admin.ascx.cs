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
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DotNetNuke.Modules.QLCS
{
    public partial class note_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    DataTable dtChuong = csCont.LoadChuong(1);
                    ddlChuong.DataSource = dtChuong;
                    ddlChuong.DataTextField = "Chuong";
                    ddlChuong.DataValueField = "IDChuong";
                    ddlChuong.DataBind();
                    ddlChuong.Items.Insert(0, new ListItem("Chung", "-1"));
                    ddlChuong.Items.Insert(0, new ListItem("", "0"));

                    lnkAddNote.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "note_edit", "mid/" + this.ModuleId.ToString());
                    lnkMultiChuong.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "note_multichuong", "mid/" + this.ModuleId.ToString());

                    if (Session["AutoDisplay_Note"] != null && Convert.ToBoolean(Session["AutoDisplay_Note"]))
                    {
                        txtNgayTo.Text = Session["Note_NgayDen"].ToString();
                        txtNgayFrom.Text = Session["Note_NgayTu"].ToString();
                        ddlRowStatus.SelectedValue = Session["Note_RowStatus"].ToString();
                        Config.SetSelectedValues(ddlChuong, Session["Note_Chuong"].ToString());
                        btnLoad_Click(null, null);
                    }
                }
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    lnkAddNote.Visible = false;
                }
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "KhoaCS"))
                {
                    btnKhoa.Visible = false;
                    btnMoKhoa.Visible = false;
                    grvDanhSach.Columns[3].Visible = false;
                }
                else
                {
                    if (lblTongSo.Text == "")
                    {
                        btnKhoa.Visible = false;
                        btnMoKhoa.Visible = false;
                        grvDanhSach.Columns[3].Visible = false;
                    }
                    else
                    {
                        btnKhoa.Visible = true;
                        btnMoKhoa.Visible = true;
                        grvDanhSach.Columns[3].Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnKhoa_Click(object sender, EventArgs e)
        {
            string strNote = "";
            foreach (GridViewRow row in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                if (chkChon.Checked)
                {
                    strNote += "@" + chkChon.Value + "@";
                }
            }
            csCont.Lock_Note(strNote, true);
            btnLoad_Click(null, null);
        }

        protected void btnMoKhoa_Click(object sender, EventArgs e)
        {
            string strNote = "";
            foreach (GridViewRow row in grvDanhSach.Rows)
            {
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(row.FindControl("chkChon"));
                if (chkChon.Checked)
                {
                    strNote += "@" + chkChon.Value + "@";
                }
            }
            csCont.Lock_Note(strNote, false);
            btnLoad_Click(null, null);
        }

        protected void grvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                HtmlInputCheckBox chkChon = (HtmlInputCheckBox)(e.Row.FindControl("chkChon"));
                chkChon.Value = r["ID"].ToString();
                HyperLink lnkEdit = (HyperLink)(e.Row.FindControl("lnkEdit"));
                Button btnXemThayDoi = (Button)(e.Row.FindControl("btnXemThayDoi"));
                btnXemThayDoi.CommandArgument = r["ID"].ToString();
                btnXemThayDoi.CommandName = ddlRowStatus.SelectedValue;
                Button btnDelete = (Button)(e.Row.FindControl("btnDelete"));
                lnkEdit.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "note_edit", "noteid/" + r["ID"].ToString(), "mid/" + this.ModuleId.ToString());
                if (r["Ngay"] != DBNull.Value && Convert.ToDateTime(r["Ngay"]) < Config.NgayKhoaSo())
                {
                    //btnDelete.Visible = false;
                    btnDelete.Enabled = false;
                    btnDelete.CssClass = "buttondisable";
                }
                btnDelete.CommandArgument = r["ID"].ToString();
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    btnDelete.Visible = false;
                    lnkEdit.Visible = false;
                }
                
                if (Convert.ToBoolean(r["Lock"]))
                {
                    e.Row.CssClass = "GrayRow";
                    btnDelete.Visible = false;
                }
                else
                {
                    e.Row.CssClass = "NormalRow";
                }

                if (ddlRowStatus.SelectedValue == "0")
                {
                    btnDelete.Visible = false;
                    lnkEdit.Visible = false;
                }
                btnXemThayDoi.Attributes["style"] = "background-image:url('" + ModulePath + "images/log.gif');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;width:16px;";
                e.Row.Attributes["id"] = "row_" + btnXemThayDoi.ClientID;
                e.Row.Attributes["onclick"] = "setSelectedRow(this,'" + btnXemThayDoi.ClientID + "')";
            }
        }

        protected void grvHidden_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    DataRow r = ((DataRowView)e.Row.DataItem).Row;
            //    Label lblNote = (Label)(e.Row.FindControl("lblNote"));
            //    lblNote.Text = Server.HtmlDecode(r["Note"].ToString());
            //}
        }

        protected void grvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }

        protected void grvDanhSach_Sorting(object sender, GridViewSortEventArgs e)
        {
            hdOrderBy.Value = e.SortExpression;
            Session["Note_SortExpression"] = e.SortExpression;
            if (e.SortDirection == SortDirection.Ascending) Session["Note_SortDirection"] = "ASC";
            else Session["Note_SortDirection"] = "DESC";
        }

        private string LoadODSParameters()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture("vi-VN");
            string strWhereClause = "";
            if (Config.GetSelectedValues(ddlChuong) != "0, " && Config.GetSelectedValues(ddlChuong) != "")
            {
                if (strWhereClause != "")
                {
                    strWhereClause += " and n.Chuong in (" + Config.GetSelectedValues(ddlChuong).Remove(Config.GetSelectedValues(ddlChuong).Length - 2) + ")";
                }
                else
                {
                    strWhereClause += "n.Chuong in (" + Config.GetSelectedValues(ddlChuong).Remove(Config.GetSelectedValues(ddlChuong).Length - 2) + ")";
                }
            }
            if (txtNgayFrom.Text != "")
            {
                if (strWhereClause != "")
                {
                    strWhereClause += " and n.Ngay >='" + DateTime.Parse(txtNgayFrom.Text, ci).ToString("MM/dd/yyyy") + "'";
                }
                else
                {
                    strWhereClause += "n.Ngay >='" + DateTime.Parse(txtNgayFrom.Text, ci).ToString("MM/dd/yyyy") + "'";
                }
            }
            if (txtNgayTo.Text != "")
            {
                if (strWhereClause != "")
                {
                    strWhereClause += " and n.Ngay <'" + DateTime.Parse(txtNgayTo.Text, ci).AddDays(1).ToString("MM/dd/yyyy") + "'";
                }
                else
                {
                    strWhereClause += "n.Ngay <'" + DateTime.Parse(txtNgayTo.Text, ci).AddDays(1).ToString("MM/dd/yyyy") + "'";
                }
            }
            
            return strWhereClause;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            Session["AutoDisplay_Note"] = true;
            Session["Note_RowStatus"] = ddlRowStatus.SelectedValue;
            Session["Note_Chuong"] = Config.GetSelectedValues(ddlChuong);
            Session["Note_NgayTu"] = txtNgayFrom.Text;
            Session["Note_NgayDen"] = txtNgayTo.Text;

            grvDanhSach.PageSize = int.Parse(ddlPageSize.SelectedValue);
            string strWhereClause = LoadODSParameters();

            if (ddlRowStatus.SelectedValue == "1")
            {
                grvDanhSach.DataSourceID = "odsDanhSach";
                odsDanhSach.SelectParameters["WhereClause"].DefaultValue = strWhereClause;
                lblTongSo.Text = CaSauDataObject.CountNote(strWhereClause).ToString();
                if (lblTongSo.Text == "0")
                {
                    btnKhoa.Visible = false;
                    btnMoKhoa.Visible = false;
                    grvDanhSach.Columns[3].Visible = false;
                }
            }
            else
            {
                grvDanhSach.DataSourceID = "odsDanhSachDelete";
                odsDanhSachDelete.SelectParameters["WhereClause"].DefaultValue = strWhereClause;
                lblTongSo.Text = CaSauDataObject.CountNoteDelete(strWhereClause).ToString();
                btnKhoa.Visible = false;
                btnMoKhoa.Visible = false;
                grvDanhSach.Columns[3].Visible = false;
            }

            if (Session["Note_SortExpression"] != null && Session["Note_SortDirection"] != null)
            {
                SortDirection sd = SortDirection.Descending;
                if (Session["Note_SortDirection"].ToString() == "ASC") sd = SortDirection.Ascending;
                grvDanhSach.Sort(Session["Note_SortExpression"].ToString(), sd);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int IDNote = int.Parse(((Button)sender).CommandArgument);
            int res = csCont.Note_Delete(IDNote, UserId);
            if (res == 1)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "dadelete", "alert('Đã xóa!');", true);
                btnLoad_Click(null, null);
            }
            else Page.ClientScript.RegisterStartupScript(typeof(string), "chuadelete", "alert('Xóa không được!');", true);
        }

        protected void btnXemThayDoi_Click(object sender, EventArgs e)
        {
            DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
            int NoteLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_NoteLichSuPage"], PortalId).TabID;
            Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(NoteLichSuPage, "", "status/" + ((Button)sender).CommandName, "IDNote/" + ((Button)sender).CommandArgument) + "','',1000,600);</script>", false);
        }

        //protected void btnExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        //        System.Globalization.CultureInfo cii = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
        //        string filename = "Note_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xls";
        //        string strSQL = "QLCS_GetNote";
        //        SqlParameter[] param = new SqlParameter[2];
        //        param[0] = new SqlParameter("@WhereClause", LoadODSParameters());
        //        param[1] = new SqlParameter("@OrderBy", hdOrderBy.Value);
        //        DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
        //        Response.ContentType = "application/vnd.ms-excel";
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'></center><br/><table border='1'><tr>");
        //        DataColumn col = null;
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            col = dt.Columns[i];
        //            sb.Append("<td style='background-color:#CCC;'>" + col.Caption + "</td>");
        //        }
        //        sb.Append("</tr>");
        //        DataRow row = null;
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            row = dt.Rows[i];
        //            sb.Append("<tr>");
        //            for (int j = 0; j < dt.Columns.Count; j++)
        //            {
        //                sb.Append("<td>" + row[j].ToString() + "</td>");
        //            }
        //            sb.Append("</tr>");
        //        }
        //        sb.Append("</table></body></html>");
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
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                System.Globalization.CultureInfo cii = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
                string filename = "Note_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xls";
                string strSQL = "QLCS_GetNote";
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", LoadODSParameters());
                param[1] = new SqlParameter("@OrderBy", hdOrderBy.Value);
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                grvHidden.DataSource = dt;
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
    }
}