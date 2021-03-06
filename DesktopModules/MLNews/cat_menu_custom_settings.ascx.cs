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

using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common.Utilities;
using System.Collections.Generic;

namespace DotNetNuke.News
{
    public partial class cat_menu_custom_settings : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        protected void LoadBaiViet()
        {
            NewsController cont = new NewsController();
            DataTable tblNews = cont.LoadTable(PortalId);
            lstBaiViet1.DataSource = tblNews;
            lstBaiViet2.DataSource = tblNews;
            lstBaiViet3.DataSource = tblNews;
            lstBaiViet1.DataTextField = "Headline";
            lstBaiViet2.DataTextField = "Headline";
            lstBaiViet3.DataTextField = "Headline";
            lstBaiViet1.DataValueField = "ID";
            lstBaiViet2.DataValueField = "ID";
            lstBaiViet3.DataValueField = "ID";
            lstBaiViet1.DataBind();
            lstBaiViet2.DataBind();
            lstBaiViet3.DataBind();

            string path = PortalSettings.HomeDirectoryMapPath + "Xsl";
            DotNetNuke.NewsProvider.Utils.BindTemplateByName(ddlTemplate1, path, "news_list*.xsl");
            DotNetNuke.NewsProvider.Utils.BindTemplateByName(ddlTemplate2, path, "news_list*.xsl");
            DotNetNuke.NewsProvider.Utils.BindTemplateByName(ddlTemplate3, path, "news_list*.xsl");
        }

        public override void LoadSettings()
        {
            try
            {
                LoadBaiViet();

                if (ModuleSettings["numshortnews1"] != null) txtNumShortNews1.Text = ModuleSettings["numshortnews1"].ToString();
                if (ModuleSettings["numshortnews2"] != null) txtNumShortNews2.Text = ModuleSettings["numshortnews2"].ToString();
                if (ModuleSettings["numshortnews3"] != null) txtNumShortNews3.Text = ModuleSettings["numshortnews3"].ToString();

                if (ModuleSettings["imagewidth1"] != null) txtImageWidth1.Text = ModuleSettings["imagewidth1"].ToString();
                if (ModuleSettings["imagewidth2"] != null) txtImageWidth2.Text = ModuleSettings["imagewidth2"].ToString();
                if (ModuleSettings["imagewidth3"] != null) txtImageWidth3.Text = ModuleSettings["imagewidth3"].ToString();

                if (ModuleSettings["template1"] != null) ddlTemplate1.SelectedValue = ModuleSettings["template1"].ToString();
                if (ModuleSettings["template2"] != null) ddlTemplate2.SelectedValue = ModuleSettings["template2"].ToString();
                if (ModuleSettings["template3"] != null) ddlTemplate3.SelectedValue = ModuleSettings["template3"].ToString();

                if (ModuleSettings["tieude1"] != null) txtTieuDe1.Text = ModuleSettings["tieude1"].ToString();
                if (ModuleSettings["baiviet1"] != null && ModuleSettings["baiviet1"].ToString() != "")
                {
                    string baiviet1 = ModuleSettings["baiviet1"].ToString();
                    baiviet1 = baiviet1.Substring(1, baiviet1.Length - 2);
                    string[] a = baiviet1.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in a)
                    {
                        foreach (ListItem i in lstBaiViet1.Items)
                        {
                            if (s == i.Value)
                            {
                                i.Selected = true;
                                break;
                            }
                        }
                    }
                }

                if (ModuleSettings["tieude2"] != null) txtTieuDe2.Text = ModuleSettings["tieude2"].ToString();
                if (ModuleSettings["baiviet2"] != null && ModuleSettings["baiviet2"].ToString() != "")
                {
                    string baiviet2 = ModuleSettings["baiviet2"].ToString();
                    baiviet2 = baiviet2.Substring(1, baiviet2.Length - 2);
                    string[] a = baiviet2.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in a)
                    {
                        foreach (ListItem i in lstBaiViet2.Items)
                        {
                            if (s == i.Value)
                            {
                                i.Selected = true;
                                break;
                            }
                        }
                    }
                }

                if (ModuleSettings["tieude3"] != null) txtTieuDe3.Text = ModuleSettings["tieude3"].ToString();
                if (ModuleSettings["baiviet3"] != null && ModuleSettings["baiviet3"].ToString() != "")
                {
                    string baiviet3 = ModuleSettings["baiviet3"].ToString();
                    baiviet3 = baiviet3.Substring(1, baiviet3.Length - 2);
                    string[] a = baiviet3.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in a)
                    {
                        foreach (ListItem i in lstBaiViet3.Items)
                        {
                            if (s == i.Value)
                            {
                                i.Selected = true;
                                break;
                            }
                        }
                    }
                }
                
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            try
            {
                DotNetNuke.Entities.Modules.ModuleController objmodules = new DotNetNuke.Entities.Modules.ModuleController();
                objmodules.UpdateModuleSetting(ModuleId, "template1", ddlTemplate1.Text);
                objmodules.UpdateModuleSetting(ModuleId, "template2", ddlTemplate2.Text);
                objmodules.UpdateModuleSetting(ModuleId, "template3", ddlTemplate3.Text);

                objmodules.UpdateModuleSetting(ModuleId, "numshortnews1", txtNumShortNews1.Text);
                objmodules.UpdateModuleSetting(ModuleId, "numshortnews2", txtNumShortNews2.Text);
                objmodules.UpdateModuleSetting(ModuleId, "numshortnews3", txtNumShortNews3.Text);

                objmodules.UpdateModuleSetting(ModuleId, "imagewidth1", txtImageWidth1.Text);
                objmodules.UpdateModuleSetting(ModuleId, "imagewidth2", txtImageWidth2.Text);
                objmodules.UpdateModuleSetting(ModuleId, "imagewidth3", txtImageWidth3.Text);

                objmodules.UpdateModuleSetting(ModuleId, "tieude1", txtTieuDe1.Text);
                string sBaiViet1 = "";
                foreach (ListItem i in lstBaiViet1.Items)
                {
                    if (i.Selected)
                    {
                        sBaiViet1 += "@" + i.Value + "@";
                    }
                }
                objmodules.UpdateModuleSetting(ModuleId, "baiviet1", sBaiViet1);

                objmodules.UpdateModuleSetting(ModuleId, "tieude2", txtTieuDe2.Text);
                string sBaiViet2 = "";
                foreach (ListItem i in lstBaiViet2.Items)
                {
                    if (i.Selected)
                    {
                        sBaiViet2 += "@" + i.Value + "@";
                    }
                }
                objmodules.UpdateModuleSetting(ModuleId, "baiviet2", sBaiViet2);

                objmodules.UpdateModuleSetting(ModuleId, "tieude3", txtTieuDe3.Text);
                string sBaiViet3 = "";
                foreach (ListItem i in lstBaiViet3.Items)
                {
                    if (i.Selected)
                    {
                        sBaiViet3 += "@" + i.Value + "@";
                    }
                }
                objmodules.UpdateModuleSetting(ModuleId, "baiviet3", sBaiViet3);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}