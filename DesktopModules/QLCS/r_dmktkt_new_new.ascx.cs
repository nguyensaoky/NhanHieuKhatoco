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
    public partial class r_dmktkt_new_new : DotNetNuke.Entities.Modules.PortalModuleBase
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
                string strSQL = "";
                if (ddlChuan.SelectedValue == "1" && ddlCachTinh.SelectedValue == "1")
                {
                    strSQL = "QLCS_BCTK_DMKTKT_new_new_new_ChuanCu_SP";
                }
                else if (ddlChuan.SelectedValue == "1" && ddlCachTinh.SelectedValue == "2")
                {
                    strSQL = "QLCS_BCTK_DMKTKT_new_new_new_NgayAnCu_ChuanCu";
                }
                else if (ddlChuan.SelectedValue == "2" && ddlCachTinh.SelectedValue == "1")
                {
                    strSQL = "QLCS_BCTK_DMKTKT_new_new_new_SP";
                }
                else if (ddlChuan.SelectedValue == "2" && ddlCachTinh.SelectedValue == "2")
                {
                    strSQL = "QLCS_BCTK_DMKTKT_new_new_new_NgayAnCu";
                }
                DataSet[] ds = new DataSet[SoNam];
                SqlParameter[] param = new SqlParameter[1];
                int j = 0;
                foreach (ListItem i in l)
                {
                    param[0] = new SqlParameter("@NamBC", int.Parse(i.Value));
                    //ds[j] = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, GetFullyQualifiedName(strSQL), param);
                    ds[j] = Config.SelectSPs(strSQL, param);
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
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu úm</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng sơ sinh</td><td>kg</td><td>0.08 - 0.09</td>" + GetValues(dt, "KhoiLuongSoSinh", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td></td>" + GetValues(dt, "KhoiLuongCuoiGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>90 - 95</td>" + GetValues(dt, "TyLeNuoiSong1_30", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>7.00 - 9.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanCaCon", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 1)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu một năm tuổi)</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>1.50 - 2.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td>1.50 - 2.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td>1.50 - 2.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>85 - 90</td>" + GetValues(dt, "TyLeNuoiSongMN", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td>" + GetValues(dt, "TyLeLoaiThaiMN", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>4.93 - 4.71</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td>4.93 - 4.71</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td>4.93 - 4.71</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>7.00 - 9.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanCaMotNam", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 2)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh trưởng 1: 13-24 tháng tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>7.00 - 9.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td>7.00 - 9.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td>7.00 - 9.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>90 - 95</td>" + GetValues(dt, "TyLeNuoiSongST1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td>" + GetValues(dt, "TyLeLoaiThaiST1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>5.45 - 5.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td>5.45 - 5.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td>5.45 - 5.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>30.00 - 35.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 3)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh trưởng 2: 25-36 tháng tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>20.00 - 25.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td>20.00 - 25.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td>20.00 - 25.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>95 - 97</td>" + GetValues(dt, "TyLeNuoiSongST2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td>" + GetValues(dt, "TyLeLoaiThaiST2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>5.00 - 4.69</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td>5.00 - 4.69</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td>5.00 - 4.69</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>65.00 - 75.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Trọng lượng hơi bình quân con giết mổ</td><td>kg/con</td><td></td>" + GetValues(dt, "TrongLuongHoiTB", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 4)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu Hậu bị 1: 4 tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>30.00 - 35.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td>30.00 - 35.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td>30.00 - 35.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td></td>" + GetValues(dt, "TyLeNuoiSongHB1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td>" + GetValues(dt, "TyLeLoaiThaiHB1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>9.00 - 10.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td>9.00 - 10.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td>9.00 - 10.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Trọng lượng hơi bình quân con giết mổ</td><td>kg/con</td><td></td>" + GetValues(dt, "TrongLuongHoiTB", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 5)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu Hậu bị 2: >= 5 tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td>40.00 - 45.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td>40.00 - 45.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td>40.00 - 45.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td></td>" + GetValues(dt, "TyLeNuoiSongHB2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td>" + GetValues(dt, "TyLeLoaiThaiHB2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td>10.00 - 11.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td>10.00 - 11.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td>10.00 - 11.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Trọng lượng hơi bình quân con giết mổ</td><td>kg/con</td><td></td>" + GetValues(dt, "TrongLuongHoiTB", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 6)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh sản 1: 6-7 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td>" + GetValues(dt, "TyLeLoaiThaiSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiSS1ThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>10.00 - 15.00</td>" + GetValues(dt, "SanLuongTrungSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>65 - 75</td>" + GetValues(dt, "TyLeTrungChonApSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>50 - 60</td>" + GetValues(dt, "TyLeTrungCoPhoiSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>60 - 65</td>" + GetValues(dt, "TyLeTrungNoSS1", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Số con nở</td><td>con</td><td></td>" + GetValues(dt, "SoConNoSS1", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 7)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh sản 2: 8-9 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td>" + GetValues(dt, "TyLeLoaiThaiSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiSS2ThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>20.00 - 25.00</td>" + GetValues(dt, "SanLuongTrungSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>85 - 90</td>" + GetValues(dt, "TyLeTrungChonApSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>75 - 80</td>" + GetValues(dt, "TyLeTrungCoPhoiSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>70 - 80</td>" + GetValues(dt, "TyLeTrungNoSS2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Số con nở</td><td>con</td><td></td>" + GetValues(dt, "SoConNoSS2", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 8)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh sản 3: > 10 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td>" + GetValues(dt, "TyLeLoaiThaiTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiTSCDThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>30.00 - 40.00</td>" + GetValues(dt, "SanLuongTrungTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>95 - 98</td>" + GetValues(dt, "TyLeTrungChonApTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>95 - 98</td>" + GetValues(dt, "TyLeTrungCoPhoiTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>75 - 85</td>" + GetValues(dt, "TyLeTrungNoTSCD", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Số con nở</td><td>con</td><td></td>" + GetValues(dt, "SoConNoTSCD", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 9)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Cá sấu sinh sản 1 nước mặn</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongNM1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td>" + GetValues(dt, "TyLeLoaiThaiNM1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiNMThamGiaDe1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>30.00 - 40.00</td>" + GetValues(dt, "SanLuongTrungNM1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>95 - 98</td>" + GetValues(dt, "TyLeTrungChonApNM1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>95 - 98</td>" + GetValues(dt, "TyLeTrungCoPhoiNM1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>75 - 85</td>" + GetValues(dt, "TyLeTrungNoNM1", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Số con nở</td><td>con</td><td></td>" + GetValues(dt, "SoConNoNM1", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 10)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Cá sấu sinh sản 2 nước mặn</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongNM2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td></td>" + GetValues(dt, "TyLeLoaiThaiNM2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiNMThamGiaDe2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td>30.00 - 40.00</td>" + GetValues(dt, "SanLuongTrungNM2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td>95 - 98</td>" + GetValues(dt, "TyLeTrungChonApNM2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td>95 - 98</td>" + GetValues(dt, "TyLeTrungCoPhoiNM2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td>75 - 85</td>" + GetValues(dt, "TyLeTrungNoNM2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Số con nở</td><td>con</td><td></td>" + GetValues(dt, "SoConNoNM2", 1, 2, ci) + "</tr>");
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
                string strSQL = "";
                if (ddlChuan.SelectedValue == "1" && ddlCachTinh.SelectedValue == "1")
                {
                    strSQL = "QLCS_BCTK_DMKTKT_new_new_new_ChuanCu_SP";
                }
                else if (ddlChuan.SelectedValue == "1" && ddlCachTinh.SelectedValue == "2")
                {
                    strSQL = "QLCS_BCTK_DMKTKT_new_new_new_NgayAnCu_ChuanCu";
                }
                else if (ddlChuan.SelectedValue == "2" && ddlCachTinh.SelectedValue == "1")
                {
                    strSQL = "QLCS_BCTK_DMKTKT_new_new_new_SP";
                }
                else if (ddlChuan.SelectedValue == "2" && ddlCachTinh.SelectedValue == "2")
                {
                    strSQL = "QLCS_BCTK_DMKTKT_new_new_new_NgayAnCu";
                }
                DataSet[] ds = new DataSet[SoNam];
                SqlParameter[] param = new SqlParameter[1];
                int j = 0;
                foreach (ListItem i in l)
                {
                    param[0] = new SqlParameter("@NamBC", int.Parse(i.Value));
                    //ds[j] = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, GetFullyQualifiedName(strSQL), param);
                    ds[j] = Config.SelectSPs(strSQL, param);
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
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu úm</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng sơ sinh</td><td>kg</td><td style='text-align:center;'>0.08 - 0.09</td>" + GetValues(dt, "KhoiLuongSoSinh", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td style='text-align:center;'></td>" + GetValues(dt, "KhoiLuongCuoiGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>90 - 95</td>" + GetValues(dt, "TyLeNuoiSong1_30", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>7.00 - 9.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanCaCon", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 1)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu một năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td style='text-align:center;'>1.50 - 2.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td style='text-align:center;'>1.50 - 2.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td style='text-align:center;'>1.50 - 2.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>85 - 90</td>" + GetValues(dt, "TyLeNuoiSongMN", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td style='text-align:center;'></td>" + GetValues(dt, "TyLeLoaiThaiMN", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td style='text-align:center;'>4.93 - 4.71</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td style='text-align:center;'>4.93 - 4.71</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td style='text-align:center;'>4.93 - 4.71</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>7.00 - 9.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoanCaMotNam", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 2)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh trưởng 1: 13-24 tháng tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td style='text-align:center;'>7.00 - 9.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td style='text-align:center;'>7.00 - 9.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td style='text-align:center;'>7.00 - 9.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>90 - 95</td>" + GetValues(dt, "TyLeNuoiSongST1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td style='text-align:center;'></td>" + GetValues(dt, "TyLeLoaiThaiST1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td style='text-align:center;'>5.45 - 5.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td style='text-align:center;'>5.45 - 5.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td style='text-align:center;'>5.45 - 5.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>30.00 - 35.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 3)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh trưởng 2: 25-36 tháng tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td style='text-align:center;'>20.00 - 25.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td style='text-align:center;'>20.00 - 25.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td style='text-align:center;'>20.00 - 25.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>95 - 97</td>" + GetValues(dt, "TyLeNuoiSongST2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td style='text-align:center;'></td>" + GetValues(dt, "TyLeLoaiThaiST2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td style='text-align:center;'>5.00 - 4.69</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td style='text-align:center;'>5.00 - 4.69</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td style='text-align:center;'>5.00 - 4.69</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>65.00 - 75.00</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Trọng lượng hơi bình quân con giết mổ</td><td>kg/con</td><td style='text-align:center;'></td>" + GetValues(dt, "TrongLuongHoiTB", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 4)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu hậu bị 1: 4 tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td style='text-align:center;'>30.00 - 35.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td style='text-align:center;'>30.00 - 35.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td style='text-align:center;'>30.00 - 35.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'></td>" + GetValues(dt, "TyLeNuoiSongHB1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td style='text-align:center;'></td>" + GetValues(dt, "TyLeLoaiThaiHB1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td style='text-align:center;'>9.00 - 10.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td style='text-align:center;'>9.00 - 10.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td style='text-align:center;'>9.00 - 10.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Trọng lượng hơi bình quân con giết mổ</td><td>kg/con</td><td style='text-align:center;'></td>" + GetValues(dt, "TrongLuongHoiTB", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 5)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu hậu bị 2: >= 5 tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Khối lượng cuối giai đoạn</td><td>kg</td><td style='text-align:center;'>40.00 - 45.00</td>" + GetValues(dt, "KhoiLuongCuoiKy", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td style='text-align:center;'>40.00 - 45.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td style='text-align:center;'>40.00 - 45.00</td>" + GetValues(dt, "KhoiLuongCuoiKy_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'></td>" + GetValues(dt, "TyLeNuoiSongHB2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td style='text-align:center;'></td>" + GetValues(dt, "TyLeLoaiThaiHB2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn/kg tăng trọng</td><td>kg</td><td style='text-align:center;'>10.00 - 11.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá giống</td><td>kg</td><td style='text-align:center;'>10.00 - 11.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_Giong", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>+ Cá tăng trọng</td><td>kg</td><td style='text-align:center;'>10.00 - 11.00</td>" + GetValues(dt, "TieuTonThucAnKgTangTrong_TT", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'></td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Trọng lượng hơi bình quân con giết mổ</td><td>kg/con</td><td style='text-align:center;'></td>" + GetValues(dt, "TrongLuongHoiTB", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 6)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh sản 1: 6-7 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td style='text-align:center;'></td>" + GetValues(dt, "TyLeLoaiThaiSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>120 -130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiSS1ThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td style='text-align:center;'>10.00 - 15.00</td>" + GetValues(dt, "SanLuongTrungSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td style='text-align:center;'>65 - 75</td>" + GetValues(dt, "TyLeTrungChonApSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td style='text-align:center;'>50 - 60</td>" + GetValues(dt, "TyLeTrungCoPhoiSS1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td style='text-align:center;'>60 - 65</td>" + GetValues(dt, "TyLeTrungNoSS1", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Số con nở</td><td>con</td><td></td>" + GetValues(dt, "SoConNoSS1", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 7)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh sản 2: 8-9 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td style='text-align:center;'></td>" + GetValues(dt, "TyLeLoaiThaiSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiSS2ThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td style='text-align:center;'>20.00 - 25.00</td>" + GetValues(dt, "SanLuongTrungSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td style='text-align:center;'>85 - 90</td>" + GetValues(dt, "TyLeTrungChonApSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td style='text-align:center;'>75 - 80</td>" + GetValues(dt, "TyLeTrungCoPhoiSS2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td style='text-align:center;'>70 - 80</td>" + GetValues(dt, "TyLeTrungNoSS2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Số con nở</td><td>con</td><td></td>" + GetValues(dt, "SoConNoSS2", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 8)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Giai đoạn nuôi cá sấu sinh sản 3: > 10 năm tuổi</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td style='text-align:center;'></td>" + GetValues(dt, "TyLeLoaiThaiTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiTSCDThamGiaDe", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td style='text-align:center;'>30.00 - 40.00</td>" + GetValues(dt, "SanLuongTrungTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td style='text-align:center;'>95 - 98</td>" + GetValues(dt, "TyLeTrungChonApTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td style='text-align:center;'>95 - 98</td>" + GetValues(dt, "TyLeTrungCoPhoiTSCD", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td style='text-align:center;'>75 - 85</td>" + GetValues(dt, "TyLeTrungNoTSCD", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Số con nở</td><td>con</td><td></td>" + GetValues(dt, "SoConNoTSCD", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 9)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Cá sấu sinh sản 1 nước mặn</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongNM1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td style='text-align:center;'></td>" + GetValues(dt, "TyLeLoaiThaiNM1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiNMThamGiaDe1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td style='text-align:center;'>30.00 - 40.00</td>" + GetValues(dt, "SanLuongTrungNM1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td style='text-align:center;'>95 - 98</td>" + GetValues(dt, "TyLeTrungChonApNM1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td style='text-align:center;'>95 - 98</td>" + GetValues(dt, "TyLeTrungCoPhoiNM1", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td style='text-align:center;'>75 - 85</td>" + GetValues(dt, "TyLeTrungNoNM1", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Số con nở</td><td>con</td><td></td>" + GetValues(dt, "SoConNoNM1", 1, 2, ci) + "</tr>");
                    }
                    else if (i == 10)
                    {
                        sb.Append(@"<tr style='font-weight:bold; vertical-align:middle; font-size:larger;'><td align='left' colspan='" + TongSoCot.ToString() + "'>Cá sấu sinh sản 2 nước mặn</td></tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ nuôi sống</td><td>%</td><td style='text-align:center;'>97 - 99</td>" + GetValues(dt, "TyLeNuoiSongNM2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ loại thải</td><td>%</td><td style='text-align:center;'></td>" + GetValues(dt, "TyLeLoaiThaiNM2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tiêu tốn thức ăn toàn giai đoạn</td><td>kg/con</td><td style='text-align:center;'>120 - 130</td>" + GetValues(dt, "TieuTonThucAnToanGiaiDoan", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ cái tham gia đẻ</td><td>%</td><td></td>" + GetValues(dt, "TyLeCaiNMThamGiaDe2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Sản lượng trứng</td><td>Quả/năm</td><td style='text-align:center;'>30.00 - 40.00</td>" + GetValues(dt, "SanLuongTrungNM2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng chọn ấp</td><td>%</td><td style='text-align:center;'>95 - 98</td>" + GetValues(dt, "TyLeTrungChonApNM2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng có phôi</td><td>%</td><td style='text-align:center;'>95 - 98</td>" + GetValues(dt, "TyLeTrungCoPhoiNM2", 1, 2, ci) + "</tr>");
                        sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Tỷ lệ trứng nở/trứng có phôi</td><td>%</td><td style='text-align:center;'>75 - 85</td>" + GetValues(dt, "TyLeTrungNoNM2", 1, 2, ci) + "</tr>");
                        //sb.Append(@"<tr style='vertical-align:middle;'><td align='left'>Số con nở</td><td>con</td><td></td>" + GetValues(dt, "SoConNoNM2", 1, 2, ci) + "</tr>");
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