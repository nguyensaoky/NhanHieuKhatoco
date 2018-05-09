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

namespace DotNetNuke.Modules.QLCS
{
    public partial class vattu_edit : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        public string dataString = "";
        private CaSauController csCont = new CaSauController();

        private void InitDonViTinh()
        {
            DataTable tblDonViTinh = csCont.VatTu_GetDonViTinh();
            foreach (DataRow r in tblDonViTinh.Rows)
            {
                dataString += r["DonViTinh"].ToString() + ";";
            }
            if (dataString != "")
            {
                dataString = dataString.Remove(dataString.Length - 1);
            }
            Page.ClientScript.RegisterStartupScript(typeof(string), "initDonViTinh", "<script language=javascript>initDonViTinh();</script>", false);
        }

        private void LoadData()
        {
            if (Request.QueryString["IDVatTu"] != null)
            {
                DataTable tbl = csCont.VatTu_GetByID(int.Parse(Request.QueryString["IDVatTu"]));
                if(tbl.Rows.Count > 0)
                {
                    txtTenVatTu.Text = tbl.Rows[0]["TenVatTu"].ToString();
                    txtDonViTinh.Text = tbl.Rows[0]["DonViTinh"].ToString();
                }
                else
                {
                    Save.Visible = false;
                }
            }
            else
            {
                Save.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                InitDonViTinh();
                Page.ClientScript.RegisterClientScriptInclude("ac_actb", ResolveUrl("~/js/autocomplete/actb.js"));
                Page.ClientScript.RegisterClientScriptInclude("ac_common", ResolveUrl("~/js/autocomplete/common.js"));
                if (!Page.IsPostBack)
                {
                    LoadData();
                    lnkCancel.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "");
                }

                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    Save.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Save.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtTenVatTu.Text == "" || txtDonViTinh.Text == "")
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "dulieuthieu", "alert('Bạn phải nhập đủ dữ liệu!');", true);
                    return;
                }
                csCont.VatTu_Update(int.Parse(Request.QueryString["IDVatTu"]), txtTenVatTu.Text, txtDonViTinh.Text);
                Page.ClientScript.RegisterStartupScript(typeof(string), "updatesuccess", "alert('Cập nhật thành công!');", true);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}