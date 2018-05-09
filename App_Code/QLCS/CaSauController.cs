using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using DotNetNuke.NewsProvider;
using DotNetNuke.Services.Search;
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Common;
using Microsoft.ApplicationBlocks.Data;
using System.Web;
using System.Collections.Specialized;
using DotNetNuke.Framework.Providers;

namespace DotNetNuke.Modules.QLCS
{
    public class CaSauController
    {
        public CaSauController() { }

        /*Ca Sau=======================================================================================================================*/

        //public DataTable LoadCaSauMeByMonth(string d)
        //{
        //    DataTable result = null;
        //    try
        //    {
        //        SqlParameter[] param = new SqlParameter[1];
        //        param[0] = new SqlParameter("@d", d);
        //        result = DataProvider.SelectSP("QLCS_GetCaSauMeByMonth", param);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        public void CaSau_UpdateGhiChu(int IDCaSau, string GhiChu)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@IDCaSau", IDCaSau);
                param[1] = new SqlParameter("@GhiChu", GhiChu);
                DataProvider.ExecuteSP("QLCS_CaSau_UpdateGhiChu", param);
            }
            catch (Exception ex)
            {
            }
        }

        public DataTable LoadCaSauByID(int CaSauID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDCaSau", CaSauID);
                result = DataProvider.SelectSP("QLCS_CaSau_GetCaSauByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadCaSauHienTaiByID(int CaSauID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDCaSau", CaSauID);
                result = DataProvider.SelectSP("QLCS_CaSau_GetCaSauHienTaiByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadPageCaSau_HienTai(string WhereClause, string OrderBy, int StartIndex, int PageSize)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                param[2] = new SqlParameter("@StartIndex", StartIndex);
                param[3] = new SqlParameter("@PageSize", PageSize);
                result = DataProvider.SelectSP("QLCS_GetCaSau_Page_HienTai", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadPageCaSau(string WhereClause, string OrderBy, int StartIndex, int PageSize, DateTime dDate)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                param[2] = new SqlParameter("@StartIndex", StartIndex);
                param[3] = new SqlParameter("@PageSize", PageSize);
                param[4] = new SqlParameter("@Date", dDate.ToString("yyyyMMdd HH:mm:ss"));
                param[5] = new SqlParameter("@dDate", dDate);
                result = DataProvider.SelectSP("QLCS_GetCaSau_Page", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadPageCaSauMainTable(string WhereClause, string OrderBy, int StartIndex, int PageSize)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                param[2] = new SqlParameter("@StartIndex", StartIndex);
                param[3] = new SqlParameter("@PageSize", PageSize);
                result = DataProvider.SelectSP("QLCS_GetCaSauMainTable_Page", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadPageBienDongGroup(string WhereClause, string OrderBy, int StartIndex, int PageSize)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                param[2] = new SqlParameter("@StartIndex", StartIndex);
                param[3] = new SqlParameter("@PageSize", PageSize);
                result = DataProvider.SelectSP("QLCS_GetBienDongGroup_Page", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadPageCaChet_HienTai(string WhereClause, string OrderBy, int StartIndex, int PageSize)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                param[2] = new SqlParameter("@StartIndex", StartIndex);
                param[3] = new SqlParameter("@PageSize", PageSize);
                result = DataProvider.SelectSP("QLCS_GetCaSauChet_Page_HienTai", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadPageCaChetDelete(string WhereClause, string OrderBy, int StartIndex, int PageSize)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                param[2] = new SqlParameter("@StartIndex", StartIndex);
                param[3] = new SqlParameter("@PageSize", PageSize);
                result = DataProvider.SelectSP("QLCS_GetCaSauChet_Page_Delete", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int CountCaChet_HienTai(string WhereClause)
        {
            int TotalRecords = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@TotalRecords", TotalRecords);
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetCaSauChet_Count_HienTai", param);
                TotalRecords = Convert.ToInt32(param[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TotalRecords;
        }

        public int CountCaChetDelete(string WhereClause)
        {
            int TotalRecords = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@TotalRecords", TotalRecords);
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetCaSauChet_Count_Delete", param);
                TotalRecords = Convert.ToInt32(param[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TotalRecords;
        }

        public int CountCaSau_HienTai(string WhereClause)
        {
            int TotalRecords = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@TotalRecords", TotalRecords);
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetCaSau_Count_HienTai", param);
                TotalRecords = Convert.ToInt32(param[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TotalRecords;
        }

        public int CountCaSau(string WhereClause, DateTime dDate)
        {
            int TotalRecords = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@Date", dDate.ToString("yyyyMMdd HH:mm:ss"));
                param[2] = new SqlParameter("@dDate", dDate);
                param[3] = new SqlParameter("@TotalRecords", TotalRecords);
                param[3].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetCaSau_Count", param);
                TotalRecords = Convert.ToInt32(param[3].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TotalRecords;
        }

        public int CountCaSauMainTable(string WhereClause)
        {
            int TotalRecords = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@TotalRecords", TotalRecords);
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetCaSauMainTable_Count", param);
                TotalRecords = Convert.ToInt32(param[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TotalRecords;
        }

        public int CountBienDongGroup(string WhereClause)
        {
            int TotalRecords = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@TotalRecords", TotalRecords);
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetBienDongGroup_Count", param);
                TotalRecords = Convert.ToInt32(param[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TotalRecords;
        }

        public DataTable LoadPageBienDong(string WhereClause, string OrderBy, int StartIndex, int PageSize)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                param[2] = new SqlParameter("@StartIndex", StartIndex);
                param[3] = new SqlParameter("@PageSize", PageSize);
                result = DataProvider.SelectSP("QLCS_GetBienDong_Page", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int CountBienDong(string WhereClause)
        {
            int TotalRecords = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@TotalRecords", TotalRecords);
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetBienDong_Count", param);
                TotalRecords = Convert.ToInt32(param[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TotalRecords;
        }

        public DataTable LoadCaSauMe()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_GetCaSauMe");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadCaSauMe_AllTrangThai()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_GetCaSauMe_AllTrangThai");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadCaSauMe_AllTrangThai_ByChuong(int IDChuong)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDChuong", IDChuong);
                result = DataProvider.SelectSP("QLCS_GetCaSauMe_AllTrangThai_ByChuong", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadCaSauMe_ByChuong_ByThoiDiem(int IDChuong, DateTime ThoiDiem)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@IDChuong", IDChuong);
                param[1] = new SqlParameter("@ThoiDiem", ThoiDiem);
                result = DataProvider.SelectSP("QLCS_GetCaSauMe_ByChuong_ByThoiDiem", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadCaSauMe_Except(int IDCaSau)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDCaSau", IDCaSau);
                result = DataProvider.SelectSP("QLCS_GetCaSauMe_Except", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int InsertCaSauMultiple(int soluong, CaSauInfo cs, int NguoiThayDoi)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@MaSo", cs.MaSo);
                param[1] = new SqlParameter("@GioiTinh", cs.GioiTinh);
                param[2] = new SqlParameter("@Giong", cs.Giong);
                param[3] = new SqlParameter("@LoaiCa", cs.LoaiCa);
                param[4] = new SqlParameter("@NgayNo", cs.NgayNo);
                param[5] = new SqlParameter("@NgayXuongChuong", cs.NgayXuongChuong);
                param[6] = new SqlParameter("@NguonGoc", cs.NguonGoc);
                param[7] = new SqlParameter("@Chuong", cs.Chuong);
                param[8] = new SqlParameter("@CaMe", cs.CaMe);
                param[9] = new SqlParameter("@Status", cs.Status);
                param[10] = new SqlParameter("@GhiChu", cs.GhiChu);
                param[11] = new SqlParameter("@SoLuong", soluong);
                param[12] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[13] = new SqlParameter("@StrNgayXuongChuong", cs.NgayXuongChuong.Value.ToString("dd/MM/yyyy HH:mm:ss"));
                result = DataProvider.ExecuteSP("QLCS_CaSau_Insert_Multiple", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int UpdateCaSau(int IDCaSau, bool Giong, DateTime? NgayNo, DateTime? NgayXuongChuong, int NguonGoc, int CaMe, string GhiChu)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@IDCaSau", IDCaSau);
                param[1] = new SqlParameter("@Giong", Giong);
                param[2] = new SqlParameter("@NgayNo", NgayNo);
                param[3] = new SqlParameter("@NgayXuongChuong", NgayXuongChuong);
                param[4] = new SqlParameter("@NguonGoc", NguonGoc);
                param[5] = new SqlParameter("@CaMe", CaMe);
                param[6] = new SqlParameter("@GhiChu", GhiChu);
                result = DataProvider.ExecuteSP("QLCS_CaSau_Update", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public DataTable CaSau_GetCaSauFromArray(string StrIDCaSau)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@StrIDCaSau", StrIDCaSau);
                result = DataProvider.SelectSP("QLCS_CaSau_GetCaSauFromArray", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable CaSau_GetCaFromChuong(int Chuong, int LoaiCa, int SoLuong, DateTime? NgayChet)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@Chuong", Chuong);
                param[1] = new SqlParameter("@LoaiCa", LoaiCa);
                param[2] = new SqlParameter("@SoLuong", SoLuong);
                param[3] = new SqlParameter("@NgayChet", NgayChet);
                result = DataProvider.SelectSP("QLCS_CaSau_GetCaFromChuong", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable CaSau_GetCaFromChuong1(string TenChuong, int So, int LoaiCa, int Giong, string MaSo, int SoLuong, DateTime? NgayChet)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@TenChuong", TenChuong);
                param[1] = new SqlParameter("@So", So);
                param[2] = new SqlParameter("@LoaiCa", LoaiCa);
                param[3] = new SqlParameter("@Giong", Giong);
                param[4] = new SqlParameter("@MaSo", MaSo);
                param[5] = new SqlParameter("@SoLuong", SoLuong);
                param[6] = new SqlParameter("@NgayChet", NgayChet);
                result = DataProvider.SelectSP("QLCS_CaSau_GetCaFromChuong1", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /*Danh Muc=======================================================================================================================*/
        
        public string GetNhanVienFromStrID(string StrID)
        {
            string StrOut = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrID", StrID);
                param[1] = new SqlParameter("@StrOut", StrOut);
                param[1].Size = 4000;
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetNhanVienFromStrID", param);
                StrOut = param[1].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StrOut;
        }

        public string GetNhanVienFromStrID_CoKhuChuong(string StrID)
        {
            string StrOut = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrID", StrID);
                param[1] = new SqlParameter("@StrOut", StrOut);
                param[1].Size = 4000;
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetNhanVienFromStrID_CoKhuChuong", param);
                StrOut = param[1].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StrOut;
        }

        public DataTable LoadNhanVien(int Active)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Active", Active);
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetNhanVien",param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadKhuChuong()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_GetKhuChuong");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadPhongAp(int Active)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Active", Active);
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetPhongAp", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadLoaiCa(int Active)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Active", Active);
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetLoaiCa", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadLoaiCaLon()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetLoaiCaLon");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadChuong(int Active)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Active", Active);
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetChuong", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadChuongCaLon()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetChuongCaLon");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadChuongByTenChuong(string StrTenChuong)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@StrTenChuong", StrTenChuong);
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetChuongByTenChuong", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadChuongSoLuong()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetChuongSoLuong");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadNguonGoc(int Active)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Active", Active);
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetNguonGoc",param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadKhayAp(int Active)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Active", Active);
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetKhayAp",param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadLyDoThaiLoaiTrung(int Active)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Active", Active);
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetLyDoThaiLoaiTrung",param);
            }
            catch (Exception ex) { throw ex; }
            return result;
        }

        public DataTable LoadLyDoChet(int Active)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Active", Active);
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetLyDoChet", param);
            }
            catch (Exception ex) { throw ex; }
            return result;
        }

        public DataTable LoadNhaCungCap(int Active)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Active", Active);
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetNhaCungCap", param);
            }
            catch (Exception ex) { throw ex; }
            return result;
        }

        public DataTable LoadKhayUm()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetKhayUm");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadLoaiBienDongVatTu()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetLoaiBienDongVatTu");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadLoaiBienDong()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_DanhMuc_GetLoaiBienDong");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadKhoiLuongCuoiKy()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_KhoiLuongCuoiKy_GetAll");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int InsertChuong(string Chuong, string TenChuong, int? So, out int IDChuong)
        {
            int result = 0;
            IDChuong = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@Chuong", Chuong);
                param[1] = new SqlParameter("@DonVi", 1);
                param[2] = new SqlParameter("@Active", 1);
                param[3] = new SqlParameter("@Status", 0);
                param[4] = new SqlParameter("@TenChuong", TenChuong);
                param[5] = new SqlParameter("@So", So);
                param[6] = new SqlParameter("@IDChuong", 0);
                param[6].Direction = ParameterDirection.Output;
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_Chuong_Insert", param);
                IDChuong = Convert.ToInt32(param[6].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int InsertKhayAp(string TenKhayAp, out int IDKhayAp)
        {
            int result = 0;
            IDKhayAp = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TenKhayAp", TenKhayAp);
                param[1] = new SqlParameter("@Active", 1);
                param[2] = new SqlParameter("@Status", 0);
                param[3] = new SqlParameter("@IDKhayAp", 0);
                param[3].Direction = ParameterDirection.Output;
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_KhayAp_Insert", param);
                IDKhayAp = Convert.ToInt32(param[3].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void InsertLyDoThaiLoaiTrung(String TenLyDoThaiLoaiTrung, String Code, Boolean Active, String Status, out Int32 IDLyDoThaiLoaiTrung)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@TenLyDoThaiLoaiTrung", TenLyDoThaiLoaiTrung);
                param[1] = new SqlParameter("@Code", Code);
                param[2] = new SqlParameter("@Active", Active);
                param[3] = new SqlParameter("@Status", Status);
                param[4] = new SqlParameter("@IDLyDoThaiLoaiTrung", 0);
                param[4].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_DanhMuc_LyDoThaiLoaiTrung_Insert", param);
                IDLyDoThaiLoaiTrung = Convert.ToInt32(param[4].Value);
            }
            catch (Exception ex) { throw ex; }
        }

        public void InsertLyDoChet(String TenLyDoChet, String Code, Boolean Active, String Status, out Int32 IDLyDoChet)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@TenLyDoChet", TenLyDoChet);
                param[1] = new SqlParameter("@Code", Code);
                param[2] = new SqlParameter("@Active", Active);
                param[3] = new SqlParameter("@Status", Status);
                param[4] = new SqlParameter("@IDLyDoChet", 0);
                param[4].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_DanhMuc_LyDoChet_Insert", param);
                IDLyDoChet = Convert.ToInt32(param[4].Value);
            }
            catch (Exception ex) { throw ex; }
        }

        public void InsertNhaCungCap(String NhaCungCap, String LoaiVatTu, Boolean Active, int Status, out Int32 ID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@NhaCungCap", NhaCungCap);
                param[1] = new SqlParameter("@LoaiVatTu", LoaiVatTu);
                param[2] = new SqlParameter("@Active", Active);
                param[3] = new SqlParameter("@Status", Status);
                param[4] = new SqlParameter("@ID", 0);
                param[4].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_DanhMuc_NhaCungCap_Insert", param);
                ID = Convert.ToInt32(param[4].Value);
            }
            catch (Exception ex) { throw ex; }
        }

        public int InsertKhayUm(string TenKhayUm, out int IDKhayUm)
        {
            int result = 0;
            IDKhayUm = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TenKhayUm", TenKhayUm);
                param[1] = new SqlParameter("@Active", 1);
                param[2] = new SqlParameter("@Status", 0);
                param[3] = new SqlParameter("@IDKhayUm", 0);
                param[3].Direction = ParameterDirection.Output;
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_KhayUm_Insert", param);
                IDKhayUm = Convert.ToInt32(param[3].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int InsertNguonGoc(string TenNguonGoc, out int IDNguonGoc)
        {
            int result = 0;
            IDNguonGoc = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TenNguonGoc", TenNguonGoc);
                param[1] = new SqlParameter("@Active", 1);
                param[2] = new SqlParameter("@Status", 0);
                param[3] = new SqlParameter("@IDNguonGoc", 0);
                param[3].Direction = ParameterDirection.Output;
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_NguonGoc_Insert", param);
                IDNguonGoc = Convert.ToInt32(param[3].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int InsertLoaiCa(string TenLoaiCa, out int IDLoaiCa)
        {
            int result = 0;
            IDLoaiCa = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TenLoaiCa", TenLoaiCa);
                param[1] = new SqlParameter("@Active", 1);
                param[2] = new SqlParameter("@Status", 0);
                param[3] = new SqlParameter("@IDLoaiCa", 0);
                param[3].Direction = ParameterDirection.Output;
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_LoaiCa_Insert", param);
                IDLoaiCa = Convert.ToInt32(param[3].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int InsertNhanVien(string TenNhanVien, out int IDNhanVien)
        {
            int result = 0;
            IDNhanVien = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TenNhanVien", TenNhanVien);
                param[1] = new SqlParameter("@Active", 1);
                param[2] = new SqlParameter("@Status", 0);
                param[3] = new SqlParameter("@IDNhanVien", 0);
                param[3].Direction = ParameterDirection.Output;
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_NhanVien_Insert", param);
                IDNhanVien = Convert.ToInt32(param[3].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int InsertPhongAp(string TenPhongAp, out int IDPhongAp)
        {
            int result = 0;
            IDPhongAp = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@TenPhongAp", TenPhongAp);
                param[1] = new SqlParameter("@Active", 1);
                param[2] = new SqlParameter("@Status", 0);
                param[3] = new SqlParameter("@IDPhongAp", 0);
                param[3].Direction = ParameterDirection.Output;
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_PhongAp_Insert", param);
                IDPhongAp = Convert.ToInt32(param[3].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void InsertKhoiLuongCuoiKy(int NamCan, decimal SoSinh, decimal CuoiKyUm_Giong, decimal CuoiKyUm_TT, decimal CuoiKyUm, decimal CuoiKy1Nam_Giong, decimal CuoiKy1Nam_TT, decimal CuoiKy1Nam, decimal CuoiKyST1_Giong, decimal CuoiKyST1_TT, decimal CuoiKyST1, decimal CuoiKyST2_Giong, decimal CuoiKyST2_TT, decimal CuoiKyST2, decimal CuoiKyHB1_Giong, decimal CuoiKyHB1_TT, decimal CuoiKyHB1, decimal CuoiKyHB2_Giong, decimal CuoiKyHB2_TT, decimal CuoiKyHB2)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[20];
                param[0] = new SqlParameter("@NamCan", NamCan);
                param[1] = new SqlParameter("@SoSinh", SoSinh);
                param[2] = new SqlParameter("@CuoiKyUm_Giong", CuoiKyUm_Giong);
                param[3] = new SqlParameter("@CuoiKyUm_TT", CuoiKyUm_TT);
                param[4] = new SqlParameter("@CuoiKyUm", CuoiKyUm);
                param[5] = new SqlParameter("@CuoiKy1Nam_Giong", CuoiKy1Nam_Giong);
                param[6] = new SqlParameter("@CuoiKy1Nam_TT", CuoiKy1Nam_TT);
                param[7] = new SqlParameter("@CuoiKy1Nam", CuoiKy1Nam);
                param[8] = new SqlParameter("@CuoiKyST1_Giong", CuoiKyST1_Giong);
                param[9] = new SqlParameter("@CuoiKyST1_TT", CuoiKyST1_TT);
                param[10] = new SqlParameter("@CuoiKyST1", CuoiKyST1);
                param[11] = new SqlParameter("@CuoiKyST2_Giong", CuoiKyST2_Giong);
                param[12] = new SqlParameter("@CuoiKyST2_TT", CuoiKyST2_TT);
                param[13] = new SqlParameter("@CuoiKyST2", CuoiKyST2);
                param[14] = new SqlParameter("@CuoiKyHB1_Giong", CuoiKyHB1_Giong);
                param[15] = new SqlParameter("@CuoiKyHB1_TT", CuoiKyHB1_TT);
                param[16] = new SqlParameter("@CuoiKyHB1", CuoiKyHB1);
                param[17] = new SqlParameter("@CuoiKyHB2_Giong", CuoiKyHB2_Giong);
                param[18] = new SqlParameter("@CuoiKyHB2_TT", CuoiKyHB2_TT);
                param[19] = new SqlParameter("@CuoiKyHB2", CuoiKyHB2);
                DataProvider.ExecuteSP("QLCS_KhoiLuongCuoiKy_Insert", param);
            }
            catch (Exception ex)
            {
            }
        }

        public int UpdateChuong(int IDChuong, string Chuong, bool Active, string TenChuong, int? So)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@IDChuong", IDChuong);
                param[1] = new SqlParameter("@Chuong", Chuong);
                param[2] = new SqlParameter("@DonVi", 1);
                param[3] = new SqlParameter("@Active", Active);
                param[4] = new SqlParameter("@Status", 0);
                param[5] = new SqlParameter("@TenChuong", TenChuong);
                param[6] = new SqlParameter("@So", So);
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_Chuong_Update", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int UpdateKhayAp(int IDKhayAp, string TenKhayAp, bool Active)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@IDKhayAp", IDKhayAp);
                param[1] = new SqlParameter("@TenKhayAp", TenKhayAp);
                param[2] = new SqlParameter("@Active", Active);
                param[3] = new SqlParameter("@Status", 0);
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_KhayAp_Update", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void UpdateLyDoThaiLoaiTrung(Int32 IDLyDoThaiLoaiTrung, String TenLyDoThaiLoaiTrung, String Code, Boolean Active, String Status)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@IDLyDoThaiLoaiTrung", IDLyDoThaiLoaiTrung);
                param[1] = new SqlParameter("@TenLyDoThaiLoaiTrung", TenLyDoThaiLoaiTrung);
                param[2] = new SqlParameter("@Code", Code);
                param[3] = new SqlParameter("@Active", Active);
                param[4] = new SqlParameter("@Status", Status);
                DataProvider.ExecuteSP("QLCS_DanhMuc_LyDoThaiLoaiTrung_Update", param);
            }
            catch (Exception ex) { throw ex; }
        }

        public void UpdateLyDoChet(Int32 IDLyDoChet, String TenLyDoChet, String Code, Boolean Active, String Status)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@IDLyDoChet", IDLyDoChet);
                param[1] = new SqlParameter("@TenLyDoChet", TenLyDoChet);
                param[2] = new SqlParameter("@Code", Code);
                param[3] = new SqlParameter("@Active", Active);
                param[4] = new SqlParameter("@Status", Status);
                DataProvider.ExecuteSP("QLCS_DanhMuc_LyDoChet_Update", param);
            }
            catch (Exception ex) { throw ex; }
        }

        public void UpdateNhaCungCap(Int32 ID, String NhaCungCap, String LoaiVatTu, Boolean Active, int Status)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@ID", ID);
                param[1] = new SqlParameter("@NhaCungCap", NhaCungCap);
                param[2] = new SqlParameter("@LoaiVatTu", LoaiVatTu);
                param[3] = new SqlParameter("@Active", Active);
                param[4] = new SqlParameter("@Status", Status);
                DataProvider.ExecuteSP("QLCS_DanhMuc_NhaCungCap_Update", param);
            }
            catch (Exception ex) { throw ex; }
        }

        public int UpdateKhayUm(int IDKhayUm, string TenKhayUm, bool Active)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@IDKhayUm", IDKhayUm);
                param[1] = new SqlParameter("@TenKhayUm", TenKhayUm);
                param[2] = new SqlParameter("@Active", Active);
                param[3] = new SqlParameter("@Status", 0);
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_KhayUm_Update", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int UpdateNguonGoc(int IDNguonGoc, string TenNguonGoc, bool Active)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@IDNguonGoc", IDNguonGoc);
                param[1] = new SqlParameter("@TenNguonGoc", TenNguonGoc);
                param[2] = new SqlParameter("@Active", Active);
                param[3] = new SqlParameter("@Status", 0);
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_NguonGoc_Update", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int UpdateLoaiCa(int IDLoaiCa, string TenLoaiCa, bool Active)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@IDLoaiCa", IDLoaiCa);
                param[1] = new SqlParameter("@TenLoaiCa", TenLoaiCa);
                param[2] = new SqlParameter("@Active", Active);
                param[3] = new SqlParameter("@Status", 0);
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_LoaiCa_Update", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        
        public int UpdateNhanVien(int IDNhanVien, string TenNhanVien, bool Active)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@IDNhanVien", IDNhanVien);
                param[1] = new SqlParameter("@TenNhanVien", TenNhanVien);
                param[2] = new SqlParameter("@Active", Active);
                param[3] = new SqlParameter("@Status", 0);
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_NhanVien_Update", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        
        public int UpdatePhongAp(int IDPhongAp, string TenPhongAp, bool Active)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@IDPhongAp", IDPhongAp);
                param[1] = new SqlParameter("@TenPhongAp", TenPhongAp);
                param[2] = new SqlParameter("@Active", Active);
                param[3] = new SqlParameter("@Status", 0);
                result = DataProvider.ExecuteSP("QLCS_DanhMuc_PhongAp_Update", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void UpdateKhoiLuongCuoiKy(int NamCan, decimal SoSinh, decimal CuoiKyUm_Giong, decimal CuoiKyUm_TT, decimal CuoiKyUm, decimal CuoiKy1Nam_Giong, decimal CuoiKy1Nam_TT, decimal CuoiKy1Nam, decimal CuoiKyST1_Giong, decimal CuoiKyST1_TT, decimal CuoiKyST1, decimal CuoiKyST2_Giong, decimal CuoiKyST2_TT, decimal CuoiKyST2, decimal CuoiKyHB1_Giong, decimal CuoiKyHB1_TT, decimal CuoiKyHB1, decimal CuoiKyHB2_Giong, decimal CuoiKyHB2_TT, decimal CuoiKyHB2)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[20];
                param[0] = new SqlParameter("@NamCan", NamCan);
                param[1] = new SqlParameter("@SoSinh", SoSinh);
                param[2] = new SqlParameter("@CuoiKyUm_Giong", CuoiKyUm_Giong);
                param[3] = new SqlParameter("@CuoiKyUm_TT", CuoiKyUm_TT);
                param[4] = new SqlParameter("@CuoiKyUm", CuoiKyUm);
                param[5] = new SqlParameter("@CuoiKy1Nam_Giong", CuoiKy1Nam_Giong);
                param[6] = new SqlParameter("@CuoiKy1Nam_TT", CuoiKy1Nam_TT);
                param[7] = new SqlParameter("@CuoiKy1Nam", CuoiKy1Nam);
                param[8] = new SqlParameter("@CuoiKyST1_Giong", CuoiKyST1_Giong);
                param[9] = new SqlParameter("@CuoiKyST1_TT", CuoiKyST1_TT);
                param[10] = new SqlParameter("@CuoiKyST1", CuoiKyST1);
                param[11] = new SqlParameter("@CuoiKyST2_Giong", CuoiKyST2_Giong);
                param[12] = new SqlParameter("@CuoiKyST2_TT", CuoiKyST2_TT);
                param[13] = new SqlParameter("@CuoiKyST2", CuoiKyST2);
                param[14] = new SqlParameter("@CuoiKyHB1_Giong", CuoiKyHB1_Giong);
                param[15] = new SqlParameter("@CuoiKyHB1_TT", CuoiKyHB1_TT);
                param[16] = new SqlParameter("@CuoiKyHB1", CuoiKyHB1);
                param[17] = new SqlParameter("@CuoiKyHB2_Giong", CuoiKyHB2_Giong);
                param[18] = new SqlParameter("@CuoiKyHB2_TT", CuoiKyHB2_TT);
                param[19] = new SqlParameter("@CuoiKyHB2", CuoiKyHB2);
                DataProvider.ExecuteSP("QLCS_KhoiLuongCuoiKy_Update", param);
            }
            catch (Exception ex)
            {
            }
        }

        public DataTable LoadChuongByID(int IDChuong)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDChuong", IDChuong);
                result = DataProvider.SelectSP("QLCS_DanhMuc_Chuong_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadKhayApByID(int IDKhayAp)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDKhayAp", IDKhayAp);
                result = DataProvider.SelectSP("QLCS_DanhMuc_KhayAp_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadKhayUmByID(int IDKhayUm)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDKhayUm", IDKhayUm);
                result = DataProvider.SelectSP("QLCS_DanhMuc_KhayUm_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadLyDoThaiLoaiTrungByID(Int32 IDLyDoThaiLoaiTrung)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDLyDoThaiLoaiTrung", IDLyDoThaiLoaiTrung);
                result = DataProvider.SelectSP("QLCS_DanhMuc_LyDoThaiLoaiTrung_GetByID", param);
            }
            catch (Exception ex) { throw ex; }
            return result;
        }

        public DataTable LoadLyDoChetByID(Int32 IDLyDoChet)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDLyDoChet", IDLyDoChet);
                result = DataProvider.SelectSP("QLCS_DanhMuc_LyDoChet_GetByID", param);
            }
            catch (Exception ex) { throw ex; }
            return result;
        }

        public DataTable LoadNhaCungCapByID(Int32 ID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                result = DataProvider.SelectSP("QLCS_DanhMuc_NhaCungCap_GetByID", param);
            }
            catch (Exception ex) { throw ex; }
            return result;
        }

        public DataTable LoadNhaCungCapByVatTu(Int32 IDVatTu)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDVatTu", IDVatTu);
                result = DataProvider.SelectSP("QLCS_DanhMuc_NhaCungCap_GetByVatTu", param);
            }
            catch (Exception ex) { throw ex; }
            return result;
        }

        public DataTable LoadNguonGocByID(int IDNguonGoc)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDNguonGoc", IDNguonGoc);
                result = DataProvider.SelectSP("QLCS_DanhMuc_NguonGoc_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadNhanVienByID(int IDNhanVien)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDNhanVien", IDNhanVien);
                result = DataProvider.SelectSP("QLCS_DanhMuc_NhanVien_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadPhongApByID(int IDPhongAp)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDPhongAp", IDPhongAp);
                result = DataProvider.SelectSP("QLCS_DanhMuc_PhongAp_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadLoaiCaByID(int IDLoaiCa)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDLoaiCa", IDLoaiCa);
                result = DataProvider.SelectSP("QLCS_DanhMuc_LoaiCa_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadKhoiLuongCuoiKyByNamCan(int NamCan)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@NamCan", NamCan);
                result = DataProvider.SelectSP("QLCS_KhoiLuongCuoiKy_GetByNamCan", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void KhoiLuongCuoiKy_Delete(int NamCan)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@NamCan", NamCan);
                DataProvider.ExecuteSP("QLCS_KhoiLuongCuoiKy_Delete", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetChuongByName(string Prefix, string Name, string TenChuong, int? So)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@Prefix", Prefix);
                param[1] = new SqlParameter("@Name", Name);
                param[2] = new SqlParameter("@TenChuong", TenChuong);
                param[3] = new SqlParameter("@So", So);
                param[4] = new SqlParameter("@IDChuong", -1);
                param[4].Direction = ParameterDirection.Output; ;
                DataProvider.ExecuteSP("QLCS_DanhMuc_GetChuongByName", param);
                result = Convert.ToInt32(param[4].Value);
            }
            catch (Exception ex) {}
            return result;
        }

        public void ParseChuong(string Chuong, out string TenChuong, out int So)
        {
            So = 0;
            TenChuong = "";
            string aSo = "0123456789";
            string sTenChuong = "";
            string sSo = "";
            for (int i = 0; i < Chuong.Length; i++)
            {
                if (aSo.Contains(Chuong[i].ToString())) sSo += Chuong[i].ToString();
                else if (Chuong[i] != ' ' && Chuong[i] != '.') sTenChuong += Chuong[i].ToString();
            }
            if (sSo != "") So = int.Parse(sSo);
            TenChuong = sTenChuong;
        }

        /*Bien Dong======================================================================================================================*/

        public int CoTheNhapChuong(int IDCaSau, int IDChuong, int level)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@IDCaSau", IDCaSau);
                param[1] = new SqlParameter("@IDChuong", IDChuong);
                param[2] = new SqlParameter("@level", level);
                param[3] = new SqlParameter("@res", 0);
                param[3].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CoTheNhapChuong", param);
                result = Convert.ToInt32(param[3].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string ChuyenChuong(string strIDCaSau, int Chuong, DateTime ThoiDiemChuyen, int NguoiThayDoi)
        {
            string result;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@Chuong", Chuong);
                param[2] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[3] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[4] = new SqlParameter("@FailList", "");
                param[4].Direction = ParameterDirection.Output;
                param[4].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_ChuyenChuong", param);
                result = param[4].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string ChuyenChuong1(string strIDCaSau, string TenChuong, int So, DateTime ThoiDiemChuyen, int NguoiThayDoi)
        {
            string result;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@TenChuong", TenChuong);
                param[2] = new SqlParameter("@So", So);
                param[3] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[4] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[5] = new SqlParameter("@FailList", "");
                param[5].Direction = ParameterDirection.Output;
                param[5].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_ChuyenChuong1", param);
                result = param[5].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int EditChuyenChuong(int IDBienDong, int IDCaSau, int Chuong, DateTime ThoiDiemChuyen, int User)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                param[1] = new SqlParameter("@IDCaSau", IDCaSau);
                param[2] = new SqlParameter("@Chuong", Chuong);
                param[3] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[4] = new SqlParameter("@User", User);
                param[5] = new SqlParameter("@Res", -1);
                param[5].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSau_EditChuyenChuong", param);
                result = Convert.ToInt32(param[5].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string UpdateBienDongGroup(int IDBienDongGroup, DateTime NewThoiDiemBienDong, string NewNote, int User)
        {
            string result;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@IDBienDongGroup", IDBienDongGroup);
                param[1] = new SqlParameter("@NewThoiDiemBienDong", NewThoiDiemBienDong);
                param[2] = new SqlParameter("@NewNote", NewNote);
                param[3] = new SqlParameter("@User", User);
                param[4] = new SqlParameter("@FailList", "");
                param[4].Direction = ParameterDirection.Output;
                param[4].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_UpdateBienDongGroup", param);
                result = Convert.ToString(param[4].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string UpdateBienDongList(string StrIDBienDong, DateTime NewThoiDiemBienDong, string NewNote, int User)
        {
            string result;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@StrIDBienDong", StrIDBienDong);
                param[1] = new SqlParameter("@NewThoiDiemBienDong", NewThoiDiemBienDong);
                param[2] = new SqlParameter("@NewNote", NewNote);
                param[3] = new SqlParameter("@User", User);
                param[4] = new SqlParameter("@FailList", "");
                param[4].Direction = ParameterDirection.Output;
                param[4].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_UpdateBienDongList", param);
                result = Convert.ToString(param[4].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string ChuyenGioiTinh(string strIDCaSau, int GioiTinh, DateTime ThoiDiemChuyen, int NguoiThayDoi)
        {
            string result;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@GioiTinh", GioiTinh);
                param[2] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[3] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[4] = new SqlParameter("@FailList", "");
                param[4].Direction = ParameterDirection.Output;
                param[4].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_ChuyenGioiTinh", param);
                result = param[4].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int EditChuyenGioiTinh(int IDBienDong, int IDCaSau, int GioiTinh, DateTime ThoiDiemChuyen, int User)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                param[1] = new SqlParameter("@IDCaSau", IDCaSau);
                param[2] = new SqlParameter("@GioiTinh", GioiTinh);
                param[3] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[4] = new SqlParameter("@User", User);
                param[5] = new SqlParameter("@Res", -1);
                param[5].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSau_EditChuyenGioiTinh", param);
                result = Convert.ToInt32(param[5].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string ChuyenLoaiCa(string strIDCaSau, int LoaiCa, DateTime ThoiDiemChuyen, int NguoiThayDoi)
        {
            string result;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@LoaiCa", LoaiCa);
                param[2] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[3] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[4] = new SqlParameter("@FailList", "");
                param[4].Direction = ParameterDirection.Output;
                param[4].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_ChuyenLoaiCa", param);
                result = param[4].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int EditChuyenLoaiCa(int IDBienDong, int IDCaSau, int LoaiCa, DateTime ThoiDiemChuyen, int User)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                param[1] = new SqlParameter("@IDCaSau", IDCaSau);
                param[2] = new SqlParameter("@LoaiCa", LoaiCa);
                param[3] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[4] = new SqlParameter("@User", User);
                param[5] = new SqlParameter("@Res", -1);
                param[5].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSau_EditChuyenLoaiCa", param);
                result = Convert.ToInt32(param[5].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int ChuyenNguonGoc(string strIDCaSau, int NguonGoc)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@NguonGoc", NguonGoc);
                result = DataProvider.ExecuteSP("QLCS_CaSau_ChuyenNguonGoc", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string ChuyenMaSo(string strIDCaSau, string MaSo, DateTime ThoiDiemChuyen, int NguoiThayDoi)
        {
            string result;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@MaSo", MaSo);
                param[2] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[3] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[4] = new SqlParameter("@FailList", "");
                param[4].Direction = ParameterDirection.Output;
                param[4].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_ChuyenMaSo", param);
                result = param[4].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int EditChuyenMaSo(int IDBienDong, int IDCaSau, string MaSo, DateTime ThoiDiemChuyen, int User)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                param[1] = new SqlParameter("@IDCaSau", IDCaSau);
                param[2] = new SqlParameter("@MaSo", MaSo);
                param[3] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[4] = new SqlParameter("@User", User);
                param[5] = new SqlParameter("@Res", -1);
                param[5].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSau_EditChuyenMaSo", param);
                result = Convert.ToInt32(param[5].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string ChuyenTrangThai_CaBan(string strIDCaSau, DateTime ThoiDiemChuyen, int NguoiThayDoi)
        {
            string result;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[2] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[3] = new SqlParameter("@FailList", "");
                param[3].Direction = ParameterDirection.Output;
                param[3].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_ChuyenTrangThai_CaBan", param);
                result = param[3].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int EditChuyenTrangThai_CaBan(int IDBienDong, int IDCaSau, DateTime ThoiDiemChuyen, int User)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                param[1] = new SqlParameter("@IDCaSau", IDCaSau);
                param[2] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[3] = new SqlParameter("@User", User);
                param[4] = new SqlParameter("@Res", -1);
                param[4].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSau_EditChuyenTrangThai_CaBan", param);
                result = Convert.ToInt32(param[4].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string ChuyenTrangThai_CaSong(string strIDCaSau, int TrangThai, DateTime ThoiDiemChuyen, int NguoiThayDoi)
        {
            string result;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@Status", TrangThai);
                param[2] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[3] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[4] = new SqlParameter("@FailList", "");
                param[4].Direction = ParameterDirection.Output;
                param[4].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_ChuyenTrangThai_CaSong", param);
                result = param[4].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int EditChuyenTrangThai_CaSong(int IDBienDong, int IDCaSau, int Status, DateTime ThoiDiemChuyen, int User)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                param[1] = new SqlParameter("@IDCaSau", IDCaSau);
                param[2] = new SqlParameter("@Status", Status);
                param[3] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[4] = new SqlParameter("@User", User);
                param[5] = new SqlParameter("@Res", -1);
                param[5].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSau_EditChuyenTrangThai_CaSong", param);
                result = Convert.ToInt32(param[5].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string ChuyenGiong(string strIDCaSau, int Giong, DateTime ThoiDiemChuyen, int NguoiThayDoi)
        {
            string result;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@Giong", Giong);
                param[2] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[3] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[4] = new SqlParameter("@FailList", "");
                param[4].Direction = ParameterDirection.Output;
                param[4].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_ChuyenGiong", param);
                result = param[4].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int EditChuyenGiong(int IDBienDong, int IDCaSau, int Giong, DateTime ThoiDiemChuyen, int User)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                param[1] = new SqlParameter("@IDCaSau", IDCaSau);
                param[2] = new SqlParameter("@Giong", Giong);
                param[3] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[4] = new SqlParameter("@User", User);
                param[5] = new SqlParameter("@Res", -1);
                param[5].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSau_EditChuyenGiong", param);
                result = Convert.ToInt32(param[5].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int EditChuyenGiong_KhongCanIDCaSau(int IDBienDong, int Giong, DateTime ThoiDiemChuyen, int User)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                param[1] = new SqlParameter("@Giong", Giong);
                param[2] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[3] = new SqlParameter("@User", User);
                param[4] = new SqlParameter("@Res", -1);
                param[4].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSau_EditChuyenGiong_KhongCanIDCaSau", param);
                result = Convert.ToInt32(param[4].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        
        public int ChuyenCaMe(string strIDCaSau, int CaMe)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@CaMe", CaMe);
                result = DataProvider.ExecuteSP("QLCS_CaSau_ChuyenCaMe", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int ChuyenNgayNo(string strIDCaSau, DateTime NgayNo)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@NgayNo", NgayNo);
                result = DataProvider.ExecuteSP("QLCS_CaSau_ChuyenNgayNo", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int ChuyenNgayXuongChuong(string strIDCaSau, DateTime NgayXuongChuong)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@NgayXuongChuong", NgayXuongChuong);
                result = DataProvider.ExecuteSP("QLCS_CaSau_ChuyenNgayXuongChuong", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string CaSauChet(string strIDCaSau, DateTime ThoiDiemChuyen, int NguoiThayDoi, string StrDaBung, string StrDaPhanLoai, string StrDau, string StrPPM, string StrLDC, string StrKL, string BienBan, string Status, string StrVatTu)
        {
            string result;
            try
            {
                SqlParameter[] param = new SqlParameter[13];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[2] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[3] = new SqlParameter("@StrDaBung", StrDaBung);
                param[4] = new SqlParameter("@StrDaPhanLoai", StrDaPhanLoai);
                param[5] = new SqlParameter("@StrDau", StrDau);
                param[6] = new SqlParameter("@StrPPM", StrPPM);
                param[7] = new SqlParameter("@StrLDC", StrLDC);
                param[8] = new SqlParameter("@StrKL", StrKL);
                param[9] = new SqlParameter("@BienBan", BienBan);
                param[10] = new SqlParameter("@Status", Status);
                param[11] = new SqlParameter("@StrVatTu", StrVatTu);
                param[12] = new SqlParameter("@FailList", "");
                param[12].Direction = ParameterDirection.Output;
                param[12].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_Chet", param);
                result = param[12].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string CaSau_Chet_GetCaByLoaiCaByChuongAtDate_Org(int LoaiCa, string Chuong, int SoLuong, DateTime ThoiDiem)
        {
            string result;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@LoaiCa", LoaiCa);
                param[1] = new SqlParameter("@Chuong", Chuong);
                param[2] = new SqlParameter("@SoLuong", SoLuong);
                param[3] = new SqlParameter("@ThoiDiem", ThoiDiem);
                param[4] = new SqlParameter("@Res", "");
                param[4].Direction = ParameterDirection.Output;
                param[4].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_Chet_GetCaByLoaiCaByChuongAtDate_Org", param);
                result = param[4].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string CaSau_Chet_GetCaByLoaiCaByChuongAtDate(int LoaiCa, string TenChuong, int So, int SoLuong, DateTime ThoiDiem, bool Giong)
        {
            string result;
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@LoaiCa", LoaiCa);
                param[1] = new SqlParameter("@TenChuong", TenChuong);
                param[2] = new SqlParameter("@So", So);
                param[3] = new SqlParameter("@SoLuong", SoLuong);
                param[4] = new SqlParameter("@ThoiDiem", ThoiDiem);
                param[5] = new SqlParameter("@Giong", Giong);
                param[6] = new SqlParameter("@Res", "");
                param[6].Direction = ParameterDirection.Output;
                param[6].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_Chet_GetCaByLoaiCaByChuongAtDate", param);
                result = param[6].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int EditCaSauChet(int IDBienDong, int IDCaSau, DateTime ThoiDiemChuyen, int RealDaBung, int RealDaPhanLoai, int RealDau, string RealPPM, int RealLDC, decimal RealKL, int User, string BienBan, string Status, string sVatTu)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                param[1] = new SqlParameter("@IDCaSau", IDCaSau);
                param[2] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[3] = new SqlParameter("@RealDaBung", RealDaBung);
                param[4] = new SqlParameter("@RealDaPhanLoai", RealDaPhanLoai);
                param[5] = new SqlParameter("@RealDau", RealDau);
                param[6] = new SqlParameter("@RealPPM", RealPPM);
                param[7] = new SqlParameter("@RealLDC", RealLDC);
                param[8] = new SqlParameter("@RealKL", RealKL);
                param[9] = new SqlParameter("@User", User);
                param[10] = new SqlParameter("@BienBan", BienBan);
                param[11] = new SqlParameter("@Status", Status);
                param[12] = new SqlParameter("@sVatTu", sVatTu);
                param[13] = new SqlParameter("@Res", -1);
                param[13].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSau_EditChet", param);
                result = Convert.ToInt32(param[13].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int CaSauDelete(string strIDCaSau)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                result = DataProvider.ExecuteSP("QLCS_CaSau_Delete", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int ChuyenDan(DateTime ThoiDiemChuyen, int NguoiThayDoi, int MaxLoaiCa)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[1] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[2] = new SqlParameter("@MaxLoaiCa", MaxLoaiCa);
                param[3] = new SqlParameter("@Res", -1);
                param[3].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSau_ChuyenDan", param);
                result = Convert.ToInt32(param[3].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int ChuyenDanNew(DateTime ThoiDiemChuyen, int NguoiThayDoi, string StrLoaiCa)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@ThoiDiemChuyen", ThoiDiemChuyen);
                param[1] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[2] = new SqlParameter("@StrLoaiCa", StrLoaiCa);
                param[3] = new SqlParameter("@Res", -1);
                param[3].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSau_ChuyenDan_New", param);
                result = Convert.ToInt32(param[3].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadCaSauBienDong(int IDCaSau)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDCaSau", IDCaSau);
                result = DataProvider.SelectSP("QLCS_CaSau_GetCaSauBienDong", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadCaSauBienDong_Delete(int IDCaSau)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDCaSau", IDCaSau);
                result = DataProvider.SelectSP("QLCS_CaSau_GetCaSauBienDong_Delete", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable VatTuBienDong_GetByID(int IDBienDong, int Status)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                param[1] = new SqlParameter("@Status", Status);
                result = DataProvider.SelectSP("QLCS_VatTuBienDong_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int PhucHoiCaBan(string strIDCaSau, int User)
        {
            int result;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrIDCaSau", strIDCaSau);
                param[1] = new SqlParameter("@User", User);
                result = DataProvider.ExecuteSP("QLCS_PhucHoiCaBan", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void DeleteBienDongCaSau(int IDBienDong, int User)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                param[1] = new SqlParameter("@User", User);
                DataProvider.ExecuteSP("QLCS_DeleteBienDong", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteBienDongCaSauGroup(int IDBienDongGroup, int User)
        {
            string result = "";
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@IDBienDongGroup", IDBienDongGroup);
                param[1] = new SqlParameter("@User", User);
                param[2] = new SqlParameter("@FailList", "");
                param[2].Direction = ParameterDirection.Output;
                param[2].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSau_DeleteBienDongGroup", param);
                result = Convert.ToString(param[2].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadCaSauBienDong_DeleteByID(int IDBienDong)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                result = DataProvider.SelectSP("QLCS_CaSau_GetCaSauBienDong_DeleteByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadCaSauBienDongByID(int IDBienDong)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                result = DataProvider.SelectSP("QLCS_CaSau_GetCaSauBienDongByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadActionLogByRefIDByLoaiTable(int RefID, string LoaiTable)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@RefID", RefID);
                param[1] = new SqlParameter("@LoaiTable", LoaiTable);
                result = DataProvider.SelectSP("QLCS_ActionLog_GetByRefIDByLoaiTable", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadThuHoiDaByID(int IDThuHoiDa)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDThuHoiDa", IDThuHoiDa);
                result = DataProvider.SelectSP("QLCS_ThuHoiDa_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadThuHoiDaDeleteByID(int IDThuHoiDa)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDThuHoiDa", IDThuHoiDa);
                result = DataProvider.SelectSP("QLCS_ThuHoiDaDelete_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string TranslateBienDongValue(string type, string oldValue, string newValue)
        {
            string muiten = " ---&gt; ";
            string res = "";
            if (type == "chuyenchuong")
            {
                DataTable tbl = LoadChuongByID(int.Parse(oldValue));
                if (tbl != null && tbl.Rows.Count == 1)
                {
                    res += tbl.Rows[0]["Chuong"].ToString();
                }
                DataTable tbl1 = LoadChuongByID(int.Parse(newValue));
                if (tbl1 != null && tbl1.Rows.Count == 1)
                {
                    res += muiten + tbl1.Rows[0]["Chuong"].ToString();
                }
            }
            else if (type == "chuyengioitinh")
            {
                if (oldValue == "1") res += "Đực";
                else if (oldValue == "-1") res += "CXĐ";
                else if (oldValue == "0") res += "Cái";

                if (newValue == "1") res += muiten + "Đực";
                else if (newValue == "-1") res += muiten + "CXĐ";
                else if (newValue == "0") res += muiten + "Cái";
            }
            else if (type == "chuyennguongoc")
            {
                DataTable tbl = LoadNguonGocByID(int.Parse(oldValue));
                if (tbl != null && tbl.Rows.Count == 1)
                {
                    res += tbl.Rows[0]["TenNguonGoc"].ToString();
                }

                DataTable tbl1 = LoadNguonGocByID(int.Parse(newValue));
                if (tbl1 != null && tbl1.Rows.Count == 1)
                {
                    res += muiten + tbl1.Rows[0]["TenNguonGoc"].ToString();
                }
            }
            else if (type == "chuyenloaica")
            {
                DataTable tbl = LoadLoaiCaByID(int.Parse(oldValue));
                if (tbl != null && tbl.Rows.Count == 1)
                {
                    res += tbl.Rows[0]["TenLoaiCa"].ToString();
                }

                DataTable tbl1 = LoadLoaiCaByID(int.Parse(newValue));
                if (tbl1 != null && tbl1.Rows.Count == 1)
                {
                    res += muiten + tbl1.Rows[0]["TenLoaiCa"].ToString();
                }
            }
            else if (type == "chuyenmaso")
            {
                res += oldValue;
                res += muiten + newValue;
            }
            else if (type == "chuyentrangthai")
            {
                if (oldValue == "0") res += "BT";
                else if (oldValue == "1") res += "Bệnh";
                else if (oldValue == "-4") res += "Loại thải";
                else if (oldValue == "-1") res += "Chết";
                else if (oldValue == "-2") res += "Giết mổ";
                else if (oldValue == "-3") res += "Bán";
                else res += "--";

                if (newValue == "0") res += muiten + "BT";
                else if (newValue == "1") res += muiten + "Bệnh";
                else if (newValue == "-4") res += muiten + "Loại thải";
                else if (newValue == "-1") res += muiten + "Chết";
                else if (newValue == "-2") res += muiten + "Giết mổ";
                else if (newValue == "-3") res += muiten + "Bán";
                else res += muiten + "--";
            }
            else if (type == "chuyengiong")
            {
                if (oldValue == "1") res += "Giống";
                else res += "Tăng trọng";

                if (newValue == "1") res += muiten + "Giống";
                else res += muiten + "Tăng trọng";
            }
            return res;
        }

        public string TranslateBienDongValue(string type, DataRow r)
        {
            string muiten = " ---&gt; ";
            string res = "";
            string oldValue;
            string newValue = r["Note"].ToString();
            if (type == "chuyenchuong")
            {
                oldValue = r["Chuong"].ToString();
                DataTable tbl = LoadChuongByID(int.Parse(oldValue));
                if (tbl != null && tbl.Rows.Count == 1)
                {
                    res += tbl.Rows[0]["Chuong"].ToString();
                }
                DataTable tbl1 = LoadChuongByID(int.Parse(newValue));
                if (tbl1 != null && tbl1.Rows.Count == 1)
                {
                    res += muiten + tbl1.Rows[0]["Chuong"].ToString();
                }
            }
            else if (type == "chuyengioitinh")
            {
                oldValue = r["GioiTinh"].ToString();
                if (oldValue == "1") res += "Đực";
                else if (oldValue == "-1") res += "CXĐ";
                else if (oldValue == "0") res += "Cái";

                if (newValue == "1") res += muiten + "Đực";
                else if (newValue == "-1") res += muiten + "CXĐ";
                else if (newValue == "0") res += muiten + "Cái";
            }
            else if (type == "chuyennguongoc")
            {
                oldValue = r["NguonGoc"].ToString();
                DataTable tbl = LoadNguonGocByID(int.Parse(oldValue));
                if (tbl != null && tbl.Rows.Count == 1)
                {
                    res += tbl.Rows[0]["TenNguonGoc"].ToString();
                }

                DataTable tbl1 = LoadNguonGocByID(int.Parse(newValue));
                if (tbl1 != null && tbl1.Rows.Count == 1)
                {
                    res += muiten + tbl1.Rows[0]["TenNguonGoc"].ToString();
                }
            }
            else if (type == "chuyenloaica")
            {
                oldValue = r["LoaiCa"].ToString();
                DataTable tbl = LoadLoaiCaByID(int.Parse(oldValue));
                if (tbl != null && tbl.Rows.Count == 1)
                {
                    res += tbl.Rows[0]["TenLoaiCa"].ToString();
                }

                DataTable tbl1 = LoadLoaiCaByID(int.Parse(newValue));
                if (tbl1 != null && tbl1.Rows.Count == 1)
                {
                    res += muiten + tbl1.Rows[0]["TenLoaiCa"].ToString();
                }
            }
            else if (type == "chuyenmaso")
            {
                oldValue = r["MaSo"].ToString();
                res += oldValue;
                res += muiten + newValue;
            }
            else if (type == "chuyentrangthai")
            {
                oldValue = r["Status"].ToString();
                if (oldValue == "0") res += "BT";
                else if (oldValue == "1") res += "Bệnh";
                else if (oldValue == "-4") res += "Loại thải";
                else if (oldValue == "-1") res += "Chết";
                else if (oldValue == "-2") res += "Giết mổ";
                else if (oldValue == "-3") res += "Bán";
                else res += "--";

                if (newValue == "0") res += muiten + "BT";
                else if (newValue == "1") res += muiten + "Bệnh";
                else if (newValue == "-4") res += muiten + "Loại thải";
                else if (newValue == "-1") res += muiten + "Chết";
                else if (newValue == "-2") res += muiten + "Giết mổ";
                else if (newValue == "-3") res += muiten + "Bán";
                else res += muiten + "--";
            }
            else if (type == "chuyengiong")
            {
                oldValue = r["Giong"].ToString();
                if (oldValue == "1") res += "Giống";
                else res += "Tăng trọng";

                if (newValue == "1") res += muiten + "Giống";
                else res += muiten + "Tăng trọng";
            }
            return res;
        }

        public string TranslateThuHoiDaValue(string v1, string v2, string v3, string v4, string v5, string v6, string v7, string v8)
        {
            string LyDoChet = "";
            try
            {
                DataTable dtLyDoChet = LoadLyDoChetByID(int.Parse(v5));
                LyDoChet = dtLyDoChet.Rows[0]["TenLyDoChet"].ToString();
            }
            catch (Exception)
            {
            }
            string res = "";
            if (int.Parse(v1) > 0)
            {
                if (v3 == "1")
                {
                    res = "Kích thước da: " + v1 + " cm, loại: " + v2 + ", PPM: " + v4 + ", có lấy đầu, lý do chết: " + LyDoChet + ", khối lượng lúc chết: " + v6 + " kg, biên bản: " + v7 + ", ngày chết: " + v8;
                }
                else if (v3 == "0")
                {
                    res = "Kích thước da: " + v1 + " cm, loại: " + v2 + ", PPM: " + v4 + ", không lấy đầu, lý do chết: " + LyDoChet + ", khối lượng lúc chết: " + v6 + " kg, biên bản: " + v7 + ", ngày chết: " + v8;
                }
            }
            else
            {
                if (v3 == "1")
                {
                    res = "Không lấy da, có lấy đầu, lý do chết: " + LyDoChet + ", khối lượng lúc chết: " + v6 + " kg, biên bản: " + v7 + ", ngày chết: " + v8;
                }
                else if (v3 == "0")
                {
                    res = "Không lấy da, không lấy đầu, lý do chết: " + LyDoChet + ", khối lượng lúc chết: " + v6 + " kg, biên bản: " + v7 + ", ngày chết: " + v8;
                }
            }
            return res;
        }

        public string TranslateThuHoiDaValue(DataRow r)
        {
            string res = "";
            string v1 = r["Da_Bung"].ToString();
            string v2 = r["Da_PhanLoai"].ToString();
            string v3 = r["Dau"].ToString();
            string v4 = r["Note"].ToString();
            string v5 = r["LyDoChet"].ToString();
            string LyDoChet = "";
            try
            {
                DataTable dtLyDoChet = LoadLyDoChetByID(int.Parse(v5));
                LyDoChet = dtLyDoChet.Rows[0]["TenLyDoChet"].ToString();
            }
            catch (Exception)
            {
            }
            
            string v6 = r["KhoiLuong"].ToString();
            string v7 = r["BienBan"].ToString();
            string v8 = Convert.ToDateTime(r["Ngay"]).ToString("dd/MM/yyyy HH:mm:ss");
            if (int.Parse(v1) > 0)
            {
                if (v3 == "1")
                {

                    res = "Kích thước da: " + v1 + " cm, loại: " + v2 + ", PPM: " + v4 + ", có lấy đầu, lý do chết: " + LyDoChet + ", khối lượng lúc chết: " + v6 + " kg, biên bản: " + v7 + ", ngày chết: " + v8;
                }
                else if (v3 == "0")
                {
                    res = "Kích thước da: " + v1 + " cm, loại: " + v2 + ", PPM: " + v4 + ", không lấy đầu, lý do chết: " + LyDoChet + ", khối lượng lúc chết: " + v6 + " kg, biên bản: " + v7 + ", ngày chết: " + v8;
                }
            }
            else
            {
                if (v3 == "1")
                {
                    res = "Không lấy da, có lấy đầu, lý do chết: " + LyDoChet + ", khối lượng lúc chết: " + v6 + " kg, biên bản: " + v7 + ", ngày chết: " + v8;
                }
                else if (v3 == "0")
                {
                    res = "Không lấy da, không lấy đầu, lý do chết: " + LyDoChet + ", khối lượng lúc chết: " + v6 + " kg, biên bản: " + v7 + ", ngày chết: " + v8;
                }
            }
            return res;
        }

        public string TranslateTheoDoiDeValue(string v, System.Globalization.CultureInfo ci)
        {
            if (v == "") return "";
            System.Globalization.CultureInfo eci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            string thailoai = "";
            string res = "";
            string[] a = v.Split(new char[] { '#' }, StringSplitOptions.None);
            
            string[] b = a[0].Split(new char[] { '@' }, StringSplitOptions.None);
            if (b[1] == "1")
            {
                res = "Đã nở, ";
            }
            else if (b[1] == "0")
            {
                res = "Chưa nở, ";
            }
            else if (b[1] == "-1")
            {
                res = "Không ấp, ";
            }
            string CaMe, PA, KA, KU = "";
            TheoDoiDe_GetProperties(b[0], b[2], b[4], b[13], out CaMe, out PA, out KA, out KU);
            res += "Cá mẹ: " + CaMe + ", ấp ngày " + b[3] + ", PA: " + PA + ", KA: " + KA + ", TLTBQ: " + b[5] + ", Đẻ: " + b[6] + ", Vỡ: " + b[7] + ", Thải: " + b[8] + "@@" + ", KP: " + b[9]
             + ", CP1: " + b[10] + ", CP2: " + b[11];
            if (b[1] == "1")
            {
                res += ", Ngày nở: " + b[12] + ", KU: " + KU + ", TLCBQ: " + b[14] + ", CDBQ: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(b[15],eci)) + ", VBBQ: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(b[16],eci));
            }
            
            if(a[1] != "")
            {
                thailoai = " (";
                thailoai += TheoDoiDe_GetThaiLoaiTrungString(a[1]);
                //string t1 = a[1].Substring(1, a[1].Length - 2);
                //string[] t2 = t1.Split(new string[] { "!!" }, StringSplitOptions.None);
                //foreach (string t in t2)
                //{
                //    string[] c = t.Split(new char[] { '@' }, StringSplitOptions.None);
                //    thailoai += c[0] + ": " + c[1] + ", ";
                //}
                //thailoai = thailoai.Substring(0, thailoai.Length - 2);
                thailoai += ")";
            }
            res = res.Replace("@@", thailoai);

            if (a[2] != "")
            {
                res += ", NV thu trứng: ";
                string StrID = "@" + a[2].Replace(", ", "@@").Remove(a[2].Length - 1);
                string NhanVien = GetNhanVienFromStrID(StrID);
                res += NhanVien;
                //string r1 = a[2].Substring(1, a[2].Length - 2);
                //string[] d = r1.Split(new string[] { "@@" }, StringSplitOptions.None);
                //foreach (string r in d)
                //{
                //    res += r + ", ";
                //}
                //res = res.Substring(0, res.Length - 2);
            }
            return res;
        }

        public string TranslateTheoDoiDeValue(DataRow r, System.Globalization.CultureInfo ci)
        {
            string v = r["CaMe1"].ToString() + "@"
            + r["Status"].ToString() + "@" + r["TenPhongAp"].ToString() + "@"
            + Convert.ToDateTime(r["NgayVaoAp"]).ToString("dd/MM/yyyy HH:mm:ss") + "@"
            + r["TenKhayAp"].ToString() + "@" + r["TrongLuongTrungBQ"].ToString() + "@"
            + r["TrungDe"].ToString() + "@" + r["TrungVo"].ToString() + "@"
            + r["TrungThaiLoai"].ToString() + "@" + r["TrungKhongPhoi"].ToString() + "@"
            + r["TrungChetPhoi1"].ToString() + "@" + r["TrungChetPhoi2"].ToString() + "@";
            if (r["NgayNo"] != DBNull.Value)
                v += Convert.ToDateTime(r["NgayNo"]).ToString("dd/MM/yyyy HH:mm:ss");
            v += "@" + r["TenKhayUm"].ToString() + "@"
            + r["TrongLuongConBQ"].ToString() + "@" + r["ChieuDaiBQ"].ToString() + "@"
            + r["VongBungBQ"].ToString()
            + "#" + r["ThaiLoaiTrung"].ToString()
            + "#" + r["TenNhanVien"].ToString();
            string thailoai = "";
            string res = "";
            string[] a = v.Split(new char[] { '#' }, StringSplitOptions.None);

            string[] b = a[0].Split(new char[] { '@' }, StringSplitOptions.None);
            if (b[1] == "1")
            {
                res = "Đã nở, ";
            }
            else if (b[1] == "0")
            {
                res = "Chưa nở, ";
            }
            else if (b[1] == "-1")
            {
                res = "Không ấp, ";
            }
            res += "Cá mẹ: " + b[0] + ", ấp ngày " + b[3] + ", PA: " + b[2] + ", KA: " + b[4] + ", TLTBQ: " + b[5] + ", Đẻ: " + b[6] + ", Vỡ: " + b[7] + ", Thải: " + b[8] + "@@" + ", KP: " + b[9]
             + ", CP1: " + b[10] + ", CP2: " + b[11];
            if (b[1] == "1")
            {
                res += ", Ngày nở: " + b[12] + ", KU: " + b[13] + ", TLCBQ: " + b[14] + ", CDBQ: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(b[15])) + ", VBBQ: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(b[16]));
            }

            if (a[1] != "")
            {
                thailoai = " (";
                string t1 = a[1].Substring(1, a[1].Length - 2);
                string[] t2 = t1.Split(new string[] { "!!" }, StringSplitOptions.None);
                foreach (string t in t2)
                {
                    string[] c = t.Split(new char[] { '@' }, StringSplitOptions.None);
                    thailoai += c[0] + ": " + c[1] + ", ";
                }
                thailoai = thailoai.Substring(0, thailoai.Length - 2);
                thailoai += ")";
            }
            res = res.Replace("@@", thailoai);

            if (a[2] != "")
            {
                res += ", NV thu trứng: ";
                string r1 = a[2].Substring(1, a[2].Length - 2);
                string[] r2 = r1.Split(new string[] { "@@" }, StringSplitOptions.None);
                foreach (string d in r2)
                {
                    res += d + ", ";
                }
                res = res.Substring(0, res.Length - 2);
            }
            return res;
        }

        public string TranslateCaSauAnThucAnValue(string v, System.Globalization.CultureInfo ci)
        {
            System.Globalization.CultureInfo eci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            string res = "";
            string[] a = v.Split(new char[] { '#' }, StringSplitOptions.None);
            string[] b = a[0].Split(new char[] { '@' }, StringSplitOptions.None);
            res += "Khối lượng: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(b[0],eci)) + ", số cá ăn: " + b[1];
            string StrID = "";
            if(a[1] != "")
            {
                StrID = "@" + a[1].Replace(",", "@@").Remove(a[1].Length);
            }
            string NhanVien = GetNhanVienFromStrID_CoKhuChuong(StrID);
            if (NhanVien != "") res += ", NV cho ăn: " + NhanVien;
            return res;
        }

        public string TranslateCaSauAnThucAnValue(DataRow r, System.Globalization.CultureInfo ci)
        {
            string res = "";
            res += "Khối lượng: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(r["KhoiLuong"])) + ", số cá ăn: " + r["SoLuongCa"].ToString();
            string NhanVien = "";
            if (r["NhanVien"] != "")
            {
                NhanVien = ", NV cho ăn: ";
                string r1 = r["NhanVien"].ToString().Substring(1, r["NhanVien"].ToString().Length - 2);
                string[] r2 = r1.Split(new string[] { "@@" }, StringSplitOptions.None);
                foreach (string d in r2)
                {
                    NhanVien += d + ", ";
                }
                NhanVien = NhanVien.Substring(0, NhanVien.Length - 2);
            }
            res += NhanVien;
            return res;
        }

        public string TranslateCaSauAnThuocValue(string v, System.Globalization.CultureInfo ci)
        {
            System.Globalization.CultureInfo eci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            string res = "";
            string[] b = v.Split(new char[] { '@' }, StringSplitOptions.None);
            res += "Khối lượng: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(b[0],eci)) + ", số cá ăn: " + b[1];
            return res;
        }

        public string TranslateCaSauAnThuocValue(DataRow r, System.Globalization.CultureInfo ci)
        {
            string res = "";
            res += "Khối lượng: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(r["KhoiLuong"])) + ", số cá ăn: " + r["SoLuongCa"].ToString();
            return res;
        }

        public string TranslateGietMoCaValue(string v)
        {
            string res = "";
            string[] b = v.Split(new char[] { '@' }, StringSplitOptions.None);
            res += "Ngày mổ: " + b[0] + ", số biên bản: " + b[1];
            return res;
        }

        public string TranslateGietMoCaValue(DataRow r)
        {
            string res = "";
            res += "Ngày mổ: " + Convert.ToDateTime(r["NgayMo"]).ToString("dd/MM/yyyy") + ", số biên bản: " + r["BienBan"].ToString();
            return res;
        }

        public string TranslateGietMoCaChiTietValue(string v)
        {
            System.Globalization.CultureInfo eci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            string res = "";
            string[] b = v.Split(new char[] { '@' }, StringSplitOptions.None);
            CaSauController csCont = new CaSauController();
            DataTable tblNhanVien = csCont.LoadNhanVienByID(int.Parse(b[4]));
            string DiTat = "Không dị tât";
            if (b[7] == "-4") DiTat = "Dị tât";
            if (tblNhanVien.Rows.Count > 0)
            {
                res += "TL da: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(b[0], eci)) + ", Kích thước da: " + b[1] + ", Loại: " + b[2] + ", Đầu: " + b[3] + ", Người mổ: " + tblNhanVien.Rows[0]["TenNhanVien"].ToString() + ", Hơi: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(b[5], eci)) + ", Móc hàm: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(b[6], eci)) + ", PPM: " + b[8] + ", " + DiTat;
            }
            else
            {
                res += "TL da: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(b[0], eci)) + ", Kích thước da: " + b[1] + ", Loại: " + b[2] + ", Đầu: " + b[3] + ", Người mổ: , Hơi: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(b[5], eci)) + ", Móc hàm: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(b[6], eci)) + ", PPM: " + b[8] + ", " + DiTat;
            }
            return res;
        }

        public string TranslateGietMoCaChiTietValue(DataRow r)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            string res = "";
            string DiTat = "Không dị tât";
            if (Convert.ToInt32(r["Status"]) == -4) DiTat = "Dị tât";
            res += "TL da: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(r["Da_TrongLuong"])) + ", Kích thước da: " + r["Da_Bung"].ToString() + ", Loại: " + r["Da_PhanLoai"].ToString() + ", Đầu: " + r["Dau"].ToString() + ", Người mổ: " + r["TenNguoiMo"].ToString() + ", Hơi: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(r["TrongLuongHoi"])) + ", Móc hàm: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(r["TrongLuongMocHam"])) + ", PPM: " + r["PhuongPhapMo"].ToString() + ", " + DiTat;
            return res;
        }

        public string TranslateVatTuBienDongValue(string v)
        {
            System.Globalization.CultureInfo eci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            string res = "";
            string[] b = v.Split(new char[] { '@' }, StringSplitOptions.None);
            res += "Số lượng: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(b[0],eci)) + ", ngày biến động: " + b[1];
            if (b.Length == 3) res += ", Ghi chú: " + b[2];
            return res;
        }

        public string TranslateVatTuBienDongValue(DataRow r)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            string res = "";
            res += "Số lượng: " + String.Format(ci, "{0:0.#####}", Convert.ToDecimal(r["SoLuongBienDong"])) + ", ngày biến động: " + Convert.ToDateTime(r["NgayBienDong"]).ToString("dd/MM/yyyy") + ", ghi chú: " + r["Note"].ToString();
            return res;
        }

        public string TranslateNoteValue(string v)
        {
            CaSauController csCont = new CaSauController();
            string res = "";
            string[] b = v.Split(new char[] { '@' }, StringSplitOptions.None);
            DataTable tblChuong = csCont.LoadChuongByID(int.Parse(b[1]));
            if (tblChuong.Rows.Count > 0)
            {
                res += "Ngày: " + b[0] + ", chuồng: " + tblChuong.Rows[0]["Chuong"].ToString() + ", ghi chú: " + b[2];
            }
            else
            {
                res += "Ngày: " + b[0] + ", ghi chú: " + b[2];
            }
            return res;
        }

        public string TranslateNoteValue(DataRow r)
        {
            CaSauController csCont = new CaSauController();
            string res = "";
            DataTable tblChuong = csCont.LoadChuongByID(Convert.ToInt32(r["Chuong"]));
            if (tblChuong.Rows.Count > 0)
            {
                res += "Ngày: " + Convert.ToDateTime(r["Ngay"]).ToString("dd/MM/yyyy") + ", chuồng: " + tblChuong.Rows[0]["Chuong"].ToString() + ", ghi chú: " + r["Note"].ToString();
            }
            else
            {
                res += "Ngày: " + Convert.ToDateTime(r["Ngay"]).ToString("dd/MM/yyyy") + ", ghi chú: " + r["Note"].ToString();
            }
            return res;
        }

        public DataTable GetDanhSachCaSauBienDongByBienDongGroup(int IDBienDongGroup)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDBienDongGroup", IDBienDongGroup);
                result = DataProvider.SelectSP("QLCS_GetDanhSachCaSauBienDongByBienDongGroup", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /*Ca Sau De======================================================================================================================*/
        public DataTable GetAvailableKhayUm(DateTime Date, int IDTheoDoiDe)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Date", Date);
                param[1] = new SqlParameter("@IDTheoDoiDe", IDTheoDoiDe);
                result = DataProvider.SelectSP("QLCS_TheoDoiDe_GetAvailableKhayUm", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable GetAvailableKhayAp(DateTime Date, int IDTheoDoiDe)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Date", Date);
                param[1] = new SqlParameter("@IDTheoDoiDe", IDTheoDoiDe);
                result = DataProvider.SelectSP("QLCS_TheoDoiDe_GetAvailableKhayAp", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string TheoDoiDe_GetThaiLoaiTrungString(string StrThaiLoaiTrung)
        {
            string StrOut = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrThaiLoaiTrung", StrThaiLoaiTrung);
                param[1] = new SqlParameter("@StrOut", StrOut);
                param[1].Size = 4000;
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_TheoDoiDe_GetThaiLoaiTrungString", param);
                StrOut = param[1].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StrOut;
        }

        public void TheoDoiDe_GetProperties(string IDCaMe, string IDPA, string IDKA, string IDKU, out string CaMe, out string PA, out string KA, out string KU)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@IDCaMe", IDCaMe);
                param[1] = new SqlParameter("@IDPA", IDPA);
                param[2] = new SqlParameter("@IDKA", IDKA);
                param[3] = new SqlParameter("@IDKU", IDKU);
                param[4] = new SqlParameter("@CaMe", "");
                param[4].Size = 4000;
                param[4].Direction = ParameterDirection.Output;
                param[5] = new SqlParameter("@PA", "");
                param[5].Size = 4000;
                param[5].Direction = ParameterDirection.Output;
                param[6] = new SqlParameter("@KA", "");
                param[6].Size = 4000;
                param[6].Direction = ParameterDirection.Output;
                param[7] = new SqlParameter("@KU", "");
                param[7].Size = 4000;
                param[7].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_TheoDoiDe_GetProperties", param);
                CaMe = param[4].Value.ToString();
                PA = param[5].Value.ToString();
                KA = param[6].Value.ToString();
                KU = param[7].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable LoadPageCaSauDe(string WhereClause, string OrderBy, int StartIndex, int PageSize)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WhereClause", WhereClause);                             
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                param[2] = new SqlParameter("@StartIndex", StartIndex);
                param[3] = new SqlParameter("@PageSize", PageSize);
                result = DataProvider.SelectSP("QLCS_GetCaSauDe_Page", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int CountCaSauDe(string WhereClause)
        {
            int TotalRecords = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@TotalRecords", TotalRecords);
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetCaSauDe_Count", param);
                TotalRecords = Convert.ToInt32(param[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TotalRecords;
        }

        public DataTable LoadPageCaSauDeDelete(string WhereClause, string OrderBy, int StartIndex, int PageSize)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                param[2] = new SqlParameter("@StartIndex", StartIndex);
                param[3] = new SqlParameter("@PageSize", PageSize);
                result = DataProvider.SelectSP("QLCS_GetCaSauDe_Delete_Page", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int CountCaSauDeDelete(string WhereClause)
        {
            int TotalRecords = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@TotalRecords", TotalRecords);
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetCaSauDe_Delete_Count", param);
                TotalRecords = Convert.ToInt32(param[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TotalRecords;
        }

        public DataTable LoadTheoDoiDeByID(int IDTheoDoiDe)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDTheoDoiDe", IDTheoDoiDe);
                result = DataProvider.SelectSP("QLCS_TheoDoiDe_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadTheoDoiDeDeleteByID(int IDTheoDoiDe)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDTheoDoiDe", IDTheoDoiDe);
                result = DataProvider.SelectSP("QLCS_TheoDoiDeDelete_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadTheoDoiDeByIDCaSau(int IDCaSau)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDCaSau", IDCaSau);
                result = DataProvider.SelectSP("QLCS_TheoDoiDe_GetByIDCaSau", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable TheoDoiDe_GetThaiLoaiTrung(int IDTheoDoiDe)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDTheoDoiDe", IDTheoDoiDe);
                result = DataProvider.SelectSP("QLCS_TheoDoiDe_GetThaiLoaiTrung", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int InsertTheoDoiDe(TheoDoiDeInfo tdd, int UserID, string arrLyDoThaiLoaiTrung, string arrSoLuong, string arrNhanVien, out int IDTDD)
        {
            IDTDD = 0;
            int result = 0;
            string StrNgayNo = null;
            if (tdd.NgayNo != null) StrNgayNo = tdd.NgayNo.Value.ToString("dd/MM/yyyy HH:mm:ss");
            try
            {
                SqlParameter[] param = new SqlParameter[24];
                param[0] = new SqlParameter("@CaMe", tdd.CaMe);
                param[1] = new SqlParameter("@NgayVaoAp", tdd.NgayVaoAp);
                param[2] = new SqlParameter("@KhayAp", tdd.KhayAp);
                param[3] = new SqlParameter("@TrongLuongTrungBQ", tdd.TrongLuongTrungBQ);
                param[4] = new SqlParameter("@TrungDe", tdd.TrungDe);
                param[5] = new SqlParameter("@TrungVo", tdd.TrungVo);
                param[6] = new SqlParameter("@TrungKhongPhoi", tdd.TrungKhongPhoi);
                param[7] = new SqlParameter("@TrungChetPhoi1", tdd.TrungChetPhoi1);
                param[8] = new SqlParameter("@TrungChetPhoi2", tdd.TrungChetPhoi2);
                param[9] = new SqlParameter("@NgayNo", tdd.NgayNo);
                param[10] = new SqlParameter("@KhayUm", tdd.KhayUm);
                param[11] = new SqlParameter("@TrongLuongConBQ", tdd.TrongLuongConBQ);
                param[12] = new SqlParameter("@ChieuDaiBQ", tdd.ChieuDaiBQ);
                param[13] = new SqlParameter("@VongBungBQ", tdd.VongBungBQ);
                param[14] = new SqlParameter("@Chet1_30Ngay", tdd.Chet1_30Ngay);
                param[15] = new SqlParameter("@Status", tdd.Status);
                param[16] = new SqlParameter("@arrLyDoThaiLoaiTrung", arrLyDoThaiLoaiTrung);
                param[17] = new SqlParameter("@arrSoLuong", arrSoLuong);
                param[18] = new SqlParameter("@NguoiThucHien", UserID);
                param[19] = new SqlParameter("@StrNgayNo", StrNgayNo);
                param[20] = new SqlParameter("@PhongAp", tdd.PhongAp);
                param[21] = new SqlParameter("@arrNhanVien", arrNhanVien);
                param[22] = new SqlParameter("@Note", tdd.Note);
                param[23] = new SqlParameter("@IDTDD", IDTDD);
                param[23].Direction = ParameterDirection.Output;
                result = DataProvider.ExecuteSP("QLCS_TheoDoiDe_Insert", param);
                IDTDD = Convert.ToInt32(param[23].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int UpdateTheoDoiDe(TheoDoiDeInfo tdd, int UserID, int TrungNo, string arrLyDoThaiLoaiTrung, string arrSoLuong, string arrNhanVien)
        {
            int result = 0;
            string StrNgayNo = null;
            if (tdd.NgayNo != null) StrNgayNo = tdd.NgayNo.Value.ToString("dd/MM/yyyy HH:mm:ss");
            try
            {
                SqlParameter[] param = new SqlParameter[27];
                param[0] = new SqlParameter("@ID", tdd.ID);
                param[1] = new SqlParameter("@CaMe", tdd.CaMe);
                param[2] = new SqlParameter("@NgayVaoAp", tdd.NgayVaoAp);
                param[3] = new SqlParameter("@KhayAp", tdd.KhayAp);
                param[4] = new SqlParameter("@TrongLuongTrungBQ", tdd.TrongLuongTrungBQ);
                param[5] = new SqlParameter("@TrungDe", tdd.TrungDe);
                param[6] = new SqlParameter("@TrungVo", tdd.TrungVo);
                param[7] = new SqlParameter("@TrungThaiLoai", tdd.TrungThaiLoai);
                param[8] = new SqlParameter("@TrungKhongPhoi", tdd.TrungKhongPhoi);
                param[9] = new SqlParameter("@TrungChetPhoi1", tdd.TrungChetPhoi1);
                param[10] = new SqlParameter("@TrungChetPhoi2", tdd.TrungChetPhoi2);
                param[11] = new SqlParameter("@NgayNo", tdd.NgayNo);
                param[12] = new SqlParameter("@KhayUm", tdd.KhayUm);
                param[13] = new SqlParameter("@TrongLuongConBQ", tdd.TrongLuongConBQ);
                param[14] = new SqlParameter("@ChieuDaiBQ", tdd.ChieuDaiBQ);
                param[15] = new SqlParameter("@VongBungBQ", tdd.VongBungBQ);
                param[16] = new SqlParameter("@Chet1_30Ngay", tdd.Chet1_30Ngay);
                param[17] = new SqlParameter("@Status", tdd.Status);
                param[18] = new SqlParameter("@arrLyDoThaiLoaiTrung", arrLyDoThaiLoaiTrung);
                param[19] = new SqlParameter("@arrSoLuong", arrSoLuong);
                param[20] = new SqlParameter("@NguoiThucHien", UserID);
                param[21] = new SqlParameter("@StrNgayNo", StrNgayNo);
                param[22] = new SqlParameter("@TrungNo", TrungNo);
                param[23] = new SqlParameter("@PhongAp", tdd.PhongAp);
                param[24] = new SqlParameter("@arrNhanVien", arrNhanVien);
                param[25] = new SqlParameter("@Note", tdd.Note);
                param[26] = new SqlParameter("@Res", 0);
                param[26].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_TheoDoiDe_Update", param);
                result = Convert.ToInt32(param[26].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int TheoDoiDe_Delete(int IDTheoDoiDe, int User)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@IDTheoDoiDe", IDTheoDoiDe);
                param[1] = new SqlParameter("@User", User);
                param[2] = new SqlParameter("@Res", 0);
                param[2].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_TheoDoiDe_Delete", param);
                result = Convert.ToInt32(param[2].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void UpdateThaiLoaiTrung(int IDTheoDoiDe, int IDLyDoThaiLoaiTrung, int SoLuong, string Status)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@IDTheoDoiDe", IDTheoDoiDe);
                param[1] = new SqlParameter("@IDLyDoThaiLoaiTrung", IDLyDoThaiLoaiTrung);
                param[2] = new SqlParameter("@SoLuong", SoLuong);
                param[3] = new SqlParameter("@Status", Status);
                DataProvider.ExecuteSP("QLCS_TheoDoiDe_UpdateThaiLoaiTrung", param);
            }
            catch (Exception ex)
            {
            }
        }

        public DataTable TheoDoiDe_GetRaChuongInfo(int IDTheoDoiDe)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDTheoDoiDe", IDTheoDoiDe);
                result = DataProvider.SelectSP("QLCS_TheoDoiDe_GetRaChuongInfo", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable TheoDoiDe_GetCaDeByLoaiCaByChuongByViCat(string MaSo, int LoaiCa, string TenChuong, int So, DateTime NgayAp)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@MaSo", MaSo);
                param[1] = new SqlParameter("@LoaiCa", LoaiCa);
                param[2] = new SqlParameter("@TenChuong", TenChuong);
                param[3] = new SqlParameter("@So", So);
                param[4] = new SqlParameter("@NgayAp", NgayAp);
                result = DataProvider.SelectSP("QLCS_TheoDoiDe_GetCaDeByLoaiCaByChuongByViCat", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        //public int CaSauCon_RaChuong(int SoLuong, string MaSo, int GioiTinh, bool Giong, DateTime NgayNo, DateTime NgayXuongChuong, int NguonGoc
        //        , int Chuong, int CaMe, int tddID, int tddStatus, int NguoiThayDoi)
        //{
        //    int result = 0;
        //    try
        //    {
        //        SqlParameter[] param = new SqlParameter[13];
        //        param[0] = new SqlParameter("@SoLuong", SoLuong);
        //        param[1] = new SqlParameter("@MaSo", MaSo);
        //        param[2] = new SqlParameter("@GioiTinh", GioiTinh);
        //        param[3] = new SqlParameter("@Giong", Giong);
        //        param[4] = new SqlParameter("@NgayNo", NgayNo);
        //        param[5] = new SqlParameter("@NgayXuongChuong", NgayXuongChuong);
        //        param[6] = new SqlParameter("@NguonGoc", NguonGoc);
        //        param[7] = new SqlParameter("@Chuong", Chuong);
        //        param[8] = new SqlParameter("@CaMe", CaMe);
        //        param[9] = new SqlParameter("@IDTheoDoiDe", tddID);
        //        param[10] = new SqlParameter("@TheoDoiDeStatus", tddStatus);
        //        param[11] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
        //        param[12] = new SqlParameter("@StrNgayXuongChuong", NgayXuongChuong.ToString("dd/MM/yyyy"));
        //        result = DataProvider.ExecuteSP("QLCS_CaSauCon_RaChuong", param);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return result;
        //}

        //public int CaSauCon_VaoNhaUm(int SoLuong, DateTime NgayNo, int KhayUm, int CaMe, int NguoiThayDoi)
        //{
        //    int result = 0;
        //    try
        //    {
        //        SqlParameter[] param = new SqlParameter[6];
        //        param[0] = new SqlParameter("@SoLuong", SoLuong);
        //        param[1] = new SqlParameter("@NgayNo", NgayNo);
        //        param[2] = new SqlParameter("@KhayUm", KhayUm);
        //        param[3] = new SqlParameter("@CaMe", CaMe);
        //        param[4] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
        //        param[5] = new SqlParameter("@StrNgayXuongChuong", NgayNo.ToString("dd/MM/yyyy HH:mm:ss"));
        //        result = DataProvider.ExecuteSP("QLCS_CaSauCon_VaoNhaUm", param);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return result;
        //}

        /*Vat Tu=========================================================================================================================*/

        /*
        public DataTable LoadVatTuByLVT(string LVT)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@LVT", LVT);
                result = DataProvider.SelectSP("QLCS_VatTu_GetVatTuByLVT", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        */

        public DataTable LoadVatTu(string LVT)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@LoaiVatTu", LVT);
                result = DataProvider.SelectSP("QLCS_VatTu_GetDanhMuc", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadVatTu_HienTai(string LVT, string OrderBy)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@LVT", LVT);
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                result = DataProvider.SelectSP("QLCS_VatTu_GetVatTu_HienTai", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public decimal LoadVatTu_Ngay_ByID(int VatTuID, DateTime Ngay, out string DonViTinh)
        {
            decimal result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@VatTuID", VatTuID);
                param[1] = new SqlParameter("@Ngay", Ngay);
                param[2] = new SqlParameter("@SoLuong", 0);
                param[2].Direction = ParameterDirection.Output;
                param[2].Scale = 5;
                param[2].SqlDbType = SqlDbType.Decimal;
                param[2].DbType = DbType.Decimal;
                param[3] = new SqlParameter("@DonViTinh", "");
                param[3].Direction = ParameterDirection.Output;
                param[3].Size = 20;
                DataProvider.ExecuteSP("QLCS_VatTu_GetVatTu_Ngay_ByID", param);
                result = Convert.ToDecimal(param[2].Value);
                DonViTinh = param[3].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadVatTu_ConTonKho(string LVT, DateTime ThoiDiem)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@LVT", LVT);
                param[1] = new SqlParameter("@ThoiDiem", ThoiDiem);
                result = DataProvider.SelectSP("QLCS_VatTu_GetVatTu_ConTonKho", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadVatTu_HienTaiByID(int VatTuID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@VatTuID", VatTuID);
                result = DataProvider.SelectSP("QLCS_VatTu_GetVatTu_HienTaiByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int VatTu_ThucHienBienDong(int VatTu, decimal SoLuongBienDong, int LoaiBienDong, DateTime ThoiDiemBienDong, int NguoiThayDoi)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@VatTu", VatTu);
                param[1] = new SqlParameter("@SoLuongBienDong", SoLuongBienDong);
                param[2] = new SqlParameter("@LoaiBienDong", LoaiBienDong);
                param[3] = new SqlParameter("@ThoiDiemBienDong", ThoiDiemBienDong);
                param[4] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[5] = new SqlParameter("@Time", DateTime.Now);
                result = DataProvider.ExecuteSP("QLCS_VatTu_ThucHienBienDong", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int VatTu_ThemBienDong(int VatTu, decimal SoLuongBienDong, int LoaiBienDong, string Note, DateTime ThoiDiemBienDong, int NguoiThayDoi, int NhaCungCap)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@VatTu", VatTu);
                param[1] = new SqlParameter("@SoLuongBienDong", SoLuongBienDong);
                param[2] = new SqlParameter("@LoaiBienDong", LoaiBienDong);
                param[3] = new SqlParameter("@Note", Note);
                param[4] = new SqlParameter("@ThoiDiemBienDong", ThoiDiemBienDong);
                param[5] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[6] = new SqlParameter("@Time", DateTime.Now);
                param[7] = new SqlParameter("@NhaCungCap", NhaCungCap);
                param[8] = new SqlParameter("@Res", 0);
                param[8].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_VatTu_ThemBienDong", param);
                result = Convert.ToInt32(param[8].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int VatTu_ThemMoi(string LoaiVatTu, string TenVatTu, string MoTa, string DonViTinh, decimal SoLuong, int Thang, int Nam)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@LoaiVatTu", LoaiVatTu);
                param[1] = new SqlParameter("@TenVatTu", TenVatTu);
                param[2] = new SqlParameter("@MoTa", MoTa);
                param[3] = new SqlParameter("@DonViTinh", DonViTinh);
                param[4] = new SqlParameter("@SoLuong", SoLuong);
                param[5] = new SqlParameter("@Thang", Thang);
                param[6] = new SqlParameter("@Nam", Nam);
                result = DataProvider.ExecuteSP("QLCS_VatTu_ThemMoi", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public DataTable VatTu_GetDCS_Order()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_VatTu_GetDCS_Order");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable VatTu_GetDCS_Order_Type(string Type)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Type", Type);
                result = DataProvider.SelectSP("QLCS_VatTu_GetDCS_Order_Type", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable VatTu_GetTTY_Order()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_VatTu_GetTTY_Order");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable VatTu_GetTTY_Order_CoDonViTinh()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_VatTu_GetTTY_Order_CoDonViTinh");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable VatTu_GetSPGM_Order()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_VatTu_GetSPGM_Order");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable VatTu_GetTA_Order()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_VatTu_GetTA_Order");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable VatTu_GetVatTuBienDongByIDVatTu(int VatTuID, int Status, DateTime dateFrom, DateTime dateTo)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@VatTuID", VatTuID);
                param[1] = new SqlParameter("@Status", Status);
                param[2] = new SqlParameter("@dateFrom", dateFrom);
                param[3] = new SqlParameter("@dateTo", dateTo);
                result = DataProvider.SelectSP("QLCS_VatTu_GetVatTuBienDongByIDVatTu", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int VatTu_ThayDoiSoLuongBienDong(int IDBienDong, decimal SoLuong, int User, int NhaCungCap)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@IDBienDong", IDBienDong);
                param[1] = new SqlParameter("@SoLuong", SoLuong);
                param[2] = new SqlParameter("@User", User);
                param[3] = new SqlParameter("@NhaCungCap", NhaCungCap);
                param[4] = new SqlParameter("@Res", 0);
                param[4].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_VatTu_ThayDoiSoLuongBienDong", param);
                result = Convert.ToInt32(param[4].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        /*Ca Sau An======================================================================================================================*/

        public DataTable LoadCaSauAnTuNgayDenNgay(DateTime TuNgay, DateTime DenNgay)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@TuNgay", TuNgay);
                param[1] = new SqlParameter("@DenNgay", DenNgay);
                result = DataProvider.SelectSP("QLCS_CaSauAn_GetCaSauAnTuNgayDenNgay", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        
        public DataTable LoadPageCaSauAn(string WhereClause, string OrderBy, int StartIndex, int PageSize)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                param[2] = new SqlParameter("@StartIndex", StartIndex);
                param[3] = new SqlParameter("@PageSize", PageSize);
                result = DataProvider.SelectSP("QLCS_GetCaSauAn_Page", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int CountCaSauAn(string WhereClause)
        {
            int TotalRecords = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@TotalRecords", TotalRecords);
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetCaSauAn_Count", param);
                TotalRecords = Convert.ToInt32(param[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TotalRecords;
        }

        public int CaSauAn_ThemMoi(DateTime ThoiDiem, int User, string Status)
        {
            int ID = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@ThoiDiem", ThoiDiem);
                param[1] = new SqlParameter("@User", User);
                param[2] = new SqlParameter("@Status", Status);
                param[3] = new SqlParameter("@ID", ID);
                param[3].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSauAn_ThemMoi", param);
                ID = Convert.ToInt32(param[3].Value);
            }
            catch (Exception ex)
            {
            }
            return ID;
        }

        public int CaSauAn_GetCaSauAnByNgay(DateTime ThoiDiem, out DateTime ThoiDiemOutput)
        {
            ThoiDiemOutput = DateTime.MinValue;
            int ID = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ThoiDiem", ThoiDiem);
                param[1] = new SqlParameter("@ID", ID);
                param[1].Direction = ParameterDirection.Output;
                param[2] = new SqlParameter("@ThoiDiemOutput", DateTime.MinValue);
                param[2].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSauAn_GetCaSauAnByNgay", param);
                ID = Convert.ToInt32(param[1].Value);
                if (param[2].Value != DBNull.Value) ThoiDiemOutput = Convert.ToDateTime(param[2].Value);
            }
            catch (Exception ex)
            {
            }
            return ID;
        }

        public DataTable CaSauAn_GetByID(int CaSauAnID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@CaSauAn", CaSauAnID);
                result = DataProvider.SelectSP("QLCS_CaSauAn_GetCaSauAnByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable CaSauAn_GetCaSauAnThua(string dFrom, string dTo)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@dFrom", dFrom);
                param[1] = new SqlParameter("@dTo", dTo);
                result = DataProvider.SelectSP("QLCS_GetCaSauAnThua", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable CaSauAn_GetCaSauAnThuaByCaSauAn(int CaSauAn)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@CaSauAn", CaSauAn);
                result = DataProvider.SelectSP("QLCS_CaSauAnThua_GetByCaSauAn", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void CaSauAnThua_InsertUpdate(DateTime Ngay, string ThucAnThua)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Ngay", Ngay);
                param[1] = new SqlParameter("@ThucAnThua", ThucAnThua);
                DataProvider.ExecuteSP("QLCS_CaSauAn_InsertUpdateThucAnThua", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void CaSauAnThua_Delete(int CaSauAn)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@CaSauAn", CaSauAn);
                DataProvider.ExecuteSP("QLCS_CaSauAnThua_Delete", param);
            }
            catch (Exception ex)
            {
            }
        }

        public DataTable CaSauAnThucAn_GetByID(int IDCaSauAnThucAn, int Status)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@IDCaSauAnThucAn", IDCaSauAnThucAn);
                param[1] = new SqlParameter("@Status", Status);
                result = DataProvider.SelectSP("QLCS_CaSauAnThucAn_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable CaSauAnThuoc_GetByID(int IDCaSauAnThuoc, int Status)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@IDCaSauAnThuoc", IDCaSauAnThuoc);
                param[1] = new SqlParameter("@Status", Status);
                result = DataProvider.SelectSP("QLCS_CaSauAnThuoc_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable CaSauAn_GetChiTietByID(int CaSauAnID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@CaSauAn", CaSauAnID);
                result = DataProvider.SelectSP("QLCS_CaSauAn_GetCaSauAnChiTietByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable CaSauAn_GetChiTietThuocByID(int CaSauAnID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@CaSauAn", CaSauAnID);
                result = DataProvider.SelectSP("QLCS_CaSauAn_GetCaSauAnChiTietThuocByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int CaSauAn_Update(int ID, DateTime ThoiDiemMoi, int User)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@ID", ID);
                param[1] = new SqlParameter("@ThoiDiemMoi", ThoiDiemMoi);
                param[2] = new SqlParameter("@User", User);
                param[3] = new SqlParameter("@Res", 0);
                param[3].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSauAn_Update", param);
                result = Convert.ToInt32(param[3].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CaSauAn_UpdateChiTiet_org(int CSAID, int ThucAn, decimal KhoiLuong, DateTime ThoiDiem, int NguoiChoAn)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@ThoiDiem", ThoiDiem);
                param[4] = new SqlParameter("@NguoiChoAn", NguoiChoAn);
                result = DataProvider.ExecuteSP("QLCS_CaSauAn_UpdateChiTiet_org", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CaSauAn_UpdateChiTiet(int CSAID, int ThucAn, decimal KhoiLuong, int NguoiChoAn)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@NguoiChoAn", NguoiChoAn);
                result = DataProvider.ExecuteSP("QLCS_CaSauAn_UpdateChiTiet", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CaSauAn_UpdateChiTietThuoc_org(int CSAID, int Thuoc, decimal KhoiLuong, DateTime ThoiDiem, int NguoiChoAn)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@ThoiDiem", ThoiDiem);
                param[4] = new SqlParameter("@NguoiChoAn", NguoiChoAn);
                result = DataProvider.ExecuteSP("QLCS_CaSauAn_UpdateChiTietThuoc", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CaSauAn_UpdateChiTietThuoc(int CSAID, int Thuoc, decimal KhoiLuong, int NguoiChoAn)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@NguoiChoAn", NguoiChoAn);
                result = DataProvider.ExecuteSP("QLCS_CaSauAn_UpdateChiTietThuoc", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public DataTable CaSauAn_GetChuongByThucAnByLoaiCa(int CaSauAn, int ThucAn, int LoaiCa, out decimal KhoiLuong, out int SoLuongCa, out int SoLuongTT, out string NguoiChoAn, out DateTime NgayAn)
        {
            KhoiLuong = 0;
            SoLuongCa = 0;
            SoLuongTT = 0;
            NguoiChoAn = "";
            NgayAn = DateTime.MinValue;
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@CaSauAn", CaSauAn);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@LoaiCa", LoaiCa);
                param[3] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3].Direction = ParameterDirection.Output;
                param[3].Scale = 5;
                param[4] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[4].Direction = ParameterDirection.Output;
                param[5] = new SqlParameter("@SoLuongTT", SoLuongTT);
                param[5].Direction = ParameterDirection.Output;
                param[6] = new SqlParameter("@NguoiChoAn", NguoiChoAn);
                param[6].Direction = ParameterDirection.Output;
                param[6].Size = 4000;
                param[7] = new SqlParameter("@NgayAn", NgayAn);
                param[7].Direction = ParameterDirection.Output;
                result = DataProvider.SelectSP("QLCS_CaSauAn_GetChuongByThucAnByLoaiCa", param);
                if (param[3].Value != DBNull.Value) KhoiLuong = Convert.ToDecimal(param[3].Value);
                if (param[4].Value != DBNull.Value) SoLuongCa = Convert.ToInt32(param[4].Value);
                if (param[5].Value != DBNull.Value) SoLuongTT = Convert.ToInt32(param[5].Value);
                if (param[6].Value != DBNull.Value) NguoiChoAn = param[6].Value.ToString();
                if (param[7].Value != DBNull.Value) NgayAn = Convert.ToDateTime(param[7].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable CaSauAn_GetChuongByLoaiCa(int CaSauAn, int LoaiCa)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@CaSauAn", CaSauAn);
                param[1] = new SqlParameter("@LoaiCa", LoaiCa);
                result = DataProvider.SelectSP("QLCS_CaSauAn_GetChuongByLoaiCa", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable CaSauAn_GetChuongByThucAnByLoaiCa_Delete(int CaSauAn, int ThucAn, int LoaiCa, out decimal KhoiLuong, out int SoLuongCa, out int SoLuongTT, out string NguoiChoAn, out DateTime NgayAn)
        {
            KhoiLuong = 0;
            SoLuongCa = 0;
            SoLuongTT = 0;
            NguoiChoAn = "";
            NgayAn = DateTime.MinValue;
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@CaSauAn", CaSauAn);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@LoaiCa", LoaiCa);
                param[3] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3].Direction = ParameterDirection.Output;
                param[3].Scale = 5;
                param[4] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[4].Direction = ParameterDirection.Output;
                param[5] = new SqlParameter("@SoLuongTT", SoLuongTT);
                param[5].Direction = ParameterDirection.Output;
                param[6] = new SqlParameter("@NguoiChoAn", NguoiChoAn);
                param[6].Direction = ParameterDirection.Output;
                param[6].Size = 4000;
                param[7] = new SqlParameter("@NgayAn", NgayAn);
                param[7].Direction = ParameterDirection.Output;
                result = DataProvider.SelectSP("QLCS_CaSauAn_GetChuongByThucAnByLoaiCa_Delete", param);
                if (param[3].Value != DBNull.Value) KhoiLuong = Convert.ToDecimal(param[3].Value);
                if (param[4].Value != DBNull.Value) SoLuongCa = Convert.ToInt32(param[4].Value);
                if (param[5].Value != DBNull.Value) SoLuongTT = Convert.ToInt32(param[5].Value);
                if (param[6].Value != DBNull.Value) NguoiChoAn = param[6].Value.ToString();
                if (param[7].Value != DBNull.Value) NgayAn = Convert.ToDateTime(param[7].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable CaSauAn_GetThucAn(int CaSauAn, int Status)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@CaSauAn", CaSauAn);
                param[1] = new SqlParameter("@Status", Status);
                result = DataProvider.SelectSP("QLCS_CaSauAn_GetThucAn", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int CaSauAn_UpdateThucAn_CoBan(int CSAID, int ThucAn, decimal KhoiLuong, int LoaiCa, int SoLuongCa, string StrSoLuongChuong, string StrChuong, string StrKL, string StrPhanCachKhuChuong)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@LoaiCa", LoaiCa);
                param[4] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[5] = new SqlParameter("@StrSoLuongChuong", StrSoLuongChuong);
                param[6] = new SqlParameter("@StrChuong", StrChuong);
                param[7] = new SqlParameter("@StrKL", StrKL);
                param[8] = new SqlParameter("@StrPhanCachKhuChuong", StrPhanCachKhuChuong);
                param[9] = new SqlParameter("@Res", 0);
                param[9].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSauAn_UpdateThucAn_CoBan", param);
                result = Convert.ToInt32(param[9].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CaSauAn_InsertUpdateThucAn(int CSAID, int ThucAn, decimal KhoiLuong, int LoaiCa, int SoLuongCa, int SoLuongTT, string StrSoLuongChuong, string StrSoLuongChuongTT, string StrChuong, string StrKL, string StrPhanCachKhuChuong, string arrNhanVien, int User, bool ReplaceChuong)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[15];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@LoaiCa", LoaiCa);
                param[4] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[5] = new SqlParameter("@SoLuongTT", SoLuongTT);
                param[6] = new SqlParameter("@StrSoLuongChuong", StrSoLuongChuong);
                param[7] = new SqlParameter("@StrSoLuongChuongTT", StrSoLuongChuongTT);
                param[8] = new SqlParameter("@StrChuong", StrChuong);
                param[9] = new SqlParameter("@StrKL", StrKL);
                param[10] = new SqlParameter("@StrPhanCachKhuChuong", StrPhanCachKhuChuong);
                param[11] = new SqlParameter("@arrNhanVien", arrNhanVien);
                param[12] = new SqlParameter("@User", User);
                param[13] = new SqlParameter("@ReplaceChuong", ReplaceChuong);
                param[14] = new SqlParameter("@Res", 0);
                param[14].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSauAn_InsertUpdateThucAn", param);
                result = Convert.ToInt32(param[14].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void CaSauAn_InsertUpdateThucAn_NoCheck_ForUpdateHangLoat(int CSAID, int ThucAn, decimal KhoiLuong, int LoaiCa, int SoLuongCa, int SoLuongTT, string StrSoLuongChuong, string StrSoLuongChuongTT, string StrChuong, string StrKL, string StrPhanCachKhuChuong, int User)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@LoaiCa", LoaiCa);
                param[4] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[5] = new SqlParameter("@SoLuongTT", SoLuongTT);
                param[6] = new SqlParameter("@StrSoLuongChuong", StrSoLuongChuong);
                param[7] = new SqlParameter("@StrSoLuongChuongTT", StrSoLuongChuongTT);
                param[8] = new SqlParameter("@StrChuong", StrChuong);
                param[9] = new SqlParameter("@StrKL", StrKL);
                param[10] = new SqlParameter("@StrPhanCachKhuChuong", StrPhanCachKhuChuong);
                param[11] = new SqlParameter("@User", User);
                DataProvider.ExecuteSP("QLCS_CaSauAn_InsertUpdateThucAn_NoCheck_ForUpdateHangLoat", param);
            }
            catch (Exception ex)
            {
            }
        }

        public string CaSauAn_InsertUpdateThucAn_SS(int CSAID, int ThucAn, decimal KhoiLuong, string arrNhanVien, int User)
        {
            string result = "";
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@arrNhanVien", arrNhanVien);
                param[4] = new SqlParameter("@User", User);
                param[5] = new SqlParameter("@Res", "");
                param[5].Direction = ParameterDirection.Output;
                param[5].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSauAn_InsertUpdateThucAn_SS", param);
                result = param[5].Value.ToString();
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void CaSauAn_ThucAn_UpdateKhoiLuongChuong(int CSAID, int ThucAn, int LoaiCa, string StrChuong, string StrKL)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@LoaiCa", LoaiCa);
                param[3] = new SqlParameter("@StrChuong", StrChuong);
                param[4] = new SqlParameter("@StrKL", StrKL);
                DataProvider.ExecuteSP("QLCS_CaSauAn_ThucAn_UpdateKhoiLuongChuong", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void CaSauAn_InsertUpdateThucAn_UpdateChuong(int CSAID, int ThucAn, int LoaiCa, int SoLuongCa, string StrChuong, string StrKL)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@LoaiCa", LoaiCa);
                param[3] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[4] = new SqlParameter("@StrChuong", StrChuong);
                param[5] = new SqlParameter("@StrKL", StrKL);
                DataProvider.ExecuteSP("QLCS_CaSauAn_InsertUpdateThucAn_UpdateChuong", param);
            }
            catch (Exception ex)
            {
            }
        }

        public DataTable CaSauAn_GetChuongByThuocByLoaiCa(int CaSauAn, int Thuoc, int LoaiCa, out decimal KhoiLuong, out int SoLuongCa, out int SoLuongTT, out DateTime NgayAn)
        {
            KhoiLuong = 0;
            SoLuongCa = 0;
            SoLuongTT = 0;
            NgayAn = DateTime.MinValue;
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@CaSauAn", CaSauAn);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@LoaiCa", LoaiCa);
                param[3] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3].Direction = ParameterDirection.Output;
                param[3].Scale = 5;
                param[4] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[4].Direction = ParameterDirection.Output;
                param[5] = new SqlParameter("@SoLuongTT", SoLuongTT);
                param[5].Direction = ParameterDirection.Output;
                param[6] = new SqlParameter("@NgayAn", NgayAn);
                param[6].Direction = ParameterDirection.Output;
                result = DataProvider.SelectSP("QLCS_CaSauAn_GetChuongByThuocByLoaiCa", param);
                if (param[3].Value != DBNull.Value) KhoiLuong = Convert.ToDecimal(param[3].Value);
                if (param[4].Value != DBNull.Value) SoLuongCa = Convert.ToInt32(param[4].Value);
                if (param[5].Value != DBNull.Value) SoLuongTT = Convert.ToInt32(param[5].Value);
                if (param[6].Value != DBNull.Value) NgayAn = Convert.ToDateTime(param[6].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable CaSauAn_GetChuongByThuocByLoaiCa_Delete(int CaSauAn, int Thuoc, int LoaiCa, out decimal KhoiLuong, out int SoLuongCa, out int SoLuongTT, out DateTime NgayAn)
        {
            KhoiLuong = 0;
            SoLuongCa = 0;
            SoLuongTT = 0;
            NgayAn = DateTime.MinValue;
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@CaSauAn", CaSauAn);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@LoaiCa", LoaiCa);
                param[3] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3].Direction = ParameterDirection.Output;
                param[3].Scale = 5;
                param[4] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[4].Direction = ParameterDirection.Output;
                param[5] = new SqlParameter("@SoLuongTT", SoLuongTT);
                param[5].Direction = ParameterDirection.Output;
                param[6] = new SqlParameter("@NgayAn", NgayAn);
                param[6].Direction = ParameterDirection.Output;
                result = DataProvider.SelectSP("QLCS_CaSauAn_GetChuongByThuocByLoaiCa_Delete", param);
                if (param[3].Value != DBNull.Value) KhoiLuong = Convert.ToDecimal(param[3].Value);
                if (param[4].Value != DBNull.Value) SoLuongCa = Convert.ToInt32(param[4].Value);
                if (param[5].Value != DBNull.Value) SoLuongTT = Convert.ToInt32(param[5].Value);
                if (param[6].Value != DBNull.Value) NgayAn = Convert.ToDateTime(param[6].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable CaSauAn_GetThuoc(int CaSauAn, int Status)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@CaSauAn", CaSauAn);
                param[1] = new SqlParameter("@Status", Status);
                result = DataProvider.SelectSP("QLCS_CaSauAn_GetThuoc", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int CaSauAn_UpdateThuoc_CoBan(int CSAID, int Thuoc, decimal KhoiLuong, int LoaiCa, int SoLuongCa, string StrSoLuongChuong, string StrChuong, string StrKL, string StrPhanCachKhuChuong)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@LoaiCa", LoaiCa);
                param[4] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[5] = new SqlParameter("@StrSoLuongChuong", StrSoLuongChuong);
                param[6] = new SqlParameter("@StrChuong", StrChuong);
                param[7] = new SqlParameter("@StrKL", StrKL);
                param[8] = new SqlParameter("@StrPhanCachKhuChuong", StrPhanCachKhuChuong);
                param[9] = new SqlParameter("@Res", 0);
                param[9].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSauAn_UpdateThuoc_CoBan", param);
                result = Convert.ToInt32(param[9].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CaSauAn_InsertUpdateThuoc(int CSAID, int Thuoc, decimal KhoiLuong, int LoaiCa, int SoLuongCa, int SoLuongTT, string StrSoLuongChuong, string StrSoLuongChuongTT, string StrChuong, string StrKL, string StrPhanCachKhuChuong, int User, bool ReplaceChuong)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@LoaiCa", LoaiCa);
                param[4] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[5] = new SqlParameter("@SoLuongTT", SoLuongTT);
                param[6] = new SqlParameter("@StrSoLuongChuong", StrSoLuongChuong);
                param[7] = new SqlParameter("@StrSoLuongChuongTT", StrSoLuongChuongTT);
                param[8] = new SqlParameter("@StrChuong", StrChuong);
                param[9] = new SqlParameter("@StrKL", StrKL);
                param[10] = new SqlParameter("@StrPhanCachKhuChuong", StrPhanCachKhuChuong);
                param[11] = new SqlParameter("@User", User);
                param[12] = new SqlParameter("@ReplaceChuong", ReplaceChuong);
                param[13] = new SqlParameter("@Res", 0);
                param[13].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSauAn_InsertUpdateThuoc", param);
                result = Convert.ToInt32(param[13].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void CaSauAn_InsertUpdateThuoc_NoCheck_ForUpdateHangLoat(int CSAID, int Thuoc, decimal KhoiLuong, int LoaiCa, int SoLuongCa, int SoLuongTT, string StrSoLuongChuong, string StrSoLuongChuongTT, string StrChuong, string StrKL, string StrPhanCachKhuChuong, int User)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@LoaiCa", LoaiCa);
                param[4] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[5] = new SqlParameter("@SoLuongTT", SoLuongTT);
                param[6] = new SqlParameter("@StrSoLuongChuong", StrSoLuongChuong);
                param[7] = new SqlParameter("@StrSoLuongChuongTT", StrSoLuongChuongTT);
                param[8] = new SqlParameter("@StrChuong", StrChuong);
                param[9] = new SqlParameter("@StrKL", StrKL);
                param[10] = new SqlParameter("@StrPhanCachKhuChuong", StrPhanCachKhuChuong);
                param[11] = new SqlParameter("@User", User);
                DataProvider.ExecuteSP("QLCS_CaSauAn_InsertUpdateThuoc_NoCheck_ForUpdateHangLoat", param);
            }
            catch (Exception ex)
            {
            }
        }

        public string CaSauAn_InsertUpdateThuoc_SS(int CSAID, int Thuoc, decimal KhoiLuong, int User)
        {
            string result = "";
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@User", User);
                param[4] = new SqlParameter("@Res", "");
                param[4].Direction = ParameterDirection.Output;
                param[4].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSauAn_InsertUpdateThuoc_SS", param);
                result = param[4].Value.ToString();
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void CaSauAn_Thuoc_UpdateKhoiLuongChuong(int CSAID, int Thuoc, int LoaiCa, string StrChuong, string StrKL)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@LoaiCa", LoaiCa);
                param[3] = new SqlParameter("@StrChuong", StrChuong);
                param[4] = new SqlParameter("@StrKL", StrKL);
                DataProvider.ExecuteSP("QLCS_CaSauAn_Thuoc_UpdateKhoiLuongChuong", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void CaSauAn_InsertUpdateThuoc_UpdateChuong(int CSAID, int Thuoc, int LoaiCa, int SoLuongCa, string StrChuong, string StrKL)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@LoaiCa", LoaiCa);
                param[3] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[4] = new SqlParameter("@StrChuong", StrChuong);
                param[5] = new SqlParameter("@StrKL", StrKL);
                DataProvider.ExecuteSP("QLCS_CaSauAn_InsertUpdateThuoc_UpdateChuong", param);
            }
            catch (Exception ex)
            {
            }
        }

        public int CaSauAn_CanInsertUpdateThucAn(int CSAID, int ThucAn, decimal KhoiLuong, int LoaiCa, bool ReplaceChuong, string StrChuong)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@LoaiCa", LoaiCa);
                param[4] = new SqlParameter("@ReplaceChuong", ReplaceChuong);
                param[5] = new SqlParameter("@StrChuong", StrChuong);
                param[6] = new SqlParameter("@Res", 0);
                param[6].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSauAn_CanInsertUpdateThucAn", param);
                result = Convert.ToInt32(param[6].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public string CaSauAn_CanInsertUpdateThucAn_SS(int CSAID, int ThucAn, decimal KhoiLuong, bool ReplaceChuong)
        {
            string result = "";
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@ReplaceChuong", ReplaceChuong);
                param[4] = new SqlParameter("@Res", "");
                param[4].Direction = ParameterDirection.Output;
                param[4].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSauAn_CanInsertUpdateThucAn_SS", param);
                result = Convert.ToString(param[4].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CaSauAn_CanInsertUpdateThuoc(int CSAID, int Thuoc, decimal KhoiLuong, int LoaiCa, bool ReplaceChuong, string StrChuong)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@LoaiCa", LoaiCa);
                param[4] = new SqlParameter("@ReplaceChuong", ReplaceChuong);
                param[5] = new SqlParameter("@StrChuong", StrChuong);
                param[6] = new SqlParameter("@Res", 0);
                param[6].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaSauAn_CanInsertUpdateThuoc", param);
                result = Convert.ToInt32(param[6].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public string CaSauAn_CanInsertUpdateThuoc_SS(int CSAID, int Thuoc, decimal KhoiLuong, bool ReplaceChuong)
        {
            string result = "";
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@ReplaceChuong", ReplaceChuong);
                param[4] = new SqlParameter("@Res", "");
                param[4].Direction = ParameterDirection.Output;
                param[4].Size = 4000;
                DataProvider.ExecuteSP("QLCS_CaSauAn_CanInsertUpdateThuoc_SS", param);
                result = Convert.ToString(param[4].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void CaSauAn_InsertUpdateThucAn_NoCheck(int CSAID, int ThucAn, decimal KhoiLuong, int LoaiCa, int SoLuongCa, int SoLuongTT, string StrSoLuongChuong, string StrSoLuongChuongTT, string StrChuong, string StrKL, string StrPhanCachKhuChuong, string arrNhanVien, int User, bool ReplaceChuong)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@LoaiCa", LoaiCa);
                param[4] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[5] = new SqlParameter("@SoLuongTT", SoLuongTT);
                param[6] = new SqlParameter("@StrSoLuongChuong", StrSoLuongChuong);
                param[7] = new SqlParameter("@StrSoLuongChuongTT", StrSoLuongChuongTT);
                param[8] = new SqlParameter("@StrChuong", StrChuong);
                param[9] = new SqlParameter("@StrKL", StrKL);
                param[10] = new SqlParameter("@StrPhanCachKhuChuong", StrPhanCachKhuChuong);
                param[11] = new SqlParameter("@arrNhanVien", arrNhanVien);
                param[12] = new SqlParameter("@User", User);
                param[13] = new SqlParameter("@ReplaceChuong", ReplaceChuong);
                DataProvider.ExecuteSP("QLCS_CaSauAn_InsertUpdateThucAn_NoCheck", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void CaSauAn_InsertUpdateThucAn_SS_NoCheck(int CSAID, int ThucAn, decimal KhoiLuong, string arrNhanVien, int User)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@ThucAn", ThucAn);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@arrNhanVien", arrNhanVien);
                param[4] = new SqlParameter("@User", User);
                DataProvider.ExecuteSP("QLCS_CaSauAn_InsertUpdateThucAn_SS_NoCheck", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void CaSauAn_InsertUpdateThuoc_NoCheck(int CSAID, int Thuoc, decimal KhoiLuong, int LoaiCa, int SoLuongCa, int SoLuongTT, string StrSoLuongChuong, string StrSoLuongChuongTT, string StrChuong, string StrKL, string StrPhanCachKhuChuong, int User, bool ReplaceChuong)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[13];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@LoaiCa", LoaiCa);
                param[4] = new SqlParameter("@SoLuongCa", SoLuongCa);
                param[5] = new SqlParameter("@SoLuongTT", SoLuongTT);
                param[6] = new SqlParameter("@StrSoLuongChuong", StrSoLuongChuong);
                param[7] = new SqlParameter("@StrSoLuongChuongTT", StrSoLuongChuongTT);
                param[8] = new SqlParameter("@StrChuong", StrChuong);
                param[9] = new SqlParameter("@StrKL", StrKL);
                param[10] = new SqlParameter("@StrPhanCachKhuChuong", StrPhanCachKhuChuong);
                param[11] = new SqlParameter("@User", User);
                param[12] = new SqlParameter("@ReplaceChuong", ReplaceChuong);
                DataProvider.ExecuteSP("QLCS_CaSauAn_InsertUpdateThuoc_NoCheck", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void CaSauAn_InsertUpdateThuoc_SS_NoCheck(int CSAID, int Thuoc, decimal KhoiLuong, int User)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@CSAID", CSAID);
                param[1] = new SqlParameter("@Thuoc", Thuoc);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@User", User);
                DataProvider.ExecuteSP("QLCS_CaSauAn_InsertUpdateThuoc_SS_NoCheck", param);
            }
            catch (Exception ex)
            {
            }
        }

        /*Giet Mo Ca======================================================================================================================*/

        public DataTable LoadPageGietMoCa(string WhereClause, string OrderBy, int StartIndex, int PageSize)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                param[2] = new SqlParameter("@StartIndex", StartIndex);
                param[3] = new SqlParameter("@PageSize", PageSize);
                result = DataProvider.SelectSP("QLCS_GetGietMoCa_Page", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int CountGietMoCa(string WhereClause)
        {
            int TotalRecords = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@TotalRecords", TotalRecords);
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetGietMoCa_Count", param);
                TotalRecords = Convert.ToInt32(param[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TotalRecords;
        }

        public int GietMoCa_ThemMoi(string strSanPham, string strKhoiLuong, DateTime NgayMo, string BienBan, int Status, int NguoiThucHienBienDong, out int GietMoCa)
        {
            int result = 0;
            GietMoCa = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@NgayMo", NgayMo);
                param[1] = new SqlParameter("@BienBan", BienBan);
                param[2] = new SqlParameter("@Status", Status);
                param[3] = new SqlParameter("@NguoiThucHienBienDong", NguoiThucHienBienDong);
                param[4] = new SqlParameter("@StrSanPham", strSanPham);
                param[5] = new SqlParameter("@StrKhoiLuong", strKhoiLuong.Replace(',', '.'));
                param[6] = new SqlParameter("@GietMoCa", 0);
                param[6].Direction = ParameterDirection.Output;
                param[7] = new SqlParameter("@Res", 0);
                param[7].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GietMoCa_ThemMoi", param);
                result = Convert.ToInt32(param[7].Value);
                GietMoCa = Convert.ToInt32(param[6].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int GietMoCa_UpdatePhuongPhapMo(int Ngay, int Thang, int Nam, int LoaiCa, int Chuong, int Da_Bung, int Da_PhanLoai, decimal TLH, decimal TLMH, string PPM)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[11];
                param[0] = new SqlParameter("@Ngay", Ngay);
                param[1] = new SqlParameter("@Thang", Thang);
                param[2] = new SqlParameter("@Nam", Nam);
                param[3] = new SqlParameter("@LoaiCa", LoaiCa);
                param[4] = new SqlParameter("@Chuong", Chuong);
                param[5] = new SqlParameter("@Da_Bung", Da_Bung);
                param[6] = new SqlParameter("@Da_PhanLoai", Da_PhanLoai);
                param[7] = new SqlParameter("@TLH", TLH);
                param[8] = new SqlParameter("@TLMH", TLMH);
                param[9] = new SqlParameter("@PPM", PPM);
                param[10] = new SqlParameter("@Res", 0);
                param[10].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GietMoCa_UpdatePhuongPhapMo", param);
                result = Convert.ToInt32(param[10].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int GietMoCa_NgayMoOK(DateTime NgayMo, int IDOrg,out bool KetQua)
        {
            int result = 0;
            KetQua = false;
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@NgayMo", NgayMo);
                param[1] = new SqlParameter("@IDOrg", IDOrg);
                param[2] = new SqlParameter("@KetQua", KetQua);
                param[2].Direction = ParameterDirection.Output;
                result = DataProvider.ExecuteSP("QLCS_GietMoCa_NgayMoOK", param);
                KetQua = Convert.ToBoolean(param[2].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public DataTable GietMoCa_GetByID(int GietMoCaID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@GietMoCa", GietMoCaID);
                result = DataProvider.SelectSP("QLCS_GietMoCa_GetGietMoCaByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable GietMoCa_GetSanPhamByID(int GietMoCaID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@GietMoCa", GietMoCaID);
                result = DataProvider.SelectSP("QLCS_GietMoCa_GetGietMoCa_SanPhamByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int GietMoCa_Update(int ID, DateTime NgayMo, string BienBan, int NguoiThayDoi)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@ID", ID);
                param[1] = new SqlParameter("@NgayMoMoi", NgayMo);
                param[2] = new SqlParameter("@BienBan", BienBan);
                param[3] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[4] = new SqlParameter("@Res", 0);
                param[4].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GietMoCa_Update", param);
                result = Convert.ToInt32(param[4].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int GietMoCa_UpdateSanPham(int GMCID, int SanPham, decimal KhoiLuong, DateTime ThoiDiem, int NguoiThucHienBienDong)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@GMCID", GMCID);
                param[1] = new SqlParameter("@SanPham", SanPham);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@ThoiDiem", ThoiDiem);
                param[4] = new SqlParameter("@NguoiThucHienBienDong", NguoiThucHienBienDong);
                param[5] = new SqlParameter("@Res", 0);
                param[5].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GietMoCa_UpdateSanPham", param);
                result = Convert.ToInt32(param[5].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int GietMoCa_UpdateDCSasSPGM(int GMCID, int SanPham, decimal KhoiLuong, DateTime ThoiDiem, int NguoiThucHienBienDong)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@GMCID", GMCID);
                param[1] = new SqlParameter("@SanPham", SanPham);
                param[2] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[3] = new SqlParameter("@ThoiDiem", ThoiDiem);
                param[4] = new SqlParameter("@NguoiThucHienBienDong", NguoiThucHienBienDong);
                param[5] = new SqlParameter("@Res", 0);
                param[5].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GietMoCa_UpdateDCSasSPGM", param);
                result = Convert.ToInt32(param[5].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public string GietMoCa_TraoDoiSanPham(int GMCID, int NguoiThucHienBienDong)
        {
            string result = "";
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@GMCID", GMCID);
                param[1] = new SqlParameter("@NguoiThucHienBienDong", NguoiThucHienBienDong);
                param[2] = new SqlParameter("@Res", result);
                param[2].Direction = ParameterDirection.Output;
                param[2].Size = 4000;
                DataProvider.ExecuteSP("QLCS_GietMoCa_TraoDoiSanPham", param);
                result = Convert.ToString(param[2].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int GietMoCa_TraoDoiSanPham_so(int GMCID, int NguoiThucHienBienDong)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@GMCID", GMCID);
                param[1] = new SqlParameter("@NguoiThucHienBienDong", NguoiThucHienBienDong);
                param[2] = new SqlParameter("@Res", result);
                param[2].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GietMoCa_TraoDoiSanPham", param);
                result = Convert.ToInt32(param[2].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public DataTable GietMoCa_GetByNgayMo(DateTime DateFrom, DateTime DateTo)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@DateFrom", DateFrom);
                param[1] = new SqlParameter("@DateTo", DateTo);
                result = DataProvider.SelectSP("QLCS_GietMoCa_GetByNgayMo", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable GietMoCa_GetChiTiet(int GietMoCaID, int Status)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@GietMoCa", GietMoCaID);
                param[1] = new SqlParameter("@Status", Status);
                result = DataProvider.SelectSP("QLCS_GietMoCa_GetChiTiet", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int GietMoCa_DeleteChiTiet(int GMCCT, int GMC, int Ca, int User)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@GietMoCaChiTiet", GMCCT);
                param[1] = new SqlParameter("@GietMoCa", GMC);
                param[2] = new SqlParameter("@Ca", Ca);
                param[3] = new SqlParameter("@User", User);
                param[4] = new SqlParameter("@Res", 0);
                param[4].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GietMoCa_DeleteChiTiet", param);
                result = Convert.ToInt32(param[4].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public DataTable GietMoCa_GetChiTietByChiTietID(int GietMoCaChiTiet, int Status)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@GietMoCaChiTiet", GietMoCaChiTiet);
                param[1] = new SqlParameter("@Status", Status);
                result = Config.SelectSP("QLCS_GietMoCa_GetChiTietByChiTietID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable GietMoCa_GetSanPhamByIDGMCSP(int GietMoCaSanPham)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@GietMoCaSanPham", GietMoCaSanPham);
                result = DataProvider.SelectSP("QLCS_GietMoCa_GetSanPhamByIDGMCSP", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int GietMoCa_InsertChiTiet(int GMC, int Ca, decimal Da_TrongLuong, int Da_Bung, int Da_PhanLoai, int Dau, int NguoiThucHienMo, decimal TrongLuongHoi, decimal TrongLuongMocHam, int NguoiMo, string PhuongPhapMo, bool DiTat, string VatTuGietMo)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@GietMoCa", GMC);
                param[1] = new SqlParameter("@Ca", Ca);
                param[2] = new SqlParameter("@Da_TrongLuong", Da_TrongLuong);
                param[3] = new SqlParameter("@Da_Bung", Da_Bung);
                param[4] = new SqlParameter("@Da_PhanLoai", Da_PhanLoai);
                param[5] = new SqlParameter("@Dau", Dau);
                param[6] = new SqlParameter("@NguoiThucHienMo", NguoiThucHienMo);
                param[7] = new SqlParameter("@TrongLuongHoi", TrongLuongHoi);
                param[8] = new SqlParameter("@TrongLuongMocHam", TrongLuongMocHam);
                param[9] = new SqlParameter("@NguoiMo", NguoiMo);
                param[10] = new SqlParameter("@PhuongPhapMo", PhuongPhapMo);
                param[11] = new SqlParameter("@DiTat", DiTat);
                param[12] = new SqlParameter("@sVatTu", VatTuGietMo);
                param[13] = new SqlParameter("@Res", 0);
                param[13].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GietMoCa_InsertChiTiet", param);
                result = Convert.ToInt32(param[13].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public DataTable GietMoCa_GetCaCoTheGietMoByLoaiCaByChuong(int LoaiCa, int Chuong, DateTime NgayMo)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@LoaiCa", LoaiCa);
                param[1] = new SqlParameter("@Chuong", Chuong);
                param[2] = new SqlParameter("@NgayMo", NgayMo);
                result = DataProvider.SelectSP("QLCS_GietMoCa_GetCaCoTheGietMoByLoaiCaByChuong", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable GietMoCa_GetCaCoTheGietMoByLoaiCaByChuongByNamByGiong_org(int LoaiCa, int Chuong, DateTime NgayMo, int NamNo, bool Giong)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@LoaiCa", LoaiCa);
                param[1] = new SqlParameter("@Chuong", Chuong);
                param[2] = new SqlParameter("@NgayMo", NgayMo);
                param[3] = new SqlParameter("@NamNo", NamNo);
                param[4] = new SqlParameter("@Giong", Giong);
                result = DataProvider.SelectSP("QLCS_GietMoCa_GetCaCoTheGietMoByLoaiCaByChuongByNamByGiong_org", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable GietMoCa_GetCaCoTheGietMoByLoaiCaByChuongByNamByGiong(int LoaiCa, string TenChuong, int So, DateTime NgayMo, int NamNo, bool Giong)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@LoaiCa", LoaiCa);
                param[1] = new SqlParameter("@TenChuong", TenChuong);
                param[2] = new SqlParameter("@So", So);
                param[3] = new SqlParameter("@NgayMo", NgayMo);
                param[4] = new SqlParameter("@NamNo", NamNo);
                param[5] = new SqlParameter("@Giong", Giong);
                result = DataProvider.SelectSP("QLCS_GietMoCa_GetCaCoTheGietMoByLoaiCaByChuongByNamByGiong", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable GietMoCa_GetCaCoTheGietMoByLoaiCaByChuongByNamByGiong_Except(int LoaiCa, string TenChuong, int So, DateTime NgayMo, int NamNo, bool Giong, string aCa)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@LoaiCa", LoaiCa);
                param[1] = new SqlParameter("@TenChuong", TenChuong);
                param[2] = new SqlParameter("@So", So);
                param[3] = new SqlParameter("@NgayMo", NgayMo);
                param[4] = new SqlParameter("@NamNo", NamNo);
                param[5] = new SqlParameter("@Giong", Giong);
                param[6] = new SqlParameter("@aCa", aCa);
                result = DataProvider.SelectSP("QLCS_GietMoCa_GetCaCoTheGietMoByLoaiCaByChuongByNamByGiong_Except", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        //public int GietMoCa_UpdateChiTiet(int GMCCT, int GMC, int Ca, decimal Da_TrongLuong, int Da_Bung, int Da_PhanLoai, int Dau, decimal TrongLuongHoi, decimal TrongLuongMocHam, int NguoiThayDoi)
        //{
        //    int result = 0;
        //    try
        //    {
        //        SqlParameter[] param = new SqlParameter[10];
        //        param[0] = new SqlParameter("@GietMoCaChiTiet", GMCCT);
        //        param[1] = new SqlParameter("@GietMoCa", GMC);
        //        param[2] = new SqlParameter("@Ca", Ca);
        //        param[3] = new SqlParameter("@Da_TrongLuong", Da_TrongLuong);
        //        param[4] = new SqlParameter("@Da_Bung", Da_Bung);
        //        param[5] = new SqlParameter("@Da_PhanLoai", Da_PhanLoai);
        //        param[6] = new SqlParameter("@Dau", Dau);
        //        param[7] = new SqlParameter("@TrongLuongHoi", TrongLuongHoi);
        //        param[8] = new SqlParameter("@TrongLuongMocHam", TrongLuongMocHam);
        //        param[9] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
        //        result = DataProvider.ExecuteSP("QLCS_GietMoCa_UpdateChiTiet", param);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return result;
        //}

        public int GietMoCa_UpdateChiTiet(int GMCCT, int GMC, decimal Da_TrongLuong, int Da_Bung, int Da_PhanLoai, int Dau, int NguoiMo, decimal TrongLuongHoi, decimal TrongLuongMocHam, int NguoiThayDoi, string PhuongPhapMo, bool DiTat, string VatTuGietMo)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@GietMoCaChiTiet", GMCCT);
                param[1] = new SqlParameter("@GietMoCa", GMC);
                param[2] = new SqlParameter("@Da_TrongLuong", Da_TrongLuong);
                param[3] = new SqlParameter("@Da_Bung", Da_Bung);
                param[4] = new SqlParameter("@Da_PhanLoai", Da_PhanLoai);
                param[5] = new SqlParameter("@Dau", Dau);
                param[6] = new SqlParameter("@NguoiMo", NguoiMo);
                param[7] = new SqlParameter("@TrongLuongHoi", TrongLuongHoi);
                param[8] = new SqlParameter("@TrongLuongMocHam", TrongLuongMocHam);
                param[9] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[10] = new SqlParameter("@PhuongPhapMo", PhuongPhapMo);
                param[11] = new SqlParameter("@DiTat", DiTat);
                param[12] = new SqlParameter("@sVatTu", VatTuGietMo);
                param[13] = new SqlParameter("@Res", 0);
                param[13].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GietMoCa_UpdateChiTiet", param);
                result = Convert.ToInt32(param[13].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int GietMoCa_UpdateTong(int GMC)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDGMC", GMC);
                result = DataProvider.ExecuteSP("QLCS_GietMoCa_CapNhatTong", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public DataTable GietMoCa_GetDCS(int GietMoCaID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@GietMoCa", GietMoCaID);
                result = DataProvider.SelectSP("QLCS_GietMoCa_GetDCS", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /*Ca Chet======================================================================================================================*/

        public string ThuHoiDa_GetIDByCa(int IDCaSau, int Status)
        {
            string res = "";
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@IDCaSau", IDCaSau);
                param[1] = new SqlParameter("@Status", Status);
                result = DataProvider.SelectSP("QLCS_ThuHoiDa_GetIDByCa", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if(result.Rows.Count == 1) res = result.Rows[0][0].ToString();
            return res;
        }
        
        public int CaChet_UpdateThongSo(int Ca, int Da_Bung, int Da_PhanLoai, int Dau, string PPM, int LyDoChet, decimal KhoiLuong, string BienBan, int NguoiThayDoi, string sVatTu)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[11];
                param[0] = new SqlParameter("@Ca", Ca);
                param[1] = new SqlParameter("@Da_Bung", Da_Bung);
                param[2] = new SqlParameter("@Da_PhanLoai", Da_PhanLoai);
                param[3] = new SqlParameter("@Dau", Dau);
                param[4] = new SqlParameter("@PPM", PPM);
                param[5] = new SqlParameter("@LyDoChet", LyDoChet);
                param[6] = new SqlParameter("@KhoiLuong", KhoiLuong);
                param[7] = new SqlParameter("@BienBan", BienBan);
                param[8] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[9] = new SqlParameter("@sVatTu", sVatTu);
                param[10] = new SqlParameter("@Res", 0);
                param[10].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaChet_UpdateThongSo", param);
                result = Convert.ToInt32(param[10].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CaChet_UpdateNgayChet(int Ca, int IDThuHoiDa, DateTime NgayChetMoi, int NguoiThayDoi, string Status)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@Ca", Ca);
                param[1] = new SqlParameter("@IDThuHoiDa", IDThuHoiDa);
                param[2] = new SqlParameter("@NgayChetMoi", NgayChetMoi);
                param[3] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[4] = new SqlParameter("@Status", Status);
                param[5] = new SqlParameter("@Res", 0);
                param[5].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaChet_UpdateNgayChet", param);
                result = Convert.ToInt32(param[5].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CaChet_PhucHoi(int Ca, int NguoiThayDoi)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@Ca", Ca);
                param[1] = new SqlParameter("@NguoiThayDoi", NguoiThayDoi);
                param[2] = new SqlParameter("@Res", 0);
                param[2].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_CaChet_PhucHoi", param);
                result = Convert.ToInt32(param[2].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int Config_SetNgayKhoaSo(DateTime NgayKhoaSo)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@NgayKhoaSo", NgayKhoaSo);
                result = DataProvider.ExecuteSP("QLCS_Config_SetNgayKhoaSo", param);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public static bool HasEditPermission(string[] Roles, string ExpectRole)
        {
            for (int i = 0; i < Roles.Length; i++)
            {
                if (Roles[i] == ExpectRole)
                {
                    return true;
                }
            }
            return false;
        }

        public DataTable GetBienBanCaChet()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_GetBienBanCaChet");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /*Lock======================================================================================================================*/

        public void Lock_BienDongCaSau(string strBienDong, bool Lock)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrBienDong", strBienDong);
                param[1] = new SqlParameter("@Lock", Lock);
                DataProvider.ExecuteSP("QLCS_Lock_BienDongCaSau", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void Lock_ThuHoiDa(string strBienDong, bool Lock)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrBienDong", strBienDong);
                param[1] = new SqlParameter("@Lock", Lock);
                DataProvider.ExecuteSP("QLCS_Lock_ThuHoiDa", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void Lock_TheoDoiDe(string strBienDong, bool Lock)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrBienDong", strBienDong);
                param[1] = new SqlParameter("@Lock", Lock);
                DataProvider.ExecuteSP("QLCS_Lock_TheoDoiDe", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void Lock_Note(string strBienDong, bool Lock)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrBienDong", strBienDong);
                param[1] = new SqlParameter("@Lock", Lock);
                DataProvider.ExecuteSP("QLCS_Lock_Note", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void Lock_CaSauAn(string strBienDong, bool Lock)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrBienDong", strBienDong);
                param[1] = new SqlParameter("@Lock", Lock);
                DataProvider.ExecuteSP("QLCS_Lock_CaSauAn", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void Lock_GietMoCa(string strBienDong, bool Lock)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrBienDong", strBienDong);
                param[1] = new SqlParameter("@Lock", Lock);
                DataProvider.ExecuteSP("QLCS_Lock_GietMoCa", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void Lock_VatTu_BienDong(string strBienDong, bool Lock)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@StrBienDong", strBienDong);
                param[1] = new SqlParameter("@Lock", Lock);
                DataProvider.ExecuteSP("QLCS_Lock_VatTu_BienDong", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void UnLock(string strLoaiDuLieu)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@StrLoaiDuLieu", strLoaiDuLieu);
                DataProvider.ExecuteSP("QLCS_UnLock", param);
            }
            catch (Exception ex)
            {
            }
        }

        /*NOTE============================================================================================================*/
        public DataTable LoadPageNote(string WhereClause, string OrderBy, int StartIndex, int PageSize)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                param[2] = new SqlParameter("@StartIndex", StartIndex);
                param[3] = new SqlParameter("@PageSize", PageSize);
                result = DataProvider.SelectSP("QLCS_GetNote_Page", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int CountNote(string WhereClause)
        {
            int TotalRecords = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@TotalRecords", TotalRecords);
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetNote_Count", param);
                TotalRecords = Convert.ToInt32(param[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TotalRecords;
        }

        public DataTable LoadPageNoteDelete(string WhereClause, string OrderBy, int StartIndex, int PageSize)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@OrderBy", OrderBy);
                param[2] = new SqlParameter("@StartIndex", StartIndex);
                param[3] = new SqlParameter("@PageSize", PageSize);
                result = DataProvider.SelectSP("QLCS_GetNote_Delete_Page", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int CountNoteDelete(string WhereClause)
        {
            int TotalRecords = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@WhereClause", WhereClause);
                param[1] = new SqlParameter("@TotalRecords", TotalRecords);
                param[1].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_GetNote_Delete_Count", param);
                TotalRecords = Convert.ToInt32(param[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TotalRecords;
        }

        public DataTable LoadNoteByID(int IDNote)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDNote", IDNote);
                result = DataProvider.SelectSP("QLCS_Note_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadNoteDeleteByID(int IDNote)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDNote", IDNote);
                result = DataProvider.SelectSP("QLCS_NoteDelete_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int InsertNote(DateTime Ngay, int Chuong, string Note, int UserID)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@Ngay", Ngay);
                param[1] = new SqlParameter("@Chuong", Chuong);
                param[2] = new SqlParameter("@Note", Note);
                param[3] = new SqlParameter("@NguoiThucHien", UserID);
                param[4] = new SqlParameter("@IDNote", result);
                param[4].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_Note_Insert", param);
                result = Convert.ToInt32(param[4].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int UpdateNote(int ID, DateTime Ngay, int Chuong, string Note, int User)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@ID", ID);
                param[1] = new SqlParameter("@Ngay", Ngay);
                param[2] = new SqlParameter("@Chuong", Chuong);
                param[3] = new SqlParameter("@Note", Note);
                param[4] = new SqlParameter("@User", User);
                param[5] = new SqlParameter("@Res", result);
                param[5].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_Note_Update", param);
                result = Convert.ToInt32(param[5].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int Note_Delete(int IDNote, int User)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@IDNote", IDNote);
                param[1] = new SqlParameter("@User", User);
                param[2] = new SqlParameter("@Res", 0);
                param[2].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("QLCS_Note_Remove", param);
                result = Convert.ToInt32(param[2].Value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void InsertUpdateDeleteNote(DateTime Ngay, int Chuong, string Note, int NguoiThucHien)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@Ngay", Ngay);
                param[1] = new SqlParameter("@Chuong", Chuong);
                param[2] = new SqlParameter("@Note", Note);
                param[3] = new SqlParameter("@NguoiThucHien", NguoiThucHien);
                DataProvider.ExecuteSP("QLCS_Note_InsertUpdateDelete", param);
            }
            catch (Exception ex)
            {
            }
        }

        public void InsertUpdateMultiNote(DateTime Ngay, string StrChuong, string Note, int NguoiThucHien, bool Replace)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@Ngay", Ngay);
                param[1] = new SqlParameter("@StrChuong", StrChuong);
                param[2] = new SqlParameter("@Note", Note);
                param[3] = new SqlParameter("@NguoiThucHien", NguoiThucHien);
                param[4] = new SqlParameter("@Replace", Replace);
                DataProvider.ExecuteSP("QLCS_Note_InsertUpdateMulti", param);
            }
            catch (Exception ex)
            {
            }
        }

        /*Vat Tu============================================================================================================*/

        public DataTable VatTu_GetByID(int IDVatTu)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IDVatTu", IDVatTu);
                result = DataProvider.SelectSP("QLCS_VatTu_GetByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void VatTu_Update(int IDVatTu, string TenVatTu, string DonViTinh)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@IDVatTu", IDVatTu);
                param[1] = new SqlParameter("@TenVatTu", TenVatTu);
                param[2] = new SqlParameter("@DonViTinh", DonViTinh);
                DataProvider.ExecuteSP("QLCS_VatTu_Update", param);
            }
            catch (Exception ex)
            {
            }
        }

        public DataTable VatTu_GetDonViTinh()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_VatTu_GetDonViTinh");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable VatTu_GetByString(string sID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@sID", sID);
                result = DataProvider.SelectSP("QLCS_VatTu_GetByString", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable VatTu_GetVatTuKhac()
        {
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_VatTu_GetVatTuKhac");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }

    public static class Config
    {
        private static string TABLE_NAME = "THEPAGE";
        private static string ProviderType = "data";

        private static string strConn = DotNetNuke.Common.Utilities.Config.GetConnectionString();
        private static string objectQualifier;
        private static string databaseOwner;
        private static string[] lstCol = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

        private static string GetFullyQualifiedName(string name)
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

        public static void LoadKhuChuong(System.Web.UI.WebControls.ListBox lst)
        {
            CaSauController csCont = new CaSauController();
            lst.DataSource = csCont.LoadKhuChuong();
            lst.DataTextField = "TenChuong";
            lst.DataValueField = "TenChuong";
            lst.DataBind();
        }

        private static string getConnectionString()
        {
            return DotNetNuke.Common.Utilities.Config.GetConnectionString();
        }

        public static DateTime? NgayKhoaSo()
        {
            DateTime? ret = null;
            DataTable result = null;
            try
            {
                result = DataProvider.SelectSP("QLCS_Config_Get");
                ret = Convert.ToDateTime(result.Rows[0]["NgayKhoaSo"]);
            }
            catch (Exception ex)
            {
            }
            return ret;
        }

        public static int Days360(DateTime? date, DateTime? initialDate)
        {
            return Days360(date.Value, initialDate.Value);
        }
         
        public static int Days360(DateTime date, DateTime initialDate)
        {
            DateTime dateA = initialDate;
            DateTime dateB = date;
            int dayA = dateA.Day;
            int dayB = dateB.Day;
            if (lastDayOfFebruary(dateA) && lastDayOfFebruary(dateB))
            dayB = 30;
             
            if (dayA == 31 && lastDayOfFebruary(dateA))
            dayA = 30;
            
            if (dayA == 30 && dayB == 31)
            dayB = 30;
             
            int days = (dateB.Year - dateA.Year) * 360 + ((dateB.Month + 1) - (dateA.Month + 1)) * 30 + (dayB - dayA);
            return days;
        }
         
        private static bool lastDayOfFebruary(DateTime date) 
        {
            int lastDay = DateTime.DaysInMonth(date.Year, 2);
            return date.Day == lastDay;
        }

        public static string ToXVal(object o)
        {
            System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
            if (o == DBNull.Value || o == null)
                return "";
            if ((decimal)o == 0)
            {
                return "";
            }
            else
            {
                return String.Format(ci, "{0:0.#####}", (decimal)o);
            }
        }

        public static string ToXVal1(object o)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            if (o == DBNull.Value || o == null)
                return "";
            return String.Format(ci, "{0:0.#####}", (decimal)o);
        }

        public static string ToXVal1(object o, int scale)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            if (o == DBNull.Value || o == null)
                return "";
            string f = "{0:0.#";
            for (int i = 1; i < scale; i++)
            {
                f += "#";
            }
            f += "}";
            return String.Format(ci, f, (decimal)o);
        }

        public static string ToXVal2(object o, int scale)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            if (o == DBNull.Value || o == null)
                return "";
            string f = "{0:#,0.0";
            if (scale == 0)
            {
                f = "{0:#,0}";
                if(o.GetType() == typeof(int))
                    return String.Format(ci, f, Convert.ToInt32(o));
                else
                    return String.Format(ci, f, Convert.ToDecimal(o));
            }
            else
            {
                for (int i = 1; i < scale; i++)
                {
                    f += "0";
                }
                f += "}";
                return String.Format(ci, f, Convert.ToDecimal(o)); 
            }
        }

        public static decimal ToDecimal(object o)
        {
            if (o == DBNull.Value || o == null)
                return 0;
            return Convert.ToDecimal(o);
        }

        public static double ToDouble(object o)
        {
            if (o == DBNull.Value || o == null)
                return 0;
            return (double)o;
        }

        public static string ExcelFormat(int scale)
        {
            string f = "";
            for (int i = 0; i < scale; i++)
            {
                f += "0";
            }
            string s = @"""\#\,\#\#0\." + f + @"""";
            return s;
        }

        public static string GetSelectedValues(System.Web.UI.WebControls.ListBox l)
        {
            string ret = "";
            foreach (int i in l.GetSelectedIndices())
            {
                ret += l.Items[i].Value + ", ";
            }
            return ret;
        }

        public static string GetSelectedValuesSQL(System.Web.UI.WebControls.ListBox l)
        {
            string ret = "";
            foreach (int i in l.GetSelectedIndices())
            {
                ret += "'" + l.Items[i].Value + "', ";
            }
            return ret;
        }

        public static string GetSelectedValues_At(System.Web.UI.WebControls.ListBox l)
        {
            string ret = "";
            foreach (int i in l.GetSelectedIndices())
            {
                ret += "@" + l.Items[i].Value + "@";
            }
            return ret;
        }

        public static string GetSelectedTexts(System.Web.UI.WebControls.ListBox l)
        {
            string ret = "";
            foreach (int i in l.GetSelectedIndices())
            {
                ret += l.Items[i].Text + ", ";
            }
            if (ret != "") ret = ret.Substring(0, ret.Length - 2);
            return ret;
        }

        public static string[] GetSelectedValue(System.Web.UI.WebControls.ListBox l, int index)
        {
            string[] ret = new string[2] {"", ""};
            int idx = 0;
            foreach (int i in l.GetSelectedIndices())
            {
                if (idx == index)
                {
                    ret[0] = l.Items[i].Value;
                    ret[1] = l.Items[i].Text;
                    break;
                }
                else
                {
                    idx++;
                }
            }
            return ret;
        }

        public static string GetSelectedTextByValue(System.Web.UI.WebControls.ListBox l, string value)
        {
            string ret = "";
            int idx = 0;
            foreach (int i in l.GetSelectedIndices())
            {
                if (l.Items[i].Value == value)
                {
                    ret = l.Items[i].Text;
                    break;
                }
            }
            return ret;
        }

        public static string GetTextByValue(System.Web.UI.WebControls.ListBox l, string value)
        {
            string ret = "";
            foreach (System.Web.UI.WebControls.ListItem i in l.Items)
            {
                if (i.Value == value)
                {
                    ret = i.Text;
                    break;
                }
            }
            return ret;
        }

        public static void SetSelectedValues(System.Web.UI.WebControls.ListBox l, string vals)
        {
            foreach (System.Web.UI.WebControls.ListItem i in l.Items)
            {
                i.Selected = false;
            }
            string[] v = vals.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in v)
            {
                foreach (System.Web.UI.WebControls.ListItem item in l.Items)
                {
                    if (item.Value == s)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }

        public static DataTable SelectSP(string strSQL, SqlParameter[] param)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = GetFullyQualifiedName(strSQL);
                command.CommandTimeout = 0;
                command.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter p in param)
                {
                    command.Parameters.Add(p);
                }
                SqlDataAdapter dataAdapt = new SqlDataAdapter();
                dataAdapt.SelectCommand = command;
                dataAdapt.Fill(dt);
            }
            return dt;
        }

        public static DataSet SelectSPs(string strSQL, SqlParameter[] param)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = GetFullyQualifiedName(strSQL);
                command.CommandTimeout = 0;
                command.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter p in param)
                {
                    command.Parameters.Add(p);
                }
                SqlDataAdapter dataAdapt = new SqlDataAdapter();
                dataAdapt.SelectCommand = command;
                dataAdapt.Fill(ds);
            }
            return ds;
        }

        public static string Space(int num)
        {
            string res = "";
            for (int i = 0; i < num; i++)
            {
                res += "&nbsp;";
            }
            return res;
        }

        public static string Footer(string place, string name)
        {
            string ret = "";
            DateTime now = DateTime.Now;
            if (place == null) place = "";
            ret += "<br/><div style='width:100%;text-align:right;'>" + place + ", ngày " + now.Day.ToString() + " tháng " + now.Month.ToString() + " năm " + now.Year.ToString() +"</div>";
            ret += "<br/><div style='width:100%;text-align:right;'>Người lập bảng" + Config.Space(12) + "</div>";
            ret += "<br/><br/><br/><br/>";
            ret += "<br/><div style='width:100%;text-align:right;'>" + name + Config.Space(12 - (name.Length - 14)/2) + "</div>";
            return ret;
        }

        public static string Split(string s, int groupCount)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string[] arr = s.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            int i = 1;
            foreach (string var in arr)
            {
                sb.Append(var + ", ");
                if (i == groupCount) { sb.Append("<br/>"); i = 0; }
                i++;
            }
            string res = sb.ToString();
            if (res.EndsWith(", ")) res = res.Substring(0, res.Length - 2);
            else if (res.EndsWith(", <br/>")) res = res.Substring(0, res.Length - 7);
            return res;
        }

        public static string GetExcelColFromColIndex(int colIndex)
        {
            try
            {
                return lstCol[colIndex - 1];
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static void prepareExcelFile(string name, System.Web.HttpResponse resp)
        {
            resp.ClearContent();
            resp.ClearHeaders();
            resp.AppendHeader("Content-Disposition", "attachment; filename=" + name + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
            resp.ContentType = "application/vnd.ms-excel";
        }

        public static void prepareTitleExcel(System.Text.StringBuilder sb, string tieude)
        {
            sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
        }

        public static void prepareTitleView(System.Text.StringBuilder sb, string tieude)
        {
            sb.Append("<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>");
        }

        public static void prepareTableHeaderExcel(System.Text.StringBuilder sb, string tableHeader)
        {
            sb.Append("<table border='1' id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
            sb.Append(tableHeader);
            sb.Append("</thead><tbody>");
        }

        public static void prepareTableHeaderView(System.Text.StringBuilder sb, string tableHeader)
        {
            sb.Append("<table id='thongke' class='stripe row-border order-column cell-border' cellspacing='0' width='100%'><thead>");
            sb.Append(tableHeader);
            sb.Append("</thead><tbody>");
        }

        public static void prepareFooterExcel(System.Text.StringBuilder sb)
        {
            sb.Append("<br/><div style='text-align:right;font-style:italic;'>Ninh Ích, ngày&nbsp;&nbsp;&nbsp;tháng&nbsp;&nbsp;&nbsp;&nbsp;năm&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div></tr>");
            sb.Append("<br/><div style='font-weight:bold;text-align:left;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Giám đốc&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Kế toán trưởng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;BP. Theo dõi&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Lãnh đạo trại</div></body></html>");
        }

        public static void prepareTableFooter(System.Text.StringBuilder sb)
        {
            sb.Append("</tbody></table>");
        }

        public delegate string CreateDataAndTieuDe();
        public delegate void CreateContent(System.Text.StringBuilder sb);
        public delegate string CreateTableHeader();
        public static void exportExcel(HttpResponse Response, System.Text.StringBuilder sb, string fileName, CreateDataAndTieuDe cdtd, CreateContent cc, CreateTableHeader cth)
        {
            prepareExcelFile(fileName, Response);

            string tieude = cdtd();

            prepareTitleExcel(sb, tieude);

            prepareTableHeaderExcel(sb, cth());

            cc(sb);

            prepareTableFooter(sb);

            prepareFooterExcel(sb);
        }

        public static void exportView(HttpResponse Response, System.Text.StringBuilder sb, CreateDataAndTieuDe cdtd, CreateContent cc, CreateTableHeader cth)
        {
            string tieude = cdtd();

            prepareTitleView(sb, tieude);

            prepareTableHeaderView(sb, cth());

            cc(sb);

            prepareTableFooter(sb);
        }
    }

    public class CaSauAnParam
    {
        public int LoaiCa;
        public int VatTu;
        public decimal KhoiLuong;
        public int SoLuongCa;
        public string StrSoLuongChuong;
        public string StrChuong;
        public string StrKL;
        public string StrPhanCachKhuChuong;

        public CaSauAnParam(int pLoaiCa, int pVatTu)
        { 
            LoaiCa = pLoaiCa;
            VatTu = pVatTu;
            KhoiLuong = 0;
            SoLuongCa = 0;
            StrSoLuongChuong = "";
            StrChuong = "";
            StrKL = "";
            StrPhanCachKhuChuong = "";
        }
    }

    public class KhayUm
    {
        public int ID;
        public string Name;
        public KhayUm(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }

    public class KhayAp
    {
        public int ID;
        public string Name;
        public KhayAp(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}