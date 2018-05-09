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
    public partial class r_soluongca_map : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        private void BindControls()
        {
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindControls();
                    btnView_Click(null, null);
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string strSQL = "QLCS_BCTK_SoLuongCa_Map";
                SqlParameter[] param = new SqlParameter[1];
                if (txtDate.Text == "")
                {
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@d", txtDate.Text);
                DataTable dt = Config.SelectSP(strSQL, param);

                HtmlControl c;
                foreach (DataRow r in dt.Rows)
                {
                    c = (HtmlControl)(this.FindControl(r["Chuong"].ToString().Replace(" ","")));
                    if (c != null)
                    {
                        if (r["NgayNhapChuong1"] != DBNull.Value)
                        {
                            if (Convert.ToDateTime(r["NgayNhapChuong1"]) == Convert.ToDateTime(r["NgayNhapChuong2"]))
                            {
                                c.Attributes.Add("content", "Tổng: " + r["SoLuongTong"].ToString() + ", Giống: " + r["SoLuongGiong"].ToString() + ", TT: " + r["SoLuongTT"].ToString() + ", Ngày nhập chuồng: " + Convert.ToDateTime(r["NgayNhapChuong1"]).ToString("dd/MM/yyyy"));
                            }
                            else
                            {
                                c.Attributes.Add("content", "Tổng: " + r["SoLuongTong"].ToString() + ", Giống: " + r["SoLuongGiong"].ToString() + ", TT: " + r["SoLuongTT"].ToString() + ", Ngày nhập chuồng: " + Convert.ToDateTime(r["NgayNhapChuong1"]).ToString("dd/MM/yyyy") + " - " + Convert.ToDateTime(r["NgayNhapChuong2"]).ToString("dd/MM/yyyy"));
                            }
                        }
                        else
                        {
                            c.Attributes.Add("content", "Tổng: " + r["SoLuongTong"].ToString() + ", Giống: " + r["SoLuongGiong"].ToString() + ", TT: " + r["SoLuongTT"].ToString());
                        }
                    }
                }
                Response.Flush();
            }
            catch (Exception ex)
            {
            }
        }
    }
}