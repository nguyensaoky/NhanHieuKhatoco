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
    public partial class casauan_updatehangloat : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        private CaSauController csCont = new CaSauController();
        decimal TongKhoiLuong = 0;
        decimal TongKLTT = 0;
        decimal TongKLG = 0;

        decimal cTongKhoiLuong = 0;
        decimal cTongKLTT = 0;
        int cTongSoLuong = 0;
        int cTongSLTT = 0;

        int ChuongScale = 1;
        public int scale = 0;
        public int scaleCT = 0;
        public string thapphan = ".";

        private void BindControls()
        {
            txtThoiDiemFrom.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();

            DataTable dtLoaiCa = csCont.LoadLoaiCa(1);
            ddlLoaiCa.DataSource = dtLoaiCa;
            ddlLoaiCa.DataTextField = "TenLoaiCa";
            ddlLoaiCa.DataValueField = "IDLoaiCa";
            ddlLoaiCa.DataBind();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (System.Threading.Thread.CurrentThread.CurrentCulture.ToString() == "vi-VN")
                    thapphan = ",";
                else thapphan = ".";
                scale = int.Parse(ConfigurationManager.AppSettings["QLCS_VatTu_TA_Scale"]);
                scaleCT = scale;
                if (!Page.IsPostBack)
                {
                    BindControls();
                }

                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                {
                    btnUpdateThucAn.Visible = false;
                    btnUpdateThuoc.Visible = false;
                }       
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                btnUpdateThucAn.Visible = false;
                btnUpdateThuoc.Visible = false;
            }
        }

        protected void btnUpdateThucAn_Click(object sender, EventArgs e)
        {
            decimal KhoiLuong = 0;
            string NguoiChoAn = "";
            DateTime NgayAn = DateTime.MinValue;

            string StrPhanCachKhuChuong = "";
            string khuchuong = "";
            string currkhuchuong = "";
            string StrSoLuongChuong = "";
            string StrSoLuongChuongTT = "";
            string StrChuong = "";
            string StrKL = "";
            int SoLuongCa = 0;
            int SoLuongTT = 0;
            int index = 0;

            if (txtThoiDiemFrom.Text.Trim() == "") txtThoiDiemFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime TuNgay = DateTime.Parse(txtThoiDiemFrom.Text.Trim(), ci);
            DateTime DenNgay;
            if (txtThoiDiemTo.Text.Trim() == "") DenNgay = DateTime.Now;
            else DenNgay = DateTime.Parse(txtThoiDiemTo.Text.Trim(), ci);

            string strLoaiCa = Config.GetSelectedValues_At(ddlLoaiCa);
            
            DataTable tblCaAn = csCont.LoadCaSauAnTuNgayDenNgay(TuNgay, DenNgay);

            int current = 0;
            if (strLoaiCa == "")
            {
                foreach (DataRow r in tblCaAn.Rows)
                {
                    current++;
                    DataTable tblThucAn = csCont.CaSauAn_GetThucAn(Convert.ToInt32(r["ID"]), 1);
                    foreach (DataRow rThucAn in tblThucAn.Rows)
                    {
                        SoLuongCa = 0;
                        SoLuongTT = 0;
                        StrSoLuongChuong = "";
                        StrSoLuongChuongTT = "";
                        StrKL = "";
                        StrChuong = "";
                        StrPhanCachKhuChuong = "";
                        currkhuchuong = "";
                        khuchuong = "";

                        DataTable tblChuong = csCont.CaSauAn_GetChuongByThucAnByLoaiCa(Convert.ToInt32(r["ID"]), Convert.ToInt32(rThucAn["ThucAn"]),
                            Convert.ToInt32(rThucAn["LoaiCa"]), out KhoiLuong, out SoLuongCa, out SoLuongTT, out NguoiChoAn, out NgayAn);
                        SoLuongCa = 0;
                        SoLuongTT = 0;
                        index = 0;
                        foreach (DataRow rChuong in tblChuong.Rows)
                        {
                            if (rChuong["KhoiLuong"] != DBNull.Value && Convert.ToDecimal(rChuong["KhoiLuong"]) != 0)
                            {
                                StrSoLuongChuong += "@" + rChuong["SoLuong"].ToString() + "@";
                                StrSoLuongChuongTT += "@" + rChuong["SoLuongTT"].ToString() + "@";
                                StrChuong += "@" + rChuong["IDChuong"].ToString() + "@";
                                StrKL += "@" + Convert.ToDecimal(rChuong["KhoiLuong"]).ToString("0.#####").Replace(",", ".") + "@";
                                SoLuongCa += Convert.ToInt32(rChuong["SoLuong"]);
                                SoLuongTT += Convert.ToInt32(rChuong["SoLuongTT"]);
                                currkhuchuong = rChuong["Chuong"].ToString().Substring(0, 2);
                                if (currkhuchuong != khuchuong)
                                {
                                    StrPhanCachKhuChuong += "@" + index + "@";
                                    khuchuong = currkhuchuong;
                                }
                                index++;
                            }
                        }

                        if (StrPhanCachKhuChuong != "") StrPhanCachKhuChuong = StrPhanCachKhuChuong.Substring(3) + "@" + index.ToString() + "@";

                        csCont.CaSauAn_InsertUpdateThucAn_NoCheck_ForUpdateHangLoat(Convert.ToInt32(r["ID"]), Convert.ToInt32(rThucAn["ThucAn"]),
                            KhoiLuong, Convert.ToInt32(rThucAn["LoaiCa"]), SoLuongCa, SoLuongTT, StrSoLuongChuong,
                            StrSoLuongChuongTT, StrChuong, StrKL, StrPhanCachKhuChuong, UserId);
                    }
                }
            }
            else
            {
                foreach (DataRow r in tblCaAn.Rows)
                {
                    current++;
                    DataTable tblThucAn = csCont.CaSauAn_GetThucAn(Convert.ToInt32(r["ID"]), 1);
                    foreach (DataRow rThucAn in tblThucAn.Rows)
                    {
                        if (strLoaiCa.Contains("@" + rThucAn["LoaiCa"].ToString() + "@"))
                        {
                            SoLuongCa = 0;
                            SoLuongTT = 0;
                            StrSoLuongChuong = "";
                            StrSoLuongChuongTT = "";
                            StrKL = "";
                            StrChuong = "";
                            StrPhanCachKhuChuong = "";
                            currkhuchuong = "";
                            khuchuong = "";

                            DataTable tblChuong = csCont.CaSauAn_GetChuongByThucAnByLoaiCa(Convert.ToInt32(r["ID"]), Convert.ToInt32(rThucAn["ThucAn"]),
                                Convert.ToInt32(rThucAn["LoaiCa"]), out KhoiLuong, out SoLuongCa, out SoLuongTT, out NguoiChoAn, out NgayAn);
                            SoLuongCa = 0;
                            SoLuongTT = 0;
                            index = 0;
                            foreach (DataRow rChuong in tblChuong.Rows)
                            {
                                if (rChuong["KhoiLuong"] != DBNull.Value && Convert.ToDecimal(rChuong["KhoiLuong"]) != 0)
                                {
                                    StrSoLuongChuong += "@" + rChuong["SoLuong"].ToString() + "@";
                                    StrSoLuongChuongTT += "@" + rChuong["SoLuongTT"].ToString() + "@";
                                    StrChuong += "@" + rChuong["IDChuong"].ToString() + "@";
                                    StrKL += "@" + Convert.ToDecimal(rChuong["KhoiLuong"]).ToString("0.#####").Replace(",", ".") + "@";
                                    SoLuongCa += Convert.ToInt32(rChuong["SoLuong"]);
                                    SoLuongTT += Convert.ToInt32(rChuong["SoLuongTT"]);
                                    currkhuchuong = rChuong["Chuong"].ToString().Substring(0, 2);
                                    if (currkhuchuong != khuchuong)
                                    {
                                        StrPhanCachKhuChuong += "@" + index + "@";
                                        khuchuong = currkhuchuong;
                                    }
                                    index++;
                                }
                            }

                            if (StrPhanCachKhuChuong != "") StrPhanCachKhuChuong = StrPhanCachKhuChuong.Substring(3) + "@" + index.ToString() + "@";

                            csCont.CaSauAn_InsertUpdateThucAn_NoCheck_ForUpdateHangLoat(Convert.ToInt32(r["ID"]), Convert.ToInt32(rThucAn["ThucAn"]),
                                KhoiLuong, Convert.ToInt32(rThucAn["LoaiCa"]), SoLuongCa, SoLuongTT, StrSoLuongChuong,
                                StrSoLuongChuongTT, StrChuong, StrKL, StrPhanCachKhuChuong, UserId);
                        }
                    }
                }
            }
            Page.ClientScript.RegisterStartupScript(typeof(string), "daluuxong", "alert('Số ngày ăn đã cập nhật: " + tblCaAn.Rows.Count.ToString() + ", thời điểm kết thúc: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "');", true);
        }

        protected void btnUpdateThuoc_Click(object sender, EventArgs e)
        {
            decimal KhoiLuong = 0;
            DateTime NgayAn = DateTime.MinValue;

            string StrPhanCachKhuChuong = "";
            string khuchuong = "";
            string currkhuchuong = "";
            string StrSoLuongChuong = "";
            string StrSoLuongChuongTT = "";
            string StrChuong = "";
            string StrKL = "";
            int SoLuongCa = 0;
            int SoLuongTT = 0;
            int index = 0;

            if (txtThoiDiemFrom.Text.Trim() == "") txtThoiDiemFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime TuNgay = DateTime.Parse(txtThoiDiemFrom.Text.Trim(), ci);
            DateTime DenNgay;
            if (txtThoiDiemTo.Text.Trim() == "") DenNgay = DateTime.Now;
            else DenNgay = DateTime.Parse(txtThoiDiemTo.Text.Trim(), ci);

            string strLoaiCa = Config.GetSelectedValues_At(ddlLoaiCa);

            DataTable tblCaAn = csCont.LoadCaSauAnTuNgayDenNgay(TuNgay, DenNgay);

            int current = 0;
            if (strLoaiCa == "")
            {
                foreach (DataRow r in tblCaAn.Rows)
                {
                    current++;
                    DataTable tblThuoc = csCont.CaSauAn_GetThuoc(Convert.ToInt32(r["ID"]), 1);
                    foreach (DataRow rThuoc in tblThuoc.Rows)
                    {
                        SoLuongCa = 0;
                        SoLuongTT = 0;
                        StrSoLuongChuong = "";
                        StrSoLuongChuongTT = "";
                        StrKL = "";
                        StrChuong = "";
                        StrPhanCachKhuChuong = "";
                        currkhuchuong = "";
                        khuchuong = "";

                        DataTable tblChuong = csCont.CaSauAn_GetChuongByThuocByLoaiCa(Convert.ToInt32(r["ID"]), Convert.ToInt32(rThuoc["Thuoc"]),
                            Convert.ToInt32(rThuoc["LoaiCa"]), out KhoiLuong, out SoLuongCa, out SoLuongTT, out NgayAn);
                        SoLuongCa = 0;
                        SoLuongTT = 0;
                        index = 0;
                        foreach (DataRow rChuong in tblChuong.Rows)
                        {
                            if (rChuong["KhoiLuong"] != DBNull.Value && Convert.ToDecimal(rChuong["KhoiLuong"]) != 0)
                            {
                                StrSoLuongChuong += "@" + rChuong["SoLuong"].ToString() + "@";
                                StrSoLuongChuongTT += "@" + rChuong["SoLuongTT"].ToString() + "@";
                                StrChuong += "@" + rChuong["IDChuong"].ToString() + "@";
                                StrKL += "@" + Convert.ToDecimal(rChuong["KhoiLuong"]).ToString("0.#####").Replace(",", ".") + "@";
                                SoLuongCa += Convert.ToInt32(rChuong["SoLuong"]);
                                SoLuongTT += Convert.ToInt32(rChuong["SoLuongTT"]);
                                currkhuchuong = rChuong["Chuong"].ToString().Substring(0, 2);
                                if (currkhuchuong != khuchuong)
                                {
                                    StrPhanCachKhuChuong += "@" + index + "@";
                                    khuchuong = currkhuchuong;
                                }
                                index++;
                            }
                        }

                        if (StrPhanCachKhuChuong != "") StrPhanCachKhuChuong = StrPhanCachKhuChuong.Substring(3) + "@" + index.ToString() + "@";

                        csCont.CaSauAn_InsertUpdateThuoc_NoCheck_ForUpdateHangLoat(Convert.ToInt32(r["ID"]), Convert.ToInt32(rThuoc["Thuoc"]),
                            KhoiLuong, Convert.ToInt32(rThuoc["LoaiCa"]), SoLuongCa, SoLuongTT, StrSoLuongChuong,
                            StrSoLuongChuongTT, StrChuong, StrKL, StrPhanCachKhuChuong, UserId);
                    }
                }
            }
            else
            {
                foreach (DataRow r in tblCaAn.Rows)
                {
                    current++;
                    DataTable tblThuoc = csCont.CaSauAn_GetThuoc(Convert.ToInt32(r["ID"]), 1);
                    foreach (DataRow rThuoc in tblThuoc.Rows)
                    {
                        if (strLoaiCa.Contains("@" + rThuoc["LoaiCa"].ToString() + "@"))
                        {
                            SoLuongCa = 0;
                            SoLuongTT = 0;
                            StrSoLuongChuong = "";
                            StrSoLuongChuongTT = "";
                            StrKL = "";
                            StrChuong = "";
                            StrPhanCachKhuChuong = "";
                            currkhuchuong = "";
                            khuchuong = "";

                            DataTable tblChuong = csCont.CaSauAn_GetChuongByThuocByLoaiCa(Convert.ToInt32(r["ID"]), Convert.ToInt32(rThuoc["Thuoc"]),
                                Convert.ToInt32(rThuoc["LoaiCa"]), out KhoiLuong, out SoLuongCa, out SoLuongTT, out NgayAn);
                            SoLuongCa = 0;
                            SoLuongTT = 0;
                            index = 0;
                            foreach (DataRow rChuong in tblChuong.Rows)
                            {
                                if (rChuong["KhoiLuong"] != DBNull.Value && Convert.ToDecimal(rChuong["KhoiLuong"]) != 0)
                                {
                                    StrSoLuongChuong += "@" + rChuong["SoLuong"].ToString() + "@";
                                    StrSoLuongChuongTT += "@" + rChuong["SoLuongTT"].ToString() + "@";
                                    StrChuong += "@" + rChuong["IDChuong"].ToString() + "@";
                                    StrKL += "@" + Convert.ToDecimal(rChuong["KhoiLuong"]).ToString("0.#####").Replace(",", ".") + "@";
                                    SoLuongCa += Convert.ToInt32(rChuong["SoLuong"]);
                                    SoLuongTT += Convert.ToInt32(rChuong["SoLuongTT"]);
                                    currkhuchuong = rChuong["Chuong"].ToString().Substring(0, 2);
                                    if (currkhuchuong != khuchuong)
                                    {
                                        StrPhanCachKhuChuong += "@" + index + "@";
                                        khuchuong = currkhuchuong;
                                    }
                                    index++;
                                }
                            }

                            if (StrPhanCachKhuChuong != "") StrPhanCachKhuChuong = StrPhanCachKhuChuong.Substring(3) + "@" + index.ToString() + "@";

                            csCont.CaSauAn_InsertUpdateThuoc_NoCheck_ForUpdateHangLoat(Convert.ToInt32(r["ID"]), Convert.ToInt32(rThuoc["Thuoc"]),
                                KhoiLuong, Convert.ToInt32(rThuoc["LoaiCa"]), SoLuongCa, SoLuongTT, StrSoLuongChuong,
                                StrSoLuongChuongTT, StrChuong, StrKL, StrPhanCachKhuChuong, UserId);
                        }
                    }
                }
            }
            Page.ClientScript.RegisterStartupScript(typeof(string), "daluuxong", "alert('Số ngày ăn đã cập nhật: " + tblCaAn.Rows.Count.ToString() + ", thời điểm kết thúc: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "');", true);
        }
    }
}