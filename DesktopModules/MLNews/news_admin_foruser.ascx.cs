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
using System.Xml;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DotNetNuke.News
{
    public partial class news_admin_foruser : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        string cat = "";
        private int CountPage(int pagesize, string CatID, DateTime? fromDate, DateTime? toDate)
        {
            string strSQL = "News_countNewsByCatByDate_Admin";
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter("@CatID", CatID);
            //para[1] = new SqlParameter("@FromDate", fromDate == null ? (DateTime?)DBNull.Value : fromDate);
            //para[2] = new SqlParameter("@ToDate", toDate == null ? (DateTime?)DBNull.Value : toDate);
            if (fromDate == null)
            {
                para[1] = new SqlParameter("@FromDate", DBNull.Value);
            }
            else
            {
                para[1] = new SqlParameter("@FromDate", fromDate);
            }
            if (toDate == null)
            {
                para[2] = new SqlParameter("@ToDate", DBNull.Value);
            }
            else
            {
                para[2] = new SqlParameter("@ToDate", toDate);
            }
            para[3] = new SqlParameter("@PortalID", PortalId);

            object o = DotNetNuke.NewsProvider.DataProvider.ExecuteScalarSP(strSQL, para);
            if (o != null)
            {
                int countrecord = Convert.ToInt32(o);
                int pg = countrecord / pagesize;
                if (pagesize * pg < countrecord) pg++;
                return pg;
            }
            return 0;
        }

        private void BindCatDDL()
        {
            string[] cats = null;
            if (Settings["cats"] != null && Settings["cats"].ToString() != "")
            {
                cats = Settings["cats"].ToString().Split(new char[] { ',' }, StringSplitOptions.None);
            }
            CategoryController catCont = new CategoryController();
            CategoryInfo cat;
            if (cats != null)
            {
                foreach (string c in cats)
                {
                    cat = catCont.Load(c);
                    ddlCat.Items.Add(new ListItem(cat.CatName, c));
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lnkFromDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtFromDate);
                lnkToDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtToDate);
                if (!Page.IsPostBack)
                {
                    BindCatDDL();
                    if (ddlCat.Items.Count == 0)
                    {
                        btnLoad.Enabled = false;
                    }
                    cat = "";
                    if (Request.QueryString["cat"] != null)
                        cat = Request.QueryString["cat"];
                    if (Request.QueryString["fromdate"] != null)
                        txtFromDate.Text = Request.QueryString["fromdate"].Replace("-","/").Remove(0,1);
                    if (Request.QueryString["todate"] != null)
                        txtToDate.Text = Request.QueryString["todate"].Replace("-", "/").Remove(0, 1);
                    if (ddlCat.Items.Count > 0)
                    {
                        if (cat == "")
                        {
                            ddlCat.Items[0].Selected = true;
                            cat = ddlCat.SelectedValue;
                        }
                        else
                        {
                            ddlCat.Text = cat;
                        }
                        if (Request.QueryString["pg"] != null)
                        {
                            LoadData();
                        }
                    }
                    lnkAdd.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit_news_foruser", "mid/" + this.ModuleId.ToString());
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void LoadData()
        {
            int pg = 0;
            if (ddlCat.SelectedValue == cat)
            {
                if (Request.QueryString["pg"] != null)
                    pg = Convert.ToInt32(Request.QueryString["pg"]) - 1;
            }
            int pagesize;
            try
            {
                pagesize = int.Parse(Settings["pageSizeAdmin"].ToString());
            }
            catch (System.Exception ex)
            {
                pagesize = 10;
            }
            int pages = CountPage(pagesize, ddlCat.SelectedValue, txtFromDate.Text == "" ? null : (DateTime?)(DateTime.Parse(txtFromDate.Text.Trim())), txtToDate.Text == "" ? null : (DateTime?)(DateTime.Parse(txtToDate.Text.Trim())));
            if (pages > 1)
            {
                string format = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "mid/" + this.ModuleId.ToString(), "cat/" + ddlCat.SelectedValue, "pg/@@page", "fromdate/-" + txtFromDate.Text.Trim().Replace("/", "-"), "todate/-" + txtToDate.Text.Trim().Replace("/", "-"));
                paging.Visible = true;
                paging.showing(pages, pg, format);
            }
            else
            {
                paging.Visible = false;
            }
            NewsController engine = new NewsController();
            string editFormatUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "edit_news_foruser", "mid/" + this.ModuleId.ToString(), "id/@@id", "cat/" + ddlCat.SelectedValue);
            XmlDocument doc = engine.LoadAdminXMLByCatByDate(editFormatUrl, pg, pagesize, ddlCat.SelectedValue, PortalId, txtFromDate.Text == "" ? null : (DateTime?)(DateTime.Parse(txtFromDate.Text.Trim())), txtToDate.Text == "" ? null : (DateTime?)(DateTime.Parse(txtToDate.Text.Trim())),"");
            string template = "DesktopModules/MLNews/Xsl/news_admin.xsl";
            DotNetNuke.NewsProvider.Utils.XMLTransform(xmlTransformer, template, doc);
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}