using CacheEngineShared;
using Dapper;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MessageBroker
{
    public class PawnInfoService : BaseServiceCache<oPawnInfo> { public PawnInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class PawnInfoBehavior : BaseServiceCacheBehavior { public PawnInfoBehavior(object instance) : base(instance) { } }

    public class PawnInfoController : BaseController
    {
        static PawnInfoController() {
            _cache = _API_CONST.PAWN_INFO.initCacheService();
            m_initDataFromDbStore = "pawn_info_cacheInitData";
        }

        [AttrApiInfo("Danh sách trạng thái của hợp đồng")]
        public dynamic[] get_state_type()
        {
            var type = typeof(oContractStatus);
            var data = Enum.GetNames(type).Select(name => new { Id = (int)Enum.Parse(type, name), Name = name }).ToArray();
            return data;
        }

        [AttrApiInfo("Tạo mã hợp đồng khi mở hợp đồng mới")]
        public string get_pawnCode_createNew() {
            var param = new DynamicParameters();
            //param.Add("@ShopID", 154);// VietPost
            param.Add("@ShopID", 999); // ECPay
            string maxCode;

            using (IDbConnection cnn = new SqlConnection(_DB_CONST.get_connectString_POS()))
            {
                cnn.Open();
                maxCode = cnn.Query<string>("pos.GetMaxPawnIndexByShop", param, null, true, null, CommandType.StoredProcedure).SingleOrDefault();
            }

            int newIndex = 1;
            if (!string.IsNullOrEmpty(maxCode))
            {
                string[] codes = maxCode.Split('/');
                newIndex = Convert.ToInt16(codes.Last()) + 1;
            }

            // Lấy CodeNo dựa vào ShopID = 'DR'
            string codeNo = $"HĐCC/DR/{DateTime.Now.ToString("yyMM")}/{newIndex}";

            return codeNo;
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
