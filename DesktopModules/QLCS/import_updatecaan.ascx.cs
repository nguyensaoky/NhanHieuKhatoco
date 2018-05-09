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
    public partial class import_updatecaan : DotNetNuke.Entities.Modules.PortalModuleBase
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

        private CaSauAnParam Find(ArrayList lstParam, int LoaiCa, int VatTu)
        {
            foreach (CaSauAnParam p in lstParam)
            {
                if (p.LoaiCa == LoaiCa && p.VatTu == VatTu)
                    return p;
            }
            return null;
        }

        private void importExcelThucAn(string dir, string file)
        {
            string columnName = "";
            string NguoiChoAn = "";
            decimal KhoiLuong = 0;
            int SoLuongCa = 0;
            int SoLuongTT;
            DateTime NgayAn = DateTime.MinValue;
            decimal value = 0;
            int idVatTu = 0;
            int SoCaAn = 0;
            string StrChuong = "";
            string StrSoLuongChuong = "";
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
                        ArrayList lstParam = new ArrayList();
                        DateTime NgayChoAn = DateTime.Parse(r["Ngay"].ToString() + "/" + r["Thang"].ToString() + "/" + r["Nam"].ToString() + " 23:00:00", culture);
                        DateTime NgayChoAnOutput = DateTime.MinValue;
                        int IDCaSauAn = csCont.CaSauAn_GetCaSauAnByNgay(NgayChoAn, out NgayChoAnOutput);
                        if (IDCaSauAn < 1)
                        {
                            //Hiện thông báo ngày này chưa có cho ăn
                            lblMessage.Text += "<br/>Sheet " + num.ToString() + ",dòng " + idx.ToString() + " chưa được cho ăn ngày này.";
                            continue;
                        }
                        if (NgayChoAn != NgayChoAnOutput) csCont.CaSauAn_Update(IDCaSauAn, NgayChoAn, UserId);
                        for (int i = 4; i < dTable.Columns.Count; i++)
                        {
                            columnName = dTable.Columns[i].ColumnName;
                            if (columnName.StartsWith("LoaiCa"))
                            {
                                LoaiCa = Convert.ToInt32(r[i]);
                                aChuongOnly = null;
                                aChuongExcept = null;
                            }
                            else if (columnName.StartsWith("Chuong-Only"))
                            {
                                string sChuongOnly = r[i].ToString();
                                sChuongOnly = sChuongOnly.Substring(1, sChuongOnly.Length - 2);
                                aChuongOnly = sChuongOnly.Split(new string[] { "!!" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            else if (columnName.StartsWith("Chuong-Except"))
                            {
                                string sChuongExcept = r[i].ToString();
                                sChuongExcept = sChuongExcept.Substring(1, sChuongExcept.Length - 2);
                                aChuongExcept = sChuongExcept.Split(new string[] { "!!" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            else
                            {
                                value = 0;
                                if (r[i] != DBNull.Value) value = Convert.ToDecimal(r[i]);
                                if (value != 0)
                                {
                                    idVatTu = int.Parse(columnName.Substring(columnName.IndexOf('(') + 1, columnName.IndexOf(')') - columnName.IndexOf('(') - 1));
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

                                    CaSauAnParam pF = Find(lstParam, LoaiCa, idVatTu);
                                    if (pF == null)
                                    {
                                        pF = new CaSauAnParam(LoaiCa, idVatTu);
                                        lstParam.Add(pF);
                                    }

                                    SoCaAn = 0;
                                    StrChuong = "";
                                    StrSoLuongChuong = "";
                                    StrKL = "";
                                    StrPhanCachKhuChuong = "";
                                    foreach (DataRow rC in tblChuong.Rows)
                                    {
                                        SoCaAn += Convert.ToInt32(rC["SoLuong"]);
                                    }
                                    pF.KhoiLuong += value;
                                    pF.SoLuongCa += SoCaAn;
                                    TongDoiChieu = 0;
                                    int h = 0;
                                    int countChuong = 0;
                                    khuchuong = "";
                                    index = 0;
                                    if (pF.StrChuong != "")
                                    {
                                        countChuong = pF.StrChuong.Substring(1, pF.StrChuong.Length - 2).Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries).Length;
                                    }
                                    foreach (DataRow rC in tblChuong.Rows)
                                    {
                                        StrChuong += "@" + rC["IDChuong"].ToString() + "@";
                                        kl = value * Convert.ToDecimal(rC["SoLuong"]) / Convert.ToDecimal(SoCaAn);
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
                                        currkhuchuong = rC["Chuong"].ToString().Substring(0, 2);
                                        if (currkhuchuong != khuchuong)
                                        {
                                            int temp = index + countChuong;
                                            StrPhanCachKhuChuong += "@" + temp.ToString() + "@";
                                            khuchuong = currkhuchuong;
                                        }
                                        index++;
                                    }
                                    int temp1 = index + countChuong;
                                    StrPhanCachKhuChuong += "@" + temp1.ToString() + "@";
                                    StrPhanCachKhuChuong = StrPhanCachKhuChuong.Substring(StrPhanCachKhuChuong.IndexOf("@@") + 1);
                                    pF.StrChuong += StrChuong;
                                    pF.StrKL += StrKL;
                                    pF.StrPhanCachKhuChuong += StrPhanCachKhuChuong;
                                    pF.StrSoLuongChuong += StrSoLuongChuong;

                                    //int res = csCont.CaSauAn_InsertUpdateThucAn(IDCaSauAn, idVatTu, value, LoaiCa, SoCaAn, SoCaTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL, StrPhanCachKhuChuong, NV, UserId, (!chkValueAdd.Checked));
                                    //if (res != 1) lblMessage.Text += "<br/>Sheet " + num.ToString() + ",dòng " + idx.ToString() + ", thức ăn " + r["VT"].ToString() + " không import được do số lượng hoặc thời điểm biến động không hợp lệ.";
                                }
                            }
                        }

                        foreach (CaSauAnParam p in lstParam)
                        {
                            //Xử lý từng cụm ở đây
                            int res = csCont.CaSauAn_UpdateThucAn_CoBan(IDCaSauAn, p.VatTu, p.KhoiLuong, p.LoaiCa, p.SoLuongCa, p.StrSoLuongChuong, p.StrChuong, p.StrKL, p.StrPhanCachKhuChuong);
                            if (res != 1) lblMessage.Text += "<br/>Sheet " + num.ToString() + ",dòng " + idx.ToString() + ", thức ăn " + p.VatTu.ToString() + ", loại cá " + p.LoaiCa.ToString() + " không import được do số lượng hoặc thời điểm biến động không hợp lệ.";
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
            string columnName = "";
            decimal KhoiLuong = 0;
            int SoLuongCa = 0;
            int SoLuongTT;
            DateTime NgayAn = DateTime.MinValue;
            decimal value = 0;
            int idVatTu = 0;
            int SoCaAn = 0;
            string StrChuong = "";
            string StrSoLuongChuong = "";
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
                        ArrayList lstParam = new ArrayList();
                        DateTime NgayChoAn = DateTime.Parse(r["Ngay"].ToString() + "/" + r["Thang"].ToString() + "/" + r["Nam"].ToString() + " 23:00:00", culture);
                        DateTime NgayChoAnOutput = DateTime.MinValue;
                        int IDCaSauAn = csCont.CaSauAn_GetCaSauAnByNgay(NgayChoAn, out NgayChoAnOutput);
                        if (IDCaSauAn < 1)
                        {
                            //Hiện thông báo ngày này chưa có cho ăn
                            lblMessage.Text += "<br/>Sheet " + num.ToString() + ",dòng " + idx.ToString() + " chưa được cho ăn ngày này.";
                            continue;
                        }
                        if (NgayChoAn != NgayChoAnOutput) csCont.CaSauAn_Update(IDCaSauAn, NgayChoAn, UserId);
                        for (int i = 4; i < dTable.Columns.Count; i++)
                        {
                            columnName = dTable.Columns[i].ColumnName;
                            if (columnName.StartsWith("LoaiCa"))
                            {
                                LoaiCa = Convert.ToInt32(r[i]);
                                aChuongOnly = null;
                                aChuongExcept = null;
                            }
                            else if (columnName.StartsWith("Chuong-Only"))
                            {
                                string sChuongOnly = r[i].ToString();
                                sChuongOnly = sChuongOnly.Substring(1, sChuongOnly.Length - 2);
                                aChuongOnly = sChuongOnly.Split(new string[] { "!!" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            else if (columnName.StartsWith("Chuong-Except"))
                            {
                                string sChuongExcept = r[i].ToString();
                                sChuongExcept = sChuongExcept.Substring(1, sChuongExcept.Length - 2);
                                aChuongExcept = sChuongExcept.Split(new string[] { "!!" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            else
                            {
                                value = 0;
                                if (r[i] != DBNull.Value) value = Convert.ToDecimal(r[i]);
                                if (value != 0)
                                {
                                    idVatTu = int.Parse(columnName.Substring(columnName.IndexOf('(') + 1, columnName.IndexOf(')') - columnName.IndexOf('(') - 1));
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

                                    CaSauAnParam pF = Find(lstParam, LoaiCa, idVatTu);
                                    if (pF == null)
                                    {
                                        pF = new CaSauAnParam(LoaiCa, idVatTu);
                                        lstParam.Add(pF);
                                    }

                                    SoCaAn = 0;
                                    StrChuong = "";
                                    StrSoLuongChuong = "";
                                    StrKL = "";
                                    StrPhanCachKhuChuong = "";
                                    foreach (DataRow rC in tblChuong.Rows)
                                    {
                                        SoCaAn += Convert.ToInt32(rC["SoLuong"]);
                                    }
                                    pF.KhoiLuong += value;
                                    pF.SoLuongCa += SoCaAn;
                                    TongDoiChieu = 0;
                                    int h = 0;
                                    int countChuong = 0;
                                    khuchuong = "";
                                    index = 0;
                                    if (pF.StrChuong != "")
                                    {
                                        countChuong = pF.StrChuong.Substring(1, pF.StrChuong.Length - 2).Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries).Length;
                                    }
                                    foreach (DataRow rC in tblChuong.Rows)
                                    {
                                        StrChuong += "@" + rC["IDChuong"].ToString() + "@";
                                        kl = value * Convert.ToDecimal(rC["SoLuong"]) / Convert.ToDecimal(SoCaAn);
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
                                        currkhuchuong = rC["Chuong"].ToString().Substring(0, 2);
                                        if (currkhuchuong != khuchuong)
                                        {
                                            int temp = index + countChuong;
                                            StrPhanCachKhuChuong += "@" + temp.ToString() + "@";
                                            khuchuong = currkhuchuong;
                                        }
                                        index++;
                                    }
                                    int temp1 = index + countChuong;
                                    StrPhanCachKhuChuong += "@" + temp1.ToString() + "@";
                                    StrPhanCachKhuChuong = StrPhanCachKhuChuong.Substring(StrPhanCachKhuChuong.IndexOf("@@") + 1);
                                    pF.StrChuong += StrChuong;
                                    pF.StrKL += StrKL;
                                    pF.StrPhanCachKhuChuong += StrPhanCachKhuChuong;
                                    pF.StrSoLuongChuong += StrSoLuongChuong;

                                    //int res = csCont.CaSauAn_InsertUpdateThucAn(IDCaSauAn, idVatTu, value, LoaiCa, SoCaAn, SoCaTT, StrSoLuongChuong, StrSoLuongChuongTT, StrChuong, StrKL, StrPhanCachKhuChuong, NV, UserId, (!chkValueAdd.Checked));
                                    //if (res != 1) lblMessage.Text += "<br/>Sheet " + num.ToString() + ",dòng " + idx.ToString() + ", thức ăn " + r["VT"].ToString() + " không import được do số lượng hoặc thời điểm biến động không hợp lệ.";
                                }
                            }
                        }

                        foreach (CaSauAnParam p in lstParam)
                        {
                            //Xử lý từng cụm ở đây
                            int res = csCont.CaSauAn_UpdateThuoc_CoBan(IDCaSauAn, p.VatTu, p.KhoiLuong, p.LoaiCa, p.SoLuongCa, p.StrSoLuongChuong, p.StrChuong, p.StrKL, p.StrPhanCachKhuChuong);
                            if (res != 1) lblMessage.Text += "<br/>Sheet " + num.ToString() + ",dòng " + idx.ToString() + ", thuốc " + p.VatTu.ToString() + ", loại cá " + p.LoaiCa.ToString() + " không import được do số lượng hoặc thời điểm biến động không hợp lệ.";
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