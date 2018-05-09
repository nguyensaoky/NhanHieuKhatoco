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
    public partial class importcade_chuacothailoai : DotNetNuke.Entities.Modules.PortalModuleBase
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
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
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
                string TenChuong = "";
                int So = 0;
                foreach (DataRow r in dTable.Rows)
                {
                    if (r["KhayAp"].ToString() == "0")
                    {
                        break;
                    }
                    string NV = "";
                    if (r["NV"] != DBNull.Value) NV = r["NV"].ToString();
                    if (NV != "") NV = "@" + NV.Replace(" ", "").Replace(",", "@@") + "@";
                    string vc = r["ViCat"].ToString();
                    if (vc.Length == 1) vc = "0" + vc;
                    csCont.ParseChuong(r["Chuong"].ToString().Trim(), out TenChuong, out So);
                    DataTable dtCa = csCont.TheoDoiDe_GetCaDeByLoaiCaByChuongByViCat(vc, Convert.ToInt32(r["LoaiCa"]), TenChuong, So, Convert.ToDateTime(r["NgayVaoAp"]));
                    if (dtCa.Rows.Count == 1)
                    {
                        DataRow rCa = dtCa.Rows[0];
                        TheoDoiDeInfo tdd = new TheoDoiDeInfo();
                        
                        tdd.ID = 0;
                        tdd.CaMe = Convert.ToInt32(rCa["IDCaSau"]);
                        tdd.NgayVaoAp = Convert.ToDateTime(r["NgayVaoAp"]);
                        tdd.KhayAp = Convert.ToInt32(r["KhayAp"]);
                        tdd.PhongAp = Convert.ToInt32(r["PhongAp"]);
                        tdd.TrongLuongTrungBQ = Convert.ToInt32(r["TLTBQ"]);
                        tdd.TrungDe = Convert.ToInt32(r["TrungDe"]);
                        if(r["TrungVo"].ToString() == "") tdd.TrungVo = 0;
                        else tdd.TrungVo = Convert.ToInt32(r["TrungVo"]);
                        tdd.TrungThaiLoai = 0;
                        if(r["TrungKhongPhoi"].ToString() == "") tdd.TrungKhongPhoi = 0;
                        else tdd.TrungKhongPhoi = Convert.ToInt32(r["TrungKhongPhoi"]);
                        if(r["TrungChetPhoi1"].ToString() == "") tdd.TrungChetPhoi1 = 0;
                        else tdd.TrungChetPhoi1 = Convert.ToInt32(r["TrungChetPhoi1"]);
                        if(r["TrungChetPhoi2"].ToString() == "") tdd.TrungChetPhoi2 = 0;
                        else tdd.TrungChetPhoi2 = Convert.ToInt32(r["TrungChetPhoi2"]);
                        if (r["NgayNo"].ToString() != "")
                        {
                            tdd.NgayNo = Convert.ToDateTime(r["NgayNo"]);
                            if (tdd.NgayNo < Config.NgayKhoaSo())
                            {
                                lblMessage.Text += "<br/>Dòng " + idx.ToString() + " không import được do ngày nở trước ngày khóa sổ";
                                idx++;
                                continue;
                            }
                            if (tdd.NgayNo < tdd.NgayVaoAp)
                            {
                                lblMessage.Text += "<br/>Dòng " + idx.ToString() + " không import được do ngày nở trước ngày vào ấp";
                                idx++;
                                continue;
                            }
                        }
                        if(r["KhayUm"].ToString() != "") tdd.KhayUm = Convert.ToInt32(r["KhayUm"]);
                        if(r["TrongLuongConBQ"].ToString() != "") tdd.TrongLuongConBQ = Convert.ToInt32(r["TrongLuongConBQ"]);
                        if(r["ChieuDaiBQ"].ToString() != "") tdd.ChieuDaiBQ = Convert.ToInt32(r["ChieuDaiBQ"]);
                        if(r["VongBungBQ"].ToString() != "") tdd.VongBungBQ = Convert.ToInt32(r["VongBungBQ"]);
                        tdd.Chet1_30Ngay = 0;
                        if(tdd.NgayNo == null)
                        {
                            tdd.Status = 0;
                        }
                        else
                        {
                            tdd.Status = 1;
                        }
                        string arrLyDoThaiLoaiTrung = "@1@";
                        string arrSoLuong = "";
                        if(r["TrungThaiLoai"].ToString() == "") arrSoLuong = "@0@";
                        else arrSoLuong = "@" + r["TrungThaiLoai"].ToString() + "@";
                        int IDTDD = 0;
                        int res1 = csCont.InsertTheoDoiDe(tdd, UserId, arrLyDoThaiLoaiTrung, arrSoLuong, NV, out IDTDD);
                    }
                    else if (dtCa.Rows.Count == 0)
                    {
                        lblMessage.Text += "<br/>Dòng " + idx.ToString() + " không import được do không có cá thích hợp.";
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