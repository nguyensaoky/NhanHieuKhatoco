﻿using System;
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
    public partial class casauan_thucan_lichsu : DotNetNuke.Entities.Modules.PortalModuleBase
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
                    if (Request.QueryString["IDCaSauAnThucAn"] != null && Request.QueryString["status"] != null)
                    {
                        DataTable tblCaSauAnThucAn = null;
                        DataTable tblLichSu = null;
                        int IDCaSauAnThucAn = int.Parse(Request.QueryString["IDCaSauAnThucAn"]);
                        tblLichSu = csCont.LoadActionLogByRefIDByLoaiTable(IDCaSauAnThucAn, "CaSauAn_ThucAn");
                        lt.Text = "<table><tr><th>Trạng thái</th><th>Thông tin thức ăn cá ăn</th></tr>";
                        if (Request.QueryString["status"] == "1")
                        {
                            tblCaSauAnThucAn = csCont.CaSauAnThucAn_GetByID(IDCaSauAnThucAn,1);
                            lt.Text += "<tr><td>Tạo bởi user <b>" + tblCaSauAnThucAn.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblCaSauAnThucAn.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            foreach (DataRow r in tblLichSu.Rows)
                            {
                                lt.Text += "<td>";
                                lt.Text += csCont.TranslateCaSauAnThucAnValue(r["Note"].ToString(), ci);
                                lt.Text += "</td></tr><tr><td>";
                                lt.Text += r["Status"].ToString() + " bởi user <b>" + r["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(r["TimeUpdate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>"; 
                            }
                            lt.Text += "<td>" + csCont.TranslateCaSauAnThucAnValue(tblCaSauAnThucAn.Rows[0], ci) + "</td></tr>";
                        }
                        else if (Request.QueryString["status"] == "0")
                        {
                            tblCaSauAnThucAn = csCont.CaSauAnThucAn_GetByID(IDCaSauAnThucAn, 0);
                            lt.Text += "<tr><td>Tạo bởi user <b>" + tblCaSauAnThucAn.Rows[0]["Username"].ToString() + "</b> lúc " + Convert.ToDateTime(tblCaSauAnThucAn.Rows[0]["TimeCreate"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            foreach (DataRow r in tblLichSu.Rows)
                            {
                                lt.Text += "<td>";
                                lt.Text += csCont.TranslateCaSauAnThucAnValue(r["Note"].ToString(), ci);
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