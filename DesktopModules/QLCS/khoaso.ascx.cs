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
using System.Data.SqlClient;

namespace DotNetNuke.Modules.QLCS
{
    public partial class khoaso : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private void BindControls()
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                if (Config.NgayKhoaSo().HasValue) txtDate.Text = Config.NgayKhoaSo().Value.ToString("dd/MM/yyyy HH:mm:ss");
                else txtDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " 12:00:00";
            }
            catch (Exception ex)
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindControls();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnKhoaSo_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                CaSauController cont = new CaSauController();
                int result = cont.Config_SetNgayKhoaSo(DateTime.Parse(txtDate.Text.Trim(), ci));
                if (result == -1)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "khoasofail", "alert('Khóa sổ không thành công');", true);
                    return;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "khoasosuccess", "alert('Khóa sổ thành công!');", true);
                }
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnMoKhoa_Click(object sender, EventArgs e)
        {
            CaSauController cont = new CaSauController();
            string l = "";
            foreach (ListItem i in lstLoaiDuLieu.Items)
            {
                if (i.Selected) l += "@" + i.Value + "@";
            }
            if (l != "") cont.UnLock(l);
            Page.ClientScript.RegisterStartupScript(typeof(string), "mokhoasuccess", "alert('Đã mở khóa!');", true);
        }
    }
}