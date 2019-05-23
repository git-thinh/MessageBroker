using CacheEngineShared;

namespace MessageBroker
{
    public class ShopInfoService : BaseServiceCache<oShopInfo> { public ShopInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class ShopInfoBehavior : BaseServiceCacheBehavior { public ShopInfoBehavior(object instance) : base(instance) { } }

    public class ShopInfoController : BaseController
    {
        static ShopInfoController()
        {
            _cache = _API_CONST.SHOP_INFO.initCacheService();
            m_initDataFromDbStore = "shop_info_cacheInitData";
        }
    }
}
