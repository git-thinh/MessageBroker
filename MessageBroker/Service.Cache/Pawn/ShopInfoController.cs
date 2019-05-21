using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace MessageBroker
{
    public class ShopInfoController : BaseController
    {
        static ShopInfoController()
        {
            _cache = _API_CONST.SHOP_INFO.initCacheService();
            m_initDataFromDbStore = "dbo.mobi_shop_info_cacheInitData";
        }
    }
}
