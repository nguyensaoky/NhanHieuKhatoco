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
    public partial class importxuatvattu : DotNetNuke.Entities.Modules.PortalModuleBase
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

            string SQLstr = "SELECT * FROM [Sheet1$]";
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
                    DateTime NgayXuat = DateTime.Parse(r["Ngay"].ToString() + "/" + r["Thang"].ToString() + "/" + r["Nam"].ToString() + " 13:01:01", culture);
                    for (int i = 4; i < dTable.Columns.Count; i++)
                    {
                        decimal value = 0;
                        if(r[i] != DBNull.Value) value = Convert.ToDecimal(r[i]);
                        if (value != 0)
                        {
                            string columnName = dTable.Columns[i].ColumnName;
                            int idVatTu = int.Parse(columnName.Substring(columnName.IndexOf('(') + 1, columnName.IndexOf(')') - columnName.IndexOf('(') - 1));
                            int res = csCont.VatTu_ThemBienDong(idVatTu, value, 2, "", NgayXuat, UserId, 0);
                            if (res != 1) lblMessage.Text += "<br/>Dòng " + idx.ToString() + ", " + columnName + " không import được do số lượng hoặc thời điểm biến động không hợp lệ.";
                        }
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
    }
}