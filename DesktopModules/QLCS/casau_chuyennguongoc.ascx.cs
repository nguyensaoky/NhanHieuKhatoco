using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DotNetNuke.Services.Localization;
using System.Collections.Generic;

namespace DotNetNuke.Modules.QLCS
{
    public partial class casau_chuyennguongoc : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] != "chuyennguongoc") { wrapper.Visible = false; return; }
            if (!IsPostBack)
            {
                DataTable dtNguonGoc = csCont.LoadNguonGoc(1);
                ddlNguonGoc.DataSource = dtNguonGoc;
                ddlNguonGoc.DataTextField = "TenNguonGoc";
                ddlNguonGoc.DataValueField = "IDNguonGoc";
                ddlNguonGoc.DataBind();
            }
        }

        protected void btnChuyen_Click(object sender, EventArgs e)
        {
            if (Session["DSCaSauChuyenNguonGoc"] != null && Session["DSCaSauChuyenNguonGoc"].ToString() != "")
            {
                int res = csCont.ChuyenNguonGoc(Session["DSCaSauChuyenNguonGoc"].ToString(), int.Parse(ddlNguonGoc.SelectedValue));
                Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong với những cá sấu hợp lệ');window.opener.finishEdit();self.close();</script>", false);
            }
        }
    }
}