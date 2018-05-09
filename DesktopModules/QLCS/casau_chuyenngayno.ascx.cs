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
    public partial class casau_chuyenngayno : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] != "chuyenngayno") { wrapper.Visible = false; return; }
            if (!IsPostBack)
            {
                txtNgayNo.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        protected void btnChuyen_Click(object sender, EventArgs e)
        {
            if (Session["DSCaSauChuyenNgayNo"] != null && Session["DSCaSauChuyenNgayNo"].ToString() != "")
            {
                IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                int res = csCont.ChuyenNgayNo(Session["DSCaSauChuyenNgayNo"].ToString(), DateTime.Parse(txtNgayNo.Text, culture));
                Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong với những cá sấu hợp lệ');window.opener.finishEdit();self.close();</script>", false);
            }
        }
    }
}