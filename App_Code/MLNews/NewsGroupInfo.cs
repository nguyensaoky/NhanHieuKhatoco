using System;

namespace DotNetNuke.News
{
    public class NewsGroupInfo
    {
        #region Constructor
        public NewsGroupInfo()
        {
            this._NewsGroupID = "";
            this._NewsGroupName = "";
            this._NewsGroupCode = "";
            this._Description = "";
            this._OrderNumber = 0;
            this._PortalID = 0;
            this._DesktopListID = 0;
            this._DesktopViewID = 0;
        }

        #endregion

        #region Vars
        private string _NewsGroupID;
        private string _NewsGroupName;
        private string _NewsGroupCode;
        private string _Description;
        private int _OrderNumber;
        private int _PortalID;
        private int _DesktopListID;
        private int _DesktopViewID;
        #endregion

        #region Property
        public string NewsGroupID
        {
            get { return _NewsGroupID; }
            set { _NewsGroupID = value; }
        }
        public string NewsGroupName
        {
            get { return _NewsGroupName; }
            set { _NewsGroupName = value; }
        }
        public string NewsGroupCode
        {
            get { return _NewsGroupCode; }
            set { _NewsGroupCode = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public int OrderNumber
        {
            get { return _OrderNumber; }
            set { _OrderNumber = value; }
        }
        public int PortalID
        {
            get { return _PortalID; }
            set { _PortalID = value; }
        }
        public int DesktopListID
        {
            get { return _DesktopListID; }
            set { _DesktopListID = value; }
        }
        public int DesktopViewID
        {
            get { return _DesktopViewID; }
            set { _DesktopViewID = value; }
        }
        #endregion

    }
}