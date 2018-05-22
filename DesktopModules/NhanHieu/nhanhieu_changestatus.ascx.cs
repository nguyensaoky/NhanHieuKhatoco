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

namespace DotNetNuke.Modules.NhanHieu
{
    public partial class nhanhieu_changestatus : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        int ID = 0;
        System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("vi-VN");
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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
				if (!Page.IsPostBack)
                {
                    if (UserInfo.Profile.Website != null) website = UserInfo.Profile.Website;
                    if (Request.QueryString["NhanHieuID"] != null) NhanHieuID = int.Parse(Request.QueryString["NhanHieuID"]);
                    if (Request.QueryString["BienDongID"] != null) NhanHieuID = int.Parse(Request.QueryString["BienDongID"]);
                    if (Request.QueryString["Status"] != null) Status = int.Parse(Request.QueryString["Status"]);
                    if(NhanHieuID == 0 || BienDongID == 0 || Status == 0)
                    {
                        btnThucHien.Visible = false;
                        return;
                    }
                    if (UserInfo.Profile.Website != null) website = UserInfo.Profile.Website;
                    if (UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators") || cont.HasRole(UserInfo.Roles, "QuanLy")) admin = true;
                    cont.NhanHieu_CheckValid(NhanHieuID, BienDongID, admin, website, Status, out Valid, out ShowDonVi, out DonVi);
                    if (Valid)
                    {
                        if (ShowDonVi)
                        { 
                            
                        }
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

        private void LoadEditControl()
        {
            //DataTable dtNhanHieuGoc = cont.NhanHieu_GetAllExceptOne(int.Parse(hdNhanHieuID.Value));
            //ddlNhanHieuGoc.DataSource = dtNhanHieuGoc;
            //ddlNhanHieuGoc.DataTextField = "TenNhanHieu";
            //ddlNhanHieuGoc.DataValueField = "ID";
            //ddlNhanHieuGoc.DataBind();
            //ddlNhanHieuGoc.Items.Insert(0, new ListItem("", "0"));
        }

        protected void btnThucHien_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
            }
        }
    }
}