using CacheEngineShared;
using System;

namespace MessageBroker
{
    public class CustomerInfoService : BaseServiceCache<oCustomerInfo> { public CustomerInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class CustomerInfoBehavior : BaseServiceCacheBehavior { public CustomerInfoBehavior(object instance) : base(instance) { } }
}
