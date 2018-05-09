using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using Microsoft.VisualBasic;
using FileInfo=DotNetNuke.Services.FileSystem.FileInfo;
using Globals=DotNetNuke.Common.Globals;


namespace DotNetNuke.UI.UserControls
{
    public partial class UrlControl_Small_Unzip : UserControlBase
    {
        //public bool CanUpload = false;
        private string _localResourceFile;
        private PortalInfo _objPortal;

        public string FileName
        {
            get { return lblFileName.Text; }
        }

        public string UnzipFileNames
        {
            get { return lblUnzipFileNames.Text; }
        }

        public string UnZipFileIDs
        {
            get { return lblUnzipFileIDs.Text; }
        }

        public string FileFilter
        {
            get
            {
                if (ViewState["_FileFilter"] != null)
                {
                    return Convert.ToString(ViewState["_FileFilter"]);
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this.ViewState["_FileFilter"] = value;
            }
        }

        public string LocalResourceFile
        {
            get
            {
                string fileRoot;

                if (String.IsNullOrEmpty( _localResourceFile ))
                {
                    fileRoot = this.TemplateSourceDirectory + "/" + Localization.LocalResourceDirectory + "/URLControl_Small.ascx";
                }
                else
                {
                    fileRoot = _localResourceFile;
                }
                return fileRoot;
            }
            set
            {
                this._localResourceFile = value;
            }
        }

        public string Folder
        {
            get
            {
                return Convert.ToString(this.ViewState["Folder"]);
            }
            set
            {
                this.ViewState["Folder"] = value;
            }
        }

        public UrlControl_Small_Unzip()
        {
            Load += new EventHandler( this.Page_Load );
            Init += new EventHandler(UrlControl_Small_Init);
        }

        protected void UrlControl_Small_Init(object sender, EventArgs e)
        {
            this.cmdSave.Click += new EventHandler(this.cmdSave_Click);
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            //if (CanUpload)
            //{
                string strUnzipFileIDs = "";
                string strUnzipFileNames = "";
                string CurrentTime = DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss");
                // if no file is selected exit
                if (txtFile.PostedFile.FileName == "")
                {
                    return;
                }

                string ParentFolderName;
                if (PortalSettings.ActiveTab.ParentId == PortalSettings.SuperTabId)
                {
                    ParentFolderName = Globals.HostMapPath;
                }
                else
                {
                    ParentFolderName = PortalSettings.HomeDirectoryMapPath;
                }
                ParentFolderName += Folder;

                string strExtension = Path.GetExtension(txtFile.PostedFile.FileName).Replace(".", "");
                string fullFileFilter = "," + FileFilter.ToLower();
                if (!String.IsNullOrEmpty(FileFilter) && !fullFileFilter.Contains("," + strExtension.ToLower()) && strExtension.ToLower() != "zip")
                {
                    // trying to upload a file not allowed for current filter
                    lblMessage.Text = string.Format(Localization.GetString("UploadError", this.LocalResourceFile), FileFilter, strExtension);
                }
                else
                {
                    lblMessage.Text = FileSystemUtils.UploadFileWithTime(CurrentTime, ParentFolderName.Replace("/", "\\"), txtFile.PostedFile, true, out strUnzipFileNames, out strUnzipFileIDs);
                    lblUnzipFileNames.Text = strUnzipFileNames;
                    lblUnzipFileIDs.Text = strUnzipFileIDs;
                }

                if (lblMessage.Text == String.Empty)
                {
                    lblFileName.Text = txtFile.PostedFile.FileName.Substring(txtFile.PostedFile.FileName.LastIndexOf("\\") + 1);
                    int dotIndex = lblFileName.Text.LastIndexOf(".");
                    lblFileName.Text = lblFileName.Text.Substring(0, dotIndex) + CurrentTime + lblFileName.Text.Substring(dotIndex);
                }
            //}
            //else
            //{
            //    lblFileName.Text = "Bạn không có quyền upload file tại đây!";
            //}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PortalController objPortals = new PortalController();
                if (!(Request.QueryString["pid"] == null) && (PortalSettings.ActiveTab.ParentId == PortalSettings.SuperTabId || UserController.GetCurrentUserInfo().IsSuperUser))
                {
                    _objPortal = objPortals.GetPortal(int.Parse(Request.QueryString["pid"]));
                }
                else
                {
                    _objPortal = objPortals.GetPortal(PortalSettings.PortalId);
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
        
        public void Reset()
        {
            lblFileName.Text = "";
            lblUnzipFileNames.Text = "";
            lblUnzipFileIDs.Text = "";
        }
    }
}