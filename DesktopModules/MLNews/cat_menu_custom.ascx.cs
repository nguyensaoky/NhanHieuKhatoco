using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace DotNetNuke.News
{
    public partial class cat_menu_custom : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private XmlDocument LoadData(string sID, string thutu)
        {
            try
            {
                XmlDocument doc = null;
                NewsController engine = new NewsController();
                
                int numShortNews = 0;
                if (Settings["numshortnews" + thutu] != null)
                {
                    numShortNews = int.Parse(Settings["numshortnews" + thutu].ToString());
                }
                int imageWidth = 100;
                if (Settings["imagewidth" + thutu] != null)
                {
                    imageWidth = int.Parse(Settings["imagewidth" + thutu].ToString());
                }

                doc = engine.LoadXMLByIDArray(sID, imageWidth, numShortNews);
                return doc;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                return new XmlDocument();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                NewsController cont = new NewsController();
                int tabid = 0;
                DotNetNuke.Entities.Tabs.TabController tabCont = new DotNetNuke.Entities.Tabs.TabController();
                DotNetNuke.Entities.Tabs.TabInfo tab = tabCont.GetTabByName(ConfigurationManager.AppSettings["ThuVienSo_ListNewsPage"], PortalId);
                if (tab != null) tabid = tab.TabID;

                //lnk1.Attributes["onclick"] = "window.location.href='" + DotNetNuke.Common.Globals.NavigateURL(tabid, "", "categoryid/0_TieuDiem") + "';return false;";
                //lnk2.Attributes["onclick"] = "window.location.href='" + DotNetNuke.Common.Globals.NavigateURL(tabid, "", "categoryid/0_MoiBanHanh") + "';return false;";
                //lnk3.Attributes["onclick"] = "window.location.href='" + DotNetNuke.Common.Globals.NavigateURL(tabid, "", "categoryid/0_BanChuaBiet") + "';return false;";

                ltTieuDe1.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "categoryid/0_TieuDiem");
                ltTieuDe2.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "categoryid/0_MoiBanHanh");
                ltTieuDe3.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "categoryid/0_BanChuaBiet");

                if (Settings["tieude1"] != null) { ltTieuDe1.Text = Settings["tieude1"].ToString(); ltTieuDe1.Attributes["class"] = "TieuDe1"; }
                if (Settings["tieude2"] != null) { ltTieuDe2.Text = Settings["tieude2"].ToString(); ltTieuDe2.Attributes["class"] = "TieuDe2"; }
                if (Settings["tieude3"] != null) { ltTieuDe3.Text = Settings["tieude3"].ToString(); ltTieuDe3.Attributes["class"] = "TieuDe3"; }
                
                if (Settings["baiviet1"] != null && Settings["baiviet1"].ToString() != "")
                {
                    string baiviet1 = Settings["baiviet1"].ToString();
                    XmlDocument doc1 = LoadData(baiviet1, "1");
                    XmlElement root1 = doc1.DocumentElement;
                    if (doc1.InnerXml != "<newslist></newslist>")
                    {
                        if (Settings["template1"] != null)
                        {
                            string template1 = PortalSettings.HomeDirectory + "Xsl/" + Settings["template1"].ToString();
                            DotNetNuke.NewsProvider.Utils.XMLTransform(lt1, template1, doc1);
                        }
                    }
                }

                if (Settings["baiviet2"] != null && Settings["baiviet2"].ToString() != "")
                {
                    string baiviet2 = Settings["baiviet2"].ToString();
                    XmlDocument doc2 = LoadData(baiviet2, "2");
                    XmlElement root2 = doc2.DocumentElement;
                    if (doc2.InnerXml != "<newslist></newslist>")
                    {
                        if (Settings["template2"] != null)
                        {
                            string template2 = PortalSettings.HomeDirectory + "Xsl/" + Settings["template2"].ToString();
                            DotNetNuke.NewsProvider.Utils.XMLTransform(lt2, template2, doc2);
                        }
                    }
                }

                if (Settings["baiviet3"] != null && Settings["baiviet3"].ToString() != "")
                {
                    string baiviet3 = Settings["baiviet3"].ToString();
                    XmlDocument doc3 = LoadData(baiviet3, "3");
                    XmlElement root3 = doc3.DocumentElement;
                    if (doc3.InnerXml != "<newslist></newslist>")
                    {
                        if (Settings["template3"] != null)
                        {
                            string template3 = PortalSettings.HomeDirectory + "Xsl/" + Settings["template3"].ToString();
                            DotNetNuke.NewsProvider.Utils.XMLTransform(lt3, template3, doc3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}