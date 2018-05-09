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
    public partial class casauan_thuoc_lichsu : DotNetNuke.Entities.Modules.PortalModuleBase
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
                    if (Request.QueryString["IDCaSauAnThuoc"] != null && Request.QueryString["status"] != null)
                    {
                        DataTable tblCaSauAnThuoc = null;
                        DataTable tblLichSu = null;
                        int IDCaSauAnThuoc = int.Parse(Request.QueryString["IDCaSauAnThuoc"]);
                        tblLichSu = csCont.LoadActionLogByRefIDByLoaiTable(IDCaSauAnThuoc, "CaSauAn_Thuoc");
                        lt.Text = "<table><tr><th>Trạng thái</th><th>Thông tin thuốc cá ăn</th></tr>";
                        if (Request.QueryString["status"] == "1")
                        {
                            tblCaSauAnThuoc = csCont.CaSauAnThuoc_GetByID(IDCaSauAnThuoc,1);
                            lt.Text += "<tr><td>Tạo bởi user <b>" + tblCaSauAnThuoc.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblCaSauAnThuoc.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            foreach (DataRow r in tblLichSu.Rows)
                            {
                                lt.Text += "<td>";
                                lt.Text += csCont.TranslateCaSauAnThuocValue(r["Note"].ToString(), ci);
                                lt.Text += "</td></tr><tr><td>";
                                lt.Text += r["Status"].ToString() + " bởi user <b>" + r["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(r["TimeUpdate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>"; 
                            }
                            lt.Text += "<td>" + csCont.TranslateCaSauAnThuocValue(tblCaSauAnThuoc.Rows[0], ci) + "</td></tr>";
                        }
                        else if (Request.QueryString["status"] == "0")
                        {
                            tblCaSauAnThuoc = csCont.CaSauAnThuoc_GetByID(IDCaSauAnThuoc, 0);
                            lt.Text += "<tr><td>Tạo bởi user <b>" + tblCaSauAnThuoc.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblCaSauAnThuoc.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            foreach (DataRow r in tblLichSu.Rows)
                            {
                                lt.Text += "<td>";
                                lt.Text += csCont.TranslateCaSauAnThuocValue(r["Note"].ToString(), ci);
                                lt.Text += "</td></tr><tr><td>";
                                lt.Text += r["Status"].ToString() + " bởi user <b>" + r["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(r["TimeUpdate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            }
                            lt.Text += "<td></td></tr>";
                        }
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