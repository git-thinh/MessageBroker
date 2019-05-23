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
            _cache = _API_CONST.BANK_INFO.initCacheService();
            //m_initDataFromDbStore = "[dbo].[mobi_bank_info_cacheInitData]";
            m_initDataFromDbStore = "bank_info_cacheInitData";
        }
    }
}
