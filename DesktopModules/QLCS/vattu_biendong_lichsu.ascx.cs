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
    public partial class vattu_biendong_lichsu : DotNetNuke.Entities.Modules.PortalModuleBase
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
                    if (Request.QueryString["IDVatTuBienDong"] != null && Request.QueryString["status"] != null)
                    {
                        DataTable tblVatTuBienDong = null;
                        DataTable tblLichSu = null;
                        int IDVatTuBienDong = int.Parse(Request.QueryString["IDVatTuBienDong"]);
                        tblLichSu = csCont.LoadActionLogByRefIDByLoaiTable(IDVatTuBienDong, "vatTu_BienDong");
                        lt.Text = "<table><tr><th>Trạng thái</th><th>Thông tin biến động</th></tr>";
                        if (Request.QueryString["status"] == "1")
                        {
                            tblVatTuBienDong = csCont.VatTuBienDong_GetByID(IDVatTuBienDong, 1);
                            lt.Text += "<tr><td>Tạo bởi user <b>" + tblVatTuBienDong.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblVatTuBienDong.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            foreach (DataRow r in tblLichSu.Rows)
                            {
                                lt.Text += "<td>";
                                lt.Text += csCont.TranslateVatTuBienDongValue(r["Note"].ToString());
                                lt.Text += "</td></tr><tr><td>";
                                lt.Text += r["Status"].ToString() + " bởi user <b>" + r["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(r["TimeUpdate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>"; 
                            }
                            lt.Text += "<td>" + csCont.TranslateVatTuBienDongValue(tblVatTuBienDong.Rows[0]) + "</td></tr>";
                        }
                        else if (Request.QueryString["status"] == "0")
                        {
                            tblVatTuBienDong = csCont.VatTuBienDong_GetByID(IDVatTuBienDong, 0);
                            lt.Text += "<tr><td>Tạo bởi user <b>" + tblVatTuBienDong.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblVatTuBienDong.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            foreach (DataRow r in tblLichSu.Rows)
                            {
                                lt.Text += "<td>";
                                lt.Text += csCont.TranslateVatTuBienDongValue(r["Note"].ToString());
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