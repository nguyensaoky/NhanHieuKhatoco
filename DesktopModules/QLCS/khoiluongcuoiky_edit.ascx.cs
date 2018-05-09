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
    public partial class khoiluongcuoiky_edit : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        CaSauController csCont = new CaSauController();
        private void LoadData()
        {
            if (Request.QueryString["namcan"] != null)
            {
                DataTable tblKhoiLuongCuoiKy = csCont.LoadKhoiLuongCuoiKyByNamCan(int.Parse(Request.QueryString["namcan"]));
                if (tblKhoiLuongCuoiKy.Rows.Count != 0)
                { 
                    DataRow r = tblKhoiLuongCuoiKy.Rows[0];
                    txtNamCan.Text = r["NamCan"].ToString();
                    if (r["SoSinh"] != DBNull.Value)
                        txtSoSinh.Text = ((decimal)r["SoSinh"]).ToString("0.#####");
                    if (r["CuoiKyUm_Giong"] != DBNull.Value)
                        txtCuoiKyUm_Giong.Text = ((decimal)r["CuoiKyUm_Giong"]).ToString("0.#####");
                    if (r["CuoiKyUm_TT"] != DBNull.Value)
                        txtCuoiKyUm_TT.Text = ((decimal)r["CuoiKyUm_TT"]).ToString("0.#####");
                    if (r["CuoiKyUm"] != DBNull.Value)
                        txtCuoiKyUm.Text = ((decimal)r["CuoiKyUm"]).ToString("0.#####");
                    if (r["CuoiKy1Nam_Giong"] != DBNull.Value)
                        txtCuoiKy1Nam_Giong.Text = ((decimal)r["CuoiKy1Nam_Giong"]).ToString("0.#####");
                    if (r["CuoiKy1Nam_TT"] != DBNull.Value)
                        txtCuoiKy1Nam_TT.Text = ((decimal)r["CuoiKy1Nam_TT"]).ToString("0.#####");
                    if (r["CuoiKy1Nam"] != DBNull.Value)
                        txtCuoiKy1Nam.Text = ((decimal)r["CuoiKy1Nam"]).ToString("0.#####");
                    if (r["CuoiKyST1_Giong"] != DBNull.Value)
                        txtCuoiKyST1_Giong.Text = ((decimal)r["CuoiKyST1_Giong"]).ToString("0.#####");
                    if (r["CuoiKyST1_TT"] != DBNull.Value)
                        txtCuoiKyST1_TT.Text = ((decimal)r["CuoiKyST1_TT"]).ToString("0.#####");
                    if (r["CuoiKyST1"] != DBNull.Value)
                        txtCuoiKyST1.Text = ((decimal)r["CuoiKyST1"]).ToString("0.#####");
                    if (r["CuoiKyST2_Giong"] != DBNull.Value)
                        txtCuoiKyST2_Giong.Text = ((decimal)r["CuoiKyST2_Giong"]).ToString("0.#####");
                    if (r["CuoiKyST2_TT"] != DBNull.Value)
                        txtCuoiKyST2_TT.Text = ((decimal)r["CuoiKyST2_TT"]).ToString("0.#####");
                    if (r["CuoiKyST2"] != DBNull.Value)
                        txtCuoiKyST2.Text = ((decimal)r["CuoiKyST2"]).ToString("0.#####");
                    if (r["CuoiKyHB1_Giong"] != DBNull.Value)
                        txtCuoiKyHB1_Giong.Text = ((decimal)r["CuoiKyHB1_Giong"]).ToString("0.#####");
                    if (r["CuoiKyHB1_TT"] != DBNull.Value)
                        txtCuoiKyHB1_TT.Text = ((decimal)r["CuoiKyHB1_TT"]).ToString("0.#####");
                    if (r["CuoiKyHB1"] != DBNull.Value)
                        txtCuoiKyHB1.Text = ((decimal)r["CuoiKyHB1"]).ToString("0.#####");
                    if (r["CuoiKyHB2_Giong"] != DBNull.Value)
                        txtCuoiKyHB2_Giong.Text = ((decimal)r["CuoiKyHB2_Giong"]).ToString("0.#####");
                    if (r["CuoiKyHB2_TT"] != DBNull.Value)
                        txtCuoiKyHB2_TT.Text = ((decimal)r["CuoiKyHB2_TT"]).ToString("0.#####");
                    if (r["CuoiKyHB2"] != DBNull.Value)
                        txtCuoiKyHB2.Text = ((decimal)r["CuoiKyHB2"]).ToString("0.#####");
                }
                else
                {
                    Save.Visible = false;
                    Page.ClientScript.RegisterStartupScript(typeof(string), "khongcodulieu", "alert('Không có dữ liệu');", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    LoadData();
                    lnkCancel.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "");
                    if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !UserInfo.IsInRole("QLCS"))
                    {
                        Save.Visible = false;
                    }
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
                CaSauController csCont = new CaSauController();
                if (Request.QueryString["namcan"] != null)
                {
                    csCont.UpdateKhoiLuongCuoiKy(int.Parse(txtNamCan.Text), decimal.Parse(txtSoSinh.Text), decimal.Parse(txtCuoiKyUm_Giong.Text), decimal.Parse(txtCuoiKyUm_TT.Text), decimal.Parse(txtCuoiKyUm.Text), decimal.Parse(txtCuoiKy1Nam_Giong.Text), decimal.Parse(txtCuoiKy1Nam_TT.Text), decimal.Parse(txtCuoiKy1Nam.Text), decimal.Parse(txtCuoiKyST1_Giong.Text), decimal.Parse(txtCuoiKyST1_TT.Text), decimal.Parse(txtCuoiKyST1.Text), decimal.Parse(txtCuoiKyST2_Giong.Text), decimal.Parse(txtCuoiKyST2_TT.Text), decimal.Parse(txtCuoiKyST2.Text), decimal.Parse(txtCuoiKyHB1_Giong.Text), decimal.Parse(txtCuoiKyHB1_TT.Text), decimal.Parse(txtCuoiKyHB1.Text), decimal.Parse(txtCuoiKyHB2_Giong.Text), decimal.Parse(txtCuoiKyHB2_TT.Text), decimal.Parse(txtCuoiKyHB2.Text));
                }
                else
                {
                    csCont.InsertKhoiLuongCuoiKy(int.Parse(txtNamCan.Text), decimal.Parse(txtSoSinh.Text), decimal.Parse(txtCuoiKyUm_Giong.Text), decimal.Parse(txtCuoiKyUm_TT.Text), decimal.Parse(txtCuoiKyUm.Text), decimal.Parse(txtCuoiKy1Nam_Giong.Text), decimal.Parse(txtCuoiKy1Nam_TT.Text), decimal.Parse(txtCuoiKy1Nam.Text), decimal.Parse(txtCuoiKyST1_Giong.Text), decimal.Parse(txtCuoiKyST1_TT.Text), decimal.Parse(txtCuoiKyST1.Text), decimal.Parse(txtCuoiKyST2_Giong.Text), decimal.Parse(txtCuoiKyST2_TT.Text), decimal.Parse(txtCuoiKyST2.Text), decimal.Parse(txtCuoiKyHB1_Giong.Text), decimal.Parse(txtCuoiKyHB1_TT.Text), decimal.Parse(txtCuoiKyHB1.Text), decimal.Parse(txtCuoiKyHB2_Giong.Text), decimal.Parse(txtCuoiKyHB2_TT.Text), decimal.Parse(txtCuoiKyHB2.Text));
                }
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, ""));
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                CaSauController csCont = new CaSauController();
                if (Request.QueryString["namcan"] != null)
                {
                    csCont.KhoiLuongCuoiKy_Delete(int.Parse(Request.QueryString["namcan"]));
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}