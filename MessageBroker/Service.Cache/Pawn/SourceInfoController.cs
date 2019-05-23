using CacheEngineShared;

namespace MessageBroker
{
    public class SourceInfoService : BaseServiceCache<oSourceInfo> { public SourceInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class SourceInfoBehavior : BaseServiceCacheBehavior { public SourceInfoBehavior(object instance) : base(instance) { } }

    public class SourceInfoController : BaseController
    {
        static SourceInfoController()
        {
            _cache = _API_CONST.SHOP_INFO.initCacheService(); 
            m_initDataFromDbStore = "source_info_cacheInitData";
            
        }
    }
}
