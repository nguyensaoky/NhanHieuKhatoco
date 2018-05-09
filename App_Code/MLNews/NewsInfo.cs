using System;

namespace DotNetNuke.News
{
    public class NewsInfo
    {
        #region Constructor
        public NewsInfo()
        {
            this._ID = 0;
            this._CatID = "";
            this._Headline = "";
            this._Description = "";
            this._ImageUrl = "";
            this._Source = "";
            this._AllowComment = false;
            this._Published = false;
            this._CreatedDate = DateTime.Now;
            this._CreateID = 0;
            this._ModifyDate = DateTime.Now;
            this._ModifyID = 0;
            this._TotalView = 0;
            this._Content = "";
            this._KeyWords = "";
            this._StartDate = null;
            this._EndDate = null;
            this._Feature = 0;
        }

        #endregion

        #region Vars
        private int _ID;
        private string _CatID;
        private string _Headline;
        private string _Description;
        private string _ImageUrl;
        private string _Source;
        private bool _AllowComment;
        private bool _Published;
        private DateTime _CreatedDate;
        private int _CreateID;
        private DateTime _ModifyDate;
        private int _ModifyID;
        private int _TotalView;
        private string _Content;
        private string _KeyWords;
        private DateTime? _StartDate;
        private DateTime? _EndDate;
        private int _Feature;
        private string _Writer;
        private string _DonVi;
        private bool _FromOuter;
        #endregion

        #region Property
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string CatID
        {
            get { return _CatID; }
            set { _CatID = value; }
        }
        public string Headline
        {
            get { return _Headline; }
            set { _Headline = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string ImageUrl
        {
            get { return _ImageUrl; }
            set { _ImageUrl = value; }
        }
        public string Source
        {
            get { return _Source; }
            set { _Source = value; }
        }
        public bool AllowComment
        {
            get { return _AllowComment; }
            set { _AllowComment = value; }
        }
        public bool Published
        {
            get { return _Published; }
            set { _Published = value; }
        }
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }
        public int CreateID
        {
            get { return _CreateID; }
            set { _CreateID = value; }
        }
        public DateTime ModifyDate
        {
            get { return _ModifyDate; }
            set { _ModifyDate = value; }
        }
        public int ModifyID
        {
            get { return _ModifyID; }
            set { _ModifyID = value; }
        }
        public int TotalView
        {
            get { return _TotalView; }
            set { _TotalView = value; }
        }
        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }
        public string KeyWords
        {
            get { return _KeyWords; }
            set { _KeyWords = value; }
        }
        public DateTime? StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }
        public DateTime? EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }
        public int Feature
        {
            get { return _Feature; }
            set { _Feature = value; }
        }
        public string Writer
        {
            get { return _Writer; }
            set { _Writer = value; }
        }
        public string DonVi
        {
            get { return _DonVi; }
            set { _DonVi = value; }
        }
        public bool FromOuter
        {
            get { return _FromOuter; }
            set { _FromOuter = value; }
        }
        #endregion
    }
}