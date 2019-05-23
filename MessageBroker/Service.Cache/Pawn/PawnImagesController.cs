using CacheEngineShared;

namespace MessageBroker
{
    public class PawnImagesService : BaseServiceCache<oPawnImages> { public PawnImagesService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class PawnImagesBehavior : BaseServiceCacheBehavior { public PawnImagesBehavior(object instance) : base(instance) { } }

    public class PawnImagesController : BaseController
    {
        static PawnImagesController()
        {
            _cache = _API_CONST.SHOP_INFO.initCacheService();
            m_initDataFromDbStore = "shop_info_cacheInitData";
        }
    }
}
