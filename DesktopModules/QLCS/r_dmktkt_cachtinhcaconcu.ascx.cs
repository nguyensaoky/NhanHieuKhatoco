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
    public partial class r_dmktkt_cachtinhcaconcu : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private const string TABLE_NAME = "THEPAGE";
        private const string ProviderType = "data";

        private string strConn = DotNetNuke.Common.Utilities.Config.GetConnectionString();
        private string objectQualifier;
        private string databaseOwner;

        private string GetFullyQualifiedName(string name)
        {
            ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
            Provider objProvider = (Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider];
            objectQualifier = objProvider.Attributes["objectQualifier"];
            if (!String.IsNullOrEmpty(objectQualifier) && objectQualifier.EndsWith("_") == false)
            {
                objectQualifier += "_";
            }
            databaseOwner = objProvider.Attributes["databaseOwner"];
            if (!String.IsNullOrEmpty(databaseOwner) && databaseOwner.EndsWith(".") == false)
            {
                databaseOwner += ".";
            }
            return databaseOwner + objectQualifier + name;
        }

        private string getConnectionString()
        {
            return DotNetNuke.Common.Utilities.Config.GetConnectionString();
        }

        private void BindControls()
        {
            for (int i = 2010; i < DateTime.Now.Year + 1; i++)
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

        //protected void btnExcel_1Nam_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        //        string filename = "DMKTKT";
        //        string tieude = "";
        //        string strSQL = "QLCS_BCTK_DMKTKT";
        //        SqlParameter[] param = new SqlParameter[1];
        //        param[0] = new SqlParameter("@NamBC", int.Parse(txtYear.Text));
        //        filename += txtYear.Text + ".xls";
        //        tieude += "<b>BÁO CÁO CÁC CHỈ TIÊU ĐỊNH MỨC KINH TẾ KỸ THUẬT</b>";
        //        DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, GetFullyQualifiedName(strSQL), param);

        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
        //        Response.ContentType = "application/vnd.ms-excel";
        //        string s = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>";
        //        s += "<table border='1'><tr><td colspan='4'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/></td></tr><tr><td><b>Nội dung</b></td><td><b>ĐVT</b></td><td><b>Định mức</b></td><td><b>Thực tế</b></td></tr>";
        //        for (int i = 0; i < ds.Tables.Count; i++)
        //        {
        //            DataTable dt = ds.Tables[i];
        //            if (i == 0)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu con (0 – 1 năm tuổi)</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Khối lượng sơ sinh</td><td>kg</td><td>0.08 - 0.09</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["KhoiLuongTrungBinhCaCon"]) / 1000, 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>1.50 - 2.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["KhoiLuongCuoiKy"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td></td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ nuôi úm (0-30 ngày tuổi)</td><td>%</td><td>90 - 95</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSong1_30"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ 1-12 tháng tuổi</td><td>%</td><td>85 - 90</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSong1_12"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td></td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ nuôi úm (0-30 ngày tuổi)</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ 1-12 tháng tuổi</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>4.93 - 4.71</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnKgTangTrong"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>7.00 - 9.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanCaConMotNam"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 1)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu sinh trưởng 1: 13-24 tháng tuổi</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>7.00 - 9.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["KhoiLuongCuoiKy"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>90 - 95</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongST1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>5.45 - 5.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnKgTangTrong"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>30.00 - 35.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoan"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Giống</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanGiong"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Tăng trọng</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanTT"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 2)
        //            {
        //                double a = Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongST2"]), 2);
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu sinh trưởng 2: 25-36 tháng tuổi</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>20.00 - 25.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["KhoiLuongCuoiKy"]), 2).ToString(ci) + "</td></tr>";
        //                //s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>95 - 97</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongST2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>95 - 97</td><td>" + a.ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>5.00 - 4.69</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnKgTangTrong"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>65.00 - 75.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoan"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Giống</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanGiong"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Tăng trọng</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanTT"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 3)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu hậu bị 4-5 năm tuổi</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>kg</td><td>30.00 - 35.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["KhoiLuongCuoiKyHB1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>kg</td><td>40.00 - 45.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["KhoiLuongCuoiKyHB2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongHB"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongHB1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongHB2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>kg</td><td>9.00 - 10.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnKgTangTrongHB1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>kg</td><td>10.00 - 11.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnKgTangTrongHB2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanHB"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanHB1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanHB2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 4)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu sinh sản 1: 6-7 năm tuổi</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongSS1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 -130</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoan"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeCaiSS1ThamGiaDe"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>10.00 - 15.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["SanLuongTrungSS1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>65 - 75</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungChonApSS1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>50 - 60</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungCoPhoiSS1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>60 - 65</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungNoSS1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 5)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu sinh sản 2: 8-9 năm tuổi</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongSS2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoan"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeCaiSS2ThamGiaDe"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>20.00 - 25.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["SanLuongTrungSS2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>85 - 90</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungChonApSS2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>75 - 80</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungCoPhoiSS2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>70 - 80</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungNoSS2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 6)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu TSCĐ: > 10 năm tuổi</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongTSCD"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoan"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeCaiTSCDThamGiaDe"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>30.00 - 40.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["SanLuongTrungTSCD"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>95 - 98</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungChonApTSCD"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>95 - 98</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungCoPhoiTSCD"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>75 - 85</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungNoTSCD"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 7)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Cá sấu nước mặn</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongNM"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoan"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeCaiNMThamGiaDe"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>30.00 - 40.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["SanLuongTrungNM"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>95 - 98</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungChonApNM"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>95 - 98</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungCoPhoiNM"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>75 - 85</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungNoNM"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>";
        //            }
        //        }
        //        s += "</table>";
        //        s += "</body></html>";
        //        Response.Write(s);
        //    }

        //    catch (Exception ex)
        //    {
        //        Response.Write(ex.Message);
        //    }
        //}

        //protected void btnView_1Nam_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        //        string tieude = "";
        //        string strSQL = "QLCS_BCTK_DMKTKT";
        //        SqlParameter[] param = new SqlParameter[1];
        //        param[0] = new SqlParameter("@NamBC", int.Parse(txtYear.Text));
        //        tieude += "<b>BÁO CÁO CÁC CHỈ TIÊU ĐỊNH MỨC KINH TẾ KỸ THUẬT</b>";
        //        DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, GetFullyQualifiedName(strSQL), param);

        //        string s = "<table border='1' class='mGrid'><tr><td colspan='4'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/></td></tr><tr><td><b>Nội dung</b></td><td><b>ĐVT</b></td><td><b>Định mức</b></td><td><b>Thực tế</b></td></tr>";
        //        for (int i = 0; i < ds.Tables.Count; i++)
        //        {
        //            DataTable dt = ds.Tables[i];
        //            if (i == 0)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu con (0 – 1 năm tuổi)</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Khối lượng sơ sinh</td><td>kg</td><td>0.08 - 0.09</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["KhoiLuongTrungBinhCaCon"]) / 1000, 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>1.50 - 2.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["KhoiLuongCuoiKy"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td></td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ nuôi úm (0-30 ngày tuổi)</td><td>%</td><td>90 - 95</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSong1_30"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ 1-12 tháng tuổi</td><td>%</td><td>85 - 90</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSong1_12"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td></td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ nuôi úm (0-30 ngày tuổi)</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ 1-12 tháng tuổi</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>4.93 - 4.71</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnKgTangTrong"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>7.00 - 9.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanCaConMotNam"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 1)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu sinh trưởng 1: 13-24 tháng tuổi</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>7.00 - 9.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["KhoiLuongCuoiKy"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>90 - 95</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongST1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>5.45 - 5.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnKgTangTrong"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>30.00 - 35.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoan"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Giống</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanGiong"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Tăng trọng</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanTT"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 2)
        //            {
        //                double a = Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongST2"]), 2);
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu sinh trưởng 2: 25-36 tháng tuổi</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>20.00 - 25.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["KhoiLuongCuoiKy"]), 2).ToString(ci) + "</td></tr>";
        //                //s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>95 - 97</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongST2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>95 - 97</td><td>" + a.ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>5.00 - 4.69</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnKgTangTrong"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>65.00 - 75.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoan"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Giống</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanGiong"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Tăng trọng</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanTT"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 3)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu hậu bị 4-5 năm tuổi</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>kg</td><td>30.00 - 35.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["KhoiLuongCuoiKyHB1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>kg</td><td>40.00 - 45.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["KhoiLuongCuoiKyHB2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongHB"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongHB1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongHB2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>kg</td><td>9.00 - 10.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnKgTangTrongHB1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>kg</td><td>10.00 - 11.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnKgTangTrongHB2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanHB"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanHB1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>kg/con</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoanHB2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 4)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu sinh sản 1: 6-7 năm tuổi</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongSS1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 -130</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoan"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeCaiSS1ThamGiaDe"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>10.00 - 15.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["SanLuongTrungSS1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>65 - 75</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungChonApSS1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>50 - 60</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungCoPhoiSS1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>60 - 65</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungNoSS1"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 5)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu sinh sản 2: 8-9 năm tuổi</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongSS2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoan"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeCaiSS2ThamGiaDe"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>20.00 - 25.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["SanLuongTrungSS2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>85 - 90</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungChonApSS2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>75 - 80</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungCoPhoiSS2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>70 - 80</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungNoSS2"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 6)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Giai đoạn nuôi cá sấu TSCĐ: > 10 năm tuổi</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongTSCD"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoan"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeCaiTSCDThamGiaDe"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>30.00 - 40.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["SanLuongTrungTSCD"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>95 - 98</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungChonApTSCD"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>95 - 98</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungCoPhoiTSCD"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>75 - 85</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungNoTSCD"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>";
        //            }
        //            else if (i == 7)
        //            {
        //                s += @"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='4'>Cá sấu nước mặn</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeNuoiSongNM"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TieuTonThucAnToanGiaiDoan"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeCaiNMThamGiaDe"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>30.00 - 40.00</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["SanLuongTrungNM"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>95 - 98</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungChonApNM"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>95 - 98</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungCoPhoiNM"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>75 - 85</td><td>" + Math.Round(Config.ToDouble(dt.Rows[0]["TyLeTrungNoNM"]), 2).ToString(ci) + "</td></tr>";
        //                s += @"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>";
        //            }
        //        }
        //        s += "</table>";
        //        lt.Text = s;
        //    }

        //    catch (Exception ex)
        //    {
        //        Response.Write(ex.Message);
        //    }
        //}

        private string GetValues(DataTable[] dt, string col, int sochia, int round, System.Globalization.CultureInfo ci)
        {
            string res = "";
            for (int l = 0; l < dt.Length; l++)
            {
                res += "<td style='text-align:right;'>" + Math.Round(Config.ToDouble(dt[l].Rows[0][col]) / sochia, round).ToString(ci) + "</td>";
            }
            return res;
        }

        private ArrayList GetSelectedItems(ListBox l)
        {
            ArrayList al = new ArrayList();
            foreach (ListItem i in l.Items)
            {
                if (i.Selected) al.Add(i);
            }
            return al;
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList l = GetSelectedItems(lstYear);
                int SoNam = l.Count;
                int TongSoCot = 3 + SoNam;
                int TongSoCotPhu = 1 + SoNam;
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string strSQL = "QLCS_BCTK_DMKTKT";
                DataSet[] ds = new DataSet[SoNam];
                SqlParameter[] param = new SqlParameter[1];
                int j = 0;
                foreach (ListItem i in l)
                {
                    param[0] = new SqlParameter("@NamBC", int.Parse(i.Value));
                    ds[j] = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, GetFullyQualifiedName(strSQL), param);
                    j++;
                }

                string filename = "DMKTKT.xls";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'>");
                sb.Append("<table border='1' class='mGrid'><tr><td colspan='" + TongSoCot.ToString() + "'><center style='font-weight:bold;font-size:15pt;'><b>BÁO CÁO CÁC CHỈ TIÊU ĐỊNH MỨC KINH TẾ KỸ THUẬT</b></center><br/></td></tr><tr><td><b>Nội dung</b></td><td><b>ĐVT</b></td><td><b>Định mức</b></td>");
                foreach (ListItem i in l)
                {
                    sb.Append("<td><b>" + i.Value + "</b></td>");
                }
                sb.Append("</tr>");
                for (int i = 0; i < ds[0].Tables.Count; i++)
                {
                    DataTable[] dt = new DataTable[SoNam];
                    for (int k = 0; k < SoNam; k++)
                    {
                        dt[k] = ds[k].Tables[i];
                    }
                    if (i == 0)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu con (0 – 1 năm tuổi)</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng sơ sinh</td><td>kg</td><td>0.08 - 0.09</td>" + GetValues(dt, "KhoiLuongTrungBinhCaCon", 1000, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>1.50 - 2.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Tỷ lệ nuôi sống</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ nuôi úm (0-30 ngày tuổi)</td><td>%</td><td>90 - 95</td>" + GetValues(dt, "TyLeNuoiSong1_30", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ 1-12 tháng tuổi</td><td>%</td><td>85 - 90</td>" + GetValues(dt, "TyLeNuoiSong1_12", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td></td><td></td><td></td></tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ nuôi úm (0-30 ngày tuổi)</td><td>%</td><td></td><td></td></tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ 1-12 tháng tuổi</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>4.93 - 4.71</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>7.00 - 9.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanCaConMotNam", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>");
                    }
                    else if (i == 1)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh trưởng 1: 13-24 tháng tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>7.00 - 9.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>90 - 95</td>" + GetValues(dt, "TyLeNuoiSongST1", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>5.45 - 5.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>30.00 - 35.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Giống</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanGiong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Tăng trọng</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanTT", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>");
                    }
                    else if (i == 2)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh trưởng 2: 25-36 tháng tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>20.00 - 25.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>95 - 97</td>" + GetValues(dt, "TyLeNuoiSongST2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>5.00 - 4.69</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>65.00 - 75.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Giống</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanGiong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Tăng trọng</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanTT", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>");
                    }
                    else if (i == 3)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu hậu bị 4-5 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td colspan='" + TongSoCotPhu.ToString() + "'></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>kg</td><td>30.00 - 35.00</td>" + GetValues(dt, "KhoiLuongCuoiKyHB1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>kg</td><td>40.00 - 45.00</td>" + GetValues(dt, "KhoiLuongCuoiKyHB2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td></td>" + GetValues(dt, "TyLeNuoiSongHB", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>%</td><td></td>" + GetValues(dt, "TyLeNuoiSongHB1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>%</td><td></td>" + GetValues(dt, "TyLeNuoiSongHB2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td colspan='" + TongSoCotPhu.ToString() + "'></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>kg</td><td>9.00 - 10.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrongHB1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>kg</td><td>10.00 - 11.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrongHB2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanHB", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanHB1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanHB2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>");
                    }
                    else if (i == 4)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh sản 1: 6-7 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongSS1", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 -130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiSS1ThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>10.00 - 15.00</td>" + GetValues(dt, "SanLuongTrungSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>65 - 75</td>" + GetValues(dt, "TyLeTrungChonApSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>50 - 60</td>" + GetValues(dt, "TyLeTrungCoPhoiSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>60 - 65</td>" + GetValues(dt, "TyLeTrungNoSS1", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>");
                    }
                    else if (i == 5)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh sản 2: 8-9 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongSS2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiSS2ThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>20.00 - 25.00</td>" + GetValues(dt, "SanLuongTrungSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>85 - 90</td>" + GetValues(dt, "TyLeTrungChonApSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>75 - 80</td>" + GetValues(dt, "TyLeTrungCoPhoiSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>70 - 80</td>" + GetValues(dt, "TyLeTrungNoSS2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>");
                    }
                    else if (i == 6)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu TSCĐ: > 10 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongTSCD", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiTSCDThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>30.00 - 40.00</td>" + GetValues(dt, "SanLuongTrungTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>95 - 98</td>" + GetValues(dt, "TyLeTrungChonApTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>95 - 98</td>" + GetValues(dt, "TyLeTrungCoPhoiTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>75 - 85</td>" + GetValues(dt, "TyLeTrungNoTSCD", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>");
                    }
                    else if (i == 7)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Cá sấu nước mặn</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongNM", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiNMThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>30.00 - 40.00</td>" + GetValues(dt, "SanLuongTrungNM", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>95 - 98</td>" + GetValues(dt, "TyLeTrungChonApNM", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>95 - 98</td>" + GetValues(dt, "TyLeTrungCoPhoiNM", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>75 - 85</td>" + GetValues(dt, "TyLeTrungNoNM", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>");
                    }
                }
                sb.Append("</table>");
                sb.Append(Config.Footer(UserInfo.Profile.City, UserInfo.FirstName + " " + UserInfo.LastName));
                sb.Append("</body></html>");
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
                ArrayList l = GetSelectedItems(lstYear);
                int SoNam = l.Count;
                int TongSoCot = 3 + SoNam;
                int TongSoCotPhu = 1 + SoNam;
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string strSQL = "QLCS_BCTK_DMKTKT";
                DataSet[] ds = new DataSet[SoNam];
                SqlParameter[] param = new SqlParameter[1];
                int j = 0;
                foreach (ListItem i in l)
                {
                    param[0] = new SqlParameter("@NamBC", int.Parse(i.Value));
                    ds[j] = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, GetFullyQualifiedName(strSQL), param);
                    j++;
                }
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<table border='1' class='mGrid'><tr><td colspan='" + TongSoCot.ToString() + "'><br/><center style='font-weight:bold;font-size:15pt;'><b>BÁO CÁO CÁC CHỈ TIÊU ĐỊNH MỨC KINH TẾ KỸ THUẬT</b></center><br/></td></tr><tr><td style='text-align:center;'><b>Nội dung</b></td><td style='text-align:center;'><b>ĐVT</b></td><td style='text-align:center;'><b>Định mức</b></td>");
                foreach (ListItem i in l)
                {
                    sb.Append("<td style='text-align:center;'><b>" + i.Value + "</b></td>");
                }
                sb.Append("</tr>");
                for (int i = 0; i < ds[0].Tables.Count; i++)
                {
                    DataTable[] dt = new DataTable[SoNam];
                    for (int k = 0; k < SoNam; k++)
                    {
                        dt[k] = ds[k].Tables[i];
                    }
                    if (i == 0)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu con (0 – 1 năm tuổi)</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng sơ sinh</td><td>kg</td><td style='text-align:center;'>0.08 - 0.09</td>" + GetValues(dt, "KhoiLuongTrungBinhCaCon", 1000, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td style='text-align:center;'>1.50 - 2.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Tỷ lệ nuôi sống</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ nuôi úm (0-30 ngày tuổi)</td><td>%</td><td style='text-align:center;'>90 - 95</td>" + GetValues(dt, "TyLeNuoiSong1_30", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ 1-12 tháng tuổi</td><td>%</td><td style='text-align:center;'>85 - 90</td>" + GetValues(dt, "TyLeNuoiSong1_12", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td></td><td></td><td></td></tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ nuôi úm (0-30 ngày tuổi)</td><td>%</td><td></td><td></td></tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ GĐ 1-12 tháng tuổi</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td style='text-align:center;'>4.93 - 4.71</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>7.00 - 9.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanCaConMotNam", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>");
                    }
                    else if (i == 1)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh trưởng 1: 13-24 tháng tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td style='text-align:center;'>7.00 - 9.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>90 - 95</td>" + GetValues(dt, "TyLeNuoiSongST1", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td style='text-align:center;'>5.45 - 5.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>30.00 - 35.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Giống</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanGiong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Tăng trọng</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanTT", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>");
                    }
                    else if (i == 2)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh trưởng 2: 25-36 tháng tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td style='text-align:center;'>20.00 - 25.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>95 - 97</td>" + GetValues(dt, "TyLeNuoiSongST2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td style='text-align:center;'>5.00 - 4.69</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>65.00 - 75.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Giống</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanGiong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ Tăng trọng</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanTT", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>");
                    }
                    else if (i == 3)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu hậu bị 4-5 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td colspan='" + TongSoCotPhu.ToString() + "'></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>kg</td><td style='text-align:center;'>30.00 - 35.00</td>" + GetValues(dt, "KhoiLuongCuoiKyHB1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>kg</td><td style='text-align:center;'>40.00 - 45.00</td>" + GetValues(dt, "KhoiLuongCuoiKyHB2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td></td>" + GetValues(dt, "TyLeNuoiSongHB", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>%</td><td></td>" + GetValues(dt, "TyLeNuoiSongHB1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>%</td><td></td>" + GetValues(dt, "TyLeNuoiSongHB2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td colspan='" + TongSoCotPhu.ToString() + "'></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>kg</td><td style='text-align:center;'>9.00 - 10.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrongHB1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>kg</td><td style='text-align:center;'>10.00 - 11.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrongHB2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanHB", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 4 tuổi</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanHB1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>&nbsp;+ 5 tuổi</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanHB2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/kg tăng trọng</td><td>đ/kg</td><td></td><td></td></tr>");
                    }
                    else if (i == 4)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh sản 1: 6-7 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongSS1", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>120 -130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiSS1ThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td style='text-align:center;'>10.00 - 15.00</td>" + GetValues(dt, "SanLuongTrungSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td style='text-align:center;'>65 - 75</td>" + GetValues(dt, "TyLeTrungChonApSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td style='text-align:center;'>50 - 60</td>" + GetValues(dt, "TyLeTrungCoPhoiSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td style='text-align:center;'>60 - 65</td>" + GetValues(dt, "TyLeTrungNoSS1", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>");
                    }
                    else if (i == 5)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh sản 2: 8-9 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongSS2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiSS2ThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td style='text-align:center;'>20.00 - 25.00</td>" + GetValues(dt, "SanLuongTrungSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td style='text-align:center;'>85 - 90</td>" + GetValues(dt, "TyLeTrungChonApSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td style='text-align:center;'>75 - 80</td>" + GetValues(dt, "TyLeTrungCoPhoiSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td style='text-align:center;'>70 - 80</td>" + GetValues(dt, "TyLeTrungNoSS2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>");
                    }
                    else if (i == 6)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu TSCĐ: > 10 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongTSCD", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiTSCDThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td style='text-align:center;'>30.00 - 40.00</td>" + GetValues(dt, "SanLuongTrungTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td style='text-align:center;'>95 - 98</td>" + GetValues(dt, "TyLeTrungChonApTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td style='text-align:center;'>95 - 98</td>" + GetValues(dt, "TyLeTrungCoPhoiTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td style='text-align:center;'>75 - 85</td>" + GetValues(dt, "TyLeTrungNoTSCD", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>");
                    }
                    else if (i == 7)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Cá sấu nước mặn</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongNM", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td><td></td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiNMThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td style='text-align:center;'>30.00 - 40.00</td>" + GetValues(dt, "SanLuongTrungNM", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td style='text-align:center;'>95 - 98</td>" + GetValues(dt, "TyLeTrungChonApNM", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td style='text-align:center;'>95 - 98</td>" + GetValues(dt, "TyLeTrungCoPhoiNM", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td style='text-align:center;'>75 - 85</td>" + GetValues(dt, "TyLeTrungNoNM", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Chi phí thức ăn/trứng có phôi</td><td>đ/trứng</td><td></td><td></td></tr>");
                    }
                }
                sb.Append("</table>");
                lt.Text = sb.ToString();
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}