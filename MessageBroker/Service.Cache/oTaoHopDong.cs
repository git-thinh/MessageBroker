using CacheEngineShared;

namespace MessageBroker
{
    #region [ MODEL ]

    public enum oTaiSanLoai
    {
        CMT = 1,
        GIAY_PHEP_LAI_XE = 2,
        GIAY_TO_XE = 3,
        GIAY_TO_KHAC = 99
    }

    public enum oTaiKhoanLoai
    {
        NGAN_HANG = 1,
        VI = 2
    }

    public class oTaiKhoanThankToan
    {
        public oTaiKhoanLoai Loai { set; get; }
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
        public oNguoiLienHe[] SoHoKhau { set; get; }
    }

    public enum oHopDongTrangThai
    {
        HOP_DONG_NHAP_DOI_DUYET = 1,
        HOP_DONG_DA_DUYET_DOI_GIAI_NGAN = 2,
        HOP_DONG_THANH_CONG = 3,
    }
    
    public class oHongDongKhachHang
    {
        public oHopDongTrangThai TrangThai { get; set; }
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string ChanDungKH_Img { get; set; }
        public string[] SoHoKhau_Img { get; set; }
        public string HoaDonDien_Img { get; set; }
        public string[] TaiSan_Img { get; set; }
        public oKhachHangThanNhan ThongTinThanNhan { get; set; }
        public oTaiSanLoai[] TaiSan { get; set; }
        public string MaCuaHangTatToan { get; set; }
        public string TenCuaHangTatToan { get; set; }
    }

    #endregion

    public class oTaoHopDongService : BaseServiceCache<oUser>
    {
        public oTaoHopDongService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel)
        {
            this.insertItems(new oHongDongKhachHang[] {
                new oHongDongKhachHang(){
                    ChanDungKH_Img = "",
                    HoaDonDien_Img = "",
                    MaCuaHangTatToan = "001",
                    TenCuaHangTatToan = "Số 22 Khương Hạ - Q. Thanh Xuân - Hà Nội",
                    MaKH = "000111",
                    SoHoKhau_Img = new string[]{ },
                    TaiSan = new oTaiSanLoai[]{ oTaiSanLoai.CMT, oTaiSanLoai.GIAY_PHEP_LAI_XE, oTaiSanLoai.GIAY_TO_KHAC, oTaiSanLoai.GIAY_TO_XE },
                    TaiSan_Img = new string[]{ },
                    TenKH = "Nguyễn Song Toàn",
                    ThongTinThanNhan = new oKhachHangThanNhan(){
                        LangRieng = new oNguoiLienHe[]{
                            new oNguoiLienHe(){ DienThoai = "0948003456", HoTen="Nguyễn Văn Thịnh" },
                            new oNguoiLienHe(){ DienThoai = "0975600710", HoTen="Nguyễn Cẩm Tú" },
                        },
                        SoHoKhau = new oNguoiLienHe[]{
                            new oNguoiLienHe(){ DienThoai = "0123456789", HoTen="Nguyễn Hoàng Doanh" },
                            new oNguoiLienHe(){ DienThoai = "", HoTen="Nguyễn Hồng Giang" },
                        }
                    },
                },
                new oHongDongKhachHang(){
                    ChanDungKH_Img = "",
                    HoaDonDien_Img = "",
                    MaCuaHangTatToan = "001",
                    TenCuaHangTatToan = "Số 580 Trường Chinh - Q. Đống Đa - Hà Nội.",
                    MaKH = "000111",
                    SoHoKhau_Img = new string[]{ },
                    TaiSan = new oTaiSanLoai[]{ oTaiSanLoai.CMT, oTaiSanLoai.GIAY_PHEP_LAI_XE, oTaiSanLoai.GIAY_TO_KHAC, oTaiSanLoai.GIAY_TO_XE },
                    TaiSan_Img = new string[]{ },
                    TenKH = "Nguyễn Văn A",
                    ThongTinThanNhan = new oKhachHangThanNhan(){
                        LangRieng = new oNguoiLienHe[]{
                            new oNguoiLienHe(){ DienThoai = "0948003456", HoTen="Nguyễn Văn Thịnh" },
                            new oNguoiLienHe(){ DienThoai = "0975600710", HoTen="Nguyễn Cẩm Tú" },
                        },
                        SoHoKhau = new oNguoiLienHe[]{
                            new oNguoiLienHe(){ DienThoai = "0123456789", HoTen="Nguyễn Hoàng Doanh" },
                            new oNguoiLienHe(){ DienThoai = "", HoTen="Nguyễn Hồng Giang" },
                        }
                    },
                }
            });
        }
    }

    public class oTaoHopDongBehavior : BaseServiceCacheBehavior { public oTaoHopDongBehavior(object instance) : base(instance) { } }
}
