using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace MessageBroker
{
    public class CustomerInfoController : BaseController
    {
        static CustomerInfoController()
        {
            _cache = _API_CONST.CUSTOMER_INFO.initCacheService();
            m_initDataFromDbStore = "dbo.mobi_customer_info_cacheInitData";
        }
    }
}
