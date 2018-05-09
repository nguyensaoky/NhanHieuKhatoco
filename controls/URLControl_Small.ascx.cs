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
    public partial class UrlControl_Small : UserControlBase
    {
        public bool CanUpload = false;
        private string _localResourceFile;
        private int _ModuleID = -2;
        private PortalInfo _objPortal;
        private bool _Required = true;
        private string _Url = "";
        private string _Width = "";

        public string FileName
        {
            get { return lblFileName.Text; }
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

        public int ModuleID
        {
            get
            {
                int returnValue;
                returnValue = Convert.ToInt32(ViewState["ModuleId"]);
                if (returnValue == -2)
                {
                    if (Request.QueryString["mid"] != null)
                    {
                        returnValue = int.Parse(Request.QueryString["mid"]);
                    }
                }
                return returnValue;
            }
            set
            {
                this._ModuleID = value;
            }
        }

        public bool Required
        {
            set
            {
                this._Required = value;
            }
        }

        public string Url
        {
            get
            {
                string returnValue;
                returnValue = "";
                if (lblFileName.Text != "")
                {
                    returnValue = "FileID=" + lblFileID.Text;
                }
                else
                {
                    returnValue = "FileID=0";
                }
                return returnValue;
            }
            set
            {
                this._Url = value;
            }
        }

        public string Width
        {
            get
            {
                return Convert.ToString( this.ViewState["SkinControlWidth"] );
            }
            set
            {
                this._Width = value;
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

        public UrlControl_Small()
        {
            Load += new EventHandler( this.Page_Load );
            Init += new EventHandler(UrlControl_Small_Init);

            this._ModuleID = -2;
            this._Required = true;
            this._Url = "";
            this._Width = "";
        }

        protected void UrlControl_Small_Init(object sender, EventArgs e)
        {
            this.cmdSave.Click += new EventHandler(this.cmdSave_Click);
        }

        private ArrayList GetFileList(bool NoneSpecified)
        {
            ArrayList fileList;
            if (PortalSettings.ActiveTab.ParentId == PortalSettings.SuperTabId)
            {
                fileList = Globals.GetFileList(Null.NullInteger, FileFilter, NoneSpecified, Folder, false);
            }
            else
            {
                fileList = Globals.GetFileList(_objPortal.PortalID, FileFilter, NoneSpecified, Folder, false);
            }

            return fileList;
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
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
            if (!String.IsNullOrEmpty(FileFilter) && !fullFileFilter.Contains("," + strExtension.ToLower()))
            {
                // trying to upload a file not allowed for current filter
                lblMessage.Text = string.Format(Localization.GetString("UploadError", this.LocalResourceFile), FileFilter, strExtension);
            }
            else
            {
                lblMessage.Text = FileSystemUtils.UploadFileWithTime(CurrentTime, ParentFolderName.Replace("/", "\\"), txtFile.PostedFile, false);
            }

            if (lblMessage.Text == String.Empty)
            {
                lblFileName.Text = txtFile.PostedFile.FileName.Substring(txtFile.PostedFile.FileName.LastIndexOf("\\") + 1);
                int dotIndex = lblFileName.Text.LastIndexOf(".");
                lblFileName.Text = lblFileName.Text.Substring(0, dotIndex) + CurrentTime + lblFileName.Text.Substring(dotIndex);
                FileController fileCont = new FileController();
                FileInfo file = fileCont.GetFile(lblFileName.Text, _objPortal.PortalID, Folder);
                lblFileID.Text = file.FileId.ToString();
            }
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

                if (!Page.IsPostBack)
                {
                    ViewState["ModuleId"] = Convert.ToString(_ModuleID);
                    ViewState["SkinControlWidth"] = _Width;
                    //ShowControls();
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public void ShowControls()
        {
            if (_Url != string.Empty)
            {
                string FileID;
                FileID = _Url.Substring(7);
                if (FileID != "0")
                {
                    FileController fileCont = new FileController();
                    FileInfo file = fileCont.GetFileById(int.Parse(FileID), _objPortal.PortalID);
                    lblFileID.Text = file.FileId.ToString();
                    lblFileName.Text = file.FileName;
                }
            }
        }

        public void Reset()
        {
            lblFileName.Text = "";
            lblFileID.Text = "0";
        }
    }
}