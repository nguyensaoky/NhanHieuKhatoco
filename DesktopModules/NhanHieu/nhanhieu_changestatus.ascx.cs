using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Web.Security;
using System.Data.OleDb;
using System.Data.SqlClient;
using DotNetNuke.Framework.Providers;
using FileInfo = DotNetNuke.Services.FileSystem.FileInfo;
using DotNetNuke.Common.Utilities;
using System.IO;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Entities.Users;

namespace DotNetNuke.Modules.NhanHieu
{
    public partial class nhanhieu_changestatus : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        int ID = 0;
        System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("vi-VN");
        DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();
        NhanHieuController cont = new NhanHieuController();
        string website;
        string FolderUpload = "TaiLieu/";
        bool admin;
        bool Valid = false;
        bool ShowDonVi = false;
        string DonVi = "";
        private void LoadDonVis()
        {
            ddlDonVi.Items.Clear();
            ArrayList aUser = rc.GetUsersByRoleName(PortalId, "DonVi");
            foreach (UserInfo u in aUser)
            {
                ddlDonVi.Items.Add(new ListItem(u.Profile.IM, u.Profile.Website));
            }
            ddlDonVi.Visible = true;
            lblDonVi.Visible = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
				if (!Page.IsPostBack)
                {
                    if (UserInfo.Profile.Website != null) website = UserInfo.Profile.Website;
                    if (Request.QueryString["NhanHieuID"] != null) hdNhanHieuID.Value = Request.QueryString["NhanHieuID"];
                    if (Request.QueryString["BienDongID"] != null) hdBienDongID.Value = Request.QueryString["BienDongID"];
                    if (Request.QueryString["Status"] != null) hdStatus.Value = Request.QueryString["Status"];
                    if(hdNhanHieuID.Value == "0" || hdBienDongID.Value == "0" || hdStatus.Value == "0")
                    {
                        btnThucHien.Visible = false;
                        return;
                    }
                    if (hdStatus.Value == "6") 
                    {
                        lblNamHieuLuc.Visible = true;
                        txtNamHieuLuc.Visible = true;
                        lblFromDate.Visible = true;
                        txtFromDate.Visible = true;
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        lblNamHieuLuc.Visible = false;
                        txtNamHieuLuc.Visible = false;
                        lblFromDate.Visible = false;
                        txtFromDate.Visible = false;
                    }
                    hdStatusName.Value = cont.GetStatus(int.Parse(hdStatus.Value));
                    if (UserInfo.Profile.Website != null) website = UserInfo.Profile.Website; else website = "";
                    if (UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators") || cont.HasRole(UserInfo.Roles, "QuanLy")) admin = true; else admin = false;
                    cont.NhanHieu_CheckValid(int.Parse(hdNhanHieuID.Value), int.Parse(hdBienDongID.Value), admin, website, int.Parse(hdStatus.Value), out Valid, out ShowDonVi, out DonVi);
                    if (Valid)
                    {
                        if (ShowDonVi)
                        {
                            LoadDonVis();
                            if (DonVi != "")
                            {
                                ListItem li = ddlDonVi.Items.FindByValue(DonVi);
                                if(li != null) 
                                {
                                    li.Selected = true;
                                    ddlDonVi.Enabled = false;
                                }
                            }
                        }
                        else
                        {
                            ddlDonVi.Visible = false;
                            lblDonVi.Visible = false;
                        }
                        btnThucHien.Visible = true;
                    }
                    else
                    {
                        btnThucHien.Visible = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public int SaveFile(string Folder, out string FullFileName)
        {
            int fileID = 0;
            FullFileName = "";
            string returnFileName = "";
            string CurrentTime = DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss");
            string ParentFolderName;
            ParentFolderName = PortalSettings.HomeDirectoryMapPath;
            ParentFolderName += Folder;
            string strExtension = Path.GetExtension(txtFile.PostedFile.FileName).Replace(".", "");
            string fullFileFilter = ",png,gif,cad,ps,jpg,jpeg,doc,docx,pdf,xls,xlsx,ppt,pptx,txt";
            if (!fullFileFilter.Contains("," + strExtension.ToLower()))
            {
                lblMessage.Text = "File có phần mở rộng không hợp lệ.";
            }
            else
            {
                lblMessage.Text = FileSystemUtils.UploadFileWithTime(CurrentTime, ParentFolderName.Replace("/", "\\"), txtFile.PostedFile, false);
            }

            if (lblMessage.Text == String.Empty)
            {
                FullFileName = txtFile.PostedFile.FileName.Substring(txtFile.PostedFile.FileName.LastIndexOf("\\") + 1);
                int dotIndex = FullFileName.LastIndexOf(".");
                returnFileName = FullFileName.Substring(0, dotIndex);
                FullFileName = returnFileName + CurrentTime + FullFileName.Substring(dotIndex);
                FileController fileCont = new FileController();
                FileInfo file = fileCont.GetFile(FullFileName, PortalId, Folder);
                if (file != null) fileID = file.FileId;
            }
            return fileID;
        }

        protected void btnThucHien_Click(object sender, EventArgs e)
        {
            try
            {
                int NamHieuLuc = -1;
                if (txtNamHieuLuc.Visible && hdStatus.Value == "6") NamHieuLuc = int.Parse(txtNamHieuLuc.Text.Trim());
                int fileID = 0;
                string donvi = "";
                string FullFileName = "";
                if (ddlDonVi.Visible && ddlDonVi.Enabled) donvi = ddlDonVi.SelectedValue;
                if (txtFile.PostedFile.FileName != "")
                {
                    fileID = SaveFile(FolderUpload, out FullFileName);
                }
                DateTime dt = DateTime.Now;
                if(txtFromDate.Visible) dt = DateTime.Parse(txtFromDate.Text.Trim(), ci);
                cont.ChangeStatus_Insert(int.Parse(hdNhanHieuID.Value), int.Parse(hdBienDongID.Value), int.Parse(hdStatus.Value), txtMessage.Text.Trim(), fileID, donvi, dt, NamHieuLuc);
                Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã thực hiện!');window.opener.finishEdit();self.close();</script>", false);
                //if(fileID != 0)
                //{
                //    string download = PortalSettings.HomeDirectory + FolderUpload + FullFileName;
                //    download = txtMessage.Text.Trim() + " " + "<a href=\"" + download + "\">Download</a>";
                //    //Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã thực hiện!');window.opener.finishEdit('" + hdStatus.Value + "','" + hdStatusName.Value + "','" + download + "');self.close();</script>", false);
                    
                //}
                //else
                //{
                //    //Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã thực hiện!');window.opener.finishEdit('" + hdStatus.Value + "','" + hdStatusName.Value + "','" + txtMessage.Text.Trim() + "');self.close();</script>", false);
                //}
            }
            catch (Exception ex)
            {
            }
        }
    }
}