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
    public partial class r_xuatnhapdacasau : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        int scale = 0;
        Dictionary<int, string> dicDa;
        /*Must have for baocao coding template*/
        DataTable dt = null;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        /*End Must have for baocao coding template*/
        DataTable tblDCS;
        int countDCS;
        string strDaChon;
        ArrayList tong = new ArrayList();
        ArrayList tong1 = new ArrayList();

        private void BindControls()
        {
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable tblDaCBMoi = csCont.VatTu_GetDCS_Order_Type("DCS_CBM");
            ddlDaCBMoi.DataSource = tblDaCBMoi;
            ddlDaCBMoi.DataValueField = "IDVatTu";
            ddlDaCBMoi.DataTextField = "TenVatTu";
            ddlDaCBMoi.DataBind();

            DataTable tblDaCLMoi = csCont.VatTu_GetDCS_Order_Type("DCS_CLM");
            ddlDaCLMoi.DataSource = tblDaCLMoi;
            ddlDaCLMoi.DataValueField = "IDVatTu";
            ddlDaCLMoi.DataTextField = "TenVatTu";
            ddlDaCLMoi.DataBind();

            DataTable tblDaMDL = csCont.VatTu_GetDCS_Order_Type("DCS_MDL");
            ddlDaMDL.DataSource = tblDaMDL;
            ddlDaMDL.DataValueField = "IDVatTu";
            ddlDaMDL.DataTextField = "TenVatTu";
            ddlDaMDL.DataBind();

            DataTable tblDauDaLung = csCont.VatTu_GetDCS_Order_Type("DCS_DDL");
            ddlDauDaLung.DataSource = tblDauDaLung;
            ddlDauDaLung.DataValueField = "IDVatTu";
            ddlDauDaLung.DataTextField = "TenVatTu";
            ddlDauDaLung.DataBind();

            DataTable tblDa = csCont.VatTu_GetDCS_Order_Type("");
            ddlDa.DataSource = tblDa;
            ddlDa.DataValueField = "IDVatTu";
            ddlDa.DataTextField = "TenVatTu";
            ddlDa.DataBind();

            DataTable tblDaCB = csCont.VatTu_GetDCS_Order_Type("CB");
            ddlDaCB.DataSource = tblDaCB;
            ddlDaCB.DataValueField = "IDVatTu";
            ddlDaCB.DataTextField = "TenVatTu";
            ddlDaCB.DataBind();

            DataTable tblDaCL = csCont.VatTu_GetDCS_Order_Type("CL");
            ddlDaCL.DataSource = tblDaCL;
            ddlDaCL.DataValueField = "IDVatTu";
            ddlDaCL.DataTextField = "TenVatTu";
            ddlDaCL.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_DCS_Scale"]);
                if (!Page.IsPostBack)
                {
                    BindControls();
                }
                else
                {
                    DataTable tblDa = csCont.VatTu_GetDCS_Order_Type("All");
                    dicDa = new Dictionary<int, string>();
                    foreach (DataRow r in tblDa.Rows)
                    {
                        dicDa.Add(Convert.ToInt32(r["IDVatTu"]), r["TenVatTu"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        /*Must have for baocao coding template: fill data to dt and return title*/
        public string createDataAndTieuDe()
        {
            if (ddlLoaiBC.SelectedValue == "TongHop")
            {
                string tieude = "";
                string strDa = "";
                string strSQL = "";
                SqlParameter[] param;
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                tieude = "<b>BẢNG THEO DÕI XUẤT, NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
                if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");

                tblDCS = new DataTable();
                tblDCS.Columns.Add("IDVatTu", typeof(Int32));
                tblDCS.Columns.Add("TenVatTu", typeof(String));
                string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < arrDa.Length; j++)
                {
                    strDa += "@" + arrDa[j] + "@";
                    DataRow r = tblDCS.NewRow();
                    r["IDVatTu"] = int.Parse(arrDa[j]);
                    r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
                    tblDCS.Rows.Add(r);
                }
                countDCS = tblDCS.Rows.Count;
                strSQL = "QLCS_BCTK_XuatNhapDaCaSau_FilterDa";
                param = new SqlParameter[4];
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                param[3] = new SqlParameter("@StrDa", strDa);
                dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                return tieude;
            }
            else if (ddlLoaiBC.SelectedValue == "Nhap")
            {
                string tieude = "";
                string strDa = "";
                string strSQL = "";
                SqlParameter[] param;
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                tieude = "<b>BẢNG THEO DÕI NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
                if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");

                tblDCS = new DataTable();
                tblDCS.Columns.Add("IDVatTu", typeof(Int32));
                tblDCS.Columns.Add("TenVatTu", typeof(String));
                string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < arrDa.Length; j++)
                {
                    strDa += "@" + arrDa[j] + "@";
                    DataRow r = tblDCS.NewRow();
                    r["IDVatTu"] = int.Parse(arrDa[j]);
                    r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
                    tblDCS.Rows.Add(r);
                }
                countDCS = tblDCS.Rows.Count;
                strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
                param = new SqlParameter[5];
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                param[3] = new SqlParameter("@StrDa", strDa);
                param[4] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
                dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                return tieude;
            }
            else if (ddlLoaiBC.SelectedValue == "NhapBT")
            {
                string tieude = "";
                string strDa = "";
                string strSQL = "";
                SqlParameter[] param;
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                tieude = "<b>BẢNG THEO DÕI NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
                if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");

                tblDCS = new DataTable();
                tblDCS.Columns.Add("IDVatTu", typeof(Int32));
                tblDCS.Columns.Add("TenVatTu", typeof(String));
                string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < arrDa.Length; j++)
                {
                    strDa += "@" + arrDa[j] + "@";
                    DataRow r = tblDCS.NewRow();
                    r["IDVatTu"] = int.Parse(arrDa[j]);
                    r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
                    tblDCS.Rows.Add(r);
                }
                countDCS = tblDCS.Rows.Count;
                strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
                param = new SqlParameter[5];
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                param[3] = new SqlParameter("@StrDa", strDa);
                param[4] = new SqlParameter("@LoaiBC", "Nhap");
                dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                return tieude;
            }
            else if (ddlLoaiBC.SelectedValue == "NhapDC")
            {
                string tieude = "";
                string strDa = "";
                string strSQL = "";
                SqlParameter[] param;
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                tieude = "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
                if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");

                tblDCS = new DataTable();
                tblDCS.Columns.Add("IDVatTu", typeof(Int32));
                tblDCS.Columns.Add("TenVatTu", typeof(String));
                string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < arrDa.Length; j++)
                {
                    strDa += "@" + arrDa[j] + "@";
                    DataRow r = tblDCS.NewRow();
                    r["IDVatTu"] = int.Parse(arrDa[j]);
                    r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
                    tblDCS.Rows.Add(r);
                }
                countDCS = tblDCS.Rows.Count;
                strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
                param = new SqlParameter[5];
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                param[3] = new SqlParameter("@StrDa", strDa);
                param[4] = new SqlParameter("@LoaiBC", "Nhap");
                dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                return tieude;
            }
            else if (ddlLoaiBC.SelectedValue == "Xuat")
            {
                string tieude = "";
                string strDa = "";
                string strSQL = "";
                SqlParameter[] param;
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                tieude = "<b>BẢNG THEO DÕI XUẤT DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
                if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");

                tblDCS = new DataTable();
                tblDCS.Columns.Add("IDVatTu", typeof(Int32));
                tblDCS.Columns.Add("TenVatTu", typeof(String));
                string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < arrDa.Length; j++)
                {
                    strDa += "@" + arrDa[j] + "@";
                    DataRow r = tblDCS.NewRow();
                    r["IDVatTu"] = int.Parse(arrDa[j]);
                    r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
                    tblDCS.Rows.Add(r);
                }
                countDCS = tblDCS.Rows.Count;
                strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
                param = new SqlParameter[5];
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                param[3] = new SqlParameter("@StrDa", strDa);
                param[4] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
                dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                return tieude;
            }
            else if (ddlLoaiBC.SelectedValue == "XuatBT")
            {
                string tieude = "";
                string strDa = "";
                string strSQL = "";
                SqlParameter[] param;
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                tieude = "<b>BẢNG THEO DÕI XUẤT DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
                if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");

                tblDCS = new DataTable();
                tblDCS.Columns.Add("IDVatTu", typeof(Int32));
                tblDCS.Columns.Add("TenVatTu", typeof(String));
                string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < arrDa.Length; j++)
                {
                    strDa += "@" + arrDa[j] + "@";
                    DataRow r = tblDCS.NewRow();
                    r["IDVatTu"] = int.Parse(arrDa[j]);
                    r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
                    tblDCS.Rows.Add(r);
                }
                countDCS = tblDCS.Rows.Count;
                strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
                param = new SqlParameter[5];
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                param[3] = new SqlParameter("@StrDa", strDa);
                param[4] = new SqlParameter("@LoaiBC", "Xuat");
                dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                return tieude;
            }
            else if (ddlLoaiBC.SelectedValue == "XuatDC")
            {
                string tieude = "";
                string strDa = "";
                string strSQL = "";
                SqlParameter[] param;
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                tieude = "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
                if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");

                tblDCS = new DataTable();
                tblDCS.Columns.Add("IDVatTu", typeof(Int32));
                tblDCS.Columns.Add("TenVatTu", typeof(String));
                string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < arrDa.Length; j++)
                {
                    strDa += "@" + arrDa[j] + "@";
                    DataRow r = tblDCS.NewRow();
                    r["IDVatTu"] = int.Parse(arrDa[j]);
                    r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
                    tblDCS.Rows.Add(r);
                }
                countDCS = tblDCS.Rows.Count;
                strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
                param = new SqlParameter[5];
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                param[3] = new SqlParameter("@StrDa", strDa);
                param[4] = new SqlParameter("@LoaiBC", "Xuat");
                dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                return tieude;
            }
            else if (ddlLoaiBC.SelectedValue == "XuatH")
            {
                string tieude = "";
                string strDa = "";
                string strSQL = "";
                SqlParameter[] param;
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                tieude = "<b>BẢNG THEO DÕI HỦY DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
                if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");

                tblDCS = new DataTable();
                tblDCS.Columns.Add("IDVatTu", typeof(Int32));
                tblDCS.Columns.Add("TenVatTu", typeof(String));
                string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < arrDa.Length; j++)
                {
                    strDa += "@" + arrDa[j] + "@";
                    DataRow r = tblDCS.NewRow();
                    r["IDVatTu"] = int.Parse(arrDa[j]);
                    r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
                    tblDCS.Rows.Add(r);
                }
                countDCS = tblDCS.Rows.Count;
                strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
                param = new SqlParameter[5];
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                param[3] = new SqlParameter("@StrDa", strDa);
                param[4] = new SqlParameter("@LoaiBC", "Xuat");
                dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                return tieude;
            }
            return "";
        }

        /*Must have for baocao coding template: create table header (just the tr and th of the table header)*/
        public string createTableHeader_Ngang()
        {
            if (ddlLoaiBC.SelectedValue == "TongHop")
            {
                for (int l = 0; l < countDCS * 3; l++) tong.Add(new decimal(0));
                for (int l1 = 0; l1 < countDCS; l1++) tong1.Add(new decimal(0));
                int tonIndex1 = countDCS * 2;
                foreach (DataRow r in tblDCS.Rows)
                {
                    tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                    tong1[tonIndex1 - (countDCS * 2)] = tong[tonIndex1];
                    tonIndex1++;
                }
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            if (r[rDCS["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rDCS["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0 && ((decimal)tong[k + countDCS + countDCS]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k + countDCS + countDCS);
                        tong.RemoveAt(k + countDCS);
                        tong.RemoveAt(k);
                        tong1.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có xuất nhập tồn đối với da được chọn!";
                    return "";
                }
                //int countDCS = tblDCS.Rows.Count;
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                s.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th rowspan=2>Ngày</th>
                              <th colspan=" + countDCS.ToString() + @">Nhập (tấm/cái)</th>
                              <th colspan=" + countDCS.ToString() + @">Xuất (tấm/cái)</th>
                              <th colspan=" + countDCS.ToString() + @">Tồn (tấm/cái)</th>
                              <th rowspan=2>Ghi chú</th>
                             </tr>
                             <tr style='font-weight:bold; vertical-align:middle;'>");
                for (int i = 0; i < 3; i++)
                {
                    foreach (DataRow r in tblDCS.Rows)
                    {
                        s.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                }
                s.Append("</tr>");
                return s.ToString();
            }
            else if (ddlLoaiBC.SelectedValue == "Nhap")
            {
                for (int l = 0; l < countDCS * 2; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k + countDCS);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có nhập đối với da được chọn!";
                    return "";
                }
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                s.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Ngày</th>
                          <th colspan=" + countDCS.ToString() + @">Nhập (tấm/cái)</th>
                          <th colspan=" + countDCS.ToString() + @">Nhập điều chỉnh (tấm/cái)</th>
                          <th rowspan=2>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>");
                for (int i = 0; i < 2; i++)
                {
                    foreach (DataRow r in tblDCS.Rows)
                    {
                        s.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                }
                s.Append("</tr>");
                return s.ToString();
            }
            else if (ddlLoaiBC.SelectedValue == "NhapBT")
            {
                for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có nhập đối với da được chọn!";
                    return "";
                }
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                s.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Ngày</th>
                          <th colspan=" + countDCS.ToString() + @">Nhập (tấm/cái)</th>
                          <th rowspan=2>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>");
                foreach (DataRow r in tblDCS.Rows)
                {
                    s.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                }
                s.Append("</tr>");
                return s.ToString();
            }
            else if (ddlLoaiBC.SelectedValue == "NhapDC")
            {
                for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có nhập điều chỉnh đối với da được chọn!";
                    return "";
                }
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                s.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Ngày</th>
                          <th colspan=" + countDCS.ToString() + @">Nhập điều chỉnh (tấm/cái)</th>
                          <th rowspan=2>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>");
                foreach (DataRow r in tblDCS.Rows)
                {
                    s.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                }
                s.Append("</tr>");
                return s.ToString();
            }
            else if (ddlLoaiBC.SelectedValue == "Xuat")
            {
                for (int l = 0; l < countDCS * 3; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0 && ((decimal)tong[k + countDCS + countDCS]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k + countDCS + countDCS);
                        tong.RemoveAt(k + countDCS);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có xuất đối với da được chọn!";
                    return "";
                }
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                s.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Ngày</th>
                          <th colspan=" + countDCS.ToString() + @">Xuất (tấm/cái)</th>
                          <th colspan=" + countDCS.ToString() + @">Xuất điều chỉnh (tấm/cái)</th>
                          <th colspan=" + countDCS.ToString() + @">Hủy (tấm/cái)</th>
                          <th rowspan=2>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>");
                for (int i = 0; i < 3; i++)
                {
                    foreach (DataRow r in tblDCS.Rows)
                    {
                        s.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                    }
                }
                s.Append("</tr>");
                return s.ToString();
            }
            else if (ddlLoaiBC.SelectedValue == "XuatBT")
            {
                for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có xuất đối với da được chọn!";
                    return "";
                }
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                s.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Ngày</th>
                          <th colspan=" + countDCS.ToString() + @">Xuất (tấm/cái)</th>
                          <th rowspan=2>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>");
                foreach (DataRow r in tblDCS.Rows)
                {
                    s.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                }
                s.Append("</tr>");
                return s.ToString();
            }
            else if (ddlLoaiBC.SelectedValue == "XuatDC")
            {
                for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có xuất điều chỉnh đối với da được chọn!";
                    return "";
                }
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                s.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Ngày</th>
                          <th colspan=" + countDCS.ToString() + @">Xuất điều chỉnh (tấm/cái)</th>
                          <th rowspan=2>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>");
                foreach (DataRow r in tblDCS.Rows)
                {
                    s.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                }
                s.Append("</tr>");
                return s.ToString();
            }
            else if (ddlLoaiBC.SelectedValue == "XuatH")
            {
                for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có hủy đối với da được chọn!";
                    return "";
                }
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                s.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th rowspan=2>Ngày</th>
                          <th colspan=" + countDCS.ToString() + @">Hủy (tấm/cái)</th>
                          <th rowspan=2>Ghi chú</th>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>");
                foreach (DataRow r in tblDCS.Rows)
                {
                    s.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
                }
                s.Append("</tr>");
                return s.ToString();
            }
            return "";
        }

        public string createTableHeader_Doc()
        {
            if (ddlLoaiBC.SelectedValue == "TongHop")
            {
                for (int l = 0; l < countDCS * 3; l++) tong.Add(new decimal(0));
                for (int l1 = 0; l1 < countDCS; l1++) tong1.Add(new decimal(0));
                int tonIndex1 = countDCS * 2;
                foreach (DataRow r in tblDCS.Rows)
                {
                    tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                    tong1[tonIndex1 - (countDCS * 2)] = tong[tonIndex1];
                    tonIndex1++;
                }
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            if (r[rDCS["IDVatTu"].ToString()] != DBNull.Value)
                            {
                                tong[j] = Config.ToDecimal(r[rDCS["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0 && ((decimal)tong[k + countDCS + countDCS]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k + countDCS + countDCS);
                        tong.RemoveAt(k + countDCS);
                        tong.RemoveAt(k);
                        tong1.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có xuất nhập tồn đối với da được chọn!";
                    return "";
                }
                return @"<tr style='font-weight:bold; vertical-align:middle;'>
                              <th>Ngày</th><th>Da cá sấu</th>
                              <th>Nhập (tấm/cái)</th>
                              <th>Xuất (tấm/cái)</th>
                              <th>Tồn (tấm/cái)</th>
                              <th>Ghi chú</th>
                             </tr>";
            }
            else if (ddlLoaiBC.SelectedValue == "Nhap")
            {
                for (int l = 0; l < countDCS * 2; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k + countDCS);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có nhập đối với da được chọn!";
                    return "";
                }
                return @"<tr style='font-weight:bold; vertical-align:middle;'>
                            <th>Ngày</th><th>Da cá sấu</th>
                              <th>Nhập (tấm/cái)</th>
                              <th>Nhập điều chỉnh (tấm/cái)</th>
                              <th>Ghi chú</th>
                             </tr>";
            }
            else if (ddlLoaiBC.SelectedValue == "NhapBT")
            {
                for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có nhập đối với da được chọn!";
                    return "";
                }
                return @"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th>Ngày</th><th>Da cá sấu</th>
                              <th>Nhập (tấm/cái)</th>
                              <th>Ghi chú</th>
                             </tr>";
            }
            else if (ddlLoaiBC.SelectedValue == "NhapDC")
            {
                for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có nhập điều chỉnh đối với da được chọn!";
                    return "";
                }
                return @"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th>Ngày</th><th>Da cá sấu</th>
                              <th>Nhập điều chỉnh (tấm/cái)</th>
                              <th>Ghi chú</th>
                             </tr>";
            }
            else if (ddlLoaiBC.SelectedValue == "Xuat")
            {
                for (int l = 0; l < countDCS * 3; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0 && ((decimal)tong[k + countDCS + countDCS]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k + countDCS + countDCS);
                        tong.RemoveAt(k + countDCS);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có xuất đối với da được chọn!";
                    return "";
                }
                return @"<tr style='font-weight:bold; vertical-align:middle;'>
                            <th>Ngày</th><th>Da cá sấu</th>
                              <th>Xuất (tấm/cái)</th>
                                <th>Xuất điều chỉnh (tấm/cái)</th>
                                <th>Hủy (tấm/cái)</th>
                              <th>Ghi chú</th>
                             </tr>";
            }
            else if (ddlLoaiBC.SelectedValue == "XuatBT")
            {
                for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có xuất đối với da được chọn!";
                    return "";
                }
                return @"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th>Ngày</th><th>Da cá sấu</th>
                              <th>Xuất (tấm/cái)</th>
                              <th>Ghi chú</th>
                             </tr>";
            }
            else if (ddlLoaiBC.SelectedValue == "XuatDC")
            {
                for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có xuất điều chỉnh đối với da được chọn!";
                    return "";
                }
                return @"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th>Ngày</th><th>Da cá sấu</th>
                                <th>Xuất điều chỉnh (tấm/cái)</th>
                              <th>Ghi chú</th>
                             </tr>";
            }
            else if (ddlLoaiBC.SelectedValue == "XuatH")
            {
                for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rDCS["IDVatTu"].ToString()]);
                            j++;
                        }
                    }
                }
                for (int k = countDCS - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0)
                    {
                        tblDCS.Rows.RemoveAt(k);
                        tong.RemoveAt(k);
                        countDCS--;
                    }
                }
                if (countDCS == 0)
                {
                    lblMessage.Text = "Không có hủy đối với da được chọn!";
                    return "";
                }
                return @"<tr style='font-weight:bold; vertical-align:middle;'>
                          <th>Ngày</th><th>Da cá sấu</th>
                                <th>Hủy (tấm/cái)</th>
                              <th>Ghi chú</th>
                             </tr>";
            }
            return "";
        }

        /*Must have for baocao coding template: create content of the table (just the tr and td of the table)*/
        public void createContent_Ngang(System.Text.StringBuilder sb)
        {
            if (ddlLoaiBC.SelectedValue == "TongHop")
            {
                if (countDCS == 0)
                {
                    return;
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>ĐK</td>");
                for (int i = 0; i < countDCS * 2; i++)
                {
                    sb.Append("<td></td>");
                }
                foreach (DataRow r in tblDCS.Rows)
                {
                    sb.Append("<td align='right'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()], scale) + "</td>");
                }
                sb.Append("<td></td></tr>");
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        int j = 0;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            if (r[rDCS["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(tong1[j], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(r[rDCS["IDVatTu"].ToString()], scale) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rDCS["IDVatTu"].ToString()]);
                            }
                            j++;
                        }
                        sb.Append("<td></td></tr>");
                    }
                }
                //Tính tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                for (int i = 0; i < countDCS * 3; i++)
                {
                    sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                }
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "Nhap")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }
                }

                //Tính tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                for (int i = 0; i < countDCS * 2; i++)
                {
                    sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                }
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "NhapBT")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }
                }

                //Tính tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                for (int i = 0; i < countDCS; i++)
                {
                    sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                }
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "NhapDC")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }
                }

                //Tính tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                for (int i = 0; i < countDCS; i++)
                {
                    sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                }
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "Xuat")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }
                }

                //Tính tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                for (int i = 0; i < countDCS * 3; i++)
                {
                    sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                }
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "XuatBT")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }
                }

                //Tính tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                for (int i = 0; i < countDCS; i++)
                {
                    sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                }
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "XuatDC")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }
                }

                //Tính tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                for (int i = 0; i < countDCS; i++)
                {
                    sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                }
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "XuatH")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                        }
                        sb.Append("<td></td></tr>");
                    }
                }

                //Tính tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
                for (int i = 0; i < countDCS; i++)
                {
                    sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
                }
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
        }

        public void createContent_Doc(System.Text.StringBuilder sb)
        {
            if (ddlLoaiBC.SelectedValue == "TongHop")
            {
                if (countDCS == 0)
                {
                    return;
                }
                bool DaCoDongDau = false;
                foreach (DataRow r in tblDCS.Rows)
                {
                    sb.Append("<tr>");
                    if (!DaCoDongDau)
                    {
                        sb.Append("<td align='center'>ĐK</td>");
                        DaCoDongDau = true;
                    }
                    else
                    {
                        sb.Append("<td></td>");
                    }
                    sb.Append("<td>" + r["TenVatTu"].ToString() + "</td><td></td><td></td>");
                    sb.Append("<td align='right'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()], scale) + "</td>");
                    sb.Append("<td></td></tr>");
                }
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        int j = 0;
                        DaCoDongDau = false;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            //Voi moi loai da, neu co xuat nhap thi viet 1 dong
                            if (r["Nhap" + rDCS["IDVatTu"].ToString()] == DBNull.Value && r["Xuat" + rDCS["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                j++;
                                continue;
                            }
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                            if (r[rDCS["IDVatTu"].ToString()] == DBNull.Value)
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(tong1[j], scale) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td align='right'>" + Config.ToXVal2(r[rDCS["IDVatTu"].ToString()], scale) + "</td>");
                                tong1[j] = Config.ToDecimal(r[rDCS["IDVatTu"].ToString()]);
                            }
                            j++;
                            sb.Append("<td></td></tr>");
                        }
                    }
                }
                //Viet dong cuoi day du
                if (dt.Rows.Count > 1)
                {
                    DaCoDongDau = false;
                    DataRow rE = dt.Rows[dt.Rows.Count - 1];
                    int jE = 0;
                    foreach (DataRow rDCS in tblDCS.Rows)
                    {
                        //Viet 1 dong
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countDCS], scale) + "</td>");
                        if (rE[rDCS["IDVatTu"].ToString()] == DBNull.Value)
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(tong1[jE], scale) + "</td>");
                        }
                        else
                        {
                            sb.Append("<td align='right'>" + Config.ToXVal2(rE[rDCS["IDVatTu"].ToString()], scale) + "</td>");
                            tong1[jE] = Config.ToDecimal(rE[rDCS["IDVatTu"].ToString()]);
                        }
                        jE++;
                        sb.Append("<td></td></tr>");
                    }
                }
                //Tính tổng
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                decimal tongNhap = 0;
                decimal tongXuat = 0;
                decimal tongTon = 0;
                for (int i = 0; i < countDCS; i++)
                {
                    tongNhap += (decimal)tong[i];
                    tongXuat += (decimal)tong[i + countDCS];
                    tongTon += (decimal)tong1[i];
                }
                sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
                sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
                sb.Append("<td align='right'>" + Config.ToXVal2(tongTon, scale) + "</td>");
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "Nhap")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        bool DaCoDongDau = false;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rDCS["IDVatTu"].ToString()] == DBNull.Value && r["NhapDC" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                }
                //Viet dong cuoi day du
                if (dt.Rows.Count > 1)
                {
                    bool DaCoDongDau = false;
                    int jE = 0;
                    foreach (DataRow rDCS in tblDCS.Rows)
                    {
                        //Viet 1 dong
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countDCS], scale) + "</td>");
                        jE++;
                        sb.Append("<td></td></tr>");
                    }
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                decimal tongNhap = 0;
                decimal tongNhapDC = 0;
                for (int i = 0; i < countDCS; i++)
                {
                    tongNhap += (decimal)tong[i];
                    tongNhapDC += (decimal)tong[i + countDCS];
                }
                sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
                sb.Append("<td align='right'>" + Config.ToXVal2(tongNhapDC, scale) + "</td>");
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "NhapBT")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        bool DaCoDongDau = false;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Nhap" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                }
                //Viet dong cuoi day du
                if (dt.Rows.Count > 1)
                {
                    bool DaCoDongDau = false;
                    int jE = 0;
                    foreach (DataRow rDCS in tblDCS.Rows)
                    {
                        //Viet 1 dong
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                        jE++;
                        sb.Append("<td></td></tr>");
                    }
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                decimal tongNhap = 0;
                for (int i = 0; i < countDCS; i++)
                {
                    tongNhap += (decimal)tong[i];
                }
                sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "NhapDC")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        bool DaCoDongDau = false;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["NhapDC" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                }
                //Viet dong cuoi day du
                if (dt.Rows.Count > 1)
                {
                    bool DaCoDongDau = false;
                    int jE = 0;
                    foreach (DataRow rDCS in tblDCS.Rows)
                    {
                        //Viet 1 dong
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                        jE++;
                        sb.Append("<td></td></tr>");
                    }
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                decimal tongNhapDC = 0;
                for (int i = 0; i < countDCS; i++)
                {
                    tongNhapDC += (decimal)tong[i];
                }
                sb.Append("<td align='right'>" + Config.ToXVal2(tongNhapDC, scale) + "</td>");
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "Xuat")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        bool DaCoDongDau = false;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rDCS["IDVatTu"].ToString()] == DBNull.Value && r["XuatDC" + rDCS["IDVatTu"].ToString()] == DBNull.Value && r["XuatH" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                }
                //Viet dong cuoi day du
                if (dt.Rows.Count > 1)
                {
                    bool DaCoDongDau = false;
                    int jE = 0;
                    foreach (DataRow rDCS in tblDCS.Rows)
                    {
                        //Viet 1 dong
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countDCS], scale) + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countDCS + countDCS], scale) + "</td>");
                        jE++;
                        sb.Append("<td></td></tr>");
                    }
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                decimal tongXuat = 0;
                decimal tongXuatDC = 0;
                decimal tongXuatH = 0;
                for (int i = 0; i < countDCS; i++)
                {
                    tongXuat += (decimal)tong[i];
                    tongXuatDC += (decimal)tong[i + countDCS];
                    tongXuatH += (decimal)tong[i + countDCS + countDCS];
                }
                sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
                sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatDC, scale) + "</td>");
                sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatH, scale) + "</td>");
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "XuatBT")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        bool DaCoDongDau = false;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["Xuat" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                }
                //Viet dong cuoi day du
                if (dt.Rows.Count > 1)
                {
                    bool DaCoDongDau = false;
                    int jE = 0;
                    foreach (DataRow rDCS in tblDCS.Rows)
                    {
                        //Viet 1 dong
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                        jE++;
                        sb.Append("<td></td></tr>");
                    }
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                decimal tongXuat = 0;
                for (int i = 0; i < countDCS; i++)
                {
                    tongXuat += (decimal)tong[i];
                }
                sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "XuatDC")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        bool DaCoDongDau = false;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatDC" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                }
                //Viet dong cuoi day du
                if (dt.Rows.Count > 1)
                {
                    bool DaCoDongDau = false;
                    int jE = 0;
                    foreach (DataRow rDCS in tblDCS.Rows)
                    {
                        //Viet 1 dong
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                        jE++;
                        sb.Append("<td></td></tr>");
                    }
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                decimal tongXuatDC = 0;
                for (int i = 0; i < countDCS; i++)
                {
                    tongXuatDC += (decimal)tong[i];
                }
                sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatDC, scale) + "</td>");
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
            else if (ddlLoaiBC.SelectedValue == "XuatH")
            {
                if (countDCS == 0)
                {
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    if (Convert.ToBoolean(r["HasValue"]))
                    {
                        bool DaCoDongDau = false;
                        foreach (DataRow rDCS in tblDCS.Rows)
                        {
                            //Voi moi loai da, neu co nhap thi viet 1 dong
                            if (r["XuatH" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
                            sb.Append("<tr>");
                            if (!DaCoDongDau)
                            {
                                sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
                                DaCoDongDau = true;
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                            sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
                            sb.Append("<td></td></tr>");
                        }
                    }
                }
                //Viet dong cuoi day du
                if (dt.Rows.Count > 1)
                {
                    bool DaCoDongDau = false;
                    int jE = 0;
                    foreach (DataRow rDCS in tblDCS.Rows)
                    {
                        //Viet 1 dong
                        sb.Append("<tr>");
                        if (!DaCoDongDau)
                        {
                            sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
                            DaCoDongDau = true;
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }
                        sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
                        jE++;
                        sb.Append("<td></td></tr>");
                    }
                }
                sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
                decimal tongXuatH = 0;
                for (int i = 0; i < countDCS; i++)
                {
                    tongXuatH += (decimal)tong[i];
                }
                sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatH, scale) + "</td>");
                sb.Append("<td></td></tr>");
                lblMessage.Text = "";
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (ddlSapXep.SelectedValue == "Doc") ExcelDoc();
            else ExcelNgang();
        }

        protected void ExcelNgang()
        {
            try
            {
                //Just call this
                Config.exportExcel(Response, sb, "XuatNhapDa", createDataAndTieuDe, createContent_Ngang, createTableHeader_Ngang);

                Response.Write(sb.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void ExcelDoc()
        {
            try
            {
                //Just call this
                Config.exportExcel(Response, sb, "XuatNhapDa", createDataAndTieuDe, createContent_Doc, createTableHeader_Doc);

                Response.Write(sb.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            if (ddlSapXep.SelectedValue == "Doc") ViewDoc();
            else ViewNgang();
        }

        protected void ViewNgang()
        {
            try
            {
                //Just call this
                Config.exportView(Response, sb, createDataAndTieuDe, createContent_Ngang, createTableHeader_Ngang);

                lt.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void ViewDoc()
        {
            try
            {
                //Just call this
                Config.exportView(Response, sb, createDataAndTieuDe, createContent_Doc, createTableHeader_Doc);

                lt.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

//        protected void ExcelNgang()
//        {
//            try
//            {
//                string filename = "DCS";
//                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
//                filename = filename.Replace("/", "_");
//                Response.ClearContent();
//                Response.ClearHeaders();
//                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
//                Response.ContentType = "application/vnd.ms-excel";
//                System.Text.StringBuilder sb = new System.Text.StringBuilder();
//                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");
//                if (ddlLoaiBC.SelectedValue == "TongHop")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_FilterDa";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau";
//                        param = new SqlParameter[3];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    //decimal[] tong = new decimal[countDCS * 3];
//                    ArrayList tong = new ArrayList();
//                    ArrayList tong1 = new ArrayList();
//                    for (int l = 0; l < countDCS * 3; l++) tong.Add(new decimal(0));
//                    for (int l1 = 0; l1 < countDCS; l1++) tong1.Add(new decimal(0));
//                    int tonIndex1 = countDCS * 2;
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
//                        tong1[tonIndex1 - (countDCS * 2)] = tong[tonIndex1];
//                        tonIndex1++;
//                    }
//                    for (int i = 1; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            if (r[rDCS["IDVatTu"].ToString()] != DBNull.Value)
//                            {
//                                tong[j] = Config.ToDecimal(r[rDCS["IDVatTu"].ToString()]);
//                            }
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0 && ((decimal)tong[k + countDCS + countDCS]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k + countDCS + countDCS);
//                            tong.RemoveAt(k + countDCS);
//                            tong.RemoveAt(k);
//                            tong1.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }
//                    //int countDCS = tblDCS.Rows.Count;

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                              <th rowspan=2>Ngày</th>
//                              <th colspan=" + countDCS.ToString() + @">Nhập (tấm/cái)</th>
//                              <th colspan=" + countDCS.ToString() + @">Xuất (tấm/cái)</th>
//                              <th colspan=" + countDCS.ToString() + @">Tồn (tấm/cái)</th>
//                              <th rowspan=2>Ghi chú</th>
//                             </tr>
//                             <tr style='font-weight:bold; vertical-align:middle;'>");
//                    for (int i = 0; i < 3; i++)
//                    {
//                        foreach (DataRow r in tblDCS.Rows)
//                        {
//                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                        }
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>ĐK</td>");
//                    for (int i = 0; i < countDCS * 2; i++)
//                    {
//                        sb.Append("<td></td>");
//                    }
//                    //decimal[] tong = new decimal[countDCS * 3];
//                    //int tonIndex = countDCS * 2;
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()], scale) + "</td>");
//                        //tong[tonIndex] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
//                        //tonIndex++;
//                    }
//                    sb.Append("<td></td></tr>");
//                    for (int i = 1; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        sb.Append("<tr>");
//                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                        //int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            //tong[j] += Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                            //j++;
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            //tong[j] += Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                            //j++;
//                        }
//                        //int j = countDCS * 2;
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            if (r[rDCS["IDVatTu"].ToString()] == DBNull.Value)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(tong1[j], scale) + "</td>");
//                            }
//                            else
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r[rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                tong1[j] = Config.ToDecimal(r[rDCS["IDVatTu"].ToString()]);
//                            }
//                            j++;
//                        }
//                        sb.Append("<td></td></tr>");
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;''><td>T.C</td>");
//                    for (int i = 0; i < countDCS * 3; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "Nhap")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS * 2; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k + countDCS);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Nhập (tấm/cái)</th>
//                          <th colspan=" + countDCS.ToString() + @">Nhập điều chỉnh (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    for (int i = 0; i < 2; i++)
//                    {
//                        foreach (DataRow r in tblDCS.Rows)
//                        {
//                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                        }
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        sb.Append("<tr>");
//                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                        }
//                        sb.Append("<td></td></tr>");
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS * 2; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "NhapBT")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", "Nhap");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Nhập (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        sb.Append("<tr>");
//                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                        }
//                        sb.Append("<td></td></tr>");
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "NhapDC")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", "Nhap");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Nhập điều chỉnh (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        sb.Append("<tr>");
//                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                        }
//                        sb.Append("<td></td></tr>");
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "Xuat")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    //decimal[] tong = new decimal[countDCS * 3];
//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS * 3; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0 && ((decimal)tong[k + countDCS + countDCS]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k + countDCS + countDCS);
//                            tong.RemoveAt(k + countDCS);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Xuất (tấm/cái)</th>
//                          <th colspan=" + countDCS.ToString() + @">Xuất điều chỉnh (tấm/cái)</th>
//                          <th colspan=" + countDCS.ToString() + @">Hủy (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    for (int i = 0; i < 3; i++)
//                    {
//                        foreach (DataRow r in tblDCS.Rows)
//                        {
//                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                        }
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        sb.Append("<tr>");
//                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                        }
//                        sb.Append("<td></td></tr>");
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS * 3; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "XuatBT")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", "Xuat");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Xuất (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        sb.Append("<tr>");
//                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                        }
//                        sb.Append("<td></td></tr>");
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "XuatDC")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", "Xuat");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Xuất điều chỉnh (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        sb.Append("<tr>");
//                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                        }
//                        sb.Append("<td></td></tr>");
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "XuatH")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI HỦY DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", "Xuat");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Hủy (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        sb.Append("<tr>");
//                        sb.Append(@"<td style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                        }
//                        sb.Append("<td></td></tr>");
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                sb.Append(Config.Footer(UserInfo.Profile.City, UserInfo.FirstName + " " + UserInfo.LastName));
//                sb.Append("</body></html>");
//                Response.Write(sb.ToString());
//            }
//            catch (Exception ex)
//            {
//                Response.Write(ex.Message);
//            }
//        }

//        protected void ExcelDoc()
//        {
//            try
//            {
//                string filename = "DCS";
//                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
//                filename = filename.Replace("/", "_");
//                Response.ClearContent();
//                Response.ClearHeaders();
//                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
//                Response.ContentType = "application/vnd.ms-excel";
//                System.Text.StringBuilder sb = new System.Text.StringBuilder();
//                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");
//                if (ddlLoaiBC.SelectedValue == "TongHop")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_FilterDa";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau";
//                        param = new SqlParameter[3];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    //decimal[] tong = new decimal[countDCS * 3];
//                    ArrayList tong = new ArrayList();
//                    ArrayList tong1 = new ArrayList();
//                    for (int l = 0; l < countDCS * 3; l++) tong.Add(new decimal(0));
//                    for (int l1 = 0; l1 < countDCS; l1++) tong1.Add(new decimal(0));
//                    int tonIndex1 = countDCS * 2;
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
//                        tong1[tonIndex1 - (countDCS * 2)] = tong[tonIndex1];
//                        tonIndex1++;
//                    }
//                    for (int i = 1; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            if (r[rDCS["IDVatTu"].ToString()] != DBNull.Value)
//                            {
//                                tong[j] = Config.ToDecimal(r[rDCS["IDVatTu"].ToString()]);
//                            }
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0 && ((decimal)tong[k + countDCS + countDCS]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k + countDCS + countDCS);
//                            tong.RemoveAt(k + countDCS);
//                            tong.RemoveAt(k);
//                            tong1.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }
//                    //int countDCS = tblDCS.Rows.Count;

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                              <th>Ngày</th><th>Da cá sấu</th>
//                              <th>Nhập (tấm/cái)</th>
//                              <th>Xuất (tấm/cái)</th>
//                              <th>Tồn (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    bool DaCoDongDau = false;
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<tr>");
//                        if (!DaCoDongDau)
//                        {
//                            sb.Append("<td align='center'>ĐK</td>");
//                            DaCoDongDau = true;
//                        }
//                        else
//                        {
//                            sb.Append("<td></td>");
//                        }
//                        sb.Append("<td style='text-align:left;'>" + r["TenVatTu"].ToString() + "</td><td></td><td></td>");
//                        sb.Append("<td align='right'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()], scale) + "</td>");
//                        sb.Append("<td></td></tr>");
//                    }
//                    for (int i = 1; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        DaCoDongDau = false;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Voi moi loai da, neu co xuat nhap thi viet 1 dong
//                            if (r["Nhap" + rDCS["IDVatTu"].ToString()] == DBNull.Value && r["Xuat" + rDCS["IDVatTu"].ToString()] == DBNull.Value)
//                            {
//                                j++;
//                                continue;
//                            }
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td style='text-align:left;'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            if (r[rDCS["IDVatTu"].ToString()] == DBNull.Value)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(tong1[j], scale) + "</td>");
//                            }
//                            else
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r[rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                tong1[j] = Config.ToDecimal(r[rDCS["IDVatTu"].ToString()]);
//                            }
//                            j++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        DaCoDongDau = false;
//                        DataRow rE = dt.Rows[dt.Rows.Count - 1];
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countDCS], scale) + "</td>");
//                            if (rE[rDCS["IDVatTu"].ToString()] == DBNull.Value)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(tong1[jE], scale) + "</td>");
//                            }
//                            else
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(rE[rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                tong1[jE] = Config.ToDecimal(rE[rDCS["IDVatTu"].ToString()]);
//                            }
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongNhap = 0;
//                    decimal tongXuat = 0;
//                    decimal tongTon = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongNhap += (decimal)tong[i];
//                        tongXuat += (decimal)tong[i + countDCS];
//                        tongTon += (decimal)tong1[i];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongTon, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "Nhap")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS * 2; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k + countDCS);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th>Ngày</th><th>Da cá sấu</th>
//                              <th>Nhập (tấm/cái)</th>
//                              <th>Nhập điều chỉnh (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        bool DaCoDongDau = false;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Voi moi loai da, neu co nhap thi viet 1 dong
//                            if (r["Nhap" + rDCS["IDVatTu"].ToString()] == DBNull.Value && r["NhapDC" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countDCS], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongNhap = 0;
//                    decimal tongNhapDC = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongNhap += (decimal)tong[i];
//                        tongNhapDC += (decimal)tong[i + countDCS];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhapDC, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "NhapBT")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", "Nhap");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th>Ngày</th><th>Da cá sấu</th>
//                              <th>Nhập (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        bool DaCoDongDau = false;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Voi moi loai da, neu co nhap thi viet 1 dong
//                            if (r["Nhap" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongNhap = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongNhap += (decimal)tong[i];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "NhapDC")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", "Nhap");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", "Nhap");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th>Ngày</th><th>Da cá sấu</th>
//                              <th>Nhập điều chỉnh (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        bool DaCoDongDau = false;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Voi moi loai da, neu co nhap thi viet 1 dong
//                            if (r["NhapDC" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongNhapDC = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongNhapDC += (decimal)tong[i];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhapDC, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "Xuat")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    //decimal[] tong = new decimal[countDCS * 3];
//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS * 3; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0 && ((decimal)tong[k + countDCS + countDCS]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k + countDCS + countDCS);
//                            tong.RemoveAt(k + countDCS);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th>Ngày</th><th>Da cá sấu</th>
//                              <th>Xuất (tấm/cái)</th>
//                                <th>Xuất điều chỉnh (tấm/cái)</th>
//                                <th>Hủy (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        bool DaCoDongDau = false;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Voi moi loai da, neu co nhap thi viet 1 dong
//                            if (r["Xuat" + rDCS["IDVatTu"].ToString()] == DBNull.Value && r["XuatDC" + rDCS["IDVatTu"].ToString()] == DBNull.Value && r["XuatH" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countDCS], scale) + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countDCS + countDCS], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongXuat = 0;
//                    decimal tongXuatDC = 0;
//                    decimal tongXuatH = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongXuat += (decimal)tong[i];
//                        tongXuatDC += (decimal)tong[i + countDCS];
//                        tongXuatH += (decimal)tong[i + countDCS + countDCS];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatDC, scale) + "</td>");
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatH, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "XuatBT")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", "Xuat");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th>Ngày</th><th>Da cá sấu</th>
//                              <th>Xuất (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        bool DaCoDongDau = false;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Voi moi loai da, neu co nhap thi viet 1 dong
//                            if (r["Xuat" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongXuat = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongXuat += (decimal)tong[i];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "XuatDC")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", "Xuat");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th>Ngày</th><th>Da cá sấu</th>
//                                <th>Xuất điều chỉnh (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        bool DaCoDongDau = false;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Voi moi loai da, neu co nhap thi viet 1 dong
//                            if (r["XuatDC" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongXuatDC = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongXuatDC += (decimal)tong[i];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatDC, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                else if (ddlLoaiBC.SelectedValue == "XuatH")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI HỦY DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon != "")
//                    {
//                        tblDCS = new DataTable();
//                        tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                        tblDCS.Columns.Add("TenVatTu", typeof(String));
//                        string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                        for (int j = 0; j < arrDa.Length; j++)
//                        {
//                            strDa += "@" + arrDa[j] + "@";
//                            DataRow r = tblDCS.NewRow();
//                            r["IDVatTu"] = int.Parse(arrDa[j]);
//                            r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                            tblDCS.Rows.Add(r);
//                        }
//                        countDCS = tblDCS.Rows.Count;
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                        param = new SqlParameter[5];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@StrDa", strDa);
//                        param[4] = new SqlParameter("@LoaiBC", "Xuat");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                    }
//                    else
//                    {
//                        strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet";
//                        param = new SqlParameter[4];
//                        param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                        param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                        DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                        param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                        param[3] = new SqlParameter("@LoaiBC", "Xuat");
//                        dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
//                        tblDCS = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='DCS' and Active=1 and Status > 0 order by MoTa Asc");
//                        countDCS = tblDCS.Rows.Count;
//                    }

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        int j = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rDCS["IDVatTu"].ToString()]);
//                            j++;
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        sb.Append("</body></html>");
//                        Response.Write(sb.ToString());
//                        return;
//                    }

//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table border='1'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th>Ngày</th><th>Da cá sấu</th>
//                                <th>Hủy (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        bool DaCoDongDau = false;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Voi moi loai da, neu co nhap thi viet 1 dong
//                            if (r["XuatH" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='mso-number-format:""dd\\/mm\\/yyyy"";'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongXuatH = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongXuatH += (decimal)tong[i];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatH, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                }
//                sb.Append(Config.Footer(UserInfo.Profile.City, UserInfo.FirstName + " " + UserInfo.LastName));
//                sb.Append("</body></html>");
//                Response.Write(sb.ToString());
//            }
//            catch (Exception ex)
//            {
//                Response.Write(ex.Message);
//            }
//        }

//        protected void ViewNgang()
//        {
//            try
//            {
//                System.Text.StringBuilder sb = new System.Text.StringBuilder();
//                if (ddlLoaiBC.SelectedValue == "TongHop")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;
                    
//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_FilterDa";
//                    param = new SqlParameter[4];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

//                    //decimal[] tong = new decimal[countDCS * 3];
//                    ArrayList tong = new ArrayList();
//                    ArrayList tong1 = new ArrayList();
//                    for (int l = 0; l < countDCS * 3; l++) tong.Add(new decimal(0));
//                    for (int l1 = 0; l1 < countDCS; l1++) tong1.Add(new decimal(0));
//                    int tonIndex1 = countDCS * 2;
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
//                        tong1[tonIndex1 - (countDCS * 2)] = tong[tonIndex1];
//                        tonIndex1++;
//                    }
//                    for (int i = 1; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                if (r[rDCS["IDVatTu"].ToString()] != DBNull.Value)
//                                {
//                                    tong[j] = Config.ToDecimal(r[rDCS["IDVatTu"].ToString()]);
//                                }
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0 && ((decimal)tong[k + countDCS + countDCS]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k + countDCS + countDCS);
//                            tong.RemoveAt(k + countDCS);
//                            tong.RemoveAt(k);
//                            tong1.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có xuất nhập tồn đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    //int countDCS = tblDCS.Rows.Count;
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                              <th rowspan=2>Ngày</th>
//                              <th colspan=" + countDCS.ToString() + @">Nhập (tấm/cái)</th>
//                              <th colspan=" + countDCS.ToString() + @">Xuất (tấm/cái)</th>
//                              <th colspan=" + countDCS.ToString() + @">Tồn (tấm/cái)</th>
//                              <th rowspan=2>Ghi chú</th>
//                             </tr>
//                             <tr style='font-weight:bold; vertical-align:middle;'>");
//                    for (int i = 0; i < 3; i++)
//                    {
//                        foreach (DataRow r in tblDCS.Rows)
//                        {
//                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                        }
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>ĐK</td>");
//                    for (int i = 0; i < countDCS * 2; i++)
//                    {
//                        sb.Append("<td></td>");
//                    }
//                    //decimal[] tong = new decimal[countDCS * 3];
//                    //int tonIndex = countDCS * 2;
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()], scale) + "</td>");
//                        //tong[tonIndex] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
//                        //tonIndex++;
//                    }
//                    sb.Append("<td></td></tr>");
//                    for (int i = 1; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            sb.Append("<tr>");
//                            sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                            //int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                //tong[j] += Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                                //j++;
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                //tong[j] += Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                                //j++;
//                            }
//                            //int j = countDCS * 2;
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                if (r[rDCS["IDVatTu"].ToString()] == DBNull.Value)
//                                {
//                                    sb.Append("<td align='right'>" + Config.ToXVal2(tong1[j], scale) + "</td>");
//                                }
//                                else
//                                {
//                                    sb.Append("<td align='right'>" + Config.ToXVal2(r[rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                    tong1[j] = Config.ToDecimal(r[rDCS["IDVatTu"].ToString()]);
//                                }
//                                j++;
//                            }
//                            sb.Append("<td></td></tr>");
//                        }
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS * 3; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "Nhap")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;

//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS * 2; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k + countDCS);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có nhập đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Nhập (tấm/cái)</th>
//                          <th colspan=" + countDCS.ToString() + @">Nhập điều chỉnh (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    for (int i = 0; i < 2; i++)
//                    {
//                        foreach (DataRow r in tblDCS.Rows)
//                        {
//                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                        }
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            sb.Append("<tr>");
//                            sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            }
//                            sb.Append("<td></td></tr>");
//                        }
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS * 2; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "NhapBT")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;
                    
//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", "Nhap");
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có nhập đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Nhập (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            sb.Append("<tr>");
//                            sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            }
//                            sb.Append("<td></td></tr>");
//                        }
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "NhapDC")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;
                    
//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", "Nhap");
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có nhập điều chỉnh đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Nhập điều chỉnh (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            sb.Append("<tr>");
//                            sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            }
//                            sb.Append("<td></td></tr>");
//                        }
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "Xuat")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;
                    
//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    
//                    //decimal[] tong = new decimal[countDCS * 3];
//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS * 3; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0 && ((decimal)tong[k + countDCS + countDCS]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k + countDCS + countDCS);
//                            tong.RemoveAt(k + countDCS);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có xuất đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Xuất (tấm/cái)</th>
//                          <th colspan=" + countDCS.ToString() + @">Xuất điều chỉnh (tấm/cái)</th>
//                          <th colspan=" + countDCS.ToString() + @">Hủy (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    for (int i = 0; i < 3; i++)
//                    {
//                        foreach (DataRow r in tblDCS.Rows)
//                        {
//                            sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                        }
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            sb.Append("<tr>");
//                            sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            }
//                            sb.Append("<td></td></tr>");
//                        }
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS * 3; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "XuatBT")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;
                    
//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", "Xuat");
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có xuất đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Xuất (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            sb.Append("<tr>");
//                            sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            }
//                            sb.Append("<td></td></tr>");
//                        }
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "XuatDC")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;
                    
//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", "Xuat");
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có xuất điều chỉnh đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Xuất điều chỉnh (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            sb.Append("<tr>");
//                            sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            }
//                            sb.Append("<td></td></tr>");
//                        }
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "XuatH")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI HỦY DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;
                    
//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", "Xuat");
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có hủy đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th rowspan=2>Ngày</th>
//                          <th colspan=" + countDCS.ToString() + @">Hủy (tấm/cái)</th>
//                          <th rowspan=2>Ghi chú</th>
//                         </tr>
//                         <tr style='font-weight:bold; vertical-align:middle;'>");
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<th>" + r["TenVatTu"].ToString() + "</th>");
//                    }
//                    sb.Append("</tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            sb.Append("<tr>");
//                            sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                            }
//                            sb.Append("<td></td></tr>");
//                        }
//                    }

//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td>T.C</td>");
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        sb.Append("<td align='right'>" + Config.ToXVal2(tong[i], scale) + "</td>");
//                    }
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//            }
//            catch (Exception ex)
//            {
//                Response.Write(ex.Message);
//            }
//        }

//        protected void ViewDoc()
//        {
//            try
//            {
//                System.Text.StringBuilder sb = new System.Text.StringBuilder();
//                if (ddlLoaiBC.SelectedValue == "TongHop")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;
                        
//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_FilterDa";
//                    param = new SqlParameter[4];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

//                    ArrayList tong = new ArrayList();
//                    ArrayList tong1 = new ArrayList();
//                    for (int l = 0; l < countDCS * 3; l++) tong.Add(new decimal(0));
//                    for (int l1 = 0; l1 < countDCS; l1++) tong1.Add(new decimal(0));
//                    int tonIndex1 = countDCS * 2;
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
//                        tong1[tonIndex1 - (countDCS * 2)] = tong[tonIndex1];
//                        tonIndex1++;
//                    }
//                    for (int i = 1; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                if (r[rDCS["IDVatTu"].ToString()] != DBNull.Value)
//                                {
//                                    tong[j] = Config.ToDecimal(r[rDCS["IDVatTu"].ToString()]);
//                                }
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0 && ((decimal)tong[k + countDCS + countDCS]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k + countDCS + countDCS);
//                            tong.RemoveAt(k + countDCS);
//                            tong.RemoveAt(k);
//                            tong1.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có xuất nhập tồn đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    //int countDCS = tblDCS.Rows.Count;
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                              <th>Ngày</th><th>Da cá sấu</th>
//                              <th>Nhập (tấm/cái)</th>
//                              <th>Xuất (tấm/cái)</th>
//                              <th>Tồn (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    bool DaCoDongDau = false;
//                    foreach (DataRow r in tblDCS.Rows)
//                    {
//                        sb.Append("<tr>");
//                        if (!DaCoDongDau)
//                        {
//                            sb.Append("<td align='center'>ĐK</td>");
//                            DaCoDongDau = true;
//                        }
//                        else
//                        {
//                            sb.Append("<td></td>");
//                        }
//                        sb.Append("<td>" + r["TenVatTu"].ToString() + "</td><td></td><td></td>");
//                        sb.Append("<td align='right'>" + Config.ToXVal2(dt.Rows[0][r["IDVatTu"].ToString()], scale) + "</td>");
//                        sb.Append("<td></td></tr>");
//                    }
//                    for (int i = 1; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            DaCoDongDau = false;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                //Voi moi loai da, neu co xuat nhap thi viet 1 dong
//                                if (r["Nhap" + rDCS["IDVatTu"].ToString()] == DBNull.Value && r["Xuat" + rDCS["IDVatTu"].ToString()] == DBNull.Value)
//                                {
//                                    j++;
//                                    continue;
//                                }
//                                sb.Append("<tr>");
//                                if (!DaCoDongDau)
//                                {
//                                    sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                    DaCoDongDau = true;
//                                }
//                                else
//                                {
//                                    sb.Append("<td></td>");
//                                }
//                                sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                if (r[rDCS["IDVatTu"].ToString()] == DBNull.Value)
//                                {
//                                    sb.Append("<td align='right'>" + Config.ToXVal2(tong1[j], scale) + "</td>");
//                                }
//                                else
//                                {
//                                    sb.Append("<td align='right'>" + Config.ToXVal2(r[rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                    tong1[j] = Config.ToDecimal(r[rDCS["IDVatTu"].ToString()]);
//                                }
//                                j++;
//                                sb.Append("<td></td></tr>");
//                            }
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        DaCoDongDau = false;
//                        DataRow rE = dt.Rows[dt.Rows.Count - 1];
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countDCS], scale) + "</td>");
//                            if (rE[rDCS["IDVatTu"].ToString()] == DBNull.Value)
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(tong1[jE], scale) + "</td>");
//                            }
//                            else
//                            {
//                                sb.Append("<td align='right'>" + Config.ToXVal2(rE[rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                tong1[jE] = Config.ToDecimal(rE[rDCS["IDVatTu"].ToString()]);
//                            }
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    //Tính tổng
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongNhap = 0;
//                    decimal tongXuat = 0;
//                    decimal tongTon = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongNhap += (decimal)tong[i];
//                        tongXuat += (decimal)tong[i + countDCS];
//                        tongTon += (decimal)tong1[i];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongTon, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "Nhap")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;

//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                    
//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS * 2; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k + countDCS);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có nhập đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                            <th>Ngày</th><th>Da cá sấu</th>
//                              <th>Nhập (tấm/cái)</th>
//                              <th>Nhập điều chỉnh (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            bool DaCoDongDau = false;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                //Voi moi loai da, neu co nhap thi viet 1 dong
//                                if (r["Nhap" + rDCS["IDVatTu"].ToString()] == DBNull.Value && r["NhapDC" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                                sb.Append("<tr>");
//                                if (!DaCoDongDau)
//                                {
//                                    sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                    DaCoDongDau = true;
//                                }
//                                else
//                                {
//                                    sb.Append("<td></td>");
//                                }
//                                sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                sb.Append("<td></td></tr>");
//                            }
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countDCS], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongNhap = 0;
//                    decimal tongNhapDC = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongNhap += (decimal)tong[i];
//                        tongNhapDC += (decimal)tong[i + countDCS];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhapDC, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "NhapBT")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI NHẬP DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;

//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", "Nhap");
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có nhập đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th>Ngày</th><th>Da cá sấu</th>
//                              <th>Nhập (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            bool DaCoDongDau = false;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                //Voi moi loai da, neu co nhap thi viet 1 dong
//                                if (r["Nhap" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                                sb.Append("<tr>");
//                                if (!DaCoDongDau)
//                                {
//                                    sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                    DaCoDongDau = true;
//                                }
//                                else
//                                {
//                                    sb.Append("<td></td>");
//                                }
//                                sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["Nhap" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                sb.Append("<td></td></tr>");
//                            }
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongNhap = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongNhap += (decimal)tong[i];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhap, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "NhapDC")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI NHẬP ĐIỀU CHỈNH DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;

//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", "Nhap");
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["NhapDC" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có nhập điều chỉnh đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th>Ngày</th><th>Da cá sấu</th>
//                              <th>Nhập điều chỉnh (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            bool DaCoDongDau = false;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                //Voi moi loai da, neu co nhap thi viet 1 dong
//                                if (r["NhapDC" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                                sb.Append("<tr>");
//                                if (!DaCoDongDau)
//                                {
//                                    sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                    DaCoDongDau = true;
//                                }
//                                else
//                                {
//                                    sb.Append("<td></td>");
//                                }
//                                sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["NhapDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                sb.Append("<td></td></tr>");
//                            }
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongNhapDC = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongNhapDC += (decimal)tong[i];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongNhapDC, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "Xuat")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;

//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", ddlLoaiBC.SelectedValue);
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);
                   
//                    //decimal[] tong = new decimal[countDCS * 3];
//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS * 3; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countDCS]) == 0 && ((decimal)tong[k + countDCS + countDCS]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k + countDCS + countDCS);
//                            tong.RemoveAt(k + countDCS);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có xuất đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                            <th>Ngày</th><th>Da cá sấu</th>
//                              <th>Xuất (tấm/cái)</th>
//                                <th>Xuất điều chỉnh (tấm/cái)</th>
//                                <th>Hủy (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            bool DaCoDongDau = false;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                //Voi moi loai da, neu co nhap thi viet 1 dong
//                                if (r["Xuat" + rDCS["IDVatTu"].ToString()] == DBNull.Value && r["XuatDC" + rDCS["IDVatTu"].ToString()] == DBNull.Value && r["XuatH" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                                sb.Append("<tr>");
//                                if (!DaCoDongDau)
//                                {
//                                    sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                    DaCoDongDau = true;
//                                }
//                                else
//                                {
//                                    sb.Append("<td></td>");
//                                }
//                                sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                sb.Append("<td></td></tr>");
//                            }
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countDCS], scale) + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE + countDCS + countDCS], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongXuat = 0;
//                    decimal tongXuatDC = 0;
//                    decimal tongXuatH = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongXuat += (decimal)tong[i];
//                        tongXuatDC += (decimal)tong[i + countDCS];
//                        tongXuatH += (decimal)tong[i + countDCS + countDCS];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatDC, scale) + "</td>");
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatH, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "XuatBT")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;

//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", "Xuat");
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có xuất đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th>Ngày</th><th>Da cá sấu</th>
//                              <th>Xuất (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            bool DaCoDongDau = false;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                //Voi moi loai da, neu co nhap thi viet 1 dong
//                                if (r["Xuat" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                                sb.Append("<tr>");
//                                if (!DaCoDongDau)
//                                {
//                                    sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                    DaCoDongDau = true;
//                                }
//                                else
//                                {
//                                    sb.Append("<td></td>");
//                                }
//                                sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["Xuat" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                sb.Append("<td></td></tr>");
//                            }
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongXuat = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongXuat += (decimal)tong[i];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuat, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "XuatDC")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI XUẤT ĐIỀU CHỈNH DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;

//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", "Xuat");
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatDC" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có xuất điều chỉnh đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th>Ngày</th><th>Da cá sấu</th>
//                                <th>Xuất điều chỉnh (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            bool DaCoDongDau = false;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                //Voi moi loai da, neu co nhap thi viet 1 dong
//                                if (r["XuatDC" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                                sb.Append("<tr>");
//                                if (!DaCoDongDau)
//                                {
//                                    sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                    DaCoDongDau = true;
//                                }
//                                else
//                                {
//                                    sb.Append("<td></td>");
//                                }
//                                sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatDC" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                sb.Append("<td></td></tr>");
//                            }
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongXuatDC = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongXuatDC += (decimal)tong[i];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatDC, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//                else if (ddlLoaiBC.SelectedValue == "XuatH")
//                {
//                    lblMessage.Text = "";
//                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
//                    string tieude = "";
//                    string strDa = "";
//                    string strSQL = "";
//                    SqlParameter[] param;
//                    if (txtFromDate.Text == "")
//                    {
//                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    if (txtToDate.Text == "")
//                    {
//                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
//                    }
//                    tieude += "<b>BẢNG THEO DÕI HỦY DA CÁ SẤU TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
//                    DataTable dt;
//                    DataTable tblDCS;
//                    int countDCS;
//                    string strDaChon = Config.GetSelectedValues(ddlDa) + Config.GetSelectedValues(ddlDaCB) + Config.GetSelectedValues(ddlDaCL) + Config.GetSelectedValues(ddlDaCBMoi) + Config.GetSelectedValues(ddlDaCLMoi) + Config.GetSelectedValues(ddlDaMDL) + Config.GetSelectedValues(ddlDauDaLung);
//                    if (strDaChon.StartsWith("0, ")) strDaChon = strDaChon.Substring(3); strDaChon = strDaChon.Replace(", 0", "");
//                    if (strDaChon == "") return;

//                    tblDCS = new DataTable();
//                    tblDCS.Columns.Add("IDVatTu", typeof(Int32));
//                    tblDCS.Columns.Add("TenVatTu", typeof(String));
//                    string[] arrDa = strDaChon.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
//                    for (int j = 0; j < arrDa.Length; j++)
//                    {
//                        strDa += "@" + arrDa[j] + "@";
//                        DataRow r = tblDCS.NewRow();
//                        r["IDVatTu"] = int.Parse(arrDa[j]);
//                        r["TenVatTu"] = dicDa[int.Parse(arrDa[j])];
//                        tblDCS.Rows.Add(r);
//                    }
//                    countDCS = tblDCS.Rows.Count;
//                    strSQL = "QLCS_BCTK_XuatNhapDaCaSau_ChiTiet_FilterDa";
//                    param = new SqlParameter[5];
//                    param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
//                    param[1] = new SqlParameter("@dTo", txtToDate.Text);
//                    DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
//                    param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
//                    param[3] = new SqlParameter("@StrDa", strDa);
//                    param[4] = new SqlParameter("@LoaiBC", "Xuat");
//                    dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

//                    ArrayList tong = new ArrayList();
//                    for (int l = 0; l < countDCS; l++) tong.Add(new decimal(0));
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            int j = 0;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["XuatH" + rDCS["IDVatTu"].ToString()]);
//                                j++;
//                            }
//                        }
//                    }
//                    for (int k = countDCS - 1; k >= 0; k--)
//                    {
//                        if (((decimal)tong[k]) == 0)
//                        {
//                            tblDCS.Rows.RemoveAt(k);
//                            tong.RemoveAt(k);
//                            countDCS--;
//                        }
//                    }
//                    if (countDCS == 0)
//                    {
//                        lblMessage.Text = "Không có hủy đối với da được chọn!";
//                        lt.Text = "";
//                        return;
//                    }
//                    sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
//                    sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
//                    sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>
//                          <th>Ngày</th><th>Da cá sấu</th>
//                                <th>Hủy (tấm/cái)</th>
//                              <th>Ghi chú</th>
//                             </tr></thead><tbody>");
//                    for (int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        DataRow r = dt.Rows[i];
//                        if (Convert.ToBoolean(r["HasValue"]))
//                        {
//                            bool DaCoDongDau = false;
//                            foreach (DataRow rDCS in tblDCS.Rows)
//                            {
//                                //Voi moi loai da, neu co nhap thi viet 1 dong
//                                if (r["XuatH" + rDCS["IDVatTu"].ToString()] == DBNull.Value) continue;
//                                sb.Append("<tr>");
//                                if (!DaCoDongDau)
//                                {
//                                    sb.Append("<td align='center'>" + ((DateTime)r["NgayBienDong1"]).ToString("dd/MM/yyyy") + "</td>");
//                                    DaCoDongDau = true;
//                                }
//                                else
//                                {
//                                    sb.Append("<td></td>");
//                                }
//                                sb.Append("<td>" + rDCS["TenVatTu"].ToString() + "</td>");
//                                sb.Append("<td align='right'>" + Config.ToXVal2(r["XuatH" + rDCS["IDVatTu"].ToString()], scale) + "</td>");
//                                sb.Append("<td></td></tr>");
//                            }
//                        }
//                    }
//                    //Viet dong cuoi day du
//                    if (dt.Rows.Count > 1)
//                    {
//                        bool DaCoDongDau = false;
//                        int jE = 0;
//                        foreach (DataRow rDCS in tblDCS.Rows)
//                        {
//                            //Viet 1 dong
//                            sb.Append("<tr>");
//                            if (!DaCoDongDau)
//                            {
//                                sb.Append(@"<td align='center' style='font-weight:bold;color:#F00;'>T.C</td>");
//                                DaCoDongDau = true;
//                            }
//                            else
//                            {
//                                sb.Append("<td></td>");
//                            }
//                            sb.Append("<td align='left'>" + rDCS["TenVatTu"].ToString() + "</td>");
//                            sb.Append("<td align='right'>" + Config.ToXVal2(tong[jE], scale) + "</td>");
//                            jE++;
//                            sb.Append("<td></td></tr>");
//                        }
//                    }
//                    sb.Append("<tr style='color: #FF0000;font-weight:bold; vertical-align:middle;'><td align='center'>Tổng cộng</td><td></td>");
//                    decimal tongXuatH = 0;
//                    for (int i = 0; i < countDCS; i++)
//                    {
//                        tongXuatH += (decimal)tong[i];
//                    }
//                    sb.Append("<td align='right'>" + Config.ToXVal2(tongXuatH, scale) + "</td>");
//                    sb.Append("<td></td></tr>");
//                    sb.Append("</tbody></table>");
//                    lt.Text = sb.ToString();
//                    lblMessage.Text = "";
//                }
//            }
//            catch (Exception ex)
//            {
//                Response.Write(ex.Message);
//            }
//        }

        protected void btnCol_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = "Col.xls";
                string strSQL = "";
                DataTable tblDCS;

                strSQL = "QLCS_VatTu_GetDCS";
                tblDCS = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><br/>");
                sb.Append("<table border='1'>");
                sb.Append(@"<tr style='font-weight:bold; vertical-align:middle;'>");
                //CB
                int MoTa = 106;
                for (int i = 6; i < 79; i = i + 6)
                {
                    DataRow r = tblDCS.Rows[i];
                    if (r["MoTa"].ToString() == MoTa.ToString())
                    {
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "(" + r["IDVatTu"].ToString() + ")</td>");
                    }
                    MoTa = MoTa + 7;
                }

                MoTa = 105;
                for (int i = 5; i < 79; i = i + 6)
                {
                    DataRow r = tblDCS.Rows[i];
                    if (r["MoTa"].ToString() == MoTa.ToString())
                    {
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "(" + r["IDVatTu"].ToString() + ")</td>");
                    }
                    MoTa = MoTa + 7;
                }

                MoTa = 104;
                for (int i = 4; i < 79; i = i + 6)
                {
                    DataRow r = tblDCS.Rows[i];
                    if (r["MoTa"].ToString() == MoTa.ToString())
                    {
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "(" + r["IDVatTu"].ToString() + ")</td>");
                    }
                    MoTa = MoTa + 7;
                }

                MoTa = 103;
                for (int i = 3; i < 79; i = i + 6)
                {
                    DataRow r = tblDCS.Rows[i];
                    if (r["MoTa"].ToString() == MoTa.ToString())
                    {
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "(" + r["IDVatTu"].ToString() + ")</td>");
                    }
                    MoTa = MoTa + 7;
                }

                MoTa = 102;
                for (int i = 2; i < 79; i = i + 6)
                {
                    DataRow r = tblDCS.Rows[i];
                    if (r["MoTa"].ToString() == MoTa.ToString())
                    {
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "(" + r["IDVatTu"].ToString() + ")</td>");
                    }
                    MoTa = MoTa + 7;
                }

                MoTa = 101;
                for (int i = 1; i < 79; i = i + 6)
                {
                    DataRow r = tblDCS.Rows[i];
                    if (r["MoTa"].ToString() == MoTa.ToString())
                    {
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "(" + r["IDVatTu"].ToString() + ")</td>");
                    }
                    MoTa = MoTa + 7;
                }

                //CL
                MoTa = 206;
                for (int i = 85; i < 158; i = i + 6)
                {
                    DataRow r = tblDCS.Rows[i];
                    if (r["MoTa"].ToString() == MoTa.ToString())
                    {
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "(" + r["IDVatTu"].ToString() + ")</td>");
                    }
                    MoTa = MoTa + 7;
                }

                MoTa = 205;
                for (int i = 84; i < 158; i = i + 6)
                {
                    DataRow r = tblDCS.Rows[i];
                    if (r["MoTa"].ToString() == MoTa.ToString())
                    {
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "(" + r["IDVatTu"].ToString() + ")</td>");
                    }
                    MoTa = MoTa + 7;
                }

                MoTa = 204;
                for (int i = 83; i < 158; i = i + 6)
                {
                    DataRow r = tblDCS.Rows[i];
                    if (r["MoTa"].ToString() == MoTa.ToString())
                    {
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "(" + r["IDVatTu"].ToString() + ")</td>");
                    }
                    MoTa = MoTa + 7;
                }

                MoTa = 203;
                for (int i = 82; i < 158; i = i + 6)
                {
                    DataRow r = tblDCS.Rows[i];
                    if (r["MoTa"].ToString() == MoTa.ToString())
                    {
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "(" + r["IDVatTu"].ToString() + ")</td>");
                    }
                    MoTa = MoTa + 7;
                }

                MoTa = 202;
                for (int i = 81; i < 158; i = i + 6)
                {
                    DataRow r = tblDCS.Rows[i];
                    if (r["MoTa"].ToString() == MoTa.ToString())
                    {
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "(" + r["IDVatTu"].ToString() + ")</td>");
                    }
                    MoTa = MoTa + 7;
                }

                MoTa = 201;
                for (int i = 80; i < 158; i = i + 6)
                {
                    DataRow r = tblDCS.Rows[i];
                    if (r["MoTa"].ToString() == MoTa.ToString())
                    {
                        sb.Append("<td>" + r["TenVatTu"].ToString() + "(" + r["IDVatTu"].ToString() + ")</td>");
                    }
                    MoTa = MoTa + 7;
                }

                sb.Append("</tr>");
                sb.Append("</table></body></html>");
                Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}