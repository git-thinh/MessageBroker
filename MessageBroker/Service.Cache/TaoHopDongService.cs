using CacheEngineShared;

namespace MessageBroker
{
    public class TaoHopDongService : BaseServiceCache<oHongDongKhachHang>
    {
        public TaoHopDongService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel)
        {
            this.insertItems(new oHongDongKhachHang[] {
                new oHongDongKhachHang(){
                    TaiKhoanId = "1",
                    TrangThai = oHopDongTrangThai.HOP_DONG_NHAP_DOI_DUYET,
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
                    TaiKhoanId = "2",
                    TrangThai = oHopDongTrangThai.HOP_DONG_NHAP_DOI_DUYET,
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

    public class TaoHopDongBehavior : BaseServiceCacheBehavior { public TaoHopDongBehavior(object instance) : base(instance) { } }
}
