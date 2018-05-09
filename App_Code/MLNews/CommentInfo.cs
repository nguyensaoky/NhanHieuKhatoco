using System;

namespace DotNetNuke.News
{
    public class CommentInfo
    {
        #region Constructor
        public CommentInfo()
        {
            this._ID = 0;
            this._NewsID = 0;
            this._Headline = "";
            this._Content = "";
            this._AuthorEmail = "";
            this._Author = "";
            this._CreatedDate = DateTime.Now;
            this._Status = 0;
            this._ClientIPAddress = "";
            this._ClientHostName = "";
        }

        #endregion

        #region Vars
        private int _ID;
        private int _NewsID;
        private string _Headline;
        private string _Content;
        private string _AuthorEmail;
        private string _Author;
        private DateTime _CreatedDate;
        private int _Status;
        private string _ClientIPAddress;
        private string _ClientHostName;
        #endregion

        #region Property
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public int NewsID
        {
            get { return _NewsID; }
            set { _NewsID = value; }
        }
        public string Headline
        {
            get { return _Headline; }
            set { _Headline = value; }
        }
        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }
        public string AuthorEmail
        {
            get { return _AuthorEmail; }
            set { _AuthorEmail = value; }
        }
        public string Author
        {
            get { return _Author; }
            set { _Author = value; }
        }
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string ClientIPAddress
        {
            get { return _ClientIPAddress; }
            set { _ClientIPAddress = value; }
        }
        public string ClientHostName
        {
            get { return _ClientHostName; }
            set { _ClientHostName = value; }
        }
        #endregion

    }
}