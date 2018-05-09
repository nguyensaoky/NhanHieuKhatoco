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
    public partial class gietmoca_sanpham_lichsu : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        private CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo eci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
                if (!IsPostBack)
                {
                    if (Request.QueryString["IDGietMoCaSanPham"] != null)
                    {
                        DataTable tblGietMoCaSanPham = null;
                        DataTable tblLichSu = null;
                        int IDGietMoCaSanPham = int.Parse(Request.QueryString["IDGietMoCaSanPham"]);
                        tblLichSu = csCont.LoadActionLogByRefIDByLoaiTable(IDGietMoCaSanPham, "GietMoCa_SanPham");
                        lt.Text = "<table><tr><th>Trạng thái</th><th>Thông tin sản phẩm giết mổ</th></tr>";
                        tblGietMoCaSanPham = csCont.GietMoCa_GetSanPhamByIDGMCSP(IDGietMoCaSanPham);
                        lt.Text += "<tr><td>Tạo bởi user <b>" + tblGietMoCaSanPham.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblGietMoCaSanPham.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        foreach (DataRow r in tblLichSu.Rows)
                        {
                            lt.Text += "<td>";
                            lt.Text += "Khối lượng: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(r["Note"], eci));
                            lt.Text += "</td></tr><tr><td>";
                            lt.Text += r["Status"].ToString() + " bởi user <b>" + r["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(r["TimeUpdate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>"; 
                        }
                        lt.Text += "<td>" + "Khối lượng: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(tblGietMoCaSanPham.Rows[0]["KhoiLuong"])) + "</td></tr>";
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