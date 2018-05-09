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
using DotNetNuke.Framework;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.UI.Skins.Controls
{
    public partial class Language_New : SkinObjectBase
    {
        private string _cssClass;
        public string CssClass
        {
            get
            {
                if( _cssClass != null )
                {
                    return _cssClass;
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                _cssClass = value;
            }
        }

        private int red = 128;
        public int Red
        {
            get { return red; }
            set { red = value; }
        }

        private int green = 128;
        public int Green
        {
            get { return green; }
            set { green = value; }
        }

        private int blue = 128;
        public int Blue
        {
            get { return blue; }
            set { blue = value; }
        }
        
        protected void Page_Load( Object sender, EventArgs e )
        {
            if (!Page.IsPostBack)
            {
                cmdVi.CssClass = CssClass;
                cmdEn.CssClass = CssClass;
                if (((PageBase)Page).PageCulture.Name.ToLower() == "vi-vn")
                {
                    cmdVi.ForeColor = System.Drawing.Color.FromArgb(Red, Green, Blue);
                }
                else if (((PageBase)Page).PageCulture.Name.ToLower() == "en-us")
                {
                    cmdEn.ForeColor = System.Drawing.Color.FromArgb(Red, Green, Blue);
                }
            }
        }

        protected void cmdLanguage_Click(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.LinkButton lnkLanguage = (System.Web.UI.WebControls.LinkButton)sender;
            if (lnkLanguage.Text == "Tiếng Việt")
            {
                Localization.SetLanguage("Vi-Vn");
                //Response.Redirect(Request.RawUrl, true);
            }
            else if (lnkLanguage.Text == "English")
            {
                Localization.SetLanguage("En-Us");
                //Response.Redirect(Request.RawUrl, true);
            }
            Response.Redirect(PortalSettings.PortalAlias.HTTPAlias, true);
        }
    }
}