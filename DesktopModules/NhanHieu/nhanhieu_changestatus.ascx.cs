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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Control c = DotNetNuke.Common.Globals.FindControlRecursiveDown(Page, "ScriptManager");
                if (c != null)
                {
                    ((ScriptManager)c).RegisterPostBackControl(btnSaveNoiDung);
                }
				if (!Page.IsPostBack)
                {
                    if (UserInfo.Profile.Website != null) website = UserInfo.Profile.Website;
                    LoadEditControl();
                    if (Request.QueryString["ID"] != null)
                    {
                        SetNhanHieuID(Request.QueryString["ID"]);
                        LoadData();
                    }
                    else
                    {
                        trCurrentFile.Visible = false;
                        lblChooseFile.Text = "Chọn hình";
                        SetNhanHieuID("0");
                    }
                    SetButtonStatus();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadEditControl()
        {
            DataTable dtLoaiNhanHieu = cont.DanhMuc_SelectByType("LoaiNhanHieu");
            ddlLoaiNhanHieu.DataSource = dtLoaiNhanHieu;
            ddlLoaiNhanHieu.DataTextField = "Mota";
            ddlLoaiNhanHieu.DataValueField = "ID";
            ddlLoaiNhanHieu.DataBind();

            DataTable dtNuocDangKy = cont.DanhMuc_SelectByType("Nuoc");
            ddlNuocDangKy.DataSource = dtNuocDangKy;
            ddlNuocDangKy.DataTextField = "Mota";
            ddlNuocDangKy.DataValueField = "ID";
            ddlNuocDangKy.DataBind();

            DataTable dtLinhVuc = cont.DanhMuc_SelectByType("LinhVuc");
            lstLinhVuc.DataSource = dtLinhVuc;
            lstLinhVuc.DataTextField = "Mota";
            lstLinhVuc.DataValueField = "ID";
            lstLinhVuc.DataBind();
            lstLinhVuc.SelectionMode = ListSelectionMode.Multiple;

            DataTable dtNhanHieuGoc = cont.NhanHieu_GetAllExceptOne(int.Parse(hdNhanHieuID.Value));
            ddlNhanHieuGoc.DataSource = dtNhanHieuGoc;
            ddlNhanHieuGoc.DataTextField = "TenNhanHieu";
            ddlNhanHieuGoc.DataValueField = "ID";
            ddlNhanHieuGoc.DataBind();
            ddlNhanHieuGoc.Items.Insert(0, new ListItem("", "0"));
        }

        private void SetButton(bool b1, bool b2, bool b3, bool b4, bool b5, bool b6, bool b7, bool b8)
        {
            btnSaveThongTinChung.Visible = b1;
            btnSaveNoiDung.Visible = b2;
            btnDVGuiTCT_TCT.Visible = b3;
            btnDVTrieuHoi_DV.Visible = b4;
            btnTCTGuiDV_DV.Visible = b5;
            btnTCTChapNhan_TCT.Visible = b6;
            btnCucGopYTCT_TCT.Visible = b7;
            btnCucDuyet_TCT.Visible = b8;
        }

        private void SetButtonStatus()
        { 
            if (UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators") || cont.HasRole(UserInfo.Roles, "QuanLy"))
            {
                if (hdIsReferenced.Value == "0")
                {
                    if (website != hdUnit.Value)
                    {
                        SetButton(true, false, false, false, false, false, false, false);
                    }
                    else
                    {
                        SetButton(true, true, false, false, true, true, false, false);
                    }
                }
                else
                {
                    if (hdStatusID.Value == "1")
                    {
                        SetButton(true, true, false, false, true, true, false, false);
                    }
                    else if (hdStatusID.Value == "2")
                    {
                        SetButton(true, false, false, false, false, false, false, false);
                    }
                    else if (hdStatusID.Value == "3")
                    {
                        SetButton(true, false, false, false, false, false, false, false);
                    }
                    else if (hdStatusID.Value == "4")
                    {
                        SetButton(true, true, false, false, true, false, true, true);
                    }
                    else if (hdStatusID.Value == "5")
                    {
                        SetButton(true, true, false, false, true, true, false, false);
                    }
                    else if (hdStatusID.Value == "6")
                    {
                        SetButton(true, true, false, false, false, false, false, false);
                    }
                }
            }
            else
            {
                if (hdIsReferenced.Value == "0")
                {
                    if (website != hdUnit.Value)
                    {
                        if (hdNhanHieuID.Value == "0")
                        {
                            SetButton(true, false, false, false, false, false, false, false);
                        }
                        else
                        {
                            SetButton(false, false, false, false, false, false, false, false);
                        }
                    }
                    else
                    {
                        SetButton(true, true, true, false, false, false, false, false);
                    }
                }
                else
                {
                    if (hdStatusID.Value == "1")
                    {
                        SetButton(false, false, false, true, false, false, false, false);
                    }
                    else if (hdStatusID.Value == "2")
                    {
                        SetButton(true, true, true, false, false, false, false, false);
                    }
                    else if (hdStatusID.Value == "3")
                    {
                        SetButton(true, true, true, false, false, false, false, false);
                    }
                    else if (hdStatusID.Value == "4")
                    {
                        SetButton(false, false, false, false, false, false, false, false);
                    }
                    else if (hdStatusID.Value == "5")
                    {
                        SetButton(false, false, false, false, false, false, false, false);
                    }
                    else if (hdStatusID.Value == "6")
                    {
                        SetButton(false, false, false, false, false, false, false, false);
                    }     
                }
            }
            udpButton.Update();
        }

        void SetNhanHieuID(string s)
        {
            hdNhanHieuID.Value = s;
            if (s != "0") btnSaveNoiDung.Enabled = true;
            else btnSaveNoiDung.Enabled = false;
            udpNoiDung.Update();
        }

        private void LoadData()
        {
            NhanHieuController cont = new NhanHieuController();
            DataTable tbl = cont.NhanHieu_SelectByNhanHieuID(int.Parse(hdNhanHieuID.Value));
            if (tbl.Rows.Count == 1)
            { 
                DataRow r = tbl.Rows[0];

                if (r["Image"] != DBNull.Value)
                {
                    trCurrentFile.Visible = true;
                    lblCurrentFileName.Text = cont.GetFileNameByFileID(Convert.ToInt32(r["Image"]), PortalId);
                    imgCurrentFile.ImageUrl = PortalSettings.HomeDirectory + FolderUpload + lblCurrentFileName.Text;
                    imgCurrentFile.Width = new Unit(50, UnitType.Percentage);
                    hdImage.Value = r["Image"].ToString();
                    lblChooseFile.Text = "Muốn đổi hình? Mời bạn chọn hình mới";
                }
                else
                {
                    trCurrentFile.Visible = false;
                    lblChooseFile.Text = "Chọn hình";
                }
				txtTenNhanHieu.Text = Convert.ToString(r["TenNhanHieu"]);
                ddlNuocDangKy.SelectedValue = Convert.ToString(r["NuocDangKy"]);
                txtSoDon.Text = r["SoDon"].ToString();
                txtNgayNopDon.Text = r["NgayNopDon"] != DBNull.Value ? Convert.ToDateTime(r["NgayNopDon"]).ToString("dd/MM/yyyy") : "";
                txtNgayUuTien.Text = r["NgayUuTien"] != DBNull.Value ? Convert.ToDateTime(r["NgayUuTien"]).ToString("dd/MM/yyyy") : "";
                txtSoChungNhan.Text = r["SoChungNhan"].ToString();
                txtNgayChungNhan.Text = r["NgayChungNhan"] != DBNull.Value ? Convert.ToDateTime(r["NgayChungNhan"]).ToString("dd/MM/yyyy") : "";
                txtNgayCongBo.Text = r["NgayCongBo"] != DBNull.Value ? Convert.ToDateTime(r["NgayCongBo"]).ToString("dd/MM/yyyy") : "";
                txtSoQuyetDinh.Text = r["SoQuyetDinh"].ToString();
                txtNgayQuyetDinh.Text = r["NgayQuyetDinh"] != DBNull.Value ? Convert.ToDateTime(r["NgayQuyetDinh"]).ToString("dd/MM/yyyy") : "";
                txtNote.Text = r["Note"].ToString();
                ddlNhanHieuGoc.SelectedValue = r["NhanHieuGoc"].ToString();

                txtMoTa.Text = r["MoTa"].ToString();
                txtMauSac.Text = r["MauSac"].ToString();
                ddlLoaiNhanHieu.SelectedValue = r["LoaiNhanHieu"].ToString();
                if (r["LinhVuc"] != DBNull.Value && r["LinhVuc"].ToString() != "")
                {
                    string[] alv = r["LinhVuc"].ToString().Substring(1, r["LinhVuc"].ToString().Length - 2).Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in alv)
                    {
                        foreach (ListItem i in lstLinhVuc.Items)
                        {
                            if (i.Value == s)
                            {
                                i.Selected = true;
                                break;
                            }
                        }
                    }
                }
                txtGhiChuThayDoi.Text = r["GhiChuThayDoi"].ToString();

                hdIsReferenced.Value = r["IsReferenced"] == DBNull.Value? "0": Convert.ToInt16(r["IsReferenced"]).ToString();
                hdBienDongID.Value = r["NhanHieuBienDongID"] == DBNull.Value ? "0" : Convert.ToInt16(r["NhanHieuBienDongID"]).ToString();
                hdStatusID.Value = r["StatusID"] == DBNull.Value ? "0" : Convert.ToInt16(r["StatusID"]).ToString();
                hdStatusName.Value = r["StatusName"].ToString();
                hdOwner.Value = r["Owner"].ToString();
                hdUnit.Value = r["CreatedUnit"].ToString();
            }
        }

        public string SaveFile(string Folder, out string FullFileName)
        {
            FullFileName = "";
            string returnFileName = "";
            string CurrentTime = DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss");
            string ParentFolderName;
            ParentFolderName = PortalSettings.HomeDirectoryMapPath;
            ParentFolderName += Folder;
            string strExtension = Path.GetExtension(txtFile.PostedFile.FileName).Replace(".", "");
            string fullFileFilter = ",png,gif,cad,ps,jpg,jpeg";
            if (!fullFileFilter.Contains("," + strExtension.ToLower()))
            {
                lblMessage.Text = "File có phần mở rộng không hợp lệ.";
            }
            else
            {
                lblMessage.Text = FileSystemUtils.UploadFileWithTime(CurrentTime, ParentFolderName.Replace("/", "\\"), txtFile.PostedFile, false);
            }

            if (lblMessage.Text == String.Empty)
            {
                FullFileName = txtFile.PostedFile.FileName.Substring(txtFile.PostedFile.FileName.LastIndexOf("\\") + 1);
                int dotIndex = FullFileName.LastIndexOf(".");
                returnFileName = FullFileName.Substring(0, dotIndex);
                FullFileName = returnFileName + CurrentTime + FullFileName.Substring(dotIndex);
                FileController fileCont = new FileController();
                FileInfo file = fileCont.GetFile(FullFileName, PortalId, Folder);
                hdImage.Value = file.FileId.ToString();
            }
            return returnFileName;
        }

        protected void btnSaveThongTinChung_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime? dtNopDon = null;
                if (txtNgayNopDon.Text.Trim() != "")
                    dtNopDon = DateTime.Parse(txtNgayNopDon.Text.Trim(), ci);

                DateTime? dtUuTien = null;
                if (txtNgayUuTien.Text.Trim() != "")
                    dtUuTien = DateTime.Parse(txtNgayUuTien.Text.Trim(), ci);

                DateTime? dtChungNhan = null;
                if (txtNgayChungNhan.Text.Trim() != "")
                    dtChungNhan = DateTime.Parse(txtNgayChungNhan.Text.Trim(), ci);

                DateTime? dtCongBo = null;
                if (txtNgayCongBo.Text.Trim() != "")
                    dtCongBo = DateTime.Parse(txtNgayCongBo.Text.Trim(), ci);

                DateTime? dtQuyetDinh = null;
                if (txtNgayQuyetDinh.Text.Trim() != "")
                    dtQuyetDinh = DateTime.Parse(txtNgayQuyetDinh.Text.Trim(), ci);
                DateTime dt = DateTime.Now;
                if (hdNhanHieuID.Value == "0")
                {
                    if (UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators") || cont.HasRole(UserInfo.Roles, "QuanLy"))
                    {
                        cont.NhanHieu_Insert(txtTenNhanHieu.Text, int.Parse(ddlNuocDangKy.SelectedValue), txtSoDon.Text, dtNopDon, dtUuTien, txtSoChungNhan.Text, dtChungNhan, dtCongBo, txtSoQuyetDinh.Text, dtQuyetDinh, dt, UserId, website, dt, UserId, website, txtNote.Text, int.Parse(ddlNhanHieuGoc.SelectedValue), "", out ID);
                    }
                    else
                    {
                        cont.NhanHieu_Insert(txtTenNhanHieu.Text, int.Parse(ddlNuocDangKy.SelectedValue), txtSoDon.Text, dtNopDon, dtUuTien, txtSoChungNhan.Text, dtChungNhan, dtCongBo, txtSoQuyetDinh.Text, dtQuyetDinh, dt, UserId, website, dt, UserId, website, txtNote.Text, int.Parse(ddlNhanHieuGoc.SelectedValue), website, out ID);
                    }
                    if (ID == -1) ID = 0;
                    SetNhanHieuID(ID.ToString());
                    hdUnit.Value = website;
                }
                else
                {
                    cont.NhanHieu_Update(int.Parse(hdNhanHieuID.Value), txtTenNhanHieu.Text, int.Parse(ddlNuocDangKy.SelectedValue), txtSoDon.Text, dtNopDon, dtUuTien, txtSoChungNhan.Text, dtChungNhan, dtCongBo, txtSoQuyetDinh.Text, dtQuyetDinh, dt, UserId, website, txtNote.Text, int.Parse(ddlNhanHieuGoc.SelectedValue));
                    SetNhanHieuID(ID.ToString());
                }
                SetButtonStatus();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSaveNoiDung_Click(object sender, EventArgs e)
        {
            try
            {
                NhanHieuController cont = new NhanHieuController();
                string tieude = "";
                if (lblCurrentFileName.Text != "") tieude = lblCurrentFileName.Text.Substring(0, lblCurrentFileName.Text.LastIndexOf('.'));
                string fullFileName = lblCurrentFileName.Text;
                if (txtFile.PostedFile.FileName != "")
                {
                    tieude = SaveFile(FolderUpload, out fullFileName);
                    imgCurrentFile.ImageUrl = PortalSettings.HomeDirectory + FolderUpload + fullFileName;
                }
                if (hdImage.Value == "0")
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "<script language=javascript>alert('Bạn chưa chọn file hoặc file không hợp lệ!');</script>", false);
                    return;
                }

                string lv = "";
                for (int i = 0; i < lstLinhVuc.Items.Count; i++)
                {
                    ListItem li = lstLinhVuc.Items[i];
                    if (li.Selected)
                    {
                        lv += "@" + li.Value + "@";
                    }
                }
                DateTime dt = DateTime.Now;
                int BienDongID = 0;
                if (hdIsReferenced.Value == "1")
                {
                    cont.NhanHieu_BienDong_Insert(int.Parse(hdNhanHieuID.Value), int.Parse(hdImage.Value), txtMoTa.Text, txtMauSac.Text, int.Parse(ddlLoaiNhanHieu.SelectedValue), lv, txtGhiChuThayDoi.Text, dt, UserId, website, out BienDongID);
                    if (BienDongID == -1) BienDongID = 0;
                    hdBienDongID.Value = BienDongID.ToString();
                    hdIsReferenced.Value = "0";
                    hdUnit.Value = website;
                }
                else
                {
                    if (hdBienDongID.Value != "0")
                    {
                        cont.NhanHieu_BienDong_Update(int.Parse(hdBienDongID.Value), int.Parse(hdNhanHieuID.Value), int.Parse(hdImage.Value), txtMoTa.Text, txtMauSac.Text, int.Parse(ddlLoaiNhanHieu.SelectedValue), lv, txtGhiChuThayDoi.Text, dt, UserId, website);
                        hdBienDongID.Value = BienDongID.ToString();
                        hdIsReferenced.Value = "0";
                        hdUnit.Value = website;
                    }
                    else
                    {
                        cont.NhanHieu_BienDong_Insert(int.Parse(hdNhanHieuID.Value), int.Parse(hdImage.Value), txtMoTa.Text, txtMauSac.Text, int.Parse(ddlLoaiNhanHieu.SelectedValue), lv, txtGhiChuThayDoi.Text, dt, UserId, website, out BienDongID);
                        if (BienDongID == -1) BienDongID = 0;
                        hdBienDongID.Value = BienDongID.ToString();
                        hdIsReferenced.Value = "0";
                        hdUnit.Value = website;
                    }
                }
                SetButtonStatus();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnDVGuiTCT_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnDVTrieuHoi_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnTCTGuiDV_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnTCTChapNhan_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnCucGopYTCT_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnCucDuyet_Click(object sender, EventArgs e)
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