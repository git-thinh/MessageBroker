using CacheEngineShared;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MessageBroker
{
    public class PawnInfoController : BaseController
    {
        static PawnInfoController() {
            _cache = _API_CONST.PAWN_INFO.initCacheService();
            m_initDataFromDbStore = "[dbo].[mobi_pawn_info_cacheInitData]";
        }

        [AttrApiInfo("Danh sách trạng thái của hợp đồng")]
        public dynamic[] get_state_type()
        {
            var type = typeof(oContractStatus);
            var data = Enum.GetNames(type).Select(name => new { Id = (int)Enum.Parse(type, name), Name = name }).ToArray();
            return data;
        }

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

    }
}
