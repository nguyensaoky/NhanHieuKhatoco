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
using System.IO;

namespace DotNetNuke.Modules.QLCS
{
    public partial class phuchoidulieu : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private void ReadBackupFiles()
        {
            try
            {
                string BackupDir = ConfigurationManager.AppSettings["QLCS_BackupDir"];
                if (!Directory.Exists(BackupDir))
                {
                    Directory.CreateDirectory(BackupDir);
                }

                string[] files = Directory.GetFiles(BackupDir, "QLCSbk_*.bak");
                Array.Sort(files, StringComparer.InvariantCulture);
                Array.Reverse(files);
                ddlBackupFile.DataSource = files;
                ddlBackupFile.DataBind();
                ddlBackupFile.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
            }
        }

        private void BindControls()
        {
            try
            {
                ReadBackupFiles();
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

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                string _DatabaseName = "QLCS";
                string _ConnectionString = ConfigurationManager.AppSettings["OtherDataServer"];
                string _BackupName = ddlBackupFile.SelectedItem.Text.ToString();

                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = _ConnectionString;
                sqlConnection.Open();
                string sqlQuery = "USE MASTER ALTER DATABASE [" + _DatabaseName + "] SET Single_User WITH Rollback Immediate ALTER DATABASE [" + _DatabaseName + "] SET Multi_User RESTORE DATABASE " + _DatabaseName + " FROM DISK ='" + _BackupName + "' WITH REPLACE";
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                sqlCommand.CommandType = CommandType.Text;
                int iRows = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                Page.ClientScript.RegisterStartupScript(typeof(string), "finish", "alert('Đã xong');", true);
                SqlConnection.ClearAllPools();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}