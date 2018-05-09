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
    public partial class loaica_admin : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lnkAddNew.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "loaica_edit", "mid/" + this.ModuleId.ToString());
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
                HyperLink lnkLoaiCa = (HyperLink)(e.Row.FindControl("lnkLoaiCa"));
                CheckBox chkActive = (CheckBox)(e.Row.FindControl("chkActive"));
                lnkLoaiCa.Text = r["TenLoaiCa"].ToString();
                lnkLoaiCa.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "loaica_edit", "mid/" + this.ModuleId.ToString(), "loaicaid/" + r["IDLoaiCa"].ToString());
                chkActive.Checked = Convert.ToBoolean(r["Active"]);
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            DataTable tblLoaiCa = csCont.LoadLoaiCa(int.Parse(ddlActive.SelectedValue));
            grvDanhSach.DataSource = tblLoaiCa;
            grvDanhSach.DataBind();
        }
    }
}