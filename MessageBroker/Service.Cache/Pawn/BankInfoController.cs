using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace MessageBroker
{
    public class BankInfoController : ApiController
    {
        static readonly ICacheService _cache; 
        static BankInfoController()
        {
            _cache = _API_CONST.BANK_INFO.initCacheService();
            _cache.initDataFromDbStore("dbo.mobi_bank_info_cacheInitData");
        }

        [AttrApiInfo("Danh sách tất cả ngân hàng")]
        public oCacheResult get_All()
        {
            oCacheResult result = _cache.getAllJsonReplyCacheKey().getResultByCacheKey();
            return result;
        }

        [AttrApiInfo("Tìm kiếm ngân hàng", Description = "{\"Conditions\":\" Name.Contain(\"\"a\"\") \"}")]
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
