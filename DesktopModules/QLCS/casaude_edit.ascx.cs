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
using DotNetNuke.Data;
using System.Data.SqlClient;
using DotNetNuke.NewsProvider;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Common;
using Microsoft.ApplicationBlocks.Data;
using System.Collections.Specialized;
using DotNetNuke.Framework.Providers;
using System.Web.Script.Serialization;

namespace DotNetNuke.Modules.QLCS
{
    public partial class casaude_edit : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        private CaSauController csCont = new CaSauController();
        decimal TongThaiLoai = 0;
        public class DataCheck
        {
            public int Type;
            public string Date;
            public string IDTheoDoiDe;
        }

        private void BindControls()
        {
            txtNgayVaoAp.Text = DateTime.Now.ToString("dd/MM/yyyy") + " 12:00:00";

            DataTable dtChuong = csCont.LoadChuongByTenChuong("@KSS@@TP@");
            ddlChuong.DataSource = dtChuong;
            ddlChuong.DataTextField = "Chuong";
            ddlChuong.DataValueField = "IDChuong";
            ddlChuong.DataBind();

            DataTable dtPhongAp = csCont.LoadPhongAp(1);
            ddlPhongAp.DataSource = dtPhongAp;
            ddlPhongAp.DataValueField = "IDPhongAp";
            ddlPhongAp.DataTextField = "TenPhongAp";
            ddlPhongAp.DataBind();
            ddlPhongAp.Items.Insert(0, new ListItem("-----", "0"));

            DataTable dtNhanVien = csCont.LoadNhanVien(1);
            lstNhanVien.DataSource = dtNhanVien;
            lstNhanVien.DataTextField = "TenNhanVien";
            lstNhanVien.DataValueField = "IDNhanVien";
            lstNhanVien.DataBind();
        }

        private void LoadData(int TheoDoiDeID)
        {
            int SoTrungNoCu = 0;
            DataTable tdd = csCont.LoadTheoDoiDeByID(TheoDoiDeID);
            if (tdd.Rows.Count == 0)
                return;
            if (Convert.ToBoolean(tdd.Rows[0]["Lock"]))
            {
                Save.Visible = false;
                Delete.Visible = false;
            }

            DateTime dtNgayVaoAp = DateTime.MinValue;
            if (tdd.Rows[0]["NgayVaoAp"] != DBNull.Value)
            {
                dtNgayVaoAp = (DateTime)tdd.Rows[0]["NgayVaoAp"];
                txtNgayVaoAp.Text = ((DateTime)tdd.Rows[0]["NgayVaoAp"]).ToString("dd/MM/yyyy HH:mm:ss");
            }
            ddlChuong.SelectedValue = tdd.Rows[0]["Chuong"].ToString();
            ddlChuong_SelectedIndexChanged(null, null);

            ddlCaMe.SelectedValue = tdd.Rows[0]["CaMe"].ToString();
            Config.SetSelectedValues(lstNhanVien, tdd.Rows[0]["NhanVien"].ToString());
            
            if (tdd.Rows[0]["KhayAp"] != DBNull.Value)
            {
                DataTable dtKA = csCont.GetAvailableKhayAp(dtNgayVaoAp, TheoDoiDeID);
                ddlKhayAp.DataSource = dtKA;
                ddlKhayAp.DataTextField = "TenKhayAp";
                ddlKhayAp.DataValueField = "IDKhayAp";
                ddlKhayAp.DataBind();
                ddlKhayAp.SelectedValue = tdd.Rows[0]["KhayAp"].ToString();
                hdDaLoadKhayAp.Value = "1";
            }
            if (tdd.Rows[0]["PhongAp"] != DBNull.Value)
            {
                ddlPhongAp.SelectedValue = tdd.Rows[0]["PhongAp"].ToString();
            }
            if (tdd.Rows[0]["TrongLuongTrungBQ"] != DBNull.Value)
            {
                txtTLTBQ.Text = tdd.Rows[0]["TrongLuongTrungBQ"].ToString();
            }
            if (tdd.Rows[0]["TrungDe"] != DBNull.Value)
            {
                txtTrungDe.Text = tdd.Rows[0]["TrungDe"].ToString();
                SoTrungNoCu = int.Parse(txtTrungDe.Text);
            }
            if (tdd.Rows[0]["TrungVo"] != DBNull.Value)
            {
                txtTrungVo.Text = tdd.Rows[0]["TrungVo"].ToString();
                SoTrungNoCu -= int.Parse(txtTrungVo.Text);
            }
            if (tdd.Rows[0]["TrungKhongPhoi"] != DBNull.Value)
            {
                txtTrungKhongPhoi.Text = tdd.Rows[0]["TrungKhongPhoi"].ToString();
                SoTrungNoCu -= int.Parse(txtTrungKhongPhoi.Text);
            }
            if (tdd.Rows[0]["TrungChetPhoi1"] != DBNull.Value)
            {
                txtTrungChetPhoi1.Text = tdd.Rows[0]["TrungChetPhoi1"].ToString();
                SoTrungNoCu -= int.Parse(txtTrungChetPhoi1.Text);
            }
            if (tdd.Rows[0]["TrungChetPhoi2"] != DBNull.Value)
            {
                txtTrungChetPhoi2.Text = tdd.Rows[0]["TrungChetPhoi2"].ToString();
                SoTrungNoCu -= int.Parse(txtTrungChetPhoi2.Text);
            }
            if (tdd.Rows[0]["TrungThaiLoai"] != DBNull.Value)
            {
                SoTrungNoCu -= Convert.ToInt32(tdd.Rows[0]["TrungThaiLoai"]);
            }
            DateTime dtNgayNo = DateTime.MinValue;
            if (tdd.Rows[0]["NgayNo"] != DBNull.Value)
            {
                dtNgayNo = (DateTime)tdd.Rows[0]["NgayNo"];
                txtNgayNo.Text = dtNgayNo.ToString("dd/MM/yyyy HH:mm:ss");
                if (dtNgayNo < Config.NgayKhoaSo())
                {
                    //Save.Visible = false;
                    //Delete.Visible = false;
                    Save.Enabled = false;
                    Save.CssClass = "buttondisable";
                    Delete.Enabled = false;
                    Delete.CssClass = "buttondisable";
                }
            }
            if (tdd.Rows[0]["KhayUm"] != DBNull.Value)
            {
                DataTable dtKU = csCont.GetAvailableKhayUm(dtNgayNo, TheoDoiDeID);
                ddlKhayUm.DataSource = dtKU;
                ddlKhayUm.DataTextField = "Chuong";
                ddlKhayUm.DataValueField = "IDChuong";
                ddlKhayUm.DataBind();
                ddlKhayUm.SelectedValue = tdd.Rows[0]["KhayUm"].ToString();
            }
            if (tdd.Rows[0]["TrongLuongConBQ"] != DBNull.Value)
            {
                txtTLCBQ.Text = tdd.Rows[0]["TrongLuongConBQ"].ToString();
            }
            if (tdd.Rows[0]["ChieuDaiBQ"] != DBNull.Value)
            {
                txtCDBQ.Text = Convert.ToDecimal(tdd.Rows[0]["ChieuDaiBQ"]).ToString("0.#");
            }
            if (tdd.Rows[0]["VongBungBQ"] != DBNull.Value)
            {
                txtVBBQ.Text = Convert.ToDecimal(tdd.Rows[0]["VongBungBQ"]).ToString("0.#"); ;
            }
            if (tdd.Rows[0]["Note"] != DBNull.Value)
            {
                txtNote.Text = tdd.Rows[0]["Note"].ToString();
            }
            hdSoTtrungNoCu.Value = SoTrungNoCu.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Headers["X-OFFICIAL-REQUEST"] == "TRUE")
                {
                    AjaxWrapper();
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/impromptu/jquery.js"));
                    if (!Page.IsPostBack)
                    {
                        BindControls();
                        lnkCancel.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "");
                        if (Request.QueryString["tddid"] != null)
                        {
                            hdIDTDD.Value = Request.QueryString["tddid"];
                            LoadData(int.Parse(hdIDTDD.Value));
                        }
                        else
                        {
                            ddlChuong_SelectedIndexChanged(null, null);
                            Delete.Visible = false;
                        }
                        LoadThaiLoaiTrungData(int.Parse(hdIDTDD.Value));
                    }
                    else
                    {
                        if (hdKhayUm.Value != "0")
                        {
                            DataTable dtKU = csCont.GetAvailableKhayUm(DateTime.Parse(txtNgayNo.Text, ci), int.Parse(hdIDTDD.Value));
                            ddlKhayUm.DataSource = dtKU;
                            ddlKhayUm.DataTextField = "Chuong";
                            ddlKhayUm.DataValueField = "IDChuong";
                            ddlKhayUm.DataBind();
                            ddlKhayUm.SelectedValue = hdKhayUm.Value;
                        }
                        if (hdKhayAp.Value != "0")
                        {
                            DataTable dtKA = csCont.GetAvailableKhayAp(DateTime.Parse(txtNgayVaoAp.Text, ci), int.Parse(hdIDTDD.Value));
                            ddlKhayAp.DataSource = dtKA;
                            ddlKhayAp.DataTextField = "TenKhayAp";
                            ddlKhayAp.DataValueField = "IDKhayAp";
                            ddlKhayAp.DataBind();
                            ddlKhayAp.SelectedValue = hdKhayAp.Value;
                        }
                    }
                    if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                    {
                        Save.Visible = false;
                        Delete.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void AjaxWrapper()
        {
            Response.Clear();
            JavaScriptSerializer jscript = new JavaScriptSerializer();
            string strDataCheck = HttpContext.Current.Request.Form[0];
            DataCheck dc = jscript.Deserialize<DataCheck>(strDataCheck);
            int Type = dc.Type;
            DateTime d = DateTime.Parse(dc.Date, ci);
            if (Type == 2)
            {
                DataTable dt = csCont.GetAvailableKhayUm(d, int.Parse(dc.IDTheoDoiDe));
                List<KhayUm> list = new List<KhayUm>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        KhayUm obj = new KhayUm(Convert.ToInt32(dt.Rows[i]["IDChuong"]), Convert.ToString(dt.Rows[i]["Chuong"]));
                        list.Insert(i, obj);
                    }
                }
                string strData = jscript.Serialize(list);
                Response.Write(strData);
                Response.Flush();
                try { Response.Close(); }
                catch { }//It likes to break sometimes - and other times it's needed
                Response.End();
            }
            else if (Type == 1)
            {
                DataTable dt = csCont.GetAvailableKhayAp(d, int.Parse(dc.IDTheoDoiDe));
                List<KhayAp> list = new List<KhayAp>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        KhayAp obj = new KhayAp(Convert.ToInt32(dt.Rows[i]["IDKhayAp"]), Convert.ToString(dt.Rows[i]["TenKhayAp"]));
                        list.Insert(i, obj);
                    }
                }
                string strData = jscript.Serialize(list);
                Response.Write(strData);
                Response.Flush();
                try { Response.Close(); }
                catch { }//It likes to break sometimes - and other times it's needed
                Response.End();
            }
        }

        private void LoadThaiLoaiTrungData(int IDTheoDoiDe)
        {
            DataTable tblThaiLoaiTrung = csCont.TheoDoiDe_GetThaiLoaiTrung(IDTheoDoiDe);
            grvDanhSach.DataSource = tblThaiLoaiTrung;
            grvDanhSach.DataBind();
        }

        protected void grvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                Label lblLyDoThaiLoai = (Label)(e.Row.FindControl("lblLyDoThaiLoai"));
                lblLyDoThaiLoai.Text = r["TenLyDoThaiLoaiTrung"].ToString();
                TextBox txtSoLuong = (TextBox)(e.Row.FindControl("txtSoLuong"));
                Button btnSaveThaiLoaiTrung = (Button)(e.Row.FindControl("btnSaveThaiLoaiTrung"));
                if (r["SoLuongTrungThaiLoai"] == DBNull.Value) txtSoLuong.Text = "0";
                else txtSoLuong.Text = r["SoLuongTrungThaiLoai"].ToString();
                btnSaveThaiLoaiTrung.CommandArgument = r["IDLyDoThaiLoaiTrung"].ToString();
                TongThaiLoai += int.Parse(txtSoLuong.Text);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTongThaiLoai = (Label)(e.Row.FindControl("lblTongThaiLoai"));
                lblTongThaiLoai.Text = TongThaiLoai.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                TheoDoiDeInfo tdd = new TheoDoiDeInfo();
                tdd.ID = Convert.ToInt32(hdIDTDD.Value);
                tdd.CaMe = int.Parse(ddlCaMe.SelectedValue);
                if (txtNgayVaoAp.Text != "")
                    tdd.NgayVaoAp = DateTime.Parse(txtNgayVaoAp.Text, ci);
                if (ddlKhayAp.SelectedValue != "0")
                    tdd.KhayAp = int.Parse(ddlKhayAp.SelectedValue);
                if (ddlPhongAp.SelectedValue != "0")
                    tdd.PhongAp = int.Parse(ddlPhongAp.SelectedValue);
                if (txtTLTBQ.Text != "")
                    tdd.TrongLuongTrungBQ = int.Parse(txtTLTBQ.Text);
                if (txtTrungDe.Text != "")
                    tdd.TrungDe = int.Parse(txtTrungDe.Text);
                else
                    tdd.TrungDe = 0;
                if (txtTrungVo.Text != "")
                    tdd.TrungVo = int.Parse(txtTrungVo.Text);
                else
                    tdd.TrungVo = 0;
                if (txtTrungKhongPhoi.Text != "")
                    tdd.TrungKhongPhoi = int.Parse(txtTrungKhongPhoi.Text);
                else
                    tdd.TrungKhongPhoi = 0;
                if (txtTrungChetPhoi1.Text != "")
                    tdd.TrungChetPhoi1 = int.Parse(txtTrungChetPhoi1.Text);
                else
                    tdd.TrungChetPhoi1 = 0;
                if (txtTrungChetPhoi2.Text != "")
                    tdd.TrungChetPhoi2 = int.Parse(txtTrungChetPhoi2.Text);
                else
                    tdd.TrungChetPhoi2 = 0;
                if (txtNgayNo.Text != "")
                {
                    tdd.NgayNo = DateTime.Parse(txtNgayNo.Text, ci);
                    if (tdd.NgayNo < Config.NgayKhoaSo())
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Ngày nở không được trước ngày khóa sổ!');", true);
                        return;
                    }
                    if (tdd.NgayNo < tdd.NgayVaoAp)
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Ngày vào ấp không được sau ngày nở!');", true);
                        return;
                    }
                }
                if (ddlKhayUm.SelectedValue != "0" && ddlKhayUm.SelectedValue != "")
                    tdd.KhayUm = int.Parse(ddlKhayUm.SelectedValue);
                if (txtTLCBQ.Text != "")
                    tdd.TrongLuongConBQ = int.Parse(txtTLCBQ.Text);
                if (txtCDBQ.Text != "")
                    tdd.ChieuDaiBQ = decimal.Parse(txtCDBQ.Text);
                if (txtVBBQ.Text != "")
                    tdd.VongBungBQ = decimal.Parse(txtVBBQ.Text);
                //Tuong thich
                tdd.Chet1_30Ngay = 0;
                if (tdd.NgayNo == null)
                {
                    tdd.Status = 0;
                    tdd.KhayUm = null;
                    tdd.TrongLuongConBQ = 0;
                    tdd.VongBungBQ = 0;
                    tdd.ChieuDaiBQ = 0;
                }
                else
                {
                    tdd.Status = 1;
                }
                tdd.Note = txtNote.Text;
                int result;
                string arrLyDoThaiLoaiTrung = "";
                string arrSoLuong = "";
                tdd.TrungThaiLoai = 0;
                foreach (GridViewRow r in grvDanhSach.Rows)
                {
                    if (r.RowType == DataControlRowType.DataRow)
                    {
                        TextBox txtSoLuong = (TextBox)(r.FindControl("txtSoLuong"));
                        Button btnSaveThaiLoaiTrung = (Button)(r.FindControl("btnSaveThaiLoaiTrung"));
                        int soluong = 0;
                        arrLyDoThaiLoaiTrung += "@" + btnSaveThaiLoaiTrung.CommandArgument + "@";
                        if (int.TryParse(txtSoLuong.Text, out soluong) && soluong > 0)
                        {
                            arrSoLuong += "@" + txtSoLuong.Text + "@";
                            tdd.TrungThaiLoai += soluong;
                        }
                        else
                        {
                            arrSoLuong += "@0@";
                        }
                    }
                }
                string arrNhanVien = Config.GetSelectedValues_At(lstNhanVien);
                string lnkListCaSauDe = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "mid/" + this.ModuleId.ToString());
                if (tdd.ID != 0)
                {
                    result = csCont.UpdateTheoDoiDe(tdd, UserId, CountSoTrungNo(), arrLyDoThaiLoaiTrung, arrSoLuong, arrNhanVien);
                    if (result == 1) Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Cập nhật thành công!');window.location='" + lnkListCaSauDe + "';", true);
                    else if (result == -1) Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Không cập nhật được do đã có cá con chết hoặc giết mổ hoặc bán!');", true);
                    else if (result == -2) Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Cá mẹ không hợp lệ!');", true);
                    else if (result == -3) Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Ngày nở không hợp lệ!');", true);
                    else if (result == -4) Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Cá mẹ hoặc ngày nở không hợp lệ!');", true);
                    else if (result == -5) Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Số trứng nở mới ít hơn số trứng nở cũ: không hợp lệ!');", true);
                    else if (result == -1000) Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Cá này đã đẻ trong năm này rồi!');", true);
                    else Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Cập nhật không thành công!');", true);
                }
                else
                {
                    int IDTDD = 0;
                    result = csCont.InsertTheoDoiDe(tdd, UserId, arrLyDoThaiLoaiTrung, arrSoLuong, arrNhanVien, out IDTDD);
                    if (IDTDD != 0)
                    {
                        hdIDTDD.Value = IDTDD.ToString();
                        Delete.Visible = true;
                        Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Đã thêm xong!');window.location='" + lnkListCaSauDe + "';", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(string), "insertfail", "alert('Không thêm được do cá này đã đẻ trong năm này rồi!');", true);
                    }
                }
                //if (result == -1)
                //{
                //    Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Cập nhật không thành công!');", true);
                //}
                //else
                //{
                //    Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Cập nhật thành công!');", true);
                //}
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int IDTDD = int.Parse(hdIDTDD.Value);
            int res = csCont.TheoDoiDe_Delete(IDTDD, UserId);
            if (res == 1)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "dadelete", "alert('Đã xóa!');", true);
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "mid/" + this.ModuleId.ToString()));
            }
            else Page.ClientScript.RegisterStartupScript(typeof(string), "chuadelete", "alert('Xóa không được do đã có cá con chết hoặc giết mổ hoặc bán!');", true);
        }

        protected void ddlChuong_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataTable dtCaMe = csCont.LoadCaSauMe_AllTrangThai_ByChuong(int.Parse(ddlChuong.SelectedValue));
            DataTable dtCaMe = csCont.LoadCaSauMe_ByChuong_ByThoiDiem(int.Parse(ddlChuong.SelectedValue), DateTime.Parse(txtNgayVaoAp.Text, ci));
            ddlCaMe.DataSource = dtCaMe;
            ddlCaMe.DataValueField = "IDCaSau";
            ddlCaMe.DataTextField = "CaMe";
            ddlCaMe.DataBind();
            udpCaDe.Update();
        }

        protected void btnRefreshCaDe_Click(object sender, EventArgs e)
        {
            ddlChuong_SelectedIndexChanged(null, null);
        }

        private int CountSoTrungNo()
        {
            int trungde = 0;
            int trungvo = 0;
            int trungkhongphoi = 0;
            int trungchetphoi1 = 0;
            int trungchetphoi2 = 0;
            int TN = 0;

            if (int.TryParse(txtTrungDe.Text, out trungde) && trungde > 0)
                TN = trungde;
            if (int.TryParse(txtTrungVo.Text, out trungvo) && trungvo > 0)
                TN -= trungvo;
            if (int.TryParse(txtTrungKhongPhoi.Text, out trungkhongphoi) && trungkhongphoi > 0)
                TN -= trungkhongphoi;
            if (int.TryParse(txtTrungChetPhoi1.Text, out trungchetphoi1) && trungchetphoi1 > 0)
                TN -= trungchetphoi1;
            if (int.TryParse(txtTrungChetPhoi2.Text, out trungchetphoi2) && trungchetphoi2 > 0)
                TN -= trungchetphoi2;
            foreach (GridViewRow r in grvDanhSach.Rows)
            {
                int sl = 0;
                TextBox txtSoLuong = (TextBox)(r.FindControl("txtSoLuong"));
                if (int.TryParse(txtSoLuong.Text, out sl) && sl > 0)
                    TN -= sl;
            }
            return TN;
        }
    }
}