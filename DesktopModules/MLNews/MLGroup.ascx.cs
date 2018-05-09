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
	public partial class MLGroup : PortalModuleBase
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
					this.BindGroups();
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
				this.BindGroups();
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
				//Save();
				ViewState["ViewStateLastLanguageCode"] = ddlLocale.SelectedValue;
				this.BindGroups();
				lblTranslations.Text = string.Format(Localization.GetString("lblTranslations", this.LocalResourceFile), ddlLocale.SelectedValue);
			}
			catch (Exception ex)
			{
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
			}
		}
		
		protected void repGroups_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				MLNewsGroupInfo groupInfo = (MLNewsGroupInfo) e.Item.DataItem;
                Label lblGroupName = (Label)e.Item.FindControl("lblGroupName");
                TextBox txtGroupName = (TextBox)e.Item.FindControl("txtGroupName");
                HiddenField groupId = (HiddenField)e.Item.FindControl("GroupId");
				
                if (lblGroupName != null)
				{
                    lblGroupName.Text = groupInfo.NewsGroupName;
				}
				
				bool GroupNameTranslated = false;
                if (groupInfo.MLGroupName != null)
				{
                    GroupNameTranslated = !groupInfo.MLGroupName.StringTextIsNull;
				}
				else
				{
                    GroupNameTranslated = false;
				}

                if (!GroupNameTranslated)
				{
                    txtGroupName.Style.Add("border-color", "Red");
                    txtGroupName.ToolTip = Localization.GetString("notTranslated", this.LocalResourceFile);
				}

                if (txtGroupName != null && groupInfo.MLGroupName != null)
				{
                    txtGroupName.Text = groupInfo.MLGroupName.StringText;
				}
				
				if (groupId != null)
				{
					groupId.Value = groupInfo.NewsGroupID;
				}
			}
		}
		
		#endregion
		
		#region " Private Methods "
		
		private void BindGroups()
		{
            List<MLNewsGroupInfo> liste = MLNewsGroupController.GetNewsGroupsAsListe(PortalId, ddlLocale.SelectedValue, false);
			repGroups.DataSource = liste;
			repGroups.DataBind();
		}
		
		private void Save()
		{
			string lastLocale = (string)(ViewState["ViewStateLastLanguageCode"]);
			if (! string.IsNullOrEmpty(lastLocale))
			{
				foreach (RepeaterItem item in repGroups.Items)
				{
					TextBox txtGroupName = (TextBox)item.FindControl("txtGroupName");
                    HiddenField GroupId = (HiddenField)item.FindControl("GroupId");
					string groupName = "";
                    string groupID = "";
					if (txtGroupName != null)
					{
						groupName = txtGroupName.Text;
					}
					if (GroupId != null)
					{
						groupID = GroupId.Value;
					}
					MLNewsGroupController.UpdateGroup(groupID, groupName, lastLocale);
				}
			}
		}
		
		#endregion
		
	}
}
