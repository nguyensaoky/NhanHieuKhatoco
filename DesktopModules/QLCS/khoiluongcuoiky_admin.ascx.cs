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
    public partial class khoiluongcuoiky_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lnkAddNew.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "khoiluongcuoiky_edit", "mid/" + this.ModuleId.ToString());
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !UserInfo.IsInRole("QLCS"))
                {
                    lnkAddNew.Visible = false;
                }
                LoadData();
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
                HyperLink lnkNamCan = (HyperLink)(e.Row.FindControl("lnkNamCan"));
                lnkNamCan.Text = r["NamCan"].ToString();
                lnkNamCan.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "khoiluongcuoiky_edit", "mid/" + this.ModuleId.ToString(), "namcan/" + r["NamCan"].ToString());
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            DataTable tblKhoiLuongCuoiKy = csCont.LoadKhoiLuongCuoiKy();
            grvDanhSach.DataSource = tblKhoiLuongCuoiKy;
            grvDanhSach.DataBind();
        }
        
        protected void grvDanhSach_DataBound(object sender, EventArgs e)
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);

            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "";
            cell.ColumnSpan = 1;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "";
            cell.ColumnSpan = 1;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Khối lượng cuối kỳ cá úm (kg)";
            cell.ColumnSpan = 3;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Khối lượng cuối kỳ cá 1 năm (kg)";
            cell.ColumnSpan = 3;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Khối lượng cuối kỳ cá ST1 (kg)";
            cell.ColumnSpan = 3;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Khối lượng cuối kỳ cá ST2 (kg)";
            cell.ColumnSpan = 3;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Khối lượng cuối kỳ cá HB1 (kg)";
            cell.ColumnSpan = 3;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Khối lượng cuối kỳ cá HB2 (kg)";
            cell.ColumnSpan = 3;
            row.Controls.Add(cell);

            row.Style.Add("background-color","#4099FF");
            grvDanhSach.HeaderRow.Parent.Controls.AddAt(0, row);
        }
    }
}