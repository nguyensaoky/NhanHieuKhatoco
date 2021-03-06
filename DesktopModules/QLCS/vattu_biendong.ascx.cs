﻿using System;
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
    public partial class vattu_biendong : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
        CaSauController csCont = new CaSauController();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["VatTu"] != null)
                    {
                        DataTable tblVatTu = csCont.LoadVatTu_HienTaiByID(int.Parse(Request.QueryString["VatTu"]));
                        if (tblVatTu.Rows.Count == 0)
                        {
                            btnSave.Enabled = false;
                            return;
                        }

                        DataTable tblNhaCungCap = csCont.LoadNhaCungCapByVatTu(int.Parse(Request.QueryString["VatTu"]));
                        ddlNhaCungCap.DataSource = tblNhaCungCap;
                        ddlNhaCungCap.DataTextField = "NhaCungCap";
                        ddlNhaCungCap.DataValueField = "ID";
                        ddlNhaCungCap.DataBind();
                        ddlNhaCungCap.Items.Insert(0, new ListItem("", "0"));
                        if (ddlNhaCungCap.Items.Count > 1)
                            divNhaCungCap.Visible = true;
                        else
                            divNhaCungCap.Visible = false;

                        lblTenVatTu.Text = tblVatTu.Rows[0]["TenVatTu"].ToString();
                        lblDonViTinh.Text = tblVatTu.Rows[0]["DonViTinh"].ToString();
                        txtThoiDiemBienDong.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        DataTable tblLoaiBD = csCont.LoadLoaiBienDongVatTu();
                        ddlLoaiBienDong.DataSource = tblLoaiBD;
                        ddlLoaiBienDong.DataTextField = "TenLoaiBienDong";
                        ddlLoaiBienDong.DataValueField = "IDLoaiBienDong";
                        ddlLoaiBienDong.DataBind();
                        string LVT = tblVatTu.Rows[0]["LoaiVatTu"].ToString();
                        if (LVT == "TTY" || LVT == "TA") ddlLoaiBienDong.SelectedValue = "1";
                        else ddlLoaiBienDong.SelectedValue = "2";
                    }
                    else
                    {
                        btnSave.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlLoaiBienDong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((ddlLoaiBienDong.SelectedValue == "1" || ddlLoaiBienDong.SelectedValue == "4") && ddlNhaCungCap.Items.Count > 1)
                divNhaCungCap.Visible = true;
            else
                divNhaCungCap.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtThoiDiemBienDong.Text == "")
                {
                    lblError.Text = "Thời điểm biến động không hợp lệ";
                    return;
                }
                DateTime dtThoiDiemBienDong = DateTime.Parse(txtThoiDiemBienDong.Text, culture);
                if (dtThoiDiemBienDong < Config.NgayKhoaSo())
                {
                    lblError.Text = "Ngày biến động không được trước ngày khóa sổ";
                    return;
                }
                int res = csCont.VatTu_ThemBienDong(int.Parse(Request.QueryString["VatTu"]), decimal.Parse(txtSoLuongBienDong.Text), int.Parse(ddlLoaiBienDong.SelectedValue), txtNote.Text, DateTime.Parse(txtThoiDiemBienDong.Text, culture), Convert.ToInt32(Session["UserID"]), int.Parse(ddlNhaCungCap.SelectedValue));
                if(res == 1) Page.ClientScript.RegisterStartupScript(typeof(string), "updated", "<script language=javascript>alert('Đã lưu biến động');window.opener.finishEdit();self.close();</script>", false);
                else Page.ClientScript.RegisterStartupScript(typeof(string), "notupdated", "<script language=javascript>alert('Không thành công. Số lượng hoặc thời điểm biến động không hợp lệ');</script>", false);
            }
            catch (Exception ex)
            {
            }
        }
    }
}