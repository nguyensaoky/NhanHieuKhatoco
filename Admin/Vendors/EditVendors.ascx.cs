#region DotNetNuke License

// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2006
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Collections;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Mail;
using DotNetNuke.Services.Vendors;
using DotNetNuke.UI.Skins.Controls;
using DotNetNuke.UI.Utilities;
using Globals=DotNetNuke.Common.Globals;

namespace DotNetNuke.Modules.Admin.Vendors
{
    public partial class EditVendors : PortalModuleBase
    {
        public int VendorID = - 1;

        /// <summary>
        /// AddModuleMessage adds a module message
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="type">The type of message</param>
        /// <history>
        /// 	[cnurse]	08/24/2006
        /// </history>
        private void AddModuleMessage( string message, ModuleMessageType type )
        {
            UI.Skins.Skin.AddModuleMessage( this, Localization.GetString( message, LocalResourceFile ), type );
        }

        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// <history>
        /// 	[cnurse]	9/17/2004	Updated to reflect design changes for Help, 508 support
        ///                       and localisation
        /// </history>
        protected void Page_Load( Object sender, EventArgs e )
        {
            try
            {
                TabController objTabs = new TabController();
                
                bool blnBanner = false;
                bool blnSignup = false;

                if( ( Request.QueryString["VendorID"] != null ) )
                {
                    VendorID = int.Parse( Request.QueryString["VendorID"] );
                }

                if( Request.QueryString["ctl"] != null && VendorID == - 1 )
                {
                    blnSignup = true;
                }

                if( Request.QueryString["banner"] != null )
                {
                    blnBanner = true;
                }

                if( Page.IsPostBack == false )
                {
                    ClientAPI.AddButtonConfirm( cmdDelete, Localization.GetString( "DeleteItem" ) );

                    VendorController objVendors = new VendorController();
                    if( VendorID != - 1 )
                    {
                        VendorInfo objVendor;
                        if( PortalSettings.ActiveTab.ParentId == PortalSettings.SuperTabId && UserInfo.IsSuperUser )
                        {
                            //Get Host Vendor
                            objVendor = objVendors.GetVendor( VendorID, Null.NullInteger );
                        }
                        else
                        {
                            //Get Portal Vendor
                            objVendor = objVendors.GetVendor( VendorID, PortalId );
                        }
                        if( objVendor != null )
                        {
                            txtVendorName.Text = objVendor.VendorName;
                            chkAuthorized.Checked = objVendor.Authorized;
                        }

                        // use dispatch method to load modules
                        Banners objBanners;
                        objBanners = (Banners)this.LoadControl( "~" + this.TemplateSourceDirectory.Remove( 0, Globals.ApplicationPath.Length ) + "/Banners.ascx" );
                        objBanners.ID = "/Banners.ascx";
                        objBanners.VendorID = this.VendorID;
                        objBanners.ModuleConfiguration = ModuleConfiguration;
                        divBanners.Controls.Add( objBanners );
                    }
                    else
                    {
                        chkAuthorized.Checked = true;
                        cmdDelete.Visible = false;
                        pnlBanners.Visible = false;
                    }

                    if( blnSignup || blnBanner )
                    {
                        rowVendor2.Visible = false;
                        cmdDelete.Visible = false;

                        if( blnBanner )
                        {
                            cmdUpdate.Visible = false;
                        }
                        else
                        {
                            cmdUpdate.Text = "Signup";
                        }
                    }
                    else
                    {
                        TabInfo objTab;
                        if( PortalSettings.ActiveTab.ParentId == PortalSettings.SuperTabId )
                        {
                            objTab = objTabs.GetTabByName( "Vendors", Null.NullInteger );
                        }
                        else
                        {
                            objTab = objTabs.GetTabByName( "Vendors", PortalId );
                        }
                        if( objTab != null )
                        {
                            ViewState["filter"] = Request.QueryString["filter"];
                        }
                    }
                }
            }
            catch( Exception exc ) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException( this, exc );
            }
        }

        /// <summary>
        /// cmdCancel_Click runs when the Cancel button is clicked.
        /// </summary>
        /// <history>
        /// 	[cnurse]	9/17/2004	Updated to reflect design changes for Help, 508 support
        ///                       and localisation
        /// </history>
        protected void cmdCancel_Click( object sender, EventArgs e )
        {
            try
            {
                //Response.Redirect( Globals.NavigateURL( this.TabId, Null.NullString, "filter=" + Convert.ToString( ViewState["filter"] ) ), true );
                Response.Redirect(Globals.NavigateURL(this.TabId, Null.NullString, "filter=All"), true);
            }
            catch( Exception exc ) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException( this, exc );
            }
        }

        /// <summary>
        /// cmdDelete_Click runs when the Delete button is clicked.
        /// </summary>
        /// <history>
        /// 	[cnurse]	9/17/2004	Updated to reflect design changes for Help, 508 support
        ///                       and localisation
        /// </history>
        protected void cmdDelete_Click( object sender, EventArgs e )
        {
            try
            {
                if( VendorID != - 1 )
                {
                    VendorController objVendors = new VendorController();
                    objVendors.DeleteVendor( VendorID );
                }
                Response.Redirect( Globals.NavigateURL() );
            }
            catch( Exception exc ) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException( this, exc );
            }
        }

        /// <summary>
        /// cmdUpdate_Click runs when the Update button is clicked.
        /// </summary>
        /// <history>
        /// 	[cnurse]	9/17/2004	Updated to reflect design changes for Help, 508 support
        ///                       and localisation
        /// </history>
        protected void cmdUpdate_Click( object sender, EventArgs e )
        {
            try
            {
                if( Page.IsValid )
                {
                    int intPortalID;
                    if( PortalSettings.ActiveTab.ParentId == PortalSettings.SuperTabId )
                    {
                        intPortalID = - 1;
                    }
                    else
                    {
                        intPortalID = PortalId;
                    }

                    VendorController objVendors = new VendorController();
                    VendorInfo objVendor = new VendorInfo();

                    objVendor.PortalId = intPortalID;
                    objVendor.VendorId = VendorID;
                    objVendor.VendorName = txtVendorName.Text;
                    objVendor.UserName = UserInfo.UserID.ToString();
                    objVendor.Authorized = chkAuthorized.Checked;

                    if( VendorID == - 1 )
                    {                        
                        try
                        {
                            VendorID = objVendors.AddVendor(objVendor);
                        }
                        catch
                        {
                            AddModuleMessage("ErrorAddVendor", ModuleMessageType.RedError);
                            return;
                        }
                    }
                    else
                    {
                        VendorInfo objVendorCheck = objVendors.GetVendor( VendorID, intPortalID );
                        if( objVendorCheck != null )
                        {
                            objVendors.UpdateVendor( objVendor );
                        }
                        else
                        {
                            Response.Redirect( Globals.NavigateURL() );
                        }
                    }

                    if( cmdUpdate.Text == "Signup" )
                    {
                        ArrayList Custom = new ArrayList();
                        Custom.Add( DateTime.Now.ToString() );
                        Custom.Add( txtVendorName.Text );
                        string strMessage = "";
                        if( strMessage == "" )
                        {
                            Custom.Clear();
                        }
                        else
                        {
                            AddModuleMessage( "EmailErrorAdmin", ModuleMessageType.RedError );
                        }

                        if( strMessage == "" )
                        {
                            Response.Redirect( Globals.NavigateURL( this.TabId, Null.NullString, "filter=" + txtVendorName.Text.Substring( 0, 1 ) ), true );
                        }
                        else
                        {
                            AddModuleMessage( "EmailErrorVendor", ModuleMessageType.RedError );
                        }
                    }
                    else
                    {
                        Response.Redirect( Globals.NavigateURL( this.TabId, Null.NullString, "filter=All"), true );
                    }
                }
            }
            catch( Exception exc ) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException( this, exc );
            }
        }
    }
}