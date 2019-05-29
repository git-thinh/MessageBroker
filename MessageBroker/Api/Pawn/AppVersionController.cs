using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace MessageBroker
{
    public class AppVersionService : BaseServiceCache<oAppVersion> { public AppVersionService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class AppVersionBehavior : BaseServiceCacheBehavior { public AppVersionBehavior(object instance) : base(instance) { } }

    public class AppVersionController : BaseController
    {
        static AppVersionController()
        {
            initCacheService(_API_CONST.APP_VERSION); 
        }
    }
}
