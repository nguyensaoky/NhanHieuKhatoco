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

namespace DotNetNuke.Modules.QLCS
{
    public partial class gietmoca_chitiet : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private CaSauController csCont = new CaSauController();
        decimal tongHoi = 0;
        decimal tongMocHam = 0;
        bool isAdmin = false;
        bool afterKhoaSo = false;
        private System.Collections.Generic.Dictionary<string, string> dicVatTu = new System.Collections.Generic.Dictionary<string, string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/js/jquery.js"));
                string VTGM = ConfigurationManager.AppSettings["QLCS_VatTuGietMo"];
                string VTGMText = ConfigurationManager.AppSettings["QLCS_VatTuGietMoText"];
                if (VTGM != "" && VTGM != null)
                {
                    string[] aVTGM = VTGM.Substring(1, VTGM.Length - 2).Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] aVTGMText = VTGMText.Substring(1, VTGMText.Length - 2).Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < aVTGM.Length; i++)
                    {
                        dicVatTu.Add(aVTGM[i], aVTGMText[i]);
                    }
                }
                if (!IsPostBack)
                {
                    if (Request.QueryString["gmcid"] != null)
                    {
                        lblGMC.Text = Request.QueryString["gmcid"];
                        DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
                        hdGietMoCaChonCaPage.Value = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_GietMoCaChonCaPage"], PortalId).TabID.ToString();
                        btnAddGietMoCaChiTiet.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdGietMoCaChonCaPage.Value), "", "gietmoca/" + lblGMC.Text) + "','',900,600);";
                        btnAddGietMoCaChiTiet.Attributes["style"] = "cursor:pointer;font-weight:bold;";
                        LoadData();
                    }
                    lnkCancel.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "gietmoca_edit", "mid/" + this.ModuleId.ToString(), "gmcid/" + Request.QueryString["gmcid"]);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void grvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow r = ((DataRowView)e.Row.DataItem).Row;
                Button btnXemThayDoi = (Button)(e.Row.FindControl("btnXemThayDoi"));
                btnXemThayDoi.CommandArgument = r["IDGMCCT"].ToString();
                btnXemThayDoi.CommandName = ddlRowStatus.SelectedValue;
                Label lblSTT = (Label)(e.Row.FindControl("lblSTT"));
                int stt = e.Row.RowIndex + 1;
                lblSTT.Text = stt.ToString();
                CheckBox chkDau = (CheckBox)(e.Row.FindControl("chkDau"));
                chkDau.Checked = Convert.ToBoolean(r["Dau"]);
                HyperLink lnkCaSau = (HyperLink)(e.Row.FindControl("lnkCaSau"));
                Label lblDa_TrongLuong = (Label)(e.Row.FindControl("lblDa_TrongLuong"));
                Label lblTrongLuongHoi = (Label)(e.Row.FindControl("lblTrongLuongHoi"));
                Label lblTrongLuongMocHam = (Label)(e.Row.FindControl("lblTrongLuongMocHam"));
                lnkCaSau.Attributes["onclick"] = "openwindow('" + DotNetNuke.Common.Globals.NavigateURL(int.Parse(hdGietMoCaChonCaPage.Value), "", "gmcct/" + r["IDGMCCT"].ToString(), "gietmoca/" + lblGMC.Text) + "','',800,600);";
                lnkCaSau.Attributes["style"] = "cursor:pointer;";
                tongHoi += decimal.Parse(r["TrongLuongHoi"].ToString());
                tongMocHam += decimal.Parse(r["TrongLuongMocHam"].ToString());
                CheckBox chkDiTat = (CheckBox)(e.Row.FindControl("chkDiTat"));
                chkDiTat.Checked = false;
                int Status = Convert.ToInt32(r["Status"]);
                if (Status == -4) chkDiTat.Checked = true;

                if (!afterKhoaSo)
                {
                    lnkCaSau.Attributes["onclick"] = "";
                }
                else
                {
                    if (!isAdmin) lnkCaSau.Attributes["onclick"] = "";
                }
                //lblDa_TrongLuong.Text = Convert.ToDecimal(r["Da_TrongLuong"]).ToString("0.#####");
                //lblTrongLuongHoi.Text = Convert.ToDecimal(r["TrongLuongHoi"]).ToString("0.#####");
                //lblTrongLuongMocHam.Text = Convert.ToDecimal(r["TrongLuongMocHam"]).ToString("0.#####");
                lblDa_TrongLuong.Text = Config.ToXVal2(r["Da_TrongLuong"],0);
                lblTrongLuongHoi.Text = Config.ToXVal2(r["TrongLuongHoi"],1);
                lblTrongLuongMocHam.Text = Config.ToXVal2(r["TrongLuongMocHam"],1);
                btnXemThayDoi.Attributes["style"] = "background-image:url('" + ModulePath + "images/log.gif');border:none;background-color:transparent;background-repeat:no-repeat;cursor:pointer;vertical-align:middle;width:16px;";
                e.Row.Attributes["id"] = "row_" + btnXemThayDoi.ClientID;
                e.Row.Attributes["onclick"] = "setSelectedRow(this,'" + btnXemThayDoi.ClientID + "')";

                Label lblVatTu = (Label)(e.Row.FindControl("lblVatTu"));
                string sVatTu = r["VatTu"].ToString();
                string sRes = "";
                if (sVatTu != "")
                {
                    string[] aVatTu = sVatTu.Substring(1, sVatTu.Length - 2).Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string VatTuGroup in aVatTu)
                    {
                        string[] aVatTuGroup = VatTuGroup.Split(new char[] { '/' });
                        sRes += dicVatTu[aVatTuGroup[0]] + ": " + aVatTuGroup[1] + ", ";
                    }
                }
                if (sRes != "")
                    sRes = sRes.Substring(0, sRes.Length - 2);
                lblVatTu.Text = sRes;
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void LoadData()
        {
            if (lblGMC.Text != "0")
            {
                DataTable tblGMC = csCont.GietMoCa_GetByID(int.Parse(lblGMC.Text));
                if (Convert.ToDateTime(tblGMC.Rows[0]["NgayMo"]) < Config.NgayKhoaSo())
                {
                    //btnAddGietMoCaChiTiet.Visible = false;
                    btnAddGietMoCaChiTiet.Enabled = false;
                    btnAddGietMoCaChiTiet.CssClass = "buttondisable";
                    afterKhoaSo = false;
                }
                else
                {
                    afterKhoaSo = true;
                    if (!UserInfo.IsSuperUser && !UserInfo.IsInRole("Administrators") && !CaSauController.HasEditPermission(UserInfo.Roles, "QLCS"))
                    {
                        btnAddGietMoCaChiTiet.Visible = false;
                        isAdmin = false;
                    }
                    else
                    {
                        if (Convert.ToBoolean(tblGMC.Rows[0]["Lock"]))
                        {
                            btnAddGietMoCaChiTiet.Visible = false;
                        }
                        else
                        {
                            btnAddGietMoCaChiTiet.Visible = true;
                            isAdmin = true;
                        }
                    }
                }
                grvDanhSach.DataSource = csCont.GietMoCa_GetChiTiet(int.Parse(lblGMC.Text), int.Parse(ddlRowStatus.SelectedValue));
                grvDanhSach.DataBind();
                if (grvDanhSach.Rows.Count == 0)
                {
                    lblNoData.Visible = true;
                }
                else
                {
                    lblNoData.Visible = false;
                }
                lblTongHoi.Text = tongHoi.ToString("0.#####");
                lblTongMocHam.Text = tongMocHam.ToString("0.#####");
            }
            else
            {
                btnAddGietMoCaChiTiet.Visible = false;
            }
        }

        protected void btnXemThayDoi_Click(object sender, EventArgs e)
        {
            DotNetNuke.Entities.Tabs.TabController t = new DotNetNuke.Entities.Tabs.TabController();
            int GietMoCaChiTietLichSuPage = t.GetTabByName(ConfigurationManager.AppSettings["QLCS_GietMoCaChiTietLichSuPage"], PortalId).TabID;
            Page.ClientScript.RegisterStartupScript(typeof(string), "fail", "<script language=javascript>openwindow('" + DotNetNuke.Common.Globals.NavigateURL(GietMoCaChiTietLichSuPage, "", "IDGietMoCaChiTiet/" + ((Button)sender).CommandArgument, "status/" + ((Button)sender).CommandName) + "','',1000,600);</script>", false);
        }
    }
}