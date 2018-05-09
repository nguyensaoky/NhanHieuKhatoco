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
    public partial class note_edit : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        private void BindControls()
        {
            DataTable dtChuong = csCont.LoadChuong(1);
            ddlChuong.DataSource = dtChuong;
            ddlChuong.DataTextField = "Chuong";
            ddlChuong.DataValueField = "IDChuong";
            ddlChuong.DataBind();
            ddlChuong.Items.Insert(0, new ListItem("Chung", "-1"));
        }

        private void LoadData(int NoteID)
        {
            DataTable note = csCont.LoadNoteByID(NoteID);
            if (note.Rows.Count == 0)
                return;
            if (Convert.ToBoolean(note.Rows[0]["Lock"]))
            {
                Save.Visible = false;
                Delete.Visible = false;
            }
            ddlChuong.SelectedValue = note.Rows[0]["Chuong"].ToString();
            if (note.Rows[0]["Ngay"] != DBNull.Value)
            {
                DateTime dtNgay = (DateTime)note.Rows[0]["Ngay"];
                txtNgay.Text = dtNgay.ToString("dd/MM/yyyy");
                if (dtNgay < Config.NgayKhoaSo())
                {
                    //Save.Visible = false;
                    //Delete.Visible = false;
                    Save.Enabled = false;
                    Save.CssClass = "buttondisable";
                    Delete.Enabled = false;
                    Delete.CssClass = "buttondisable";
                }
            }
            txtNote.Text = note.Rows[0]["Note"].ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/impromptu/jquery.js"));
                if (!Page.IsPostBack)
                {
                    BindControls();
                    lnkCancel.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "");
                    if (Request.QueryString["noteid"] != null)
                    {
                        hdIDNote.Value = Request.QueryString["noteid"];
                        LoadData(int.Parse(hdIDNote.Value));
                    }
                    else
                    {
                        Delete.Visible = false;
                    }
                }
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    Save.Visible = false;
                    Delete.Visible = false;
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
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                int ID = Convert.ToInt32(hdIDNote.Value);
                int Chuong = int.Parse(ddlChuong.SelectedValue);
                DateTime Ngay = DateTime.MaxValue;
                if (txtNgay.Text != "")
                {
                    Ngay = DateTime.Parse(txtNgay.Text, ci);
                    if (Ngay < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Ngày không được trước ngày khóa sổ!');", true);
                        return;
                    }
                }
                int res;
                if (ID != 0)
                {
                    res = csCont.UpdateNote(ID, Ngay, Chuong, txtNote.Text, UserId);
                    if (res == 1) Page.ClientScript.RegisterStartupScript(typeof(string), "insertsucess", "alert('Đã sửa xong!');", true);
                    else Page.ClientScript.RegisterStartupScript(typeof(string), "insertfailed", "alert('Không sửa được do dữ liệu mới bị trùng (ngày hoặc chuồng)!');", true);
                }
                else
                {
                    res = csCont.InsertNote(Ngay, Chuong, txtNote.Text, UserId);
                    hdIDNote.Value = res.ToString();
                    if (res != 0)
                    {
                        Delete.Visible = true;
                        Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Đã thêm xong!');", true);
                    }
                    else
                    {
                        Delete.Visible = false;
                        Page.ClientScript.RegisterStartupScript(typeof(string), "insertfailed", "alert('Không thêm được do dữ liệu mới bị trùng (ngày hoặc chuồng)!');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int IDNote = int.Parse(hdIDNote.Value);
            int res = csCont.Note_Delete(IDNote, UserId);
            if (res == 1)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "dadelete", "alert('Đã xóa!');", true);
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "mid/" + this.ModuleId.ToString()));
            }
            else Page.ClientScript.RegisterStartupScript(typeof(string), "chuadelete", "alert('Xóa không được!');", true);
        }
    }
}