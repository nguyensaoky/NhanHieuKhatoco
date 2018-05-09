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
using System.Data.OleDb;
using System.Data.SqlClient;
using DotNetNuke.Framework.Providers;
using FileInfo = DotNetNuke.Services.FileSystem.FileInfo;
using DotNetNuke.Common.Utilities;
using System.IO;

namespace DotNetNuke.Modules.QLCS
{
    public partial class importcaan : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void cmdUploadThucAn_Click(object sender, EventArgs e)
        {
            // if no file is selected exit
            if (txtFileThucAn.PostedFile.FileName == "")
            {
                return;
            }

            string ParentFolderName = PortalSettings.HomeDirectoryMapPath;
            if (!System.IO.Directory.Exists(ParentFolderName + UserInfo.Username))
            {
                FileSystemUtils.AddFolder(PortalSettings, ParentFolderName, UserInfo.Username);
            }
            string strExtension = Path.GetExtension(txtFileThucAn.PostedFile.FileName).Replace(".", "");
            if (strExtension != "xls")
            {
                lblMessage.Text = "Chỉ chấp nhận phần mở rộng là file Excel";
                return;
            }
            else
            {
                if (System.IO.File.Exists(ParentFolderName + UserInfo.Username + "\\" + Path.GetFileName(txtFileThucAn.PostedFile.FileName)))
                    System.IO.File.Delete(ParentFolderName + UserInfo.Username + "\\" + Path.GetFileName(txtFileThucAn.PostedFile.FileName));
                lblMessage.Text = DotNetNuke.Common.Utilities.FileSystemUtils.UploadFile(ParentFolderName.Replace("/", "\\") + UserInfo.Username + "\\", txtFileThucAn.PostedFile, false);
            }

            importExcelThucAn(ParentFolderName.Replace("/", "\\") + UserInfo.Username + "\\", Path.GetFileName(txtFileThucAn.PostedFile.FileName));
        }

        protected void cmdUploadThuoc_Click(object sender, EventArgs e)
        {
            // if no file is selected exit
            if (txtFileThuoc.PostedFile.FileName == "")
            {
                return;
            }

            string ParentFolderName = PortalSettings.HomeDirectoryMapPath;
            if (!System.IO.Directory.Exists(ParentFolderName + UserInfo.Username))
            {
                FileSystemUtils.AddFolder(PortalSettings, ParentFolderName, UserInfo.Username);
            }
            string strExtension = Path.GetExtension(txtFileThuoc.PostedFile.FileName).Replace(".", "");
            if (strExtension != "xls")
            {
                lblMessage.Text = "Chỉ chấp nhận phần mở rộng là file Excel";
                return;
            }
            else
            {
                if (System.IO.File.Exists(ParentFolderName + UserInfo.Username + "\\" + Path.GetFileName(txtFileThuoc.PostedFile.FileName)))
                    System.IO.File.Delete(ParentFolderName + UserInfo.Username + "\\" + Path.GetFileName(txtFileThuoc.PostedFile.FileName));
                lblMessage.Text = DotNetNuke.Common.Utilities.FileSystemUtils.UploadFile(ParentFolderName.Replace("/", "\\") + UserInfo.Username + "\\", txtFileThuoc.PostedFile, false);
            }

            importExcelThuoc(ParentFolderName.Replace("/", "\\") + UserInfo.Username + "\\", Path.GetFileName(txtFileThuoc.PostedFile.FileName));
        }

        protected void cmdUpdateThucAnKhoiLuongChuong_Click(object sender, EventArgs e)
        {
            // if no file is selected exit
            if (txtFileThucAn.PostedFile.FileName == "")
            {
                return;
            }

            string ParentFolderName = PortalSettings.HomeDirectoryMapPath;
            if (!System.IO.Directory.Exists(ParentFolderName + UserInfo.Username))
            {
                FileSystemUtils.AddFolder(PortalSettings, ParentFolderName, UserInfo.Username);
            }
            string strExtension = Path.GetExtension(txtFileThucAn.PostedFile.FileName).Replace(".", "");
            if (strExtension != "xls")
            {
                lblMessage.Text = "Chỉ chấp nhận phần mở rộng là file Excel";
                return;
            }
            else
            {
                if (System.IO.File.Exists(ParentFolderName + UserInfo.Username + "\\" + Path.GetFileName(txtFileThucAn.PostedFile.FileName)))
                    System.IO.File.Delete(ParentFolderName + UserInfo.Username + "\\" + Path.GetFileName(txtFileThucAn.PostedFile.FileName));
                lblMessage.Text = DotNetNuke.Common.Utilities.FileSystemUtils.UploadFile(ParentFolderName.Replace("/", "\\") + UserInfo.Username + "\\", txtFileThucAn.PostedFile, false);
            }

            importExcelThucAnUpdate(ParentFolderName.Replace("/", "\\") + UserInfo.Username + "\\", Path.GetFileName(txtFileThucAn.PostedFile.FileName));
        }

        protected void cmdUpdateThuocKhoiLuongChuong_Click(object sender, EventArgs e)
        {
            // if no file is selected exit
            if (txtFileThuoc.PostedFile.FileName == "")
            {
                return;
            }

            string ParentFolderName = PortalSettings.HomeDirectoryMapPath;
            if (!System.IO.Directory.Exists(ParentFolderName + UserInfo.Username))
            {
                FileSystemUtils.AddFolder(PortalSettings, ParentFolderName, UserInfo.Username);
            }
            string strExtension = Path.GetExtension(txtFileThuoc.PostedFile.FileName).Replace(".", "");
            if (strExtension != "xls")
            {
                lblMessage.Text = "Chỉ chấp nhận phần mở rộng là file Excel";
                return;
            }
            else
            {
                if (System.IO.File.Exists(ParentFolderName + UserInfo.Username + "\\" + Path.GetFileName(txtFileThuoc.PostedFile.FileName)))
                    System.IO.File.Delete(ParentFolderName + UserInfo.Username + "\\" + Path.GetFileName(txtFileThuoc.PostedFile.FileName));
                lblMessage.Text = DotNetNuke.Common.Utilities.FileSystemUtils.UploadFile(ParentFolderName.Replace("/", "\\") + UserInfo.Username + "\\", txtFileThuoc.PostedFile, false);
            }

            importExcelThuocUpdate(ParentFolderName.Replace("/", "\\") + UserInfo.Username + "\\", Path.GetFileName(txtFileThuoc.PostedFile.FileName));
        }

        private void importExcelThucAn(string dir, string file)
        {
            string NguoiChoAn = "";
            decimal KhoiLuong = 0;
            int SoLuongCa = 0;
            int SoLuongTT = 0;
            DateTime NgayAn = DateTime.MinValue;
            decimal value = 0;
            int idVatTu = 0;
            int SoCaAn = 0;
            int SoCaTT = 0;
            string StrChuong = "";
            string StrSoLuongChuong = "";
            string StrSoLuongChuongTT = "";
            string StrPhanCachKhuChuong = "";
            string khuchuong = "";
            string currkhuchuong = "";
            int index = 0;
            string StrKL = "";
            decimal kl = 0;
            int LoaiCa = 0;
            string[] aChuongOnly = null;
            string[] aChuongExcept = null;
            decimal TongDoiChieu = 0;
            string f = "{0:0.";
            for (int i = 0; i < int.Parse(txtThapPhanTA.Text); i++)
            {
                f += "#";
            }
            f += "}";

            IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
            string Excelstrcon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir + file + @";Extended Properties=""Excel 8.0;HDR=YES;""";
            int numSheet = int.Parse(txtNumSheet.Text);
            for (int num = 1; num <= numSheet; num++)
            {
                string SQLstr = "SELECT * FROM [Sheet" + num.ToString() + "$]";
                OleDbConnection ExcelCon = new OleDbConnection(Excelstrcon);
                ExcelCon.Open();
                OleDbCommand dbCommand = new OleDbCommand(SQLstr, ExcelCon);
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);
                DataTable dTable = new DataTable();
                try
                {
                    dataAdapter.Fill(dTable);
                    int idx = 2;
                    foreach (DataRow r in dTable.Rows)
                    {
                        DateTime NgayChoAn = DateTime.Parse(r["Ngay"].ToString() + "/" + r["Thang"].ToString() + "/" + r["Nam"].ToString() + " 23:00:00", culture);
                        DateTime NgayChoAnOutput = DateTime.MinValue;
                        int IDCaSauAn = csCont.CaSauAn_GetCaSauAnByNgay(NgayChoAn, out NgayChoAnOutput);
                        if (IDCaSauAn != 0 && NgayChoAn != NgayChoAnOutput) csCont.CaSauAn_Update(IDCaSauAn, NgayChoAn, UserId);
                        if (idx == 2)
                        {
                            LoaiCa = Convert.ToInt32(r["LoaiCa"]);
                            aChuongOnly = null;
                            aChuongExcept = null;
                            string sChuongOnly = r["Chuong-Only"].ToString();
                            if (sChuongOnly != "")
                            {
                                sChuongOnly = sChuongOnly.Substring(1, sChuongOnly.Length - 2);
                                aChuongOnly = sChuongOnly.Split(new string[] { "!!" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            string sChuongExcept = r["Chuong-Except"].ToString();
                            if (sChuongExcept != "")
                            {
                                sChuongExcept = sChuongExcept.Substring(1, sChuongExcept.Length - 2);
                                aChuongExcept = sChuongExcept.Split(new string[] { "!!" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                        }
                        if (r["SL"] != DBNull.Value && r["SL"].ToString() != "" && Convert.ToDecimal(r["SL"]) != 0)
                        {
                            string NV = "";
                            if (r["NV"] != DBNull.Value) NV = r["NV"].ToString();
                            if (NV != "") NV = "@" + NV.Replace(" ","").Replace(",", "@@") + "@";
                            value = Convert.ToDecimal(r["SL"]);
                            string VT = r["VT"].ToString();
                            idVatTu = int.Parse(VT.Substring(VT.IndexOf('(') + 1, VT.IndexOf(')') - VT.IndexOf('(') - 1));
                            if (IDCaSauAn == 0) IDCaSauAn = csCont.CaSauAn_ThemMoi(NgayChoAn, UserId, "");

                            DataTable tblChuong = csCont.CaSauAn_GetChuongByThucAnByLoaiCa(IDCaSauAn, idVatTu, LoaiCa, out KhoiLuong, out SoLuongCa, out SoLuongTT, out NguoiChoAn, out NgayAn);
                            if (aChuongOnly != null)
                            {
                                for (int j = tblChuong.Rows.Count - 1; j > -1; j--)
                                {
                                    int k = 0;
                                    for (k = 0; k < aChuongOnly.Length; k++)
                                    {
                                        if (tblChuong.Rows[j]["Chuong"].ToString().Contains(aChuongOnly[k]))
                                        {
                                            break;
                                        }
                                    }
                                    if (k == aChuongOnly.Length) tblChuong.Rows.RemoveAt(j);
                                }
                            }
                            if (aChuongExcept != null)
                            {
                                for (int j = tblChuong.Rows.Count - 1; j > -1; j--)
                                {
                                    for (int k = 0; k < aChuongExcept.Length; k++)
                                    {
                                        if (tblChuong.Rows[j]["Chuong"].ToString().Contains(aChuongExcept[k]))
                                        {
                                            tblChuong.Rows.RemoveAt(j);
                                            break;
                                        }
                                    }
                                }
                            }
                            SoCaAn = 0;
                            SoCaTT = 0;
                            StrChuong = "";
                            StrSoLuongChuong = "";
                            StrSoLuongChuongTT = "";
                            StrKL = "";
                            foreach (DataRow rC in tblChuong.Rows)
                            {
                                SoCaAn += Convert.ToInt32(rC["SoLuong"]);
                                SoCaTT += Convert.ToInt32(rC["SoLuongTT"]);
                            }
                            TongDoiChieu = 0;
                            int h = 0;
                            foreach (DataRow rC in tblChuong.Rows)
                            {
                                StrChuong += "@" + rC["IDChuong"].ToString() + "@";
                                kl = value * Convert.ToDecimal(rC["SoLuong"]) / Convert.ToDecimal(SoCaAn);
                                //StrKL += "@" + String.Format("{0:0.#####}", kl).Replace(',', '.') + "@";
                                TongDoiChieu += Convert.ToDecimal(String.Format(f, kl));
                                if (h == tblChuong.Rows.Count - 1)
                                {
                                    if (TongDoiChieu != value)
                                    {
                                        decimal temp = Convert.ToDecimal(String.Format(f, kl)) + value - TongDoiChieu;
                                        StrKL += "@" +  String.Format(f, temp).Replace(',', '.') + "@";
                                    }
                                    else StrKL += "@" + String.Format(f, kl).Replace(',', '.') + "@";
                                }
                                else StrKL += "@" + String.Format(f, kl).Replace(',', '.') + "@";
                                h++;
                                StrSoLuongChuong += "@" + rC["SoLuong"].ToString() + "@";
                                StrSoLuongChuongTT += "@" + rC["SoLuongTT"].ToString() + "@";
                                currkhuchuong = rC["Chuong"].ToString().Substring(0, 2);
                                if (currkhuchuong != khuchuong)
                                {
                                    StrPhanCachKhuChuong += "@" + index + "@";
                                    khuchuong = currkhuchuong;
                                }
                                index++;
                            }
                            StrPhanCachKhuChuong = StrPhanCachKhuChuong.Substring(3) + "@" + index.ToString() + "@";
                            int res = csCont.CaSauAn_InsertUpdateThucAn(IDCaSauAn, idVatTu, value, LoaiCa, SoCaAn, SoCaTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL, StrPhanCachKhuChuong, NV, UserId, (!chkValueAdd.Checked));
                            if (res != 1) lblMessage.Text += "<br/>Sheet " + num.ToString() + ",dòng " + idx.ToString() + ", thức ăn " + r["VT"].ToString() + " không import được do số lượng hoặc thời điểm biến động không hợp lệ.";
                        }
                        idx++;
                    }

                    // dispose used objects            
                    dTable.Dispose();
                    dataAdapter.Dispose();
                    dbCommand.Dispose();
                    ExcelCon.Close();
                    ExcelCon.Dispose();

                    lblMessage.Text += "<br/>Đã import xong Sheet " + num.ToString() + "!";
                }
                catch (Exception ex)
                {
                    // dispose used objects            
                    dTable.Dispose();
                    dataAdapter.Dispose();
                    dbCommand.Dispose();
                    ExcelCon.Close();
                    ExcelCon.Dispose();
                    Response.Write(ex.ToString());
                }
            }
        }

        private void importExcelThuoc(string dir, string file)
        {
            DateTime NgayAn = DateTime.MinValue;
            decimal KhoiLuong = 0;
            int SoLuongCa = 0;
            int SoLuongTT = 0;
            decimal value = 0;
            int idVatTu = 0;
            int SoCaAn = 0;
            int SoCaTT = 0;
            string StrChuong = "";
            string StrSoLuongChuong = "";
            string StrSoLuongChuongTT = "";
            string StrPhanCachKhuChuong = "";
            string khuchuong = "";
            string currkhuchuong = "";
            int index = 0;
            string StrKL = "";
            decimal kl = 0;
            int LoaiCa = 0;
            string[] aChuongOnly = null;
            string[] aChuongExcept = null;
            decimal TongDoiChieu = 0;
            string f = "{0:0.";
            for (int i = 0; i < int.Parse(txtThapPhanT.Text); i++)
            {
                f += "#";
            }
            f += "}";

            IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
            string Excelstrcon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir + file + @";Extended Properties=""Excel 8.0;HDR=YES;""";
            int numSheet = int.Parse(txtNumSheet.Text);
            for (int num = 1; num <= numSheet; num++)
            {
                string SQLstr = "SELECT * FROM [Sheet" + num.ToString() + "$]";
                OleDbConnection ExcelCon = new OleDbConnection(Excelstrcon);
                ExcelCon.Open();
                OleDbCommand dbCommand = new OleDbCommand(SQLstr, ExcelCon);
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);
                DataTable dTable = new DataTable();
                try
                {
                    dataAdapter.Fill(dTable);
                    int idx = 2;
                    foreach (DataRow r in dTable.Rows)
                    {
                        DateTime NgayChoAn = DateTime.Parse(r["Ngay"].ToString() + "/" + r["Thang"].ToString() + "/" + r["Nam"].ToString() + " 23:00:00", culture);
                        DateTime NgayChoAnOutput = DateTime.MinValue;
                        int IDCaSauAn = csCont.CaSauAn_GetCaSauAnByNgay(NgayChoAn, out NgayChoAnOutput);
                        if (IDCaSauAn != 0 && NgayChoAn != NgayChoAnOutput) csCont.CaSauAn_Update(IDCaSauAn, NgayChoAn, UserId);
                        if (idx == 2)
                        {
                            LoaiCa = Convert.ToInt32(r["LoaiCa"]);
                            aChuongOnly = null;
                            aChuongExcept = null;
                            string sChuongOnly = r["Chuong-Only"].ToString();
                            if (sChuongOnly != "")
                            {
                                sChuongOnly = sChuongOnly.Substring(1, sChuongOnly.Length - 2);
                                aChuongOnly = sChuongOnly.Split(new string[] { "!!" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            string sChuongExcept = r["Chuong-Except"].ToString();
                            if (sChuongExcept != "")
                            {
                                sChuongExcept = sChuongExcept.Substring(1, sChuongExcept.Length - 2);
                                aChuongExcept = sChuongExcept.Split(new string[] { "!!" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                        }
                        if (r["SL"] != DBNull.Value && r["SL"].ToString() != "" && Convert.ToDecimal(r["SL"]) != 0)
                        {
                            value = Convert.ToDecimal(r["SL"]);
                            string VT = r["VT"].ToString();
                            idVatTu = int.Parse(VT.Substring(VT.IndexOf('(') + 1, VT.IndexOf(')') - VT.IndexOf('(') - 1));
                            if (IDCaSauAn == 0) IDCaSauAn = csCont.CaSauAn_ThemMoi(NgayChoAn, UserId, "");

                            DataTable tblChuong = csCont.CaSauAn_GetChuongByThuocByLoaiCa(IDCaSauAn, idVatTu, LoaiCa, out KhoiLuong, out SoLuongCa, out SoLuongTT, out NgayAn);
                            if (aChuongOnly != null)
                            {
                                for (int j = tblChuong.Rows.Count - 1; j > -1; j--)
                                {
                                    int k = 0;
                                    for (k = 0; k < aChuongOnly.Length; k++)
                                    {
                                        if (tblChuong.Rows[j]["Chuong"].ToString().Contains(aChuongOnly[k]))
                                        {
                                            break;
                                        }
                                    }
                                    if (k == aChuongOnly.Length) tblChuong.Rows.RemoveAt(j);
                                }
                            }
                            if (aChuongExcept != null)
                            {
                                for (int j = tblChuong.Rows.Count - 1; j > -1; j--)
                                {
                                    for (int k = 0; k < aChuongExcept.Length; k++)
                                    {
                                        if (tblChuong.Rows[j]["Chuong"].ToString().Contains(aChuongExcept[k]))
                                        {
                                            tblChuong.Rows.RemoveAt(j);
                                            break;
                                        }
                                    }
                                }
                            }
                            SoCaAn = 0;
                            SoCaTT = 0;
                            StrChuong = "";
                            StrSoLuongChuong = "";
                            StrSoLuongChuongTT = "";
                            StrKL = "";
                            foreach (DataRow rC in tblChuong.Rows)
                            {
                                SoCaAn += Convert.ToInt32(rC["SoLuong"]);
                                SoCaTT += Convert.ToInt32(rC["SoLuongTT"]);
                            }
                            TongDoiChieu = 0;
                            int h = 0;
                            foreach (DataRow rC in tblChuong.Rows)
                            {
                                StrChuong += "@" + rC["IDChuong"].ToString() + "@";
                                kl = value * Convert.ToDecimal(rC["SoLuong"]) / Convert.ToDecimal(SoCaAn);
                                //StrKL += "@" + String.Format("{0:0.#####}", kl).Replace(',', '.') + "@";
                                TongDoiChieu += Convert.ToDecimal(String.Format(f, kl));
                                if (h == tblChuong.Rows.Count - 1)
                                {
                                    if (TongDoiChieu != value)
                                    {
                                        decimal temp = Convert.ToDecimal(String.Format(f, kl)) + value - TongDoiChieu;
                                        StrKL += "@" + String.Format(f, temp).Replace(',', '.') + "@";
                                    }
                                    else StrKL += "@" + String.Format(f, kl).Replace(',', '.') + "@";
                                }
                                else StrKL += "@" + String.Format(f, kl).Replace(',', '.') + "@";
                                h++;
                                StrSoLuongChuong += "@" + rC["SoLuong"].ToString() + "@";
                                StrSoLuongChuongTT += "@" + rC["SoLuongTT"].ToString() + "@";
                                currkhuchuong = rC["Chuong"].ToString().Substring(0, 2);
                                if (currkhuchuong != khuchuong)
                                {
                                    StrPhanCachKhuChuong += "@" + index + "@";
                                    khuchuong = currkhuchuong;
                                }
                                index++;
                            }
                            StrPhanCachKhuChuong = StrPhanCachKhuChuong.Substring(3) + "@" + index.ToString() + "@";
                            int res = csCont.CaSauAn_InsertUpdateThuoc(IDCaSauAn, idVatTu, value, LoaiCa, SoCaAn, SoLuongTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL, StrPhanCachKhuChuong, UserId, (!chkValueAdd.Checked));
                            if (res != 1) lblMessage.Text += "<br/>Sheet " + num.ToString() + ",dòng " + idx.ToString() + ", thuốc " + r["VT"].ToString() + " không import được do số lượng hoặc thời điểm biến động không hợp lệ.";
                        }
                        idx++;
                    }

                    // dispose used objects            
                    dTable.Dispose();
                    dataAdapter.Dispose();
                    dbCommand.Dispose();
                    ExcelCon.Close();
                    ExcelCon.Dispose();

                    lblMessage.Text += "<br/>Đã import xong Sheet " + num.ToString() + "!";
                }
                catch (Exception ex)
                {
                    // dispose used objects            
                    dTable.Dispose();
                    dataAdapter.Dispose();
                    dbCommand.Dispose();
                    ExcelCon.Close();
                    ExcelCon.Dispose();
                    Response.Write(ex.ToString());
                }
            }
        }

        private void importExcelThucAnUpdate(string dir, string file)
        {
            string NguoiChoAn = "";
            int SoCaAn = 0;
            decimal KhoiLuong = 0;
            int SoLuongCa = 0;
            int SoLuongTT = 0;
            DateTime NgayAn = DateTime.MinValue;
            decimal value = 0;
            int idVatTu = 0;
            string StrChuong = "";
            string StrKL = "";
            decimal kl = 0;
            int LoaiCa = 0;
            string[] aChuongOnly = null;
            string[] aChuongExcept = null;

            string f = "{0:0.";
            for (int i = 0; i < int.Parse(txtThapPhanTA.Text); i++)
            {
                f += "#";
            }
            f += "}";

            IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
            string Excelstrcon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir + file + @";Extended Properties=""Excel 8.0;HDR=YES;""";
            int numSheet = int.Parse(txtNumSheet.Text);
            for (int num = 1; num <= numSheet; num++)
            {
                string SQLstr = "SELECT * FROM [Sheet" + num.ToString() + "$]";
                OleDbConnection ExcelCon = new OleDbConnection(Excelstrcon);
                ExcelCon.Open();
                OleDbCommand dbCommand = new OleDbCommand(SQLstr, ExcelCon);
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);
                DataTable dTable = new DataTable();
                try
                {
                    dataAdapter.Fill(dTable);
                    int idx = 2;
                    foreach (DataRow r in dTable.Rows)
                    {
                        DateTime NgayChoAn = DateTime.Parse(r["Ngay"].ToString() + "/" + r["Thang"].ToString() + "/" + r["Nam"].ToString() + " 23:00:00", culture);
                        DateTime NgayChoAnOutput = DateTime.MinValue;
                        int IDCaSauAn = csCont.CaSauAn_GetCaSauAnByNgay(NgayChoAn, out NgayChoAnOutput);
                        if (IDCaSauAn != 0 && NgayChoAn != NgayChoAnOutput) csCont.CaSauAn_Update(IDCaSauAn, NgayChoAn, UserId);
                        if (idx == 2)
                        {
                            LoaiCa = Convert.ToInt32(r["LoaiCa"]);
                            aChuongOnly = null;
                            aChuongExcept = null;
                            string sChuongOnly = r["Chuong-Only"].ToString();
                            if (sChuongOnly != "")
                            {
                                sChuongOnly = sChuongOnly.Substring(1, sChuongOnly.Length - 2);
                                aChuongOnly = sChuongOnly.Split(new string[] { "!!" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            string sChuongExcept = r["Chuong-Except"].ToString();
                            if (sChuongExcept != "")
                            {
                                sChuongExcept = sChuongExcept.Substring(1, sChuongExcept.Length - 2);
                                aChuongExcept = sChuongExcept.Split(new string[] { "!!" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                        }
                        if (r["SL"] != DBNull.Value && r["SL"].ToString() != "" && Convert.ToDecimal(r["SL"]) != 0)
                        {
                            value = Convert.ToDecimal(r["SL"]);
                            string VT = r["VT"].ToString();
                            idVatTu = int.Parse(VT.Substring(VT.IndexOf('(') + 1, VT.IndexOf(')') - VT.IndexOf('(') - 1));
                            if (IDCaSauAn == 0) IDCaSauAn = csCont.CaSauAn_ThemMoi(NgayChoAn, UserId, "");

                            DataTable tblChuong = csCont.CaSauAn_GetChuongByThucAnByLoaiCa(IDCaSauAn, idVatTu, LoaiCa, out KhoiLuong, out SoLuongCa, out SoLuongTT, out NguoiChoAn, out NgayAn);
                            if (aChuongOnly != null)
                            {
                                for (int j = tblChuong.Rows.Count - 1; j > -1; j--)
                                {
                                    int k = 0;
                                    for (k = 0; k < aChuongOnly.Length; k++)
                                    {
                                        if (tblChuong.Rows[j]["Chuong"].ToString().Contains(aChuongOnly[k]))
                                        {
                                            break;
                                        }
                                    }
                                    if (k == aChuongOnly.Length) tblChuong.Rows.RemoveAt(j);
                                }
                            }
                            if (aChuongExcept != null)
                            {
                                for (int j = tblChuong.Rows.Count - 1; j > -1; j--)
                                {
                                    for (int k = 0; k < aChuongExcept.Length; k++)
                                    {
                                        if (tblChuong.Rows[j]["Chuong"].ToString().Contains(aChuongExcept[k]))
                                        {
                                            tblChuong.Rows.RemoveAt(j);
                                            break;
                                        }
                                    }
                                }
                            }
                            SoCaAn = 0;
                            StrChuong = "";
                            StrKL = "";
                            foreach (DataRow rC in tblChuong.Rows)
                            {
                                SoCaAn += Convert.ToInt32(rC["SoLuong"]);
                            }

                            foreach (DataRow rC in tblChuong.Rows)
                            {
                                StrChuong += "@" + rC["IDChuong"].ToString() + "@";
                                kl = value * Convert.ToDecimal(rC["SoLuong"]) / Convert.ToDecimal(SoCaAn);
                                //StrKL += "@" + String.Format("{0:0.#####}", kl).Replace(',', '.') + "@";
                                StrKL += "@" + String.Format(f, kl).Replace(',', '.') + "@";
                            }

                            csCont.CaSauAn_ThucAn_UpdateKhoiLuongChuong(IDCaSauAn, idVatTu, LoaiCa, StrChuong, StrKL);
                        }
                        idx++;
                    }

                    // dispose used objects            
                    dTable.Dispose();
                    dataAdapter.Dispose();
                    dbCommand.Dispose();
                    ExcelCon.Close();
                    ExcelCon.Dispose();

                    lblMessage.Text += "<br/>Đã import xong Sheet " + num.ToString() + "!";
                }
                catch (Exception ex)
                {
                    // dispose used objects            
                    dTable.Dispose();
                    dataAdapter.Dispose();
                    dbCommand.Dispose();
                    ExcelCon.Close();
                    ExcelCon.Dispose();
                    Response.Write(ex.ToString());
                }
            }
        }

        private void importExcelThuocUpdate(string dir, string file)
        {
            DateTime NgayAn = DateTime.MinValue;
            int SoCaAn = 0;
            decimal KhoiLuong = 0;
            int SoLuongCa = 0;
            int SoLuongTT = 0;
            decimal value = 0;
            int idVatTu = 0;
            string StrChuong = "";
            string StrKL = "";
            decimal kl = 0;
            int LoaiCa = 0;
            string[] aChuongOnly = null;
            string[] aChuongExcept = null;

            string f = "{0:0.";
            for (int i = 0; i < int.Parse(txtThapPhanT.Text); i++)
            {
                f += "#";
            }
            f += "}";

            IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
            string Excelstrcon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir + file + @";Extended Properties=""Excel 8.0;HDR=YES;""";
            int numSheet = int.Parse(txtNumSheet.Text);
            for (int num = 1; num <= numSheet; num++)
            {
                string SQLstr = "SELECT * FROM [Sheet" + num.ToString() + "$]";
                OleDbConnection ExcelCon = new OleDbConnection(Excelstrcon);
                ExcelCon.Open();
                OleDbCommand dbCommand = new OleDbCommand(SQLstr, ExcelCon);
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);
                DataTable dTable = new DataTable();
                try
                {
                    dataAdapter.Fill(dTable);
                    int idx = 2;
                    foreach (DataRow r in dTable.Rows)
                    {
                        DateTime NgayChoAn = DateTime.Parse(r["Ngay"].ToString() + "/" + r["Thang"].ToString() + "/" + r["Nam"].ToString() + " 23:00:00", culture);
                        DateTime NgayChoAnOutput = DateTime.MinValue;
                        int IDCaSauAn = csCont.CaSauAn_GetCaSauAnByNgay(NgayChoAn, out NgayChoAnOutput);
                        if (IDCaSauAn != 0 && NgayChoAn != NgayChoAnOutput) csCont.CaSauAn_Update(IDCaSauAn, NgayChoAn, UserId);
                        if (idx == 2)
                        {
                            LoaiCa = Convert.ToInt32(r["LoaiCa"]);
                            aChuongOnly = null;
                            aChuongExcept = null;
                            string sChuongOnly = r["Chuong-Only"].ToString();
                            if (sChuongOnly != "")
                            {
                                sChuongOnly = sChuongOnly.Substring(1, sChuongOnly.Length - 2);
                                aChuongOnly = sChuongOnly.Split(new string[] { "!!" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            string sChuongExcept = r["Chuong-Except"].ToString();
                            if (sChuongExcept != "")
                            {
                                sChuongExcept = sChuongExcept.Substring(1, sChuongExcept.Length - 2);
                                aChuongExcept = sChuongExcept.Split(new string[] { "!!" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                        }
                        if (r["SL"] != DBNull.Value && r["SL"].ToString() != "" && Convert.ToDecimal(r["SL"]) != 0)
                        {
                            value = Convert.ToDecimal(r["SL"]);
                            string VT = r["VT"].ToString();
                            idVatTu = int.Parse(VT.Substring(VT.IndexOf('(') + 1, VT.IndexOf(')') - VT.IndexOf('(') - 1));
                            if (IDCaSauAn == 0) IDCaSauAn = csCont.CaSauAn_ThemMoi(NgayChoAn, UserId, "");

                            DataTable tblChuong = csCont.CaSauAn_GetChuongByThuocByLoaiCa(IDCaSauAn, idVatTu, LoaiCa, out KhoiLuong, out SoLuongCa, out SoLuongTT, out NgayAn);
                            if (aChuongOnly != null)
                            {
                                for (int j = tblChuong.Rows.Count - 1; j > -1; j--)
                                {
                                    int k = 0;
                                    for (k = 0; k < aChuongOnly.Length; k++)
                                    {
                                        if (tblChuong.Rows[j]["Chuong"].ToString().Contains(aChuongOnly[k]))
                                        {
                                            break;
                                        }
                                    }
                                    if (k == aChuongOnly.Length) tblChuong.Rows.RemoveAt(j);
                                }
                            }
                            if (aChuongExcept != null)
                            {
                                for (int j = tblChuong.Rows.Count - 1; j > -1; j--)
                                {
                                    for (int k = 0; k < aChuongExcept.Length; k++)
                                    {
                                        if (tblChuong.Rows[j]["Chuong"].ToString().Contains(aChuongExcept[k]))
                                        {
                                            tblChuong.Rows.RemoveAt(j);
                                            break;
                                        }
                                    }
                                }
                            }
                            SoCaAn = 0;
                            StrChuong = "";
                            StrKL = "";
                            foreach (DataRow rC in tblChuong.Rows)
                            {
                                SoCaAn += Convert.ToInt32(rC["SoLuong"]);
                            }

                            foreach (DataRow rC in tblChuong.Rows)
                            {
                                StrChuong += "@" + rC["IDChuong"].ToString() + "@";
                                kl = value * Convert.ToDecimal(rC["SoLuong"]) / Convert.ToDecimal(SoCaAn);
                                //StrKL += "@" + String.Format("{0:0.#####}", kl).Replace(',', '.') + "@";
                                StrKL += "@" + String.Format(f, kl).Replace(',', '.') + "@";
                            }

                            csCont.CaSauAn_Thuoc_UpdateKhoiLuongChuong(IDCaSauAn, idVatTu, LoaiCa, StrChuong, StrKL);
                        }
                        idx++;
                    }

                    // dispose used objects            
                    dTable.Dispose();
                    dataAdapter.Dispose();
                    dbCommand.Dispose();
                    ExcelCon.Close();
                    ExcelCon.Dispose();

                    lblMessage.Text += "<br/>Đã import xong Sheet " + num.ToString() + "!";
                }
                catch (Exception ex)
                {
                    // dispose used objects            
                    dTable.Dispose();
                    dataAdapter.Dispose();
                    dbCommand.Dispose();
                    ExcelCon.Close();
                    ExcelCon.Dispose();
                    Response.Write(ex.ToString());
                }
            }
        }
    }
}