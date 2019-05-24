using CacheEngineShared;

namespace MessageBroker
{
    public class SourceInfoService : BaseServiceCache<oSourceInfo> { public SourceInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class SourceInfoBehavior : BaseServiceCacheBehavior { public SourceInfoBehavior(object instance) : base(instance) { } }

    public class SourceInfoController : BaseController
    {
        static SourceInfoController()
        {
            initCacheService(_API_CONST.SOURCE_INFO);         
        }
    }
}
