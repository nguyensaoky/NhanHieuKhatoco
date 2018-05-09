using System;
using System.Data;
using System.Data.SqlClient;
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
    public partial class news_relative : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        private string GetCat(string newsid)
        {
            string strSQL = "News_getCatIDOfNews";
            SqlParameter p = new SqlParameter("@NewsID", int.Parse(newsid));
            DataTable o = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, p);
            if (o != null && o.Rows.Count==1)
            {
                return o.Rows[0][0].ToString();
            }
            return "";
        }

        private XmlDocument LoadData()
        {
            XmlDocument doc = new XmlDocument();
            string newsid = Request.QueryString["id"];
            int source = Convert.ToInt32(Settings["source"]);
            int numRecord = 0;
            if (source == 0)
            {
                int pagesize = Convert.ToInt32(Settings["limits"]);
                if (pagesize == 0) pagesize = 4;
                if (newsid != null)
                {
                    string categoryid = GetCat(newsid).ToString();
                    NewsController engine = new NewsController();
                    doc = engine.LoadXML_RelativeNews(categoryid, newsid, pagesize, out numRecord);
                }
            }
            else if (source == 1)
            {
                if (newsid != null)
                {
                    NewsController engine = new NewsController();
                    doc = engine.LoadXML_RelativeNewsKeyWords(newsid, out numRecord);
                }
            }
            return doc;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    NewsController engine = new NewsController();
                    XmlDocument doc = LoadData();
                    string template;
                    if (doc.InnerXml != "<newslist></newslist>")
                    {
                        if (Settings["template"] == null)
                        {
                            template = PortalSettings.HomeDirectory + "Xsl/news_relative.xsl";
                        }
                        else
                        {
                            template = PortalSettings.HomeDirectory + "Xsl/" + Settings["template"].ToString();
                        }

                        DotNetNuke.NewsProvider.Utils.XMLTransform(xmlTransformer, template, doc);
                    }
                    else
                    {
                        ContainerControl.Visible = false;
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