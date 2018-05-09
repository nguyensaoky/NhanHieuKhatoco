using System;

namespace DotNetNuke.News
{
    public class CategoryInfo
    {
        #region Constructor
        public CategoryInfo()
        {
            this._CatID = "";
            this._CatName = "";
            this._ParentID = "";
            this._CatCode = "";
            this._Description = "";
            this._OrderNumber = 0;
            this._DesktopListID = 0;
            this._DesktopViewID = 0;
            this._PortalID = 0;
            this._Visible = true;
        }

        #endregion

        #region Vars
        private string _CatID;
        private string _CatName;
        private string _ParentID;
        private string _CatCode;
        private string _Description;
        private int _OrderNumber;
        private int _DesktopListID;
        private int _DesktopViewID;
        private int _NewsID;
        private int _PortalID;
        private bool _Visible;
        #endregion

        #region Property
        public string CatID
        {
            get { return _CatID; }
            set { _CatID = value; }
        }
        public string CatName
        {
            get { return _CatName; }
            set { _CatName = value; }
        }
        public string ParentID
        {
            get { return _ParentID; }
            set { _ParentID = value; }
        }
        public string CatCode
        {
            get { return _CatCode; }
            set { _CatCode = value; }
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
        public int NewsID
        {
            get { return _NewsID; }
            set { _NewsID = value; }
        }
        public int PortalID
        {
            get { return _PortalID; }
            set { _PortalID = value; }
        }
        public bool Visible
        {
            get { return _Visible; }
            set { _Visible = value; }
        }
        #endregion

    }
}