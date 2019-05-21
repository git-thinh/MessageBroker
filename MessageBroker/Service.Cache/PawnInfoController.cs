using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Web.Caching;
using System.Web.Http;

namespace MessageBroker
{
    public class PawnInfoController : ApiController
    {
        static readonly ICacheService _cache;
        static void initData()
        {
            ////            ObjectCache cache = MemoryCache.Default;
            ////            string key = Guid.NewGuid().ToString();
            ////            cache.Set(key, new oHopDongKhachHang[] {
            ////                new oHopDongKhachHang(){
            ////{
            ////            "PawnID": 1140369,
            ////            "PawnCode": "HĐCC/HN39HTM/1810/39",
            ////            "CustomerName": "Dương Văn Dân",
            ////            "Phone": "0903511768",
            ////            "AssetName": " Xe máy HONDA Lead 110cc Fi 2010",
            ////            "Status": 14,
            ////            "StatusStr": "Quá hạn 137 ngày",
            ////            "MoneyCurrent": 4666667
            ////        },
            ////        {
            ////            "PawnID": 1141699,
            ////            "PawnCode": "HĐCC/HN39HTM/1810/80",
            ////            "CustomerName": "Nguyễn Minh Hùng",
            ////            "Phone": "0944184444",
            ////            "AssetName": " Dây chuyền Vàng mặt Phật 9.7 chỉ",
            ////            "Status": 14,
            ////            "StatusStr": "Quá hạn 133 ngày",
            ////            "MoneyCurrent": 20000000
            ////        },
            ////        {
            ////            "PawnID": 1151720,
            ////            "PawnCode": "HĐCC/HN39HTM/1811/84",
            ////            "CustomerName": "Trần Ngọc Khoa",
            ////            "Phone": "0936020788",
            ////            "AssetName": " Laptop MSI I7 GL62/Core i7-7700HQ/RAM 8GB/HDD 1TB 2017",
            ////            "Status": 14,
            ////            "StatusStr": "Quá hạn 133 ngày",
            ////            "MoneyCurrent": 10000000
            ////        },
            ////        {
            ////            "PawnID": 1150105,
            ////            "PawnCode": "HĐCC/HN39HTM/1811/42",
            ////            "CustomerName": "Mai Thị Tuyết",
            ////            "Phone": "0962888788",
            ////            "AssetName": " Laptop ASUS  K43S/Core  i3-2310m /RAM 4GB/HDD 500GB",
            ////            "Status": 14,
            ////            "StatusStr": "Quá hạn 132 ngày",
            ////            "MoneyCurrent": 600000
            ////        },
            ////        {
            ////            "PawnID": 1157546,
            ////            "PawnCode": "HĐCC/HN39HTM/1811/237",
            ////            "CustomerName": "Trần Thanh Giang ",
            ////            "Phone": "0365196707",
            ////            "AssetName": " Laptop Surface i5 Pro 4 /Core i5-6300U/Ram 4GB/Ssd 128Gb 2016",
            ////            "Status": 14,
            ////            "StatusStr": "Quá hạn 132 ngày",
            ////            "MoneyCurrent": 7300000
            ////        },
            ////        {
            ////            "PawnID": 1109313,
            ////            "PawnCode": "HĐCC/HN39HTM/1806/118",
            ////            "CustomerName": "Phạm Ngọc Tuấn Linh",
            ////            "Phone": "0963019994",
            ////            "AssetName": " Laptop DELL  Inspiron 5547 /Core i7-4510CPU @ 2.00GHZ, sony corporation  intel i3-3217CPU@1.8GHZ",
            ////            "Status": 14,
            ////            "StatusStr": "Quá hạn 131 ngày",
            ////            "MoneyCurrent": 7582000
            ////        },
            ////        {
            ////            "PawnID": 1142821,
            ////            "PawnCode": "HĐCC/HN39HTM/1810/108",
            ////            "CustomerName": "Phạm Văn Dũng",
            ////            "Phone": "0913221182",
            ////            "AssetName": " Xe máy HONDA Vision (phom 2015) 2016",
            ////            "Status": 14,
            ////            "StatusStr": "Quá hạn 131 ngày",
            ////            "MoneyCurrent": 17000000
            ////        },
            ////        {
            ////            "PawnID": 1149218,
            ////            "PawnCode": "HĐCC/HN39HTM/1811/11",
            ////            "CustomerName": "Hoàng Thị Hường",
            ////            "Phone": "0986766594",
            ////            "AssetName": " Laptop DELL  Inspiron 3567/ Core i3-6006u/RAM 4GB/HDD 500GB",
            ////            "Status": 14,
            ////            "StatusStr": "Quá hạn 131 ngày",
            ////            "MoneyCurrent": 4500000
            ////        },
            ////        {
            ////            "PawnID": 1129605,
            ////            "PawnCode": "HĐCC/HN39HTM/1809/15",
            ////            "CustomerName": "Nguyễn Văn Duy",
            ////            "Phone": "0977351124",
            ////            "AssetName": " nhẫn vàng 10k 2,457 chỉ (đá 0,169 chỉ vàng 2,288)",
            ////            "Status": 14,
            ////            "StatusStr": "Quá hạn 128 ngày",
            ////            "MoneyCurrent": 1800000
            ////        },
            ////        {
            ////            "PawnID": 1142258,
            ////            "PawnCode": "HĐCC/HN39HTM/1810/91",
            ////            "CustomerName": "Vũ Văn Tú",
            ////            "Phone": "0988356156",
            ////            "AssetName": " Laptop DELL  Inspiron 3558/ Core i5-5200u/RAM 4GB/HDD 500GB",
            ////            "Status": 14,
            ////            "StatusStr": "Quá hạn 128 ngày",
            ////            "MoneyCurrent": 3500000
            ////        }

            ////                    MaTaiKhoanTaoHD = "1",
            ////                    TrangThai = oHopDongTrangThai.HOP_DONG_NHAP_DOI_DUYET,
            ////                    ChanDungKH_Img = "",
            ////                    HoaDonDien_Img = "",
            ////                    MaCuaHangTatToan = "001",
            ////                    TenCuaHangTatToan = "Số 22 Khương Hạ - Q. Thanh Xuân - Hà Nội",
            ////                    MaKH = "000111",
            ////                    SoHoKhau_Img = new string[]{ },
            ////                    TaiSan = new oTaiSanLoai[]{ oTaiSanLoai.CMT, oTaiSanLoai.GIAY_PHEP_LAI_XE, oTaiSanLoai.GIAY_TO_KHAC, oTaiSanLoai.GIAY_TO_XE },
            ////                    TaiSan_Img = new string[]{ },
            ////                    TenKH = "Nguyễn Song Toàn",
            ////                    ThongTinThanNhan = new oKhachHangThanNhan(){
            ////                        LangRieng = new oNguoiLienHe[]{
            ////                            new oNguoiLienHe(){ DienThoai = "0948003456", HoTen="Nguyễn Văn Thịnh" },
            ////                            new oNguoiLienHe(){ DienThoai = "0975600710", HoTen="Nguyễn Cẩm Tú" },
            ////                        },
            ////                        NguoiThanSoHoKhau = new oNguoiLienHe[]{
            ////                            new oNguoiLienHe(){ DienThoai = "0123456789", HoTen="Nguyễn Hoàng Doanh" },
            ////                            new oNguoiLienHe(){ DienThoai = "", HoTen="Nguyễn Hồng Giang" },
            ////                        }
            ////                    },
            ////                },
            ////                new oHopDongKhachHang(){
            ////                    MaTaiKhoanTaoHD = "2",
            ////                    TrangThai = oHopDongTrangThai.HOP_DONG_NHAP_DOI_DUYET,
            ////                    ChanDungKH_Img = "",
            ////                    HoaDonDien_Img = "",
            ////                    MaCuaHangTatToan = "001",
            ////                    TenCuaHangTatToan = "Số 580 Trường Chinh - Q. Đống Đa - Hà Nội.",
            ////                    MaKH = "000111",
            ////                    SoHoKhau_Img = new string[]{ },
            ////                    TaiSan = new oTaiSanLoai[]{ oTaiSanLoai.CMT, oTaiSanLoai.GIAY_PHEP_LAI_XE, oTaiSanLoai.GIAY_TO_KHAC, oTaiSanLoai.GIAY_TO_XE },
            ////                    TaiSan_Img = new string[]{ },
            ////                    TenKH = "Nguyễn Văn A",
            ////                    ThongTinThanNhan = new oKhachHangThanNhan(){
            ////                        LangRieng = new oNguoiLienHe[]{
            ////                            new oNguoiLienHe(){ DienThoai = "0948003456", HoTen="Nguyễn Văn Thịnh" },
            ////                            new oNguoiLienHe(){ DienThoai = "0975600710", HoTen="Nguyễn Cẩm Tú" },
            ////                        },
            ////                        NguoiThanSoHoKhau = new oNguoiLienHe[]{
            ////                            new oNguoiLienHe(){ DienThoai = "0123456789", HoTen="Nguyễn Hoàng Doanh" },
            ////                            new oNguoiLienHe(){ DienThoai = "", HoTen="Nguyễn Hồng Giang" },
            ////                        }
            ////                    },
            ////                }
            ////            }, new CacheItemPolicy());
            ////            _cache.insertItemsByCacheKey(key);
        }
        static PawnInfoController()
        {
            _cache = _API_CONST.PAWN_INFO.initCacheService();
            initData();
        }

        public oModelInfo get_model_Attrs()
        {
            oModelInfo model = new oModelInfo();
            Type typeModel = typeof(oPawnInfo);
            model.Name = typeModel.Name;

            object[] attrsModel = typeModel.GetCustomAttributes(true);
            foreach (var attr in attrsModel)
            {
                AttrModelInfo attrExt = attr as AttrModelInfo;
                if (attrExt != null) 
                    model.Title = attrExt.GetTitle(); 
            }

            PropertyInfo[] props = typeModel.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrsField = prop.GetCustomAttributes(true);
                foreach (object attr in attrsField)
                {
                    AttrFieldInfo attrExt = attr as AttrFieldInfo;
                    if (attrExt != null)
                        model.Properties.Add(new oModelFielInfo(prop.Name, attrExt));
                }
            }

            return model;
        }

        //public oCacheResult get_All()
        //{
        //    oCacheResult result = _cache.getAllJsonReplyCacheKey().getResultByCacheKey();
        //    return result;
        //}

        //public dynamic[] get_ds_Loai_TaiSan()
        //{
        //    var type = typeof(oTaiSanLoai);
        //    var data = Enum.GetNames(type).Select(name => new { Id = (int)Enum.Parse(type, name), Name = name }).ToArray();
        //    return data;
        //}

        //public dynamic[] get_ds_TrangThai_HopDong()
        //{
        //    var type = typeof(oHopDongTrangThai);
        //    var data = Enum.GetNames(type).Select(name => new { Id = (int)Enum.Parse(type, name), Name = name }).ToArray();
        //    return data;
        //}

        //public dynamic[] get_ds_Loai_ThanhToan()
        //{
        //    var type = typeof(oThanhToanLoai);
        //    var data = Enum.GetNames(type).Select(name => new { Id = (int)Enum.Parse(type, name), Name = name }).ToArray();
        //    return data;
        //}

        //public oCacheResult post_Search([FromBody]oCacheRequest request)
        //{
        //    request.RequestId = Guid.NewGuid().ToString();
        //    oCacheResult result = _cache
        //        .executeReplyCacheKey(request.Conditions)
        //        .getResultByCacheKey();
        //    result.Request = request;
        //    return result;
        //}

        //public oCacheResult post_AddNew([FromBody]oHopDongKhachHang item)
        //{
        //    item.MaTaiKhoanTaoHD = Guid.NewGuid().ToString();

        //    if (string.IsNullOrWhiteSpace(item.MaKH))
        //        return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("MaKH is NULL or empty");

        //    if (string.IsNullOrWhiteSpace(item.TenKH))
        //        return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("TenKH is NULL or empty");

        //    if (string.IsNullOrWhiteSpace(item.TenCuaHangTatToan))
        //        return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("TenCuaHangTatToan is NULL or empty");

        //    oCacheResult result = _cache.insertItemReplyCacheKey(JsonConvert.SerializeObject(item)).getResultByCacheKey();
        //    return result;
        //}
    }
}
