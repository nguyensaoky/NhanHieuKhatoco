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
    public partial class note_lichsu : DotNetNuke.Entities.Modules.PortalModuleBase
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
                    if (Request.QueryString["IDNote"] != null && Request.QueryString["status"] != null)
                    {
                        DataTable tblNote = null;
                        DataTable tblLichSu = null;
                        int IDNote = int.Parse(Request.QueryString["IDNote"]);
                        tblLichSu = csCont.LoadActionLogByRefIDByLoaiTable(IDNote, "Note");
                        lt.Text = "<table><tr><th>Trạng thái</th><th>Nội dung ghi chú</th></tr>";
                        if (Request.QueryString["status"] == "1")
                        {
                            tblNote = csCont.LoadNoteByID(IDNote);
                            lt.Text += "<tr><td>Tạo bởi user <b>" + tblNote.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblNote.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            foreach (DataRow r in tblLichSu.Rows)
                            {
                                lt.Text += "<td>";
                                lt.Text += csCont.TranslateNoteValue(r["Note"].ToString());
                                lt.Text += "</td></tr><tr><td>";
                                lt.Text += r["Status"].ToString() + " bởi user <b>" + r["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(r["TimeUpdate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>"; 
                            }
                            lt.Text += "<td>" + csCont.TranslateNoteValue(tblNote.Rows[0]) + "</td></tr>";
                        }
                        else if (Request.QueryString["status"] == "0")
                        {
                            tblNote = csCont.LoadNoteDeleteByID(IDNote);
                            lt.Text += "<tr><td>Tạo bởi user <b>" + tblNote.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblNote.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            foreach (DataRow r in tblLichSu.Rows)
                            {
                                lt.Text += "<td>";
                                lt.Text += csCont.TranslateNoteValue(r["Note"].ToString());
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