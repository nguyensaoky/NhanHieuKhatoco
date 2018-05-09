using System.Diagnostics;
using System.Data;
using System.Collections;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System;

using effority.Ealo;

namespace DotNetNuke.News
{
	public partial class MLCategoryController : EaloBase
	{
        static string CatQualifier = "Cat";

        //public static MLCategoryInfo GetCategory(int portalID, string catID, string Locale)
        //{
        //    return GetCategory(portalID, catID, Locale, true);
        //}

        //public static MLCategoryInfo GetCategory(int portalID, string catID, string Locale, bool FromCache)
        //{
        //    System.Collections.Generic.Dictionary<string, MLCategoryInfo> dic = GetCategoriesAsDictionary(portalID, Locale, FromCache);
        //    MLCategoryInfo fCatInfo = null;
        //    dic.TryGetValue(catID, out fCatInfo);
        //    return fCatInfo;
        //}

        public static MLCategoryInfo GetCategory(string catID, string Locale)
        {
            return GetCategory(catID, Locale, true);
        }

        public static MLCategoryInfo GetCategory(string catID, string Locale, bool FromCache)
        {
            StringInfo CatName = effority.Ealo.Controller.GetStringByQualifierAndStringName(CatQualifier, catID, Locale, FromCache);
            MLCategoryInfo fMLCatInfo = null;
            CategoryController catCont = new CategoryController();
            CategoryInfo catInfo = catCont.Load(catID);
            fMLCatInfo = new MLCategoryInfo(catInfo);
            if (CatName != null)
            {
                if (!String.IsNullOrEmpty(CatName.StringText))
                {
                    fMLCatInfo.MLCatName = CatName;
                }
            }
            //if (CatName!=null && String.IsNullOrEmpty(CatName.StringText))
            //{
            //    CatName.StringText = catInfo.CatName;
            //}
            //fMLCatInfo.MLCatName = CatName;
            return fMLCatInfo;
        }

		public static System.Collections.Generic.Dictionary<string, MLCategoryInfo> GetAllCategoriesAsDictionary(string Locale)
		{
			return GetAllCategoriesAsDictionary(Locale, true);
		}

        public static System.Collections.Generic.Dictionary<string, MLCategoryInfo> GetAllCategoriesAsDictionary(string Locale, bool FromCache)
		{
			System.Collections.Generic.Dictionary<string, MLCategoryInfo> dic = new System.Collections.Generic.Dictionary<string, MLCategoryInfo>();
			System.Collections.Generic.List<MLCategoryInfo> Liste = GetAllCategoriesAsListe(Locale, FromCache);
			foreach (CategoryInfo info in Liste)
			{
				dic.Add(info.CatID, (MLCategoryInfo)info);
			}
			return dic;
		}

        public static System.Collections.Generic.List<MLCategoryInfo> GetAllCategoriesAsListe(string Locale)
		{
            return GetAllCategoriesAsListe(Locale, true);
		}

        public static System.Collections.Generic.List<MLCategoryInfo> GetAllCategoriesAsListe(string Locale, bool FromCache)
		{
			DotNetNuke.Entities.Portals.PortalController pCont = new DotNetNuke.Entities.Portals.PortalController();
			ArrayList portals = pCont.GetPortals();
			System.Collections.Generic.List<MLCategoryInfo> liste = new System.Collections.Generic.List<MLCategoryInfo>();
			foreach (DotNetNuke.Entities.Portals.PortalInfo portal in portals)
			{
				liste.AddRange(GetCategoriesAsListe(portal.PortalID, Locale, FromCache));
			}
			return liste;
		}
		
		public static System.Collections.Generic.List<MLCategoryInfo> GetCategoriesAsListe(int PortalId, string Locale)
		{
            return GetCategoriesAsListe(PortalId, Locale, true);
		}

        public static System.Collections.Generic.List<MLCategoryInfo> GetCategoriesAsListe(int PortalId, string Locale, bool FromCache)
		{
            System.Collections.Generic.Dictionary<string, StringInfo> dic = effority.Ealo.Controller.GetStringsByQualifier(CatQualifier, Locale, FromCache);
            CategoryController catController = new CategoryController();
            System.Collections.Generic.Dictionary<string, CategoryInfo> catDic = catController.GetCatsByPortal(PortalId);
			System.Collections.Generic.List<MLCategoryInfo> neueListe = new System.Collections.Generic.List<MLCategoryInfo>();

            foreach (CategoryInfo info in catDic.Values)
			{
				StringInfo localizedCatName = null;
				MLCategoryInfo MLCatInfo = new MLCategoryInfo(info);
                if (dic.TryGetValue(info.CatID, out localizedCatName))
				{
                    if (!string.IsNullOrEmpty(localizedCatName.StringText))
					{
                        localizedCatName.FallBack = MLCatInfo.CatName;
                        localizedCatName.FallbackIsNull = false;
                        MLCatInfo.MLCatName = localizedCatName;
					}
				}
                neueListe.Add(MLCatInfo);
			}
			return neueListe;
		}

		public static System.Collections.Generic.Dictionary<string, MLCategoryInfo> GetCategoriesAsDictionary(int PortalId, string Locale)
		{
            return GetCategoriesAsDictionary(PortalId, Locale, true);
		}

        public static System.Collections.Generic.Dictionary<string, MLCategoryInfo> GetCategoriesAsDictionary(int PortalId, string Locale, bool FromCache)
		{
			System.Collections.Generic.Dictionary<string, MLCategoryInfo> dic = new System.Collections.Generic.Dictionary<string, MLCategoryInfo>();
            System.Collections.Generic.List<MLCategoryInfo> Liste = GetCategoriesAsListe(PortalId, Locale, FromCache);
			foreach (CategoryInfo info in Liste)
			{
				dic.Add(info.CatID, (MLCategoryInfo)info);
			}
			return dic;
		}
		
		public static List<MLCategoryInfo> LocalizedCategoryList(ArrayList Categories, string Locale, bool FromCache)
		{
			List<CategoryInfo> newList = new List<CategoryInfo>();
			foreach (CategoryInfo cat in Categories)
			{
                newList.Add(cat);
			}
			return LocalizedCategoryList(newList, Locale, FromCache);
		}

        public static List<MLCategoryInfo> LocalizedCategoryList(List<CategoryInfo> Categories, string Locale, bool FromCache)
		{
			System.Collections.Generic.Dictionary<string, StringInfo> dic = effority.Ealo.Controller.GetStringsByQualifier(CatQualifier, Locale, FromCache);
			System.Collections.Generic.List<MLCategoryInfo> neueListe = new System.Collections.Generic.List<MLCategoryInfo>();
			foreach (CategoryInfo info in Categories)
			{
				StringInfo localizedCatName = null;
				MLCategoryInfo MLCatInfo = new MLCategoryInfo(info);
                if (dic.TryGetValue(info.CatID, out localizedCatName))
				{
                    MLCatInfo.MLCatName = localizedCatName;
				}
                neueListe.Add(MLCatInfo);
			}
			return neueListe;
		}
		
		public static void UpdateCat(string CatId, string CatName, string Locale)
		{
            StringInfo nameInfo = effority.Ealo.Controller.GetStringByQualifierAndStringName(CatQualifier, CatId, Locale, false);
			if (! string.IsNullOrEmpty(Locale))
			{
                if (!string.IsNullOrEmpty(CatName))
				{
                    effority.Ealo.Controller.UpdateStringByQualiferAndStringName(CatQualifier, CatId, CatName, Locale, true);
				}
				else
				{
					if (nameInfo != null)
					{
                        effority.Ealo.Controller.DeleteByQualifierAndStringNameAndLocale(CatQualifier, CatId, Locale);
					}
				}
			}
		}
	}
	
	public partial class MLCategoryInfo : CategoryInfo
	{
		private StringInfo _MLCatName;
		
		private void ImportCategoryInfo(CategoryInfo CatInfo)
		{
            base.CatID = CatInfo.CatID;
			base.CatName = CatInfo.CatName;
			base.CatCode = CatInfo.CatCode;
			base.Description = CatInfo.Description;
			base.DesktopListID = CatInfo.DesktopListID;
			base.DesktopViewID = CatInfo.DesktopViewID;
			base.NewsID = CatInfo.NewsID;
			base.OrderNumber = CatInfo.OrderNumber;
			base.ParentID = CatInfo.ParentID;
			base.PortalID = CatInfo.PortalID;
		}

        public MLCategoryInfo(CategoryInfo catInfo)
		{
			this.ImportCategoryInfo(catInfo);
		}
		
		public StringInfo MLCatName
		{
			get
			{
				return _MLCatName;
			}
			set
			{
				_MLCatName = value;
			}
		}
	}

    public partial class MLNewsController : EaloBase
    {
        static string NewsHeadlineQualifier = "NewsHeadline";
        static string NewsDescriptionQualifier = "NewsDescription";
        static string NewsContentQualifier = "NewsContent";

        //public static MLNewsInfo GetNews(int ID, string catID, string Locale)
        //{
        //    return GetNews(ID, catID, Locale, true);
        //}

        //public static MLNewsInfo GetNews(int ID, string catID, string Locale, bool FromCache)
        //{
        //    System.Collections.Generic.Dictionary<int, MLNewsInfo> dic = GetNewsAsDictionaryByCat(catID, Locale, FromCache);
        //    MLNewsInfo fNewsInfo = null;
        //    dic.TryGetValue(ID, out fNewsInfo);
        //    return fNewsInfo;
        //}

        public static MLNewsInfo GetNews(int ID, string Locale)
        {
            return GetNews(ID, Locale, true);
        }

        public static MLNewsInfo GetNews(int ID, string Locale, bool FromCache)
        {
            StringInfo Headline = effority.Ealo.Controller.GetStringByQualifierAndStringName(NewsHeadlineQualifier, ID.ToString(), Locale, FromCache);
            StringInfo Description = effority.Ealo.Controller.GetStringByQualifierAndStringName(NewsDescriptionQualifier, ID.ToString(), Locale, FromCache);
            StringInfo Content = effority.Ealo.Controller.GetStringByQualifierAndStringName(NewsContentQualifier, ID.ToString(), Locale, FromCache);
            MLNewsInfo fMLNewsInfo = null;
            NewsController newsCont = new NewsController();
            NewsInfo newsInfo = newsCont.LoadNoML(ID);
            fMLNewsInfo = new MLNewsInfo(newsInfo);
            if (Headline != null)
            {
                if (!String.IsNullOrEmpty(Headline.StringText))
                {
                    fMLNewsInfo.MLHeadline = Headline;
                }
            }
            if (Description != null)
            {
                if (!String.IsNullOrEmpty(Description.StringText))
                {
                    fMLNewsInfo.MLDescription = Description;
                }
            }
            if (Content != null)
            {
                if (!String.IsNullOrEmpty(Content.StringText))
                {
                    fMLNewsInfo.MLContent = Content;
                }
            }
            //if (Headline != null && String.IsNullOrEmpty(Headline.StringText))
            //{
            //    Headline.StringText = newsInfo.Headline;
            //}
            //if (Description != null && String.IsNullOrEmpty(Description.StringText))
            //{
            //    Description.StringText = newsInfo.Description;
            //}
            //if (Content != null && String.IsNullOrEmpty(Content.StringText))
            //{
            //    Content.StringText = newsInfo.Content;
            //}
            //fMLNewsInfo.MLHeadline = Headline;
            //fMLNewsInfo.MLDescription = Description;
            //fMLNewsInfo.MLContent = Content;
            return fMLNewsInfo;
        }

        public static System.Collections.Generic.Dictionary<int, MLNewsInfo> GetAllNewsAsDictionary(string Locale)
        {
            return GetAllNewsAsDictionary(Locale, true);
        }

        public static System.Collections.Generic.Dictionary<int, MLNewsInfo> GetAllNewsAsDictionary(string Locale, bool FromCache)
        {
            System.Collections.Generic.Dictionary<int, MLNewsInfo> dic = new System.Collections.Generic.Dictionary<int, MLNewsInfo>();
            System.Collections.Generic.List<MLNewsInfo> Liste = GetAllNewsAsListe(Locale, FromCache);
            foreach (NewsInfo info in Liste)
            {
                dic.Add(info.ID, (MLNewsInfo)info);
            }
            return dic;
        }

        public static System.Collections.Generic.List<MLNewsInfo> GetAllNewsAsListe(string Locale)
        {
            return GetAllNewsAsListe(Locale, true);
        }

        public static System.Collections.Generic.List<MLNewsInfo> GetAllNewsAsListe(string Locale, bool FromCache)
        {
            DotNetNuke.Entities.Portals.PortalController pCont = new DotNetNuke.Entities.Portals.PortalController();
            ArrayList portals = pCont.GetPortals();
            System.Collections.Generic.List<MLNewsInfo> liste = new System.Collections.Generic.List<MLNewsInfo>();
            foreach (DotNetNuke.Entities.Portals.PortalInfo portal in portals)
            {
                liste.AddRange(GetNewsAsListe(portal.PortalID, Locale, FromCache));
            }
            return liste;
        }

        public static System.Collections.Generic.List<MLNewsInfo> GetNewsAsListe(int PortalId, string Locale)
        {
            return GetNewsAsListe(PortalId, Locale, true);
        }

        public static System.Collections.Generic.List<MLNewsInfo> GetNewsAsListe(int PortalId, string Locale, bool FromCache)
        {
            CategoryController catCont = new CategoryController();
            Dictionary<string, CategoryInfo> cats = catCont.GetCatsByPortal(PortalId);
            System.Collections.Generic.List<MLNewsInfo> liste = new System.Collections.Generic.List<MLNewsInfo>();
            foreach (CategoryInfo catInfo in cats.Values)
            {
                liste.AddRange(GetNewsAsListeByCat(catInfo.CatID, Locale, FromCache));
            }
            return liste;
        }

        public static System.Collections.Generic.List<MLNewsInfo> GetNewsAsListeByCat(string CatId, string Locale)
        {
            return GetNewsAsListeByCat(CatId, Locale, true);
        }

        public static System.Collections.Generic.List<MLNewsInfo> GetNewsAsListeByCat(string CatId, string Locale, bool FromCache)
        {
            System.Collections.Generic.Dictionary<string, StringInfo> dicHeadline = effority.Ealo.Controller.GetStringsByQualifier(NewsHeadlineQualifier, Locale, FromCache);
            System.Collections.Generic.Dictionary<string, StringInfo> dicDescription = effority.Ealo.Controller.GetStringsByQualifier(NewsDescriptionQualifier, Locale, FromCache);
            System.Collections.Generic.Dictionary<string, StringInfo> dicContent = effority.Ealo.Controller.GetStringsByQualifier(NewsContentQualifier, Locale, FromCache);

            NewsController newsController = new NewsController();
            System.Collections.Generic.Dictionary<int, NewsInfo> newsDic = newsController.GetNewsByCat(CatId);
            System.Collections.Generic.List<MLNewsInfo> neueListe = new System.Collections.Generic.List<MLNewsInfo>();

            foreach (NewsInfo info in newsDic.Values)
            {
                StringInfo localizedHeadline = null;
                StringInfo localizedDescription = null;
                StringInfo localizedContent = null;

                MLNewsInfo MLNewsInfo = new MLNewsInfo(info);
                
                if (dicHeadline.TryGetValue(info.ID.ToString(), out localizedHeadline))
                {
                    if (!string.IsNullOrEmpty(localizedHeadline.StringText))
                    {
                        localizedHeadline.FallBack = MLNewsInfo.Headline;
                        localizedHeadline.FallbackIsNull = false;
                        MLNewsInfo.MLHeadline = localizedHeadline;
                    }
                }

                if (dicDescription.TryGetValue(info.ID.ToString(), out localizedDescription))
                {
                    if (!string.IsNullOrEmpty(localizedDescription.StringText))
                    {
                        localizedDescription.FallBack = MLNewsInfo.Description;
                        localizedDescription.FallbackIsNull = false;
                        MLNewsInfo.MLDescription = localizedDescription;
                    }
                }

                if (dicContent.TryGetValue(info.ID.ToString(), out localizedContent))
                {
                    if (!string.IsNullOrEmpty(localizedContent.StringText))
                    {
                        localizedContent.FallBack = MLNewsInfo.Content;
                        localizedContent.FallbackIsNull = false;
                        MLNewsInfo.MLContent = localizedContent;
                    }
                }
                neueListe.Add(MLNewsInfo);
            }
            return neueListe;
        }

        public static System.Collections.Generic.Dictionary<int, MLNewsInfo> GetNewsAsDictionary(int PortalId, string Locale)
        {
            return GetNewsAsDictionary(PortalId, Locale, true);
        }

        public static System.Collections.Generic.Dictionary<int, MLNewsInfo> GetNewsAsDictionary(int PortalId, string Locale, bool FromCache)
        {
            System.Collections.Generic.Dictionary<int, MLNewsInfo> dic = new System.Collections.Generic.Dictionary<int, MLNewsInfo>();
            System.Collections.Generic.List<MLNewsInfo> Liste = GetNewsAsListe(PortalId, Locale, FromCache);
            foreach (NewsInfo info in Liste)
            {
                dic.Add(info.ID, (MLNewsInfo)info);
            }
            return dic;
        }

        public static System.Collections.Generic.Dictionary<int, MLNewsInfo> GetNewsAsDictionaryByCat(string CatId, string Locale)
        {
            return GetNewsAsDictionaryByCat(CatId, Locale, true);
        }

        public static System.Collections.Generic.Dictionary<int, MLNewsInfo> GetNewsAsDictionaryByCat(string CatId, string Locale, bool FromCache)
        {
            System.Collections.Generic.Dictionary<int, MLNewsInfo> dic = new System.Collections.Generic.Dictionary<int, MLNewsInfo>();
            System.Collections.Generic.List<MLNewsInfo> Liste = GetNewsAsListeByCat(CatId, Locale, FromCache);
            foreach (NewsInfo info in Liste)
            {
                dic.Add(info.ID, (MLNewsInfo)info);
            }
            return dic;
        }

        public static List<MLNewsInfo> LocalizedNewsList(ArrayList News, string Locale, bool FromCache)
        {
            List<NewsInfo> newList = new List<NewsInfo>();
            foreach (NewsInfo news in News)
            {
                newList.Add(news);
            }
            return LocalizedNewsList(newList, Locale, FromCache);
        }

        public static List<MLNewsInfo> LocalizedNewsList(List<NewsInfo> News, string Locale, bool FromCache)
        {
            System.Collections.Generic.Dictionary<string, StringInfo> dicHeadline = effority.Ealo.Controller.GetStringsByQualifier(NewsHeadlineQualifier, Locale, FromCache);
            System.Collections.Generic.Dictionary<string, StringInfo> dicDescription = effority.Ealo.Controller.GetStringsByQualifier(NewsDescriptionQualifier, Locale, FromCache);
            System.Collections.Generic.Dictionary<string, StringInfo> dicContent = effority.Ealo.Controller.GetStringsByQualifier(NewsContentQualifier, Locale, FromCache);
            System.Collections.Generic.List<MLNewsInfo> neueListe = new System.Collections.Generic.List<MLNewsInfo>();
            foreach (NewsInfo info in News)
            {
                StringInfo localizedHeadline = null;
                StringInfo localizedDescription = null;
                StringInfo localizedContent = null;

                MLNewsInfo MLNewsInfo = new MLNewsInfo(info);
                if (dicHeadline.TryGetValue(info.ID.ToString(), out localizedHeadline))
                {
                    MLNewsInfo.MLHeadline = localizedHeadline;
                }

                if (dicDescription.TryGetValue(info.ID.ToString(), out localizedDescription))
                {
                    MLNewsInfo.MLDescription = localizedDescription;
                }

                if (dicContent.TryGetValue(info.ID.ToString(), out localizedContent))
                {
                    MLNewsInfo.MLContent = localizedContent;
                }
                neueListe.Add(MLNewsInfo);
            }
            return neueListe;
        }

        public static void UpdateNews(int NewsId, string Headline, string Description, string Content, string Locale)
        {
            StringInfo headlineInfo = effority.Ealo.Controller.GetStringByQualifierAndStringName(NewsHeadlineQualifier, NewsId.ToString(), Locale, false);
            StringInfo descriptionInfo = effority.Ealo.Controller.GetStringByQualifierAndStringName(NewsDescriptionQualifier, NewsId.ToString(), Locale, false);
            StringInfo contentInfo = effority.Ealo.Controller.GetStringByQualifierAndStringName(NewsContentQualifier, NewsId.ToString(), Locale, false);
            if (!string.IsNullOrEmpty(Locale))
            {
                if (!string.IsNullOrEmpty(Headline))
                {
                    effority.Ealo.Controller.UpdateStringByQualiferAndStringName(NewsHeadlineQualifier, NewsId.ToString(), Headline, Locale, true);
                }
                else
                {
                    if (headlineInfo != null)
                    {
                        effority.Ealo.Controller.DeleteByQualifierAndStringNameAndLocale(NewsHeadlineQualifier, NewsId.ToString(), Locale);
                    }
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    effority.Ealo.Controller.UpdateStringByQualiferAndStringName(NewsDescriptionQualifier, NewsId.ToString(), Description, Locale, true);
                }
                else
                {
                    if (descriptionInfo != null)
                    {
                        effority.Ealo.Controller.DeleteByQualifierAndStringNameAndLocale(NewsDescriptionQualifier, NewsId.ToString(), Locale);
                    }
                }

                if (!string.IsNullOrEmpty(Content))
                {
                    effority.Ealo.Controller.UpdateStringByQualiferAndStringName(NewsContentQualifier, NewsId.ToString(), Content, Locale, true);
                }
                else
                {
                    if (contentInfo != null)
                    {
                        effority.Ealo.Controller.DeleteByQualifierAndStringNameAndLocale(NewsContentQualifier, NewsId.ToString(), Locale);
                    }
                }
            }
        }
    }

    public partial class MLNewsInfo : NewsInfo
    {
        private StringInfo _MLHeadline;
        private StringInfo _MLDescription;
        private StringInfo _MLContent;

        private void ImportNewsInfo(NewsInfo NewsInfo)
        {
            base.CatID = NewsInfo.CatID;
            base.AllowComment = NewsInfo.AllowComment;
            base.Content = NewsInfo.Content;
            base.CreatedDate = NewsInfo.CreatedDate;
            base.CreateID = NewsInfo.CreateID;
            base.Description = NewsInfo.Description;
            base.Headline = NewsInfo.Headline;
            base.ID = NewsInfo.ID;
            base.ImageUrl = NewsInfo.ImageUrl;
            base.KeyWords = NewsInfo.KeyWords;
            base.ModifyDate = NewsInfo.ModifyDate;
            base.ModifyID = NewsInfo.ModifyID;
            base.Published = NewsInfo.Published;
            base.Source = NewsInfo.Source;
            base.TotalView = NewsInfo.TotalView;
        }

        public MLNewsInfo(NewsInfo NewsInfo)
        {
            this.ImportNewsInfo(NewsInfo);
        }

        public StringInfo MLHeadline
        {
            get
            {
                return _MLHeadline;
            }
            set
            {
                _MLHeadline = value;
            }
        }

        public StringInfo MLDescription
        {
            get
            {
                return _MLDescription;
            }
            set
            {
                _MLDescription = value;
            }
        }

        public StringInfo MLContent
        {
            get
            {
                return _MLContent;
            }
            set
            {
                _MLContent = value;
            }
        }
    }

    public partial class MLNewsGroupController : EaloBase
    {
        static string GroupQualifier = "Group";

        //public static MLNewsGroupInfo GetNewsGroup(int portalID, string groupID, string Locale)
        //{
        //    return GetNewsGroup(portalID, groupID, Locale, true);
        //}

        //public static MLNewsGroupInfo GetNewsGroup(int portalID, string groupID, string Locale, bool FromCache)
        //{
        //    System.Collections.Generic.Dictionary<string, MLNewsGroupInfo> dic = GetNewsGroupsAsDictionary(portalID, Locale, FromCache);
        //    MLNewsGroupInfo fGroupInfo = null;
        //    dic.TryGetValue(groupID, out fGroupInfo);
        //    return fGroupInfo;
        //}

        public static MLNewsGroupInfo GetNewsGroup(string groupID, string Locale)
        {
            return GetNewsGroup(groupID, Locale, true);
        }

        public static MLNewsGroupInfo GetNewsGroup(string groupID, string Locale, bool FromCache)
        {
            StringInfo GroupName = effority.Ealo.Controller.GetStringByQualifierAndStringName(GroupQualifier, groupID, Locale, FromCache);
            MLNewsGroupInfo fMLGroupInfo = null;
            NewsGroupController groupCont = new NewsGroupController();
            NewsGroupInfo groupInfo = groupCont.Load(groupID);
            fMLGroupInfo = new MLNewsGroupInfo(groupInfo);
            if (GroupName != null)
            {
                if (!String.IsNullOrEmpty(GroupName.StringText))
                {
                    fMLGroupInfo.MLGroupName = GroupName;
                }
            }
            //if (GroupName != null && String.IsNullOrEmpty(GroupName.StringText))
            //{
            //    GroupName.StringText = groupInfo.NewsGroupName;
            //}
            //fMLGroupInfo.MLGroupName = GroupName;
            return fMLGroupInfo;
        }

        public static System.Collections.Generic.Dictionary<string, MLNewsGroupInfo> GetAllNewsGroupsAsDictionary(string Locale)
        {
            return GetAllNewsGroupsAsDictionary(Locale, true);
        }

        public static System.Collections.Generic.Dictionary<string, MLNewsGroupInfo> GetAllNewsGroupsAsDictionary(string Locale, bool FromCache)
        {
            System.Collections.Generic.Dictionary<string, MLNewsGroupInfo> dic = new System.Collections.Generic.Dictionary<string, MLNewsGroupInfo>();
            System.Collections.Generic.List<MLNewsGroupInfo> Liste = GetAllNewsGroupsAsListe(Locale, FromCache);
            foreach (NewsGroupInfo info in Liste)
            {
                dic.Add(info.NewsGroupID, (MLNewsGroupInfo)info);
            }
            return dic;
        }

        public static System.Collections.Generic.List<MLNewsGroupInfo> GetAllNewsGroupsAsListe(string Locale)
        {
            return GetAllNewsGroupsAsListe(Locale, true);
        }

        public static System.Collections.Generic.List<MLNewsGroupInfo> GetAllNewsGroupsAsListe(string Locale, bool FromCache)
        {
            DotNetNuke.Entities.Portals.PortalController pCont = new DotNetNuke.Entities.Portals.PortalController();
            ArrayList portals = pCont.GetPortals();
            System.Collections.Generic.List<MLNewsGroupInfo> liste = new System.Collections.Generic.List<MLNewsGroupInfo>();
            foreach (DotNetNuke.Entities.Portals.PortalInfo portal in portals)
            {
                liste.AddRange(GetNewsGroupsAsListe(portal.PortalID, Locale, FromCache));
            }
            return liste;
        }

        public static System.Collections.Generic.List<MLNewsGroupInfo> GetNewsGroupsAsListe(int PortalId, string Locale)
        {
            return GetNewsGroupsAsListe(PortalId, Locale, true);
        }

        public static System.Collections.Generic.List<MLNewsGroupInfo> GetNewsGroupsAsListe(int PortalId, string Locale, bool FromCache)
        {
            System.Collections.Generic.Dictionary<string, StringInfo> dic = effority.Ealo.Controller.GetStringsByQualifier(GroupQualifier, Locale, FromCache);
            NewsGroupController groupController = new NewsGroupController();
            System.Collections.Generic.Dictionary<string, NewsGroupInfo> groupDic = groupController.GetNewsGroupsByPortal(PortalId);
            System.Collections.Generic.List<MLNewsGroupInfo> neueListe = new System.Collections.Generic.List<MLNewsGroupInfo>();

            foreach (NewsGroupInfo info in groupDic.Values)
            {
                StringInfo localizedGroupName = null;
                MLNewsGroupInfo MLGroupInfo = new MLNewsGroupInfo(info);
                if (dic.TryGetValue(info.NewsGroupID, out localizedGroupName))
                {
                    if (!string.IsNullOrEmpty(localizedGroupName.StringText))
                    {
                        localizedGroupName.FallBack = MLGroupInfo.NewsGroupName;
                        localizedGroupName.FallbackIsNull = false;
                        MLGroupInfo.MLGroupName = localizedGroupName;
                    }
                }
                neueListe.Add(MLGroupInfo);
            }
            return neueListe;
        }

        public static System.Collections.Generic.Dictionary<string, MLNewsGroupInfo> GetNewsGroupsAsDictionary(int PortalId, string Locale)
        {
            return GetNewsGroupsAsDictionary(PortalId, Locale, true);
        }

        public static System.Collections.Generic.Dictionary<string, MLNewsGroupInfo> GetNewsGroupsAsDictionary(int PortalId, string Locale, bool FromCache)
        {
            System.Collections.Generic.Dictionary<string, MLNewsGroupInfo> dic = new System.Collections.Generic.Dictionary<string, MLNewsGroupInfo>();
            System.Collections.Generic.List<MLNewsGroupInfo> Liste = GetNewsGroupsAsListe(PortalId, Locale, FromCache);
            foreach (NewsGroupInfo info in Liste)
            {
                dic.Add(info.NewsGroupID, (MLNewsGroupInfo)info);
            }
            return dic;
        }

        public static List<MLNewsGroupInfo> LocalizedNewsGroupList(ArrayList NewsGroups, string Locale, bool FromCache)
        {
            List<NewsGroupInfo> newList = new List<NewsGroupInfo>();
            foreach (NewsGroupInfo group in NewsGroups)
            {
                newList.Add(group);
            }
            return LocalizedNewsGroupList(newList, Locale, FromCache);
        }

        public static List<MLNewsGroupInfo> LocalizedNewsGroupList(List<NewsGroupInfo> NewsGroups, string Locale, bool FromCache)
        {
            System.Collections.Generic.Dictionary<string, StringInfo> dic = effority.Ealo.Controller.GetStringsByQualifier(GroupQualifier, Locale, FromCache);
            System.Collections.Generic.List<MLNewsGroupInfo> neueListe = new System.Collections.Generic.List<MLNewsGroupInfo>();
            foreach (NewsGroupInfo info in NewsGroups)
            {
                StringInfo localizedGroupName = null;
                MLNewsGroupInfo MLGroupInfo = new MLNewsGroupInfo(info);
                if (dic.TryGetValue(info.NewsGroupID, out localizedGroupName))
                {
                    MLGroupInfo.MLGroupName = localizedGroupName;
                }
                neueListe.Add(MLGroupInfo);
            }
            return neueListe;
        }

        public static void UpdateGroup(string GroupId, string GroupName, string Locale)
        {
            StringInfo nameInfo = effority.Ealo.Controller.GetStringByQualifierAndStringName(GroupQualifier, GroupId, Locale, false);
            if (!string.IsNullOrEmpty(Locale))
            {
                if (!string.IsNullOrEmpty(GroupName))
                {
                    effority.Ealo.Controller.UpdateStringByQualiferAndStringName(GroupQualifier, GroupId, GroupName, Locale, true);
                }
                else
                {
                    if (nameInfo != null)
                    {
                        effority.Ealo.Controller.DeleteByQualifierAndStringNameAndLocale(GroupQualifier, GroupId, Locale);
                    }
                }
            }
        }
    }

    public partial class MLNewsGroupInfo : NewsGroupInfo
    {
        private StringInfo _MLGroupName;

        private void ImportNewsGroupInfo(NewsGroupInfo GroupInfo)
        {
            base.NewsGroupID = GroupInfo.NewsGroupID;
            base.NewsGroupName = GroupInfo.NewsGroupName;
            base.NewsGroupCode = GroupInfo.NewsGroupCode;
            base.Description = GroupInfo.Description;
            base.OrderNumber = GroupInfo.OrderNumber;
            base.PortalID = GroupInfo.PortalID;
            base.DesktopListID = GroupInfo.DesktopListID;
            base.DesktopViewID = GroupInfo.DesktopViewID;
        }

        public MLNewsGroupInfo(NewsGroupInfo groupInfo)
        {
            this.ImportNewsGroupInfo(groupInfo);
        }

        public StringInfo MLGroupName
        {
            get
            {
                return _MLGroupName;
            }
            set
            {
                _MLGroupName = value;
            }
        }
    }
}