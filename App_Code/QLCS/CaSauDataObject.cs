using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using DotNetNuke.NewsProvider;
using System.ComponentModel;
using System.Globalization;

namespace DotNetNuke.Modules.QLCS
{
    public class CaSauDataObject
    {
        public CaSauDataObject() { }

        public static string GetStandardSQLOrder(string sortBy)
        {
            if (sortBy == "")
                return "";
            string[] arrSortBy = sortBy.Split(new char[]{' '});
            if (arrSortBy.Length <= 2)
            {
                return sortBy;
            }
            else if (arrSortBy.Length == 3)
            {
                if (((string)(arrSortBy[1])).ToUpper() == "DESC")
                {
                    return arrSortBy[0] + " ASC"; 
                }
                else
                {
                    return arrSortBy[0] + " DESC"; 
                }
            }
            return sortBy;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetVatTu(string LVT, string sortBy)
        {
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadVatTu_HienTai(LVT, GetStandardSQLOrder(sortBy));
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable Get_HienTai(string WhereClause, string sortBy, int startIndex, int pageSize)
        {
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadPageCaSau_HienTai(WhereClause, GetStandardSQLOrder(sortBy), startIndex, pageSize);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static int Count_HienTai(string WhereClause)
        {
            CaSauController csCont = new CaSauController();
            return csCont.CountCaSau_HienTai(WhereClause);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable Get(string WhereClause, string sortBy, int startIndex, int pageSize, string Date)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            DateTime dDate = DateTime.Parse(Date, ci);
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadPageCaSau(WhereClause, GetStandardSQLOrder(sortBy), startIndex, pageSize, dDate);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static int Count(string WhereClause, string Date)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            DateTime dDate = DateTime.Parse(Date, ci);
            CaSauController csCont = new CaSauController();
            return csCont.CountCaSau(WhereClause, dDate);
        }

        public static DataTable GetMainTable(string WhereClause, string sortBy, int startIndex, int pageSize)
        {
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadPageCaSauMainTable(WhereClause, GetStandardSQLOrder(sortBy), startIndex, pageSize);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static int CountMainTable(string WhereClause)
        {
            CaSauController csCont = new CaSauController();
            return csCont.CountCaSauMainTable(WhereClause);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetCaChet_HienTai(string WhereClause, string sortBy, int startIndex, int pageSize)
        {
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadPageCaChet_HienTai(WhereClause, GetStandardSQLOrder(sortBy), startIndex, pageSize);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetCaChetDelete(string WhereClause, string sortBy, int startIndex, int pageSize)
        {
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadPageCaChetDelete(WhereClause, GetStandardSQLOrder(sortBy), startIndex, pageSize);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static int CountCaChet_HienTai(string WhereClause)
        {
            CaSauController csCont = new CaSauController();
            return csCont.CountCaChet_HienTai(WhereClause);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static int CountCaChetDelete(string WhereClause)
        {
            CaSauController csCont = new CaSauController();
            return csCont.CountCaChetDelete(WhereClause);
        }

        //[DataObjectMethod(DataObjectMethodType.Select)]
        //public static int CountCaBan_HienTai(string WhereClause)
        //{
        //    CaSauController csCont = new CaSauController();
        //    return csCont.CountCaSauBan_HienTai(WhereClause);
        //}

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetCaSauDe(string WhereClause, string sortBy, int startIndex, int pageSize)
        {
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadPageCaSauDe(WhereClause, GetStandardSQLOrder(sortBy), startIndex, pageSize);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static int CountCaSauDe(string WhereClause)
        {
            CaSauController csCont = new CaSauController();
            return csCont.CountCaSauDe(WhereClause);
        }

        public static DataTable GetCaSauDeDelete(string WhereClause, string sortBy, int startIndex, int pageSize)
        {
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadPageCaSauDeDelete(WhereClause, GetStandardSQLOrder(sortBy), startIndex, pageSize);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static int CountCaSauDeDelete(string WhereClause)
        {
            CaSauController csCont = new CaSauController();
            return csCont.CountCaSauDeDelete(WhereClause);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetCaSauAn(string WhereClause, string sortBy, int startIndex, int pageSize)
        {
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadPageCaSauAn(WhereClause, GetStandardSQLOrder(sortBy), startIndex, pageSize);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static int CountCaSauAn(string WhereClause)
        {
            CaSauController csCont = new CaSauController();
            return csCont.CountCaSauAn(WhereClause);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetGietMoCa(string WhereClause, string sortBy, int startIndex, int pageSize)
        {
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadPageGietMoCa(WhereClause, GetStandardSQLOrder(sortBy), startIndex, pageSize);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static int CountGietMoCa(string WhereClause)
        {
            CaSauController csCont = new CaSauController();
            return csCont.CountGietMoCa(WhereClause);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetBienDongGroup(string WhereClause, string sortBy, int startIndex, int pageSize)
        {
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadPageBienDongGroup(WhereClause, GetStandardSQLOrder(sortBy), startIndex, pageSize);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static int CountBienDongGroup(string WhereClause)
        {
            CaSauController csCont = new CaSauController();
            return csCont.CountBienDongGroup(WhereClause);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetNote(string WhereClause, string sortBy, int startIndex, int pageSize)
        {
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadPageNote(WhereClause, GetStandardSQLOrder(sortBy), startIndex, pageSize);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static int CountNote(string WhereClause)
        {
            CaSauController csCont = new CaSauController();
            return csCont.CountNote(WhereClause);
        }

        public static DataTable GetNoteDelete(string WhereClause, string sortBy, int startIndex, int pageSize)
        {
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadPageNoteDelete(WhereClause, GetStandardSQLOrder(sortBy), startIndex, pageSize);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static int CountNoteDelete(string WhereClause)
        {
            CaSauController csCont = new CaSauController();
            return csCont.CountNoteDelete(WhereClause);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetBienDong(string WhereClause, string sortBy, int startIndex, int pageSize)
        {
            CaSauController csCont = new CaSauController();
            DataTable result = csCont.LoadPageBienDong(WhereClause, GetStandardSQLOrder(sortBy), startIndex, pageSize);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static int CountBienDong(string WhereClause)
        {
            CaSauController csCont = new CaSauController();
            return csCont.CountBienDong(WhereClause);
        }
    }
}