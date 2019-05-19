using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Web.Caching;
using System.Web.Http;

namespace MessageBroker
{
    public class TaoHopDongController : ApiController
    {
        static readonly ICacheService _cache;
        static void initData() {
            ObjectCache cache = MemoryCache.Default;
            string key = Guid.NewGuid().ToString();
            cache.Set(key, new oHongDongKhachHang[] {
                new oHongDongKhachHang(){
                    MaTaiKhoanTaoHD = "1",
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
                        NguoiThanSoHoKhau = new oNguoiLienHe[]{
                            new oNguoiLienHe(){ DienThoai = "0123456789", HoTen="Nguyễn Hoàng Doanh" },
                            new oNguoiLienHe(){ DienThoai = "", HoTen="Nguyễn Hồng Giang" },
                        }
                    },
                },
                new oHongDongKhachHang(){
                    MaTaiKhoanTaoHD = "2",
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
                        NguoiThanSoHoKhau = new oNguoiLienHe[]{
                            new oNguoiLienHe(){ DienThoai = "0123456789", HoTen="Nguyễn Hoàng Doanh" },
                            new oNguoiLienHe(){ DienThoai = "", HoTen="Nguyễn Hồng Giang" },
                        }
                    },
                }
            }, new CacheItemPolicy());
            _cache.insertItemsByCacheKey(key);
        }
        static TaoHopDongController()
        {
            _cache = _API_CONST.TAO_HOP_DONG.initCacheService();
            initData();
        }

        public oCacheResult get_All()
        {
            oCacheResult result = _cache.getAllJsonReplyCacheKey().getResultByCacheKey();
            return result;
        }

        public dynamic[] get_ds_Loai_TaiSan()
        {
            var type = typeof(oTaiSanLoai);
            var data = Enum.GetNames(type).Select(name => new { Id = (int)Enum.Parse(type, name), Name = name }).ToArray();
            return data;
        }

        public dynamic[] get_ds_TrangThai_HopDong()
        {
            var type = typeof(oHopDongTrangThai);
            var data = Enum.GetNames(type).Select(name => new { Id = (int)Enum.Parse(type, name), Name = name }).ToArray();
            return data;
        }

        public dynamic[] get_ds_Loai_ThanhToan()
        {
            var type = typeof(oThanhToanLoai);
            var data = Enum.GetNames(type).Select(name => new { Id = (int)Enum.Parse(type, name), Name = name }).ToArray();
            return data;
        }

        public oCacheResult post_AddNew([FromBody]oHongDongKhachHang item)
        {
            item.MaTaiKhoanTaoHD = Guid.NewGuid().ToString();

            if (string.IsNullOrWhiteSpace(item.MaKH))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("MaKH is NULL or empty");

            if (string.IsNullOrWhiteSpace(item.TenKH))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("TenKH is NULL or empty");

            if (string.IsNullOrWhiteSpace(item.TenCuaHangTatToan))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("TenCuaHangTatToan is NULL or empty");

            oCacheResult result = _cache.insertItemReplyCacheKey(JsonConvert.SerializeObject(item)).getResultByCacheKey();
            return result;
        }
    }
}
