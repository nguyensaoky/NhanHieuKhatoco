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
using System.Data.OleDb;
using DotNetNuke.Framework.Providers;
using FileInfo = DotNetNuke.Services.FileSystem.FileInfo;
using DotNetNuke.Common.Utilities;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.GZip;

namespace DotNetNuke.Modules.QLCS
{
    public partial class r_casauan_thucanpivot : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        Dictionary<string, string> dicTenLoaiCa = new Dictionary<string,string>();

        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            System.Data.DataTable dtLoaiCa = csCont.LoadLoaiCa(1);
            ddlLoaiCa.DataSource = dtLoaiCa;
            ddlLoaiCa.DataTextField = "TenLoaiCa";
            ddlLoaiCa.DataValueField = "IDLoaiCa";
            ddlLoaiCa.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindControls();
                }
                dicTenLoaiCa.Clear();
                dicTenLoaiCa.Add("1", "CaCon");
                dicTenLoaiCa.Add("2", "MotNam");
                dicTenLoaiCa.Add("3", "ST1");
                dicTenLoaiCa.Add("4", "ST2");
                dicTenLoaiCa.Add("5", "HB1");
                dicTenLoaiCa.Add("-1", "HB2");
                dicTenLoaiCa.Add("6", "ChonGiong");
                dicTenLoaiCa.Add("7", "SS1");
                dicTenLoaiCa.Add("8", "SS2");
                dicTenLoaiCa.Add("9", "SS3");
                dicTenLoaiCa.Add("10", "SS2NuocMan");
                dicTenLoaiCa.Add("11", "SS1NuocMan");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private SqlParameter[] CreateParameters(int LoaiCa)
        {
            if (txtFromDate.Text == "")
            {
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            if (txtToDate.Text == "")
            {
                txtToDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            }

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@LoaiCa", LoaiCa);
            DateTime dtFrom = DateTime.Parse(txtFromDate.Text, ci);
            param[1] = new SqlParameter("@dateFrom", dtFrom);
            DateTime dtTo = DateTime.Parse(txtToDate.Text, ci);
            param[2] = new SqlParameter("@dateTo", dtTo.AddDays(1));
            return param;
        }

        private void PrepareResponse(string filename)
        {
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.ContentType = "application/vnd.ms-excel";
        }

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public static bool TryKillProcessByMainWindowHwnd(int hWnd)
        {
            uint processID;
            GetWindowThreadProcessId((IntPtr)hWnd, out processID);
            if (processID == 0) return false;
            try
            {
                System.Diagnostics.Process.GetProcessById((int)processID).Kill();
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }

        public static void KillProcessByMainWindowHwnd(int hWnd)
        {
            uint processID;
            GetWindowThreadProcessId((IntPtr)hWnd, out processID);
            if (processID == 0)
                throw new ArgumentException("Process has not been found by the given main window handle.", "hWnd");
            System.Diagnostics.Process.GetProcessById((int)processID).Kill();
        }

        public int DownloadMultiFiles(ArrayList FileList, string ZipFileName)
        {
            Crc32 objCrc32 = new Crc32();
            ZipOutputStream zos;
            zos = new ZipOutputStream(System.IO.File.Create(PortalSettings.HomeDirectoryMapPath + ZipFileName));
            foreach (string fileName in FileList)
            {
                FileStream strmFile = System.IO.File.OpenRead(fileName);
                byte[] abyBuffer = new byte[(int)(strmFile.Length)];
                strmFile.Read(abyBuffer, 0, abyBuffer.Length);
                ZipEntry objZipEntry = new ZipEntry(fileName.Substring(fileName.LastIndexOf("\\") + 1));
                objZipEntry.DateTime = DateTime.Now;
                objZipEntry.Size = strmFile.Length;
                strmFile.Close();
                objCrc32.Reset();
                objCrc32.Update(abyBuffer);
                objZipEntry.Crc = objCrc32.Value;
                zos.PutNextEntry(objZipEntry);
                zos.Write(abyBuffer, 0, abyBuffer.Length);
            }
            zos.Finish();
            zos.Close();

            int res = StreamFile(PortalSettings.HomeDirectoryMapPath + ZipFileName, ZipFileName);
            File.Delete(PortalSettings.HomeDirectoryMapPath + ZipFileName);
            foreach (string fileName in FileList)
            {
                File.Delete(fileName);
            }
            return res;
        }

        private int StreamFile(string FilePath, string DownloadAs)
        {
            DownloadAs = DownloadAs.Replace(" ", "_");

            System.IO.FileInfo objFile = new System.IO.FileInfo(FilePath);
            if (!objFile.Exists)
                return 0;
            
            System.Web.HttpResponse objResponse = System.Web.HttpContext.Current.Response;
            objResponse.ClearContent();
            objResponse.ClearHeaders();
            objResponse.AppendHeader("Content-Disposition", "attachment; filename=" + DownloadAs);
            objResponse.AppendHeader("Content-Length", objFile.Length.ToString());

            string strContentType;
            strContentType = "application/octet-stream";
            objResponse.ContentType = strContentType;
            WriteFile(objFile.FullName);

            objResponse.Flush();
            objResponse.Close();
            return 1;
        }

        public void WriteFile(string strFileName)
        {
            System.Web.HttpResponse objResponse = System.Web.HttpContext.Current.Response;
            System.IO.Stream objStream = null;
            byte[] bytBuffer = new byte[10000];
            int intLength;
            long lngDataToRead;
            try
            {
                objStream = new System.IO.FileStream(strFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                lngDataToRead = objStream.Length;
                objResponse.ContentType = "application/octet-stream";
                while(lngDataToRead > 0)
                {
                    if(objResponse.IsClientConnected)
                    {
                        intLength = objStream.Read(bytBuffer, 0, 10000);
                        objResponse.OutputStream.Write(bytBuffer, 0, intLength);
                        objResponse.Flush();
                        bytBuffer = new byte[10000];
                        lngDataToRead = lngDataToRead - intLength;
                    }
                    else
                    {
                        lngDataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.Write("Error : " + ex.Message);
            }
            finally
            {
                if(objStream!= null)
                    objStream.Close();
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string templateFileName = "TieuTonThucAn_TuNgayDenNgay_template.xlsx";
            string outputFileName = "TieuTonThucAn_";
            string now = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            ArrayList FileList = ProcessTieuTonThucAn(templateFileName, outputFileName, now);
            //Download
            if(FileList.Count > 0) DownloadMultiFiles(FileList, outputFileName + now + ".zip");
        }

        protected ArrayList ProcessTieuTonThucAn(string templateFileName, string outputFileName, string now)
        {
            string[] aLoaiCa = Config.GetSelectedTexts(ddlLoaiCa).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            Application _excelApp = new Application();
            int hWnd = _excelApp.Application.Hwnd;
            Workbook workBook = null;
            string fileName = PortalSettings.HomeDirectoryMapPath + templateFileName;
            string fileNameNew = PortalSettings.HomeDirectoryMapPath + outputFileName;
            ArrayList FileList = new ArrayList();
            int a = 0;
            try
            {
                //Thực hiện SQl để lấy dữ liệu vào 1 bảng tạm
                string strSQL = "QLCS_BCTK_CaAnTheoNgay_pivot";
                foreach (int i in ddlLoaiCa.GetSelectedIndices())
                {
                    SqlParameter[] param = CreateParameters(int.Parse(ddlLoaiCa.Items[i].Value));
                    DataSet ds = Config.SelectSPs(strSQL, param);
                    System.Data.DataTable dtThucAn = ds.Tables[0];
                    System.Data.DataTable dt = ds.Tables[1];

                    //Mở file excel template có sẵn
                    workBook = _excelApp.Workbooks.Open(fileName,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing);
                    //insert các dữ liệu từ bảng tạm vào các dòng trong file template
                    Worksheet sheet = (Worksheet)workBook.Sheets[1];
                    Range excelRange = sheet.UsedRange;
                    //Title
                    excelRange[5, 1] = "BIÊN BẢN THEO DÕI THỨC ĂN CỦA CÁ GIAI ĐOẠN " + aLoaiCa[a] + " TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text;
                    //Table header
                    Range rowHeader = ((Range)sheet.Cells[36, 1]).EntireRow;
                    rowHeader.Insert(XlInsertShiftDirection.xlShiftDown, false);
                    excelRange[36, 1] = "STT";
                    excelRange[36, 2] = "Ngày";
                    excelRange[36, 3] = "Số lượng cá";
                    int iThucAn = 0;
                    foreach (System.Data.DataRow rThucAn in dtThucAn.Rows)
                    {
                        excelRange[36, 4 + iThucAn] = rThucAn["TenVatTu"];
                        iThucAn++;
                    }
                    excelRange[36, 4 + iThucAn] = "Ghi chú";

                    //Table Content
                    int colCount = dt.Columns.Count;
                    int j = 0;
                    int k = 1;
                    decimal[] tongThucAn = new decimal[colCount - 2];
                    for (int m = 0; m < tongThucAn.Length; m++)
                    {
                        tongThucAn[m] = 0;
                    }
                    int rowIndex;
                    foreach (DataRow r in dt.Rows)
                    {
                        rowIndex = 37 + j;
                        Range row = ((Range)sheet.Cells[rowIndex, 1]).EntireRow;
                        row.Insert(XlInsertShiftDirection.xlShiftDown, false);
                        for (k = 1; k <= colCount; k++)
                        {
                            if (k == 1)
                                excelRange[rowIndex, k + 1] = ((DateTime)r[k - 1]).ToString("dd/MM/yyyy");
                            else
                            {
                                excelRange[rowIndex, k + 1] = r[k - 1];
                                if (k >= 3)
                                {
                                    if (r[k - 1] != DBNull.Value)
                                        tongThucAn[k - 3] += Convert.ToDecimal(r[k - 1]);
                                }
                            }
                        }
                        excelRange[rowIndex, 1] = j+1;
                        j++;
                    }
                    //Dòng tổng
                    rowIndex = 37 + j;
                    Range rowFooter = ((Range)sheet.Cells[rowIndex, 1]).EntireRow;
                    rowFooter.Insert(XlInsertShiftDirection.xlShiftDown, false);
                    excelRange[rowIndex, 2] = "Tổng cộng";
                    for (int m = 0; m < tongThucAn.Length; m++)
                    {
                        excelRange[rowIndex, m + 4] = tongThucAn[m];
                    }

                    //Style
                    //border toàn bộ
                    Range decoratedRange = sheet.get_Range("A36", Config.GetExcelColFromColIndex(k + 1) + ((int)rowIndex).ToString());
                    Borders border = decoratedRange.Borders;
                    border.LineStyle = XlLineStyle.xlContinuous;
                    border.Weight = 2d;
                    //bold, canh giữa dòng đầu
                    decoratedRange = sheet.get_Range("A36", Config.GetExcelColFromColIndex(k + 1) + "36");
                    decoratedRange.Font.Italic = false;
                    decoratedRange.Font.Bold = true;
                    decoratedRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    //bình thường các dòng trong
                    decoratedRange = sheet.get_Range("A37", Config.GetExcelColFromColIndex(k + 1) + ((int)(rowIndex - 1)).ToString());
                    decoratedRange.Font.Bold = false;
                    decoratedRange.Font.Italic = false;
                    //canh trái riêng cột ngày
                    decoratedRange = sheet.get_Range("B37", "B" + ((int)(rowIndex - 1)).ToString());
                    decoratedRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    //bold dòng cuối
                    decoratedRange = sheet.get_Range("A" + rowIndex.ToString(), Config.GetExcelColFromColIndex(k + 1) + rowIndex.ToString());
                    decoratedRange.Font.Bold = true;
                    decoratedRange.Font.Italic = false;

                    string FileName = fileNameNew + dicTenLoaiCa[ddlLoaiCa.Items[i].Value] + "_" + now + ".xlsx";
                    workBook.SaveAs(FileName, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, true, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    FileList.Add(FileName);

                    workBook.Close(false, fileName, null);
                    a++;
                }
                //Release component
                _excelApp.Quit();
                if(workBook != null) Marshal.FinalReleaseComObject(workBook);
                Marshal.FinalReleaseComObject(_excelApp.Workbooks);
                Marshal.FinalReleaseComObject(_excelApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                TryKillProcessByMainWindowHwnd(hWnd);
                return FileList;
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
                _excelApp.Quit();
                if(workBook != null) Marshal.FinalReleaseComObject(workBook);
                Marshal.FinalReleaseComObject(_excelApp.Workbooks);
                Marshal.FinalReleaseComObject(_excelApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                TryKillProcessByMainWindowHwnd(hWnd);
                return new ArrayList();
            }
        }
    }
}