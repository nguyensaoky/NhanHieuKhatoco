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
    public partial class casau_biendong_lichsu : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
                if (!IsPostBack)
                {
                    if (Request.QueryString["IDBienDong"] != null && Request.QueryString["type"] != null && Request.QueryString["status"] != null)
                    {
                        DataTable tblBienDong = null;
                        DataTable tblLichSu = null;
                        int IDBienDong = int.Parse(Request.QueryString["IDBienDong"]);
                        tblLichSu = csCont.LoadActionLogByRefIDByLoaiTable(IDBienDong, "CaSau_BienDong");
                        lblLBD.Text = Request.QueryString["type"];
                        lt.Text = "<table><tr><th>Trạng thái</th><th>Nội dung biến động</th></tr>";
                        if (Request.QueryString["status"] == "1")
                        {
                            tblBienDong = csCont.LoadCaSauBienDongByID(IDBienDong);
                            lt.Text += "<tr><td>Tạo bởi user <b>" + tblBienDong.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblBienDong.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            foreach (DataRow r in tblLichSu.Rows)
                            {
                                string[] arr = r["Note"].ToString().Split(new char[] { '@' });
                                lt.Text += "<td>";
                                lt.Text += csCont.TranslateBienDongValue(Request.QueryString["type"], arr[2], arr[3]);
                                lt.Text += " lúc " + arr[1] + "</td></tr><tr><td>";
                                lt.Text += r["Status"].ToString() + " bởi user <b>" + r["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(r["TimeUpdate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>"; 
                            }
                            lt.Text += "<td>" + csCont.TranslateBienDongValue(Request.QueryString["type"], tblBienDong.Rows[0]) + " lúc " + Convert.ToDateTime(tblBienDong.Rows[0]["ThoiDiemBienDong"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td></tr>";
                        }
                        else if (Request.QueryString["status"] == "0")
                        {
                            tblBienDong = csCont.LoadCaSauBienDong_DeleteByID(IDBienDong);
                            lt.Text += "<tr><td>Tạo bởi user <b>" + tblBienDong.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblBienDong.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            foreach (DataRow r in tblLichSu.Rows)
                            {
                                string[] arr = r["Note"].ToString().Split(new char[] { '@' });
                                lt.Text += "<td>";
                                lt.Text += csCont.TranslateBienDongValue(Request.QueryString["type"], arr[2], arr[3]);
                                lt.Text += " lúc " + arr[1] + "</td></tr><tr><td>";
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