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
    public partial class casau_chuyendan : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
            if (!IsPostBack)
            {
                DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
                txtThoiDiemChuyen.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                lblCode.Text = DateTime.Now.ToString("MMmmHHyyssdd");
            }
        }

        protected void btnChuyen_Click(object sender, EventArgs e)
        {
            if (lblCode.Text == txtCode.Text)
            {
                IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                if (DateTime.Parse(txtThoiDiemChuyen.Text, culture) < Config.NgayKhoaSo())
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "chuyenfail", "<script language=javascript>alert('Ngày chuyển không được trước ngày khóa sổ');</script>", false);
                    return;
                }
                string StrLoaiCa = "";
                foreach (ListItem i in cblLoaiCa.Items)
                {
                    if (i.Selected)
                    {
                        StrLoaiCa += "@" + i.Value + "@";
                    }
                }
                int res = csCont.ChuyenDanNew(DateTime.Parse(txtThoiDiemChuyen.Text, culture), UserId, StrLoaiCa);
                if (res == 0) Page.ClientScript.RegisterStartupScript(typeof(string), "failed", "<script language=javascript>alert('Ngày chuyển không hợp lệ');</script>", false);
                else Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã chuyển xong với những cá sấu hợp lệ');window.opener.finishEdit();self.close();</script>", false);
            }
            else
            {
                lblError.Text = "Nhập lại mã";
            }
        }
    }
}