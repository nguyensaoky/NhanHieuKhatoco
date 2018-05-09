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
    public partial class importcachet : DotNetNuke.Entities.Modules.PortalModuleBase
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

        //private void importExcel(string dir, string file)
        //{
        //    IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
        //    string Excelstrcon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir + file + @";Extended Properties=""Excel 8.0;HDR=YES;""";

        //    string SQLstr = "SELECT * FROM [Sheet1$]";
        //    OleDbConnection ExcelCon = new OleDbConnection(Excelstrcon);
        //    ExcelCon.Open();

        //    OleDbCommand dbCommand = new OleDbCommand(SQLstr, ExcelCon);
        //    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);

        //    DataTable dTable = new DataTable();
        //    try
        //    {
        //        dataAdapter.Fill(dTable);
        //        int idx = 1;
        //        DateTime NgayChet = DateTime.MinValue;
        //        DateTime currNgay = DateTime.MinValue;
        //        string StrIDCaSau = "";
        //        string StrDaBung = "";
        //        string StrDaPhanLoai = "";
        //        string StrDau = "";
        //        string LyDo = "";
        //        foreach (DataRow r in dTable.Rows)
        //        {
        //            NgayChet = DateTime.Parse(r["Ngay"].ToString() + "/" + r["Thang"].ToString() + "/" + r["Nam"].ToString() + " 22:00:00", culture);

        //            if (NgayChet != currNgay)
        //            {
        //                if (StrIDCaSau != "")
        //                {
        //                    string fail = csCont.CaSauChet(StrIDCaSau, currNgay, UserId, LyDo, StrDaBung, StrDaPhanLoai, StrDau);
        //                    if (fail != "")
        //                    {
        //                        int sidx = idx - 1;
        //                        lblMessage.Text += "<br/>Dòng " + sidx.ToString() + ":" + fail;
        //                    }
        //                    StrIDCaSau = "";
        //                    StrDaBung = "";
        //                    StrDaPhanLoai = "";
        //                    StrDau = "";
        //                }
        //                currNgay = NgayChet;
        //            }
        //            string s = csCont.CaSau_Chet_GetCaByLoaiCaByChuongAtDate(Convert.ToInt32(r["LoaiCa"]), r["Chuong"].ToString(), Convert.ToInt32(r["SL"]), NgayChet);
        //            string sTemp = s.Replace("@", "");
        //            if ((s.Length - sTemp.Length) / 2 != Convert.ToInt32(r["SL"]))
        //            {
        //                lblMessage.Text += "<br/>Dòng " + idx.ToString() + " không import do số lượng cá không đủ.";
        //                idx++;
        //                continue;
        //            }

        //            StrIDCaSau += s;
        //            if (r["SanPham"] != DBNull.Value && r["SanPham"].ToString() != "")
        //            { 
        //                if(r["SanPham"].ToString() == "-1")
        //                {
        //                    for(int i = 0; i<Convert.ToInt32(r["SL"]); i++)
        //                    {
        //                        StrDaBung += "@0@";
        //                        StrDaPhanLoai += "@4@";
        //                        StrDau += "@1@";
        //                    }
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < Convert.ToInt32(r["SL"]); i++)
        //                    {
        //                        StrDaBung += "@" + r["SanPham"].ToString() + "@";
        //                        StrDaPhanLoai += "@4@";
        //                        StrDau += "@0@";
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                for (int i = 0; i < Convert.ToInt32(r["SL"]); i++)
        //                {
        //                    StrDaBung += "@0@";
        //                    StrDaPhanLoai += "@4@";
        //                    StrDau += "@0@";
        //                }
        //            }
                    
        //            idx++;
        //        }

        //        if (StrIDCaSau != "")
        //        {
        //            string fail = csCont.CaSauChet(StrIDCaSau, currNgay, UserId, LyDo, StrDaBung, StrDaPhanLoai, StrDau);
        //            if (fail != "")
        //            {
        //                int sidx = idx - 1;
        //                lblMessage.Text += "<br/>Dòng " + sidx.ToString() + ":" + fail;
        //            }
        //        }

        //        // dispose used objects
        //        dTable.Dispose();
        //        dataAdapter.Dispose();
        //        dbCommand.Dispose();
        //        ExcelCon.Close();
        //        ExcelCon.Dispose();

        //        lblMessage.Text += "<br/>Đã import xong!";
        //    }
        //    catch (Exception ex)
        //    {
        //        // dispose used objects
        //        dTable.Dispose();
        //        dataAdapter.Dispose();
        //        dbCommand.Dispose();
        //        ExcelCon.Close();
        //        ExcelCon.Dispose();
        //        Response.Write(ex.ToString());
        //    }
        //}

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
                DateTime NgayChet = DateTime.MinValue;
                string StrIDCaSau = "";
                string StrDaBung = "";
                string StrDaPhanLoai = "";
                string StrDau = "";
                string StrPPM = "";
                string StrLDC = "";
                string StrKL = "";
                string LyDo = "";
                int hh = 11;
                int mm = 10;
                int ss = 10;
                string TenChuong = "";
                int So = 0;
                string BienBan = "";
                foreach (DataRow r in dTable.Rows)
                {
                    //NgayChet = DateTime.Parse(r["Ngay"].ToString() + "/" + r["Thang"].ToString() + "/" + r["Nam"].ToString() + " 22:00:00", culture);
                    NgayChet = DateTime.Parse(r["Ngay"].ToString() + "/" + r["Thang"].ToString() + "/" + r["Nam"].ToString() + " " + hh.ToString() + ":" + mm.ToString() + ":" + ss.ToString(), culture);
                    ss++; if (ss > 59) { ss = 10; mm++; if (mm > 59) { mm = 10; hh++; } }
                    csCont.ParseChuong(r["Chuong"].ToString().Trim(), out TenChuong, out So);
                    string s = csCont.CaSau_Chet_GetCaByLoaiCaByChuongAtDate(Convert.ToInt32(r["LoaiCa"]), TenChuong, So, Convert.ToInt32(r["SL"]), NgayChet, Convert.ToBoolean(r["Giong"]));
                    string sTemp = s.Replace("@", "");
                    if ((s.Length - sTemp.Length) / 2 != Convert.ToInt32(r["SL"]))
                    {
                        lblMessage.Text += "<br/>Dòng " + idx.ToString() + " không import do số lượng cá không đủ.";
                        idx++;
                        continue;
                    }
                    if (r["BienBan"] != DBNull.Value) BienBan = r["BienBan"].ToString();
                    else BienBan = "";
                    StrIDCaSau = s;
                    if (r["SanPham"] != DBNull.Value && r["SanPham"].ToString() != "")
                    {
                        if (r["SanPham"].ToString() == "-1")
                        {
                            for (int i = 0; i < Convert.ToInt32(r["SL"]); i++)
                            {
                                StrDaBung += "@0@";
                                StrDaPhanLoai += "@4@";
                                StrDau += "@1@";
                                StrPPM += "@CL@";
                                StrLDC += "@" + r["LyDoChet"].ToString() + "@";
                                StrKL += "@" + r["KhoiLuong"].ToString().Replace(",", ".") + "@";
                            }
                        }
                        else
                        {
                            for (int i = 0; i < Convert.ToInt32(r["SL"]); i++)
                            {
                                StrDaBung += "@" + r["SanPham"].ToString() + "@";
                                StrDaPhanLoai += "@" + r["PhanLoai"].ToString() + "@";
                                StrDau += "@0@";
                                StrPPM += "@" + r["PPM"].ToString() + "@";
                                StrLDC += "@" + r["LyDoChet"].ToString() + "@";
                                StrKL += "@" + r["KhoiLuong"].ToString().Replace(",",".") + "@";
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Convert.ToInt32(r["SL"]); i++)
                        {
                            StrDaBung += "@0@";
                            StrDaPhanLoai += "@4@";
                            StrDau += "@0@";
                            StrPPM += "@CL@";
                            StrLDC += "@" + r["LyDoChet"].ToString() + "@";
                            StrKL += "@" + r["KhoiLuong"].ToString().Replace(",", ".") + "@";
                        }
                    }

                    if (StrIDCaSau != "")
                    {
                        string fail = csCont.CaSauChet(StrIDCaSau, NgayChet, UserId, StrDaBung, StrDaPhanLoai, StrDau, StrPPM, StrLDC, StrKL, BienBan, "-1", "");
                        if (fail != "")
                        {
                            lblMessage.Text += "<br/>Dòng " + idx.ToString() + ":" + fail;
                        }
                        StrIDCaSau = "";
                        StrDaBung = "";
                        StrDaPhanLoai = "";
                        StrDau = "";
                        StrPPM = "";
                        StrLDC = "";
                        StrKL = "";
                    }
                    else
                    {
                        StrDaBung = "";
                        StrDaPhanLoai = "";
                        StrDau = "";
                        StrPPM = "";
                        StrLDC = "";
                        StrKL = "";
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