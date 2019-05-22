using CacheEngineShared;

namespace MessageBroker
{
    public class CustomerInfoService : BaseServiceCache<oCustomerInfo> { public CustomerInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class CustomerInfoBehavior : BaseServiceCacheBehavior { public CustomerInfoBehavior(object instance) : base(instance) { } }

    public class CustomerInfoController : BaseController
    {
        static CustomerInfoController()
        {
            _cache = _API_CONST.CUSTOMER_INFO.initCacheService();
            m_initDataFromDbStore = "customer_info_cacheInitData";
        }
    }
}
