using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Collections;
using DotNetNuke.NewsProvider;

using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Common;
using Microsoft.ApplicationBlocks.Data;

namespace DotNetNuke.News
{
    public class NewsGroupController
    {
        #region Constructor
        public NewsGroupController() { }
        #endregion

        #region Function
        private const string INSERT_NEWS_NewsGroup = "News_insertNewsGroup";
        public int Insert(NewsGroupInfo group)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@NewsGroupID", group.NewsGroupID);
                param[1] = new SqlParameter("@NewsGroupName", group.NewsGroupName);
                param[2] = new SqlParameter("@Description", group.Description);
                param[3] = new SqlParameter("@NewsGroupCode", group.NewsGroupCode);
                param[4] = new SqlParameter("@OrderNumber", group.OrderNumber);
                param[5] = new SqlParameter("@PortalID", group.PortalID);
                param[6] = new SqlParameter("@DesktopListID", group.DesktopListID);
                param[7] = new SqlParameter("@DesktopViewID", group.DesktopViewID);
                result = DataProvider.ExecuteSP(INSERT_NEWS_NewsGroup, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string UPDATE_NEWS_NewsGroup = "News_updateNewsGroup";
        public int Update(NewsGroupInfo group)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@NewsGroupID", group.NewsGroupID);
                param[1] = new SqlParameter("@NewsGroupName", group.NewsGroupName);
                param[2] = new SqlParameter("@Description", group.Description);
                param[3] = new SqlParameter("@NewsGroupCode", group.NewsGroupCode);
                param[4] = new SqlParameter("@OrderNumber", group.OrderNumber);
                param[5] = new SqlParameter("@PortalID", group.PortalID);
                param[6] = new SqlParameter("@DesktopListID", group.DesktopListID);
                param[7] = new SqlParameter("@DesktopViewID", group.DesktopViewID);
                result = DataProvider.ExecuteSP(UPDATE_NEWS_NewsGroup, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string DELETE_NEWS_NewsGroup = "News_deleteNewsGroup";
        public int Delete(string NewsGroupID)
        {
            int result = 0;
            try
            {
                SqlParameter param = new SqlParameter("@NewsGroupID", NewsGroupID);
                result = DataProvider.ExecuteSP(DELETE_NEWS_NewsGroup, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public NewsGroupInfo Converting(DataRow drNews_NewsGroup)
        {
            NewsGroupInfo result = null;
            try
            {
                result = new NewsGroupInfo();
                result.NewsGroupID = (string)drNews_NewsGroup["NewsGroupID"];
                result.NewsGroupName = (string)drNews_NewsGroup["NewsGroupName"];
                result.NewsGroupCode = (string)drNews_NewsGroup["NewsGroupCode"];
                result.Description = (string)drNews_NewsGroup["Description"];
                result.OrderNumber = (int)drNews_NewsGroup["OrderNumber"];
                result.PortalID = (int)drNews_NewsGroup["PortalID"];
                result.DesktopListID = (int)drNews_NewsGroup["DesktopListID"];
                result.DesktopViewID = (int)drNews_NewsGroup["DesktopViewID"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string LOAD_NEWS_NewsGroup = "News_getNewsGroupByID";
        public NewsGroupInfo Load(string NewsGroupID)
        {
            NewsGroupInfo result = null;
            try
            {
                SqlParameter param = new SqlParameter("@NewsGroupID", NewsGroupID);
                DataTable dt = DataProvider.SelectSP(LOAD_NEWS_NewsGroup, param);
                if (dt.Rows.Count > 0) result = Converting(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string LOAD_LIST_NEWS_NewsGroup = "News_getNewsGroupFull";
        public DataTable LoadTable(int PortalID)
        {
            DataTable result = null;
            try
            {
                SqlParameter param = new SqlParameter("@PortalID", PortalID);
                result = DataProvider.SelectSP(LOAD_LIST_NEWS_NewsGroup, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public Dictionary<string, NewsGroupInfo> GetNewsGroupsByPortal(int PortalId)
        {
            string key = string.Format(DataCache.NewsGroupCacheKey, PortalId);
            //First Check the NewsGroup Cache
            Dictionary<string, NewsGroupInfo> groups = DataCache.GetCache(key) as Dictionary<string, NewsGroupInfo>;
            if (groups == null)
            {
                Int32 timeOut = DataCache.NewsGroupCacheTimeOut * Convert.ToInt32(Globals.PerformanceSetting);
                groups = FillNewsGroupInfoDictionary(DataProvider.ExecuteReader("News_getNewsGroupByPortal", new object[] { PortalId }));
                //Cache cats
                if (timeOut > 0)
                {
                    DataCache.SetCache(key, groups, TimeSpan.FromMinutes(timeOut));
                }
            }
            return groups;
        }

        private static Dictionary<string, NewsGroupInfo> FillNewsGroupInfoDictionary(IDataReader dr)
        {
            Dictionary<string, NewsGroupInfo> dic = new Dictionary<string, NewsGroupInfo>();
            try
            {
                while (dr.Read())
                {
                    NewsGroupInfo obj = FillNewsGroupInfo(dr);
                    dic.Add(obj.NewsGroupID, obj);
                }
            }
            catch (Exception exc)
            {
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
            return dic;
        }

        private static NewsGroupInfo FillNewsGroupInfo(IDataReader dr)
        {
            NewsGroupInfo objNewsGroupInfo = new NewsGroupInfo();

            objNewsGroupInfo.NewsGroupID = Convert.ToString(Null.SetNull(dr["NewsGroupID"], objNewsGroupInfo.NewsGroupID));
            objNewsGroupInfo.NewsGroupName = Convert.ToString(Null.SetNull(dr["NewsGroupName"], objNewsGroupInfo.NewsGroupName));
            objNewsGroupInfo.NewsGroupCode = Convert.ToString(Null.SetNull(dr["NewsGroupCode"], objNewsGroupInfo.NewsGroupCode));
            objNewsGroupInfo.Description = Convert.ToString(Null.SetNull(dr["Description"], objNewsGroupInfo.Description));
            objNewsGroupInfo.OrderNumber = Convert.ToInt32(Null.SetNull(dr["OrderNumber"], objNewsGroupInfo.OrderNumber));
            objNewsGroupInfo.PortalID = Convert.ToInt32(Null.SetNull(dr["PortalID"], objNewsGroupInfo.PortalID));
            objNewsGroupInfo.DesktopListID = Convert.ToInt32(Null.SetNull(dr["DesktopListID"], objNewsGroupInfo.DesktopListID));
            objNewsGroupInfo.DesktopViewID = Convert.ToInt32(Null.SetNull(dr["DesktopViewID"], objNewsGroupInfo.DesktopViewID));

            return objNewsGroupInfo;
        }

        #endregion

        #region load tree NewsGroup
        public XmlDocument LoadTreeXML(string editFormatUrl, bool ismenu, int PortalID, int source, string codes)
        {
            DataTable dt=null;
            if(source == 0)
            {
                dt = LoadTree(ismenu, PortalID);
            }
            else if(source == 1)
            {
                dt = LoadTreeByCode(ismenu, PortalID, codes);
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<newsgroups></newsgroups>");
            XmlElement root = doc.DocumentElement;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                XmlElement NewsGroup = doc.CreateElement("newsgroup");

                int tabid = Convert.ToInt32(dt.Rows[i]["DesktopListID"]);
                string link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "newsgroupid/" + dt.Rows[i]["NewsGroupID"].ToString());
                string url_edit = editFormatUrl.Replace("@@groupid", dt.Rows[i]["NewsGroupID"].ToString());

                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "Img", DotNetNuke.Common.Globals.ApplicationPath + "/images/action_right.gif");
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "ImgLevel2", DotNetNuke.Common.Globals.ApplicationPath + "/images/edit.gif");
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "OrderNumber", dt.Rows[i]["OrderNumber"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "NewsGroupName", dt.Rows[i]["NewsGroupName"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "NewsGroupCode", dt.Rows[i]["NewsGroupCode"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "PortalID", dt.Rows[i]["PortalID"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "link", link);
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "url_edit", url_edit);                
                if (i % 2 == 1)
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "odd", "true");
                root.AppendChild(NewsGroup);
            }
            return doc;
        }

        public XmlDocument MLLoadTreeXML(string editFormatUrl, bool ismenu, int PortalID, int source, string codes, string Locale)
        {
            DataTable dt = null;
            if (source == 0)
            {
                dt = LoadTree(ismenu, PortalID);
            }
            else if (source == 1)
            {
                dt = LoadTreeByCode(ismenu, PortalID, codes);
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<newsgroups></newsgroups>");
            XmlElement root = doc.DocumentElement;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                XmlElement NewsGroup = doc.CreateElement("newsgroup");
                int tabid = Convert.ToInt32(dt.Rows[i]["DesktopListID"]);
                string link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "newsgroupid/" + dt.Rows[i]["NewsGroupID"].ToString());
                string url_edit = editFormatUrl.Replace("@@groupid", dt.Rows[i]["NewsGroupID"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "Img", DotNetNuke.Common.Globals.ApplicationPath + "/images/action_right.gif");
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "ImgLevel2", DotNetNuke.Common.Globals.ApplicationPath + "/images/edit.gif");
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "OrderNumber", dt.Rows[i]["OrderNumber"].ToString());

                MLNewsGroupInfo fMLGroup = MLNewsGroupController.GetNewsGroup(dt.Rows[i]["NewsGroupID"].ToString(), Locale, false);
                if (fMLGroup != null)
                {
                    if (fMLGroup.MLGroupName != null)
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "NewsGroupName", fMLGroup.MLGroupName.StringTextOrFallBack);
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "NewsGroupName", fMLGroup.NewsGroupName);
                    }
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "NewsGroupName", dt.Rows[i]["NewsGroupName"].ToString());
                }
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "NewsGroupCode", dt.Rows[i]["NewsGroupCode"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "PortalID", dt.Rows[i]["PortalID"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "link", link);
                DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "url_edit", url_edit);
                if (i % 2 == 1)
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, NewsGroup, "odd", "true");
                root.AppendChild(NewsGroup);
            }
            return doc;
        }

        public DataTable LoadTree(bool ismenu, int PortalID)
        {
            SqlParameter param = new SqlParameter("@PortalID", PortalID);
            DataTable dt = DataProvider.SelectSP("News_getNewsGroupFullOrder", param);
            return dt;
        }

        public DataTable LoadTreeByCode(bool ismenu, int PortalID, string Codes)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@PortalID", PortalID);
            param[1] = new SqlParameter("@Codes", Codes);
            DataTable dt = DataProvider.SelectSP("News_getNewsGroupByCode", param);
            return dt;
        }

        public DataTable LoadTreeForMenu(int PortalID, int source, string codes, string Locale)
        {
            DataTable dt = null;
            if (source == 0)
            {
                dt = LoadTree(true, PortalID);
            }
            else if (source == 1)
            {
                dt = LoadTreeByCode(true, PortalID, codes);
            }
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Link", typeof(string)));
            dt.Columns.Add(new DataColumn("Level", typeof(int)));
            dt.Columns.Add(new DataColumn("Target", typeof(string)));
            dt.Columns.Add(new DataColumn("Image", typeof(string)));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Target"] = "_self";
                dt.Rows[i]["Image"] = "";
                dt.Rows[i]["Level"] = 1;
                int tabid = Convert.ToInt32(dt.Rows[i]["DesktopListID"]);
                dt.Rows[i]["Link"] = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "newsgroupid/" + dt.Rows[i]["NewsGroupID"].ToString());
                MLNewsGroupInfo fMLNewsGroup = MLNewsGroupController.GetNewsGroup(dt.Rows[i]["NewsGroupID"].ToString(), Locale, false);
                if (fMLNewsGroup != null)
                {
                    if (fMLNewsGroup.MLGroupName != null)
                    {
                        dt.Rows[i]["Name"] = fMLNewsGroup.MLGroupName.StringText;
                    }
                    else
                    {
                        dt.Rows[i]["Name"] = fMLNewsGroup.NewsGroupName;
                    }
                }
                else
                {
                    dt.Rows[i]["Name"] = dt.Rows[i]["NewsGroupName"].ToString();
                }

            }
            return dt;
        }

        #endregion
    }
}