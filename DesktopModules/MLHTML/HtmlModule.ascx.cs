using DotNetNuke; 
using System.Web.UI; 
using System.Text.RegularExpressions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System.Web.UI.WebControls;
using System.Collections;
using Microsoft.VisualBasic;
using System;

namespace DotNetNuke.HTML
{ 
    public partial class HtmlModule : DotNetNuke.Entities.Modules.PortalModuleBase, Entities.Modules.IActionable 
    { 
        #region " Private Members " 
        
        private Hashtable tagCache; 
        
        #endregion 
        
        #region " Event Handlers " 
        
        private void Page_Load(object sender, System.EventArgs e) 
        { 
            try 
            { 
                effority.Ealo.StringInfo objText = null; 
                objText = effority.Ealo.Controller.GetStringByQualifierAndStringName(Consts.DesktopHTMLQualifier, ModuleId.ToString(), System.Threading.Thread.CurrentThread.CurrentCulture.Name, true); 
                string strContent = "";
                if ((objText != null)) 
                { 
                    strContent = Server.HtmlDecode((string)objText.StringText); 
                } 
                ltContent.Text = strContent;
                UI.Skins.Skin ParentSkin = UI.Skins.Skin.GetParentSkin(this); 
                if ((ParentSkin != null)) 
                { 
                    ParentSkin.RegisterModuleActionEvent(this.ModuleId, ModuleAction_Click); 
                }
                if (strContent == "")
                {
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.ToString() != Common.Globals.GetPortalSettings().DefaultLanguage) ContainerControl.Visible = false;
                }
            }
            catch (Exception exc) 
            { 
                Exceptions.ProcessModuleLoadException(this, exc);
            } 
        } 
        
        public void ModuleAction_Click(object sender, Entities.Modules.Actions.ActionEventArgs e) 
        { 
            if (e.Action.Url.Length > 0) 
            { 
                Response.Redirect(e.Action.Url, true); 
            } 
        } 
        
        #endregion 
        
        #region " Optional Interfaces " 
        
        public Entities.Modules.Actions.ModuleActionCollection ModuleActions 
        { 
            get 
            { 
                Entities.Modules.Actions.ModuleActionCollection Actions = new Entities.Modules.Actions.ModuleActionCollection(); 
                Actions.Add(GetNextActionID(), Localization.GetString(Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl(), false, Security.SecurityAccessLevel.Edit, true, false ); 
                return Actions; 
            } 
        } 

        #endregion
    } 
} 