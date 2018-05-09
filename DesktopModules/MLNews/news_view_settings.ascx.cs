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
    public partial class news_view_settings : DotNetNuke.Entities.Modules.ModuleSettingsBase
    {
        private void LoadDropdown()
        {
            string path = PortalSettings.HomeDirectoryMapPath + "Xsl";
            DotNetNuke.NewsProvider.Utils.BindTemplateByName(ddlTemplate, path, "comment_list*.xsl");

            NewsController newsCont = new NewsController();
            Dictionary<int, NewsInfo> dicNews = newsCont.GetNewsByPortal(PortalId);
            ArrayList fLstNews = new ArrayList();
            NewsInfo info = new NewsInfo();
            info.ID = 0;
            info.Headline = "";
            fLstNews.Add(info);
            foreach (NewsInfo fNewsInfo in dicNews.Values)
            {
                fLstNews.Add(fNewsInfo);
            }
            ddlNews.DataSource = fLstNews;
            ddlNews.DataTextField = "Headline";
            ddlNews.DataValueField = "ID";
            ddlNews.DataBind();
        }

        public override void LoadSettings()
        {
            try
            {
                LoadDropdown();
                if (ModuleSettings["source"] != null) radSource.SelectedValue = ModuleSettings["source"].ToString();
                if (ModuleSettings["newsid"] != null) ddlNews.SelectedValue = ModuleSettings["newsid"].ToString();
                if (ModuleSettings["CommentListTemplate"] != null) ddlTemplate.SelectedValue = ModuleSettings["CommentListTemplate"].ToString();
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
                DotNetNuke.Entities.Modules.ModuleController objModules = new DotNetNuke.Entities.Modules.ModuleController();
                objModules.UpdateModuleSetting(ModuleId, "source", radSource.SelectedValue);
                objModules.UpdateModuleSetting(ModuleId, "newsid", ddlNews.SelectedValue);
                objModules.UpdateModuleSetting(ModuleId, "CommentListTemplate", ddlTemplate.SelectedValue);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}