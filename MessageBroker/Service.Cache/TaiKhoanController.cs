using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Web.Http;

namespace MessageBroker
{
    public class TaiKhoanController : ApiController
    {
        static readonly ICacheService _cache;
        static void initData()
        {
            ObjectCache cache = MemoryCache.Default;
            string key = Guid.NewGuid().ToString();
            cache.Set(key, new oTaiKhoan[] {
                new oTaiKhoan(){ TaiKHoanId="1", MatKhau = "123", TenTaiKhoan="admin", MaThietBiTruyCap = Guid.NewGuid().ToString(), NhomKH="XE_OM_CONG_NGHE" },
                new oTaiKhoan(){ TaiKHoanId="2", MatKhau = "123", TenTaiKhoan="user", MaThietBiTruyCap = Guid.NewGuid().ToString(), NhomKH="ECPAY" },
            }, new CacheItemPolicy());
            _cache.insertItemsByCacheKey(key);
        }

        static TaiKhoanController()
        {
            _cache = _API_CONST.TAI_KHOAN.initCacheService();
            initData();
        } 

        public oCacheResult get_All()
        {
            oCacheResult result = _cache.getAllJsonReplyCacheKey().getResultByCacheKey();
            return result;
        }

        public oCacheResult post_Login([FromBody]oTaiKhoan user)
        {
            oCacheResult result = _cache
                .executeReplyCacheKey("TenTaiKhoan=\"" + user.TenTaiKhoan + "\" And MatKhau=\"" + user.MatKhau + "\"")
                .getResultByCacheKey();
            return result;
        }

        public oCacheResult post_Search([FromBody]oCacheRequest request)
        {
            request.RequestId = Guid.NewGuid().ToString();
            oCacheResult result = _cache
                .executeReplyCacheKey(request.Conditions)
                .getResultByCacheKey();
            result.Request = request;
            return result;
        }

        public oCacheResult post_AddNew([FromBody]oTaiKhoan item)
        {
            item.TaiKHoanId = Guid.NewGuid().ToString();

            if (string.IsNullOrWhiteSpace(item.TenTaiKhoan))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("TenTaiKhoan is NULL or empty");

            if (string.IsNullOrWhiteSpace(item.MatKhau))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("MatKhau is NULL or empty");

            if (string.IsNullOrWhiteSpace(item.NhomKH))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("NhomKH is NULL or empty");

            oCacheResult result = _cache.insertItemReplyCacheKey(JsonConvert.SerializeObject(item)).getResultByCacheKey();
            return result;
        }
    }
}
