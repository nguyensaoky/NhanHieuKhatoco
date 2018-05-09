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
    public partial class nguongoc_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lnkAddNew.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "nguongoc_edit", "mid/" + this.ModuleId.ToString());
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
                HyperLink lnkNguonGoc = (HyperLink)(e.Row.FindControl("lnkNguonGoc"));
                CheckBox chkActive = (CheckBox)(e.Row.FindControl("chkActive"));
                lnkNguonGoc.Text = r["TenNguonGoc"].ToString();
                lnkNguonGoc.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "nguongoc_edit", "mid/" + this.ModuleId.ToString(), "nguongocid/" + r["IDNguonGoc"].ToString());
                chkActive.Checked = Convert.ToBoolean(r["Active"]);
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            DataTable tblNguonGoc = csCont.LoadNguonGoc(int.Parse(ddlActive.SelectedValue));
            grvDanhSach.DataSource = tblNguonGoc;
            grvDanhSach.DataBind();
        }
    }
}