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

namespace DotNetNuke.Modules.NhanHieu
{
    public partial class nhanhieu_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private NhanHieuController cont = new NhanHieuController();
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!IsPostBack)
            //    {
            //        DateTime dtPrev = DateTime.Now.Subtract(new TimeSpan(365,0,0,0));
            //        txtNgayGuiFrom.Text = dtPrev.ToString("dd/MM/yyyy");
            //        txtNgayGuiTo.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            //        btnLoad_Click(null, null);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Response.Write(ex.Message);
            //}
        }

        protected void grvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    DataRow r = ((DataRowView)e.Row.DataItem).Row;
            //    HyperLink lnkID = (HyperLink)(e.Row.FindControl("lnkID"));
            //    lnkID.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "bc_donvi_order_edit", "mid/" + this.ModuleId.ToString(), "idorder/" + r["ID"].ToString());
            //    Label lblStatus = (Label)(e.Row.FindControl("lblStatus"));
            //    Button btnDelete = (Button)(e.Row.FindControl("btnDelete"));
            //    HtmlAnchor delete = (HtmlAnchor)e.Row.Cells[3].FindControl("delete");
            //    string prompt = @"showconfirm('" + btnDelete.ClientID + @"');";
            //    delete.Attributes["onclick"] = prompt;
            //    if (r["Status"].ToString() == "1")
            //    {
            //        lblStatus.Text = "Chưa gửi";
            //        delete.Visible = true;
            //    }
            //    else if (r["Status"].ToString() == "2")
            //    {
            //        lblStatus.Text = "Đã gửi";
            //        delete.Visible = false;
            //    }
            //    else if (r["Status"].ToString() == "3")
            //    {
            //        lblStatus.Text = "Đang được cấp mã";
            //        delete.Visible = false;
            //    }
            //    else if (r["Status"].ToString() == "4")
            //    {
            //        lblStatus.Text = "Đã được cấp mã";
            //        delete.Visible = false;
            //    }
            //}
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    CultureInfo ci = CultureInfo.CreateSpecificCulture("vi-VN");
            //    DateTime? from = null;
            //    DateTime? to = null;
            //    if (txtNgayGuiFrom.Text != "") from = DateTime.Parse(txtNgayGuiFrom.Text, ci);
            //    if (txtNgayGuiTo.Text != "") to = DateTime.Parse(txtNgayGuiTo.Text, ci);
            //    DataTable tblOrder = cont.GetOrderByDonViByStatusByThoiDiem(UserInfo.Profile.Website, int.Parse(ddlStatus.SelectedValue), from, to);
            //    grvDanhSach.DataSource = tblOrder;
            //    grvDanhSach.DataBind();
            //}
            //catch (Exception)
            //{
            //}
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //int idOrder = int.Parse(((Button)sender).CommandArgument);
            //cont.DeleteOrder(idOrder);
            //btnLoad_Click(null, null);
        }
    }
}