using CacheEngineShared;

namespace MessageBroker
{
    public class ShopInfoService : BaseServiceCache<oShopInfo> { public ShopInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class ShopInfoBehavior : BaseServiceCacheBehavior { public ShopInfoBehavior(object instance) : base(instance) { } }

    public class ShopInfoController : BaseController
    {
        static ShopInfoController()
        {
            initCacheService(_API_CONST.SHOP_INFO);
        }
    }
}
