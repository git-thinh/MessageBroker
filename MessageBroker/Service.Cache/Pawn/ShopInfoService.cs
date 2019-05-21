using CacheEngineShared;
using System;

namespace MessageBroker
{
    public class ShopInfoService : BaseServiceCache<oShopInfo> { public ShopInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class ShopInfoBehavior : BaseServiceCacheBehavior { public ShopInfoBehavior(object instance) : base(instance) { } }
}
