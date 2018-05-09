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
	public partial class MLCat : PortalModuleBase
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
					this.BindCats();
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
				this.BindCats();
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
				this.BindCats();
				lblTranslations.Text = string.Format(Localization.GetString("lblTranslations", this.LocalResourceFile), ddlLocale.SelectedValue);
			}
			catch (Exception ex)
			{
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
			}
		}
		
		protected void repCats_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				MLCategoryInfo catInfo = (MLCategoryInfo) e.Item.DataItem;
                Label lblCatName = (Label)e.Item.FindControl("lblCatName");
                TextBox txtCatName = (TextBox)e.Item.FindControl("txtCatName");
                HiddenField catId = (HiddenField)e.Item.FindControl("CatId");
				
                if (lblCatName != null)
				{
                    lblCatName.Text = catInfo.CatName;
				}
				
				bool CatNameTranslated = false;
                if (catInfo.MLCatName != null)
				{
                    CatNameTranslated = !catInfo.MLCatName.StringTextIsNull;
				}
				else
				{
                    CatNameTranslated = false;
				}

                if (!CatNameTranslated)
				{
                    txtCatName.Style.Add("border-color", "Red");
                    txtCatName.ToolTip = Localization.GetString("notTranslated", this.LocalResourceFile);
				}

                if (txtCatName != null && catInfo.MLCatName != null)
				{
                    txtCatName.Text = catInfo.MLCatName.StringText;
				}
				
				if (catId != null)
				{
					catId.Value = catInfo.CatID;
				}
			}
		}
		
		#endregion
		
		#region " Private Methods "
		
		private void BindCats()
		{
            List<MLCategoryInfo> liste = MLCategoryController.GetCategoriesAsListe(PortalId, ddlLocale.SelectedValue, false);
			repCats.DataSource = liste;
			repCats.DataBind();
		}
		
		private void Save()
		{
			string lastLocale = (string)(ViewState["ViewStateLastLanguageCode"]);
			if (! string.IsNullOrEmpty(lastLocale))
			{
				foreach (RepeaterItem item in repCats.Items)
				{
					TextBox txtCatName = (TextBox)item.FindControl("txtCatName");
                    HiddenField CatId = (HiddenField)item.FindControl("CatId");
					string catName = "";
                    string catID = "";
					if (txtCatName != null)
					{
						catName = txtCatName.Text;
					}
					if (CatId != null)
					{
						catID = CatId.Value;
					}
					MLCategoryController.UpdateCat(catID, catName, lastLocale);
				}
			}
		}
		
		#endregion
		
	}
}
