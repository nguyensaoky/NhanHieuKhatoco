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
using System.Web;

namespace DotNetNuke.News
{
    public class CategoryController
    {
        #region Constructor
        public CategoryController() { }
        #endregion

        #region Function
        private const string INSERT_NEWS_CATEGORY = "News_insertCat";
        public int Insert(CategoryInfo cat)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[11];
                param[0] = new SqlParameter("@CatID", cat.CatID);
                param[1] = new SqlParameter("@CatName", cat.CatName);
                param[2] = new SqlParameter("@ParentID", cat.ParentID);
                param[3] = new SqlParameter("@CatCode", cat.CatCode);
                param[4] = new SqlParameter("@Description", cat.Description);
                param[5] = new SqlParameter("@OrderNumber", cat.OrderNumber);
                param[6] = new SqlParameter("@DesktopListID", cat.DesktopListID);
                param[7] = new SqlParameter("@DesktopViewID", cat.DesktopViewID);
                param[8] = new SqlParameter("@NewsID", cat.NewsID);
                param[9] = new SqlParameter("@PortalID", cat.PortalID);
                param[10] = new SqlParameter("@Visible", cat.Visible);
                result = DataProvider.ExecuteSP(INSERT_NEWS_CATEGORY, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string INSERT_ROLE = "News_insertRoles";
        public void InsertRole(string CatID, string RolesString)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@CatID", CatID);
                param[1] = new SqlParameter("@RolesString", RolesString);
                DataProvider.ExecuteSP(INSERT_ROLE, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private const string UPDATE_NEWS_CATEGORY = "News_updateCat";
        public int Update(CategoryInfo cat)
        {
            int result = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[11];
                param[0] = new SqlParameter("@CatName", cat.CatName);
                param[1] = new SqlParameter("@ParentID", cat.ParentID);
                param[2] = new SqlParameter("@CatCode", cat.CatCode);
                param[3] = new SqlParameter("@Description", cat.Description);
                param[4] = new SqlParameter("@OrderNumber", cat.OrderNumber);
                param[5] = new SqlParameter("@DesktopListID", cat.DesktopListID);
                param[6] = new SqlParameter("@DesktopViewID", cat.DesktopViewID);
                param[7] = new SqlParameter("@NewsID", cat.NewsID);
                param[8] = new SqlParameter("@CatID", cat.CatID);
                param[9] = new SqlParameter("@PortalID", cat.PortalID);
                param[10] = new SqlParameter("@Visible", cat.Visible);
                result = DataProvider.ExecuteSP(UPDATE_NEWS_CATEGORY, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string DELETE_NEWS_CATEGORY = "News_deleteCat";
        public int Delete(string CatID)
        {
            int result = 0;
            try
            {
                SqlParameter param = new SqlParameter("@CatID", CatID);
                result = DataProvider.ExecuteSP(DELETE_NEWS_CATEGORY, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public CategoryInfo Converting(DataRow drNews_Category)
        {
            CategoryInfo result = null;
            try
            {
                result = new CategoryInfo();
                result.CatID = (string)drNews_Category["CatID"];
                result.CatName = (string)drNews_Category["CatName"];
                result.ParentID = (string)drNews_Category["ParentID"];
                result.NewsID = (int)drNews_Category["NewsID"];
                result.CatCode = (string)drNews_Category["CategoryCode"];
                result.Description = (string)drNews_Category["Description"];
                result.OrderNumber = (int)drNews_Category["OrderNumber"];
                result.DesktopListID = (int)drNews_Category["DesktopListID"];
                result.DesktopViewID = (int)drNews_Category["DesktopViewID"];
                result.PortalID = (int)drNews_Category["PortalID"];
                result.Visible = (bool)drNews_Category["Visible"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string LOAD_NEWS_CATEGORY = "News_getCatByID";
        public CategoryInfo Load(string CatID)
        {
            CategoryInfo result = null;
            try
            {
                SqlParameter param = new SqlParameter("@CatID", CatID);
                DataTable dt = DataProvider.SelectSP(LOAD_NEWS_CATEGORY, param);
                if (dt.Rows.Count > 0) result = Converting(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string LOAD_ROLES = "News_getRolesByCat";
        public DataTable LoadRolesByCat(string CatID)
        {
            DataTable result = null;
            try
            {
                SqlParameter param = new SqlParameter("@CatID", CatID);
                result = DataProvider.SelectSP(LOAD_ROLES, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private const string LOAD_LIST_NEWS_CATEGORY = "News_getCatFull";
        public DataTable LoadTable(int PortalID)
        {
            DataTable result = null;
            try
            {
                SqlParameter param = new SqlParameter("@PortalID", PortalID);
                result = DataProvider.SelectSP(LOAD_LIST_NEWS_CATEGORY, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public Dictionary<string, CategoryInfo> GetCatsByPortal(int PortalId)
        {
            Dictionary<string, CategoryInfo> cats;
            Int32 timeOut = DataCache.CatCacheTimeOut * Convert.ToInt32(Globals.PerformanceSetting);
            cats = FillCatInfoDictionary(DataProvider.ExecuteReader("News_getCatByPortal", new object[] { PortalId }));
            return cats;
        }

        private static Dictionary<string, CategoryInfo> FillCatInfoDictionary(IDataReader dr)
        {
            Dictionary<string, CategoryInfo> dic = new Dictionary<string, CategoryInfo>();
            try
            {
                while (dr.Read())
                {
                    CategoryInfo obj = FillCatInfo(dr);
                    dic.Add(obj.CatID, obj);
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

        private static CategoryInfo FillCatInfo(IDataReader dr)
        {
            CategoryInfo objCategoryInfo = new CategoryInfo();

            objCategoryInfo.CatID = Convert.ToString(Null.SetNull(dr["CatID"], objCategoryInfo.CatID));
            objCategoryInfo.CatName = Convert.ToString(Null.SetNull(dr["CatName"], objCategoryInfo.CatName));
            objCategoryInfo.CatCode = Convert.ToString(Null.SetNull(dr["CategoryCode"], objCategoryInfo.CatCode));
            objCategoryInfo.Description = Convert.ToString(Null.SetNull(dr["Description"], objCategoryInfo.Description));
            objCategoryInfo.DesktopListID = Convert.ToInt32(Null.SetNull(dr["DesktopListID"], objCategoryInfo.DesktopListID));
            objCategoryInfo.DesktopViewID = Convert.ToInt32(Null.SetNull(dr["DesktopViewID"], objCategoryInfo.DesktopViewID));
            objCategoryInfo.NewsID = Convert.ToInt32(Null.SetNull(dr["NewsID"], objCategoryInfo.NewsID));
            objCategoryInfo.OrderNumber = Convert.ToInt32(Null.SetNull(dr["OrderNumber"], objCategoryInfo.OrderNumber));
            objCategoryInfo.ParentID = Convert.ToString(Null.SetNull(dr["ParentID"], objCategoryInfo.ParentID));
            objCategoryInfo.PortalID = Convert.ToInt32(Null.SetNull(dr["PortalID"], objCategoryInfo.PortalID));
            objCategoryInfo.Visible = Convert.ToBoolean(Null.SetNull(dr["Visible"], objCategoryInfo.Visible));

            return objCategoryInfo;
        }

        #endregion

        #region load tree category
        public XmlDocument LoadTreeXML(string editFormatUrl, bool ismenu, int PortalID, int source, string codes, string parentCat)
        {
            DataTable dt=null;
            if (source == 0)
            {
                dt = LoadTree(ismenu, PortalID, "");
            }
            else if (source == 1)
            {
                dt = LoadTreeByCode(ismenu, PortalID, codes);
            }
            else if (source == 2)
            {
                dt = LoadTreeByParents(ismenu, PortalID, parentCat);
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<categories></categories>");
            XmlElement root = doc.DocumentElement;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                XmlElement category = doc.CreateElement("category");
                
                int tabid = Convert.ToInt32(dt.Rows[i]["DesktopListID"]);
                string link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "categoryid/" + dt.Rows[i]["CatID"].ToString());
                if (dt.Rows[i]["NewsID"].ToString() != "0")
                {
                    tabid = Convert.ToInt32(dt.Rows[i]["DesktopViewID"]);
                    link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "id/" + dt.Rows[i]["NewsID"].ToString());
                }
                string url_edit = editFormatUrl.Replace("@@catid", dt.Rows[i]["CatID"].ToString());

                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "ImgLevel1", DotNetNuke.Common.Globals.ApplicationPath + "/images/action_right.gif");
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "ImgLevel2", DotNetNuke.Common.Globals.ApplicationPath + "/images/edit.gif");
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "OrderNumber", dt.Rows[i]["OrderNumber"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "CatName", dt.Rows[i]["CatName"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "Level", dt.Rows[i]["Level"].ToString());
                string indent = "";
                int level = int.Parse(dt.Rows[i]["Level"].ToString());

                for (int j = 1; j < level; j++)
                {
                    indent += HttpContext.Current.Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                }
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "Indent", indent);
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "CatCode", dt.Rows[i]["CatCode"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "PortalID", dt.Rows[i]["PortalID"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "link", link);
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "url_edit", url_edit);                
                if (i % 2 == 1)
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "odd", "true");

                root.AppendChild(category);
            }

            return doc;
        }

        public XmlDocument MLLoadTreeXML(string editFormatUrl, bool ismenu, int PortalID, int source, string codes, string parentCat, string Locale)
        {
            DataTable dt = null;
            if (source == 0)
            {
                dt = LoadTree(ismenu, PortalID,"");
            }
            else if (source == 1)
            {
                dt = LoadTreeByCode(ismenu, PortalID, codes);
            }
            else if (source == 2)
            {
                dt = LoadTreeByParents(ismenu, PortalID, parentCat);
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<categories></categories>");
            XmlElement root = doc.DocumentElement;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                XmlElement category = doc.CreateElement("category");

                int tabid = Convert.ToInt32(dt.Rows[i]["DesktopListID"]);
                string link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "categoryid/" + dt.Rows[i]["CatID"].ToString());
                if (dt.Rows[i]["NewsID"].ToString() != "0")
                {
                    tabid = Convert.ToInt32(dt.Rows[i]["DesktopViewID"]);
                    link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "id/" + dt.Rows[i]["NewsID"].ToString());
                }
                string url_edit = editFormatUrl.Replace("@@catid", dt.Rows[i]["CatID"].ToString());

                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "ImgLevel1", DotNetNuke.Common.Globals.ApplicationPath + "/images/action_right.gif");
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "ImgLevel2", DotNetNuke.Common.Globals.ApplicationPath + "/images/edit.gif");
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "OrderNumber", dt.Rows[i]["OrderNumber"].ToString());
                MLCategoryInfo fMLCat = MLCategoryController.GetCategory(dt.Rows[i]["CatID"].ToString(), Locale, false);
                if (fMLCat != null)
                {
                    if (fMLCat.MLCatName != null)
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "CatName", fMLCat.MLCatName.StringTextOrFallBack);
                    }
                    else
                    {
                        DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "CatName", fMLCat.CatName);
                    }
                }
                else
                {
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "CatName", dt.Rows[i]["CatName"].ToString());
                }
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "Level", dt.Rows[i]["Level"].ToString());
                string indent = "";
                int level = int.Parse(dt.Rows[i]["Level"].ToString());

                for (int j = 1; j < level; j++)
                {
                    indent += HttpContext.Current.Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                }
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "Indent", indent);
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "CatCode", dt.Rows[i]["CatCode"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "PortalID", dt.Rows[i]["PortalID"].ToString());
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "link", link);
                DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "url_edit", url_edit);
                if (i % 2 == 1)
                    DotNetNuke.NewsProvider.Utils.AddNode(doc, category, "odd", "true");

                root.AppendChild(category);
            }

            return doc;
        }

        public DataTable LoadTree(bool ismenu, int PortalID, string CatID)
        {
            DataTable dtTree = new DataTable();
            dtTree.Columns.Add(new DataColumn("CatID", typeof(string)));
            dtTree.Columns.Add(new DataColumn("CatName", typeof(string)));
            dtTree.Columns.Add(new DataColumn("DesktopListID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("DesktopViewID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("OrderNumber", typeof(int)));
            dtTree.Columns.Add(new DataColumn("NewsID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("Level", typeof(int)));
            dtTree.Columns.Add(new DataColumn("CatCode", typeof(string)));
            dtTree.Columns.Add(new DataColumn("PortalID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("Visible", typeof(bool)));

            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@PortalID", PortalID);
            param[1] = new SqlParameter("@CatID", CatID);
            DataTable dt = DataProvider.SelectSP("News_getCatFullOrder", param);
            for (int i = 0; i < dt.Rows.Count; i++)
                if (dt.Rows[i]["ParentID"].ToString() == "0")
                    dtTree = add_row_tree(dt, dtTree, i, "", 1, ismenu);
            return dtTree;
        }

        public DataTable LoadTree(bool ismenu, int UserID, int PortalID, string CatID)
        {
            DataTable dtTree = new DataTable();
            dtTree.Columns.Add(new DataColumn("CatID", typeof(string)));
            dtTree.Columns.Add(new DataColumn("CatName", typeof(string)));
            dtTree.Columns.Add(new DataColumn("DesktopListID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("DesktopViewID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("OrderNumber", typeof(int)));
            dtTree.Columns.Add(new DataColumn("NewsID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("Level", typeof(int)));
            dtTree.Columns.Add(new DataColumn("CatCode", typeof(string)));
            dtTree.Columns.Add(new DataColumn("PortalID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("Visible", typeof(bool)));

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@UserID", UserID);
            param[1] = new SqlParameter("@PortalID", PortalID);
            param[2] = new SqlParameter("@CatID", CatID);
            DataTable dt = DataProvider.SelectSP("News_getCatFullOrder_ByUserID", param);
            for (int i = 0; i < dt.Rows.Count; i++)
                if (dt.Rows[i]["ParentID"].ToString() == "0")
                    dtTree = add_row_tree(dt, dtTree, i, "", 1, ismenu);
            return dtTree;
        }

        public DataTable LoadTreeByCode(bool ismenu, int PortalID, string Codes)
        {
            DataTable dtTree = new DataTable();
            dtTree.Columns.Add(new DataColumn("CatID", typeof(string)));
            dtTree.Columns.Add(new DataColumn("CatName", typeof(string)));
            dtTree.Columns.Add(new DataColumn("DesktopListID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("DesktopViewID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("OrderNumber", typeof(int)));
            dtTree.Columns.Add(new DataColumn("NewsID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("Level", typeof(int)));
            dtTree.Columns.Add(new DataColumn("CatCode", typeof(string)));
            dtTree.Columns.Add(new DataColumn("PortalID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("Visible", typeof(bool)));

            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@PortalID", PortalID);
            param[1] = new SqlParameter("@Codes", Codes);
            DataTable dt = DataProvider.SelectSP("News_getCatByCode", param);
            while (dt.Rows.Count > 0)
            {
                dtTree = add_row_tree_while(dt, dtTree, 0, "", 1, ismenu);
            }
            return dtTree;
        }

        public DataTable LoadTreeByParents(bool ismenu, int PortalID, string ParentCat)
        {
            DataTable dtTree = new DataTable();
            dtTree.Columns.Add(new DataColumn("CatID", typeof(string)));
            dtTree.Columns.Add(new DataColumn("CatName", typeof(string)));
            dtTree.Columns.Add(new DataColumn("DesktopListID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("DesktopViewID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("OrderNumber", typeof(int)));
            dtTree.Columns.Add(new DataColumn("NewsID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("Level", typeof(int)));
            dtTree.Columns.Add(new DataColumn("CatCode", typeof(string)));
            dtTree.Columns.Add(new DataColumn("PortalID", typeof(int)));
            dtTree.Columns.Add(new DataColumn("Visible", typeof(bool)));

            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@PortalID", PortalID);
            param[1] = new SqlParameter("@ParentCat", ParentCat);
            DataTable dt = DataProvider.SelectSP("News_getCatByParent", param);
            for (int i = 0; i < dt.Rows.Count; i++)
                if (dt.Rows[i]["ParentID"].ToString() == ParentCat)
                    dtTree = add_row_tree(dt, dtTree, i, "", 1, ismenu);
            return dtTree;
        }

        private DataTable add_row_tree(DataTable dt, DataTable dest, int idx, string prefix, int level, bool ismenu)
        {
            DataRow row = dest.NewRow();

            if (ismenu)
            {
                row["CatName"] = dt.Rows[idx]["CatName"];
            }
            else
            {
                if (prefix == "")
                    row["CatName"] = dt.Rows[idx]["CatName"];
                else
                    row["CatName"] = prefix + " :: " + dt.Rows[idx]["CatName"];
            }

            row["CatID"] = dt.Rows[idx]["CatID"];
            row["DesktopListID"] = dt.Rows[idx]["DesktopListID"];
            row["DesktopViewID"] = dt.Rows[idx]["DesktopViewID"];
            row["OrderNumber"] = dt.Rows[idx]["OrderNumber"];
            row["NewsID"] = dt.Rows[idx]["NewsID"];
            row["Level"] = level;
            row["CatCode"] = dt.Rows[idx]["CategoryCode"];
            row["PortalID"] = dt.Rows[idx]["PortalID"];
            row["Visible"] = dt.Rows[idx]["Visible"];
            dest.Rows.Add(row);

            for (int i = 0; i < dt.Rows.Count; i++)
                if (dt.Rows[i]["ParentID"].ToString() == row["CatID"].ToString())
                    dest = add_row_tree(dt, dest, i, row["CatName"].ToString(), level + 1, ismenu);

            return dest;
        }

        private DataTable add_row_tree_norecursive(DataTable dt, DataTable dest, int idx, string prefix, int level, bool ismenu)
        {
            DataRow row = dest.NewRow();

            if (ismenu)
            {
                row["CatName"] = dt.Rows[idx]["CatName"];
            }
            else
            {
                if (prefix == "")
                    row["CatName"] = dt.Rows[idx]["CatName"];
                else
                    row["CatName"] = prefix + " :: " + dt.Rows[idx]["CatName"];
            }

            row["CatID"] = dt.Rows[idx]["CatID"];
            row["DesktopListID"] = dt.Rows[idx]["DesktopListID"];
            row["DesktopViewID"] = dt.Rows[idx]["DesktopViewID"];
            row["OrderNumber"] = dt.Rows[idx]["OrderNumber"];
            row["NewsID"] = dt.Rows[idx]["NewsID"];
            row["Level"] = level;
            row["CatCode"] = dt.Rows[idx]["CategoryCode"];
            row["PortalID"] = dt.Rows[idx]["PortalID"];
            row["Visible"] = dt.Rows[idx]["Visible"];
            dest.Rows.Add(row);
            return dest;
        }

        private DataTable add_row_tree_while(DataTable dt, DataTable dest, int idx, string prefix, int level, bool ismenu)
        {
            DataRow row = dest.NewRow();

            if (ismenu)
            {
                row["CatName"] = dt.Rows[idx]["CatName"];
            }
            else
            {
                if (prefix == "")
                    row["CatName"] = dt.Rows[idx]["CatName"];
                else
                    row["CatName"] = prefix + " :: " + dt.Rows[idx]["CatName"];
            }

            row["CatID"] = dt.Rows[idx]["CatID"];
            row["DesktopListID"] = dt.Rows[idx]["DesktopListID"];
            row["DesktopViewID"] = dt.Rows[idx]["DesktopViewID"];
            row["OrderNumber"] = dt.Rows[idx]["OrderNumber"];
            row["NewsID"] = dt.Rows[idx]["NewsID"];
            row["Level"] = level;
            row["CatCode"] = dt.Rows[idx]["CategoryCode"];
            row["PortalID"] = dt.Rows[idx]["PortalID"];
            row["Visible"] = dt.Rows[idx]["Visible"];
            dest.Rows.Add(row);
            dt.Rows.RemoveAt(idx);

            int i = 0;
            while (dt.Rows.Count > 0 && i < dt.Rows.Count)
            {
                if (dt.Rows[i]["ParentID"].ToString() == row["CatID"].ToString())
                {
                    dest = add_row_tree_while(dt, dest, i, row["CatName"].ToString(), level + 1, ismenu);
                    i = 0;
                }
                else
                {
                    i++;
                }
            }
            return dest;
        }

        public DataTable LoadTreeForMenu(int PortalID, int source, string codes, string parentCat, string Locale)
        {
            DataTable dt = null;
            if (source == 0)
            {
                dt = LoadTree(true, PortalID,"");
            }
            else if (source == 1)
            {
                dt = LoadTreeByCode(true, PortalID, codes);
            }
            else if (source == 2)
            {
                dt = LoadTreeByParents(true, PortalID, parentCat);
            }
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Link", typeof(string)));
            dt.Columns.Add(new DataColumn("Target", typeof(string)));
            dt.Columns.Add(new DataColumn("Image", typeof(string)));

            int MotherLevel = -1;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool visible = Convert.ToBoolean(dt.Rows[i]["Visible"]);
                int level = Convert.ToInt32(dt.Rows[i]["Level"]);
                if (visible)
                {
                    if (MotherLevel < level && MotherLevel != -1)
                    {
                        dt.Rows[i]["Visible"] = false;
                    }
                    else if (MotherLevel >= level)
                    {
                        MotherLevel = -1;
                    }
                }
                else
                {
                    if (MotherLevel < level && MotherLevel == -1)
                    {
                        MotherLevel = level;
                    }
                    else if (MotherLevel > level)
                    {
                        MotherLevel = level;
                    }
                    else if (MotherLevel == level)
                    {
                        MotherLevel = level;
                    }
                }

                dt.Rows[i]["Target"] = "_self";
                dt.Rows[i]["Image"] = "";
                int tabid = Convert.ToInt32(dt.Rows[i]["DesktopListID"]);
                string link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "categoryid/" + dt.Rows[i]["CatID"].ToString());
                if (dt.Rows[i]["NewsID"].ToString() != "0")
                {
                    tabid = Convert.ToInt32(dt.Rows[i]["DesktopViewID"]);
                    link = DotNetNuke.Common.Globals.NavigateURL(tabid, "", "id/" + dt.Rows[i]["NewsID"].ToString());
                }
                dt.Rows[i]["Link"] = link;
                MLCategoryInfo fMLCategory = MLCategoryController.GetCategory(dt.Rows[i]["CatID"].ToString(), Locale, false);
                if (fMLCategory != null)
                {
                    if (fMLCategory.MLCatName != null)
                    {
                        dt.Rows[i]["Name"] = fMLCategory.MLCatName.StringText;
                    }
                    else
                    {
                        dt.Rows[i]["Name"] = fMLCategory.CatName;
                    }
                }
                else
                {
                    dt.Rows[i]["Name"] = dt.Rows[i]["CatName"].ToString();
                }

            }

            for (int j = dt.Rows.Count - 1; j >= 0; j-- )
            {
                if (!Convert.ToBoolean(dt.Rows[j]["Visible"]))
                {
                    dt.Rows.RemoveAt(j);
                }
            }

            return dt;
        }

        #endregion
    }
}