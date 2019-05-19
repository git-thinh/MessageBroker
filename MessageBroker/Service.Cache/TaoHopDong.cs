namespace MessageBroker
{
    public enum oTaiSanLoai
    {
        CMT = 1,
        GIAY_PHEP_LAI_XE = 2,
        GIAY_TO_XE = 3,
        GIAY_TO_KHAC = 99
    }

    public enum oHopDongTrangThai
    {
        HOP_DONG_NHAP_DOI_DUYET = 1,
        HOP_DONG_DA_DUYET_DOI_GIAI_NGAN = 2,
        HOP_DONG_THANH_CONG = 3,
    }

    public enum oThanhToanLoai
    {
        NGAN_HANG = 1,
        VI_DIEN_TU = 2
    }

    public class oTaiKhoanThankToan
    {
        public oThanhToanLoai Loai { set; get; }
        public string TaiKhoan { set; get; }
        public string DonVi { set; get; }
    }

    public class oNguoiLienHe
    {
        public string HoTen { set; get; }
        public string DienThoai { set; get; }
    }

    public class oKhachHangThanNhan
    {
        public oNguoiLienHe[] LangRieng { set; get; }
        public oNguoiLienHe[] DongNghiep { set; get; }
        public oNguoiLienHe[] NguoiThanSoHoKhau { set; get; }
    }

    public class oHongDongKhachHang
    {
        public string MaTaiKhoanTaoHD { get; set; }
        public string MaTaiKhoanGioiThieu { get; set; }

        public oHopDongTrangThai TrangThai { get; set; }

        public oTaiSanLoai[] TaiSan { get; set; }

        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public int SoGiayTo { get; set; }
        public string ChanDungKH_Img { get; set; }
        public string[] SoHoKhau_Img { get; set; }
        public string HoaDonDien_Img { get; set; }
        public string[] TaiSan_Img { get; set; }
        public oKhachHangThanNhan ThongTinThanNhan { get; set; }
        public string MaCuaHangTatToan { get; set; }
        public string TenCuaHangTatToan { get; set; }
    }

}
