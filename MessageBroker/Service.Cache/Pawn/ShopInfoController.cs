using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace MessageBroker
{
    public class ShopInfoController : ApiController
    {
        static readonly ICacheService _cache; 
        static ShopInfoController()
        {
            _cache = _API_CONST.SHOP_INFO.initCacheService();
            _cache.initDataFromDbStore("dbo.mobi_shop_info_cacheInitData");
        }

        [AttrApiInfo("Danh sách tất cả cửa hàng")]
        public oCacheResult get_All()
        {
            oCacheResult result = _cache.getAllJsonReplyCacheKey().getResultByCacheKey();
            return result;
        }

        [AttrApiInfo("Tìm kiếm cửa hàng", Description = "{\"Conditions\":\" Name.Contain(\"\"a\"\") \"}")]
        public oCacheResult post_Search([FromBody]oCacheRequest request)
        {
            request.RequestId = Guid.NewGuid().ToString();
            oCacheResult result = _cache
                .executeReplyCacheKey(request.Conditions)
                .getResultByCacheKey();
            result.Request = request;
            return result;
        }

    }
}
