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
    public partial class importchuyenchuong : DotNetNuke.Entities.Modules.PortalModuleBase
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
                int idx = 1;
                string strIDCaSau = "";
                DateTime NgayChuyen = DateTime.MinValue;
                int hh = int.Parse(txtGio.Text);
                int mm = 10;
                int ss = 10;
                foreach (DataRow r in dTable.Rows)
                {
                    string TenChuong = "";
                    int So = 0;
                    csCont.ParseChuong(r["Di"].ToString().Trim(), out TenChuong, out So);
                    //string Di = r["Di"].ToString().Trim();
                    //int lastSpace = Di.LastIndexOf(" ");
                    //if (lastSpace > -1)
                    //{
                    //    TenChuong = Di.Substring(0, lastSpace);
                    //    So = int.Parse(Di.Substring(lastSpace + 1));
                    //}
                    //int ChuongFrom = csCont.GetChuongByName("", r["Di"].ToString().Trim(), TenChuong, So);
                    NgayChuyen = DateTime.Parse(r["Ngay"].ToString() + "/" + r["Thang"].ToString() + "/" + r["Nam"].ToString() + " " + hh.ToString() + ":" + mm.ToString() + ":" + ss.ToString(), culture);
                    ss++; if (ss > 59) { ss = 10; mm++; if (mm > 59) { mm = 10; hh++; } }
                    int Giong = -1;
                    if (r["Giong"] != DBNull.Value)
                    {
                        if (Convert.ToInt32(r["Giong"]) == 0)
                            Giong = 0;
                        else
                            Giong = 1;
                    }
                    string MaSo = "";
                    if (r["MaSo"] != DBNull.Value)
                        MaSo = r["MaSo"].ToString();
                    DataTable dtCa = csCont.CaSau_GetCaFromChuong1(TenChuong, So, Convert.ToInt32(r["LoaiCa"]), Giong, MaSo, Convert.ToInt32(r["SL"]), NgayChuyen);
                    if (dtCa.Rows.Count == Convert.ToInt32(r["SL"]))
                    {
                        strIDCaSau = "";
                        foreach (DataRow rCa in dtCa.Rows)
                        {
                            strIDCaSau += "@" + rCa["IDCaSau"].ToString() + "@";
                        }
                        string TenChuong1 = "";
                        int So1 = 0;
                        csCont.ParseChuong(r["Den"].ToString().Trim(), out TenChuong1, out So1);
                        //string Den = r["Den"].ToString().Trim();
                        //int lastSpace1 = Den.LastIndexOf(" ");
                        //if (lastSpace1 > -1)
                        //{
                        //    TenChuong1 = Den.Substring(0, lastSpace1);
                        //    So1 = int.Parse(Den.Substring(lastSpace1 + 1));
                        //}
                        //int ChuongTo = csCont.GetChuongByName("", r["Den"].ToString().Trim(), TenChuong1, So1);
                        string res = csCont.ChuyenChuong1(strIDCaSau, TenChuong1, So1, NgayChuyen, UserId);
                        if (res != "") lblMessage.Text += "<br/>Dòng " + idx.ToString() + " không import được. Cá lỗi: " + res;
                    }
                    else
                    {
                        lblMessage.Text += "<br/>Dòng " + idx.ToString() + " không đủ số lượng cá chuyển chuồng. Số lượng: " + dtCa.Rows.Count.ToString() + "/" + Convert.ToInt32(r["SL"]).ToString();
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