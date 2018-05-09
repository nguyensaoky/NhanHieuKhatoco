using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Modules;
using DotNetNuke.Services.Localization;
using System;
using DotNetNuke.Services.Exceptions;

namespace DotNetNuke.HTML 
{ 
    public partial class EditHtml : DotNetNuke.Entities.Modules.PortalModuleBase 
    { 
        #region "Private Members" 
        
        protected bool _isNew = true; 
        
        #endregion 
        
        #region "Event Handlers" 
        
        private void Page_Load(object sender, System.EventArgs e) 
        { 
            try 
            { 
                if (Page.IsPostBack == false) 
                { 
                    ddlLocale.DataSource = Localization.GetEnabledLocales().AllValues; 
                    ddlLocale.DataTextField = "Text"; 
                    ddlLocale.DataValueField = "Code"; 
                    ddlLocale.DataBind();
                    ddlLocale.SelectedValue = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                    
                    effority.Ealo.StringInfo objText = null; 
                    effority.Ealo.StringInfo objSummary = null; 
                    
                    objText = effority.Ealo.Controller.GetStringByQualifierAndStringName(Consts.DesktopHTMLQualifier, ModuleId.ToString(), System.Threading.Thread.CurrentThread.CurrentCulture.Name, true); 
                    objSummary = effority.Ealo.Controller.GetStringByQualifierAndStringName(Consts.DesktopSummaryQualifier, ModuleId.ToString(), System.Threading.Thread.CurrentThread.CurrentCulture.Name, true); 
                    
                    if ((objText != null)) 
                    { 
                        teContent.Text = objText.StringText; 
                    } 
                    else 
                    { 
                        teContent.Text = "";
                    } 
                    
                    if ((objSummary != null)) 
                    { 
                        txtDesktopSummary.Text = Server.HtmlDecode(objSummary.StringText); 
                    } 
                    else 
                    { 
                        txtDesktopSummary.Text = "";
                    } 
                    
                    ViewState["HTML.Locale"] = ddlLocale.SelectedValue; 
                } 
            } 
            catch (Exception exc) 
            { 
                Exceptions.ProcessModuleLoadException(this, exc); 
            } 
        } 
        
        protected void cmdCancel_Click(object sender, EventArgs e) 
        { 
            try 
            { 
                Response.Redirect(Globals.NavigateURL(), true); 
            } 
            catch (Exception exc) 
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            } 
        } 
        
        protected void cmdPreview_Click(object sender, System.EventArgs e) 
        { 
            try 
            { 
                lblPreview.Text = Server.HtmlDecode(teContent.Text);
            } 
            catch (Exception exc) 
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            } 
        } 
        
        protected void cmdUpdate_Click(object sender, EventArgs e) 
        { 
            try 
            { 
                effority.Ealo.Controller.UpdateStringByQualiferAndStringName(Consts.DesktopHTMLQualifier, ModuleId.ToString(), teContent.Text, ddlLocale.SelectedValue); 
                effority.Ealo.Controller.UpdateStringByQualiferAndStringName(Consts.DesktopSummaryQualifier, ModuleId.ToString(), txtDesktopSummary.Text, ddlLocale.SelectedValue); 
                effority.Ealo.Utils.ClearCache(); 
                SynchronizeModule(); 
            } 
            catch (Exception exc) 
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            } 
        } 
        
        protected void ddlLocale_SelectedIndexChanged(object sender, System.EventArgs e) 
        { 
            try 
            {
                ViewState["HTML.Locale"] = ddlLocale.SelectedValue; 
                effority.Ealo.StringInfo objText = null; 
                effority.Ealo.StringInfo objSummary = null; 
                objText = effority.Ealo.Controller.GetStringByQualifierAndStringName(Consts.DesktopHTMLQualifier, ModuleId.ToString(), ddlLocale.SelectedValue, false); 
                objSummary = effority.Ealo.Controller.GetStringByQualifierAndStringName(Consts.DesktopSummaryQualifier, ModuleId.ToString(), ddlLocale.SelectedValue, false); 
                if ((objText != null)) 
                { 
                    teContent.Text = objText.StringText; 
                } 
                else 
                { 
                    teContent.Text = ""; 
                } 
                if ((objSummary != null)) 
                { 
                    txtDesktopSummary.Text = Server.HtmlDecode(objSummary.StringText); 
                } 
                else 
                { 
                    txtDesktopSummary.Text = ""; 
                } 
                SynchronizeModule(); 
            } 
            catch (Exception exc) 
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            } 
        } 
        
        protected void cmdUpdateAndClose_Click(object sender, System.EventArgs e) 
        { 
            try 
            { 
                effority.Ealo.Controller.UpdateStringByQualiferAndStringName(Consts.DesktopHTMLQualifier, ModuleId.ToString(), teContent.Text, ddlLocale.SelectedValue); 
                effority.Ealo.Controller.UpdateStringByQualiferAndStringName(Consts.DesktopSummaryQualifier, ModuleId.ToString(), txtDesktopSummary.Text, ddlLocale.SelectedValue); 
                effority.Ealo.Utils.ClearCache(); 
                // refresh cache 
                SynchronizeModule();
                // redirect back to portal 
                Response.Redirect(Globals.NavigateURL(), true); 
            } 
            catch (Exception exc) 
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            } 
        }
    } 
    #endregion 
} 