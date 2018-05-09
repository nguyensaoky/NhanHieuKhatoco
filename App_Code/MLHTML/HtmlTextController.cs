using System;
using System.Configuration;
using System.Data;
using DotNetNuke.Services.Search;
using DotNetNuke.Services.Localization;
using DotNetNuke;
using System.Xml;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Common;

namespace DotNetNuke.HTML
{
    public class HtmlTextController : DotNetNuke.Entities.Modules.ISearchable, DotNetNuke.Entities.Modules.IPortable
    {
        private const int MAX_DESCRIPTION_LENGTH = 100;

        public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo ModInfo)
        {
            SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();
            LocaleCollection lCollection = Localization.GetEnabledLocales();
            foreach (Locale localeInfo in lCollection)
            {
                effority.Ealo.StringInfo objText;
                effority.Ealo.StringInfo objSummary;
                objText = effority.Ealo.Controller.GetStringByQualifierAndStringName(Consts.DesktopHTMLQualifier, ModInfo.ModuleID.ToString(), localeInfo.Code, true);
                objSummary = effority.Ealo.Controller.GetStringByQualifierAndStringName(Consts.DesktopSummaryQualifier, ModInfo.ModuleID.ToString(), localeInfo.Code, true);
                if (!objText.StringTextIsNull)
                {
                    string strDesktopHtml = HttpUtility.HtmlDecode(objText.StringText);
                    string strDescription = HtmlUtils.Shorten(HtmlUtils.Clean(strDesktopHtml, false), MAX_DESCRIPTION_LENGTH, "...");
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, localeInfo.Text + " - " + strDescription, -1, DateTime.Now, ModInfo.ModuleID, "", objSummary.StringText + " " + strDesktopHtml, "", Null.NullInteger);
                    SearchItemCollection.Add(SearchItem);
                }
            }
            return SearchItemCollection;
        }

        public string ExportModule(int ModuleID)
        {
            string strXML = "";
            effority.Ealo.StringInfo objText;
            effority.Ealo.StringInfo objSummary;
            objText = effority.Ealo.Controller.GetStringByQualifierAndStringName(Consts.DesktopHTMLQualifier, ModuleID.ToString(), System.Threading.Thread.CurrentThread.CurrentCulture.Name, true);
            objSummary = effority.Ealo.Controller.GetStringByQualifierAndStringName(Consts.DesktopSummaryQualifier, ModuleID.ToString(), System.Threading.Thread.CurrentThread.CurrentCulture.Name, true);
            if ((objText != null))
            {
                strXML += "<htmltext>";
                strXML += "<desktophtml>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objText.StringText) + "</desktophtml>";
                if ((objSummary != null))
                {
                    strXML += "<desktopsummary>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objSummary.StringText) + "</desktopsummary>";
                }
                strXML += "</htmltext>";
            }
            return strXML;
        }

        public void ImportModule(int ModuleID, string Content, string Version, int UserId)
        {
            XmlNode xmlHtmlText = Globals.GetContent(Content, "htmltext");
            effority.Ealo.StringInfo objText;
            effority.Ealo.StringInfo objSummary;

            objText = effority.Ealo.Controller.GetStringByQualifierAndStringName(Consts.DesktopHTMLQualifier, ModuleID.ToString(), System.Threading.Thread.CurrentThread.CurrentCulture.Name, true);
            objSummary = effority.Ealo.Controller.GetStringByQualifierAndStringName(Consts.DesktopSummaryQualifier, ModuleID.ToString(), System.Threading.Thread.CurrentThread.CurrentCulture.Name, true);
            string newObjText = xmlHtmlText.SelectSingleNode("desktophtml").InnerText;
            if (!objText.StringTextIsNull)
            {
                newObjText = objText.StringText + newObjText;
            }
            effority.Ealo.Controller.UpdateStringByQualiferAndStringName(Consts.DesktopHTMLQualifier, ModuleID.ToString(), newObjText, System.Threading.Thread.CurrentThread.CurrentCulture.Name, true);
            string newObjSummary = xmlHtmlText.SelectSingleNode("desktopsummary").InnerText;
            if (!objSummary.StringTextIsNull)
            {
                newObjSummary = objSummary.StringText + newObjSummary;
            }
            effority.Ealo.Controller.UpdateStringByQualiferAndStringName(Consts.DesktopSummaryQualifier, ModuleID.ToString(), newObjSummary, System.Threading.Thread.CurrentThread.CurrentCulture.Name, true);
        }
    }
}