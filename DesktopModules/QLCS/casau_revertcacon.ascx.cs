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
    public partial class casau_revertcacon : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ArrayList listroles = DotNetNuke.Entities.Users.UserController.GetUsers(PortalId);
                ddlUser.DataSource = listroles;
                ddlUser.DataTextField = "Username";
                ddlUser.DataValueField = "UserID";
                ddlUser.DataBind();
                ddlUser.Items.Insert(0, new ListItem("----", "0"));
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
            //System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            //string Excelstrcon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dir + file + @";Extended Properties=""Excel 8.0;HDR=YES;""";

            //string SQLstr1 = "SELECT * FROM [Sheet1$]";
            //string SQLstr2 = "SELECT * FROM [Sheet2$]";
            //string SQLstr3 = "SELECT * FROM [Sheet3$]";
            //string SQLstr4 = "SELECT * FROM [Sheet4$]";
            //OleDbConnection ExcelCon = new OleDbConnection(Excelstrcon);
            //ExcelCon.Open();

            //OleDbCommand dbCommand1 = new OleDbCommand(SQLstr1, ExcelCon);
            //OleDbDataAdapter dataAdapter1 = new OleDbDataAdapter(dbCommand1);
            //DataTable dTable1 = new DataTable();

            //OleDbCommand dbCommand2 = new OleDbCommand(SQLstr2, ExcelCon);
            //OleDbDataAdapter dataAdapter2 = new OleDbDataAdapter(dbCommand2);
            //DataTable dTable2 = new DataTable();

            //OleDbCommand dbCommand3 = new OleDbCommand(SQLstr3, ExcelCon);
            //OleDbDataAdapter dataAdapter3 = new OleDbDataAdapter(dbCommand3);
            //DataTable dTable3 = new DataTable();

            //OleDbCommand dbCommand4 = new OleDbCommand(SQLstr4, ExcelCon);
            //OleDbDataAdapter dataAdapter4 = new OleDbDataAdapter(dbCommand4);
            //DataTable dTable4 = new DataTable();

            //try
            //{
            //    //Sheet1
            //    dataAdapter1.Fill(dTable1);
            //    int idx1 = 1;
            //    string strIDCaSau1 = "";
            //    string strThamSo1 = "";
            //    foreach (DataRow r1 in dTable1.Rows)
            //    {
            //        if (r1["KU"].ToString() == "0")
            //        {
            //            break;
            //        }
            //        DataTable dtCa1 = csCont.CaSau_GetCaFromChuong(Convert.ToInt32(r1["KU"]), 1, Convert.ToInt32(r1["SL"]), Convert.ToDateTime(r1["Ngay"]));
            //        if (dtCa1.Rows.Count == Convert.ToInt32(r1["SL"]))
            //        {
            //            strIDCaSau1 = "";
            //            strThamSo1 = "";
            //            foreach (DataRow rCa1 in dtCa1.Rows)
            //            {
            //                strIDCaSau1 += "@" + rCa1["IDCaSau"].ToString() + "@";
            //                strThamSo1 += "@0@";
            //            }
            //            string res1 = csCont.CaSauChet(strIDCaSau1, Convert.ToDateTime(r1["Ngay"]), int.Parse(ddlUser.SelectedValue), "", strThamSo1, strThamSo1, strThamSo1);
            //            if (res1 != "") lblMessage.Text += "<br/>Dòng " + idx1.ToString() + " Sheet 1 không import được. Cá lỗi: " + res1;
            //        }
            //        else
            //        {
            //            lblMessage.Text += "<br/>Dòng " + idx1.ToString() + " Sheet 1 không đủ số lượng cá để chết. Số lượng: " + dtCa1.Rows.Count.ToString() + "/" + Convert.ToInt32(r1["SL"]).ToString();
            //        }
            //        idx1++;
            //    }


            //    //Sheet2
            //    dataAdapter2.Fill(dTable2);
            //    int idx2 = 1;
            //    string strIDCaSau2 = "";
            //    foreach (DataRow r2 in dTable2.Rows)
            //    {
            //        if (r2["KU"].ToString() == "0")
            //        {
            //            break;
            //        }
            //        DataTable dtCa2 = csCont.CaSau_GetCaFromChuong(Convert.ToInt32(r2["KU"]), 1, Convert.ToInt32(r2["SL"]), Convert.ToDateTime(r2["Ngay"]));
            //        if (dtCa2.Rows.Count == Convert.ToInt32(r2["SL"]))
            //        {
            //            strIDCaSau2 = "";
            //            foreach (DataRow rCa2 in dtCa2.Rows)
            //            {
            //                strIDCaSau2 += "@" + rCa2["IDCaSau"].ToString() + "@";
            //            }
            //            int Chuong = csCont.GetChuongByName("ca con", r2["Chuong"].ToString(), "ca con", int.Parse(r2["Chuong"].ToString()));
            //            string res2 = csCont.ChuyenChuong(strIDCaSau2, Chuong, Convert.ToDateTime(r2["Ngay"]), int.Parse(ddlUser.SelectedValue));
            //            if (res2 != "") lblMessage.Text += "<br/>Dòng " + idx2.ToString() + " Sheet 2 không import được. Cá lỗi: " + res2;
            //        }
            //        else
            //        {
            //            lblMessage.Text += "<br/>Dòng " + idx2.ToString() + " Sheet 2 không đủ số lượng cá chuyển chuồng. Số lượng: " + dtCa2.Rows.Count.ToString() + "/" + Convert.ToInt32(r2["SL"]).ToString();
            //        }
            //        idx2++;
            //    }

            //    //Sheet4
            //    dataAdapter4.Fill(dTable4);
            //    int idx4 = 1;
            //    string strIDCaSau4 = "";
            //    foreach (DataRow r4 in dTable4.Rows)
            //    {
            //        if (r4["From"].ToString() == "0")
            //        {
            //            break;
            //        }
            //        int ChuongFrom = csCont.GetChuongByName("ca con", r4["From"].ToString(), "ca con", int.Parse(r4["From"].ToString()));
            //        DataTable dtCa4 = csCont.CaSau_GetCaFromChuong(ChuongFrom, 1, Convert.ToInt32(r4["SL"]), Convert.ToDateTime(r4["Ngay"]));
            //        if (dtCa4.Rows.Count == Convert.ToInt32(r4["SL"]))
            //        {
            //            strIDCaSau4 = "";
            //            foreach (DataRow rCa4 in dtCa4.Rows)
            //            {
            //                strIDCaSau4 += "@" + rCa4["IDCaSau"].ToString() + "@";
            //            }
            //            int ChuongTo = csCont.GetChuongByName("ca con", r4["To"].ToString(), "ca con", int.Parse(r4["To"].ToString()));
            //            string res4 = csCont.ChuyenChuong(strIDCaSau4, ChuongTo, Convert.ToDateTime(r4["Ngay"]), int.Parse(ddlUser.SelectedValue));
            //            if (res4 != "") lblMessage.Text += "<br/>Dòng " + idx4.ToString() + " Sheet 4 không import được. Cá lỗi: " + res4;
            //        }
            //        else
            //        {
            //            lblMessage.Text += "<br/>Dòng " + idx4.ToString() + " Sheet 4 không đủ số lượng cá chuyển chuồng. Số lượng: " + dtCa4.Rows.Count.ToString() + "/" + Convert.ToInt32(r4["SL"]).ToString();
            //        }
            //        idx4++;
            //    }

            //    //Sheet3
            //    dataAdapter3.Fill(dTable3);
            //    int idx3 = 1;
            //    string strIDCaSau3 = "";
            //    string strThamSo3 = "";
            //    foreach (DataRow r3 in dTable3.Rows)
            //    {
            //        if (r3["Chuong"].ToString() == "0")
            //        {
            //            break;
            //        }
            //        int Chuong3 = csCont.GetChuongByName("ca con", r3["Chuong"].ToString(), "ca con", int.Parse(r3["Chuong"].ToString()));
            //        DataTable dtCa3 = csCont.CaSau_GetCaFromChuong(Chuong3, 1, Convert.ToInt32(r3["SL"]), Convert.ToDateTime(r3["Ngay"]));
            //        if (dtCa3.Rows.Count == Convert.ToInt32(r3["SL"]))
            //        {
            //            strIDCaSau3 = "";
            //            strThamSo3 = "";
            //            foreach (DataRow rCa3 in dtCa3.Rows)
            //            {
            //                strIDCaSau3 += "@" + rCa3["IDCaSau"].ToString() + "@";
            //                strThamSo3 += "@0@";
            //            }
            //            string res3 = csCont.CaSauChet(strIDCaSau3, Convert.ToDateTime(r3["Ngay"]), int.Parse(ddlUser.SelectedValue), "", strThamSo3, strThamSo3, strThamSo3);
            //            if (res3 != "") lblMessage.Text += "<br/>Dòng " + idx3.ToString() + " Sheet 3 không import được. Cá lỗi: " + res3;
            //        }
            //        else
            //        {
            //            lblMessage.Text += "<br/>Dòng " + idx3.ToString() + " Sheet 3 không đủ số lượng cá để chết. Số lượng: " + dtCa3.Rows.Count.ToString() + "/" + Convert.ToInt32(r3["SL"]).ToString();
            //        }
            //        idx3++;
            //    }

            //    // dispose used objects            
            //    dTable1.Dispose();
            //    dataAdapter1.Dispose();
            //    dbCommand1.Dispose();
            //    dTable2.Dispose();
            //    dataAdapter2.Dispose();
            //    dbCommand2.Dispose();
            //    dTable3.Dispose();
            //    dataAdapter3.Dispose();
            //    dbCommand3.Dispose();
            //    dTable4.Dispose();
            //    dataAdapter4.Dispose();
            //    dbCommand4.Dispose();
            //    ExcelCon.Close();
            //    ExcelCon.Dispose();

            //    lblMessage.Text += "<br/>Đã import xong!";
            //}
            //catch (Exception ex)
            //{
            //    // dispose used objects            
            //    dTable1.Dispose();
            //    dataAdapter1.Dispose();
            //    dbCommand1.Dispose();
            //    dTable2.Dispose();
            //    dataAdapter2.Dispose();
            //    dbCommand2.Dispose();
            //    dTable3.Dispose();
            //    dataAdapter3.Dispose();
            //    dbCommand3.Dispose();
            //    dTable4.Dispose();
            //    dataAdapter4.Dispose();
            //    dbCommand4.Dispose();
            //    ExcelCon.Close();
            //    ExcelCon.Dispose();
            //    Response.Write(ex.ToString());
            //}
        }
    }
}