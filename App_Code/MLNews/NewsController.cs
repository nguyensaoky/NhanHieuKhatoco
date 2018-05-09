using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using DotNetNuke.NewsProvider;
using DotNetNuke.Services.Search;
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Common;
using Microsoft.ApplicationBlocks.Data;
using System.Web;

namespace DotNetNuke.News
{
    public class NewsController : Entities.Modules.ISearchable
    {
        #region Constructor
        public NewsController() { }
        #endregion

        #region Function
        private const string INSERT_NEWS_NEWS = "News_insertNews";
        public int Insert(NewsInfo news)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[21];
                param[0] = new SqlParameter("@CatID", news.CatID);
                param[1] = new SqlParameter("@Headline", news.Headline);
                param[2] = new SqlParameter("@Description", news.Description);
                param[3] = new SqlParameter("@ImageUrl", news.ImageUrl);
                param[4] = new SqlParameter("@Source", news.Source);
                param[5] = new SqlParameter("@AllowComment", news.AllowComment);
                param[6] = new SqlParameter("@Published", news.Published);
                param[7] = new SqlParameter("@CreatedDate", news.CreatedDate);
                param[8] = new SqlParameter("@CreateID", news.CreateID);
                param[9] = new SqlParameter("@ModifyDate", news.ModifyDate);
                param[10] = new SqlParameter("@ModifyID", news.ModifyID);
                param[11] = new SqlParameter("@TotalView", news.TotalView);
                param[12] = new SqlParameter("@Content", news.Content);
                param[13] = new SqlParameter("@KeyWords", news.KeyWords);
                param[14] = new SqlParameter("@StartDate", news.StartDate);
                param[15] = new SqlParameter("@EndDate", news.EndDate);
                param[16] = new SqlParameter("@Feature", news.Feature);
                param[17] = new SqlParameter("@Writer", news.Writer);
                param[18] = new SqlParameter("@DonVi", news.DonVi);
                param[19] = new SqlParameter("@FromOuter", news.FromOuter);
                param[20] = new SqlParameter("@NewsID", news.ID);
                param[20].Direction = ParameterDirection.Output;
                result = DataProvider.ExecuteSP(INSERT_NEWS_NEWS, param);
                news.ID = int.Parse(param[20].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string INSERT_NEWS_COMMENT = "News_insertComment";
        public int InsertComment(CommentInfo info)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@NewsID", info.NewsID);
                param[1] = new SqlParameter("@Headline", info.Headline);
                param[2] = new SqlParameter("@Content", info.Content);
                param[3] = new SqlParameter("@AuthorEmail", info.AuthorEmail);
                param[4] = new SqlParameter("@Author", info.Author);
                param[5] = new SqlParameter("@CreatedDate", info.CreatedDate);
                param[6] = new SqlParameter("@Status", info.Status);
                param[7] = new SqlParameter("@ClientIPAddress", info.ClientIPAddress);
                param[8] = new SqlParameter("@ClientHostName", info.ClientHostName);
                result = DataProvider.ExecuteSP(INSERT_NEWS_COMMENT, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string UPDATE_NEWS_NEWS = "News_updateNews";
        public int Update(NewsInfo news)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[21];
                param[0] = new SqlParameter("@CatID", news.CatID);
                param[1] = new SqlParameter("@Headline", news.Headline);
                param[2] = new SqlParameter("@Description", news.Description);
                param[3] = new SqlParameter("@ImageUrl", news.ImageUrl);
                param[4] = new SqlParameter("@Source", news.Source);
                param[5] = new SqlParameter("@AllowComment", news.AllowComment);
                param[6] = new SqlParameter("@Published", news.Published);
                param[7] = new SqlParameter("@CreatedDate", news.CreatedDate);
                param[8] = new SqlParameter("@CreateID", news.CreateID);
                param[9] = new SqlParameter("@ModifyDate", news.ModifyDate);
                param[10] = new SqlParameter("@ModifyID", news.ModifyID);
                param[11] = new SqlParameter("@TotalView", news.TotalView);
                param[12] = new SqlParameter("@Content", news.Content);
                param[13] = new SqlParameter("@ID", news.ID);
                param[14] = new SqlParameter("@KeyWords", news.KeyWords);
                param[15] = new SqlParameter("@StartDate", news.StartDate);
                param[16] = new SqlParameter("@EndDate", news.EndDate);
                param[17] = new SqlParameter("@Feature", news.Feature);
                param[18] = new SqlParameter("@Writer", news.Writer);
                param[19] = new SqlParameter("@DonVi", news.DonVi);
                param[20] = new SqlParameter("@FromOuter", news.FromOuter);
                result = DataProvider.ExecuteSP(UPDATE_NEWS_NEWS, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string UPDATE_NEWS_SOURCENEWS = "News_updateSourceNews";
        public int UpdateSource(int orgID, int newID, int PortalID)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@orgID", orgID);
                param[1] = new SqlParameter("@newID", PortalID.ToString() + "_" + newID.ToString());
                result = DataProvider.ExecuteSP(UPDATE_NEWS_SOURCENEWS, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string UPDATE_NEWS_NEWSGROUPNEWS = "News_updateNewsGroupNews";
        public int UpdateNewsGroupNews(int NewsID, string NewsGroupString)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@NewsID", NewsID);
                param[1] = new SqlParameter("@NewsGroupString", NewsGroupString);
                result = DataProvider.ExecuteSP(UPDATE_NEWS_NEWSGROUPNEWS, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string DELETE_NEWS_NEWS = "News_deleteNews";
        public int Delete(int ID)
        {
            int result = 0;
            try
            {
                SqlParameter param = new SqlParameter("@ID", ID);
                result = DataProvider.ExecuteSP(DELETE_NEWS_NEWS, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string DELETE_NEWS_COMMENT = "News_deleteComment";
        public int DeleteComment(int ID)
        {
            int result = 0;
            try
            {
                SqlParameter param = new SqlParameter("@ID", ID);
                result = DataProvider.ExecuteSP(DELETE_NEWS_COMMENT, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string UPDATE_NEWS_COMMENT_STATUS = "News_updateCommentStatus";
        public int UpdateCommentStatus(int ID)
        {
            int result = 0;
            try
            {
                SqlParameter param = new SqlParameter("@ID", ID);
                result = DataProvider.ExecuteSP(UPDATE_NEWS_COMMENT_STATUS, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string DELETE_NEWS_NEWSGROUPNEWS = "News_deleteNewsGroupNews";
        public int DeleteNewsGroupNews(int NewsID)
        {
            int result = 0;
            try
            {
                SqlParameter param = new SqlParameter("@NewsID", NewsID);
                result = DataProvider.ExecuteSP(DELETE_NEWS_NEWSGROUPNEWS, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public NewsInfo Converting(DataRow drNews_News)
        {
            NewsInfo result = null;
            try
            {
                result = new NewsInfo();
                result.ID = (int)drNews_News["ID"];
                result.CatID = (string)drNews_News["CatID"];
                result.Headline = (string)drNews_News["Headline"];
                result.Description = (string)drNews_News["Description"];
                result.ImageUrl = (string)drNews_News["ImageUrl"];
                result.Source = (string)drNews_News["Source"];
                result.AllowComment = Convert.ToBoolean(drNews_News["AllowComment"]);
                result.Published = Convert.ToBoolean(drNews_News["Published"]);
                result.CreatedDate = (DateTime)drNews_News["CreatedDate"];
                result.CreateID = (int)drNews_News["CreateID"];
                result.ModifyDate = (DateTime)drNews_News["ModifyDate"];
                result.ModifyID = (int)drNews_News["ModifyID"];
                result.TotalView = (int)drNews_News["TotalView"];
                result.Content = (string)drNews_News["Content"];
                if (drNews_News["Writer"] != DBNull.Value) result.Writer = Convert.ToString(drNews_News["Writer"]);
                if (drNews_News["DonVi"] != DBNull.Value) result.DonVi = Convert.ToString(drNews_News["DonVi"]);
                if (drNews_News["FromOuter"] != DBNull.Value) result.FromOuter = Convert.ToBoolean(drNews_News["FromOuter"]);
                if (drNews_News["KeyWords"] != DBNull.Value)
                {
                    result.KeyWords = (string)drNews_News["KeyWords"];
                }
                else
                {
                    result.KeyWords = "";
                }
                if (drNews_News["StartDate"] != DBNull.Value)
                {
                    result.StartDate = Convert.ToDateTime(drNews_News["StartDate"]);
                }
                else
                {
                    result.StartDate = null;
                }
                if (drNews_News["EndDate"] != DBNull.Value)
                {
                    result.EndDate = Convert.ToDateTime(drNews_News["EndDate"]);
                }
                else
                {
                    result.EndDate = null;
                }
                if (drNews_News["Feature"] != DBNull.Value)
                {
                    result.Feature = Convert.ToInt32(drNews_News["Feature"]);
                }
                else
                {
                    result.Feature = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public NewsInfo ConvertingML(DataRow drNews_News)
        {
            NewsInfo result = null;
            try
            {
                result = new NewsInfo();
                result.ID = (int)drNews_News["ID"];
                result.CatID = (string)drNews_News["CatID"];
                MLNewsInfo fMLNewsInfo = MLNewsController.GetNews(result.ID, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), false);
                if ((fMLNewsInfo == null || fMLNewsInfo.MLHeadline == null) && System.Threading.Thread.CurrentThread.CurrentCulture.ToString() != Common.Globals.GetPortalSettings().DefaultLanguage)
                {
                    return null;
                }
                if (fMLNewsInfo != null)
                {
                    if (fMLNewsInfo.MLHeadline != null)
                    {
                        result.Headline = fMLNewsInfo.MLHeadline.StringTextOrFallBack;
                    }
                    else
                    {
                        result.Headline = fMLNewsInfo.Headline;
                    }
                    if (fMLNewsInfo.MLDescription != null)
                    {
                        result.Description = fMLNewsInfo.MLDescription.StringTextOrFallBack;
                    }
                    else
                    {
                        result.Description = fMLNewsInfo.Description;
                    }
                    if (fMLNewsInfo.MLContent != null)
                    {
                        result.Content = fMLNewsInfo.MLContent.StringTextOrFallBack;
                    }
                    else
                    {
                        result.Content = fMLNewsInfo.Content;
                    }
                }
                else
                {
                    result.Headline = (string)drNews_News["Headline"];
                    result.Description = (string)drNews_News["Description"];
                    result.Content = (string)drNews_News["Content"];
                }
                
                result.ImageUrl = (string)drNews_News["ImageUrl"];
                result.Source = (string)drNews_News["Source"];
                result.AllowComment = Convert.ToBoolean(drNews_News["AllowComment"]);
                result.Published = Convert.ToBoolean(drNews_News["Published"]);
                result.CreatedDate = (DateTime)drNews_News["CreatedDate"];
                result.CreateID = (int)drNews_News["CreateID"];
                result.ModifyDate = (DateTime)drNews_News["ModifyDate"];
                result.ModifyID = (int)drNews_News["ModifyID"];
                result.TotalView = (int)drNews_News["TotalView"];
                if (drNews_News["KeyWords"] != DBNull.Value)
                {
                    result.KeyWords = (string)drNews_News["KeyWords"];
                }
                else
                {
                    result.KeyWords = "";
                }
                if (drNews_News["StartDate"] != DBNull.Value)
                {
                    result.StartDate = Convert.ToDateTime(drNews_News["StartDate"]);
                }
                else
                {
                    result.StartDate = null;
                }
                if (drNews_News["EndDate"] != DBNull.Value)
                {
                    result.EndDate = Convert.ToDateTime(drNews_News["EndDate"]);
                }
                else
                {
                    result.EndDate = null;
                }
                if (drNews_News["Feature"] != DBNull.Value)
                {
                    result.Feature = Convert.ToInt32(drNews_News["Feature"]);
                }
                else
                {
                    result.Feature = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string LOAD_NEWS_NEWS = "News_getNewsByID";
        public NewsInfo Load(int ID)
        {
            NewsInfo result = null;
            try
            {
                SqlParameter param = new SqlParameter("@ID", ID);
                DataTable dt = DataProvider.SelectSP(LOAD_NEWS_NEWS, param);
                if (dt.Rows.Count > 0) result = ConvertingML(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public NewsInfo LoadNoML(int ID)
        {
            NewsInfo result = null;
            try
            {
                SqlParameter param = new SqlParameter("@ID", ID);
                DataTable dt = DataProvider.SelectSP(LOAD_NEWS_NEWS, param);
                if (dt.Rows.Count > 0) result = Converting(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string LOAD_LIST_NEWS_NEWS = "News_getNewsFull";
        public DataTable LoadTable(string CatID, int PortalID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@CatID", CatID);
                param[1] = new SqlParameter("@PortalID", PortalID);
                result = DataProvider.SelectSP(LOAD_LIST_NEWS_NEWS, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable LoadTable(int PortalID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@PortalID", PortalID);
                result = DataProvider.SelectSP("News_getNews", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string LOAD_NEWSGROUP_BYNEWS = "News_getNewsGroupByNews";
        public DataTable GetNewsGroupByNews(int NewsID)
        {
            DataTable result = null;
            try
            {
                SqlParameter param = new SqlParameter("@NewsID", NewsID);
                result = DataProvider.SelectSP(LOAD_NEWSGROUP_BYNEWS, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public Dictionary<int, NewsInfo> GetNewsByCat(string CatID)
        {
            //string key = string.Format(DataCache.NewsCacheKey, CatID);
            //First Check the News Cache
            //Dictionary<int, NewsInfo> news = DataCache.GetCache(key) as Dictionary<int, NewsInfo>;
            //if (news == null)
            //{
                //Int32 timeOut = DataCache.NewsCacheTimeOut * Convert.ToInt32(Globals.PerformanceSetting);
                //news = FillNewsInfoDictionary(DataProvider.ExecuteReader("News_getNewsByCat", new object[] { CatID }));
                //Cache cats
                //if (timeOut > 0)
                //{
                //    DataCache.SetCache(key, news, TimeSpan.FromMinutes(timeOut));
                //}
            //}
            Dictionary<int, NewsInfo> news = FillNewsInfoDictionary(DataProvider.ExecuteReader("News_getNewsByCat", new object[] { CatID }));
            return news;
        }

        public DataTable GetNewsByCat_All(string CatID, int PortalID)
        {
            DataTable result = null;
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@CatID", CatID);
                param[1] = new SqlParameter("@PortalID", PortalID);
                result = DataProvider.SelectSP("News_GetNewsByCat_All", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public Dictionary<int, NewsInfo> GetNewsByPortal(int PortalID)
        {
            Dictionary<int, NewsInfo> news = FillNewsInfoDictionary(DataProvider.ExecuteReader("News_getNewsByPortal", new object[] { PortalID }));
            return news;
        }

        private static Dictionary<int, NewsInfo> FillNewsInfoDictionary(IDataReader dr)
        {
            Dictionary<int, NewsInfo> dic = new Dictionary<int, NewsInfo>();
            try
            {
                while (dr.Read())
                {
                    NewsInfo obj = FillNewsInfo(dr);
                    dic.Add(obj.ID, obj);
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

        private static NewsInfo FillNewsInfo(IDataReader dr)
        {
            NewsInfo objNewsInfo = new NewsInfo();

            objNewsInfo.AllowComment = Convert.ToBoolean(Null.SetNull(dr["AllowComment"], objNewsInfo.AllowComment));
            objNewsInfo.CatID = Convert.ToString(Null.SetNull(dr["CatID"], objNewsInfo.CatID));
            objNewsInfo.Content = Convert.ToString(Null.SetNull(dr["Content"], objNewsInfo.Content));
            objNewsInfo.CreatedDate = Convert.ToDateTime(Null.SetNull(dr["CreatedDate"], objNewsInfo.CreatedDate));
            objNewsInfo.CreateID = Convert.ToInt32(Null.SetNull(dr["CreateID"], objNewsInfo.CreateID));
            objNewsInfo.Description = Convert.ToString(Null.SetNull(dr["Description"], objNewsInfo.Description));
            objNewsInfo.Headline = Convert.ToString(Null.SetNull(dr["Headline"], objNewsInfo.Headline));
            objNewsInfo.ID = Convert.ToInt32(Null.SetNull(dr["ID"], objNewsInfo.ID));
            objNewsInfo.ImageUrl = Convert.ToString(Null.SetNull(dr["ImageUrl"], objNewsInfo.ImageUrl));
            objNewsInfo.KeyWords = Convert.ToString(Null.SetNull(dr["KeyWords"], objNewsInfo.KeyWords));
            objNewsInfo.ModifyDate = Convert.ToDateTime(Null.SetNull(dr["ModifyDate"], objNewsInfo.ModifyDate));
            objNewsInfo.ModifyID = Convert.ToInt32(Null.SetNull(dr["ModifyID"], objNewsInfo.ModifyID));
            objNewsInfo.Published = Convert.ToBoolean(Null.SetNull(dr["Published"], objNewsInfo.Published));
            objNewsInfo.Source = Convert.ToString(Null.SetNull(dr["Source"], objNewsInfo.Source));
            objNewsInfo.TotalView = Convert.ToInt32(Null.SetNull(dr["TotalView"], objNewsInfo.TotalView));
            if (dr["StartDate"] != DBNull.Value)
            {
                objNewsInfo.StartDate = Convert.ToDateTime(dr["StartDate"]);
            }
            else
            {
                objNewsInfo.StartDate = null;
            }
            if (dr["EndDate"] != DBNull.Value)
            {
                objNewsInfo.EndDate = Convert.ToDateTime(dr["EndDate"]);
            }
            else
            {
                objNewsInfo.EndDate = null;
            }
            if (dr["Feature"] != DBNull.Value)
            {
                objNewsInfo.Feature = Convert.ToInt32(dr["Feature"]);
            }
            else
            {
                objNewsInfo.Feature = 0;
            }
            return objNewsInfo;
        }

        #endregion

        private string GenTextML(DataTable dt, int imageWidth, int TimeOut, bool DisplayHeadline)
        {
            string returnText = "";
            returnText += "<SCRIPT TYPE='text/javascript'>ss = new slideshow('ss');";
            string strHeadline = "";
            string strDescription = "";
            string strImageUrl = "";
            string strLink = "";
            string strBullet = "";
            MLNewsInfo fMLNewsInfo;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int tabid = Convert.ToInt32(dt.Rows[i]["DesktopViewID"]);
                string link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "id/" + dt.Rows[i]["ID"].ToString());
                if (DisplayHeadline)
                {
                    int newsID = int.Parse(dt.Rows[i]["ID"].ToString());
                    fMLNewsInfo = MLNewsController.GetNews(newsID, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), false);
                    if (fMLNewsInfo != null)
                    {
                        if (fMLNewsInfo.MLHeadline != null)
                        {
                            strHeadline = fMLNewsInfo.MLHeadline.StringTextOrFallBack;
                        }
                        else
                        {
                            strHeadline = fMLNewsInfo.Headline;
                        }
                    }
                    else
                    {
                        strHeadline = dt.Rows[i]["Headline"].ToString();
                    }
                }
                if (dt.Rows[i]["ImageUrl"].ToString().LastIndexOf(';') > -1)
                {
                    strImageUrl = dt.Rows[i]["ImageUrl"].ToString().Substring(0, dt.Rows[i]["ImageUrl"].ToString().LastIndexOf(';'));
                }
                else if (dt.Rows[i]["ImageUrl"].ToString() != "")
                {
                    strImageUrl = dt.Rows[i]["ImageUrl"].ToString();
                }
                else
                {
                    strImageUrl = DotNetNuke.Common.Globals.ApplicationPath + "/images/noimage.gif";
                }
                strLink = link;

                returnText += "s = new slide();" +
                                    "s.src = '" + strImageUrl + "';" +
                                    "s.link = '" + strLink + "';" +
                                    "s.text = '" + strHeadline + "';" +
                                    "s.timeout = " + TimeOut.ToString() + ";" + 
                                    "ss.add_slide(s);";
            }
            returnText += "for (var i=0; i < ss.slides.length; i++) {" +
                            "s = ss.slides[i];" +
                            "s.target = '_self';" +
                            "s.attr = 'width=320,height=420,resizable=yes,scrollbars=yes';" +
                            "}" +
                            "</SCRIPT>" +
                            "<DIV ID='slideshow'>" +
                            "<DIV ID='ss_controls'>" +
                            "</DIV>" +
                            "<DIV ID='ss_img_div'>" +
                            "<A ID='ss_img_link' href='javascript:ss.hotlink()'>" +
                            "<IMG border='0' ID='ss_img' NAME='ss_img' STYLE='width:" + imageWidth.ToString() + "px;filter:progid:DXImageTransform.Microsoft.Fade();'>" +
                            "</A>" +
                            "</DIV>" +
                            "<DIV style='width:" + imageWidth.ToString() + "px;'><DIV align='center' class='NewsHeadline' ID='ss_text'>" +
                            "</DIV></DIV>" +
                            "</DIV>" +
                            "<SCRIPT TYPE='text/javascript'>" +
                            "if (document.images) {" +
                            "ss.image = document.images.ss_img;" +
                            "ss.textid = 'ss_text';" +
                            "ss.update();" +
                            "var fadein_opacity = 0.04;" +
                            "var fadein_img = ss.image;" +
                            "function fadein(opacity) {" +
                            "if (typeof opacity != 'undefined') { fadein_opacity = opacity; }" +
                            "if (fadein_opacity < 0.99 && fadein_img && fadein_img.style &&" +
                            "typeof fadein_img.style.MozOpacity != 'undefined') {" +
                            "fadein_opacity += .05;" +
                            "fadein_img.style.MozOpacity = fadein_opacity;" +
                            "setTimeout('fadein()', 100);" +
                            "}" +
                            "}" +
                            //"ss.post_update_hook = function() { fadein(0.04); };" +
                            "ss.shuffle();ss.play();" +
                            "}" +
                            "</SCRIPT>";
            return returnText;
        }

        private XmlDocument GenDocML(DataTable dt, string editFormatUrl, int imageWidth, int numShortNews)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml("<newslist></newslist>");
                XmlElement root = doc.DocumentElement;

                int shortNewsUpperBound = 0;
                if (numShortNews <= dt.Rows.Count)
                {
                    shortNewsUpperBound = numShortNews;
                }
                else
                {
                    shortNewsUpperBound = dt.Rows.Count;
                }
                
                XmlElement news;
                for (int i = 0; i < shortNewsUpperBound; i++)
                {
                    int newsID = int.Parse(dt.Rows[i]["ID"].ToString());
                    MLNewsController MLNewsController = new MLNewsController();
                    MLNewsInfo fMLNewsInfo = MLNewsController.GetNews(newsID, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), false);

                    if ((fMLNewsInfo == null || fMLNewsInfo.MLHeadline == null) && System.Threading.Thread.CurrentThread.CurrentCulture.ToString() != Common.Globals.GetPortalSettings().DefaultLanguage)
                    {
                        continue;
                    }

                    news = doc.CreateElement("firstnews");

                    int tabid = Convert.ToInt32(dt.Rows[i]["DesktopViewID"]);
                    string link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "id/" + dt.Rows[i]["ID"].ToString());
                    string url_edit = editFormatUrl.Replace("@@id", dt.Rows[i]["ID"].ToString());
                    if (fMLNewsInfo != null)
                    {
                        if (fMLNewsInfo.MLHeadline != null)
                        {
                            DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", fMLNewsInfo.MLHeadline.StringTextOrFallBack);
                        }
                        else
                        {
                            DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", fMLNewsInfo.Headline);
                        }
                        if (fMLNewsInfo.MLDescription != null)
                        {
                            DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Description", fMLNewsInfo.MLDescription.StringTextOrFallBack);
                        }
                        else
                        {
                            DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Description", fMLNewsInfo.Description);
                        }
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", dt.Rows[i]["Headline"].ToString());
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Description", dt.Rows[i]["Description"].ToString());
                    }
                    if (dt.Rows[i]["ImageUrl"].ToString().LastIndexOf(';') > -1)
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageUrl", dt.Rows[i]["ImageUrl"].ToString().Substring(0, dt.Rows[i]["ImageUrl"].ToString().LastIndexOf(';')));
                    }
                    else if (dt.Rows[i]["ImageUrl"].ToString() != "")
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageUrl", dt.Rows[i]["ImageUrl"].ToString());
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageUrl", DotNetNuke.Common.Globals.ApplicationPath + "/images/noimage.gif");
                    }

                    if (dt.Rows[i]["ImageUrl"].ToString() == "")
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageWidth", "1px");
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageWidth", imageWidth.ToString() + "px");
                    }
				    DateTime mdf = Convert.ToDateTime(dt.Rows[i]["ModifyDate"]);
				    if(mdf > DateTime.Now) DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ModifyDate", DateTime.Now.ToString("dd/MM/yyyy"));
				    else DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ModifyDate", mdf.ToString("dd/MM/yyyy"));

                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "link", link);
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "url_edit", url_edit);
                    if (i % 2 == 1)
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "odd", "true");
                    if (i == 0)
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "first", "true");
                    else
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "first", "false");
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Bullet", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "images/bullet.gif");

                    int Feature = Convert.ToInt32(dt.Rows[i]["Feature"]);
                    int New = Feature % 2;
                    int Hot = (Feature - New) / 2;
                    bool allow = true;
                    if (dt.Rows[i]["StartDate"] != DBNull.Value && DateTime.Now < (DateTime)dt.Rows[i]["StartDate"])
                    {
                        allow = false;
                    }
                    else if (dt.Rows[i]["EndDate"] != DBNull.Value && DateTime.Now > (DateTime)dt.Rows[i]["EndDate"])
                    {
                        allow = false;
                    }

                    if (New == 1 && allow)
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "New", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "/images/new.gif");
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "New", DotNetNuke.Common.Globals.ApplicationPath + "/images/transparent.gif");
                    }

                    if (Hot == 1 && allow)
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Hot", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "/images/hot.gif");
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Hot", DotNetNuke.Common.Globals.ApplicationPath + "/images/transparent.gif");
                    }

                    CategoryController catCont = new CategoryController();
                    CategoryInfo cat = catCont.Load(dt.Rows[i]["CatID"].ToString());
                    string catCode = cat.CatCode;
                    int startBooking = catCode.IndexOf("@@Booking_");
                    if (startBooking == 0)
                    {
                        string tempCode = catCode.Substring(10);
                        int stopBooking = tempCode.IndexOf(',');
                        string bookingPage = tempCode.Substring(0, stopBooking);
                        int bookingPageID = Convert.ToInt32(bookingPage);
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "BookingImage", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "images/booking_" + System.Threading.Thread.CurrentThread.CurrentCulture.ToString() + ".gif");
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "BookingLink", DotNetNuke.Common.Globals.NavigateURL(bookingPageID, "", "BookingID/" + newsID.ToString()));
                    }
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ReadMoreImage", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "images/readmore_" + System.Threading.Thread.CurrentThread.CurrentCulture.ToString() + ".gif");
				    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "STT", i.ToString());
                    root.AppendChild(news);
                }

                for (int i = shortNewsUpperBound; i < dt.Rows.Count; i++)
                {
                    int newsID = int.Parse(dt.Rows[i]["ID"].ToString());
                    MLNewsController MLNewsController = new MLNewsController();
                    MLNewsInfo fMLNewsInfo = MLNewsController.GetNews(newsID, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), false);

                    if ((fMLNewsInfo == null || fMLNewsInfo.MLHeadline == null) && System.Threading.Thread.CurrentThread.CurrentCulture.ToString() != Common.Globals.GetPortalSettings().DefaultLanguage)
                    {
                        continue;
                    }

                    news = doc.CreateElement("news");

                    int tabid = Convert.ToInt32(dt.Rows[i]["DesktopViewID"]);
                    string link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "id/" + dt.Rows[i]["ID"].ToString());
                    string url_edit = editFormatUrl.Replace("@@id", dt.Rows[i]["ID"].ToString());
                    if (fMLNewsInfo != null)
                    {
                        if (fMLNewsInfo.MLHeadline != null)
                        {
                            DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", fMLNewsInfo.MLHeadline.StringTextOrFallBack);
                        }
                        else
                        {
                            DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", fMLNewsInfo.Headline);
                        }
                        if (fMLNewsInfo.MLDescription != null)
                        {
                            DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Description", HttpContext.Current.Server.HtmlDecode(fMLNewsInfo.MLDescription.StringTextOrFallBack));
                        }
                        else
                        {
                            DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Description", HttpContext.Current.Server.HtmlDecode(fMLNewsInfo.Description));
                        }
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", dt.Rows[i]["Headline"].ToString());
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Description", HttpContext.Current.Server.HtmlDecode(dt.Rows[i]["Description"].ToString()));
                    }
                    if (dt.Rows[i]["ImageUrl"].ToString().LastIndexOf(';') > -1)
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageUrl", dt.Rows[i]["ImageUrl"].ToString().Substring(0, dt.Rows[i]["ImageUrl"].ToString().LastIndexOf(';')));
                    }
                    else if (dt.Rows[i]["ImageUrl"].ToString() != "")
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageUrl", dt.Rows[i]["ImageUrl"].ToString());
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageUrl", DotNetNuke.Common.Globals.ApplicationPath + "/images/noimage.gif");
                    }
                    if (dt.Rows[i]["ImageUrl"].ToString() == "")
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageWidth", "1px");
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageWidth", imageWidth.ToString() + "px");
                    }
				    DateTime mdf = Convert.ToDateTime(dt.Rows[i]["ModifyDate"]);
				    if(mdf > DateTime.Now) DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ModifyDate", DateTime.Now.ToString("dd/MM/yyyy"));
				    else DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ModifyDate", mdf.ToString("dd/MM/yyyy"));
                    //DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ModifyDate", dt.Rows[i]["ModifyDate"].ToString());

                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "link", link);
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "url_edit", url_edit);
                    if (i % 2 == 1)
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "odd", "true");
                    if (i == shortNewsUpperBound)
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "first", "true");
                    else
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "first", "false");
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Bullet", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "images/bullet.gif");

                    int Feature = Convert.ToInt32(dt.Rows[i]["Feature"]);
                    int New = Feature % 2;
                    int Hot = (Feature - New) / 2;
                    bool allow = true;
                    if (dt.Rows[i]["StartDate"] != DBNull.Value && DateTime.Now < (DateTime)dt.Rows[i]["StartDate"])
                    {
                        allow = false;
                    }
                    else if (dt.Rows[i]["EndDate"] != DBNull.Value && DateTime.Now > (DateTime)dt.Rows[i]["EndDate"])
                    {
                        allow = false;
                    }

                    if (New == 1 && allow)
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "New", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "images/new.gif");
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "New", DotNetNuke.Common.Globals.ApplicationPath + "/images/transparent.gif");
                    }

                    if (Hot == 1 && allow)
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Hot", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "images/hot.gif");
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Hot", DotNetNuke.Common.Globals.ApplicationPath + "/images/transparent.gif");
                    }

                    CategoryController catCont = new CategoryController();
                    CategoryInfo cat = catCont.Load(dt.Rows[i]["CatID"].ToString());
                    string catCode = cat.CatCode;
                    int startBooking = catCode.IndexOf("@@Booking_");
                    if (startBooking == 0)
                    {
                        string tempCode = catCode.Substring(10);
                        int stopBooking = tempCode.IndexOf(',');
                        string bookingPage = tempCode.Substring(0, stopBooking);
                        int bookingPageID = Convert.ToInt32(bookingPage);
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "BookingImage", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "images/booking_" + System.Threading.Thread.CurrentThread.CurrentCulture.ToString() + ".gif");
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "BookingLink", DotNetNuke.Common.Globals.NavigateURL(bookingPageID, "", "BookingID/" + newsID.ToString()));
                    }
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ReadMoreImage", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "images/readmore_" + System.Threading.Thread.CurrentThread.CurrentCulture.ToString() + ".gif");
				    int STT = i - shortNewsUpperBound;
				    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "STT", STT.ToString());
                    root.AppendChild(news);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            return doc;
        }

        private XmlDocument GenDoc(DataTable dt, string editFormatUrl)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<newslist></newslist>");
            XmlElement root = doc.DocumentElement;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                XmlElement news = doc.CreateElement("news");

                int tabid = Convert.ToInt32(dt.Rows[i]["DesktopViewID"]);
                string link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "id/" + dt.Rows[i]["ID"].ToString());
                string url_edit = editFormatUrl.Replace("@@id", dt.Rows[i]["ID"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "CatName", dt.Rows[i]["CatName"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", dt.Rows[i]["Headline"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Description", dt.Rows[i]["Description"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImgEdit", DotNetNuke.Common.Globals.ApplicationPath + "/images/edit.gif");
                if (dt.Rows[i]["ImageUrl"].ToString().LastIndexOf(';') > -1)
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageUrl", dt.Rows[i]["ImageUrl"].ToString().Substring(0, dt.Rows[i]["ImageUrl"].ToString().LastIndexOf(';')));
                }
                else if (dt.Rows[i]["ImageUrl"].ToString() != "")
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageUrl", dt.Rows[i]["ImageUrl"].ToString());
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageUrl", DotNetNuke.Common.Globals.ApplicationPath + "/images/noimage.gif");
                }
                if (dt.Rows[i]["ImageUrl"].ToString() == "")
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageWidth", "1px");
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageWidth", "70px");
                }
				DateTime mdf = Convert.ToDateTime(dt.Rows[i]["ModifyDate"]);
				if(mdf > DateTime.Now) DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ModifyDate", DateTime.Now.ToString("dd/MM/yyyy"));
				else DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ModifyDate", mdf.ToString("dd/MM/yyyy"));
                //DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ModifyDate", dt.Rows[i]["ModifyDate"].ToString());

                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "link", link);
                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "url_edit", url_edit);
                if (i % 2 == 1)
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "odd", "true");

                int Feature = Convert.ToInt32(dt.Rows[i]["Feature"]);
                int New = Feature % 2;
                int Hot = (Feature - New) / 2;
                bool allow = true;
                if (dt.Rows[i]["StartDate"] != DBNull.Value && DateTime.Now < (DateTime)dt.Rows[i]["StartDate"])
                {
                    allow = false;
                }
                else if (dt.Rows[i]["EndDate"] != DBNull.Value && DateTime.Now > (DateTime)dt.Rows[i]["EndDate"])
                {
                    allow = false;
                }

                if (New == 1 && allow)
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "New", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "images/new.gif");
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "New", DotNetNuke.Common.Globals.ApplicationPath + "/images/transparent.gif");
                }

                if (Hot == 1 && allow)
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Hot", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "images/hot.gif");
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Hot", DotNetNuke.Common.Globals.ApplicationPath + "/images/transparent.gif");
                }

                root.AppendChild(news);
            }

            return doc;
        }

        private XmlDocument GenDocForShared(DataTable dt, string editFormatUrl, int PortalID)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<newslist></newslist>");
            XmlElement root = doc.DocumentElement;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                XmlElement news = doc.CreateElement("news");

                int tabid = Convert.ToInt32(dt.Rows[i]["DesktopViewID"]);
                string link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "id/" + dt.Rows[i]["ID"].ToString());
                string url_edit = editFormatUrl.Replace("@@id", dt.Rows[i]["ID"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "PortalName", dt.Rows[i]["PortalName"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "CatName", dt.Rows[i]["CatName"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", dt.Rows[i]["Headline"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Description", dt.Rows[i]["Description"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImgEdit", DotNetNuke.Common.Globals.ApplicationPath + "/images/edit.gif");
                if (dt.Rows[i]["ImageUrl"].ToString().LastIndexOf(';') > -1)
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageUrl", dt.Rows[i]["ImageUrl"].ToString().Substring(0, dt.Rows[i]["ImageUrl"].ToString().LastIndexOf(';')));
                }
                else if (dt.Rows[i]["ImageUrl"].ToString() != "")
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageUrl", dt.Rows[i]["ImageUrl"].ToString());
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageUrl", DotNetNuke.Common.Globals.ApplicationPath + "/images/noimage.gif");
                }

                string sSource = dt.Rows[i]["Source"].ToString();
                if (sSource.Contains("@" + PortalID.ToString() + "_"))
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "IsCopied", "true");
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "IsCopied", "false");
                }

                if (dt.Rows[i]["ImageUrl"].ToString() == "")
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageWidth", "1px");
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ImageWidth", "70px");
                }
				DateTime mdf = Convert.ToDateTime(dt.Rows[i]["ModifyDate"]);
				if(mdf > DateTime.Now) DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ModifyDate", DateTime.Now.ToString("dd/MM/yyyy"));
				else DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ModifyDate", mdf.ToString("dd/MM/yyyy"));
                //DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "ModifyDate", dt.Rows[i]["ModifyDate"].ToString());

                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "link", link);
                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "url_edit", url_edit);
                if (i % 2 == 1)
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "odd", "true");

                root.AppendChild(news);
            }

            return doc;
        }

        private XmlDocument GenDocRelative(DataTable dt, string editFormatUrl)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<newslist></newslist>");
            XmlElement root = doc.DocumentElement;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int newsID = int.Parse(dt.Rows[i]["ID"].ToString());
                MLNewsController MLNewsController = new MLNewsController();
                MLNewsInfo fMLNewsInfo = MLNewsController.GetNews(newsID, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), false);
                XmlElement news = doc.CreateElement("news");

                int tabid = Convert.ToInt32(dt.Rows[i]["DesktopViewID"]);
                string link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "id/" + dt.Rows[i]["ID"].ToString());
                string url_edit = editFormatUrl.Replace("@@id", dt.Rows[i]["ID"].ToString());

                if (fMLNewsInfo != null)
                {
                    if (fMLNewsInfo.MLHeadline != null)
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", fMLNewsInfo.MLHeadline.StringTextOrFallBack);
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", fMLNewsInfo.Headline);
                    }
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", dt.Rows[i]["Headline"].ToString());
                }
                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Bullet", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "images/bullet.gif");
                
                int Feature = Convert.ToInt32(dt.Rows[i]["Feature"]);
                int New = Feature % 2;
                int Hot = (Feature - New) / 2;
                bool allow = true;
                if (dt.Rows[i]["StartDate"] != DBNull.Value && DateTime.Now < (DateTime)dt.Rows[i]["StartDate"])
                {
                    allow = false;
                }
                else if (dt.Rows[i]["EndDate"] != DBNull.Value && DateTime.Now > (DateTime)dt.Rows[i]["EndDate"])
                {
                    allow = false;
                }

                if (New == 1 && allow)
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "New", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "/images/new.gif");
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "New", DotNetNuke.Common.Globals.ApplicationPath + "/images/transparent.gif");
                }

                if (Hot == 1 && allow)
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Hot", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "/images/hot.gif");
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Hot", DotNetNuke.Common.Globals.ApplicationPath + "/images/transparent.gif");
                }

                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "link", link);
                root.AppendChild(news);
            }

            return doc;
        }

        private XmlDocument GenDocRelative(string catID, DataTable dt, string editFormatUrl)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<newslist></newslist>");
            XmlElement root = doc.DocumentElement;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int newsID = int.Parse(dt.Rows[i]["ID"].ToString());
                MLNewsController MLNewsController = new MLNewsController();
                MLNewsInfo fMLNewsInfo = MLNewsController.GetNews(newsID, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), false);
                if ((fMLNewsInfo == null || fMLNewsInfo.MLHeadline == null) && System.Threading.Thread.CurrentThread.CurrentCulture.ToString() != Common.Globals.GetPortalSettings().DefaultLanguage)
                {
                    continue;
                }

                XmlElement news = doc.CreateElement("news");

                int tabid = Convert.ToInt32(dt.Rows[i]["DesktopViewID"]);
                string link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "id/" + dt.Rows[i]["ID"].ToString());
                string url_edit = editFormatUrl.Replace("@@id", dt.Rows[i]["ID"].ToString());
                if (fMLNewsInfo != null)
                {
                    if (fMLNewsInfo.MLHeadline != null)
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", fMLNewsInfo.MLHeadline.StringTextOrFallBack);
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", fMLNewsInfo.Headline);
                    }
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Headline", dt.Rows[i]["Headline"].ToString());
                }
                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Bullet", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "images/bullet.gif");

                int Feature = Convert.ToInt32(dt.Rows[i]["Feature"]);
                int New = Feature % 2;
                int Hot = (Feature - New) / 2;
                bool allow = true;
                if (dt.Rows[i]["StartDate"] != DBNull.Value && DateTime.Now < (DateTime)dt.Rows[i]["StartDate"])
                {
                    allow = false;
                }
                else if (dt.Rows[i]["EndDate"] != DBNull.Value && DateTime.Now > (DateTime)dt.Rows[i]["EndDate"])
                {
                    allow = false;
                }

                if (New == 1 && allow)
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "New", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "/images/new.gif");
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "New", DotNetNuke.Common.Globals.ApplicationPath + "/images/transparent.gif");
                }

                if (Hot == 1 && allow)
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Hot", DotNetNuke.Common.Globals.GetPortalSettings().HomeDirectory + "/images/hot.gif");
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "Hot", DotNetNuke.Common.Globals.ApplicationPath + "/images/transparent.gif");
                }

                DotNetNuke.NewsProvider.Utils.AddNode(doc, news, "link", link);
                root.AppendChild(news);
            }

            return doc;
        }

        private XmlDocument GenDocComment(DataTable dt)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<commentlist></commentlist>");
            XmlElement root = doc.DocumentElement;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                XmlElement comment = doc.CreateElement("comment");
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "Author", dt.Rows[i]["Author"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "AuthorEmail", dt.Rows[i]["AuthorEmail"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "Headline", dt.Rows[i]["Headline"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "Content", dt.Rows[i]["Content"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "CreatedDate", dt.Rows[i]["CreatedDate"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "ImgEdit", DotNetNuke.Common.Globals.ApplicationPath + "/images/edit.gif");

                if (i % 2 == 1)
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "odd", "true");
                root.AppendChild(comment);
            }
            return doc;
        }

        private XmlDocument GenDocComment(DataTable dt, string deleteFormatUrl)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<commentlist></commentlist>");
            XmlElement root = doc.DocumentElement;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                XmlElement comment = doc.CreateElement("comment");
                string url_delete = deleteFormatUrl.Replace("@@id", dt.Rows[i]["ID"].ToString()).Replace("@@action", "delete");
                string url_changestatus = deleteFormatUrl.Replace("@@id", dt.Rows[i]["ID"].ToString()).Replace("@@action", "changestatus");
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "Author", dt.Rows[i]["Author"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "AuthorEmail", dt.Rows[i]["AuthorEmail"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "Headline", dt.Rows[i]["Headline"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "NewsHeadline", dt.Rows[i]["NewsHeadline"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "CatName", dt.Rows[i]["CatName"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "Content", dt.Rows[i]["Content"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "CreatedDate", dt.Rows[i]["CreatedDate"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "ClientIPAddress", dt.Rows[i]["ClientIPAddress"] == DBNull.Value?"": dt.Rows[i]["ClientIPAddress"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "ClientHostName", dt.Rows[i]["ClientHostName"] == DBNull.Value?"": dt.Rows[i]["ClientHostName"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "ImgDelete", DotNetNuke.Common.Globals.ApplicationPath + "/images/delete.gif");
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "url_delete", url_delete);
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "url_changestatus", url_changestatus);
                if (i % 2 == 1)
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "odd", "true");
                if (Convert.ToInt32(dt.Rows[i]["Status"]) == 1)
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "ImgStatus", DotNetNuke.Common.Globals.ApplicationPath + "/images/checked.gif");
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "ImgStatus", DotNetNuke.Common.Globals.ApplicationPath + "/images/unchecked.gif");
                }
                DotNetNuke.NewsProvider.Utils.AddNode(doc, comment, "ImgChangeStatus", DotNetNuke.Common.Globals.ApplicationPath + "/images/refresh.gif");
                root.AppendChild(comment);
            }
            return doc;
        }

        public XmlDocument LoadAdminXML(string editFormatUrl, int pg, int pagesize, string CatID, int PortalID)
        {
            string strSQL = "News_getShortNewsFull";
            int start = pg * pagesize;
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@CatID", CatID);
            param[1] = new SqlParameter("@PortalID", PortalID);
            DataTable dt = DotNetNuke.NewsProvider.DataProvider.LoadPageSP(strSQL, start, pagesize, param);
            return GenDoc(dt, editFormatUrl);
        }

        public XmlDocument LoadAdminXMLByCatByDate(string editFormatUrl, int pg, int pagesize, string CatID, int PortalID, DateTime? fromDate, DateTime? toDate, string Search)
        {
            string strSQL = "News_getShortNewsFullByCatByDate";
            int start = pg * pagesize;
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@CatID", CatID);
            //param[1] = new SqlParameter("@FromDate", fromDate==null ? (DateTime?)DBNull.Value : fromDate);
            //param[2] = new SqlParameter("@ToDate", toDate == null ? (DateTime?)DBNull.Value : toDate);
            if (fromDate == null)
            {
                param[1] = new SqlParameter("@FromDate", DBNull.Value);
            }
            else
            {
                param[1] = new SqlParameter("@FromDate", fromDate);
            }
            if (toDate == null)
            {
                param[2] = new SqlParameter("@ToDate", DBNull.Value);
            }
            else
            {
                param[2] = new SqlParameter("@ToDate", toDate);
            }
            param[3] = new SqlParameter("@PortalID", PortalID);
            param[4] = new SqlParameter("@Search", Search);
            DataTable dt = DotNetNuke.NewsProvider.DataProvider.LoadPageSP(strSQL, start, pagesize, param);
            return GenDoc(dt, editFormatUrl);
        }

        public XmlDocument LoadAdminSharedXMLByNewsGroup(string editFormatUrl, string newsgroupid, int portalID, int pg, int pagesize)
        {
            int start = pagesize * pg;
            string strSQL = "News_getSharedShortNewsByGroup";
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@NewsGroupID", newsgroupid);
            param[1] = new SqlParameter("@PortalID", portalID);
            DataTable dt = DataProvider.LoadPageSP(strSQL, start, pagesize, param);
            return GenDocForShared(dt, editFormatUrl, portalID);
        }

        //public XmlDocument LoadXML(string categoryid, int pg, int pagesize, int imageWidth, int numShortNews)
        //{
        //    int start = pagesize * pg;
        //    string strSQL = "News_getShortNewsByCat";
        //    SqlParameter p = new SqlParameter("@CatID", categoryid);
        //    DataTable dt = DataProvider.LoadPageSP(strSQL, start, pagesize, p);
        //    return GenDocML(dt, "", imageWidth, numShortNews);
        //}

        public XmlDocument LoadXML(string categoryid, int pg, int pagesize, int imageWidth, int numShortNews)
        {
            //int start = pagesize * pg;
            string strSQL = "News_getShortNewsByCat";
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@CatID", categoryid);
            //p[1] = new SqlParameter("@StartIndex", start);
            p[1] = new SqlParameter("@StartIndex", pg);
            p[2] = new SqlParameter("@PageSize", pagesize);
            DataTable dt = DataProvider.SelectSP(strSQL, p);
            return GenDocML(dt, "", imageWidth, numShortNews);
        }

        public string LoadText(string categoryid, int pagesize, int imageWidth, int timeOut, bool displayHeadline)
        {
            string strSQL = "News_getShortNewsByCat";
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@CatID", categoryid);
            p[1] = new SqlParameter("@StartIndex", 0);
            p[2] = new SqlParameter("@PageSize", pagesize);
            DataTable dt = DataProvider.SelectSP(strSQL, p);
            return GenTextML(dt, imageWidth, timeOut, displayHeadline);
        }

        public XmlDocument LoadXMLByNewsGroup(string newsgroupid, int pg, int pagesize, int imageWidth, int numShortNews)
        {
            int start = pagesize * pg;
            string strSQL = "News_getShortNewsByGroup";
            SqlParameter p = new SqlParameter("@NewsGroupID", newsgroupid);
            DataTable dt = DataProvider.LoadPageSP(strSQL, start, pagesize, p);
            return GenDocML(dt, "", imageWidth, numShortNews);
        }

        public string LoadTextByNewsGroup(string newsgroupid, int pagesize, int imageWidth, int timeOut, bool displayHeadline)
        {
            string strSQL = "News_getShortNewsByGroup";
            SqlParameter p = new SqlParameter("@NewsGroupID", newsgroupid);
            DataTable dt = DataProvider.LoadPageSP(strSQL, 0, pagesize, p);
            return GenTextML(dt, imageWidth, timeOut, displayHeadline);
        }

        public XmlDocument LoadXMLByCatCode(int portalid, string categorycode, int pg, int pagesize, out int numrecord, int imageWidth, int numShortNews)
        {
            int NumRecord = 0;
            int start = pagesize * pg;
            string strSQL = "News_getShortNewsByCatCode";
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@PortalID", portalid);
            p[1] = new SqlParameter("@Codes", categorycode);
            p[2] = new SqlParameter("@NumRecord", NumRecord);
            p[2].Direction = ParameterDirection.Output;
            DataTable dt = DataProvider.LoadPageSP(strSQL, start, pagesize, p);
            numrecord = int.Parse(p[2].Value.ToString());
            return GenDocML(dt, "", imageWidth, numShortNews);
        }

        public string LoadTextByCatCode(int portalid, string categorycode, int pagesize, int imageWidth, int timeOut, bool displayHeadline)
        {
            int NumRecord = 0;
            string strSQL = "News_getShortNewsByCatCode";
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@PortalID", portalid);
            p[1] = new SqlParameter("@Codes", categorycode);
            p[2] = new SqlParameter("@NumRecord", NumRecord);
            p[2].Direction = ParameterDirection.Output;
            DataTable dt = DataProvider.LoadPageSP(strSQL, 0, pagesize, p);
            return GenTextML(dt, imageWidth, timeOut, displayHeadline);
        }

        public XmlDocument LoadXMLByGroupCode(int portalid, string groupcode, int pg, int pagesize, out int numrecord, int imageWidth, int numShortNews)
        {
            int NumRecord = 0;
            int start = pagesize * pg;
            string strSQL = "News_getShortNewsByGroupCode";
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@PortalID", portalid);
            p[1] = new SqlParameter("@Codes", groupcode);
            p[2] = new SqlParameter("@NumRecord", NumRecord);
            p[2].Direction = ParameterDirection.Output;
            DataTable dt = DataProvider.LoadPageSP(strSQL, start, pagesize, p);
            numrecord = int.Parse(p[2].Value.ToString());
            return GenDocML(dt, "", imageWidth, numShortNews);
        }

        public string LoadTextByGroupCode(int portalid, string groupcode, int pagesize, int imageWidth, int timeOut, bool displayHeadline)
        {
            int NumRecord = 0;
            string strSQL = "News_getShortNewsByGroupCode";
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@PortalID", portalid);
            p[1] = new SqlParameter("@Codes", groupcode);
            p[2] = new SqlParameter("@NumRecord", NumRecord);
            p[2].Direction = ParameterDirection.Output;
            DataTable dt = DataProvider.LoadPageSP(strSQL, 0, pagesize, p);
            return GenTextML(dt, imageWidth, timeOut, displayHeadline);
        }

        public XmlDocument LoadHotXML(int pagesize, string CatID, int PortalID, int imageWidth, int numShortNews)
        {
            string strSQL = "News_getShortNewsFullOrderTotalView";
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@CatID", CatID);
            param[1] = new SqlParameter("@PortalID", PortalID);
            DataTable dt = DataProvider.LoadPageSP(strSQL, 0, pagesize, param);
            return GenDocML(dt, "", imageWidth, numShortNews);
        }

        public string LoadHotText(int pagesize, string CatID, int PortalID, int imageWidth, int timeOut, bool displayHeadline)
        {
            string strSQL = "News_getShortNewsFullOrderTotalView";
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@CatID", CatID);
            param[1] = new SqlParameter("@PortalID", PortalID);
            DataTable dt = DataProvider.LoadPageSP(strSQL, 0, pagesize, param);
            return GenTextML(dt, imageWidth, timeOut, displayHeadline);
        }

        //public XmlDocument LoadNewestXML(int pagesize, string CatID, int PortalID, int imageWidth, int numShortNews)
        //{
        //    string strSQL = "News_getShortNewsFullOrderNewest";
        //    SqlParameter[] param = new SqlParameter[2];
        //    param[0] = new SqlParameter("@CatID", CatID);
        //    param[1] = new SqlParameter("@PortalID", PortalID);
        //    DataTable dt = DataProvider.LoadPageSP(strSQL, 0, pagesize, param);
        //    return GenDocML(dt, "", imageWidth, numShortNews);
        //}

        public XmlDocument LoadNewestXML(int pagesize, string CatID, int PortalID, int imageWidth, int numShortNews)
        {
            if (System.Threading.Thread.CurrentThread.CurrentCulture.ToString() != Common.Globals.GetPortalSettings().DefaultLanguage)
            {
                string strSQL = "News_getShortNewsFullOrderNewest_OtherLanguage";
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@CatID", CatID);
                param[1] = new SqlParameter("@PortalID", PortalID);
                param[2] = new SqlParameter("@PageSize", pagesize);
                param[3] = new SqlParameter("@Locale", System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                DataTable dt = DataProvider.SelectSP(strSQL, param);
                return GenDocML(dt, "", imageWidth, numShortNews);
            }
            else
            {
                string strSQL = "News_getShortNewsFullOrderNewest";
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@CatID", CatID);
                param[1] = new SqlParameter("@PortalID", PortalID);
                param[2] = new SqlParameter("@PageSize", pagesize);
                DataTable dt = DataProvider.SelectSP(strSQL, param);
                return GenDocML(dt, "", imageWidth, numShortNews);
            }
        }

        public XmlDocument LoadXMLByIDArray(string IDArray, int imageWidth, int numShortNews)
        {
            string strSQL = "News_getShortNewsByIDArray";
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@IDArray", IDArray);
            DataTable dt = DataProvider.SelectSP(strSQL, param);
            return GenDocML(dt, "", imageWidth, numShortNews);
        }

        public string LoadNewestText(int pagesize, string CatID, int PortalID, int imageWidth, int timeOut, bool displayHeadline)
        {
            string strSQL = "News_getShortNewsFullOrderNewest";
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@CatID", CatID);
            param[1] = new SqlParameter("@PortalID", PortalID);
            param[2] = new SqlParameter("@PageSize", pagesize);
            DataTable dt = DataProvider.SelectSP(strSQL, param);
            return GenTextML(dt, imageWidth, timeOut, displayHeadline);
        }

        public XmlDocument LoadXMLBySearch(int portalid, string search, int imageWidth, int numShortNews)
        {
            string strSQL = "News_getShortNewsBySearch";
            SqlParameter[] p = new SqlParameter[2];
            p[0] = new SqlParameter("@PortalID", portalid);
            p[1] = new SqlParameter("@Search", search);
            DataTable dt = DataProvider.SelectSP(strSQL, p);
            return GenDocML(dt, "", imageWidth, numShortNews);
        }

        public void UpdateTotalView(int id)
        {
            string strSQL = "News_updateNewsTotalView";
            SqlParameter p = new SqlParameter("@ID", id);
            DotNetNuke.NewsProvider.DataProvider.ExecuteSP(strSQL, p);
        }

        public XmlDocument LoadNewsXML(int newsid, int imageWidth, int numShortNews)
        {
            string strSQL = "News_getShortNewsByID";
            SqlParameter p = new SqlParameter("@ID", newsid);
            DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, p);
            return GenDocML(dt, "", imageWidth, numShortNews);
        }

        public XmlDocument LoadXML_RelativeNews(string categoryid, string newsid, int pagesize, out int numRecord)
        {
            int NumRecord = 0;
            string strSQL = "News_GetRelativeNews";
            SqlParameter[] p = new SqlParameter[4];
            p[0] = new SqlParameter("@CatID", categoryid);
            p[1] = new SqlParameter("@NewsID", int.Parse(newsid));
            p[2] = new SqlParameter("@Top", pagesize/2);
            p[3] = new SqlParameter("@NumRecord", NumRecord);
            p[3].Direction = ParameterDirection.Output;
            DataTable dt = DataProvider.LoadPageSP(strSQL, 0, pagesize, p);
            numRecord = int.Parse(p[3].Value.ToString());
            return GenDocRelative(categoryid, dt, "");
        }

        public XmlDocument LoadXML_RelativeNewsKeyWords(string newsid, out int numRecord)
        {
            int NumRecord = 0;
            string strSQL = "News_GetRelativeNewsKeyWords";
            SqlParameter[] p = new SqlParameter[2];
            p[0] = new SqlParameter("@NewsID", int.Parse(newsid));
            p[1] = new SqlParameter("@NumRecord", NumRecord);
            p[1].Direction = ParameterDirection.Output;
            DataTable dt = DataProvider.SelectSP(strSQL, p);
            numRecord = int.Parse(p[1].Value.ToString());
            return GenDocRelative(dt, "");
        }

        public XmlDocument LoadCommentXML(int newsID, int Status, out int numRecord)
        {
            int NumRecord = 0;
            string strSQL = "News_getCommentByNewsByStatus";
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@NewsID", newsID);
            p[1] = new SqlParameter("@Status", Status);
            p[2] = new SqlParameter("@NumRecord", NumRecord);
            p[2].Direction = ParameterDirection.Output;
            DataTable dt = DataProvider.SelectSP(strSQL,p);
            numRecord = int.Parse(p[2].Value.ToString());
            return GenDocComment(dt);
        }

        public XmlDocument LoadPageCommentXML(string deleteFormatUrl, int newsID, int pg, int pagesize, out int NumRecord)
        {
            NumRecord = 0;
            int start = pg * pagesize;
            string strSQL = "News_getCommentByNews";
            SqlParameter[] p = new SqlParameter[2];
            p[0] = new SqlParameter("@NewsID", newsID);
            p[1] = new SqlParameter("@NumRecord", NumRecord);
            p[1].Direction = ParameterDirection.Output;
            DataTable dt = DataProvider.LoadPageSP(strSQL, start, pagesize, p);
            NumRecord = int.Parse(p[4].Value.ToString());
            return GenDocComment(dt, deleteFormatUrl);
        }

        public XmlDocument LoadPageCommentXML_All(string deleteFormatUrl, int newsID, string CatID, int PortalID, int Status, int pg, int pagesize, out int NumRecord)
        {
            NumRecord = 0;
            int start = pg * pagesize;
            string strSQL = "News_getCommentByNews_All";
            SqlParameter[] p = new SqlParameter[5];
            p[0] = new SqlParameter("@NewsID", newsID);
            p[1] = new SqlParameter("@CatID", CatID);
            p[2] = new SqlParameter("@PortalID", PortalID);
            p[3] = new SqlParameter("@Status", Status);
            p[4] = new SqlParameter("@NumRecord", NumRecord);
            p[4].Direction = ParameterDirection.Output;
            DataTable dt = DataProvider.LoadPageSP(strSQL, start, pagesize, p);
            NumRecord = int.Parse(p[4].Value.ToString());
            return GenDocComment(dt, deleteFormatUrl);
        }

        private const string MENU_GETFILE = "Menu_GetFileById";
        public DataTable GetFileInfo(int FileID)
        {
            DataTable result = null;
            try
            {
                SqlParameter param = new SqlParameter("@FileID", FileID);
                result = DataProvider.SelectSP(MENU_GETFILE, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private string GenTextMLAsYahooSlideshow(DataTable dt, int thumbnailHeight, int thumbnailWidth, int titleLength, int descLength)
        {
            string returnText = "";
            returnText += @"<ul id=""slideshow"">";
            string strHeadline = "";
            string strDescription = "";
            string strImageUrl = "";
            string strLink = "";
            MLNewsInfo fMLNewsInfo;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int tabid = Convert.ToInt32(dt.Rows[i]["DesktopViewID"]);
                string link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "id/" + dt.Rows[i]["ID"].ToString());
                int newsID = int.Parse(dt.Rows[i]["ID"].ToString());
                fMLNewsInfo = MLNewsController.GetNews(newsID, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), false);
                if (fMLNewsInfo != null)
                {
                    if (fMLNewsInfo.MLHeadline != null)
                    {
                        strHeadline = fMLNewsInfo.MLHeadline.StringTextOrFallBack;
                        strDescription = fMLNewsInfo.MLDescription.StringTextOrFallBack;
                    }
                    else
                    {
                        strHeadline = fMLNewsInfo.Headline;
                        strDescription = fMLNewsInfo.Description;
                    }
                }
                else
                {
                    strHeadline = dt.Rows[i]["Headline"].ToString();
                    strDescription = dt.Rows[i]["Description"].ToString();
                }
                if (dt.Rows[i]["ImageUrl"].ToString().LastIndexOf(';') > -1)
                {
                    strImageUrl = dt.Rows[i]["ImageUrl"].ToString().Substring(0, dt.Rows[i]["ImageUrl"].ToString().LastIndexOf(';'));
                }
                else if (dt.Rows[i]["ImageUrl"].ToString() != "")
                {
                    strImageUrl = dt.Rows[i]["ImageUrl"].ToString();
                }
                else
                {
                    strImageUrl = DotNetNuke.Common.Globals.ApplicationPath + "/images/noimage_large.gif";
                }
                strLink = link;
                string realHeadline = strHeadline.Length < titleLength ? strHeadline : strHeadline.Substring(0, titleLength).Substring(0, strHeadline.Substring(0, titleLength).LastIndexOf(' ')) + "...";
                string realDescription1 = System.Text.RegularExpressions.Regex.Replace(strDescription, "<[^<>]*>", "");
                string realDescription = realDescription1.Length < descLength ? realDescription1 : realDescription1.Substring(0, descLength).Substring(0, realDescription1.Substring(0, descLength).LastIndexOf(' ')) + "...";
                returnText += "<li>" +
                                    "<h3>" + realHeadline + "</h3>" +
                                    "<span>" + strImageUrl + "</span>" +
                                    "<p style='text-align:justify;'>" + realDescription + "</p>" +
                                    "<div style='overflow:hidden;display:block;float:left;width:98%;margin-top:10px;margin-bottom:10px;height:" + ((int)(thumbnailHeight + 7)).ToString() + "px;'>" +
                                    "<img src='" + strImageUrl + "' alt='" + strHeadline + "' style='float:left;margin-right:5px;width:" + thumbnailWidth.ToString() + "px;height:" + thumbnailHeight.ToString() + "px;'/>" +
                                    "<span style='color:white;text-align:justify;cursor:pointer;height:" + thumbnailHeight.ToString() + "px;'>" +
                                    realHeadline + "</span>" +
                                    "<span style='display:none;'>" + strLink + "</span>" +
                                    "</div></li>";
            }
            returnText += "</ul>";
            return returnText;
        }


        public string LoadTextAsYahooSlideshow(string categoryid, int pagesize, int thumbnailHeight, int thumbnailWidth, int titleLength, int descLength)
        {
            string strSQL = "News_getShortNewsByCat";
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@CatID", categoryid);
            p[1] = new SqlParameter("@StartIndex", 0);
            p[2] = new SqlParameter("@PageSize", pagesize);
            DataTable dt = DataProvider.SelectSP(strSQL, p);
            return GenTextMLAsYahooSlideshow(dt, thumbnailHeight, thumbnailWidth, titleLength, descLength);
        }


        public string LoadTextByNewsGroupAsYahooSlideshow(string newsgroupid, int pagesize, int thumbnailHeight, int thumbnailWidth, int titleLength, int descLength)
        {
            string strSQL = "News_getShortNewsByGroup";
            SqlParameter p = new SqlParameter("@NewsGroupID", newsgroupid);
            DataTable dt = DataProvider.LoadPageSP(strSQL, 0, pagesize, p);
            return GenTextMLAsYahooSlideshow(dt, thumbnailHeight, thumbnailWidth, titleLength, descLength);
        }


        public string LoadTextByCatCodeAsYahooSlideshow(int portalid, string categorycode, int pagesize, int thumbnailHeight, int thumbnailWidth, int titleLength, int descLength)
        {
            int NumRecord = 0;
            string strSQL = "News_getShortNewsByCatCode";
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@PortalID", portalid);
            p[1] = new SqlParameter("@Codes", categorycode);
            p[2] = new SqlParameter("@NumRecord", NumRecord);
            p[2].Direction = ParameterDirection.Output;
            DataTable dt = DataProvider.LoadPageSP(strSQL, 0, pagesize, p);
            return GenTextMLAsYahooSlideshow(dt, thumbnailHeight, thumbnailWidth, titleLength, descLength);
        }


        public string LoadTextByGroupCodeAsYahooSlideshow(int portalid, string groupcode, int pagesize, int thumbnailHeight, int thumbnailWidth, int titleLength, int descLength)
        {
            int NumRecord = 0;
            string strSQL = "News_getShortNewsByGroupCode";
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@PortalID", portalid);
            p[1] = new SqlParameter("@Codes", groupcode);
            p[2] = new SqlParameter("@NumRecord", NumRecord);
            p[2].Direction = ParameterDirection.Output;
            DataTable dt = DataProvider.LoadPageSP(strSQL, 0, pagesize, p);
            return GenTextMLAsYahooSlideshow(dt, thumbnailHeight, thumbnailWidth, titleLength, descLength);
        }


        public string LoadHotTextAsYahooSlideshow(int pagesize, string CatID, int PortalID, int thumbnailHeight, int thumbnailWidth, int titleLength, int descLength)
        {
            string strSQL = "News_getShortNewsFullOrderTotalView";
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@CatID", CatID);
            param[1] = new SqlParameter("@PortalID", PortalID);
            DataTable dt = DataProvider.LoadPageSP(strSQL, 0, pagesize, param);
            return GenTextMLAsYahooSlideshow(dt, thumbnailHeight, thumbnailWidth, titleLength, descLength);
        }


        public string LoadNewestTextAsYahooSlideshow(int pagesize, string CatID, int PortalID, int thumbnailHeight, int thumbnailWidth, int titleLength, int descLength)
        {
            string strSQL = "News_getShortNewsFullOrderNewest";
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@CatID", CatID);
            param[1] = new SqlParameter("@PortalID", PortalID);
            DataTable dt = DataProvider.LoadPageSP(strSQL, 0, pagesize, param);
            return GenTextMLAsYahooSlideshow(dt, thumbnailHeight, thumbnailWidth, titleLength, descLength);
        }

        #region ISearchable Members

        public SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo ModInfo)
        {
            //  Get the Surveys for this Module instance
            Dictionary<int, NewsInfo> news = GetNewsByPortal(ModInfo.PortalID);
            SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();
            foreach (NewsInfo newsInfo in news.Values)
            {
                SearchItemInfo SearchItem;
                SearchItem = new SearchItemInfo
                (newsInfo.Headline,
                newsInfo.Description,
                newsInfo.CreateID,
                newsInfo.ModifyDate, ModInfo.ModuleID,
                Convert.ToString(newsInfo.ID),
                newsInfo.KeyWords);
                SearchItemCollection.Add(SearchItem);
            }
            return SearchItemCollection;
        }

        #endregion
    }
}
