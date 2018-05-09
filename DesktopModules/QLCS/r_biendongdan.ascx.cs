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
    public partial class r_biendongdan : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private const string TABLE_NAME = "THEPAGE";
        private const string ProviderType = "data";
        int scale = 0;
        private string strConn = DotNetNuke.Common.Utilities.Config.GetConnectionString();
        private string objectQualifier;
        private string databaseOwner;
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dtPLGM = null;
        DataTable dtPLGM1 = null;

        DataTable dt_DDA = null;
        DataTable dt1_DDA = null;
        DataTable dt_NgoaiDDA = null;
        DataTable dt1_NgoaiDDA = null;
        DataTable dtPLGM_DDA = null;
        DataTable dtPLGM1_DDA = null;
        DataTable dtPLGM_NgoaiDDA = null;
        DataTable dtPLGM1_NgoaiDDA = null;

        DataTable dtSub = null;
        DataTable dtSub_a = null;
        DataTable dtSub1 = null;
        DataTable dtSub1_a = null;
        DataTable dtSub2 = null;
        DataTable dtMax = null;
        DataTable dtTD = null;
        DataTable dtTDAn = null;

        //Data Chuyen giai doan
        DataTable dtPre = null;
        DataTable dt1Pre = null;
        DataTable dt_DDAPre = null;
        DataTable dt1_DDAPre = null;
        DataTable dt_NgoaiDDAPre = null;
        DataTable dt1_NgoaiDDAPre = null;
        DataTable dtTDPre = null;

        string ListThucAnTD = "";
        int SoCotThucAn = 0;
        string tieude = "";
        int iThucAn = 0;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        //private string GetFullyQualifiedName(string name)
        //{
        //    ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        //    Provider objProvider = (Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider];
        //    objectQualifier = objProvider.Attributes["objectQualifier"];
        //    if (!String.IsNullOrEmpty(objectQualifier) && objectQualifier.EndsWith("_") == false)
        //    {
        //        objectQualifier += "_";
        //    }
        //    databaseOwner = objProvider.Attributes["databaseOwner"];
        //    if (!String.IsNullOrEmpty(databaseOwner) && databaseOwner.EndsWith(".") == false)
        //    {
        //        databaseOwner += ".";
        //    }
        //    return databaseOwner + objectQualifier + name;
        //}

        //private string getConnectionString()
        //{
        //    return DotNetNuke.Common.Utilities.Config.GetConnectionString();
        //}

        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        int[] IDLoaiCa = new int[] {1,2,3,4,5,-1,6,7,8,9,11,10 };
        int[] IDLoaiCaDDA = new int[] { 4, 5, -1};

        private void BindControls()
        {
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate_GomCGD.Text = DateTime.Now.ToString("dd/MM/yyyy");
            for (int i = 2012; i < DateTime.Now.Year + 1; i++)
            {
                ddlNam.Items.Insert(0, new ListItem(i.ToString(), i.ToString()));
                ddlNam_GomCGD.Items.Insert(0, new ListItem(i.ToString(), i.ToString()));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
            try
            {
                scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_TA_Scale"]);
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

        private decimal GetKhoiLuongFromTable(DataTable dt, int ThucAn)
        {
            foreach (DataRow r in dt.Rows)
            {
                if (Convert.ToInt32(r["ThucAn"]) == ThucAn) return Convert.ToDecimal(r["KhoiLuong"]);
            }
            return 0;
        }

        public void prepareExcelFile(string name, System.Web.HttpResponse resp)
        {
            resp.ClearContent();
            resp.ClearHeaders();
            resp.AppendHeader("Content-Disposition", "attachment; filename=" + name + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
            resp.ContentType = "application/vnd.ms-excel";
        }

        public void prepareData()
        {
            string strSQL = "QLCS_BCTK_BienDongDan_JoinMauBC";
            string strSQL_DDA = "QLCS_BCTK_BienDongDan_JoinMauBC_DDA";
            string strSQL_NgoaiDDA = "QLCS_BCTK_BienDongDan_JoinMauBC_NgoaiDDA";
            
            string strSQL_PLGM = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo";
            string strSQL_PLGM_DDA = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_DDA";
            string strSQL_PLGM_NgoaiDDA = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_NgoaiDDA";
            if (ddlChuan.SelectedValue == "1")
            {
                strSQL_PLGM = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_ChuanCu";
                strSQL_PLGM_DDA = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_DDA_ChuanCu";
                strSQL_PLGM_NgoaiDDA = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_NgoaiDDA_ChuanCu";
            }

            string strSubSQL = "QLCS_BCTK_CaAn";
            string strMaxSQL = "QLCS_BCTK_MaxSoLoaiThucAn";
            string strSQLTD = "QLCS_BCTK_BienDongDan_CaTanDung";
            string strSQLTDAn = "QLCS_BCTK_CaKhongMeAn";
			string strSQLTDAn_Zero = "QLCS_BCTK_CaKhongMeAn_Zero";

            if (txtFromDate.Text == "")
            {
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            if (txtToDate.Text == "")
            {
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

            string sFrom = txtFromDate.Text;
            if (sFrom.StartsWith("01/10") || sFrom.StartsWith("1/10"))
                sFrom = sFrom + " 04:00:00";

            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@dFrom", sFrom);
            param[1] = new SqlParameter("@dTo", txtToDate.Text);
            DataSet ds = Config.SelectSPs(strSQL, param);
            dt = ds.Tables[0];
            dt1 = ds.Tables[1];

            SqlParameter[] paramPLGM = new SqlParameter[2];
            paramPLGM[0] = new SqlParameter("@dFrom", sFrom);
            paramPLGM[1] = new SqlParameter("@dTo", txtToDate.Text);
            DataSet dsPLGM = Config.SelectSPs(strSQL_PLGM, paramPLGM);
            dtPLGM = dsPLGM.Tables[0];
            dtPLGM1 = dsPLGM.Tables[1];

            SqlParameter[] param_DDA = new SqlParameter[2];
            param_DDA[0] = new SqlParameter("@dFrom", sFrom);
            param_DDA[1] = new SqlParameter("@dTo", txtToDate.Text);
            DataSet ds_DDA = Config.SelectSPs(strSQL_DDA, param_DDA);
            dt_DDA = ds_DDA.Tables[0];
            dt1_DDA = ds_DDA.Tables[1];

            SqlParameter[] param_NgoaiDDA = new SqlParameter[2];
            param_NgoaiDDA[0] = new SqlParameter("@dFrom", sFrom);
            param_NgoaiDDA[1] = new SqlParameter("@dTo", txtToDate.Text);
            DataSet ds_NgoaiDDA = Config.SelectSPs(strSQL_NgoaiDDA, param_NgoaiDDA);
            dt_NgoaiDDA = ds_NgoaiDDA.Tables[0];
            dt1_NgoaiDDA = ds_NgoaiDDA.Tables[1];

            SqlParameter[] paramPLGM_DDA = new SqlParameter[2];
            paramPLGM_DDA[0] = new SqlParameter("@dFrom", sFrom);
            paramPLGM_DDA[1] = new SqlParameter("@dTo", txtToDate.Text);
            DataSet dsPLGM_DDA = Config.SelectSPs(strSQL_PLGM_DDA, paramPLGM_DDA);
            dtPLGM_DDA = dsPLGM_DDA.Tables[0];
            dtPLGM1_DDA = dsPLGM_DDA.Tables[1];

            SqlParameter[] paramPLGM_NgoaiDDA = new SqlParameter[2];
            paramPLGM_NgoaiDDA[0] = new SqlParameter("@dFrom", sFrom);
            paramPLGM_NgoaiDDA[1] = new SqlParameter("@dTo", txtToDate.Text);
            DataSet dsPLGM_NgoaiDDA = Config.SelectSPs(strSQL_PLGM_NgoaiDDA, paramPLGM_NgoaiDDA);
            dtPLGM_NgoaiDDA = dsPLGM_NgoaiDDA.Tables[0];
            dtPLGM1_NgoaiDDA = dsPLGM_NgoaiDDA.Tables[1];

            SqlParameter[] paramSub = new SqlParameter[2];
            paramSub[0] = new SqlParameter("@dFrom", sFrom);
            paramSub[1] = new SqlParameter("@dTo", txtToDate.Text);
            DataSet dsSub = Config.SelectSPs(strSubSQL, paramSub);
            dtSub = dsSub.Tables[0];
            dtSub_a = dsSub.Tables[1];
            dtSub1 = dsSub.Tables[2];
            dtSub1_a = dsSub.Tables[3];
            dtSub2 = dsSub.Tables[4];

            SqlParameter[] param2 = new SqlParameter[2];
            param2[0] = new SqlParameter("@dFrom", sFrom);
            param2[1] = new SqlParameter("@dTo", txtToDate.Text);
            dtMax = DotNetNuke.NewsProvider.DataProvider.SelectSP(strMaxSQL, param2);
            dtTD = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTD, param2);

            SqlParameter[] param1 = new SqlParameter[4];
            param1[0] = new SqlParameter("@LoaiCa", 1);
            param1[1] = new SqlParameter("@dFrom", sFrom);
            param1[2] = new SqlParameter("@dTo", txtToDate.Text);
            param1[3] = new SqlParameter("@ListThucAn", "");
            param1[3].Direction = ParameterDirection.Output;
            param1[3].Size = 4000;
            //dtTDAn = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTDAn, param1);
            dtTDAn = Config.SelectSP(strSQLTDAn, param1);
			//dtTDAn = Config.SelectSP(strSQLTDAn_Zero, param1);

            ListThucAnTD = param1[3].Value.ToString();
            SoCotThucAn = 2;
            if (dtMax != null && dtMax.Rows.Count == 1 && dtMax.Rows[0]["MaxSoLoaiThucAn"] != DBNull.Value) SoCotThucAn = Convert.ToInt32(dtMax.Rows[0]["MaxSoLoaiThucAn"]) * 2;
            if (Convert.ToInt32(dtSub2.Rows[0]["MaxSoLoaiThucAnHauBi"]) * 2 > SoCotThucAn) SoCotThucAn = Convert.ToInt32(dtSub2.Rows[0]["MaxSoLoaiThucAnHauBi"]) * 2;
            if (SoCotThucAn < 2) SoCotThucAn = 2;
            tieude += "<b>BẢNG THEO DÕI BIẾN ĐỘNG TỔNG ĐÀN CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
        }

        public void prepareDataCGD()
        {
            string strSQL = "QLCS_BCTK_BienDongDan_JoinMauBC";
            string strSQL_DDA = "QLCS_BCTK_BienDongDan_JoinMauBC_DDA";
            string strSQL_NgoaiDDA = "QLCS_BCTK_BienDongDan_JoinMauBC_NgoaiDDA";
            string strSQLTD = "QLCS_BCTK_BienDongDan_CaTanDung";

            string sFrom = "01/10/" + ddlNam.SelectedValue + " 00:00:00";
            string sTo = "30/09/" + ddlNam.SelectedValue + " 04:00:00";
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@dFrom", sFrom);
            param[1] = new SqlParameter("@dTo", sTo);
            DataSet ds = Config.SelectSPs(strSQL, param);
            dt = ds.Tables[0];
            dt1 = ds.Tables[1];

            SqlParameter[] param_DDA = new SqlParameter[2];
            param_DDA[0] = new SqlParameter("@dFrom", sFrom);
            param_DDA[1] = new SqlParameter("@dTo", sTo);
            DataSet ds_DDA = Config.SelectSPs(strSQL_DDA, param_DDA);
            dt_DDA = ds_DDA.Tables[0];
            dt1_DDA = ds_DDA.Tables[1];

            SqlParameter[] param_NgoaiDDA = new SqlParameter[2];
            param_NgoaiDDA[0] = new SqlParameter("@dFrom", sFrom);
            param_NgoaiDDA[1] = new SqlParameter("@dTo", sTo);
            DataSet ds_NgoaiDDA = Config.SelectSPs(strSQL_NgoaiDDA, param_NgoaiDDA);
            dt_NgoaiDDA = ds_NgoaiDDA.Tables[0];
            dt1_NgoaiDDA = ds_NgoaiDDA.Tables[1];

            SqlParameter[] param2 = new SqlParameter[2];
            param2[0] = new SqlParameter("@dFrom", sFrom);
            param2[1] = new SqlParameter("@dTo", sTo);
            dtTD = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTD, param2);

            tieude += "<b>BẢNG THEO DÕI CHUYỂN GIAI ĐOẠN CÁ SẤU NĂM " + ddlNam.SelectedValue + "</b>";
        }

        public void prepareData_GomCGD()
        {
            if (txtToDate_GomCGD.Text == "")
            {
                txtToDate_GomCGD.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

            //Phan chuyen giai doan
            string strSQLPre = "QLCS_BCTK_BienDongDan_JoinMauBC";
            string strSQL_DDAPre = "QLCS_BCTK_BienDongDan_JoinMauBC_DDA";
            string strSQL_NgoaiDDAPre = "QLCS_BCTK_BienDongDan_JoinMauBC_NgoaiDDA";
            string strSQLTDPre = "QLCS_BCTK_BienDongDan_CaTanDung";

            string sFromPre = "01/10/" + ddlNam_GomCGD.SelectedValue + " 00:00:00";
            string sToPre = "30/09/" + ddlNam_GomCGD.SelectedValue + " 04:00:00";
            SqlParameter[] paramPre = new SqlParameter[2];
            paramPre[0] = new SqlParameter("@dFrom", sFromPre);
            paramPre[1] = new SqlParameter("@dTo", sToPre);
            DataSet dsPre = Config.SelectSPs(strSQLPre, paramPre);
            dtPre = dsPre.Tables[0];
            dt1Pre = dsPre.Tables[1];

            SqlParameter[] param_DDAPre = new SqlParameter[2];
            param_DDAPre[0] = new SqlParameter("@dFrom", sFromPre);
            param_DDAPre[1] = new SqlParameter("@dTo", sToPre);
            DataSet ds_DDAPre = Config.SelectSPs(strSQL_DDAPre, param_DDAPre);
            dt_DDAPre = ds_DDAPre.Tables[0];
            dt1_DDAPre = ds_DDAPre.Tables[1];

            SqlParameter[] param_NgoaiDDAPre = new SqlParameter[2];
            param_NgoaiDDAPre[0] = new SqlParameter("@dFrom", sFromPre);
            param_NgoaiDDAPre[1] = new SqlParameter("@dTo", sToPre);
            DataSet ds_NgoaiDDAPre = Config.SelectSPs(strSQL_NgoaiDDAPre, param_NgoaiDDAPre);
            dt_NgoaiDDAPre = ds_NgoaiDDAPre.Tables[0];
            dt1_NgoaiDDAPre = ds_NgoaiDDAPre.Tables[1];

            SqlParameter[] param2Pre = new SqlParameter[2];
            param2Pre[0] = new SqlParameter("@dFrom", sFromPre);
            param2Pre[1] = new SqlParameter("@dTo", sToPre);
            dtTDPre = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTDPre, param2Pre);

            //Phan bien dong sau do
            string strSQL = "QLCS_BCTK_BienDongDan_JoinMauBC";
            string strSQL_DDA = "QLCS_BCTK_BienDongDan_JoinMauBC_DDA";
            string strSQL_NgoaiDDA = "QLCS_BCTK_BienDongDan_JoinMauBC_NgoaiDDA";

            string strSQL_PLGM = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo";
            string strSQL_PLGM_DDA = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_DDA";
            string strSQL_PLGM_NgoaiDDA = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_NgoaiDDA";
            if (ddlChuan_GomCGD.SelectedValue == "1")
            {
                strSQL_PLGM = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_ChuanCu";
                strSQL_PLGM_DDA = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_DDA_ChuanCu";
                strSQL_PLGM_NgoaiDDA = "QLCS_BCTK_BienDongDan_JoinMauBC_PhanLoaiGietMo_NgoaiDDA_ChuanCu";
            }

            string strSubSQL = "QLCS_BCTK_CaAn";
            string strMaxSQL = "QLCS_BCTK_MaxSoLoaiThucAn";
            string strSQLTD = "QLCS_BCTK_BienDongDan_CaTanDung";
            string strSQLTDAn = "QLCS_BCTK_CaKhongMeAn";
            string strSQLTDAn_Zero = "QLCS_BCTK_CaKhongMeAn_Zero";

            string sFrom = "01/10/" + ddlNam_GomCGD.SelectedValue + " 04:00:00";

            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@dFrom", sFrom);
            param[1] = new SqlParameter("@dTo", txtToDate_GomCGD.Text);
            DataSet ds = Config.SelectSPs(strSQL, param);
            dt = ds.Tables[0];
            dt1 = ds.Tables[1];

            SqlParameter[] paramPLGM = new SqlParameter[2];
            paramPLGM[0] = new SqlParameter("@dFrom", sFrom);
            paramPLGM[1] = new SqlParameter("@dTo", txtToDate_GomCGD.Text);
            DataSet dsPLGM = Config.SelectSPs(strSQL_PLGM, paramPLGM);
            dtPLGM = dsPLGM.Tables[0];
            dtPLGM1 = dsPLGM.Tables[1];

            SqlParameter[] param_DDA = new SqlParameter[2];
            param_DDA[0] = new SqlParameter("@dFrom", sFrom);
            param_DDA[1] = new SqlParameter("@dTo", txtToDate_GomCGD.Text);
            DataSet ds_DDA = Config.SelectSPs(strSQL_DDA, param_DDA);
            dt_DDA = ds_DDA.Tables[0];
            dt1_DDA = ds_DDA.Tables[1];

            SqlParameter[] param_NgoaiDDA = new SqlParameter[2];
            param_NgoaiDDA[0] = new SqlParameter("@dFrom", sFrom);
            param_NgoaiDDA[1] = new SqlParameter("@dTo", txtToDate_GomCGD.Text);
            DataSet ds_NgoaiDDA = Config.SelectSPs(strSQL_NgoaiDDA, param_NgoaiDDA);
            dt_NgoaiDDA = ds_NgoaiDDA.Tables[0];
            dt1_NgoaiDDA = ds_NgoaiDDA.Tables[1];

            SqlParameter[] paramPLGM_DDA = new SqlParameter[2];
            paramPLGM_DDA[0] = new SqlParameter("@dFrom", sFrom);
            paramPLGM_DDA[1] = new SqlParameter("@dTo", txtToDate_GomCGD.Text);
            DataSet dsPLGM_DDA = Config.SelectSPs(strSQL_PLGM_DDA, paramPLGM_DDA);
            dtPLGM_DDA = dsPLGM_DDA.Tables[0];
            dtPLGM1_DDA = dsPLGM_DDA.Tables[1];

            SqlParameter[] paramPLGM_NgoaiDDA = new SqlParameter[2];
            paramPLGM_NgoaiDDA[0] = new SqlParameter("@dFrom", sFrom);
            paramPLGM_NgoaiDDA[1] = new SqlParameter("@dTo", txtToDate_GomCGD.Text);
            DataSet dsPLGM_NgoaiDDA = Config.SelectSPs(strSQL_PLGM_NgoaiDDA, paramPLGM_NgoaiDDA);
            dtPLGM_NgoaiDDA = dsPLGM_NgoaiDDA.Tables[0];
            dtPLGM1_NgoaiDDA = dsPLGM_NgoaiDDA.Tables[1];

            SqlParameter[] paramSub = new SqlParameter[2];
            paramSub[0] = new SqlParameter("@dFrom", sFrom);
            paramSub[1] = new SqlParameter("@dTo", txtToDate_GomCGD.Text);
            DataSet dsSub = Config.SelectSPs(strSubSQL, paramSub);
            dtSub = dsSub.Tables[0];
            dtSub_a = dsSub.Tables[1];
            dtSub1 = dsSub.Tables[2];
            dtSub1_a = dsSub.Tables[3];
            dtSub2 = dsSub.Tables[4];

            SqlParameter[] param2 = new SqlParameter[2];
            param2[0] = new SqlParameter("@dFrom", sFrom);
            param2[1] = new SqlParameter("@dTo", txtToDate_GomCGD.Text);
            dtMax = DotNetNuke.NewsProvider.DataProvider.SelectSP(strMaxSQL, param2);
            dtTD = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTD, param2);

            SqlParameter[] param1 = new SqlParameter[4];
            param1[0] = new SqlParameter("@LoaiCa", 1);
            param1[1] = new SqlParameter("@dFrom", sFrom);
            param1[2] = new SqlParameter("@dTo", txtToDate_GomCGD.Text);
            param1[3] = new SqlParameter("@ListThucAn", "");
            param1[3].Direction = ParameterDirection.Output;
            param1[3].Size = 4000;
            //dtTDAn = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQLTDAn, param1);
            dtTDAn = Config.SelectSP(strSQLTDAn, param1);
            //dtTDAn = Config.SelectSP(strSQLTDAn_Zero, param1);

            ListThucAnTD = param1[3].Value.ToString();
            SoCotThucAn = 2;
            if (dtMax != null && dtMax.Rows.Count == 1 && dtMax.Rows[0]["MaxSoLoaiThucAn"] != DBNull.Value) SoCotThucAn = Convert.ToInt32(dtMax.Rows[0]["MaxSoLoaiThucAn"]) * 2;
            if (Convert.ToInt32(dtSub2.Rows[0]["MaxSoLoaiThucAnHauBi"]) * 2 > SoCotThucAn) SoCotThucAn = Convert.ToInt32(dtSub2.Rows[0]["MaxSoLoaiThucAnHauBi"]) * 2;
            if (SoCotThucAn < 2) SoCotThucAn = 2;
            tieude += "<b>BẢNG THEO DÕI BIẾN ĐỘNG TỔNG ĐÀN CÁ SẤU TỪ NGÀY 01/10/" + ddlNam_GomCGD.SelectedValue + " ĐẾN NGÀY " + txtToDate_GomCGD.Text + "</b>";
        }

        public void prepareTitleExcel()
        {
            sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
        }

        public void prepareTitleView()
        {
            sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
        }

        public void prepareFooterExcel()
        {
            sb.Append("<br/><div style='text-align:right;font-style:italic;'>Ninh Ích, ngày&nbsp;&nbsp;&nbsp;tháng&nbsp;&nbsp;&nbsp;&nbsp;năm&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div></tr>");
            sb.Append("<br/><div style='font-weight:bold;text-align:left;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Giám đốc&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Kế toán trưởng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;BP. Theo dõi&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Lãnh đạo trại</div></body></html>");
        }

        public void createTableHeader(bool Excel)
        {
            if(Excel)
                sb.Append("<table border='1' id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
            else
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
            sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=3>STT</th>
                          <th rowspan=3>Nội dung</th>
                          <th colspan=3>Tồn đầu</th>
                          <th colspan=3>Nhập</th>
                          <th colspan=3>Xuất</th>
                          <th colspan=3>Bán</th>
                          <th colspan=3>Chết</th>
                          <th colspan=5>Giết mổ</th>
                          <th colspan=3>Tồn cuối</th>
                          <th colspan=" + SoCotThucAn.ToString() + @">Tiêu tốn thức ăn</th>
                          <th rowspan=3>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=4>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>");
            for (int ii = 0; ii < SoCotThucAn / 2; ii++)
            {
                sb.Append(@"<th rowspan=2>Loại</th>
                          <th rowspan=2>SL (kg)</th>");
            }

            sb.Append(@"</tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>TP</th>
                          <th>LT</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>");
            sb.Append("</tr></thead><tbody>");
        }

        public void createTableHeaderCGD(bool Excel)
        {
            if (Excel)
                sb.Append("<table border='1' id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
            else
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
            sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=3>STT</th>
                          <th rowspan=3>Nội dung</th>
                          <th colspan=3>Tồn đầu</th>
                          <th colspan=3>Nhập</th>
                          <th colspan=3>Xuất</th>
                          <th colspan=3>Tồn cuối</th>
                          <th rowspan=3>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                         </tr></thead><tbody>");
        }

        public void createTableHeader_GomCGD(bool Excel)
        {
            if (Excel)
                sb.Append("<table border='1' id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
            else
                sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
            sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=3>STT</th>
                          <th rowspan=3>Nội dung</th>
                          <th colspan=3>Tồn đầu</th>
                          <th colspan=3>Nhập chuyển GĐ</th>
                          <th colspan=3>Xuất chuyển GĐ</th>
                          <th colspan=3>Tồn đầu tháng 10</th>
                          <th colspan=3>Nhập</th>
                          <th colspan=3>Xuất</th>
                          <th colspan=3>Bán</th>
                          <th colspan=3>Chết</th>
                          <th colspan=5>Giết mổ</th>
                          <th colspan=3>Tồn cuối</th>
                          <th colspan=" + SoCotThucAn.ToString() + @">Tiêu tốn thức ăn</th>
                          <th rowspan=3>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=4>T.Đó</th>
                          <th rowspan=2>Tổng</th>
                          <th colspan=2>T.Đó</th>");
            for (int ii = 0; ii < SoCotThucAn / 2; ii++)
            {
                sb.Append(@"<th rowspan=2>Loại</th>
                          <th rowspan=2>SL (kg)</th>");
            }
            sb.Append(@"</tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>TP</th>
                          <th>LT</th>
                          <th>Đ</th>
                          <th>C</th>
                          <th>Đ</th>
                          <th>C</th>
                         </tr></thead><tbody>");
        }

        /*****************************************************************/

        public void CaCon_Others()
        {
            int i = 0;
            //Cá loại 1 (cá con)
            DataRow[] r1 = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r1[jj] = dt.Rows[i * 6 + jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>1.1</td>");

            int[,] GiaTri1 = new int[3, 8];
            int t1;

            //-------------------------------------------

            //GiaTri1[1, 0] - Giong Ton Dau
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 0] += Convert.ToInt32(r1[t1]["TonDau"]);// +Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
            }
            //GiaTri1[2, 0] - Tang Trong Ton Dau
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 0] += Convert.ToInt32(r1[t1]["TonDau"]);// +Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
            }
            //GiaTri1[0, 0] - Tong Ton Dau
            GiaTri1[0, 0] = GiaTri1[1, 0] + GiaTri1[2, 0];

            //-------------------------------------------

            //GiaTri1[1, 1] - Giong Nhap
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]);// - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
            }
            //GiaTri1[2, 1] - Tang Trong Nhap
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]);// - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
            }
            //GiaTri1[0, 1] - Tong Nhap
            GiaTri1[0, 1] = GiaTri1[1, 1] + GiaTri1[2, 1];

            //-------------------------------------------

            //GiaTri1[1, 2] - Giong Xuat
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 2] += Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGTO"]) + Convert.ToInt32(r1[t1]["Xuat_CGTI"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPLO"]) + Convert.ToInt32(r1[t1]["Xuat_CPLI"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
            }
            //GiaTri1[2, 2] - Tang Trong Xuat
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 2] += Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGTO"]) + Convert.ToInt32(r1[t1]["Xuat_CGTI"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPLO"]) + Convert.ToInt32(r1[t1]["Xuat_CPLI"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
            }
            //GiaTri1[0, 2] - Tong Xuat
            GiaTri1[0, 2] = GiaTri1[1, 2] + GiaTri1[2, 2];

            //-------------------------------------------

            //GiaTri1[1, 2] - Giong Ban
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 3] += Convert.ToInt32(r1[t1]["Xuat_Ban"]);
            }
            //GiaTri1[2, 2] - Tang Trong Ban
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 3] += Convert.ToInt32(r1[t1]["Xuat_Ban"]);
            }
            //GiaTri1[0, 2] - Tong Ban
            GiaTri1[0, 3] = GiaTri1[1, 3] + GiaTri1[2, 3];

            //-------------------------------------------

            //GiaTri1[1, 3] - Giong Chet
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 4] += Convert.ToInt32(r1[t1]["Chet"]);
            }
            //GiaTri1[2, 3] - Tang Trong Chet
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 4] += Convert.ToInt32(r1[t1]["Chet"]);
            }
            //GiaTri1[0, 3] - Tong Chet
            GiaTri1[0, 4] = GiaTri1[1, 4] + GiaTri1[2, 4];

            //-------------------------------------------

            //GiaTri1[1, 3] - Giong LoaiThai
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 5] += Convert.ToInt32(r1[t1]["LoaiThai"]);
            }
            //GiaTri1[2, 3] - Tang Trong Chet
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 5] += Convert.ToInt32(r1[t1]["LoaiThai"]);
            }
            //GiaTri1[0, 3] - Tong Chet
            GiaTri1[0, 5] = GiaTri1[1, 5] + GiaTri1[2, 5];

            //-------------------------------------------

            //GiaTri1[1, 4] - Giong Giet Mo
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 6] += Convert.ToInt32(r1[t1]["GietMo"]);
            }
            //GiaTri1[2, 4] - Tang Trong Giet Mo
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 6] += Convert.ToInt32(r1[t1]["GietMo"]);
            }
            //GiaTri1[0, 4] - Tong Giet Mo
            GiaTri1[0, 6] = GiaTri1[1, 6] + GiaTri1[2, 6];

            //-------------------------------------------

            //GiaTri1[1, 5] - Giong Ton Cuoi
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 7] += Convert.ToInt32(r1[t1]["TonCuoi"]);
            }
            //GiaTri1[2, 5] - Tang Trong Ton Cuoi
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 7] += Convert.ToInt32(r1[t1]["TonCuoi"]);
            }
            //GiaTri1[0, 5] - Tong Ton Cuoi
            GiaTri1[0, 7] = GiaTri1[1, 7] + GiaTri1[2, 7];

            //-------------------------------------------

            DataRow rTD = dtTD.Rows[0];
            sb.Append("<td align='left'>Cá GĐ úm</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 0] - Convert.ToInt32(rTD["TonDau"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 1] - (Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"])), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 2] - Convert.ToInt32(rTD["Xuat_CLC"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 3] - Convert.ToInt32(rTD["Xuat_Ban"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 4] - Convert.ToInt32(rTD["Chet"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 6] - Convert.ToInt32(rTD["GietMo"]) + GiaTri1[0, 5] - Convert.ToInt32(rTD["LoaiThai"]), 0) + "</td><td></td><td>" + Config.ToXVal2(GiaTri1[0, 6] - Convert.ToInt32(rTD["GietMo"]) + GiaTri1[0, 5] - Convert.ToInt32(rTD["LoaiThai"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 7] - Convert.ToInt32(rTD["TonCuoi"]), 0) + "</td><td></td><td></td>");

            //ThucAn Start
            int j = 0;
            for (int h = iThucAn; h < dtSub.Rows.Count; h++)
            {
                DataRow rta = dtSub.Rows[h];
                if (rta["LoaiCa"].ToString() == "1")
                {
                    if (ListThucAnTD.Contains("@" + rta["ThucAn"].ToString() + "@"))
                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(Convert.ToDecimal(rta["KhoiLuong"]) - GetKhoiLuongFromTable(dtTDAn, Convert.ToInt32(rta["ThucAn"])), scale) + "</td>");
                    else
                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                    j++;
                }
                else
                {
                    iThucAn = h;
                    break;
                }
            }
            for (int t = j; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
            //ThucAn End
            sb.Append("<td></td>");
            sb.Append("</tr>");
        }

        public void CaCon_FirstOctCGD()
        {
            int i = 0;
            //Cá loại 1 (cá con)
            DataRow[] r1 = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r1[jj] = dt.Rows[i * 6 + jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>1.1</td>");

            int[,] GiaTri1 = new int[3, 8];
            int t1;

            //-------------------------------------------

            //GiaTri1[1, 0] - Giong Ton Dau
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 0] += Convert.ToInt32(r1[t1]["TonDau"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
            }
            //GiaTri1[2, 0] - Tang Trong Ton Dau
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 0] += Convert.ToInt32(r1[t1]["TonDau"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
            }
            //GiaTri1[0, 0] - Tong Ton Dau
            GiaTri1[0, 0] = GiaTri1[1, 0] + GiaTri1[2, 0];

            //-------------------------------------------

            //GiaTri1[1, 1] - Giong Nhap
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]) - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
            }
            //GiaTri1[2, 1] - Tang Trong Nhap
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]) - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
            }
            //GiaTri1[0, 1] - Tong Nhap
            GiaTri1[0, 1] = GiaTri1[1, 1] + GiaTri1[2, 1];

            //-------------------------------------------

            //GiaTri1[1, 2] - Giong Xuat
            for (t1 = 0; t1 < 3; t1++)
            {
                //GiaTri1[1, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]) - Convert.ToInt32(r1[t1]["Nhap_CGT"]) - Convert.ToInt32(r1[t1]["Nhap_CPL"]);
                GiaTri1[1, 2] += Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
            }
            //GiaTri1[2, 2] - Tang Trong Xuat
            for (t1 = 3; t1 < 6; t1++)
            {
                //GiaTri1[2, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]) - Convert.ToInt32(r1[t1]["Nhap_CGT"]) - Convert.ToInt32(r1[t1]["Nhap_CPL"]);
                GiaTri1[2, 2] += Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
            }
            //GiaTri1[0, 2] - Tong Xuat
            GiaTri1[0, 2] = GiaTri1[1, 2] + GiaTri1[2, 2];

            //-------------------------------------------

            //GiaTri1[1, 5] - Giong Ton Cuoi
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 7] += Convert.ToInt32(r1[t1]["TonCuoi"]);
            }
            //GiaTri1[2, 5] - Tang Trong Ton Cuoi
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 7] += Convert.ToInt32(r1[t1]["TonCuoi"]);
            }
            //GiaTri1[0, 5] - Tong Ton Cuoi
            GiaTri1[0, 7] = GiaTri1[1, 7] + GiaTri1[2, 7];

            //-------------------------------------------

            DataRow rTD = dtTD.Rows[0];
            sb.Append("<td align='left'>Cá GĐ úm</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 0] - Convert.ToInt32(rTD["TonDau"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 1] - (Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"])), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 2] - Convert.ToInt32(rTD["Xuat_CLC"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 7] - Convert.ToInt32(rTD["TonCuoi"]), 0) + "</td><td></td><td></td>");

            sb.Append("<td></td>");
            sb.Append("</tr>");
        }

        public void CaCon_GomCGD()
        {
            int i = 0;
            //Cá loại 1 (cá con)
            DataRow[] r1 = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r1[jj] = dtPre.Rows[i * 6 + jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>1.1</td>");

            int[,] GiaTri1 = new int[3, 8];
            int t1;

            //-------------------------------------------

            //GiaTri1[1, 0] - Giong Ton Dau
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 0] += Convert.ToInt32(r1[t1]["TonDau"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
            }
            //GiaTri1[2, 0] - Tang Trong Ton Dau
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 0] += Convert.ToInt32(r1[t1]["TonDau"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
            }
            //GiaTri1[0, 0] - Tong Ton Dau
            GiaTri1[0, 0] = GiaTri1[1, 0] + GiaTri1[2, 0];

            //-------------------------------------------

            //GiaTri1[1, 1] - Giong Nhap
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]) - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
            }
            //GiaTri1[2, 1] - Tang Trong Nhap
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]) - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
            }
            //GiaTri1[0, 1] - Tong Nhap
            GiaTri1[0, 1] = GiaTri1[1, 1] + GiaTri1[2, 1];

            //-------------------------------------------

            //GiaTri1[1, 2] - Giong Xuat
            for (t1 = 0; t1 < 3; t1++)
            {
                //GiaTri1[1, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]) - Convert.ToInt32(r1[t1]["Nhap_CGT"]) - Convert.ToInt32(r1[t1]["Nhap_CPL"]);
                GiaTri1[1, 2] += Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
            }
            //GiaTri1[2, 2] - Tang Trong Xuat
            for (t1 = 3; t1 < 6; t1++)
            {
                //GiaTri1[2, 2] += Convert.ToInt32(r1[t1]["Xuat_Ban"]) + Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]) - Convert.ToInt32(r1[t1]["Nhap_CGT"]) - Convert.ToInt32(r1[t1]["Nhap_CPL"]);
                GiaTri1[2, 2] += Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
            }
            //GiaTri1[0, 2] - Tong Xuat
            GiaTri1[0, 2] = GiaTri1[1, 2] + GiaTri1[2, 2];

            //-------------------------------------------

            DataRow rTD = dtTDPre.Rows[0];
            sb.Append("<td align='left'>Cá GĐ úm</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 0] - Convert.ToInt32(rTD["TonDau"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 1] - (Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"])), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 2] - Convert.ToInt32(rTD["Xuat_CLC"]), 0) + "</td><td></td><td></td>");

            

            /************************/




            i = 0;
            //Cá loại 1 (cá con)
            r1 = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r1[jj] = dt.Rows[i * 6 + jj];
            }
            GiaTri1 = new int[3, 8];

            //-------------------------------------------

            //GiaTri1[1, 0] - Giong Ton Dau
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 0] += Convert.ToInt32(r1[t1]["TonDau"]);// +Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
            }
            //GiaTri1[2, 0] - Tang Trong Ton Dau
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 0] += Convert.ToInt32(r1[t1]["TonDau"]);// +Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) - Convert.ToInt32(r1[t1]["Xuat_CGTO"]) - Convert.ToInt32(r1[t1]["Xuat_CPLO"]);
            }
            //GiaTri1[0, 0] - Tong Ton Dau
            GiaTri1[0, 0] = GiaTri1[1, 0] + GiaTri1[2, 0];

            //-------------------------------------------

            //GiaTri1[1, 1] - Giong Nhap
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]);// - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
            }
            //GiaTri1[2, 1] - Tang Trong Nhap
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 1] += Convert.ToInt32(r1[t1]["Nhap_NhapChuong"]) + Convert.ToInt32(r1[t1]["Nhap_CLC"]) + Convert.ToInt32(r1[t1]["Nhap_CGTO"]) + Convert.ToInt32(r1[t1]["Nhap_CGTI"]) + Convert.ToInt32(r1[t1]["Nhap_CGT_D"]) + Convert.ToInt32(r1[t1]["Nhap_CPLO"]) + Convert.ToInt32(r1[t1]["Nhap_CPLI"]) + Convert.ToInt32(r1[t1]["Nhap_CPL_D"]) + Convert.ToInt32(r1[t1]["Nhap_CTT"]);// - Convert.ToInt32(r1[t1]["Xuat_CGTI"]) - Convert.ToInt32(r1[t1]["Xuat_CPLI"]);
            }
            //GiaTri1[0, 1] - Tong Nhap
            GiaTri1[0, 1] = GiaTri1[1, 1] + GiaTri1[2, 1];

            //-------------------------------------------

            //GiaTri1[1, 2] - Giong Xuat
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 2] += Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGTO"]) + Convert.ToInt32(r1[t1]["Xuat_CGTI"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPLO"]) + Convert.ToInt32(r1[t1]["Xuat_CPLI"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
            }
            //GiaTri1[2, 2] - Tang Trong Xuat
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 2] += Convert.ToInt32(r1[t1]["Xuat_CLC"]) + Convert.ToInt32(r1[t1]["Xuat_CGTO"]) + Convert.ToInt32(r1[t1]["Xuat_CGTI"]) + Convert.ToInt32(r1[t1]["Xuat_CGT_D"]) + Convert.ToInt32(r1[t1]["Xuat_CPLO"]) + Convert.ToInt32(r1[t1]["Xuat_CPLI"]) + Convert.ToInt32(r1[t1]["Xuat_CPL_D"]);
            }
            //GiaTri1[0, 2] - Tong Xuat
            GiaTri1[0, 2] = GiaTri1[1, 2] + GiaTri1[2, 2];

            //-------------------------------------------

            //GiaTri1[1, 2] - Giong Ban
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 3] += Convert.ToInt32(r1[t1]["Xuat_Ban"]);
            }
            //GiaTri1[2, 2] - Tang Trong Ban
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 3] += Convert.ToInt32(r1[t1]["Xuat_Ban"]);
            }
            //GiaTri1[0, 2] - Tong Ban
            GiaTri1[0, 3] = GiaTri1[1, 3] + GiaTri1[2, 3];

            //-------------------------------------------

            //GiaTri1[1, 3] - Giong Chet
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 4] += Convert.ToInt32(r1[t1]["Chet"]);
            }
            //GiaTri1[2, 3] - Tang Trong Chet
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 4] += Convert.ToInt32(r1[t1]["Chet"]);
            }
            //GiaTri1[0, 3] - Tong Chet
            GiaTri1[0, 4] = GiaTri1[1, 4] + GiaTri1[2, 4];

            //-------------------------------------------

            //GiaTri1[1, 3] - Giong LoaiThai
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 5] += Convert.ToInt32(r1[t1]["LoaiThai"]);
            }
            //GiaTri1[2, 3] - Tang Trong Chet
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 5] += Convert.ToInt32(r1[t1]["LoaiThai"]);
            }
            //GiaTri1[0, 3] - Tong Chet
            GiaTri1[0, 5] = GiaTri1[1, 5] + GiaTri1[2, 5];

            //-------------------------------------------

            //GiaTri1[1, 4] - Giong Giet Mo
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 6] += Convert.ToInt32(r1[t1]["GietMo"]);
            }
            //GiaTri1[2, 4] - Tang Trong Giet Mo
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 6] += Convert.ToInt32(r1[t1]["GietMo"]);
            }
            //GiaTri1[0, 4] - Tong Giet Mo
            GiaTri1[0, 6] = GiaTri1[1, 6] + GiaTri1[2, 6];

            //-------------------------------------------

            //GiaTri1[1, 5] - Giong Ton Cuoi
            for (t1 = 0; t1 < 3; t1++)
            {
                GiaTri1[1, 7] += Convert.ToInt32(r1[t1]["TonCuoi"]);
            }
            //GiaTri1[2, 5] - Tang Trong Ton Cuoi
            for (t1 = 3; t1 < 6; t1++)
            {
                GiaTri1[2, 7] += Convert.ToInt32(r1[t1]["TonCuoi"]);
            }
            //GiaTri1[0, 5] - Tong Ton Cuoi
            GiaTri1[0, 7] = GiaTri1[1, 7] + GiaTri1[2, 7];

            //-------------------------------------------

            rTD = dtTD.Rows[0];
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 0] - Convert.ToInt32(rTD["TonDau"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 1] - (Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"])), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 2] - Convert.ToInt32(rTD["Xuat_CLC"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 3] - Convert.ToInt32(rTD["Xuat_Ban"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 4] - Convert.ToInt32(rTD["Chet"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 6] - Convert.ToInt32(rTD["GietMo"]) + GiaTri1[0, 5] - Convert.ToInt32(rTD["LoaiThai"]), 0) + "</td><td></td><td>" + Config.ToXVal2(GiaTri1[0, 6] - Convert.ToInt32(rTD["GietMo"]) + GiaTri1[0, 5] - Convert.ToInt32(rTD["LoaiThai"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri1[0, 7] - Convert.ToInt32(rTD["TonCuoi"]), 0) + "</td><td></td><td></td>");

            //ThucAn Start
            int j = 0;
            for (int h = iThucAn; h < dtSub.Rows.Count; h++)
            {
                DataRow rta = dtSub.Rows[h];
                if (rta["LoaiCa"].ToString() == "1")
                {
                    if (ListThucAnTD.Contains("@" + rta["ThucAn"].ToString() + "@"))
                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(Convert.ToDecimal(rta["KhoiLuong"]) - GetKhoiLuongFromTable(dtTDAn, Convert.ToInt32(rta["ThucAn"])), scale) + "</td>");
                    else
                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                    j++;
                }
                else
                {
                    iThucAn = h;
                    break;
                }
            }
            for (int t = j; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
            //ThucAn End
            sb.Append("<td></td>");
            sb.Append("</tr>");
        }


        public void CaTanDung()
        {
            DataRow rTD = dtTD.Rows[0];
            //Cá tận dụng
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>1.2</td>");
            sb.Append("<td align='left'>Cá GĐ úm tận dụng</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonDau"], 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Xuat_CLC"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Xuat_Ban"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["Chet"], 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["GietMo"]) + Convert.ToInt32(rTD["LoaiThai"]), 0) + "</td><td></td><td>" + Config.ToXVal2(Convert.ToInt32(rTD["GietMo"]) + Convert.ToInt32(rTD["LoaiThai"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonCuoi"], 0) + "</td><td></td><td></td>");
            //ThucAn Start
            if (ListThucAnTD == "")
            {
                for (int t = 0; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
            }
            else
            {
                int o = 0;
                for (o = 0; o < dtTDAn.Rows.Count; o++)
                {
                    DataRow r = dtTDAn.Rows[o];
                    sb.Append("<td>" + r["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(r["KhoiLuong"], scale) + "</td>");
					//sb.Append("<td></td><td></td>");
                }
                for (int t = o; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
            }
            //ThucAn End
            sb.Append("<td></td>");
            sb.Append("</tr>");
        }

        public void CaTanDungCGD()
        {
            DataRow rTD = dtTD.Rows[0];
            //Cá tận dụng
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>1.2</td>");
            sb.Append("<td align='left'>Cá GĐ úm tận dụng</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonDau"], 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Xuat_CLC"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonCuoi"], 0) + "</td><td></td><td></td>");
            sb.Append("<td></td>");
            sb.Append("</tr>");
        }

        public void CaTanDung_GomCGD()
        {
            DataRow rTD = dtTDPre.Rows[0];
            //Cá tận dụng
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>1.2</td>");
            sb.Append("<td align='left'>Cá GĐ úm tận dụng</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonDau"], 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Xuat_CLC"]), 0) + "</td><td></td><td></td>");


            /****************************/


            rTD = dtTD.Rows[0];
            //Cá tận dụng
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonDau"], 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Nhap_NhapChuong"]) + Convert.ToInt32(rTD["Nhap_CLC"]) + Convert.ToInt32(rTD["Nhap_CTT"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Xuat_CLC"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["Xuat_Ban"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["Chet"], 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(rTD["GietMo"]) + Convert.ToInt32(rTD["LoaiThai"]), 0) + "</td><td></td><td>" + Config.ToXVal2(Convert.ToInt32(rTD["GietMo"]) + Convert.ToInt32(rTD["LoaiThai"]), 0) + "</td><td></td><td></td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(rTD["TonCuoi"], 0) + "</td><td></td><td></td>");
            //ThucAn Start
            if (ListThucAnTD == "")
            {
                for (int t = 0; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
            }
            else
            {
                int o = 0;
                for (o = 0; o < dtTDAn.Rows.Count; o++)
                {
                    DataRow r = dtTDAn.Rows[o];
                    sb.Append("<td>" + r["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(r["KhoiLuong"], scale) + "</td>");
                    //sb.Append("<td></td><td></td>");
                }
                for (int t = o; t < SoCotThucAn / 2; t++) sb.Append("<td></td><td></td>");
            }
            //ThucAn End
            sb.Append("<td></td>");
            sb.Append("</tr>");
        }


        public void CaMotNamST1_Others()
        {
            //Cá từ loại 2 -> loại 3
            for (int i = 1; i < 3; i++)
            {
                DataRow[] r = new DataRow[6];
                for (int jj = 0; jj < 6; jj++)
                {
                    r[jj] = dt.Rows[i * 6 + jj];
                }
                sb.Append("<tr>");
                int idx = i + 1;
                sb.Append("<td style='vertical-align:middle;'>" + idx.ToString() + "</td>");
                string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                int[,] GiaTri = new int[3, 8];
                int t;

                //-------------------------------------------

                //GiaTri[1, 0] - Giong Ton Dau
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[2, 0] - Tang Trong Ton Dau
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[0, 0] - Tong Ton Dau
                GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];

                //-------------------------------------------

                //GiaTri[1, 1] - Giong Nhap
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[2, 1] - Tang Trong Nhap
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[0, 1] - Tong Nhap
                GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];

                //-------------------------------------------

                //GiaTri[1, 2] - Giong Xuat
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[2, 2] - Tang Trong Xuat
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[0, 2] - Tong Xuat
                GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];

                //-------------------------------------------

                //GiaTri[1, 2] - Giong Ban
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 3] += Convert.ToInt32(r[t]["Xuat_Ban"]);
                }
                //GiaTri[2, 2] - Tang Trong Ban
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 3] += Convert.ToInt32(r[t]["Xuat_Ban"]);
                }
                //GiaTri[0, 2] - Tong Ban
                GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];

                //-------------------------------------------

                //GiaTri[1, 3] - Giong Chet
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 4] += Convert.ToInt32(r[t]["Chet"]);
                }
                //GiaTri[2, 3] - Tang Trong Chet
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 4] += Convert.ToInt32(r[t]["Chet"]);
                }
                //GiaTri[0, 3] - Tong Chet
                GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];

                //-------------------------------------------

                //GiaTri[1, 3] - Giong LoaiThai
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 5] += Convert.ToInt32(r[t]["LoaiThai"]);
                }
                //GiaTri[2, 3] - Tang Trong Chet
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 5] += Convert.ToInt32(r[t]["LoaiThai"]);
                }
                //GiaTri[0, 3] - Tong Chet
                GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];

                //-------------------------------------------

                //GiaTri[1, 4] - Giong Giet Mo
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 6] += Convert.ToInt32(r[t]["GietMo"]);
                }
                //GiaTri[2, 4] - Tang Trong Giet Mo
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 6] += Convert.ToInt32(r[t]["GietMo"]);
                }
                //GiaTri[0, 4] - Tong Giet Mo
                GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];

                //-------------------------------------------

                //GiaTri[1, 5] - Giong Ton Cuoi
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[2, 5] - Tang Trong Ton Cuoi
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[0, 5] - Tong Ton Cuoi
                GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];

                //-------------------------------------------

                int iThucAnGoc = iThucAn;
                for (t = 0; t < 3; t++)
                {
                    if (t == 0)
                    {
                        sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                        for (int y = 0; y < 5; y++)
                        {
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                        }
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6] + GiaTri[t, 5], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 3]["GietMo"]), 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 2]["GietMo"]) + GiaTri[t, 5], 0) + "</td><td></td><td></td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");

                        //ThucAn Start
                        int j = 0;
                        for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                        {
                            DataRow rta = dtSub.Rows[h];
                            if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                j++;
                            }
                            else
                            {
                                iThucAn = h;
                                break;
                            }
                        }
                        for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                        //ThucAn End
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.Append("<tr><td></td>");
                        sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                        for (int y = 0; y < 5; y++)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                        }
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6] + GiaTri[t, 5], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + t * 2 - 2]["GietMo"]) + GiaTri[t, 5], 0) + "</td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");
                        //
                        //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                        //ThucAn Start
                        int j = 0;
                        for (int h = iThucAnGoc; h < dtSub.Rows.Count; h++)
                        {
                            DataRow rta = dtSub.Rows[h];
                            if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                            {
                                if (t == 1)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                }
                                j++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                        //ThucAn End
                        //
                        sb.Append("<td></td>");
                    }
                    sb.Append("</tr>");
                }
            }
        }

        public void CaMotNamST1_FirstOctCGD()
        {
            //Cá từ loại 2 -> loại 3
            for (int i = 1; i < 3; i++)
            {
                DataRow[] r = new DataRow[6];
                for (int jj = 0; jj < 6; jj++)
                {
                    r[jj] = dt.Rows[i * 6 + jj];
                }
                sb.Append("<tr>");
                int idx = i + 1;
                sb.Append("<td style='vertical-align:middle;'>" + idx.ToString() + "</td>");
                string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                int[,] GiaTri = new int[3, 8];
                int t;

                //-------------------------------------------

                //GiaTri[1, 0] - Giong Ton Dau
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[2, 0] - Tang Trong Ton Dau
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[0, 0] - Tong Ton Dau
                GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];

                //-------------------------------------------

                //GiaTri[1, 1] - Giong Nhap
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[2, 1] - Tang Trong Nhap
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[0, 1] - Tong Nhap
                GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];

                //-------------------------------------------

                //GiaTri[1, 2] - Giong Xuat
                for (t = 0; t < 3; t++)
                {
                    //GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                    GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[2, 2] - Tang Trong Xuat
                for (t = 3; t < 6; t++)
                {
                    //GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                    GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[0, 2] - Tong Xuat
                GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];

                //-------------------------------------------

                //GiaTri[1, 5] - Giong Ton Cuoi
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[2, 5] - Tang Trong Ton Cuoi
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[0, 5] - Tong Ton Cuoi
                GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];

                //-------------------------------------------

                for (t = 0; t < 3; t++)
                {
                    if (t == 0)
                    {
                        sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                        for (int y = 0; y < 3; y++)
                        {
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                        }
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.Append("<tr><td></td>");
                        sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                        for (int y = 0; y < 3; y++)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                        }
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");
                        sb.Append("<td></td>");
                    }
                    sb.Append("</tr>");
                }
            }
        }

        public void CaMotNamST1_GomCGD()
        {
            //Cá từ loại 2 -> loại 3
            for (int i = 1; i < 3; i++)
            {
                DataRow[] r = new DataRow[6];
                for (int jj = 0; jj < 6; jj++)
                {
                    r[jj] = dtPre.Rows[i * 6 + jj];
                }
                sb.Append("<tr>");
                int idx = i + 1;
                sb.Append("<td style='vertical-align:middle;'>" + idx.ToString() + "</td>");
                string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                int[,] GiaTriPre = new int[3, 8];
                int t;

                //-------------------------------------------

                //GiaTri[1, 0] - Giong Ton Dau
                for (t = 0; t < 3; t++)
                {
                    GiaTriPre[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[2, 0] - Tang Trong Ton Dau
                for (t = 3; t < 6; t++)
                {
                    GiaTriPre[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[0, 0] - Tong Ton Dau
                GiaTriPre[0, 0] = GiaTriPre[1, 0] + GiaTriPre[2, 0];

                //-------------------------------------------

                //GiaTri[1, 1] - Giong Nhap
                for (t = 0; t < 3; t++)
                {
                    GiaTriPre[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[2, 1] - Tang Trong Nhap
                for (t = 3; t < 6; t++)
                {
                    GiaTriPre[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[0, 1] - Tong Nhap
                GiaTriPre[0, 1] = GiaTriPre[1, 1] + GiaTriPre[2, 1];

                //-------------------------------------------

                //GiaTri[1, 2] - Giong Xuat
                for (t = 0; t < 3; t++)
                {
                    //GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                    GiaTriPre[1, 2] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[2, 2] - Tang Trong Xuat
                for (t = 3; t < 6; t++)
                {
                    //GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                    GiaTriPre[2, 2] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[0, 2] - Tong Xuat
                GiaTriPre[0, 2] = GiaTriPre[1, 2] + GiaTriPre[2, 2];

                //-------------------------------------------


                r = new DataRow[6];
                for (int jj = 0; jj < 6; jj++)
                {
                    r[jj] = dt.Rows[i * 6 + jj];
                }
                int[,] GiaTri = new int[3, 8];

                //-------------------------------------------

                //GiaTri[1, 0] - Giong Ton Dau
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[2, 0] - Tang Trong Ton Dau
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[0, 0] - Tong Ton Dau
                GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];

                //-------------------------------------------

                //GiaTri[1, 1] - Giong Nhap
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[2, 1] - Tang Trong Nhap
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[0, 1] - Tong Nhap
                GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];

                //-------------------------------------------

                //GiaTri[1, 2] - Giong Xuat
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[2, 2] - Tang Trong Xuat
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[0, 2] - Tong Xuat
                GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];

                //-------------------------------------------

                //GiaTri[1, 2] - Giong Ban
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 3] += Convert.ToInt32(r[t]["Xuat_Ban"]);
                }
                //GiaTri[2, 2] - Tang Trong Ban
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 3] += Convert.ToInt32(r[t]["Xuat_Ban"]);
                }
                //GiaTri[0, 2] - Tong Ban
                GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];

                //-------------------------------------------

                //GiaTri[1, 3] - Giong Chet
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 4] += Convert.ToInt32(r[t]["Chet"]);
                }
                //GiaTri[2, 3] - Tang Trong Chet
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 4] += Convert.ToInt32(r[t]["Chet"]);
                }
                //GiaTri[0, 3] - Tong Chet
                GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];

                //-------------------------------------------

                //GiaTri[1, 3] - Giong LoaiThai
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 5] += Convert.ToInt32(r[t]["LoaiThai"]);
                }
                //GiaTri[2, 3] - Tang Trong Chet
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 5] += Convert.ToInt32(r[t]["LoaiThai"]);
                }
                //GiaTri[0, 3] - Tong Chet
                GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];

                //-------------------------------------------

                //GiaTri[1, 4] - Giong Giet Mo
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 6] += Convert.ToInt32(r[t]["GietMo"]);
                }
                //GiaTri[2, 4] - Tang Trong Giet Mo
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 6] += Convert.ToInt32(r[t]["GietMo"]);
                }
                //GiaTri[0, 4] - Tong Giet Mo
                GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];

                //-------------------------------------------

                //GiaTri[1, 5] - Giong Ton Cuoi
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[2, 5] - Tang Trong Ton Cuoi
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[0, 5] - Tong Ton Cuoi
                GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];

                //-------------------------------------------

                int iThucAnGoc = iThucAn;
                for (t = 0; t < 3; t++)
                {
                    if (t == 0)
                    {
                        sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                        for (int y = 0; y < 3; y++)
                        {
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td><td></td><td></td>");
                        }
                        for (int y = 0; y < 5; y++)
                        {
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                        }
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6] + GiaTri[t, 5], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 3]["GietMo"]), 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 2]["GietMo"]) + GiaTri[t, 5], 0) + "</td><td></td><td></td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");

                        //ThucAn Start
                        int j = 0;
                        for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                        {
                            DataRow rta = dtSub.Rows[h];
                            if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                                j++;
                            }
                            else
                            {
                                iThucAn = h;
                                break;
                            }
                        }
                        for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                        //ThucAn End
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.Append("<tr><td></td>");
                        sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                        for (int y = 0; y < 3; y++)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td><td></td><td></td>");
                        }
                        for (int y = 0; y < 5; y++)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                        }
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6] + GiaTri[t, 5], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + t * 2 - 2]["GietMo"]) + GiaTri[t, 5], 0) + "</td><td></td><td></td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");
                        //
                        //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                        //ThucAn Start
                        int j = 0;
                        for (int h = iThucAnGoc; h < dtSub.Rows.Count; h++)
                        {
                            DataRow rta = dtSub.Rows[h];
                            if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                            {
                                if (t == 1)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                                }
                                j++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                        //ThucAn End
                        //
                        sb.Append("<td></td>");
                    }
                    sb.Append("</tr>");
                }
            }
        }


        public void CaST2_NgoaiDDA_Others()
        {
            int i = 0;
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt_NgoaiDDA.Rows[i * 6 + jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>4</td>");
            string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
            int[,] GiaTri = new int[3, 8];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];

            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];

            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];

            //-------------------------------------------

            //GiaTri[1, 2] - Giong Ban
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 3] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[2, 2] - Tang Trong Ban
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 3] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[0, 2] - Tong Ban
            GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];

            //-------------------------------------------

            //GiaTri[1, 3] - Giong Chet
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 4] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 4] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];

            //-------------------------------------------

            //GiaTri[1, 3] - Giong LoaiThai
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 5] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 5] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];

            //-------------------------------------------

            //GiaTri[1, 4] - Giong Giet Mo
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 6] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[2, 4] - Tang Trong Giet Mo
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 6] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[0, 4] - Tong Giet Mo
            GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];

            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];

            //-------------------------------------------

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 5; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6] + GiaTri[t, 5], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + 3]["GietMo"]), 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + 2]["GietMo"]) + GiaTri[t, 5], 0) + "</td><td></td><td></td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");

                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub_a.Rows.Count; h++)
                    {
                        DataRow rta = dtSub_a.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                        {
                            if (Convert.ToInt32(rta["KhoiLuongNgoaiDDA"]) != 0)
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongNgoaiDDA"], scale) + "</td>");
                                j++;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 5; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6] + GiaTri[t, 5], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM_NgoaiDDA.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + t * 2 - 2]["GietMo"]) + GiaTri[t, 5], 0) + "</td><td></td><td></td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");
                    //
                    //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub_a.Rows.Count; h++)
                    {
                        DataRow rta = dtSub_a.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                        {
                            if (t == 1)
                            {
                                if (Convert.ToInt32(rta["KhoiLuongGiongNgoaiDDA"]) != 0)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiongNgoaiDDA"], scale) + "</td>");
                                    j++;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(rta["KhoiLuongTangTrongNgoaiDDA"]) != 0)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrongNgoaiDDA"], scale) + "</td>");
                                    j++;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    //
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }

        public void CaST2_NgoaiDDA_FirstOctCGD()
        {
            int i = 0;
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt_NgoaiDDA.Rows[i * 6 + jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>4</td>");
            string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
            int[,] GiaTri = new int[3, 8];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];

            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];

            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                //GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                //GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];

            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];

            //-------------------------------------------

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 3; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 3; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }

        public void CaST2_NgoaiDDA_GomCGD()
        {
            int i = 0;
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt_NgoaiDDAPre.Rows[i * 6 + jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>4</td>");
            string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
            int[,] GiaTriPre = new int[3, 8];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTriPre[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTriPre[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTriPre[0, 0] = GiaTriPre[1, 0] + GiaTriPre[2, 0];

            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTriPre[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTriPre[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTriPre[0, 1] = GiaTriPre[1, 1] + GiaTriPre[2, 1];

            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                //GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                GiaTriPre[1, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                //GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                GiaTriPre[2, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTriPre[0, 2] = GiaTriPre[1, 2] + GiaTriPre[2, 2];

            //-------------------------------------------

            r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt_NgoaiDDA.Rows[i * 6 + jj];
            }
            int[,] GiaTri = new int[3, 8];

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];

            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];

            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];

            //-------------------------------------------

            //GiaTri[1, 2] - Giong Ban
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 3] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[2, 2] - Tang Trong Ban
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 3] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[0, 2] - Tong Ban
            GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];

            //-------------------------------------------

            //GiaTri[1, 3] - Giong Chet
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 4] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 4] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];

            //-------------------------------------------

            //GiaTri[1, 3] - Giong LoaiThai
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 5] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 5] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];

            //-------------------------------------------

            //GiaTri[1, 4] - Giong Giet Mo
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 6] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[2, 4] - Tang Trong Giet Mo
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 6] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[0, 4] - Tong Giet Mo
            GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];

            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];

            //-------------------------------------------

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 3; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td><td></td><td></td>");
                    }
                    for (int y = 0; y < 5; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6] + GiaTri[t, 5], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + 3]["GietMo"]), 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + 2]["GietMo"]) + GiaTri[t, 5], 0) + "</td><td></td><td></td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");

                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub_a.Rows.Count; h++)
                    {
                        DataRow rta = dtSub_a.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                        {
                            if (Convert.ToInt32(rta["KhoiLuongNgoaiDDA"]) != 0)
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongNgoaiDDA"], scale) + "</td>");
                                j++;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 3; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td><td></td><td></td>");
                    }
                    for (int y = 0; y < 5; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6] + GiaTri[t, 5], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM_NgoaiDDA.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + t * 2 - 2]["GietMo"]) + GiaTri[t, 5], 0) + "</td><td></td><td></td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");
                    //
                    //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub_a.Rows.Count; h++)
                    {
                        DataRow rta = dtSub_a.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                        {
                            if (t == 1)
                            {
                                if (Convert.ToInt32(rta["KhoiLuongGiongNgoaiDDA"]) != 0)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiongNgoaiDDA"], scale) + "</td>");
                                    j++;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(rta["KhoiLuongTangTrongNgoaiDDA"]) != 0)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrongNgoaiDDA"], scale) + "</td>");
                                    j++;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    //
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }


        public void CaST2_DDA_Others()
        {
            int i = 0;
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt_DDA.Rows[i * 6 + jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>5</td>");
            string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString() + " (Khu Dưỡng da)", "Con giống", "Tăng trọng" };
            int[,] GiaTri = new int[3, 8];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];

            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];

            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];

            //-------------------------------------------

            //GiaTri[1, 2] - Giong Ban
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 3] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[2, 2] - Tang Trong Ban
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 3] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[0, 2] - Tong Ban
            GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];

            //-------------------------------------------

            //GiaTri[1, 3] - Giong Chet
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 4] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 4] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];

            //-------------------------------------------

            //GiaTri[1, 3] - Giong LoaiThai
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 5] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 5] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];

            //-------------------------------------------

            //GiaTri[1, 4] - Giong Giet Mo
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 6] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[2, 4] - Tang Trong Giet Mo
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 6] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[0, 4] - Tong Giet Mo
            GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];

            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];

            //-------------------------------------------

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 5; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6] + GiaTri[t, 5], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_DDA.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM_DDA.Rows[i * 4 + 3]["GietMo"]), 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_DDA.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM_DDA.Rows[i * 4 + 2]["GietMo"]) + GiaTri[t, 5], 0) + "</td><td></td><td></td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");

                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub_a.Rows.Count; h++)
                    {
                        DataRow rta = dtSub_a.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                        {
                            if (Convert.ToInt32(rta["KhoiLuongDDA"]) != 0)
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongDDA"], scale) + "</td>");
                                j++;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 5; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6] + GiaTri[t, 5], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM_DDA.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_DDA.Rows[i * 4 + t * 2 - 2]["GietMo"]) + GiaTri[t, 5], 0) + "</td><td></td><td></td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");
                    //
                    //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub_a.Rows.Count; h++)
                    {
                        DataRow rta = dtSub_a.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                        {
                            if (t == 1)
                            {
                                //sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(0, scale) + "</td>");
                            }
                            else
                            {
                                if (Convert.ToInt32(rta["KhoiLuongDDA"]) != 0)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongDDA"], scale) + "</td>");
                                    j++;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    //
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }

        public void CaST2_DDA_FirstOctCGD()
        {
            int i = 0;
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt_DDA.Rows[i * 6 + jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>5</td>");
            string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString() + " (Khu Dưỡng da)", "Con giống", "Tăng trọng" };
            int[,] GiaTri = new int[3, 8];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];

            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];

            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                //GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                //GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];

            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];

            //-------------------------------------------

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 3; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 3; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }

        public void CaST2_DDA_GomCGD()
        {
            int i = 0;
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt_DDAPre.Rows[i * 6 + jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>5</td>");
            string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString() + " (Khu Dưỡng da)", "Con giống", "Tăng trọng" };
            int[,] GiaTriPre = new int[3, 8];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTriPre[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTriPre[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTriPre[0, 0] = GiaTriPre[1, 0] + GiaTriPre[2, 0];

            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTriPre[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTriPre[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTriPre[0, 1] = GiaTriPre[1, 1] + GiaTriPre[2, 1];

            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                //GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                GiaTriPre[1, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                //GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CPL"]) - Convert.ToInt32(r[t]["Nhap_CGT"]);
                GiaTriPre[2, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTriPre[0, 2] = GiaTriPre[1, 2] + GiaTriPre[2, 2];

            //-------------------------------------------

            r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt_DDA.Rows[i * 6 + jj];
            }
            int[,] GiaTri = new int[3, 8];

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];

            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 1] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 1] = GiaTri[1, 1] + GiaTri[2, 1];

            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 2] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 2] = GiaTri[1, 2] + GiaTri[2, 2];

            //-------------------------------------------

            //GiaTri[1, 2] - Giong Ban
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 3] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[2, 2] - Tang Trong Ban
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 3] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[0, 2] - Tong Ban
            GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];

            //-------------------------------------------

            //GiaTri[1, 3] - Giong Chet
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 4] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 4] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 4] = GiaTri[1, 4] + GiaTri[2, 4];

            //-------------------------------------------

            //GiaTri[1, 3] - Giong LoaiThai
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 5] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 5] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 5] = GiaTri[1, 5] + GiaTri[2, 5];

            //-------------------------------------------

            //GiaTri[1, 4] - Giong Giet Mo
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 6] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[2, 4] - Tang Trong Giet Mo
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 6] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[0, 4] - Tong Giet Mo
            GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];

            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 7] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 7] = GiaTri[1, 7] + GiaTri[2, 7];

            //-------------------------------------------

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 3; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td><td></td><td></td>");
                    }
                    for (int y = 0; y < 5; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6] + GiaTri[t, 5], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_DDA.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM_DDA.Rows[i * 4 + 3]["GietMo"]), 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_DDA.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM_DDA.Rows[i * 4 + 2]["GietMo"]) + GiaTri[t, 5], 0) + "</td><td></td><td></td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");

                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub_a.Rows.Count; h++)
                    {
                        DataRow rta = dtSub_a.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                        {
                            if (Convert.ToInt32(rta["KhoiLuongDDA"]) != 0)
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongDDA"], scale) + "</td>");
                                j++;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 3; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td><td></td><td></td>");
                    }
                    for (int y = 0; y < 5; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td><td></td><td></td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 6] + GiaTri[t, 5], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(dtPLGM_DDA.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td><td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_DDA.Rows[i * 4 + t * 2 - 2]["GietMo"]) + GiaTri[t, 5], 0) + "</td><td></td><td></td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 7], 0) + "</td><td></td><td></td>");
                    //
                    //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub_a.Rows.Count; h++)
                    {
                        DataRow rta = dtSub_a.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                        {
                            if (t == 1)
                            {
                                //sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(0, scale) + "</td>");
                            }
                            else
                            {
                                if (Convert.ToInt32(rta["KhoiLuongDDA"]) != 0)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongDDA"], scale) + "</td>");
                                    j++;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    //
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }


        public void CaHauBi_NgoaiDDA_Tong_Others()
        {
            //Tổng hợp Cá hậu bị NgoaiDDA từ loại 5, -1
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt1_NgoaiDDA.Rows[jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>6</td>");
            string[] tenLoaiCa = new string[] { "Hậu bị", "Con giống", "Tăng trọng" };
            int[,] GiaTri = new int[3, 24];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Ban
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[2, 2] - Tang Trong Ban
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[0, 2] - Tong Ban
            GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
            //-------------------------------------------

            //GiaTri[1, 3] - Giong Chet
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 12] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 12] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
            //-------------------------------------------

            //GiaTri[1, 3] - Giong LoaiThai
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
            //-------------------------------------------

            //GiaTri[1, 4] - Giong Giet Mo
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 18] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[2, 4] - Tang Trong Giet Mo
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 18] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[0, 4] - Tong Giet Mo
            GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 21] = GiaTri[1, 21] + GiaTri[2, 21];
            //-------------------------------------------

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 15; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1_NgoaiDDA.Rows[1]["GietMo"]) + Convert.ToInt32(dtPLGM1_NgoaiDDA.Rows[3]["GietMo"]), 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1_NgoaiDDA.Rows[0]["GietMo"]) + Convert.ToInt32(dtPLGM1_NgoaiDDA.Rows[2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");

                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub1.Rows.Count; h++)
                    {
                        DataRow rta = dtSub1.Rows[h];
                        if (Convert.ToInt32(rta["KhoiLuongNgoaiDDA"]) != 0)
                        {
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongNgoaiDDA"], scale) + "</td>");
                            j++;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 15; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM1_NgoaiDDA.Rows[t * 2 - 1]["GietMo"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1_NgoaiDDA.Rows[t * 2 - 2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    //
                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub1.Rows.Count; h++)
                    {
                        DataRow rta = dtSub1.Rows[h];
                        if (t == 1)
                        {
                            if (Convert.ToInt32(rta["KhoiLuongGiongNgoaiDDA"]) != 0)
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiongNgoaiDDA"], scale) + "</td>");
                                j++;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(rta["KhoiLuongTangTrongNgoaiDDA"]) != 0)
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrongNgoaiDDA"], scale) + "</td>");
                                j++;
                            }
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    //
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }

        public void CaHauBi_NgoaiDDA_Tong_FirstOctCGD()
        {
            //Tổng Cá hậu bị NgoaiDDA từ loại 5, -1
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt1_NgoaiDDA.Rows[jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>6</td>");
            string[] tenLoaiCa = new string[] { "Hậu bị", "Con giống", "Tăng trọng" };
            int[,] GiaTri = new int[3, 24];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 21] = GiaTri[1, 21] + GiaTri[2, 21];
            //-------------------------------------------

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 9; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 9; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }

        public void CaHauBi_NgoaiDDA_Tong_GomCGD()
        {
            //Tổng Cá hậu bị NgoaiDDA từ loại 5, -1
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt1_NgoaiDDAPre.Rows[jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>6</td>");
            string[] tenLoaiCa = new string[] { "Hậu bị", "Con giống", "Tăng trọng" };
            int[,] GiaTriPre = new int[3, 24];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTriPre[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTriPre[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTriPre[0, 0] = GiaTriPre[1, 0] + GiaTriPre[2, 0];
            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTriPre[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTriPre[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTriPre[0, 3] = GiaTriPre[1, 3] + GiaTriPre[2, 3];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTriPre[1, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTriPre[2, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTriPre[0, 6] = GiaTriPre[1, 6] + GiaTriPre[2, 6];
            //-------------------------------------------

            //Tổng hợp Cá hậu bị NgoaiDDA từ loại 5, -1
            r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt1_NgoaiDDA.Rows[jj];
            }
            int[,] GiaTri = new int[3, 24];

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Ban
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[2, 2] - Tang Trong Ban
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[0, 2] - Tong Ban
            GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
            //-------------------------------------------

            //GiaTri[1, 3] - Giong Chet
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 12] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 12] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
            //-------------------------------------------

            //GiaTri[1, 3] - Giong LoaiThai
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
            //-------------------------------------------

            //GiaTri[1, 4] - Giong Giet Mo
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 18] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[2, 4] - Tang Trong Giet Mo
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 18] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[0, 4] - Tong Giet Mo
            GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 21] = GiaTri[1, 21] + GiaTri[2, 21];
            //-------------------------------------------

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 9; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td>");
                    }
                    for (int y = 0; y < 15; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1_NgoaiDDA.Rows[1]["GietMo"]) + Convert.ToInt32(dtPLGM1_NgoaiDDA.Rows[3]["GietMo"]), 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1_NgoaiDDA.Rows[0]["GietMo"]) + Convert.ToInt32(dtPLGM1_NgoaiDDA.Rows[2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");

                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub1.Rows.Count; h++)
                    {
                        DataRow rta = dtSub1.Rows[h];
                        if (Convert.ToInt32(rta["KhoiLuongNgoaiDDA"]) != 0)
                        {
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongNgoaiDDA"], scale) + "</td>");
                            j++;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 9; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td>");
                    }
                    for (int y = 0; y < 15; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM1_NgoaiDDA.Rows[t * 2 - 1]["GietMo"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1_NgoaiDDA.Rows[t * 2 - 2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    //
                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub1.Rows.Count; h++)
                    {
                        DataRow rta = dtSub1.Rows[h];
                        if (t == 1)
                        {
                            if (Convert.ToInt32(rta["KhoiLuongGiongNgoaiDDA"]) != 0)
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiongNgoaiDDA"], scale) + "</td>");
                                j++;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(rta["KhoiLuongTangTrongNgoaiDDA"]) != 0)
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrongNgoaiDDA"], scale) + "</td>");
                                j++;
                            }
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    //
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }


        public void CaHauBi_NgoaiDDA_ChiTiet_Others()
        {
            //Cá loại 5, -1
            for (int i = 1; i < 3; i++)
            {
                DataRow[] r = new DataRow[6];
                for (int jj = 0; jj < 6; jj++)
                {
                    r[jj] = dt_NgoaiDDA.Rows[i * 6 + jj];
                }
                sb.Append("<tr>");
                if (i == 1) sb.Append("<td style='vertical-align:middle;'>6.1</td>");
                else if (i == 2) sb.Append("<td style='vertical-align:middle;'>6.2</td>");
                string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                int[,] GiaTri = new int[3, 24];
                int t;

                //-------------------------------------------

                //GiaTri[1, 0] - Giong Ton Dau
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[2, 0] - Tang Trong Ton Dau
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[0, 0] - Tong Ton Dau
                GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
                //-------------------------------------------

                //GiaTri[1, 1] - Giong Nhap
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[2, 1] - Tang Trong Nhap
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[0, 1] - Tong Nhap
                GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
                //-------------------------------------------

                //GiaTri[1, 2] - Giong Xuat
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[2, 2] - Tang Trong Xuat
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[0, 2] - Tong Xuat
                GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
                //-------------------------------------------

                //GiaTri[1, 2] - Giong Ban
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
                }
                //GiaTri[2, 2] - Tang Trong Ban
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
                }
                //GiaTri[0, 2] - Tong Ban
                GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
                //-------------------------------------------

                //GiaTri[1, 3] - Giong Chet
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 12] += Convert.ToInt32(r[t]["Chet"]);
                }
                //GiaTri[2, 3] - Tang Trong Chet
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 12] += Convert.ToInt32(r[t]["Chet"]);
                }
                //GiaTri[0, 3] - Tong Chet
                GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
                //-------------------------------------------

                //GiaTri[1, 3] - Giong LoaiThai
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
                }
                //GiaTri[2, 4] - Tang Trong Giet Mo
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
                }
                //GiaTri[0, 4] - Tong Giet Mo
                GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
                //-------------------------------------------

                //GiaTri[1, 4] - Giong Giet Mo
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 18] += Convert.ToInt32(r[t]["GietMo"]);
                }
                //GiaTri[2, 5] - Tang Trong Ton Cuoi
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 18] += Convert.ToInt32(r[t]["GietMo"]);
                }
                //GiaTri[0, 5] - Tong Ton Cuoi
                GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
                //-------------------------------------------

                //GiaTri[1, 5] - Giong Ton Cuoi
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[2, 5] - Tang Trong Ton Cuoi
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[0, 5] - Tong Ton Cuoi
                GiaTri[0, 21] = GiaTri[1, 21] + GiaTri[2, 21];
                //-------------------------------------------

                //Tính vị trí dòng đầu tiên của loại cá IDLoaiCaDDA[i]
                int rowIndex = 0;
                for (int l = 0; l < dtSub_a.Rows.Count; l++)
                {
                    DataRow rta = dtSub_a.Rows[l];
                    if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                    {
                        rowIndex = l;
                        break;
                    }
                }

                for (t = 0; t < 3; t++)
                {
                    if (t == 0)
                    {
                        sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                        for (int y = 0; y < 15; y++)
                        {
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                        }
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + 3]["GietMo"]), 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + 2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                        //ThucAn Start
                        int j = 0;
                        for (int h = rowIndex; h < dtSub_a.Rows.Count; h++)
                        {
                            DataRow rta = dtSub_a.Rows[h];
                            if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                            {
                                if (Convert.ToInt32(rta["KhoiLuongNgoaiDDA"]) != 0)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongNgoaiDDA"], scale) + "</td>");
                                    j++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                        //ThucAn End
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.Append("<tr><td></td>");
                        sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                        for (int y = 0; y < 15; y++)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                        }
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM_NgoaiDDA.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + t * 2 - 2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                        //
                        //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                        //ThucAn Start
                        int j = 0;
                        for (int h = rowIndex; h < dtSub_a.Rows.Count; h++)
                        {
                            DataRow rta = dtSub_a.Rows[h];
                            if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                            {
                                if (t == 1)
                                {
                                    if (Convert.ToInt32(rta["KhoiLuongGiongNgoaiDDA"]) != 0)
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiongNgoaiDDA"], scale) + "</td>");
                                        j++;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(rta["KhoiLuongTangTrongNgoaiDDA"]) != 0)
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrongNgoaiDDA"], scale) + "</td>");
                                        j++;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                        //ThucAn End
                        //
                        sb.Append("<td></td>");
                    }
                    sb.Append("</tr>");
                }
            }
        }

        public void CaHauBi_NgoaiDDA_ChiTiet_FirstOctCGD()
        {
            //Cá loại 5, -1
            for (int i = 1; i < 3; i++)
            {
                DataRow[] r = new DataRow[6];
                for (int jj = 0; jj < 6; jj++)
                {
                    r[jj] = dt_NgoaiDDA.Rows[i * 6 + jj];
                }
                sb.Append("<tr>");
                if (i == 1) sb.Append("<td style='vertical-align:middle;'>6.1</td>");
                else if (i == 2) sb.Append("<td style='vertical-align:middle;'>6.2</td>");
                string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                int[,] GiaTri = new int[3, 24];
                int t;

                //-------------------------------------------

                //GiaTri[1, 0] - Giong Ton Dau
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[2, 0] - Tang Trong Ton Dau
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[0, 0] - Tong Ton Dau
                GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
                //-------------------------------------------

                //GiaTri[1, 1] - Giong Nhap
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[2, 1] - Tang Trong Nhap
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[0, 1] - Tong Nhap
                GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
                //-------------------------------------------

                //GiaTri[1, 2] - Giong Xuat
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[2, 2] - Tang Trong Xuat
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[0, 2] - Tong Xuat
                GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
                //-------------------------------------------

                //GiaTri[1, 5] - Giong Ton Cuoi
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[2, 5] - Tang Trong Ton Cuoi
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[0, 5] - Tong Ton Cuoi
                GiaTri[0, 21] = GiaTri[1, 21] + GiaTri[2, 21];
                //-------------------------------------------

                for (t = 0; t < 3; t++)
                {
                    if (t == 0)
                    {
                        sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                        for (int y = 0; y < 9; y++)
                        {
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                        }
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.Append("<tr><td></td>");
                        sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                        for (int y = 0; y < 9; y++)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                        }
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                        sb.Append("<td></td>");
                    }
                    sb.Append("</tr>");
                }
            }
        }

        public void CaHauBi_NgoaiDDA_ChiTiet_GomCGD()
        {
            //Cá loại 5, -1
            for (int i = 1; i < 3; i++)
            {
                DataRow[] r = new DataRow[6];
                for (int jj = 0; jj < 6; jj++)
                {
                    r[jj] = dt_NgoaiDDAPre.Rows[i * 6 + jj];
                }
                sb.Append("<tr>");
                if (i == 1) sb.Append("<td style='vertical-align:middle;'>6.1</td>");
                else if (i == 2) sb.Append("<td style='vertical-align:middle;'>6.2</td>");
                string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
                int[,] GiaTriPre = new int[3, 24];
                int t;

                //-------------------------------------------

                //GiaTri[1, 0] - Giong Ton Dau
                for (t = 0; t < 3; t++)
                {
                    GiaTriPre[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[2, 0] - Tang Trong Ton Dau
                for (t = 3; t < 6; t++)
                {
                    GiaTriPre[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[0, 0] - Tong Ton Dau
                GiaTriPre[0, 0] = GiaTriPre[1, 0] + GiaTriPre[2, 0];
                //-------------------------------------------

                //GiaTri[1, 1] - Giong Nhap
                for (t = 0; t < 3; t++)
                {
                    GiaTriPre[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[2, 1] - Tang Trong Nhap
                for (t = 3; t < 6; t++)
                {
                    GiaTriPre[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[0, 1] - Tong Nhap
                GiaTriPre[0, 3] = GiaTriPre[1, 3] + GiaTriPre[2, 3];
                //-------------------------------------------

                //GiaTri[1, 2] - Giong Xuat
                for (t = 0; t < 3; t++)
                {
                    GiaTriPre[1, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[2, 2] - Tang Trong Xuat
                for (t = 3; t < 6; t++)
                {
                    GiaTriPre[2, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[0, 2] - Tong Xuat
                GiaTriPre[0, 6] = GiaTriPre[1, 6] + GiaTriPre[2, 6];
                //-------------------------------------------

                r = new DataRow[6];
                for (int jj = 0; jj < 6; jj++)
                {
                    r[jj] = dt_NgoaiDDA.Rows[i * 6 + jj];
                }
                int[,] GiaTri = new int[3, 24];

                //-------------------------------------------

                //GiaTri[1, 0] - Giong Ton Dau
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[2, 0] - Tang Trong Ton Dau
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[0, 0] - Tong Ton Dau
                GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
                //-------------------------------------------

                //GiaTri[1, 1] - Giong Nhap
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[2, 1] - Tang Trong Nhap
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
                }
                //GiaTri[0, 1] - Tong Nhap
                GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
                //-------------------------------------------

                //GiaTri[1, 2] - Giong Xuat
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[2, 2] - Tang Trong Xuat
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[0, 2] - Tong Xuat
                GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
                //-------------------------------------------

                //GiaTri[1, 2] - Giong Ban
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
                }
                //GiaTri[2, 2] - Tang Trong Ban
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
                }
                //GiaTri[0, 2] - Tong Ban
                GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
                //-------------------------------------------

                //GiaTri[1, 3] - Giong Chet
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 12] += Convert.ToInt32(r[t]["Chet"]);
                }
                //GiaTri[2, 3] - Tang Trong Chet
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 12] += Convert.ToInt32(r[t]["Chet"]);
                }
                //GiaTri[0, 3] - Tong Chet
                GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
                //-------------------------------------------

                //GiaTri[1, 3] - Giong LoaiThai
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
                }
                //GiaTri[2, 4] - Tang Trong Giet Mo
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
                }
                //GiaTri[0, 4] - Tong Giet Mo
                GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
                //-------------------------------------------

                //GiaTri[1, 4] - Giong Giet Mo
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 18] += Convert.ToInt32(r[t]["GietMo"]);
                }
                //GiaTri[2, 5] - Tang Trong Ton Cuoi
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 18] += Convert.ToInt32(r[t]["GietMo"]);
                }
                //GiaTri[0, 5] - Tong Ton Cuoi
                GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
                //-------------------------------------------

                //GiaTri[1, 5] - Giong Ton Cuoi
                for (t = 0; t < 3; t++)
                {
                    GiaTri[1, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[2, 5] - Tang Trong Ton Cuoi
                for (t = 3; t < 6; t++)
                {
                    GiaTri[2, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[0, 5] - Tong Ton Cuoi
                GiaTri[0, 21] = GiaTri[1, 21] + GiaTri[2, 21];
                //-------------------------------------------

                //Tính vị trí dòng đầu tiên của loại cá IDLoaiCaDDA[i]
                int rowIndex = 0;
                for (int l = 0; l < dtSub_a.Rows.Count; l++)
                {
                    DataRow rta = dtSub_a.Rows[l];
                    if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                    {
                        rowIndex = l;
                        break;
                    }
                }

                for (t = 0; t < 3; t++)
                {
                    if (t == 0)
                    {
                        sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                        for (int y = 0; y < 9; y++)
                        {
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td>");
                        }
                        for (int y = 0; y < 15; y++)
                        {
                            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                        }
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + 3]["GietMo"]), 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + 2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                        //ThucAn Start
                        int j = 0;
                        for (int h = rowIndex; h < dtSub_a.Rows.Count; h++)
                        {
                            DataRow rta = dtSub_a.Rows[h];
                            if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                            {
                                if (Convert.ToInt32(rta["KhoiLuongNgoaiDDA"]) != 0)
                                {
                                    sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongNgoaiDDA"], scale) + "</td>");
                                    j++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                        //ThucAn End
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.Append("<tr><td></td>");
                        sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                        for (int y = 0; y < 9; y++)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td>");
                        }
                        for (int y = 0; y < 15; y++)
                        {
                            sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                        }
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM_NgoaiDDA.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM_NgoaiDDA.Rows[i * 4 + t * 2 - 2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                        //
                        //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                        //ThucAn Start
                        int j = 0;
                        for (int h = rowIndex; h < dtSub_a.Rows.Count; h++)
                        {
                            DataRow rta = dtSub_a.Rows[h];
                            if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCaDDA[i])
                            {
                                if (t == 1)
                                {
                                    if (Convert.ToInt32(rta["KhoiLuongGiongNgoaiDDA"]) != 0)
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiongNgoaiDDA"], scale) + "</td>");
                                        j++;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(rta["KhoiLuongTangTrongNgoaiDDA"]) != 0)
                                    {
                                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrongNgoaiDDA"], scale) + "</td>");
                                        j++;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                        //ThucAn End
                        //
                        sb.Append("<td></td>");
                    }
                    sb.Append("</tr>");
                }
            }
        }


        public void CaHauBi_DDA_Others()
        {
            //Tổng hợp Cá hậu bị DDA từ loại 5, -1
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt1_DDA.Rows[jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>7</td>");
            string[] tenLoaiCa = new string[] { "Hậu bị (Khu Dưỡng da)", "Con giống", "Tăng trọng" };
            int[,] GiaTri = new int[3, 24];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Ban
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[2, 2] - Tang Trong Ban
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[0, 2] - Tong Ban
            GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
            //-------------------------------------------

            //GiaTri[1, 3] - Giong Chet
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 12] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 12] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
            //-------------------------------------------

            //GiaTri[1, 3] - Giong LoaiThai
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
            //-------------------------------------------

            //GiaTri[1, 4] - Giong Giet Mo
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 18] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[2, 4] - Tang Trong Giet Mo
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 18] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[0, 4] - Tong Giet Mo
            GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 21] = GiaTri[1, 21] + GiaTri[2, 21];
            //-------------------------------------------

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 15; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1_DDA.Rows[1]["GietMo"]) + Convert.ToInt32(dtPLGM1_DDA.Rows[3]["GietMo"]), 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1_DDA.Rows[0]["GietMo"]) + Convert.ToInt32(dtPLGM1_DDA.Rows[2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");

                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub1_a.Rows.Count; h++)
                    {
                        DataRow rta = dtSub1_a.Rows[h];
                        if (Convert.ToInt32(rta["KhoiLuongDDA"]) != 0)
                        {
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongDDA"], scale) + "</td>");
                            j++;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 15; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM1_DDA.Rows[t * 2 - 1]["GietMo"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1_DDA.Rows[t * 2 - 2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    //
                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub1_a.Rows.Count; h++)
                    {
                        DataRow rta = dtSub1_a.Rows[h];
                        if (t == 1)
                        {
                            //sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(0, scale) + "</td>");
                        }
                        else
                        {
                            if (Convert.ToInt32(rta["KhoiLuongDDA"]) != 0)
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongDDA"], scale) + "</td>");
                                j++;
                            }
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    //
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }

        public void CaHauBi_DDA_FirstOctCGD()
        {
            //Tổng Cá hậu bị DDA từ loại 5, -1
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt1_DDA.Rows[jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>7</td>");
            string[] tenLoaiCa = new string[] { "Hậu bị (Khu Dưỡng da)", "Con giống", "Tăng trọng" };
            int[,] GiaTri = new int[3, 24];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 21] = GiaTri[1, 21] + GiaTri[2, 21];
            //-------------------------------------------

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 9; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 9; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }

        public void CaHauBi_DDA_GomCGD()
        {
            //Tổng Cá hậu bị DDA từ loại 5, -1
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt1_DDAPre.Rows[jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>7</td>");
            string[] tenLoaiCa = new string[] { "Hậu bị (Khu Dưỡng da)", "Con giống", "Tăng trọng" };
            int[,] GiaTriPre = new int[3, 24];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTriPre[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTriPre[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTriPre[0, 0] = GiaTriPre[1, 0] + GiaTriPre[2, 0];
            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTriPre[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTriPre[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTriPre[0, 3] = GiaTriPre[1, 3] + GiaTriPre[2, 3];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTriPre[1, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTriPre[2, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTriPre[0, 6] = GiaTriPre[1, 6] + GiaTriPre[2, 6];
            //-------------------------------------------

            r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt1_DDA.Rows[jj];
            }
            int[,] GiaTri = new int[3, 24];

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_ChuyenChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_ChuyenChuong"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Ban
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[2, 2] - Tang Trong Ban
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[0, 2] - Tong Ban
            GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
            //-------------------------------------------

            //GiaTri[1, 3] - Giong Chet
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 12] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 12] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
            //-------------------------------------------

            //GiaTri[1, 3] - Giong LoaiThai
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
            //-------------------------------------------

            //GiaTri[1, 4] - Giong Giet Mo
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 18] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[2, 4] - Tang Trong Giet Mo
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 18] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[0, 4] - Tong Giet Mo
            GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 21] = GiaTri[1, 21] + GiaTri[2, 21];
            //-------------------------------------------

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 9; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td>");
                    }
                    for (int y = 0; y < 15; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1_DDA.Rows[1]["GietMo"]) + Convert.ToInt32(dtPLGM1_DDA.Rows[3]["GietMo"]), 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1_DDA.Rows[0]["GietMo"]) + Convert.ToInt32(dtPLGM1_DDA.Rows[2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");

                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub1_a.Rows.Count; h++)
                    {
                        DataRow rta = dtSub1_a.Rows[h];
                        if (Convert.ToInt32(rta["KhoiLuongDDA"]) != 0)
                        {
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongDDA"], scale) + "</td>");
                            j++;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 9; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td>");
                    }
                    for (int y = 0; y < 15; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM1_DDA.Rows[t * 2 - 1]["GietMo"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM1_DDA.Rows[t * 2 - 2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    //
                    //ThucAn Start
                    int j = 0;
                    for (int h = 0; h < dtSub1_a.Rows.Count; h++)
                    {
                        DataRow rta = dtSub1_a.Rows[h];
                        if (t == 1)
                        {
                            //sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(0, scale) + "</td>");
                        }
                        else
                        {
                            if (Convert.ToInt32(rta["KhoiLuongDDA"]) != 0)
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongDDA"], scale) + "</td>");
                                j++;
                            }
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    //
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }

        
        public void Ca234_Others()
        {
            int i = 6;
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt.Rows[i * 6 + jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>8</td>");
            string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
            int[,] GiaTri = new int[3, 24];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Ban
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[2, 2] - Tang Trong Ban
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[0, 2] - Tong Ban
            GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
            //-------------------------------------------

            //GiaTri[1, 3] - Giong Chet
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 12] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 12] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
            //-------------------------------------------

            //GiaTri[1, 3] - Giong LoaiThai
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[2, 4] - Tang Trong Giet Mo
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[0, 4] - Tong Giet Mo
            GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
            //-------------------------------------------

            //GiaTri[1, 4] - Giong Giet Mo
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 18] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 18] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 21] = GiaTri[1, 21] + GiaTri[2, 21];
            //-------------------------------------------

            //Tinh iThucAn truoc
            for (int l = 0; l < dtSub.Rows.Count; l++)
            {
                DataRow rta = dtSub.Rows[l];
                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                {
                    iThucAn = l;
                    break;
                }
            }

            int iThucAnGoc = iThucAn;
            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 15; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 3]["GietMo"]), 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    //ThucAn Start
                    int j = 0;
                    for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                    {
                        DataRow rta = dtSub.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                        {
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                            j++;
                        }
                        else
                        {
                            iThucAn = h;
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 15; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + t * 2 - 2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    //
                    //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn Start
                    int j = 0;
                    for (int h = iThucAnGoc; h < dtSub.Rows.Count; h++)
                    {
                        DataRow rta = dtSub.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                        {
                            if (t == 1)
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                            }
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    //
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }

        public void Ca234_FirstOctCGD()
        {
            int i = 6;
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt.Rows[i * 6 + jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>8</td>");
            string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
            int[,] GiaTri = new int[3, 24];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 21] = GiaTri[1, 21] + GiaTri[2, 21];
            //-------------------------------------------

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 9; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 9; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }

        public void Ca234_GomCGD()
        {
            int i = 6;
            DataRow[] r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dtPre.Rows[i * 6 + jj];
            }
            sb.Append("<tr>");
            sb.Append("<td style='vertical-align:middle;'>8</td>");
            string[] tenLoaiCa = new string[] { r[0]["TenLoaiCa"].ToString(), "Con giống", "Tăng trọng" };
            int[,] GiaTriPre = new int[3, 24];
            int t;

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTriPre[1, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTriPre[2, 0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTriPre[0, 0] = GiaTriPre[1, 0] + GiaTriPre[2, 0];
            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTriPre[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTriPre[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTriPre[0, 3] = GiaTriPre[1, 3] + GiaTriPre[2, 3];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTriPre[1, 6] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTriPre[2, 6] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTriPre[0, 6] = GiaTriPre[1, 6] + GiaTriPre[2, 6];
            //-------------------------------------------

            r = new DataRow[6];
            for (int jj = 0; jj < 6; jj++)
            {
                r[jj] = dt.Rows[i * 6 + jj];
            }
            int[,] GiaTri = new int[3, 24];

            //-------------------------------------------

            //GiaTri[1, 0] - Giong Ton Dau
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[2, 0] - Tang Trong Ton Dau
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
            }
            //GiaTri[0, 0] - Tong Ton Dau
            GiaTri[0, 0] = GiaTri[1, 0] + GiaTri[2, 0];
            //-------------------------------------------

            //GiaTri[1, 1] - Giong Nhap
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[2, 1] - Tang Trong Nhap
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CPLI"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]);
            }
            //GiaTri[0, 1] - Tong Nhap
            GiaTri[0, 3] = GiaTri[1, 3] + GiaTri[2, 3];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Xuat
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 6] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[2, 2] - Tang Trong Xuat
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 6] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
            }
            //GiaTri[0, 2] - Tong Xuat
            GiaTri[0, 6] = GiaTri[1, 6] + GiaTri[2, 6];
            //-------------------------------------------

            //GiaTri[1, 2] - Giong Ban
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[2, 2] - Tang Trong Ban
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
            }
            //GiaTri[0, 2] - Tong Ban
            GiaTri[0, 9] = GiaTri[1, 9] + GiaTri[2, 9];
            //-------------------------------------------

            //GiaTri[1, 3] - Giong Chet
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 12] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[2, 3] - Tang Trong Chet
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 12] += Convert.ToInt32(r[t]["Chet"]);
            }
            //GiaTri[0, 3] - Tong Chet
            GiaTri[0, 12] = GiaTri[1, 12] + GiaTri[2, 12];
            //-------------------------------------------

            //GiaTri[1, 3] - Giong LoaiThai
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[2, 4] - Tang Trong Giet Mo
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 15] += Convert.ToInt32(r[t]["LoaiThai"]);
            }
            //GiaTri[0, 4] - Tong Giet Mo
            GiaTri[0, 15] = GiaTri[1, 15] + GiaTri[2, 15];
            //-------------------------------------------

            //GiaTri[1, 4] - Giong Giet Mo
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 18] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 18] += Convert.ToInt32(r[t]["GietMo"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 18] = GiaTri[1, 18] + GiaTri[2, 18];
            //-------------------------------------------

            //GiaTri[1, 5] - Giong Ton Cuoi
            for (t = 0; t < 3; t++)
            {
                GiaTri[1, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[2, 5] - Tang Trong Ton Cuoi
            for (t = 3; t < 6; t++)
            {
                GiaTri[2, 21] += Convert.ToInt32(r[t]["TonCuoi"]);
            }
            //GiaTri[0, 5] - Tong Ton Cuoi
            GiaTri[0, 21] = GiaTri[1, 21] + GiaTri[2, 21];
            //-------------------------------------------

            //Tinh iThucAn truoc
            for (int l = 0; l < dtSub.Rows.Count; l++)
            {
                DataRow rta = dtSub.Rows[l];
                if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                {
                    iThucAn = l;
                    break;
                }
            }

            int iThucAnGoc = iThucAn;

            for (t = 0; t < 3; t++)
            {
                if (t == 0)
                {
                    sb.Append("<td align='left'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 9; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td>");
                    }
                    for (int y = 0; y < 15; y++)
                    {
                        sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + 1]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 3]["GietMo"]), 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + Convert.ToInt32(dtPLGM.Rows[i * 4 + 2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    //ThucAn Start
                    int j = 0;
                    for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                    {
                        DataRow rta = dtSub.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                        {
                            sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                            j++;
                        }
                        else
                        {
                            iThucAn = h;
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    sb.Append("<td></td>");
                }
                else
                {
                    sb.Append("<tr><td></td>");
                    sb.Append("<td align='right'>" + tenLoaiCa[t] + "</td>");
                    for (int y = 0; y < 9; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTriPre[t, y], 0) + "</td>");
                    }
                    for (int y = 0; y < 15; y++)
                    {
                        sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, y], 0) + "</td>");
                    }
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 18] + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + t * 2 - 1]["GietMo"], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4 + t * 2 - 2]["GietMo"]) + GiaTri[t, 15], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 19] + GiaTri[t, 16], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 20] + GiaTri[t, 17], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 21], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 22], 0) + "</td>");
                    sb.Append("<td style='text-align:right;'>" + Config.ToXVal2(GiaTri[t, 23], 0) + "</td>");
                    //
                    //for (int k = 0; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn Start
                    int j = 0;
                    for (int h = iThucAnGoc; h < dtSub.Rows.Count; h++)
                    {
                        DataRow rta = dtSub.Rows[h];
                        if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                        {
                            if (t == 1)
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongGiong"], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuongTangTrong"], scale) + "</td>");
                            }
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                    //ThucAn End
                    //
                    sb.Append("<td></td>");
                }
                sb.Append("</tr>");
            }
        }


        public void CaSS_Others()
        {
            //Cá từ loại 7 -> loại 11
            for (int i = 7; i < 12; i++)
            {
                DataRow[] r = new DataRow[3];
                for (int jj = 0; jj < 3; jj++)
                {
                    r[jj] = dt.Rows[i * 6 + jj];
                }
                sb.Append("<tr>");
                int stt = i + 2;
                sb.Append("<td>" + stt.ToString() + "</td>");
                sb.Append("<td align='left'>" + r[0]["TenLoaiCa"].ToString() + "</td>");
                int[] GiaTri = new int[24];
                int t;

                //-------------------------------------------

                //GiaTri[0] - Tong Ton Dau
                for (t = 0; t < 3; t++)
                {
                    GiaTri[0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[2] - Cai Ton Dau
                GiaTri[2] += Convert.ToInt32(r[1]["TonDau"]);// +Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) - Convert.ToInt32(r[1]["Xuat_CGTO"]) - Convert.ToInt32(r[1]["Xuat_CPLO"]);
                //GiaTri[1] - Duc Ton Dau
                GiaTri[1] += Convert.ToInt32(r[2]["TonDau"]);// +Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) - Convert.ToInt32(r[2]["Xuat_CGTO"]) - Convert.ToInt32(r[2]["Xuat_CPLO"]);

                //-------------------------------------------

                //GiaTri[3] - Tong Nhap
                for (t = 0; t < 3; t++)
                {
                    GiaTri[3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CGTI"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]);
                }
                //GiaTri[5] - Cai Nhap
                GiaTri[5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]);// -Convert.ToInt32(r[1]["Xuat_CGTI"]) - Convert.ToInt32(r[1]["Xuat_CPLI"]);
                //GiaTri[4] - Duc Nhap
                GiaTri[4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]);// -Convert.ToInt32(r[2]["Xuat_CGTI"]) - Convert.ToInt32(r[2]["Xuat_CPLI"]);

                //-------------------------------------------

                //GiaTri[6] - Tong Xuat
                for (t = 0; t < 3; t++)
                {
                    GiaTri[6] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[8] - Cai Xuat
                GiaTri[8] += Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGTO"]) + Convert.ToInt32(r[1]["Xuat_CGTI"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPLO"]) + Convert.ToInt32(r[1]["Xuat_CPLI"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                //GiaTri[7] - Duc Xuat
                GiaTri[7] += Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGTO"]) + Convert.ToInt32(r[2]["Xuat_CGTI"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPLO"]) + Convert.ToInt32(r[2]["Xuat_CPLI"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);

                //-------------------------------------------

                //GiaTri[6] - Tong Ban
                for (t = 0; t < 3; t++)
                {
                    GiaTri[9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
                }
                //GiaTri[8] - Cai Ban
                GiaTri[11] += Convert.ToInt32(r[1]["Xuat_Ban"]);
                //GiaTri[7] - Duc Ban
                GiaTri[10] += Convert.ToInt32(r[2]["Xuat_Ban"]);

                //-------------------------------------------

                //GiaTri[9] - Tong Chet
                for (t = 0; t < 3; t++)
                {
                    GiaTri[12] += Convert.ToInt32(r[t]["Chet"]);
                }
                //GiaTri[11] - Cai Chet
                GiaTri[14] += Convert.ToInt32(r[1]["Chet"]);
                //GiaTri[10] - Duc Chet
                GiaTri[13] += Convert.ToInt32(r[2]["Chet"]);

                //-------------------------------------------

                //GiaTri[9] - Tong LoaiThai
                for (t = 0; t < 3; t++)
                {
                    GiaTri[15] += Convert.ToInt32(r[t]["LoaiThai"]);
                }
                //GiaTri[14] - Cai Giet Mo
                GiaTri[17] += Convert.ToInt32(r[1]["LoaiThai"]);
                //GiaTri[13] - Duc Giet Mo
                GiaTri[16] += Convert.ToInt32(r[2]["LoaiThai"]);

                //-------------------------------------------

                //GiaTri[12] - Tong Giet Mo
                for (t = 0; t < 3; t++)
                {
                    GiaTri[18] += Convert.ToInt32(r[t]["GietMo"]);
                }
                //GiaTri[17] - Cai Ton Cuoi
                GiaTri[20] += Convert.ToInt32(r[1]["GietMo"]);
                //GiaTri[16] - Duc Ton Cuoi
                GiaTri[19] += Convert.ToInt32(r[2]["GietMo"]);

                //-------------------------------------------

                //GiaTri[15] - Tong Ton Cuoi
                for (t = 0; t < 3; t++)
                {
                    GiaTri[21] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[17] - Cai Ton Cuoi
                GiaTri[23] += Convert.ToInt32(r[1]["TonCuoi"]);
                //GiaTri[16] - Duc Ton Cuoi
                GiaTri[22] += Convert.ToInt32(r[2]["TonCuoi"]);

                //-------------------------------------------

                for (int y = 0; y < 5; y++)
                {
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 1], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 2], 0) + "</td>");
                }
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[18] + GiaTri[15], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + 1]["GietMo"], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + GiaTri[15], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[19] + GiaTri[16], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[20] + GiaTri[17], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[21], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[22], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[23], 0) + "</td>");
                //ThucAn Start
                //Tinh iThucAn
                for (int l = 0; l < dtSub.Rows.Count; l++)
                {
                    DataRow rta = dtSub.Rows[l];
                    if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                    {
                        iThucAn = l;
                        break;
                    }
                }
                int j = 0;
                for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                {
                    DataRow rta = dtSub.Rows[h];
                    if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                    {
                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                        j++;
                    }
                    else
                    {
                        iThucAn = h;
                        break;
                    }
                }
                for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                //ThucAn End
                sb.Append("<td></td>");
                sb.Append("</tr>");
            }
        }

        public void CaSS_FirstOctCGD()
        {
            //Cá từ loại 7 -> loại 11
            for (int i = 7; i < 12; i++)
            {
                DataRow[] r = new DataRow[3];
                for (int jj = 0; jj < 3; jj++)
                {
                    r[jj] = dt.Rows[i * 6 + jj];
                }
                sb.Append("<tr>");
                int stt = i + 2;
                sb.Append("<td>" + stt.ToString() + "</td>");
                sb.Append("<td align='left'>" + r[0]["TenLoaiCa"].ToString() + "</td>");
                int[] GiaTri = new int[24];
                int t;

                //-------------------------------------------

                //GiaTri[0] - Tong Ton Dau
                for (t = 0; t < 3; t++)
                {
                    GiaTri[0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[2] - Cai Ton Dau
                GiaTri[2] += Convert.ToInt32(r[1]["TonDau"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) - Convert.ToInt32(r[1]["Xuat_CGTO"]) - Convert.ToInt32(r[1]["Xuat_CPLO"]);
                //GiaTri[1] - Duc Ton Dau
                GiaTri[1] += Convert.ToInt32(r[2]["TonDau"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) - Convert.ToInt32(r[2]["Xuat_CGTO"]) - Convert.ToInt32(r[2]["Xuat_CPLO"]);

                //-------------------------------------------

                //GiaTri[3] - Tong Nhap
                for (t = 0; t < 3; t++)
                {
                    GiaTri[3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]);
                }
                //GiaTri[5] - Cai Nhap
                GiaTri[5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]) - Convert.ToInt32(r[1]["Xuat_CGTI"]) - Convert.ToInt32(r[1]["Xuat_CPLI"]);
                //GiaTri[4] - Duc Nhap
                GiaTri[4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]) - Convert.ToInt32(r[2]["Xuat_CGTI"]) - Convert.ToInt32(r[2]["Xuat_CPLI"]);

                //-------------------------------------------

                //GiaTri[6] - Tong Xuat
                for (t = 0; t < 3; t++)
                {
                    //GiaTri[6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CGT"]) - Convert.ToInt32(r[t]["Nhap_CPL"]);
                    GiaTri[6] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[8] - Cai Xuat
                //GiaTri[8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGT"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPL"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]) - Convert.ToInt32(r[1]["Nhap_CGT"]) - Convert.ToInt32(r[1]["Nhap_CPL"]);
                GiaTri[8] += Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                //GiaTri[7] - Duc Xuat
                //GiaTri[7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGT"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPL"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]) - Convert.ToInt32(r[2]["Nhap_CGT"]) - Convert.ToInt32(r[2]["Nhap_CPL"]);
                GiaTri[7] += Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);

                //-------------------------------------------

                //GiaTri[15] - Tong Ton Cuoi
                for (t = 0; t < 3; t++)
                {
                    GiaTri[21] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[17] - Cai Ton Cuoi
                GiaTri[23] += Convert.ToInt32(r[1]["TonCuoi"]);
                //GiaTri[16] - Duc Ton Cuoi
                GiaTri[22] += Convert.ToInt32(r[2]["TonCuoi"]);

                //-------------------------------------------

                for (int y = 0; y < 3; y++)
                {
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 1], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 2], 0) + "</td>");
                }
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[21], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[22], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[23], 0) + "</td>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
            }
        }

        public void CaSS_GomCGD()
        {
            //Cá từ loại 7 -> loại 11
            for (int i = 7; i < 12; i++)
            {
                DataRow[] r = new DataRow[3];
                for (int jj = 0; jj < 3; jj++)
                {
                    r[jj] = dtPre.Rows[i * 6 + jj];
                }
                sb.Append("<tr>");
                int stt = i + 2;
                sb.Append("<td>" + stt.ToString() + "</td>");
                sb.Append("<td align='left'>" + r[0]["TenLoaiCa"].ToString() + "</td>");
                int[] GiaTriPre = new int[24];
                int t;

                //-------------------------------------------

                //GiaTri[0] - Tong Ton Dau
                for (t = 0; t < 3; t++)
                {
                    GiaTriPre[0] += Convert.ToInt32(r[t]["TonDau"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[2] - Cai Ton Dau
                GiaTriPre[2] += Convert.ToInt32(r[1]["TonDau"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) - Convert.ToInt32(r[1]["Xuat_CGTO"]) - Convert.ToInt32(r[1]["Xuat_CPLO"]);
                //GiaTri[1] - Duc Ton Dau
                GiaTriPre[1] += Convert.ToInt32(r[2]["TonDau"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) - Convert.ToInt32(r[2]["Xuat_CGTO"]) - Convert.ToInt32(r[2]["Xuat_CPLO"]);

                //-------------------------------------------

                //GiaTri[3] - Tong Nhap
                for (t = 0; t < 3; t++)
                {
                    GiaTriPre[3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]) - Convert.ToInt32(r[t]["Xuat_CGTI"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]);
                }
                //GiaTri[5] - Cai Nhap
                GiaTriPre[5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]) - Convert.ToInt32(r[1]["Xuat_CGTI"]) - Convert.ToInt32(r[1]["Xuat_CPLI"]);
                //GiaTri[4] - Duc Nhap
                GiaTriPre[4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]) - Convert.ToInt32(r[2]["Xuat_CGTI"]) - Convert.ToInt32(r[2]["Xuat_CPLI"]);

                //-------------------------------------------

                //GiaTri[6] - Tong Xuat
                for (t = 0; t < 3; t++)
                {
                    //GiaTri[6] += Convert.ToInt32(r[t]["Xuat_Ban"]) + Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]) - Convert.ToInt32(r[t]["Nhap_CGT"]) - Convert.ToInt32(r[t]["Nhap_CPL"]);
                    GiaTriPre[6] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[8] - Cai Xuat
                //GiaTri[8] += Convert.ToInt32(r[1]["Xuat_Ban"]) + Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGT"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPL"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]) - Convert.ToInt32(r[1]["Nhap_CGT"]) - Convert.ToInt32(r[1]["Nhap_CPL"]);
                GiaTriPre[8] += Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                //GiaTri[7] - Duc Xuat
                //GiaTri[7] += Convert.ToInt32(r[2]["Xuat_Ban"]) + Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGT"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPL"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]) - Convert.ToInt32(r[2]["Nhap_CGT"]) - Convert.ToInt32(r[2]["Nhap_CPL"]);
                GiaTriPre[7] += Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);

                //-------------------------------------------

                r = new DataRow[3];
                for (int jj = 0; jj < 3; jj++)
                {
                    r[jj] = dt.Rows[i * 6 + jj];
                }
                int[] GiaTri = new int[24];

                //-------------------------------------------

                //GiaTri[0] - Tong Ton Dau
                for (t = 0; t < 3; t++)
                {
                    GiaTri[0] += Convert.ToInt32(r[t]["TonDau"]);// +Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) - Convert.ToInt32(r[t]["Xuat_CGTO"]) - Convert.ToInt32(r[t]["Xuat_CPLO"]);
                }
                //GiaTri[2] - Cai Ton Dau
                GiaTri[2] += Convert.ToInt32(r[1]["TonDau"]);// +Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) - Convert.ToInt32(r[1]["Xuat_CGTO"]) - Convert.ToInt32(r[1]["Xuat_CPLO"]);
                //GiaTri[1] - Duc Ton Dau
                GiaTri[1] += Convert.ToInt32(r[2]["TonDau"]);// +Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) - Convert.ToInt32(r[2]["Xuat_CGTO"]) - Convert.ToInt32(r[2]["Xuat_CPLO"]);

                //-------------------------------------------

                //GiaTri[3] - Tong Nhap
                for (t = 0; t < 3; t++)
                {
                    GiaTri[3] += Convert.ToInt32(r[t]["Nhap_NhapChuong"]) + Convert.ToInt32(r[t]["Nhap_CLC"]) + Convert.ToInt32(r[t]["Nhap_CGTO"]) + Convert.ToInt32(r[t]["Nhap_CGTI"]) + Convert.ToInt32(r[t]["Nhap_CGT_D"]) + Convert.ToInt32(r[t]["Nhap_CPLO"]) + Convert.ToInt32(r[t]["Nhap_CPLI"]) + Convert.ToInt32(r[t]["Nhap_CPL_D"]) + Convert.ToInt32(r[t]["Nhap_CTT"]);// -Convert.ToInt32(r[t]["Xuat_CGTI"]) - Convert.ToInt32(r[t]["Xuat_CPLI"]);
                }
                //GiaTri[5] - Cai Nhap
                GiaTri[5] += Convert.ToInt32(r[1]["Nhap_NhapChuong"]) + Convert.ToInt32(r[1]["Nhap_CLC"]) + Convert.ToInt32(r[1]["Nhap_CGTO"]) + Convert.ToInt32(r[1]["Nhap_CGTI"]) + Convert.ToInt32(r[1]["Nhap_CGT_D"]) + Convert.ToInt32(r[1]["Nhap_CPLO"]) + Convert.ToInt32(r[1]["Nhap_CPLI"]) + Convert.ToInt32(r[1]["Nhap_CPL_D"]) + Convert.ToInt32(r[1]["Nhap_CTT"]);// -Convert.ToInt32(r[1]["Xuat_CGTI"]) - Convert.ToInt32(r[1]["Xuat_CPLI"]);
                //GiaTri[4] - Duc Nhap
                GiaTri[4] += Convert.ToInt32(r[2]["Nhap_NhapChuong"]) + Convert.ToInt32(r[2]["Nhap_CLC"]) + Convert.ToInt32(r[2]["Nhap_CGTO"]) + Convert.ToInt32(r[2]["Nhap_CGTI"]) + Convert.ToInt32(r[2]["Nhap_CGT_D"]) + Convert.ToInt32(r[2]["Nhap_CPLO"]) + Convert.ToInt32(r[2]["Nhap_CPLI"]) + Convert.ToInt32(r[2]["Nhap_CPL_D"]) + Convert.ToInt32(r[2]["Nhap_CTT"]);// -Convert.ToInt32(r[2]["Xuat_CGTI"]) - Convert.ToInt32(r[2]["Xuat_CPLI"]);

                //-------------------------------------------

                //GiaTri[6] - Tong Xuat
                for (t = 0; t < 3; t++)
                {
                    GiaTri[6] += Convert.ToInt32(r[t]["Xuat_CLC"]) + Convert.ToInt32(r[t]["Xuat_CGTO"]) + Convert.ToInt32(r[t]["Xuat_CGTI"]) + Convert.ToInt32(r[t]["Xuat_CGT_D"]) + Convert.ToInt32(r[t]["Xuat_CPLO"]) + Convert.ToInt32(r[t]["Xuat_CPLI"]) + Convert.ToInt32(r[t]["Xuat_CPL_D"]);
                }
                //GiaTri[8] - Cai Xuat
                GiaTri[8] += Convert.ToInt32(r[1]["Xuat_CLC"]) + Convert.ToInt32(r[1]["Xuat_CGTO"]) + Convert.ToInt32(r[1]["Xuat_CGTI"]) + Convert.ToInt32(r[1]["Xuat_CGT_D"]) + Convert.ToInt32(r[1]["Xuat_CPLO"]) + Convert.ToInt32(r[1]["Xuat_CPLI"]) + Convert.ToInt32(r[1]["Xuat_CPL_D"]);
                //GiaTri[7] - Duc Xuat
                GiaTri[7] += Convert.ToInt32(r[2]["Xuat_CLC"]) + Convert.ToInt32(r[2]["Xuat_CGTO"]) + Convert.ToInt32(r[2]["Xuat_CGTI"]) + Convert.ToInt32(r[2]["Xuat_CGT_D"]) + Convert.ToInt32(r[2]["Xuat_CPLO"]) + Convert.ToInt32(r[2]["Xuat_CPLI"]) + Convert.ToInt32(r[2]["Xuat_CPL_D"]);

                //-------------------------------------------

                //GiaTri[6] - Tong Ban
                for (t = 0; t < 3; t++)
                {
                    GiaTri[9] += Convert.ToInt32(r[t]["Xuat_Ban"]);
                }
                //GiaTri[8] - Cai Ban
                GiaTri[11] += Convert.ToInt32(r[1]["Xuat_Ban"]);
                //GiaTri[7] - Duc Ban
                GiaTri[10] += Convert.ToInt32(r[2]["Xuat_Ban"]);

                //-------------------------------------------

                //GiaTri[9] - Tong Chet
                for (t = 0; t < 3; t++)
                {
                    GiaTri[12] += Convert.ToInt32(r[t]["Chet"]);
                }
                //GiaTri[11] - Cai Chet
                GiaTri[14] += Convert.ToInt32(r[1]["Chet"]);
                //GiaTri[10] - Duc Chet
                GiaTri[13] += Convert.ToInt32(r[2]["Chet"]);

                //-------------------------------------------

                //GiaTri[9] - Tong LoaiThai
                for (t = 0; t < 3; t++)
                {
                    GiaTri[15] += Convert.ToInt32(r[t]["LoaiThai"]);
                }
                //GiaTri[14] - Cai Giet Mo
                GiaTri[17] += Convert.ToInt32(r[1]["LoaiThai"]);
                //GiaTri[13] - Duc Giet Mo
                GiaTri[16] += Convert.ToInt32(r[2]["LoaiThai"]);

                //-------------------------------------------

                //GiaTri[12] - Tong Giet Mo
                for (t = 0; t < 3; t++)
                {
                    GiaTri[18] += Convert.ToInt32(r[t]["GietMo"]);
                }
                //GiaTri[17] - Cai Ton Cuoi
                GiaTri[20] += Convert.ToInt32(r[1]["GietMo"]);
                //GiaTri[16] - Duc Ton Cuoi
                GiaTri[19] += Convert.ToInt32(r[2]["GietMo"]);

                //-------------------------------------------

                //GiaTri[15] - Tong Ton Cuoi
                for (t = 0; t < 3; t++)
                {
                    GiaTri[21] += Convert.ToInt32(r[t]["TonCuoi"]);
                }
                //GiaTri[17] - Cai Ton Cuoi
                GiaTri[23] += Convert.ToInt32(r[1]["TonCuoi"]);
                //GiaTri[16] - Duc Ton Cuoi
                GiaTri[22] += Convert.ToInt32(r[2]["TonCuoi"]);

                for (int y = 0; y < 3; y++)
                {
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTriPre[y * 3], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTriPre[y * 3 + 1], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTriPre[y * 3 + 2], 0) + "</td>");
                }
                for (int y = 0; y < 5; y++)
                {
                    sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 1], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[y * 3 + 2], 0) + "</td>");
                }
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[18] + GiaTri[15], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(dtPLGM.Rows[i * 4 + 1]["GietMo"], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Convert.ToInt32(dtPLGM.Rows[i * 4]["GietMo"]) + GiaTri[15], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[19] + GiaTri[16], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[20] + GiaTri[17], 0) + "</td>");
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[21], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[22], 0) + "</td><td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(GiaTri[23], 0) + "</td>");
                //ThucAn Start
                //Tinh iThucAn
                for (int l = 0; l < dtSub.Rows.Count; l++)
                {
                    DataRow rta = dtSub.Rows[l];
                    if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                    {
                        iThucAn = l;
                        break;
                    }
                }
                int j = 0;
                for (int h = iThucAn; h < dtSub.Rows.Count; h++)
                {
                    DataRow rta = dtSub.Rows[h];
                    if (Convert.ToInt32(rta["LoaiCa"]) == IDLoaiCa[i])
                    {
                        sb.Append("<td>" + rta["TenVatTu"].ToString() + @"</td><td style='text-align:right;mso-number-format:" + Config.ExcelFormat(scale) + "'>" + Config.ToXVal2(rta["KhoiLuong"], scale) + "</td>");
                        j++;
                    }
                    else
                    {
                        iThucAn = h;
                        break;
                    }
                }
                for (int k = j; k < SoCotThucAn / 2; k++) sb.Append("<td></td><td></td>");
                //ThucAn End
                sb.Append("<td></td>");
                sb.Append("</tr>");
            }
        }


        public void TongHop_Others()
        {
            //Tính tổng
            int[] Tong = new int[24];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow r = dt.Rows[i];
                Tong[0] += Convert.ToInt32(r["TonDau"]);// +Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[2] += Convert.ToInt32(r["TonDau"]);// +Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[1] += Convert.ToInt32(r["TonDau"]);// +Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                }

                Tong[3] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLO"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]);// -Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[5] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLO"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]);// -Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[4] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLO"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]);// -Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                }

                Tong[6] += Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGTO"]) + Convert.ToInt32(r["Xuat_CGTI"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPLO"]) + Convert.ToInt32(r["Xuat_CPLI"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[8] += Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGTO"]) + Convert.ToInt32(r["Xuat_CGTI"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPLO"]) + Convert.ToInt32(r["Xuat_CPLI"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[7] += Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGTO"]) + Convert.ToInt32(r["Xuat_CGTI"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPLO"]) + Convert.ToInt32(r["Xuat_CPLI"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                }

                Tong[9] += Convert.ToInt32(r["Xuat_Ban"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[11] += Convert.ToInt32(r["Xuat_Ban"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[10] += Convert.ToInt32(r["Xuat_Ban"]);
                }

                Tong[12] += Convert.ToInt32(r["Chet"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[14] += Convert.ToInt32(r["Chet"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[13] += Convert.ToInt32(r["Chet"]);
                }

                Tong[15] += Convert.ToInt32(r["LoaiThai"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[17] += Convert.ToInt32(r["LoaiThai"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[16] += Convert.ToInt32(r["LoaiThai"]);
                }

                Tong[18] += Convert.ToInt32(r["GietMo"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[20] += Convert.ToInt32(r["GietMo"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[19] += Convert.ToInt32(r["GietMo"]);
                }

                Tong[21] += Convert.ToInt32(r["TonCuoi"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[23] += Convert.ToInt32(r["TonCuoi"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[22] += Convert.ToInt32(r["TonCuoi"]);
                }
            }
            //Cong them nhap, xuat chuyen chuong tu chuong dda
            for (int i = 0; i < dt_DDA.Rows.Count; i++)
            {
                DataRow r = dt_DDA.Rows[i];

                Tong[3] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[5] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[4] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }

                Tong[6] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[8] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[7] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
            }
            //Cong them nhap, xuat chuyen chuong tu chuong ngoai dda
            for (int i = 0; i < dt_NgoaiDDA.Rows.Count; i++)
            {
                DataRow r = dt_NgoaiDDA.Rows[i];

                Tong[3] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[5] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[4] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }

                Tong[6] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[8] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[7] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
            }

            //Tính tổng phân loại giêt mô
            int[] TongPLGM = new int[2];
            for (int i = 0; i < dtPLGM.Rows.Count; i++)
            {
                DataRow r = dtPLGM.Rows[i];
                if (i % 2 == 1)
                {
                    TongPLGM[1] += Convert.ToInt32(r["GietMo"]);
                }
                else
                {
                    TongPLGM[0] += Convert.ToInt32(r["GietMo"]);
                }
            }
            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td></td><td>Tổng cộng</td>");
            for (int i = 0; i < 15; i++)
            {
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[i], 0) + "</td>");
            }
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[18] + Tong[15], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TongPLGM[1], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TongPLGM[0] + Tong[15], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[19] + Tong[16], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[20] + Tong[17], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[21], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[22], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[23], 0) + "</td>");

            for (int i = 0; i < SoCotThucAn; i++)
            {
                sb.Append(@"<td></td>");
            }
            sb.Append("<td></td></tr>");
            sb.Append("</tbody></table>");
        }

        public void TongHop_FirstOctCGD()
        {
            //Tính tổng
            int[] Tong = new int[24];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow r = dt.Rows[i];
                Tong[0] += Convert.ToInt32(r["TonDau"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[2] += Convert.ToInt32(r["TonDau"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[1] += Convert.ToInt32(r["TonDau"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                }

                Tong[3] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]) - Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[5] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]) - Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[4] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]) - Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                }

                //Tong[6] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL"]) + Convert.ToInt32(r["Xuat_CPL_D"]) - Convert.ToInt32(r["Nhap_CGT"]) - Convert.ToInt32(r["Nhap_CPL"]);
                Tong[6] += Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                if (i >= 42 && i % 3 == 1)
                {
                    //Tong[8] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL"]) + Convert.ToInt32(r["Xuat_CPL_D"]) - Convert.ToInt32(r["Nhap_CGT"]) - Convert.ToInt32(r["Nhap_CPL"]);
                    Tong[8] += Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    //Tong[7] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL"]) + Convert.ToInt32(r["Xuat_CPL_D"]) - Convert.ToInt32(r["Nhap_CGT"]) - Convert.ToInt32(r["Nhap_CPL"]);
                    Tong[7] += Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                }

                Tong[21] += Convert.ToInt32(r["TonCuoi"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[23] += Convert.ToInt32(r["TonCuoi"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[22] += Convert.ToInt32(r["TonCuoi"]);
                }
            }
            //Cong them nhap, xuat chuyen chuong tu chuong dda
            for (int i = 0; i < dt_DDA.Rows.Count; i++)
            {
                DataRow r = dt_DDA.Rows[i];

                Tong[3] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[5] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[4] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }

                Tong[6] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[8] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[7] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
            }
            //Cong them nhap, xuat chuyen chuong tu chuong ngoai dda
            for (int i = 0; i < dt_NgoaiDDA.Rows.Count; i++)
            {
                DataRow r = dt_NgoaiDDA.Rows[i];

                Tong[3] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[5] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[4] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }

                Tong[6] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[8] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[7] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
            }

            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td></td><td>Tổng cộng</td>");
            for (int i = 0; i < 9; i++)
            {
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[i], 0) + "</td>");
            }
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[21], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[22], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[23], 0) + "</td>");
            sb.Append("<td></td></tr>");
            sb.Append("</tbody></table>");
        }

        public void TongHop_GomCGD()
        {
            //Tính tổng
            int[] Tong = new int[24];
            for (int i = 0; i < dtPre.Rows.Count; i++)
            {
                DataRow r = dtPre.Rows[i];
                Tong[0] += Convert.ToInt32(r["TonDau"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[2] += Convert.ToInt32(r["TonDau"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[1] += Convert.ToInt32(r["TonDau"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                }

                Tong[3] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]) - Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[5] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]) - Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[4] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]) - Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                }

                //Tong[6] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL"]) + Convert.ToInt32(r["Xuat_CPL_D"]) - Convert.ToInt32(r["Nhap_CGT"]) - Convert.ToInt32(r["Nhap_CPL"]);
                Tong[6] += Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                if (i >= 42 && i % 3 == 1)
                {
                    //Tong[8] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL"]) + Convert.ToInt32(r["Xuat_CPL_D"]) - Convert.ToInt32(r["Nhap_CGT"]) - Convert.ToInt32(r["Nhap_CPL"]);
                    Tong[8] += Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    //Tong[7] += Convert.ToInt32(r["Xuat_Ban"]) + Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL"]) + Convert.ToInt32(r["Xuat_CPL_D"]) - Convert.ToInt32(r["Nhap_CGT"]) - Convert.ToInt32(r["Nhap_CPL"]);
                    Tong[7] += Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                }
            }
            //Cong them nhap, xuat chuyen chuong tu chuong dda
            for (int i = 0; i < dt_DDAPre.Rows.Count; i++)
            {
                DataRow r = dt_DDAPre.Rows[i];

                Tong[3] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[5] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[4] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }

                Tong[6] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[8] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[7] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
            }
            //Cong them nhap, xuat chuyen chuong tu chuong ngoai dda
            for (int i = 0; i < dt_NgoaiDDAPre.Rows.Count; i++)
            {
                DataRow r = dt_NgoaiDDAPre.Rows[i];

                Tong[3] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[5] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[4] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }

                Tong[6] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[8] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[7] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
            }

            sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td></td><td>Tổng cộng</td>");
            for (int i = 0; i < 9; i++)
            {
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[i], 0) + "</td>");
            }



            /***************************************/



            Tong = new int[24];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow r = dt.Rows[i];
                Tong[0] += Convert.ToInt32(r["TonDau"]);// +Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[2] += Convert.ToInt32(r["TonDau"]);// +Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[1] += Convert.ToInt32(r["TonDau"]);// +Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CPLO"]) - Convert.ToInt32(r["Xuat_CGTO"]) - Convert.ToInt32(r["Xuat_CPLO"]);
                }

                Tong[3] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLO"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]);// -Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[5] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLO"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]);// -Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[4] += Convert.ToInt32(r["Nhap_NhapChuong"]) + Convert.ToInt32(r["Nhap_CLC"]) + Convert.ToInt32(r["Nhap_CGTO"]) + Convert.ToInt32(r["Nhap_CGTI"]) + Convert.ToInt32(r["Nhap_CGT_D"]) + Convert.ToInt32(r["Nhap_CPLO"]) + Convert.ToInt32(r["Nhap_CPLI"]) + Convert.ToInt32(r["Nhap_CPL_D"]) + Convert.ToInt32(r["Nhap_CTT"]);// -Convert.ToInt32(r["Xuat_CGTI"]) - Convert.ToInt32(r["Xuat_CPLI"]);
                }

                Tong[6] += Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGTO"]) + Convert.ToInt32(r["Xuat_CGTI"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPLO"]) + Convert.ToInt32(r["Xuat_CPLI"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[8] += Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGTO"]) + Convert.ToInt32(r["Xuat_CGTI"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPLO"]) + Convert.ToInt32(r["Xuat_CPLI"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[7] += Convert.ToInt32(r["Xuat_CLC"]) + Convert.ToInt32(r["Xuat_CGTO"]) + Convert.ToInt32(r["Xuat_CGTI"]) + Convert.ToInt32(r["Xuat_CGT_D"]) + Convert.ToInt32(r["Xuat_CPLO"]) + Convert.ToInt32(r["Xuat_CPLI"]) + Convert.ToInt32(r["Xuat_CPL_D"]);
                }

                Tong[9] += Convert.ToInt32(r["Xuat_Ban"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[11] += Convert.ToInt32(r["Xuat_Ban"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[10] += Convert.ToInt32(r["Xuat_Ban"]);
                }

                Tong[12] += Convert.ToInt32(r["Chet"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[14] += Convert.ToInt32(r["Chet"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[13] += Convert.ToInt32(r["Chet"]);
                }

                Tong[15] += Convert.ToInt32(r["LoaiThai"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[17] += Convert.ToInt32(r["LoaiThai"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[16] += Convert.ToInt32(r["LoaiThai"]);
                }

                Tong[18] += Convert.ToInt32(r["GietMo"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[20] += Convert.ToInt32(r["GietMo"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[19] += Convert.ToInt32(r["GietMo"]);
                }

                Tong[21] += Convert.ToInt32(r["TonCuoi"]);
                if (i >= 42 && i % 3 == 1)
                {
                    Tong[23] += Convert.ToInt32(r["TonCuoi"]);
                }
                if (i >= 42 && i % 3 == 2)
                {
                    Tong[22] += Convert.ToInt32(r["TonCuoi"]);
                }
            }
            //Cong them nhap, xuat chuyen chuong tu chuong dda
            for (int i = 0; i < dt_DDA.Rows.Count; i++)
            {
                DataRow r = dt_DDA.Rows[i];

                Tong[3] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[5] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[4] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }

                Tong[6] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[8] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[7] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
            }
            //Cong them nhap, xuat chuyen chuong tu chuong ngoai dda
            for (int i = 0; i < dt_NgoaiDDA.Rows.Count; i++)
            {
                DataRow r = dt_NgoaiDDA.Rows[i];

                Tong[3] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[5] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[4] += Convert.ToInt32(r["Nhap_ChuyenChuong"]);
                }

                Tong[6] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                if (i >= 6 && i % 3 == 1)
                {
                    Tong[8] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
                if (i >= 6 && i % 3 == 2)
                {
                    Tong[7] += Convert.ToInt32(r["Xuat_ChuyenChuong"]);
                }
            }

            //Tính tổng phân loại giêt mô
            int[] TongPLGM = new int[2];
            for (int i = 0; i < dtPLGM.Rows.Count; i++)
            {
                DataRow r = dtPLGM.Rows[i];
                if (i % 2 == 1)
                {
                    TongPLGM[1] += Convert.ToInt32(r["GietMo"]);
                }
                else
                {
                    TongPLGM[0] += Convert.ToInt32(r["GietMo"]);
                }
            }
            for (int i = 0; i < 15; i++)
            {
                sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[i], 0) + "</td>");
            }
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[18] + Tong[15], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TongPLGM[1], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(TongPLGM[0] + Tong[15], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[19] + Tong[16], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[20] + Tong[17], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[21], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[22], 0) + "</td>");
            sb.Append("<td style='font-weight:bold;text-align:right;'>" + Config.ToXVal2(Tong[23], 0) + "</td>");

            for (int i = 0; i < SoCotThucAn; i++)
            {
                sb.Append(@"<td></td>");
            }

            sb.Append("<td></td></tr>");
            sb.Append("</tbody></table>");
        }

        /*************************************************************/

        public void createContent_Others(bool Excel)
        {
            createTableHeader(Excel);

            CaCon_Others();

            CaTanDung();

            CaMotNamST1_Others();

            CaST2_NgoaiDDA_Others();

            CaST2_DDA_Others();

            CaHauBi_NgoaiDDA_Tong_Others();

            CaHauBi_NgoaiDDA_ChiTiet_Others();

            CaHauBi_DDA_Others();

            Ca234_Others();

            CaSS_Others();

            TongHop_Others();
        }

        public void createContent_FirstOctCGD(bool Excel)
        {
            createTableHeaderCGD(Excel);

            CaCon_FirstOctCGD();

            CaTanDungCGD();

            CaMotNamST1_FirstOctCGD();

            CaST2_NgoaiDDA_FirstOctCGD();

            CaST2_DDA_FirstOctCGD();

            CaHauBi_NgoaiDDA_Tong_FirstOctCGD();

            CaHauBi_NgoaiDDA_ChiTiet_FirstOctCGD();

            CaHauBi_DDA_FirstOctCGD();

            Ca234_FirstOctCGD();

            CaSS_FirstOctCGD();

            TongHop_FirstOctCGD();
        }

        public void createContent_GomCGD(bool Excel)
        {
            createTableHeader_GomCGD(Excel);

            CaCon_GomCGD();

            CaTanDung_GomCGD();

            CaMotNamST1_GomCGD();

            CaST2_NgoaiDDA_GomCGD();

            CaST2_DDA_GomCGD();

            CaHauBi_NgoaiDDA_Tong_GomCGD();

            CaHauBi_NgoaiDDA_ChiTiet_GomCGD();

            CaHauBi_DDA_GomCGD();

            Ca234_GomCGD();

            CaSS_GomCGD();

            TongHop_GomCGD();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                prepareExcelFile("BDTD", Response);

                prepareData();

                prepareTitleExcel();

                createContent_Others(true);

                prepareFooterExcel();

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
                prepareData();

                prepareTitleView();

                createContent_Others(false);

                lt.Text = sb.ToString();
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnExcelCGD_Click(object sender, EventArgs e)
        {
            try
            {
                prepareExcelFile("CGD", Response);

                prepareDataCGD();

                prepareTitleExcel();

                createContent_FirstOctCGD(true);

                prepareFooterExcel();

                Response.Write(sb.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnViewCGD_Click(object sender, EventArgs e)
        {
            try
            {
                prepareDataCGD();

                prepareTitleView();

                createContent_FirstOctCGD(false);

                lt.Text = sb.ToString();
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnExcel_GomCGD_Click(object sender, EventArgs e)
        {
            try
            {
                prepareExcelFile("BDTD", Response);

                prepareData_GomCGD();

                prepareTitleExcel();

                createContent_GomCGD(true);

                prepareFooterExcel();

                Response.Write(sb.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnView_GomCGD_Click(object sender, EventArgs e)
        {
            try
            {
                prepareData_GomCGD();

                prepareTitleView();

                createContent_GomCGD(false);

                lt.Text = sb.ToString();
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}