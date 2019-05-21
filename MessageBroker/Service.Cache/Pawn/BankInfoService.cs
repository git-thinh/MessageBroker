using CacheEngineShared;
using System;

namespace MessageBroker
{
    public class BankInfoService : BaseServiceCache<oBankInfo> { public BankInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class BankInfoBehavior : BaseServiceCacheBehavior { public BankInfoBehavior(object instance) : base(instance) { } }
}
