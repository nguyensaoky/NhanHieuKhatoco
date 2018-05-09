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
    public partial class phongap_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lnkAddNew.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "phongap_edit", "mid/" + this.ModuleId.ToString());
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
                HyperLink lnkPhongAp = (HyperLink)(e.Row.FindControl("lnkPhongAp"));
                CheckBox chkActive = (CheckBox)(e.Row.FindControl("chkActive"));
                lnkPhongAp.Text = r["TenPhongAp"].ToString();
                lnkPhongAp.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "phongap_edit", "mid/" + this.ModuleId.ToString(), "phongapid/" + r["IDPhongAp"].ToString());
                chkActive.Checked = Convert.ToBoolean(r["Active"]);
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            DataTable tblPhongAp = csCont.LoadPhongAp(int.Parse(ddlActive.SelectedValue));
            grvDanhSach.DataSource = tblPhongAp;
            grvDanhSach.DataBind();
        }
    }
}