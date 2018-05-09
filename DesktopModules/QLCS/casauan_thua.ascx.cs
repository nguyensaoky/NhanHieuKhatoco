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
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using DotNetNuke.Framework.Providers;

namespace DotNetNuke.Modules.QLCS
{
    public partial class casauan_thua : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");

        private void BindControls()
        {
            txtTuNgay.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindControls();
                    btnLoad_Click(null, null);
                }
                else
	            {
                    if(txtTuNgay.Text.Trim() == "")
                        txtTuNgay.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                    if(txtDenNgay.Text.Trim() == "")
                        txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
	            }

                //Khoi tao danh sach chuong
                DataSet ds = Config.SelectSPs("QLCS_DanhMuc_GetAllChuong_TenChuong", new SqlParameter[] { });
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    ListBox lstChuong = new ListBox();
                    lstChuong.ID = "lstChuong" + i.ToString();
                    lstChuong.DataSource = ds.Tables[i];
                    lstChuong.DataTextField = "Chuong";
                    lstChuong.DataValueField = "IDChuong";
                    lstChuong.CssClass = "lstChuong";
                    lstChuong.DataBind();
                    lstChuong.Rows = 6;
                    lstChuong.SelectionMode = ListSelectionMode.Multiple;
                    lstChuong.EnableViewState = true;
                    phChuong.Controls.Add(lstChuong);
                }
                hdNumListChuong.Value = ds.Tables.Count.ToString();

                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable tblNgayAnThua = csCont.CaSauAn_GetCaSauAnThua(txtTuNgay.Text.Trim(), txtDenNgay.Text.Trim());
                ddlNgayThucAnThua.DataSource = tblNgayAnThua;
                ddlNgayThucAnThua.DataTextField = "ThoiDiem";
                ddlNgayThucAnThua.DataValueField = "ID";
                ddlNgayThucAnThua.DataBind();
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
                if (ddlNgayThucAnThua.SelectedValue != "" && ddlNgayThucAnThua.SelectedValue != null)
                {
                    if (DateTime.Parse(ddlNgayThucAnThua.SelectedItem.Text,ci) < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Không xóa được ngày có thức ăn thừa trước ngày khóa sổ');", true);
                        return;
                    }
                    csCont.CaSauAnThua_Delete(int.Parse(ddlNgayThucAnThua.SelectedValue));
                    btnLoad_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlNgayThucAnThua.SelectedValue != "" && ddlNgayThucAnThua.SelectedValue != null)
                {
                    txtNgayAn.Text = ddlNgayThucAnThua.SelectedItem.Text;
                    hdListChuong.Value = "";
                    hdListTenChuong.Value = "";
                    hdListThua.Value = "";
                    DataTable tblDetails = csCont.CaSauAn_GetCaSauAnThuaByCaSauAn(int.Parse(ddlNgayThucAnThua.SelectedValue));
                    foreach (DataRow r in tblDetails.Rows)
                    {
                        hdListChuong.Value += ";" + r["Chuong"].ToString();
                        hdListTenChuong.Value += ";" + r["TenChuong"].ToString();
                        hdListThua.Value += ";" + r["Thua"].ToString();
                    }
                    if (hdListChuong.Value != "") hdListChuong.Value = hdListChuong.Value.Substring(1);
                    if (hdListTenChuong.Value != "") hdListTenChuong.Value = hdListTenChuong.Value.Substring(1);
                    if (hdListThua.Value != "") hdListThua.Value = hdListThua.Value.Substring(1);

                    Page.ClientScript.RegisterStartupScript(typeof(string), "initDetails", "<script language=javascript>bindChuong();</script>", false);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ngayan = DateTime.Now.Date;
                if(!DateTime.TryParse(txtNgayAn.Text.Trim(), ci, System.Globalization.DateTimeStyles.None, out ngayan))
                    ngayan = DateTime.Now.Date;

                if (ngayan < Config.NgayKhoaSo())
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Không lưu được ngày có thức ăn thừa trước ngày khóa sổ');", true);
                    return;
                }

                string s = "";
                foreach (string k in Request.Form.AllKeys)
                {
                    if (k.StartsWith("chuong") && Request.Form[k] != "")
                    {
                        s += "@" + k.Substring(6) + "/" + Request.Form[k] + "@";
                    }
                }

                csCont.CaSauAnThua_InsertUpdate(ngayan, s);
                for (int i = 0; i < int.Parse(hdNumListChuong.Value); i++)
			    {
                    ListBox l = (ListBox)(phChuong.FindControl("lstChuong" + i.ToString()));
                    if (l != null)
                    {
                        foreach (ListItem item in l.Items)
                        {
                            item.Selected = false;
                        }
                    }
			    }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}