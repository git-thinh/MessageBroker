using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace MessageBroker
{
    public class CustomerInfoController : ApiController
    {
        static readonly ICacheService _cache; 
        static CustomerInfoController()
        {
            _cache = _API_CONST.CUSTOMER_INFO.initCacheService();
            _cache.initDataFromDbStore("dbo.mobi_customer_info_cacheInitData");
        }

        [AttrApiInfo("Danh sách tất cả khach hang")]
        public oCacheResult get_All()
        {
            oCacheResult result = _cache.getAllJsonReplyCacheKey().getResultByCacheKey();
            return result;
        }

        [AttrApiInfo("Tìm kiếm khach hang", Description = "{\"Conditions\":\" Name.Contain(\"\"a\"\") \"}")]
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
