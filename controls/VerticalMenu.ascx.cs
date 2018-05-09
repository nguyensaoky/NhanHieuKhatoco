using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using System.Data;
using System.Collections;

namespace DotNetNuke.UI.UserControls
{
    public partial class VerticalMenu : UserControl
    {
        private string parentID;
        public string ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        private string menuWidth;
        public string MenuWidth
        {
            get { return menuWidth; }
            set { menuWidth = value; }
        }
	
        private string menuName;
        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }

        private string cssPrefix="";
        public string CSSPrefix
        {
            get { return cssPrefix; }
            set { cssPrefix = value; }
        }

        private int indent=3;
        public int Indent
        {
            get { return indent; }
            set { indent = value; }
        }

        private bool keepTrack = true;
        public bool KeepTrack
        {
            get { return keepTrack; }
            set { keepTrack = value; }
        }

        private bool expandAll = false;
        public bool ExpandAll
        {
            get { return expandAll; }
            set { expandAll = value; }
        }

        DataTable dataSource = null;
        public DataTable DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        private void ClearLevel(int level, int[] arrLevel)
        {
            for (int i = level; i < 20; i++)
            {
                arrLevel[i] = 0;
            }
        }

        private string CreateMenuName(int currLevel, string MenuName, int[] arrLevel)
        {
            string menu = MenuName;
            for (int j = 0; j < currLevel - 1; j++)
            {
                menu += "." + arrLevel[j].ToString();
            }
            arrLevel[currLevel - 1]++;
            ClearLevel(currLevel, arrLevel);
            menu += "." + arrLevel[currLevel - 1].ToString();
            return menu;
        }

        private string CreateMenuNameForQueryString(int currLevel, string MenuName, int[] arrLevel)
        {
            string menu = MenuName;
            if (currLevel != 0)
            {
                for (int j = 0; j < currLevel - 1; j++)
                {
                    menu += "." + arrLevel[j].ToString();
                }
                menu += "." + arrLevel[currLevel - 1].ToString();
            }
            return menu;
        }

        public void LoadMenu()
        {
			string alt = "Tải tất cả các dữ liệu bạn được phép xem thuộc dự án này";
            string plusImg = Common.Globals.ApplicationPath + "/images/plus.gif";
            string minusImg = Common.Globals.ApplicationPath + "/images/minus.gif";
            if (DataSource != null)
            {
                ArrayList lstToggle = new ArrayList();
                int i = 0;
                int nextLevel;
                int currLevel;
                DataRow nextRow;
                DataRow currRow;

                int[] arrLevel = new int[20];
                ClearLevel(0, arrLevel);

                ltScript.Text = @"<div class=""" + CSSPrefix + @"mC"">
                ";
                for (i = 0; i < DataSource.Rows.Count; i++)
                {
                    currRow = DataSource.Rows[i];
                    currLevel = int.Parse(currRow["Level"].ToString());
                    if (i + 1 < DataSource.Rows.Count)
                    {
                        nextRow = DataSource.Rows[i + 1];
                        nextLevel = int.Parse(nextRow["Level"].ToString());
                    }
                    else
                    {
                        nextLevel = 0;
                    }

                    string blank = "";
                    if (currLevel == nextLevel)
                    {
                        string menu = CreateMenuNameForQueryString(currLevel - 1, MenuName, arrLevel);
                        int duan_index = currRow["Link"].ToString().IndexOf("duan_catid");
                        string duan_catid = "";
                        if (duan_index > -1)
                        {
                            string restLink = currRow["Link"].ToString().Substring(duan_index + 11);
                            duan_catid = restLink.Substring(0, restLink.IndexOf("/"));
                        }
                        if (Request.QueryString["BCMenu"] == menu && Request.QueryString["ParentID"] == ParentID && Request.QueryString["duan_catid"] == duan_catid)
                        {
                            if (KeepTrack)
                            {
                                ltScript.Text += @"<a class=""" + CSSPrefix + @"mO" + ((int)(currLevel - 1)).ToString() + @"_select"" href=""" + currRow["Link"].ToString() + @"?BCMenu=" + menu + @"&ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>
                                ";
                            }
                            else
                            {
                                ltScript.Text += @"<a class=""" + CSSPrefix + @"mO" + ((int)(currLevel - 1)).ToString() + @"_select"" href=""" + currRow["Link"].ToString() + @"?ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>
                                ";
                            }
                        }
                        else
                        {
                            if (KeepTrack)
                            {
                                if (Convert.ToBoolean(currRow["Enable"]))
                                {
                                    ltScript.Text += @"<a class=""" + CSSPrefix + @"mO" + ((int)(currLevel - 1)).ToString() +
                                                    @""" href=""" + currRow["Link"].ToString() + @"?BCMenu=" + menu + @"&ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                    blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>
                                    ";
                                }
                                else
                                {
                                    ltScript.Text += @"<a class=""" + CSSPrefix + @"mO" + ((int)(currLevel - 1)).ToString() +
                                                    @"_disable"" href=""" + currRow["Link"].ToString() + @"?BCMenu=" + menu + @"&ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                    blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>
                                    ";
                                }
                            }
                            else
                            {
                                if (Convert.ToBoolean(currRow["Enable"]))
                                {
                                    ltScript.Text += @"<a class=""" + CSSPrefix + @"mO" + ((int)(currLevel - 1)).ToString() +
                                                    @""" href=""" + currRow["Link"].ToString() + @"?ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                    blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>
                                    ";
                                }
                                else
                                {
                                    ltScript.Text += @"<a class=""" + CSSPrefix + @"mO" + ((int)(currLevel - 1)).ToString() +
                                                    @"_disable"" href=""" + currRow["Link"].ToString() + @"?ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                    blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>
                                    ";
                                }
                            }
                        }
                    }
                    else if (currLevel < nextLevel)
                    {
                        string menu = CreateMenuName(currLevel, MenuName, arrLevel);
                        if (KeepTrack)
                        {
                            if (Convert.ToBoolean(currRow["Enable"]))
                            {
                                ltScript.Text += @"<div class=""" + CSSPrefix + @"mH" + ((int)(currLevel - 1)).ToString() +
                                                    @"_c""><img id='img_" + menu + @"' style='padding-left:0px;margin-left:0px;' src=""" + plusImg + @""" onclick=""toggleMenu('" +
                                                    menu + @"','img_" + menu + @"','" + plusImg + @"','" + minusImg + @"');""/>" + @"<a href=""" + currRow["Link"].ToString() + @"?BCMenu=c_" + menu + @"&ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                        blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>";
                                ltScript.Text += @"</div>
                                            <div id=""" + menu + @""" class=""" + CSSPrefix + @"mL"">
                                ";
                            }
                            else
                            {
                                ltScript.Text += @"<div class=""" + CSSPrefix + @"mH" + ((int)(currLevel - 1)).ToString() +
                                                    @"_c_disable""><img id='img_" + menu + @"' style='padding-left:0px;margin-left:0px;' src=""" + plusImg + @""" onclick=""toggleMenu('" +
                                                    menu + @"','img_" + menu + @"','" + plusImg + @"','" + minusImg + @"');""/>" + @"<a href=""" + currRow["Link"].ToString() + @"?BCMenu=c_" + menu + @"&ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                        blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>";
                                ltScript.Text += @"</div>
                                            <div id=""" + menu + @""" class=""" + CSSPrefix + @"mL"">
                                ";
                            }
                        }
                        else
                        {
                            if (Convert.ToBoolean(currRow["Enable"]))
                            {
                                ltScript.Text += @"<div class=""" + CSSPrefix + @"mH" + ((int)(currLevel - 1)).ToString() +
                                                @"_c""><img id='img_" + menu + @"' style='padding-left:0px;margin-left:0px;' src=""" + plusImg + @""" onclick=""toggleMenu('" +
                                                menu + @"','img_" + menu + @"','" + plusImg + @"','" + minusImg + @"');""/>" + @"<a href=""" + currRow["Link"].ToString() + @"?ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                    blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>";
                                ltScript.Text += @"</div>
                                        <div id=""" + menu + @""" class=""" + CSSPrefix + @"mL"">
                                ";
                            }
                            else
                            {
                                ltScript.Text += @"<div class=""" + CSSPrefix + @"mH" + ((int)(currLevel - 1)).ToString() +
                                                @"_c_disable""><img id='img_" + menu + @"' style='padding-left:0px;margin-left:0px;' src=""" + plusImg + @""" onclick=""toggleMenu('" +
                                                menu + @"','img_" + menu + @"','" + plusImg + @"','" + minusImg + @"');""/>" + @"<a href=""" + currRow["Link"].ToString() + @"?ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                    blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>";
                                ltScript.Text += @"</div>
                                        <div id=""" + menu + @""" class=""" + CSSPrefix + @"mL"">
                                ";
                            }
                        }
                        lstToggle.Add(menu);
                    }
                    else
                    {
                        string menu = CreateMenuNameForQueryString(currLevel - 1, MenuName, arrLevel);
                        //if (((string)(currRow["Link"].ToString() + @"?BCMenu=" + menu + @"&ParentID=" + ParentID)).Contains(Request.RawUrl) || ((string)(currRow["Link"].ToString() + @"?ParentID=" + ParentID)).Contains(Request.RawUrl))
                        int duan_index = currRow["Link"].ToString().IndexOf("duan_catid");
                        string duan_catid = "";
                        if (duan_index > -1)
                        {
                            string restLink = currRow["Link"].ToString().Substring(duan_index + 11);
                            duan_catid = restLink.Substring(0, restLink.IndexOf("/"));
                        }
                        if (Request.QueryString["BCMenu"] == menu && Request.QueryString["ParentID"] == ParentID && Request.QueryString["duan_catid"] == duan_catid)
                        {
                            if (KeepTrack)
                            {
                                ltScript.Text += @"<a class=""" + CSSPrefix + @"mO" + ((int)(currLevel - 1)).ToString() + @"_select"" href=""" + currRow["Link"].ToString() + @"?BCMenu=" + menu + @"&ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>
                                ";
                            }
                            else
                            {
                                ltScript.Text += @"<a class=""" + CSSPrefix + @"mO" + ((int)(currLevel - 1)).ToString() + @"_select"" href=""" + currRow["Link"].ToString() + @"?ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>
                                ";
                            }
                        }
                        else
                        {
                            if (KeepTrack)
                            {
                                if (Convert.ToBoolean(currRow["Enable"]))
                                {
                                    ltScript.Text += @"<a class=""" + CSSPrefix + @"mO" + ((int)(currLevel - 1)).ToString() +
                                                    @""" href=""" + currRow["Link"].ToString() + @"?BCMenu=" + menu + @"&ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                    blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>";
                                }
                                else
                                {
                                    ltScript.Text += @"<a class=""" + CSSPrefix + @"mO" + ((int)(currLevel - 1)).ToString() +
                                                    @"_disable"" href=""" + currRow["Link"].ToString() + @"?BCMenu=" + menu + @"&ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                    blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>";
                                }
                            }
                            else
                            {
                                if (Convert.ToBoolean(currRow["Enable"]))
                                {
                                    ltScript.Text += @"<a class=""" + CSSPrefix + @"mO" + ((int)(currLevel - 1)).ToString() +
                                                    @""" href=""" + currRow["Link"].ToString() + @"?ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                    blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>";
                                }
                                else
                                {
                                    ltScript.Text += @"<a class=""" + CSSPrefix + @"mO" + ((int)(currLevel - 1)).ToString() +
                                                    @"_disable"" href=""" + currRow["Link"].ToString() + @"?ParentID=" + ParentID + @""" " + @"target=""" + currRow["Target"].ToString() + @""" >" +
                                                    blank + currRow["Image"].ToString() + currRow["Name"].ToString() + @"</a>";
                                }
                            }
                        }
                        for (int j = 0; j < currLevel - nextLevel; j++)
                        {
                            ltScript.Text += @"
                                            </div>";
                        }
                    }
                }

                string ExpandedMenu = Request.QueryString["BCMenu"];
                string ExpandScript = "";
                string ExpandScriptHead = @"
                                        <script type=""text/javascript"">
                                        ";
                string ExpandScriptTail = @"</script>";

                ltScript.Text += ExpandScriptHead;
                if (ExpandedMenu != null && ExpandedMenu.Contains(MenuName))
                {
                    if (ExpandedMenu.StartsWith("c_"))
                    {
                        ExpandedMenu = ExpandedMenu.Substring(2);
                        int dotIndex = ExpandedMenu.LastIndexOf(".");
                        if (dotIndex != -1)
                        {
                            ExpandedMenu = ExpandedMenu.Substring(0, dotIndex);
                            dotIndex = ExpandedMenu.LastIndexOf(".");
                        }
                        while (dotIndex != -1)
                        {
                            ExpandScript = "toggleMenu('" + ExpandedMenu + "','img_" + ExpandedMenu + "','" + plusImg + "','" + minusImg + @"');
                                        " + ExpandScript;
                            ExpandedMenu = ExpandedMenu.Substring(0, dotIndex);
                            dotIndex = ExpandedMenu.LastIndexOf(".");
                        }
                        ltScript.Text += ExpandScript;
                    }
                    else
                    {
                        int dotIndex = ExpandedMenu.LastIndexOf(".");
                        while (dotIndex != -1)
                        {
                            ExpandScript = "toggleMenu('" + ExpandedMenu + "','img_" + ExpandedMenu + "','" + plusImg + "','" + minusImg + @"');
                                        " + ExpandScript;
                            ExpandedMenu = ExpandedMenu.Substring(0, dotIndex);
                            dotIndex = ExpandedMenu.LastIndexOf(".");
                        }
                        ltScript.Text += ExpandScript;
                    }
                }
                if (ExpandAll)
                {
                    foreach (string s in lstToggle)
                    {
                        ltScript.Text += "expandMenu('" + s + "');";
                    }
                }
                ltScript.Text += ExpandScriptTail;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //LoadMenu();
        }
    }
}