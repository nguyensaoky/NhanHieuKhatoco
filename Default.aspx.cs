using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Log.SiteLog;
using DotNetNuke.Services.Vendors;
using DotNetNuke.UI.Skins;
using DotNetNuke.UI.Utilities;
using DataCache=DotNetNuke.Common.Utilities.DataCache;
using Globals=DotNetNuke.Common.Globals;
using System.Collections.Generic;
using System.Web.Services;

namespace DotNetNuke.Framework
{
    public partial class DefaultPage : CDefault
    {
        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }

        public int PageScrollTop
        {
            get
            {
                int result;
                if( ScrollTop.Value.Length > 0 && Int32.TryParse( ScrollTop.Value, out result ) )
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ScrollTop.Value = value.ToString();
            }
        }

        private void InitializePage()
        {
            TabController objTabs = new TabController();
            TabInfo objTab;

            // redirect to a specific tab based on name
            if( Request.QueryString["tabname"] != "" )
            {
                string strURL = "";

                objTab = objTabs.GetTabByName( Request.QueryString["TabName"], ( (PortalSettings)HttpContext.Current.Items["PortalSettings"] ).PortalId );
                if( objTab != null )
                {
                    int actualParamCount = 0;
                    string[] elements = new string[Request.QueryString.Count - 1 + 1]; //maximum number of elements
                    for( int intParam = 0; intParam <= Request.QueryString.Count - 1; intParam++ )
                    {
                        switch( Request.QueryString.Keys[intParam].ToLower() )
                        {
                            case "tabid":
                                break;

                            case "tabname":

                                break;
                            default:

                                elements[actualParamCount] = Request.QueryString.Keys[intParam] + "=" + Request.QueryString[intParam];
                                actualParamCount++;
                                break;
                        }
                    }
                    string[] copiedTo = new string[actualParamCount - 1 + 1];
                    elements.CopyTo(copiedTo, 0); //redim to remove blank elements
                    elements = copiedTo;
                    Response.Redirect( Globals.NavigateURL( objTab.TabID, Null.NullString, elements ), true );
                }
            }

            if( Request.IsAuthenticated )
            {
                // set client side page caching for authenticated users
                if( Convert.ToString( PortalSettings.HostSettings["AuthenticatedCacheability"] ) != "" )
                {
                    switch( Convert.ToString( PortalSettings.HostSettings["AuthenticatedCacheability"] ) )
                    {
                        case "0":

                            Response.Cache.SetCacheability( HttpCacheability.NoCache );
                            break;
                        case "1":

                            Response.Cache.SetCacheability( HttpCacheability.Private );
                            break;
                        case "2":

                            Response.Cache.SetCacheability( HttpCacheability.Public );
                            break;
                        case "3":

                            Response.Cache.SetCacheability( HttpCacheability.Server );
                            break;
                        case "4":

                            Response.Cache.SetCacheability( HttpCacheability.ServerAndNoCache );
                            break;
                        case "5":

                            Response.Cache.SetCacheability( HttpCacheability.ServerAndPrivate );
                            break;
                    }
                }
                else
                {
                    Response.Cache.SetCacheability( HttpCacheability.ServerAndNoCache );
                }
            }

            // set page title
            string strTitle = PortalSettings.PortalName;
            foreach( TabInfo tempLoopVar_objTab in PortalSettings.ActiveTab.BreadCrumbs )
            {
                objTab = tempLoopVar_objTab;
                effority.Ealo.Specialized.TabInfo fMLTabInfo = effority.Ealo.Specialized.Tabs.GetTab(objTab.TabID, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), false);
                if (fMLTabInfo != null)
                {
                    if (fMLTabInfo.EaloTitle != null)
                    {
                        strTitle += " > " + fMLTabInfo.EaloTitle.StringTextOrFallBack;
                    }
                    else
                    {
                        strTitle += " > " + fMLTabInfo.Title;
                    }
                }
                else
                {
                    strTitle += " > " + objTab.Title;
                }
            }
            this.Title = strTitle;

            // META description
            if( !String.IsNullOrEmpty(PortalSettings.ActiveTab.Description) )
            {
                Description = PortalSettings.ActiveTab.Description;
            }
            else
            {
                Description = PortalSettings.Description;
            }

            // META keywords
            if( !String.IsNullOrEmpty(PortalSettings.ActiveTab.KeyWords) )
            {
                KeyWords = PortalSettings.ActiveTab.KeyWords;
            }
            else
            {
                KeyWords = PortalSettings.KeyWords;
            }
                       
            Page.ClientScript.RegisterClientScriptInclude("dnncore", ResolveUrl( "~/js/dnncore.js" ));
        }

        private UserControl LoadSkin( string SkinPath )
        {
            UserControl ctlSkin = null;

            try
            {
                if( SkinPath.ToLower().IndexOf( Globals.ApplicationPath.ToLower() ) != - 1 )
                {
                    SkinPath = SkinPath.Remove( 0, Globals.ApplicationPath.Length );
                }
                ctlSkin = (UserControl)LoadControl( "~" + SkinPath );
                // call databind so that any server logic in the skin is executed
                ctlSkin.DataBind();
            }
            catch( Exception exc )
            {
                if( PortalSecurity.IsInRoles( PortalSettings.AdministratorRoleName ) || PortalSecurity.IsInRoles( PortalSettings.ActiveTab.AdministratorRoles.ToString() ) )
                {
                    // only display the error to administrators
                    SkinError.Text += "<div style=\"text-align:center\">Could Not Load Skin: " + SkinPath + " Error: " + Server.HtmlEncode(exc.Message) + "</div><br>";                    
                    SkinError.Visible = true;
                }
            }
            return ctlSkin;
        }

        private void ManageStyleSheets( bool PortalCSS )
        {
            // initialize reference paths to load the cascading style sheets
            string id;

            Hashtable objCSSCache = (Hashtable)DataCache.GetCache( "CSS" );
            if( objCSSCache == null )
            {
                objCSSCache = new Hashtable();
            }

            if( PortalCSS == false )
            {
                // default style sheet ( required )
                id = Globals.CreateValidID( Globals.HostPath );
                AddStyleSheet( id, Globals.HostPath + "default.css" );

                // skin package style sheet
                id = Globals.CreateValidID( PortalSettings.ActiveTab.SkinPath );
                if( objCSSCache.ContainsKey( id ) == false )
                {
                    if( File.Exists( Server.MapPath( PortalSettings.ActiveTab.SkinPath ) + "skin.css" ) )
                    {
                        objCSSCache[id] = PortalSettings.ActiveTab.SkinPath + "skin.css";
                    }
                    else
                    {
                        objCSSCache[id] = "";
                    }
                    if( Globals.PerformanceSetting != Globals.PerformanceSettings.NoCaching )
                    {
                        DataCache.SetCache( "CSS", objCSSCache );
                    }
                }
                if( objCSSCache[id].ToString() != "" )
                {
                    AddStyleSheet( id, objCSSCache[id].ToString() );
                }

                // skin file style sheet
                id = Globals.CreateValidID( PortalSettings.ActiveTab.SkinSrc.Replace(".ascx", ".css") );
                if( objCSSCache.ContainsKey( id ) == false )
                {
                    if( File.Exists( Server.MapPath( PortalSettings.ActiveTab.SkinSrc.Replace(".ascx", ".css")) ))
                    {
                        objCSSCache[id] = PortalSettings.ActiveTab.SkinSrc.Replace(".ascx", ".css" );
                    }
                    else
                    {
                        objCSSCache[id] = "";
                    }
                    if( Globals.PerformanceSetting != Globals.PerformanceSettings.NoCaching )
                    {
                        DataCache.SetCache( "CSS", objCSSCache );
                    }
                }
                if( objCSSCache[id].ToString() != "" )
                {
                    AddStyleSheet( id, objCSSCache[id].ToString() );
                }
            }
            else
            {
                // portal style sheet
                id = Globals.CreateValidID( PortalSettings.HomeDirectory );
                AddStyleSheet( id, PortalSettings.HomeDirectory + "portal.css" );
            }
        }

        protected void Page_Init( Object sender, EventArgs e )
        {
            InitializePage();

            UserControl ctlSkin = null;

            if( Request.QueryString["SkinSrc"] != null )
            {
                PortalSettings.ActiveTab.SkinSrc = SkinController.FormatSkinSrc( Globals.QueryStringDecode( Request.QueryString["SkinSrc"] ) + ".ascx", PortalSettings );
                ctlSkin = LoadSkin( PortalSettings.ActiveTab.SkinSrc );
            }

            // load user skin ( based on cookie )
            if( ctlSkin == null )
            {
                if( Request.Cookies["_SkinSrc" + PortalSettings.PortalId] != null )
                {
                    if( !String.IsNullOrEmpty( Request.Cookies["_SkinSrc" + PortalSettings.PortalId].Value ))
                    {
                        PortalSettings.ActiveTab.SkinSrc = SkinController.FormatSkinSrc( Request.Cookies["_SkinSrc" + PortalSettings.PortalId].Value + ".ascx", PortalSettings );
                        ctlSkin = LoadSkin( PortalSettings.ActiveTab.SkinSrc );
                    }
                }
            }

            // load assigned skin
            if( ctlSkin == null )
            {
                if( Globals.IsAdminSkin( PortalSettings.ActiveTab.IsAdminTab ) )
                {
                    SkinInfo objSkin = SkinController.GetSkin( SkinInfo.RootSkin, PortalSettings.PortalId, SkinType.Admin );
                    if( objSkin != null )
                    {
                        PortalSettings.ActiveTab.SkinSrc = objSkin.SkinSrc;
                    }
                    else
                    {
                        PortalSettings.ActiveTab.SkinSrc = "";
                    }
                }

                if( !String.IsNullOrEmpty(PortalSettings.ActiveTab.SkinSrc) )
                {
                    PortalSettings.ActiveTab.SkinSrc = SkinController.FormatSkinSrc( PortalSettings.ActiveTab.SkinSrc, PortalSettings );
                    ctlSkin = LoadSkin( PortalSettings.ActiveTab.SkinSrc );
                }
            }

            // error loading skin - load default
            if( ctlSkin == null )
            {
                // could not load skin control - load default skin
                if( Globals.IsAdminSkin( PortalSettings.ActiveTab.IsAdminTab ) )
                {
                    PortalSettings.ActiveTab.SkinSrc = Globals.HostPath + SkinInfo.RootSkin + Globals.glbDefaultSkinFolder + Globals.glbDefaultAdminSkin;
                }
                else
                {
                    PortalSettings.ActiveTab.SkinSrc = Globals.HostPath + SkinInfo.RootSkin + Globals.glbDefaultSkinFolder + Globals.glbDefaultSkin;
                }
                ctlSkin = LoadSkin( PortalSettings.ActiveTab.SkinSrc );
            }

            // set skin path
            PortalSettings.ActiveTab.SkinPath = SkinController.FormatSkinPath( PortalSettings.ActiveTab.SkinSrc );

            // set skin id to an explicit short name to reduce page payload and make it standards compliant
            ctlSkin.ID = "dnn";

            // add CSS links
            ManageStyleSheets( false );

            // add skin to page
            SkinPlaceHolder.Controls.Add( ctlSkin );

            // add CSS links
            ManageStyleSheets( true );

            // ClientCallback Logic
            ClientAPI.HandleClientAPICallbackEvent( this );
        }

        protected void Page_Load( Object sender, EventArgs e )
        {
            HtmlInputHidden scrolltop = (HtmlInputHidden)Page.FindControl( "ScrollTop" );
            if( !String.IsNullOrEmpty(scrolltop.Value) )
            {
                DNNClientAPI.AddBodyOnloadEventHandler( Page, "__dnn_setScrollTop();" );
                scrolltop.Value = scrolltop.Value;
            }
        }

        protected void Page_PreRender( object sender, EventArgs e )
        {
            //Set the Head tags
            Page.Header.Title = Title;
            HtmlLink lnk = new HtmlLink();
            lnk.Attributes["rel"] = "SHORTCUT ICON";
            lnk.Attributes["href"] = PortalSettings.HomeDirectory + "favicon.ico";
            Page.Header.Controls.Add(lnk);
            MetaKeywords.Content = KeyWords;
            MetaDescription.Content = Description;
        }
    }
}