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
    public partial class gietmoca_edit : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        public int scale = 0;

        public void LoadData(int GietMoCa)
        {
            DataTable tblGMC = csCont.GietMoCa_GetByID(GietMoCa);
            if(tblGMC.Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "khongcodulieu", "alert('Không có dữ liệu');", true);
                Save.Enabled = false;
                return;
            }
            txtNgayMo.Text = ((DateTime)tblGMC.Rows[0]["NgayMo"]).ToString("dd/MM/yyyy HH:mm:ss");
            txtTrongLuongHoi.Text = Config.ToXVal2(tblGMC.Rows[0]["TrongLuongHoi"],1);
            txtTrongLuongMocHam.Text = Config.ToXVal2(tblGMC.Rows[0]["TrongLuongMocHam"],1);
            txtBienBan.Text = tblGMC.Rows[0]["BienBan"].ToString();
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            DateTime dtNgayMo = DateTime.Parse(txtNgayMo.Text, ci);
            if (dtNgayMo < Config.NgayKhoaSo())
            {
                lblCanEdit.Text = "0";
                Save.Enabled = false;
                Save.CssClass = "buttondisable";
            }
            else
            {
                if (Convert.ToBoolean(tblGMC.Rows[0]["Lock"]))
                {
                    lblCanEdit.Text = "0";
                    Save.Visible = false;
                }
                else
                {
                    lblCanEdit.Text = "1";
                    Save.Visible = true;
                }
            }
        }

        public void LoadDataSanPham(int GietMoCa)
        {
            hdSanPham.Value = "";
            DataTable tblGMCSP = csCont.GietMoCa_GetSanPhamByID(GietMoCa);
            if (lblCanEdit.Text == "1")
            {
                int i = 0;
                foreach (DataRow r in tblGMCSP.Rows)
                {
                    i++;
                    Label lblIDVatTu = new Label();
                    lblIDVatTu.ID = "lblIDVatTu" + r["IDVatTu"].ToString();
                    lblIDVatTu.Text = "<div style='width:100px;float:left;text-align:left;'>" + r["TenVatTu"].ToString() + "</div>";
                    dsSanPham.Controls.Add(lblIDVatTu);
                    TextBox txt = new TextBox();
                    txt.ID = r["IDVatTu"].ToString();
                    txt.Attributes["style"] = "float:left;text-align:right;width:70px;";
                    if (r["KhoiLuong"] != DBNull.Value)
                        txt.Text = Convert.ToDecimal(r["KhoiLuong"]).ToString("0.#####");
                    dsSanPham.Controls.Add(txt);
                    Label lblDVT = new Label();
                    lblDVT.Text = "<div style='text-align:left;float:left;width:40px;'>&nbsp;" + r["DonViTinh"].ToString() + "</div>";
                    lblDVT.ID = "lblDVT" + r["IDVatTu"].ToString();
                    dsSanPham.Controls.Add(lblDVT);
                    
                    Label lbl = new Label();
                    lbl.ID = "lbl" + r["IDVatTu"].ToString();
                    lbl.Attributes["style"] = "visibility:hidden;";
                    if (r["KhoiLuong"] != DBNull.Value)
                        lbl.Text = Convert.ToDecimal(r["KhoiLuong"]).ToString("0.#####");
                    else
                        lbl.Text = "0";
                    dsSanPham.Controls.Add(lbl);

                    Button btnXemThayDoi = new Button();
                    btnXemThayDoi.ToolTip = "Xem thay đổi (F1)";
                    btnXemThayDoi.Click += new EventHandler(btnXemThayDoi_Click);
                    btnXemThayDoi.ID = "btnXemThayDoi" + i.ToString();
                    btnXemThayDoi.CommandArgument = r["ID"].ToString();
                    btnXemThayDoi.Attributes["style"] = "background-image:url('" + ModulePath + "images/log.gif');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;width:16px;float:left;";
                    if (r["ID"] == DBNull.Value)
                        btnXemThayDoi.Visible = false;
                    dsSanPham.Controls.Add(btnXemThayDoi);
                    Label lblXuongDong = new Label();
                    lblXuongDong.Text = "<div style='clear:both;'></div>";
                    dsSanPham.Controls.Add(lblXuongDong);
                    hdSanPham.Value += r["IDVatTu"].ToString() + ",";

                    txt.Attributes["onclick"] = "setSelectedRow(this,'" + btnXemThayDoi.ClientID + "')";
                }
                Button btn = new Button();
                btn.Text = "Lưu sản phẩm giết mổ";
                btn.Click += new EventHandler(btn_Click);
                btn.ID = "btn";
                btn.CssClass = "button";
                btn.Attributes["style"] = "float:left;margin-left: 100px;margin-top:10px;";
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    btn.Visible = false;
                }
                else if (lblCanEdit.Text == "0")
                {
                    btn.Visible = false;
                }
                dsSanPham.Controls.Add(btn);
                if (hdSanPham.Value != "") hdSanPham.Value = hdSanPham.Value.Substring(0, hdSanPham.Value.Length - 1);
            }
            else
            {
                int i = 0;
                foreach (DataRow r in tblGMCSP.Rows)
                {
                    i++;
                    Label lbl = new Label();
                    lbl.Text = "<div style='width:100px;float:left;text-align:left;'>" + r["TenVatTu"].ToString() + "</div>";
                    dsSanPham.Controls.Add(lbl);
                    TextBox txt = new TextBox();
                    txt.ID = r["IDVatTu"].ToString();
                    txt.Attributes["style"] = "float:left;text-align:right;width:70px;";
                    if (r["KhoiLuong"] != DBNull.Value)
                        txt.Text = Convert.ToDecimal(r["KhoiLuong"]).ToString("0.#####");
                    dsSanPham.Controls.Add(txt);
                    Label lblDVT = new Label();
                    lblDVT.Text = "<div style='text-align:left;float:left;width:200px;'>&nbsp;" + r["DonViTinh"].ToString() + "</div>";
                    lblDVT.ID = "lblDVT" + r["IDVatTu"].ToString();
                    dsSanPham.Controls.Add(lblDVT);
                    Button btnXemThayDoi = new Button();
                    btnXemThayDoi.ToolTip = "Xem thay đổi";
                    btnXemThayDoi.Click += new EventHandler(btnXemThayDoi_Click);
                    btnXemThayDoi.ID = "btnXemThayDoi" + i.ToString();
                    btnXemThayDoi.CommandArgument = r["ID"].ToString();
                    btnXemThayDoi.Attributes["style"] = "background-image:url('" + ModulePath + "images/log.gif');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;width:16px;float:left;";
                    if (r["ID"] == DBNull.Value)
                        btnXemThayDoi.Visible = false;
                    dsSanPham.Controls.Add(btnXemThayDoi);
                    Label lblXuongDong = new Label();
                    lblXuongDong.Text = "<div style='clear:both;'></div>";
                    dsSanPham.Controls.Add(lblXuongDong);
                }
            }
        }

        public void LoadDCS(int GietMoCa)
        {
            
            DataTable tblDCS = csCont.GietMoCa_GetDCS(GietMoCa);
            foreach (DataRow dr in tblDCS.Rows)
            {
                Label lbl = new Label();
                lbl.Text = "<div style='width:150px;float:left;text-align:left;'>" + dr["TenVatTu"].ToString() + "</div>";
                dsDCS.Controls.Add(lbl);
                TextBox txt = new TextBox();
                txt.Attributes["style"] = "float:left;text-align:right;width:70px;";
                if (dr["SoLuongBienDong"] != DBNull.Value)
                    txt.Text = Config.ToXVal2(dr["SoLuongBienDong"], scale);
                dsDCS.Controls.Add(txt);
                Label lblXuongDong = new Label();
                lblXuongDong.Text = "&nbsp;" + dr["DonViTinh"].ToString() + "<div style='clear:both;'></div>";
                dsDCS.Controls.Add(lblXuongDong);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["gmcid"] == null)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "khongcodulieu", "alert('Không có dữ liệu');", true);
                    Save.Enabled = false;
                    return;
                }
                scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_DCS_Scale"]);
                if (!Page.IsPostBack)
                {
                    lnkChiTiet.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "gietmoca_chitiet", "mid/" + this.ModuleId.ToString(), "gmcid/" + Request.QueryString["gmcid"]);
                    lnkCancel.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "");
                    LoadData(int.Parse(Request.QueryString["gmcid"]));
                }
                LoadDataSanPham(int.Parse(Request.QueryString["gmcid"]));
                LoadDCS(int.Parse(Request.QueryString["gmcid"]));
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    Save.Visible = false;
                    lnkChiTiet.Text = "Xem cá giết mổ";
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Save.Visible = false;
            }
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            try 
	        {
                //System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                //Button btn = (Button)sender;
                //TextBox txt = (TextBox)dsSanPham.FindControl(btn.ID.Substring(3));
                //if (txt.Text == "") txt.Text = "0";
                //int SanPham = int.Parse(btn.ID.Substring(3));
                //int result = csCont.GietMoCa_UpdateSanPham(int.Parse(Request.QueryString["gmcid"]), SanPham, decimal.Parse(txt.Text), DateTime.Parse(txtNgayMo.Text, ci), UserId);
                //if (result == 0) Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Khối lượng thay đổi không hợp lệ');", true);
                //else Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Cập nhật thành công!');", true);

                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string[] arrSanPham = hdSanPham.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string res = "";
                foreach (string s in arrSanPham)
                {
                    TextBox txt = (TextBox)dsSanPham.FindControl(s);
                    if (txt.Text == "") txt.Text = "0";
                    Label lbl = (Label)dsSanPham.FindControl("lbl" + s);
                    if (decimal.Parse(txt.Text) != decimal.Parse(lbl.Text))
                    {
                        int SanPham = int.Parse(s);
                        int result = csCont.GietMoCa_UpdateSanPham(int.Parse(Request.QueryString["gmcid"]), SanPham, decimal.Parse(txt.Text), DateTime.Parse(txtNgayMo.Text, ci), UserId);
                        if (result == 0) res += ((Label)dsSanPham.FindControl("lblIDVatTu" + s)).Text + ", ";
                    }
                }

                if (res != "") 
                {
                    res = res.Substring(0, res.Length - 2);
                    Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Khối lượng thay đổi không hợp lệ đối với " + res + "');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Cập nhật thành công!');", true);
                }
	        }
	        catch (Exception ex)
	        {
	        }
        }

        protected void btnXemThayDoi_Click(object sender, EventArgs e)
        {
            DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
            int GietMoCaSanPhamLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_GietMoCaSanPhamLichSuPage"], PortalId).TabID;
            Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(GietMoCaSanPhamLichSuPage, "", "IDGietMoCaSanPham/" + ((Button)sender).CommandArgument) + "','',1000,600);</script>", false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                if(txtNgayMo.Text == "")
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "thieudulieu", "alert('Bạn phải nhập thời điểm mổ!');", true);
                    return;
                }
                DateTime dtNgayMo = DateTime.Parse(txtNgayMo.Text, ci);
                if (dtNgayMo < Config.NgayKhoaSo())
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "ngaymo", "alert('Thời điểm mổ không được trước ngày khóa sổ!');", true);
                    return;
                }
                int result = csCont.GietMoCa_Update(int.Parse(Request.QueryString["gmcid"]), DateTime.Parse(txtNgayMo.Text, ci), txtBienBan.Text, UserId);
                if (result == 0) Page.ClientScript.RegisterStartupScript(typeof(string), "insertupdatefail", "alert('Thời điểm mổ không hợp lệ');", true);
                else Page.ClientScript.RegisterStartupScript(typeof(string), "insertsuccess", "alert('Cập nhật thành công!');", true);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}