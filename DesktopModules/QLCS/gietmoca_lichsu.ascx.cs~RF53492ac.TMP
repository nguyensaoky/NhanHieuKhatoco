using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Globalization;

namespace DotNetNuke.Modules.QLCS
{
    public partial class casauan_lichsu : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        private CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
                if (!IsPostBack)
                {
                    if (Request.QueryString["IDCaSauAn"] != null)
                    {
                        DataTable tblCaSauAn = null;
                        DataTable tblLichSu = null;
                        int IDCaSauAn = int.Parse(Request.QueryString["IDCaSauAn"]);
                        tblLichSu = csCont.LoadActionLogByRefIDByLoaiTable(IDCaSauAn, "CaSauAn");
                        lt.Text = "<table><tr><th>Trạng thái</th><th>Ngày cho ăn</th></tr>";
                        tblCaSauAn = csCont.CaSauAn_GetByID(IDCaSauAn);
                        lt.Text += "<tr><td>Tạo bởi user <b>" + tblCaSauAn.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblCaSauAn.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        foreach (DataRow r in tblLichSu.Rows)
                        {
                            lt.Text += "<td>";
                            lt.Text += r["Note"].ToString();
                            lt.Text += "</td></tr><tr><td>";
                            lt.Text += r["Status"].ToString() + " bởi user <b>" + r["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(r["TimeUpdate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>"; 
                        }
                        lt.Text += "<td>" + Convert.ToDateTime(tblCaSauAn.Rows[0]["ThoiDiem"]).ToString("dd/MM/yyyy") + "</td></tr>";
                        lt.Text += "</table>";
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}