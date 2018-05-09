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
using Microsoft.ApplicationBlocks.Data;
using DotNetNuke.Framework.Providers;

namespace DotNetNuke.Modules.QLCS
{
    public partial class r_cachet_phanloailydo : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        /*Must have for baocao coding template*/
        DataTable dt = null;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        /*End Must have for baocao coding template*/

        string[] lstNam = null;
        
        private void BindControls()
        {
            for (int i = 2012; i < DateTime.Now.Year + 1; i++)
            {
                lstYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindControls();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private string GetSelectedItems(ListBox l)
        {
            string res = "";
            foreach (ListItem i in l.Items)
            {
                if (i.Selected) res += "[" + i.Value + "],";
            }
            if (res != "") res = res.Substring(0, res.Length - 1);
            return res;
        }

        /*Must have for baocao coding template: fill data to dt and return title*/
        public string createDataAndTieuDe()
        {
            string year = GetSelectedItems(lstYear);
            lstNam = year.Split(new char[] { ',' });
            for (int i = 0; i< lstNam.Length; i++)
	        {
		        lstNam[i] = lstNam[i].Substring(1, lstNam[i].Length - 2);
	        }
            string strSQL = "QLCS_BCTK_CaChet_PhanLoaiLyDo";
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Nam", year);
            dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
            string tieude = "<b>TỔNG HỢP CÁ CHẾT</b>";
            return tieude;
        }

        /*Must have for baocao coding template: create table header (just the tr and th of the table header)*/
        public string createTableHeader()
        {
            string s = "<tr style='font-weight:bold; vertical-align:middle;'><th rowspan='2'>STT</th><th rowspan='2'>Đàn cá/Lý do chết</th>";
            foreach (string n in lstNam)
            {
                s += "<th colspan='2'>Năm " + n + "</th>";
            }
            s += "</tr><tr style='font-weight:bold; vertical-align:middle;'>";
            for (int i = 0; i < lstNam.Length; i++)
            {
                s += "<th>Số lượng</td><th>Tỷ lệ</th>";
            }
            s += "</tr>";
            return s;
        }

        /*Must have for baocao coding template: create content of the table (just the tr and td of the table)*/
        public void createContent(System.Text.StringBuilder sb)
        {
            int STT = 0;
            string currLoaiCa = "";
            int[] TongCa = new int[lstNam.Length];
            decimal[] TongPercent = new decimal[lstNam.Length];
            bool[] HadFirst = new bool[lstNam.Length];
            int[] TotalCa = new int[lstNam.Length];
            for (int j = 0; j < TotalCa.Length; j++)
            {
                TotalCa[j] = 0;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow r = dt.Rows[i];
                string tempLoaiCa = r["TenLoaiCa"].ToString();
                if (currLoaiCa != tempLoaiCa)
                {
                    for (int j = 0; j < TongCa.Length; j++)
                    {
                        TongCa[j] = 0;
                        TongPercent[j] = 0;
                        HadFirst[j] = false;
                    }
                    currLoaiCa = tempLoaiCa;
                    STT++;
                    sb.Append("<tr style='font-weight:bold;'><td style='text-align:center;'>" + STT.ToString() + "</td><td>" + currLoaiCa + "</td>");
                    for (int k = 0; k < lstNam.Length; k++ )
                    {
                        sb.Append("<td style='text-align:right;'>Tong" + k.ToString() + "</td><td></td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("<tr><td></td><td style='text-align:left;'>" + r["TenLyDoChet"].ToString() + "</td>");
                int m = 0;
                foreach (string item in lstNam)
                {
                    string res = r[item].ToString();
                    if (res == "")
                        sb.Append("<td></td><td></td>");
                    else
                    {
                        string[] aRes = res.Split(new char[] { '-' });
                        if (HadFirst[m])
                        {
                            sb.Append("<td align='right'>" + aRes[0] + "</td><td align='right'>" + aRes[1] + "%" + "</td>");
                            TongPercent[m] += decimal.Parse(aRes[1]);
                        }
                        else
                        {
                            sb.Append("<td align='right'>" + aRes[0] + "</td><td align='right'>P" + m.ToString() + "%" + "</td>");
                            HadFirst[m] = true;
                        }
                        TongCa[m] += int.Parse(aRes[0]);
                    }
                    m++;
                }
                sb.Append("</tr>");
                if (dt.Rows.Count - 1 == i || dt.Rows[i + 1]["TenLoaiCa"].ToString() != currLoaiCa)
                {
                    for (int h = 0; h< lstNam.Length; h++)
                    {
                        sb.Replace("Tong" + h.ToString(), TongCa[h].ToString());
                        TotalCa[h] += TongCa[h];
                        sb.Replace("P" + h.ToString(), ((decimal)(100 - TongPercent[h])).ToString());
                    }
                }
            }
            sb.Append("<tr style='font-weight:bold;'><td style='text-align:center;'>Tổng cộng</td><td></td>");
            for (int k = 0; k < lstNam.Length; k++)
            {
                sb.Append("<td style='text-align:right;'>" + TotalCa[k].ToString() + "</td><td></td>");
            }
            sb.Append("</tr>");
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //Just call this
                Config.exportExcel(Response, sb, "TongHopCaDe", createDataAndTieuDe, createContent, createTableHeader);

                Response.Write(sb.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                //Just call this
                Config.exportView(Response, sb, createDataAndTieuDe, createContent, createTableHeader);

                lt.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}