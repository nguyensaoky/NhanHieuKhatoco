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
    public partial class casau_chinhsuacachet_lichsu : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
                if (!IsPostBack)
                {
                    if (Request.QueryString["IDThuHoiDa"] != null && Request.QueryString["status"] != null)
                    {
                        DataTable tblThuHoiDa = null;
                        DataTable tblLichSu = null;
                        int IDThuHoiDa = int.Parse(Request.QueryString["IDThuHoiDa"]);
                        tblLichSu = csCont.LoadActionLogByRefIDByLoaiTable(IDThuHoiDa, "ThuHoiDa");
                        lt.Text = "<table><tr><th>Trạng thái</th><th>Thông số da thu hồi</th></tr>";
                        if (Request.QueryString["status"] == "1")
                        {
                            tblThuHoiDa = csCont.LoadThuHoiDaByID(IDThuHoiDa);
                            lt.Text += "<tr><td>Tạo bởi user <b>" + tblThuHoiDa.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblThuHoiDa.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            foreach (DataRow r in tblLichSu.Rows)
                            {
                                string[] arr = r["Note"].ToString().Split(new char[] { '@' });
                                lt.Text += "<td>";
                                lt.Text += csCont.TranslateThuHoiDaValue(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7]);
                                lt.Text += "</td></tr><tr><td>";
                                lt.Text += r["Status"].ToString() + " bởi user <b>" + r["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(r["TimeUpdate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>"; 
                            }
                            lt.Text += "<td>" + csCont.TranslateThuHoiDaValue(tblThuHoiDa.Rows[0]) + "</td></tr>";
                        }
                        else if (Request.QueryString["status"] == "0")
                        {
                            tblThuHoiDa = csCont.LoadThuHoiDaDeleteByID(IDThuHoiDa);
                            lt.Text += "<tr><td>Tạo bởi user <b>" + tblThuHoiDa.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblThuHoiDa.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            foreach (DataRow r in tblLichSu.Rows)
                            {
                                string[] arr = r["Note"].ToString().Split(new char[] { '@' });
                                lt.Text += "<td>";
                                lt.Text += csCont.TranslateThuHoiDaValue(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7]);
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