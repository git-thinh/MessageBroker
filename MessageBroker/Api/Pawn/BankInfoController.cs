using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace MessageBroker
{
    public class BankInfoService : BaseServiceCache<oBankInfo> { public BankInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class BankInfoBehavior : BaseServiceCacheBehavior { public BankInfoBehavior(object instance) : base(instance) { } }

    public class BankInfoController : BaseController
    {
        static BankInfoController()
        {
            initCacheService(_API_CONST.BANK_INFO); 
        }
    }
}
