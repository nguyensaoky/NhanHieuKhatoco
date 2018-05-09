using System.Configuration;
using System.Collections;
using System.Data;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Diagnostics;
using System.Web.Security;
using System;
using System.Text;
using Microsoft.VisualBasic;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.Web.Profile;
using System.Collections.Generic;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Specialized;
using System.Web;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using effority.Ealo.Specialized;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Common;

namespace DotNetNuke.News
{
	public partial class MLNews : PortalModuleBase
	{
		#region " Public Properties "
		
		public string ViewStateLastLanguageCode
		{
			get
			{
				return "LL" + ModuleId;
			}
		}
		
		#endregion
		
		#region " Event Handlers "
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				if (! IsPostBack)
				{
					DotNetNuke.Services.Localization.Localization.LoadCultureDropDownList(ddlLocale, CultureDropDownTypes.EnglishName, System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
					ViewState["ViewStateLastLanguageCode"] = ddlLocale.SelectedValue;
                    this.BindCatDDL();
                    if (ddlCat.Items.Count > 0)
                    {
                        ddlCat.Items[0].Selected = true;
                        ddlCat_SelectedIndexChanged(null, null);
                    }
					lblTranslations.Text = string.Format(Localization.GetString("lblTranslations", this.LocalResourceFile), ddlLocale.SelectedValue);
				}
			}
			catch (Exception ex)
			{
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
			}
		}
		
		protected void btUpdate_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Save();
                this.BindNews();
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
		}
		
		protected void ddlLocale_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                ViewState["ViewStateLastLanguageCode"] = ddlLocale.SelectedValue;
                this.BindNews();
                lblTranslations.Text = string.Format(Localization.GetString("lblTranslations", this.LocalResourceFile), ddlLocale.SelectedValue);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
		}
		
		protected void BindNews()
		{
            if (ddlNews.SelectedValue == "" || ddlCat.SelectedValue == "")
            {
                NewsId.Value = "0";
                return;
            }
            MLNewsInfo mlNews = MLNewsController.GetNews(int.Parse(ddlNews.SelectedValue), ddlLocale.SelectedValue, false);
            lblHeadline.Text = mlNews.Headline;

            //Headline
            bool HeadlineTranslated = false;
            if (mlNews.MLHeadline != null)
            {
                HeadlineTranslated = !mlNews.MLHeadline.StringTextIsNull;
            }
            else
            {
                HeadlineTranslated = false;
            }

            if (!HeadlineTranslated)
            {
                txtHeadline.Style.Add("border-color", "Red");
                txtHeadline.ToolTip = Localization.GetString("notTranslated", this.LocalResourceFile);
                txtHeadline.Text = "";
            }

            if (mlNews.MLHeadline != null)
            {
                txtHeadline.Style.Remove("border-color");
                txtHeadline.ToolTip = "";
                txtHeadline.Text = mlNews.MLHeadline.StringText;
            }

            //Description
            bool DescriptionTranslated = false;
            if (mlNews.MLDescription != null)
            {
                DescriptionTranslated = !mlNews.MLDescription.StringTextIsNull;
            }
            else
            {
                DescriptionTranslated = false;
            }

            if (!DescriptionTranslated)
            {
                pnlDescription.Style.Add("border-color", "Red");
                txtDescription.Text = "";
            }

            if (mlNews.MLDescription != null)
            {
                pnlDescription.Style.Remove("border-color");
                txtDescription.Text = mlNews.MLDescription.StringText;
            }

            //Content
            bool ContentTranslated = false;
            if (mlNews.MLContent != null)
            {
                ContentTranslated = !mlNews.MLContent.StringTextIsNull;
            }
            else
            {
                ContentTranslated = false;
            }
            if (!ContentTranslated)
            {
                pnlContent.Style.Add("border-color", "Red");
                txtContent.Text = "";
            }
            if (mlNews.MLContent != null)
            {
                pnlContent.Style.Remove("border-color");
                txtContent.Text = mlNews.MLContent.StringText;
            }
            NewsId.Value = mlNews.ID.ToString();
		}

        protected void ddlCat_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            BindNewsDDL();
            ddlNews_SelectedIndexChanged(null, null);
        }

        protected void ddlNews_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            BindNews();
        }

		#endregion
		
		#region " Private Methods "

        private void BindCatDDL()
        {
            CategoryController catdb = new CategoryController();
            DataTable dt = catdb.LoadTree(false, PortalId, "");
            ddlCat.DataSource = dt;
            ddlCat.DataTextField = "CatName";
            ddlCat.DataValueField = "CatID";
            ddlCat.DataBind();
        }

        private void BindNewsDDL()
        {
            NewsController newsCont = new NewsController();
            Dictionary<int, NewsInfo> dicNews = newsCont.GetNewsByCat(ddlCat.SelectedValue);
            ArrayList fLstNews = new ArrayList();
            foreach (NewsInfo fNewsInfo in dicNews.Values)
            {
                fLstNews.Add(fNewsInfo);
            }
            ddlNews.DataSource = fLstNews;
            ddlNews.DataTextField = "Headline";
            ddlNews.DataValueField = "ID";
            ddlNews.DataBind();
        }
		
		private void Save()
		{
            string lastLocale = (string)(ViewState["ViewStateLastLanguageCode"]);
            if ((!string.IsNullOrEmpty(lastLocale)) && NewsId.Value != "0")
            {
                MLNewsController.UpdateNews(int.Parse(NewsId.Value), txtHeadline.Text, Server.HtmlDecode(txtDescription.Text), Server.HtmlDecode(txtContent.Text), lastLocale);
            }
		}
		#endregion
	}
}
