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
using System.Data.SqlClient;

namespace DotNetNuke.Modules.QLCS
{
    public partial class cauhinh : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private void BindControls()
        {
            try
            {
                txtBackupFolderName.Text = ConfigurationManager.AppSettings["QLCS_BackupDir"];
                txtTAScale.Text = ConfigurationManager.AppSettings["QLCS_VatTu_TA_Scale"];
                txtTTYScale.Text = ConfigurationManager.AppSettings["QLCS_VatTu_TTY_Scale"];
                txtSPGMScale.Text = ConfigurationManager.AppSettings["QLCS_VatTu_SPGM_Scale"];
                txtDCSScale.Text = ConfigurationManager.AppSettings["QLCS_VatTu_DCS_Scale"];
            }
            catch (Exception ex)
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindControls();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnBackupFolderName_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationManager.AppSettings["QLCS_BackupDir"] = txtBackupFolderName.Text;
                Page.ClientScript.RegisterStartupScript(typeof(string), "finish", "alert('Đã xong');", true);
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnTAScale_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationManager.AppSettings["QLCS_VatTu_TA_Scale"] = txtTAScale.Text;
                Page.ClientScript.RegisterStartupScript(typeof(string), "finish", "alert('Đã xong');", true);
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnTTYScale_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationManager.AppSettings["QLCS_VatTu_TTY_Scale"] = txtTTYScale.Text;
                Page.ClientScript.RegisterStartupScript(typeof(string), "finish", "alert('Đã xong');", true);
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnSPGMScale_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationManager.AppSettings["QLCS_VatTu_SPGM_Scale"] = txtSPGMScale.Text;
                Page.ClientScript.RegisterStartupScript(typeof(string), "finish", "alert('Đã xong');", true);
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnDCSScale_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationManager.AppSettings["QLCS_VatTu_DCS_Scale"] = txtDCSScale.Text;
                ConfigurationManager.AppSettings["QLCS_VatTu_DCS__Scale"] = txtDCSScale.Text;
                ConfigurationManager.AppSettings["QLCS_VatTu_DCS_CB_Scale"] = txtDCSScale.Text;
                ConfigurationManager.AppSettings["QLCS_VatTu_DCS_CL_Scale"] = txtDCSScale.Text;
                ConfigurationManager.AppSettings["QLCS_VatTu_DCS_MDL_Scale"] = txtDCSScale.Text;
                Page.ClientScript.RegisterStartupScript(typeof(string), "finish", "alert('Đã xong');", true);
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}