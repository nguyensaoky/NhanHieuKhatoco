using System;

namespace DotNetNuke.Modules.QLCS
{
    public class CaSauInfo
    {
        #region Vars
        private Int32 _IDCaSau;
        private String _MaSo;
        private int _GioiTinh;
        private Boolean? _Giong;
        private Int32? _LoaiCa;
        private DateTime? _NgayNo;
        private DateTime? _NgayXuongChuong;
        private Int32? _NguonGoc;
        private Int32? _Chuong;
        private Int32? _CaMe;
        private Int32? _Status;
        private String _GhiChu;

        #endregion

        #region Property
        public Int32 IDCaSau
        {
            get { return _IDCaSau; }
            set { _IDCaSau = value; }
        }

        public String MaSo
        {
            get { return _MaSo; }
            set { _MaSo = value; }
        }

        public int GioiTinh
        {
            get { return _GioiTinh; }
            set { _GioiTinh = value; }
        }

        public Boolean? Giong
        {
            get { return _Giong; }
            set { _Giong = value; }
        }

        public Int32? LoaiCa
        {
            get { return _LoaiCa; }
            set { _LoaiCa = value; }
        }

        public DateTime? NgayNo
        {
            get { return _NgayNo; }
            set { _NgayNo = value; }
        }

        public DateTime? NgayXuongChuong
        {
            get { return _NgayXuongChuong; }
            set { _NgayXuongChuong = value; }
        }

        public Int32? NguonGoc
        {
            get { return _NguonGoc; }
            set { _NguonGoc = value; }
        }

        public Int32? Chuong
        {
            get { return _Chuong; }
            set { _Chuong = value; }
        }

        public Int32? CaMe
        {
            get { return _CaMe; }
            set { _CaMe = value; }
        }

        public Int32? Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public String GhiChu
        {
            get { return _GhiChu; }
            set { _GhiChu = value; }
        }

        #endregion

    }
}