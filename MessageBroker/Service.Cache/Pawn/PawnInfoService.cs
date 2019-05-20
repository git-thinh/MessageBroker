using CacheEngineShared;

namespace MessageBroker
{
    public class PawnInfoService : BaseServiceCache<oPawnInfo> { public PawnInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class PawnInfoBehavior : BaseServiceCacheBehavior { public PawnInfoBehavior(object instance) : base(instance) { } }
}
