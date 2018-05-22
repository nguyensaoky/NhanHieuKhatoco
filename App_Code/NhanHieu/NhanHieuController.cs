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
using DotNetNuke.NewsProvider;
using System.Collections.Specialized;
using DotNetNuke.Services.FileSystem;

namespace DotNetNuke.Modules.NhanHieu
{
    public class NhanHieuController
    {
        public NhanHieuController() { }

        public string GetFileNameByFileID(int FileID, int PortalID)
        {
            FileController fileCont = new FileController();
            FileInfo file = fileCont.GetFileById(FileID, PortalID);
            if (file != null) return file.FileName;
            return "";
        }

        public bool HasRole(string[] Roles, string ExpectRole)
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

        public DataTable NhanHieu_SelectByNhanHieuID(int ID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                result = DataProvider.SelectSP("NhanHieu_NhanHieu_SelectByID", param);
            }
            catch (Exception ex) { throw ex; }
            return result;
        }

        public void NhanHieu_Insert(String TenNhanHieu, Int32 NuocDangKy, String SoDon, DateTime? NgayNopDon, DateTime? NgayUuTien, String SoChungNhan, DateTime? NgayChungNhan, DateTime? NgayCongBo, String SoQuyetDinh, DateTime? NgayQuyetDinh, DateTime CreatedDate, Int32 CreatedUser, String CreatedUnit, DateTime ModifiedDate, Int32 ModifiedUser, String ModifiedUnit, String Note, Int32 NhanHieuGoc, string DonVi, out Int32 ID)
        {
            try
            {
                ID = -1;
                SqlParameter[] param = new SqlParameter[20];
                param[0] = new SqlParameter("@TenNhanHieu", TenNhanHieu);
                param[1] = new SqlParameter("@NuocDangKy", NuocDangKy);
                param[2] = new SqlParameter("@SoDon", SoDon);
                param[3] = new SqlParameter("@NgayNopDon", NgayNopDon);
                param[4] = new SqlParameter("@NgayUuTien", NgayUuTien);
                param[5] = new SqlParameter("@SoChungNhan", SoChungNhan);
                param[6] = new SqlParameter("@NgayChungNhan", NgayChungNhan);
                param[7] = new SqlParameter("@NgayCongBo", NgayCongBo);
                param[8] = new SqlParameter("@SoQuyetDinh", SoQuyetDinh);
                param[9] = new SqlParameter("@NgayQuyetDinh", NgayQuyetDinh);
                param[10] = new SqlParameter("@CreatedDate", CreatedDate);
                param[11] = new SqlParameter("@CreatedUser", CreatedUser);
                param[12] = new SqlParameter("@CreatedUnit", CreatedUnit);
                param[13] = new SqlParameter("@ModifiedDate", ModifiedDate);
                param[14] = new SqlParameter("@ModifiedUser", ModifiedUser);
                param[15] = new SqlParameter("@ModifiedUnit", ModifiedUnit);
                param[16] = new SqlParameter("@Note", Note);
                param[17] = new SqlParameter("@NhanHieuGoc", NhanHieuGoc);
                param[18] = new SqlParameter("@DonVi", DonVi);
                param[19] = new SqlParameter("@ID", ID);
                param[19].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("NhanHieu_NhanHieu_Insert", param);
                ID = Convert.ToInt32(param[19].Value);
            }
            catch (Exception ex) { throw ex; }
        }

        public void NhanHieu_Update(Int32 ID, String TenNhanHieu, Int32 NuocDangKy, String SoDon, DateTime? NgayNopDon, DateTime? NgayUuTien, String SoChungNhan, DateTime? NgayChungNhan, DateTime? NgayCongBo, String SoQuyetDinh, DateTime? NgayQuyetDinh, DateTime ModifiedDate, Int32 ModifiedUser, String ModifiedUnit, String Note, Int32 NhanHieuGoc)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[16];
                param[0] = new SqlParameter("@ID", ID);
                param[1] = new SqlParameter("@TenNhanHieu", TenNhanHieu);
                param[2] = new SqlParameter("@NuocDangKy", NuocDangKy);
                param[3] = new SqlParameter("@SoDon", SoDon);
                param[4] = new SqlParameter("@NgayNopDon", NgayNopDon);
                param[5] = new SqlParameter("@NgayUuTien", NgayUuTien);
                param[6] = new SqlParameter("@SoChungNhan", SoChungNhan);
                param[7] = new SqlParameter("@NgayChungNhan", NgayChungNhan);
                param[8] = new SqlParameter("@NgayCongBo", NgayCongBo);
                param[9] = new SqlParameter("@SoQuyetDinh", SoQuyetDinh);
                param[10] = new SqlParameter("@NgayQuyetDinh", NgayQuyetDinh);
                param[11] = new SqlParameter("@ModifiedDate", ModifiedDate);
                param[12] = new SqlParameter("@ModifiedUser", ModifiedUser);
                param[13] = new SqlParameter("@ModifiedUnit", ModifiedUnit);
                param[14] = new SqlParameter("@Note", Note);
                param[15] = new SqlParameter("@NhanHieuGoc", NhanHieuGoc);
                DataProvider.ExecuteSP("NhanHieu_NhanHieu_Update", param);
            }
            catch (Exception ex) { throw ex; }
        }

        public void NhanHieu_BienDong_Insert(Int32 NhanHieuID, Int32 Image, String MoTa, String MauSac, Int32 LoaiNhanHieu, String LinhVuc, String GhiChuThayDoi, DateTime CreatedDate, Int32 CreatedUser, String CreatedUnit, out Int32 ID)
        {
            try
            {
                ID = -1;
                SqlParameter[] param = new SqlParameter[11];
                param[0] = new SqlParameter("@NhanHieuID", NhanHieuID);
                param[1] = new SqlParameter("@Image", Image);
                param[2] = new SqlParameter("@MoTa", MoTa);
                param[3] = new SqlParameter("@MauSac", MauSac);
                param[4] = new SqlParameter("@LoaiNhanHieu", LoaiNhanHieu);
                param[5] = new SqlParameter("@LinhVuc", LinhVuc);
                param[6] = new SqlParameter("@GhiChuThayDoi", GhiChuThayDoi);
                param[7] = new SqlParameter("@CreatedDate", CreatedDate);
                param[8] = new SqlParameter("@CreatedUser", CreatedUser);
                param[9] = new SqlParameter("@CreatedUnit", CreatedUnit);
                param[10] = new SqlParameter("@ID", ID);
                param[10].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("NhanHieu_NhanHieu_BienDong_Insert", param);
                ID = Convert.ToInt32(param[10].Value);
            }
            catch (Exception ex) { throw ex; }
        }

        public void NhanHieu_BienDong_Update(Int32 ID, Int32 NhanHieuID, Int32 Image, String MoTa, String MauSac, Int32 LoaiNhanHieu, String LinhVuc, String GhiChuThayDoi, DateTime ModifiedDate, Int32 ModifiedUser, String ModifiedUnit)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[11];
                param[0] = new SqlParameter("@ID", ID);
                param[1] = new SqlParameter("@NhanHieuID", NhanHieuID);
                param[2] = new SqlParameter("@Image", Image);
                param[3] = new SqlParameter("@MoTa", MoTa);
                param[4] = new SqlParameter("@MauSac", MauSac);
                param[5] = new SqlParameter("@LoaiNhanHieu", LoaiNhanHieu);
                param[6] = new SqlParameter("@LinhVuc", LinhVuc);
                param[7] = new SqlParameter("@GhiChuThayDoi", GhiChuThayDoi);
                param[8] = new SqlParameter("@ModifiedDate", ModifiedDate);
                param[9] = new SqlParameter("@ModifiedUser", ModifiedUser);
                param[10] = new SqlParameter("@ModifiedUnit", ModifiedUnit);
                DataProvider.ExecuteSP("NhanHieu_NhanHieu_BienDong_Update", param);
            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable DanhMuc_SelectByType(String Type)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Type", Type);
                result = DataProvider.SelectSP("NhanHieu_DanhMuc_SelectByType", param);
            }
            catch (Exception ex) { throw ex; }
            return result;
        }

        public DataTable NhanHieu_GetAllExceptOne(Int32 ID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                result = DataProvider.SelectSP("NhanHieu_NhanHieu_GetAllExceptOne", param);
            }
            catch (Exception ex) { throw ex; }
            return result;
        }

        public void NhanHieu_CheckValid(int NhanHieuID, int BienDongID, bool Admin, string Website, int NextStatus, out bool Valid, out bool ShowDonVi, out string DonVi)
        {
            try
            {
                Valid = false;
                ShowDonVi = false;
                DonVi = "";
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@NhanHieuID", NhanHieuID);
                param[1] = new SqlParameter("@BienDongID", BienDongID);
                param[2] = new SqlParameter("@Admin", Admin);
                param[3] = new SqlParameter("@Website", Website);
                param[4] = new SqlParameter("@NextStatus", NextStatus);
                param[5] = new SqlParameter("@Valid", Valid);
                param[5].Direction = ParameterDirection.Output;
                param[6] = new SqlParameter("@ShowDonVi", ShowDonVi);
                param[6].Direction = ParameterDirection.Output;
                param[7] = new SqlParameter("@DonVi", DonVi);
                param[7].Direction = ParameterDirection.Output;
                DataProvider.ExecuteSP("NhanHieu_CheckValid", param);
                Valid = Convert.ToBoolean(param[5].Value);
                ShowDonVi = Convert.ToBoolean(param[6].Value);
                DonVi = param[7].Value.ToString();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}