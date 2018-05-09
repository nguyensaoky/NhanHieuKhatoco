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
    public partial class saoluudulieu : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private void BindControls()
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                txtBackupFileName.Text = "QLCSbk_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "_" + UserInfo.Username + ".bak";
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

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                string BackupDir = ConfigurationManager.AppSettings["QLCS_BackupDir"];
                string _DatabaseName = "QLCS";
                string _BackupName = txtBackupFileName.Text.Trim();
                string _ConnectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString();
                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = _ConnectionString;
                sqlConnection.Open();
                string sqlQuery = "BACKUP DATABASE " + _DatabaseName + " TO DISK = '" + BackupDir + _BackupName + "' WITH FORMAT, MEDIANAME = 'QLCSBackup', NAME = '" + _BackupName + "';";
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                sqlCommand.CommandType = CommandType.Text;
                int iRows = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                Page.ClientScript.RegisterStartupScript(typeof(string), "finish", "alert('Đã xong');", true);
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}