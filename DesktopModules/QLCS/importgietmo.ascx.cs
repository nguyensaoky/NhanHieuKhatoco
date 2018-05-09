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
    public partial class importgietmo : DotNetNuke.Entities.Modules.PortalModuleBase
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
        
        //private void importExcel_org(string dir, string file)
        //{
        //    IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
        //    string Excelstrcon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir + file + @";Extended Properties=""Excel 8.0;HDR=YES;""";

        //    string SQLstr = "SELECT * FROM [Sheet2$]";
        //    string SQLstr1 = "SELECT * FROM [Sheet3$]";
        //    string SQLstr2 = "SELECT * FROM [Sheet4$]";
        //    OleDbConnection ExcelCon = new OleDbConnection(Excelstrcon);
        //    ExcelCon.Open();

        //    OleDbCommand dbCommand = new OleDbCommand(SQLstr, ExcelCon);
        //    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);
        //    OleDbCommand dbCommand1 = new OleDbCommand(SQLstr1, ExcelCon);
        //    OleDbDataAdapter dataAdapter1 = new OleDbDataAdapter(dbCommand1);
        //    OleDbCommand dbCommand2 = new OleDbCommand(SQLstr2, ExcelCon);
        //    OleDbDataAdapter dataAdapter2 = new OleDbDataAdapter(dbCommand2);

        //    DataTable dTable = new DataTable();
        //    DataTable dTable1 = new DataTable();
        //    DataTable dTable2 = new DataTable();
        //    try
        //    {
        //        dataAdapter.Fill(dTable);
        //        dataAdapter1.Fill(dTable1);
        //        dataAdapter2.Fill(dTable2);
        //        int idx = 1;
        //        int idx1 = 0;
        //        int idx2 = 0;
        //        foreach (DataRow r in dTable.Rows)
        //        {
        //            string strSanPham = "";
        //            string strKhoiLuong = "";
        //            int GMC = 0;
        //            DateTime NgayMo = DateTime.Parse(r["Ngay"].ToString() + "/" + r["Thang"].ToString() + "/" + r["Nam"].ToString() + " 12:00:00", culture);
        //            decimal TrongLuongHoi = Convert.ToDecimal(r["TLH"]);
        //            decimal TrongLuongMocHam = Convert.ToDecimal(r["TLMH"]);
        //            string BienBan = r["BB"].ToString();

        //            for (int i = idx2; i < dTable2.Rows.Count; i++)
        //            {
        //                DataRow r2 = dTable2.Rows[i];
        //                strSanPham += "@" + r2["SP"].ToString() + "@";
        //                strKhoiLuong += "@" + r2["KL"].ToString() + "@";
        //                if (r2["GMC"].ToString() == "x")
        //                {
        //                    idx2 = i + 1;
        //                    break;
        //                }
        //            }
        //            //Insert GietMoCa + GietMoCa_SanPham
        //            int res = csCont.GietMoCa_ThemMoi(strSanPham, strKhoiLuong, NgayMo, TrongLuongHoi, TrongLuongMocHam, BienBan, 0, UserId, out GMC);
        //            if (res == 1)
        //            {
        //                for (int i = idx1; i < dTable1.Rows.Count; i++)
        //                {
        //                    DataRow r1 = dTable1.Rows[i];
        //                    int newi;
        //                    DataTable dtCa = csCont.GietMoCa_GetCaCoTheGietMoByLoaiCaByChuongByNamByGiong(Convert.ToInt32(r1["LoaiCa"]), Convert.ToInt32(r1["Chuong"]), NgayMo, Convert.ToInt32(r1["NamNo"]), Convert.ToBoolean(r1["Giong"]));
        //                    if (dtCa.Rows.Count == 1)
        //                    {
        //                        DataRow rCa = dtCa.Rows[0];
        //                        int res1 = csCont.GietMoCa_InsertChiTiet(GMC, Convert.ToInt32(rCa["IDCaSau"]), 0, Convert.ToInt32(r1["Da_Bung"]), Convert.ToInt32(r1["Da_PhanLoai"]), 0, Convert.ToDecimal(r1["TLH"]), Convert.ToDecimal(r1["TLMH"]), UserId, r1["PPM"].ToString());
        //                        if (res1 == 0)
        //                        {
        //                            newi = i + 1;
        //                            lblMessage.Text += "<br/>Dòng " + newi.ToString() + " trong chi tiết cá không import được do có lỗi.";
        //                        }
        //                    }
        //                    else if (dtCa.Rows.Count == 0)
        //                    {
        //                        newi = i + 1;
        //                        lblMessage.Text += "<br/>Dòng " + newi.ToString() + " trong chi tiết cá không import được do không có cá thích hợp.";
        //                    }

        //                    if (r1["GMC"].ToString() == "x")
        //                    {
        //                        idx1 = i + 1;
        //                        break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                lblMessage.Text += "<br/>Không thêm được đợt giết mổ cá thứ" + idx.ToString();
        //                break;
        //            }
        //            idx++;
        //        }

        //        // dispose used objects            
        //        dTable.Dispose();
        //        dataAdapter.Dispose();
        //        dbCommand.Dispose();
        //        dTable1.Dispose();
        //        dataAdapter1.Dispose();
        //        dbCommand1.Dispose();
        //        dTable2.Dispose();
        //        dataAdapter2.Dispose();
        //        dbCommand2.Dispose();
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
        //        dTable1.Dispose();
        //        dataAdapter1.Dispose();
        //        dbCommand1.Dispose();
        //        dTable2.Dispose();
        //        dataAdapter2.Dispose();
        //        dbCommand2.Dispose();
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
                int idx = 2;
                string SBBdc = "";
                string strSanPham = "";
                string strKhoiLuong = "";
                int GMC = 0;
                ArrayList lstIDCa = new ArrayList();
                ArrayList lstDa_Bung = new ArrayList();
                ArrayList lstDa_PhanLoai = new ArrayList();
                ArrayList lstNguoiMo = new ArrayList();
                ArrayList lstTLH = new ArrayList();
                ArrayList lstTLMH = new ArrayList();
                ArrayList lstPPM = new ArrayList();
                ArrayList lstThuTu = new ArrayList();
                DateTime NgayMo = DateTime.MinValue;
                string aCa = "";
                foreach (DataRow r in dTable.Rows)
                {
                    string SBB = r["SBB"].ToString().Trim();
                    if (SBB != SBBdc && SBB != "")
                    { 
                        //Process the last GietMoCa
                        if (SBBdc != "")
                        {
                            int res = csCont.GietMoCa_ThemMoi(strSanPham, strKhoiLuong, NgayMo, SBBdc, 0, UserId, out GMC);
                            if (res == 1)
                            {
                                for (int i = 0; i < lstIDCa.Count; i++)
                                {
                                    int res1 = csCont.GietMoCa_InsertChiTiet(GMC, Convert.ToInt32(lstIDCa[i]), 0, Convert.ToInt32(lstDa_Bung[i]), Convert.ToInt32(lstDa_PhanLoai[i]), 0, Convert.ToInt32(lstNguoiMo[i]), Convert.ToDecimal(lstTLH[i]), Convert.ToDecimal(lstTLMH[i]), UserId, lstPPM[i].ToString(), false, "");
                                    if (res1 == 0)
                                    {
                                        lblMessage.Text += "<br/>Dòng " + lstThuTu[i].ToString() + " không import được do có lỗi.";
                                    }
                                }
                            }
                            else
                            {
                                lblMessage.Text += "<br/>Không thêm được đợt giết mổ cá có SBB là " + SBBdc;
                            }
                        }
                        //Refresh all before change to new GietMoCa
                        lstIDCa.Clear();
                        lstDa_Bung.Clear();
                        lstDa_PhanLoai.Clear();
                        lstNguoiMo.Clear();
                        lstTLH.Clear();
                        lstTLMH.Clear();
                        lstPPM.Clear();
                        lstThuTu.Clear();
                        strSanPham = "";
                        strKhoiLuong = "";
                        //Xac dinh NgayMo
                        if (r["Ngay"] != DBNull.Value)
                        {
                            NgayMo = DateTime.Parse(r["Ngay"].ToString() + "/" + r["Thang"].ToString() + "/" + r["Nam"].ToString() + " 12:00:00", culture);
                        }
                        //Cap nhat SBBdc
                        SBBdc = SBB;
                        aCa = "";
                    }
                    if (r["Chuong"].ToString().Trim() != "")
                    {
                        string TenChuong = "";
                        int So = 0;
                        csCont.ParseChuong(r["Chuong"].ToString().Trim(), out TenChuong, out So);
                        DataTable dtCa = csCont.GietMoCa_GetCaCoTheGietMoByLoaiCaByChuongByNamByGiong_Except(Convert.ToInt32(r["LoaiCa"]), TenChuong, So, NgayMo, Convert.ToInt32(r["NamNo"]), Convert.ToBoolean(r["Giong"]), aCa);
                        if (dtCa.Rows.Count == 1)
                        {
                            DataRow rCa = dtCa.Rows[0];
                            lstIDCa.Add(rCa["IDCaSau"]);
                            lstDa_Bung.Add(r["Da_Bung"]);
                            lstDa_PhanLoai.Add(r["Da_PhanLoai"]);
                            lstNguoiMo.Add(r["NguoiMo"]);
                            lstTLH.Add(r["TLH"]);
                            lstTLMH.Add(r["TLMH"]);
                            lstPPM.Add(r["PPM"]);
                            lstThuTu.Add(idx);
                            aCa += "@" + rCa["IDCaSau"].ToString() + "@";
                        }
                        else if (dtCa.Rows.Count == 0)
                        {
                            lblMessage.Text += "<br/>Dòng " + idx.ToString() + " không import được do không có cá thích hợp.";
                        }
                    }
                    if (r["SP"].ToString().Trim() != "")
                    {
                        strSanPham += "@" + r["SP"].ToString() + "@";
                        strKhoiLuong += "@" + r["KL"].ToString() + "@";
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