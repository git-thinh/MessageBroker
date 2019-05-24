using CacheEngineShared;

namespace MessageBroker
{
    public class ProfileInfoService : BaseServiceCache<oProfileInfo> { public ProfileInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class ProfileInfoBehavior : BaseServiceCacheBehavior { public ProfileInfoBehavior(object instance) : base(instance) { } }

    public class ProfileInfoController : BaseController
    {
        static ProfileInfoController()
        {
            initCacheService(_API_CONST.PROFILE_INFO);
        }
    }
}
