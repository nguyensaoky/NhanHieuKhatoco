#region DotNetNuke License
// DotNetNukeŽ - http://www.dotnetnuke.com
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
using DotNetNuke.Common;
using DotNetNuke.Modules.NavigationProvider;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.UI.WebControls;

namespace DotNetNuke.UI.Skins.Controls
{
    public partial class Nav : NavObjectBase
    {
        protected void Page_Load( Object sender, EventArgs e )
        {
            try
            {
                // This setting determines if the submenu arrows will be used
                bool blnIndicateChildren = bool.Parse( GetValue( this.IndicateChildren, "True" ) ); 
                // This setting determines if the submenu will be shown
                string strRightArrow;
                string strDownArrow;

                //image for right facing arrow
                if( !String.IsNullOrEmpty(IndicateChildImageSub))
                {
                    strRightArrow = IndicateChildImageSub;
                }
                else
                {
                    //strRightArrow = "[APPIMAGEPATH]breadcrumb.gif";                    
                    strRightArrow = "breadcrumb.gif"; //removed APPIMAGEPATH token - http://www.dotnetnuke.com/Community/ForumsDotNetNuke/tabid/795/forumid/76/threadid/85554/scope/posts/Default.aspx

                }
                //image for down facing arrow
                if( !String.IsNullOrEmpty(IndicateChildImageRoot))
                {
                    strDownArrow = IndicateChildImageRoot;
                }
                else
                {
                    //strDownArrow = "[APPIMAGEPATH]menu_down.gif";
                    strDownArrow = "menu_down.gif";	//removed APPIMAGEPATH token - http://www.dotnetnuke.com/Community/ForumsDotNetNuke/tabid/795/forumid/76/threadid/85554/scope/posts/Default.aspx
                }

                //Set correct image path for all separator images
                if( !String.IsNullOrEmpty(SeparatorHTML))
                {
                    SeparatorHTML = FixImagePath( SeparatorHTML );
                }

                if( !String.IsNullOrEmpty(SeparatorLeftHTML))
                {
                    SeparatorLeftHTML = FixImagePath( SeparatorLeftHTML );
                }
                if( !String.IsNullOrEmpty(SeparatorRightHTML))
                {
                    SeparatorRightHTML = FixImagePath( SeparatorRightHTML );
                }
                if( !String.IsNullOrEmpty(SeparatorLeftHTMLBreadCrumb))
                {
                    SeparatorLeftHTMLBreadCrumb = FixImagePath( SeparatorLeftHTMLBreadCrumb );
                }
                if( !String.IsNullOrEmpty(SeparatorRightHTMLBreadCrumb))
                {
                    SeparatorRightHTMLBreadCrumb = FixImagePath( SeparatorRightHTMLBreadCrumb );
                }
                if( !String.IsNullOrEmpty(SeparatorLeftHTMLActive))
                {
                    SeparatorLeftHTMLActive = FixImagePath( SeparatorLeftHTMLActive );
                }
                if( !String.IsNullOrEmpty(SeparatorRightHTMLActive))
                {
                    SeparatorRightHTMLActive = FixImagePath( SeparatorRightHTMLActive );
                }

                if( !String.IsNullOrEmpty(NodeLeftHTMLBreadCrumbRoot))
                {
                    NodeLeftHTMLBreadCrumbRoot = FixImagePath( NodeLeftHTMLBreadCrumbRoot );
                }
                if( !String.IsNullOrEmpty(NodeRightHTMLBreadCrumbRoot))
                {
                    NodeRightHTMLBreadCrumbRoot = FixImagePath( NodeRightHTMLBreadCrumbRoot );
                }
                if( !String.IsNullOrEmpty(NodeLeftHTMLBreadCrumbSub))
                {
                    NodeLeftHTMLBreadCrumbSub = FixImagePath( NodeLeftHTMLBreadCrumbSub );
                }
                if( !String.IsNullOrEmpty(NodeRightHTMLBreadCrumbSub))
                {
                    NodeRightHTMLBreadCrumbSub = FixImagePath( NodeRightHTMLBreadCrumbSub );
                }
                if( !String.IsNullOrEmpty(NodeLeftHTMLRoot))
                {
                    NodeLeftHTMLRoot = FixImagePath( NodeLeftHTMLRoot );
                }
                if( !String.IsNullOrEmpty(NodeRightHTMLRoot))
                {
                    NodeRightHTMLRoot = FixImagePath( NodeRightHTMLRoot );
                }
                if( !String.IsNullOrEmpty(NodeLeftHTMLSub))
                {
                    NodeLeftHTMLSub = FixImagePath( NodeLeftHTMLSub );
                }
                if( !String.IsNullOrEmpty(NodeRightHTMLSub))
                {
                    NodeRightHTMLSub = FixImagePath( NodeRightHTMLSub );
                }

                if( String.IsNullOrEmpty(PathImage) )
                {
                    PathImage = PortalSettings.HomeDirectory;
                }
                if( blnIndicateChildren )
                {
                    this.IndicateChildImageSub = strRightArrow;
                    //Me.IndicateChildren = True.ToString
                    if( this.ControlOrientation.ToLower() == "vertical" ) //NavigationProvider.NavigationProvider.Orientation.Vertical Then
                    {
                        this.IndicateChildImageRoot = strRightArrow;
                    }
                    else
                    {
                        this.IndicateChildImageRoot = strDownArrow;
                    }
                }
                else
                {
                    this.IndicateChildImageSub = "[APPIMAGEPATH]spacer.gif";
                }
                this.PathSystemScript = Globals.ApplicationPath + "/controls/SolpartMenu/";
                this.PathSystemImage = "[APPIMAGEPATH]";

                BuildNodes( null );
            }
            catch( Exception exc ) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException( this, exc );
            }
        }

        private static string FixImagePath( string strPath )
        {
            if( strPath.IndexOf( "src=" ) != - 1 )
            {
                return strPath.Replace( "src=\"", "src=\"[SKINPATH]" );
            }
            else
            {
                return strPath;
            }
        }

        private void BuildNodes( DNNNode objNode )
        {
            DNNNodeCollection objNodes;
            objNodes = GetNavigationNodes( objNode );
            this.Control.ClearNodes(); //since we always bind we need to clear the nodes for providers that maintain their state
            this.Bind( objNodes );
        }

        //private void SetAttributes()
        //{
        //    //SeparateCss = "true"; // CStr(True)
        //    //Me.StyleSelectionBorderColor = Nothing

        //    //if (bool.Parse(GetValue(ClearDefaults, "false")))
        //    //{
        //        //Me.StyleSelectionBorderColor = Nothing
        //        //Me.StyleSelectionForeColor = Nothing
        //        //Me.StyleHighlightColor = Nothing
        //        //Me.StyleIconBackColor = Nothing
        //        //Me.EffectsShadowColor = Nothing
        //        //Me.StyleSelectionColor = Nothing
        //        //Me.StyleBackColor = Nothing
        //        //Me.StyleForeColor = Nothing
        //    //}
        //    //else //these defaults used to be on the page HTML
        //    //{
        //        if (String.IsNullOrEmpty(MouseOutHideDelay))
        //        {
        //            this.MouseOutHideDelay = "500";
        //        }
        //        if (String.IsNullOrEmpty(MouseOverAction))
        //        {
        //            this.MouseOverAction = true.ToString(); //NavigationProvider.NavigationProvider.HoverAction.Expand
        //        }
        //        if (String.IsNullOrEmpty(StyleBorderWidth))
        //        {
        //            this.StyleBorderWidth = "0";
        //        }
        //        if (String.IsNullOrEmpty(StyleControlHeight))
        //        {
        //            this.StyleControlHeight = "16";
        //        }
        //        if (String.IsNullOrEmpty(StyleNodeHeight))
        //        {
        //            this.StyleNodeHeight = "21";
        //        }
        //        if (String.IsNullOrEmpty(StyleIconWidth))
        //        {
        //            this.StyleIconWidth = "0";
        //        }
        //        //Me.StyleSelectionBorderColor = "#333333" 'cleared above
        //        if (String.IsNullOrEmpty(StyleSelectionColor))
        //        {
        //            this.StyleSelectionColor = "#CCCCCC";
        //        }
        //        if (String.IsNullOrEmpty(StyleSelectionForeColor))
        //        {
        //            this.StyleSelectionForeColor = "White";
        //        }
        //        if (String.IsNullOrEmpty(StyleHighlightColor))
        //        {
        //            this.StyleHighlightColor = "#FF8080";
        //        }
        //        if (String.IsNullOrEmpty(StyleIconBackColor))
        //        {
        //            this.StyleIconBackColor = "#333333";
        //        }
        //        if (String.IsNullOrEmpty(EffectsShadowColor))
        //        {
        //            this.EffectsShadowColor = "#404040";
        //        }
        //        if (String.IsNullOrEmpty(MouseOverDisplay))
        //        {
        //            this.MouseOverDisplay = "highlight"; //NavigationProvider.NavigationProvider.HoverDisplay.Highlight
        //        }
        //        if (String.IsNullOrEmpty(EffectsStyle))
        //        {
        //            this.EffectsStyle = "filter:progid:DXImageTransform.Microsoft.Shadow(color='DimGray', Direction=135, Strength=3);";
        //        }
        //        if (String.IsNullOrEmpty(StyleFontSize))
        //        {
        //            this.StyleFontSize = "9";
        //        }
        //        if (String.IsNullOrEmpty(StyleFontBold))
        //        {
        //            this.StyleFontBold = "True";
        //        }
        //        if (String.IsNullOrEmpty(StyleFontNames))
        //        {
        //            this.StyleFontNames = "Tahoma,Arial,Helvetica";
        //        }
        //        if (String.IsNullOrEmpty(StyleForeColor))
        //        {
        //            this.StyleForeColor = "White";
        //        }
        //        if (String.IsNullOrEmpty(StyleBackColor))
        //        {
        //            this.StyleBackColor = "#333333";
        //        }
        //        if (String.IsNullOrEmpty(PathSystemImage))
        //        {
        //            this.PathSystemImage = "/";
        //        }
        //    //}

        //    //if (SeparateCss != null && Boolean.Parse(SeparateCss))
        //    //{

        //    //    if (!String.IsNullOrEmpty(MenuBarCssClass))
        //    //    {
        //    //        this.CSSControl = MenuBarCssClass;
        //    //    }
        //    //    else
        //    //    {
        //            this.CSSControl = "MainMenu_MenuBar";
        //        //}


        //        //if (!String.IsNullOrEmpty(MenuContainerCssClass))
        //        //{
        //        //    this.CSSContainerRoot = MenuContainerCssClass;
        //        //}
        //        //else
        //        //{
        //            this.CSSContainerRoot = "MainMenu_MenuContainer";
        //        //}

        //        //if (!String.IsNullOrEmpty(MenuItemCssClass))
        //        //{
        //        //    this.CSSNode = MenuItemCssClass;
        //        //}
        //        //else
        //        //{
        //            this.CSSNode = "MainMenu_MenuItem";
        //        //}

        //        //if (!String.IsNullOrEmpty(MenuIconCssClass))
        //        //{
        //        //    this.CSSIcon = MenuIconCssClass;
        //        //}
        //        //else
        //        //{
        //            this.CSSIcon = "MainMenu_MenuIcon";
        //        //}


        //        //if (!String.IsNullOrEmpty(SubMenuCssClass))
        //        //{
        //        //    this.CSSContainerSub = SubMenuCssClass;
        //        //}
        //        //else
        //        //{
        //            this.CSSContainerSub = "MainMenu_SubMenu";
        //        //}


        //        //if (!String.IsNullOrEmpty(MenuBreakCssClass))
        //        //{
        //        //    this.CSSBreak = MenuBreakCssClass;
        //        //}
        //        //else
        //        //{
        //            this.CSSBreak = "MainMenu_MenuBreak";
        //        //}

        //        //if (!String.IsNullOrEmpty(MenuItemSelCssClass))
        //        //{
        //        //    this.CSSNodeHover = MenuItemSelCssClass;
        //        //}
        //        //else
        //        //{
        //            this.CSSNodeHover = "MainMenu_MenuItemSel";
        //        //}

        //        //if (!String.IsNullOrEmpty(MenuArrowCssClass))
        //        //{
        //        //    this.CSSIndicateChildSub = MenuArrowCssClass;
        //        //}
        //        //else
        //        //{
        //            this.CSSIndicateChildSub = "MainMenu_MenuArrow";
        //        //}

        //        //if (!String.IsNullOrEmpty(MenuRootArrowCssClass))
        //        //{
        //        //    this.CSSIndicateChildRoot = MenuRootArrowCssClass;
        //        //}
        //        //else
        //        //{
        //            this.CSSIndicateChildRoot = "MainMenu_RootMenuArrow";
        //        //}
        //    //}
        //}

        protected override void OnInit( EventArgs e )
        {
            //SetAttributes();
            InitializeNavControl( this, "SolpartMenuNavigationProvider" );
            Control.NodeClick += new NavigationProvider.NodeClickEventHandler(Control_NodeClick);
            Control.PopulateOnDemand += new NavigationProvider.PopulateOnDemandEventHandler(Control_PopulateOnDemand);

            base.OnInit( e );
        }

        protected void Control_NodeClick( NavigationEventArgs args )
        {
            if( args.Node == null )
            {
                args.Node = Navigation.GetNavigationNode( args.ID, Control.ID );
            }
            Response.Redirect( Globals.ApplicationURL( int.Parse( args.Node.Key ) ), true );
        }

        protected void Control_PopulateOnDemand( NavigationEventArgs args )
        {
            if( args.Node == null )
            {
                args.Node = Navigation.GetNavigationNode( args.ID, Control.ID );
            }
            BuildNodes( args.Node );
        }
    }
}