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
        string website = "";
        string FolderUpload = "TaiLieu/";
        int NhanHieuID = 0;
        int BienDongID = 0;
        int Status = 0;
        bool admin = false;
        bool Valid = false;
        bool ShowDonVi = false;
        string DonVi = "";
        string StatusName = "";
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
                    if (Request.QueryString["NhanHieuID"] != null) NhanHieuID = int.Parse(Request.QueryString["NhanHieuID"]);
                    if (Request.QueryString["BienDongID"] != null) BienDongID = int.Parse(Request.QueryString["BienDongID"]);
                    if (Request.QueryString["Status"] != null) Status = int.Parse(Request.QueryString["Status"]);
                    if(NhanHieuID == 0 || BienDongID == 0 || Status == 0)
                    {
                        btnThucHien.Visible = false;
                        return;
                    }
                    if(Status == 6) 
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
                    StatusName = cont.GetStatus(Status);
                    if (UserInfo.Profile.Website != null) website = UserInfo.Profile.Website;
                    if (UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators") || cont.HasRole(UserInfo.Roles, "QuanLy")) admin = true;
                    cont.NhanHieu_CheckValid(NhanHieuID, BienDongID, admin, website, Status, out Valid, out ShowDonVi, out DonVi);
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
                if (txtNamHieuLuc.Visible && Status == 6) NamHieuLuc = int.Parse(txtNamHieuLuc.Text.Trim());
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
                cont.ChangeStatus_Insert(NhanHieuID, BienDongID, Status, txtMessage.Text.Trim(), fileID, donvi, dt, NamHieuLuc);
                if(fileID != 0)
                {
                    string download = PortalSettings.HomeDirectory + FolderUpload + FullFileName;
                    download = txtMessage.Text.Trim() + "<br/>" + "<a href='" + download + "'>Download</a>";
                    Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã thực hiện!');window.opener.finishEdit('" + Status.ToString() + "','" + StatusName + "','" + download + "');self.close();</script>", false);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã thực hiện!');window.opener.finishEdit('" + Status.ToString() + "','" + StatusName + "','" + txtMessage.Text.Trim() + "');self.close();</script>", false);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}