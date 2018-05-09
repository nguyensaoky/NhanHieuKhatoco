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
    public partial class importgietmo_updatePPM : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void cmdUpload_Click(object sender, EventArgs e)
        {
            // if no file is selected exit
            if (txtFile.PostedFile.FileName == "")
            {
                return;
            }

            string ParentFolderName = PortalSettings.HomeDirectoryMapPath;
            if (!System.IO.Directory.Exists(ParentFolderName + UserInfo.Username))
            {
                FileSystemUtils.AddFolder(PortalSettings, ParentFolderName, UserInfo.Username);
            }
            string strExtension = Path.GetExtension(txtFile.PostedFile.FileName).Replace(".", "");
            if (strExtension != "xls")
            {
                lblMessage.Text = "Chỉ chấp nhận phần mở rộng là file Excel";
                return;
            }
            else
            {
                if (System.IO.File.Exists(ParentFolderName + UserInfo.Username + "\\" + Path.GetFileName(txtFile.PostedFile.FileName)))
                    System.IO.File.Delete(ParentFolderName + UserInfo.Username + "\\" + Path.GetFileName(txtFile.PostedFile.FileName));
                lblMessage.Text = DotNetNuke.Common.Utilities.FileSystemUtils.UploadFile(ParentFolderName.Replace("/", "\\") + UserInfo.Username + "\\", txtFile.PostedFile, false);
            }

            importExcel(ParentFolderName.Replace("/", "\\") + UserInfo.Username + "\\", Path.GetFileName(txtFile.PostedFile.FileName));
        }

        private void importExcel(string dir, string file)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
            string Excelstrcon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir + file + @";Extended Properties=""Excel 8.0;HDR=YES;""";

            string SQLstr = "SELECT * FROM [Sheet3$]";
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
                    int GMC = 0;
                    int Ngay = Convert.ToInt32(r["Ngay"]);
                    int Thang = Convert.ToInt32(r["Thang"]);
                    int Nam = Convert.ToInt32(r["Nam"]);
                    int LoaiCa = Convert.ToInt32(r["LoaiCa"]);
                    int Chuong = Convert.ToInt32(r["Chuong"]);
                    int Da_Bung = Convert.ToInt32(r["Da_Bung"]);
                    int Da_PhanLoai = Convert.ToInt32(r["Da_PhanLoai"]);
                    decimal TLH = Convert.ToDecimal(r["TLH"]);
                    decimal TLMH = Convert.ToDecimal(r["TLMH"]);
                    string PPM = r["PPM"].ToString();
                    int Res = csCont.GietMoCa_UpdatePhuongPhapMo(Ngay, Thang, Nam, LoaiCa, Chuong, Da_Bung, Da_PhanLoai, TLH, TLMH, PPM);
                    if (Res == 0)
                    {
                        lblMessage.Text += "<br/>Dòng " + idx.ToString() + " trong chi tiết cá không import được.";
                    }
                    idx++;
                }

                // dispose used objects            
                dTable.Dispose();
                dataAdapter.Dispose();
                dbCommand.Dispose();
                ExcelCon.Close();
                ExcelCon.Dispose();

                lblMessage.Text += "<br/>Đã import xong!";
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

        protected void cmdTraoDoiSanPham_Click(object sender, EventArgs e)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
            DateTime DateFrom = DateTime.Parse(txtFromDate.Text.Trim(), culture);
            DateTime DateTo = DateTime.Parse(txtToDate.Text.Trim(), culture);
            DataTable tbl = csCont.GietMoCa_GetByNgayMo(DateFrom, DateTo);
            foreach (DataRow r in tbl.Rows)
            {
                int res = csCont.GietMoCa_TraoDoiSanPham_so(int.Parse(r["ID"].ToString()), UserId);
                if(res != 1111)
                    lblMessage.Text += "<br/>" + r["NgayMo"].ToString() + " " + res.ToString();
            }
            lblMessage.Text += "<br/>Đã import xong!";
        }
    }
}