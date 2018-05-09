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
using System.Text;
using System.IO;
using System.Data.SqlClient;

namespace DotNetNuke.Modules.QLCS
{
    public partial class casau_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
                if (!IsPostBack)
                {
                    txtTruocNgay.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss");
                    DataTable dtLoaiCa = csCont.LoadLoaiCa(1);
                    ddlLoaiCa.DataSource = dtLoaiCa;
                    ddlLoaiCa.DataTextField = "TenLoaiCa";
                    ddlLoaiCa.DataValueField = "IDLoaiCa";
                    ddlLoaiCa.DataBind();
                    ddlLoaiCa.Items.Insert(0, new ListItem("", "0"));

                    DataTable dtChuong = csCont.LoadChuong(1);
                    ddlChuong.DataSource = dtChuong;
                    ddlChuong.DataTextField = "Chuong";
                    ddlChuong.DataValueField = "IDChuong";
                    ddlChuong.DataBind();
                    ddlChuong.Items.Insert(0, new ListItem("","0"));

                    DataTable dtCaMe = csCont.LoadCaSauMe_AllTrangThai();
                    ddlCaMe.DataSource = dtCaMe;
                    ddlCaMe.DataTextField = "CaMe";
                    ddlCaMe.DataValueField = "IDCaSau";
                    ddlCaMe.DataBind();
                    ddlCaMe.Items.Insert(0, new ListItem("","0"));

                    DataTable dtStatus = new DataTable("Status");
                    DataRow dr = null;
                    dtStatus.Columns.AddRange(new DataColumn[] { new DataColumn("ID"), new DataColumn("Description") });
                    dr = dtStatus.NewRow();
                    dr["ID"] = 0;
                    dr["Description"] = "Bình thường";
                    dtStatus.Rows.Add(dr);
                    dr = dtStatus.NewRow();
                    dr["ID"] = 1;
                    dr["Description"] = "Bệnh";
                    dtStatus.Rows.Add(dr);
                    dr = dtStatus.NewRow();
                    dr["ID"] = -4;
                    dr["Description"] = "Loại thải";
                    dtStatus.Rows.Add(dr);
                    dr = dtStatus.NewRow();
                    dr["ID"] = -3;
                    dr["Description"] = "Đã bán";
                    dtStatus.Rows.Add(dr);
                    dr = dtStatus.NewRow();
                    dr["ID"] = -2;
                    dr["Description"] = "Giết mổ";
                    dtStatus.Rows.Add(dr);
                    dr = dtStatus.NewRow();
                    dr["ID"] = -1;
                    dr["Description"] = "Chết";
                    dtStatus.Rows.Add(dr);
                    ddlStatus.DataSource = dtStatus;
                    ddlStatus.DataValueField = "ID";
                    ddlStatus.DataTextField = "Description";
                    ddlStatus.DataBind();

                    DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
                    int CaSauBienDongPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_CaSauBienDongPage"], PortalId).TabID;

                    btnChuyenChuong.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/chuyenchuong") + "','',600,400);";
                    btnChuyenGioiTinh.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/chuyengioitinh") + "','',600,400);";
                    btnChuyenLoaiCa.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/chuyenloaica") + "','',600,400);";
                    btnChuyenNguonGoc.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/chuyennguongoc") + "','',600,400);";
                    btnChuyenMaSo.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/chuyenmaso") + "','',600,400);";
                    btnChuyenTrangThai.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/chuyentrangthai") + "','',1200,600);";
                    btnChuyenGiong.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/chuyengiong") + "','',600,400);";
                    btnChuyenNgayNo.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/chuyenngayno") + "','',600,400);";
                    btnChuyenNgayXuongChuong.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/chuyenngayxuongchuong") + "','',600,400);";
                    btnChuyenCaMe.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(CaSauBienDongPage, "", "type/chuyencame") + "','',600,400);";
                    
                    lnkAddCaSau.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "casau_add", "mid/" + this.ModuleId.ToString());

                    if (Session["AutoDisplay_CS"] != null && Convert.ToBoolean(Session["AutoDisplay_CS"]))
                    {
                        txtID.Text = Session["CSAdmin_ID"].ToString();
                        txtMaSo.Text = Session["CSAdmin_MaSo"].ToString();
                        ddlGioiTinh.SelectedValue = Session["CSAdmin_GioiTinh"].ToString();
                        Config.SetSelectedValues(ddlLoaiCa, Session["CSAdmin_LoaiCa"].ToString());
                        Config.SetSelectedValues(ddlChuong, Session["CSAdmin_Chuong"].ToString());
                        Config.SetSelectedValues(ddlCaMe, Session["CSAdmin_CaMe"].ToString());
                        if (Session["CSAdmin_TrangThai"] != null && Session["CSAdmin_TrangThai"].ToString() != "")
                        {
                            Config.SetSelectedValues(ddlStatus, Session["CSAdmin_TrangThai"].ToString());
                        }
                        else
                        {
                            Config.SetSelectedValues(ddlStatus, "0, 1, ");
                        }
                        btnLoad_Click(null, null);
                    }
                    else
                    {
                        Config.SetSelectedValues(ddlStatus, "0, 1, ");
                    }

                    if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                    {
                        lnkAddCaSau.Visible = false;
                        tdSub.Visible = false;
                        grvDanhSach.Columns[12].Visible = false;
                    }
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
                Label lblStatus = (Label)(e.Row.FindControl("lblStatus"));
                HyperLink lnkIDCaSau = (HyperLink)(e.Row.FindControl("lnkIDCaSau"));
                HyperLink lnkCaMe= (HyperLink)(e.Row.FindControl("lnkCaMe"));
                Label lblSTT = (Label)(e.Row.FindControl("lblSTT"));
                lnkIDCaSau.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdListBienDongPage.Value), "", "IDCaSau/" + r["IDCaSau"].ToString()) + "','',800,600);";
                lnkIDCaSau.Attributes["style"] = "cursor:pointer;font-weight:bold;";
                lnkCaMe.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdListBienDongPage.Value), "", "IDCaSau/" + r["CaMe"].ToString()) + "','',800,600);";
                lnkCaMe.Attributes["style"] = "cursor:pointer;font-weight:bold;";
                chkChon.Attributes["name"] = r["IDCaSau"].ToString();
                chkChon.Attributes["onclick"] = "chon_click(event, this);";
                int tt = Convert.ToInt32(r["Status"]);
                if (tt < 0) lblStatus.Text = r["TrangThai"].ToString() + " (" + Convert.ToDateTime(r["LatestChange"]).ToString("dd/MM/yyyy") + ")";
                else 
                { 
                    lblStatus.Text = r["TrangThai"].ToString();
                    if (Convert.ToInt32(r["EndStatus"]) < 0) lblStatus.Text += " (" + Convert.ToDateTime(r["LatestChange"]).ToString("dd/MM/yyyy") + ")";
                }
                int t = e.Row.RowIndex+1;
                lblSTT.Text = t.ToString();
            }
        }

        protected void grvHidden_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                Label lblStatus = (Label)(e.Row.FindControl("lblStatus"));
                HyperLink lnkIDCaSau = (HyperLink)(e.Row.FindControl("lnkIDCaSau"));
                HyperLink lnkCaMe = (HyperLink)(e.Row.FindControl("lnkCaMe"));
                lnkIDCaSau.Attributes["style"] = "cursor:pointer;font-weight:bold;";
                lnkCaMe.Attributes["style"] = "cursor:pointer;font-weight:bold;";
                int tt = Convert.ToInt32(r["Status"]);
                if (tt < 0) lblStatus.Text = r["TrangThai"].ToString() + " (" + Convert.ToDateTime(r["LatestChange"]).ToString("dd/MM/yyyy") + ")";
                else
                {
                    lblStatus.Text = r["TrangThai"].ToString();
                    if (Convert.ToInt32(r["EndStatus"]) < 0) lblStatus.Text += " (" + Convert.ToDateTime(r["LatestChange"]).ToString("dd/MM/yyyy") + ")";
                }
            }
        }

        protected void grvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            udpDanhSach.Update();
        }

        protected void grvDanhSach_Sorting(object sender, GridViewSortEventArgs e)
        {
            hdOrderBy.Value = e.SortExpression;
            udpDanhSach.Update();
        }

        private string LoadODSParameters()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            string strWhereClause = "";
            
            string[] arr = Config.GetSelectedValues(ddlStatus).Split(new string[] {", "}, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length == 6) strWhereClause += "0=0";
            else strWhereClause += "Status in (" + Config.GetSelectedValues(ddlStatus).Remove(Config.GetSelectedValues(ddlStatus).Length - 2) + ")";
            
            if (txtID.Text.Trim() != "")
            {
                strWhereClause += " and IDCaSau = '" + txtID.Text + "'";
            }
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
            if (Config.GetSelectedValues(ddlLoaiCa) != "0, " && Config.GetSelectedValues(ddlLoaiCa) != "")
            {
                strWhereClause += " and LoaiCa in (" + Config.GetSelectedValues(ddlLoaiCa).Remove(Config.GetSelectedValues(ddlLoaiCa).Length-2) + ")";
            }
            if (Config.GetSelectedValues(ddlChuong) != "0, " && Config.GetSelectedValues(ddlChuong) != "")
            {
                strWhereClause += " and Chuong in (" + Config.GetSelectedValues(ddlChuong).Remove(Config.GetSelectedValues(ddlChuong).Length - 2) + ")";
            }
            if (Config.GetSelectedValues(ddlCaMe) != "0, " && Config.GetSelectedValues(ddlCaMe) != "")
            {
                strWhereClause += " and CaMe in (" + Config.GetSelectedValues(ddlCaMe).Remove(Config.GetSelectedValues(ddlCaMe).Length - 2) + ")";
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
            bool Die = false;
            if (txtDieFrom.Text != "")
            {
                strWhereClause += " and LatestChange >= '" + DateTime.Parse(txtDieFrom.Text, ci).ToString("yyyyMMdd") + "' and Status < 0";
                Die = true;
            }
            if (txtDieTo.Text != "")
            {
                strWhereClause += " and LatestChange < '" + DateTime.Parse(txtDieTo.Text, ci).AddDays(1).ToString("yyyyMMdd") + "'";
                if (!Die) strWhereClause += " and Status < 0";
            }
            return strWhereClause;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            Session["AutoDisplay_CS"] = true;
            Session["CSAdmin_ID"] = txtID.Text;
            Session["CSAdmin_MaSo"] = txtMaSo.Text;
            Session["CSAdmin_GioiTinh"] = ddlGioiTinh.SelectedValue;
            Session["CSAdmin_LoaiCa"] = Config.GetSelectedValues(ddlLoaiCa);
            Session["CSAdmin_Chuong"] = Config.GetSelectedValues(ddlChuong);
            Session["CSAdmin_CaMe"] = Config.GetSelectedValues(ddlCaMe);
            Session["CSAdmin_TrangThai"] = Config.GetSelectedValues(ddlStatus);

            grvDanhSach.DataSourceID = "odsDanhSach";
            grvDanhSach.PageSize = int.Parse(ddlPageSize.SelectedValue);
            string strWhereClause = LoadODSParameters();
            if (strWhereClause != "")
            {
                if (txtTruocNgay.Text == "") txtTruocNgay.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss");
                odsDanhSach.SelectParameters["WhereClause"].DefaultValue = strWhereClause;
                odsDanhSach.SelectParameters["Date"].DefaultValue = txtTruocNgay.Text.Trim();
                lblTongSo.Text = CaSauDataObject.Count(strWhereClause, txtTruocNgay.Text.Trim()).ToString();
                grvDanhSach.Visible = true;
            }
            else
            {
                grvDanhSach.Visible = false;
                lblTongSo.Text = "0";
            }
            udpDanhSach.Update();
        }

        protected void btnLoad1_Click(object sender, EventArgs e)
        {
            Session["AutoDisplay_CS"] = true;
            Session["CSAdmin_ID"] = txtID.Text;
            Session["CSAdmin_MaSo"] = txtMaSo.Text;
            Session["CSAdmin_GioiTinh"] = ddlGioiTinh.SelectedValue;
            Session["CSAdmin_LoaiCa"] = Config.GetSelectedValues(ddlLoaiCa);
            Session["CSAdmin_Chuong"] = Config.GetSelectedValues(ddlChuong);
            Session["CSAdmin_CaMe"] = Config.GetSelectedValues(ddlCaMe);
            Session["CSAdmin_TrangThai"] = Config.GetSelectedValues(ddlStatus);

            grvDanhSach.DataSourceID = "odsDanhSach";
            grvDanhSach.PageSize = int.Parse(ddlPageSize.SelectedValue);
            string strWhereClause = LoadODSParameters();
            if (strWhereClause != "")
            {
                if (txtTruocNgay.Text == "") txtTruocNgay.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss");
                odsDanhSach.SelectParameters["WhereClause"].DefaultValue = strWhereClause;
                odsDanhSach.SelectParameters["Date"].DefaultValue = txtTruocNgay.Text.Trim();
                lblTongSo.Text = CaSauDataObject.Count(strWhereClause, txtTruocNgay.Text.Trim()).ToString();
                grvDanhSach.Visible = true;
            }
            else
            {
                grvDanhSach.Visible = false;
                lblTongSo.Text = "0";
            }
            udpDanhSach.Update();
            btnBoToanBo_Click(null, null);
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

        protected void btnChuyenChuong_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (ListItem i in lstChon.Items)
            {
                if(i.Selected) s += "@" + i.Value + "@";
            }
            Session["DSCaSauChuyenChuong"] = s;
        }

        protected void btnChuyenMaSo_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (ListItem i in lstChon.Items)
            {
                if (i.Selected) s += "@" + i.Value + "@";
            }
            Session["DSCaSauChuyenMaSo"] = s;
            
        }

        protected void btnChuyenGioiTinh_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (ListItem i in lstChon.Items)
            {
                if (i.Selected) s += "@" + i.Value + "@";
            }
            Session["DSCaSauChuyenGioiTinh"] = s;
            
        }

        protected void btnChuyenLoaiCa_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (ListItem i in lstChon.Items)
            {
                if (i.Selected) s += "@" + i.Value + "@";
            }
            Session["DSCaSauChuyenLoaiCa"] = s;
            
        }

        protected void btnChuyenNguonGoc_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (ListItem i in lstChon.Items)
            {
                if (i.Selected) s += "@" + i.Value + "@";
            }
            Session["DSCaSauChuyenNguonGoc"] = s;
            
        }

        protected void btnChuyenTrangThai_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (ListItem i in lstChon.Items)
            {
                if (i.Selected) s += "@" + i.Value + "@";
            }
            Session["DSCaSauChuyenTrangThai"] = s;
            
        }

        protected void btnChuyenGiong_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (ListItem i in lstChon.Items)
            {
                if (i.Selected) s += "@" + i.Value + "@";
            }
            Session["DSCaSauChuyenGiong"] = s;
            
        }

        protected void btnChuyenNgayNo_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (ListItem i in lstChon.Items)
            {
                if (i.Selected) s += "@" + i.Value + "@";
            }
            Session["DSCaSauChuyenNgayNo"] = s;
            
        }

        protected void btnChuyenNgayXuongChuong_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (ListItem i in lstChon.Items)
            {
                if (i.Selected) s += "@" + i.Value + "@";
            }
            Session["DSCaSauChuyenNgayXuongChuong"] = s;
            
        }

        protected void btnChuyenCaMe_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (ListItem i in lstChon.Items)
            {
                if (i.Selected) s += "@" + i.Value + "@";
            }
            Session["DSCaSauChuyenCaMe"] = s;
            
        }

        protected void btnXoaCa_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (ListItem i in lstChon.Items)
            {
                if (i.Selected) s += "@" + i.Value + "@";
            }
            csCont.CaSauDelete(s);
            for (int i = lstChon.Items.Count - 1; i >= 0; i-- )
            {
                if (lstChon.Items[i].Selected) lstChon.Items.RemoveAt(i);
            }
            btnLoad_Click(null, null);
        }

        //protected void btnExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        //        string filename = "QuanLyDan" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xls";
        //        string strSQL = "QLCS_GetCaSau";
        //        SqlParameter[] param = new SqlParameter[4];
        //        if (txtTruocNgay.Text == "")
        //        {
        //            txtTruocNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
        //        }
        //        param[0] = new SqlParameter("@WhereClause", LoadODSParameters());
        //        param[1] = new SqlParameter("@OrderBy", hdOrderBy.Value);
        //        param[2] = new SqlParameter("@Date", DateTime.Parse(txtTruocNgay.Text, ci).ToString("yyyyMMdd"));
        //        param[3] = new SqlParameter("@dDate", DateTime.Parse(txtTruocNgay.Text, ci));
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
                
                string strSQL = "QLCS_GetCaSau";
                SqlParameter[] param = new SqlParameter[4];
                if (txtTruocNgay.Text == "")
                {
                    txtTruocNgay.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                }
                string filename = "QuanLyDan" + DateTime.Parse(txtTruocNgay.Text, ci).ToString("yyyy_MM_dd_HH_mm_ss") + ".xls";
                param[0] = new SqlParameter("@WhereClause", LoadODSParameters());
                param[1] = new SqlParameter("@OrderBy", hdOrderBy.Value);
                param[2] = new SqlParameter("@Date", DateTime.Parse(txtTruocNgay.Text, ci).ToString("yyyyMMdd HH:mm:ss"));
                param[3] = new SqlParameter("@dDate", DateTime.Parse(txtTruocNgay.Text, ci));
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";

                grvHidden.DataSource = dt;
                grvHidden.DataBind();

                StringWriter sw = new StringWriter();
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