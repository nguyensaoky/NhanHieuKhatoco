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
    public partial class casau_chuyencame : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] != "chuyencame") { wrapper.Visible = false; return;}
            if (!IsPostBack)
            {
                DataTable dtCaMe = csCont.LoadCaSauMe_AllTrangThai();
                ddlCaMe.DataSource = dtCaMe;
                ddlCaMe.DataTextField = "CaMe";
                ddlCaMe.DataValueField = "IDCaSau";
                ddlCaMe.DataBind();
            }
        }

        protected void btnChuyen_Click(object sender, EventArgs e)
        {
            if (Session["DSCaSauChuyenCaMe"] != null && Session["DSCaSauChuyenCaMe"].ToString() != "")
            {
                int res = csCont.ChuyenCaMe(Session["DSCaSauChuyenCaMe"].ToString(), int.Parse(ddlCaMe.SelectedValue));
                Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong với những cá sấu hợp lệ');window.opener.finishEdit();self.close();</script>", false);
            }
        }
    }
}