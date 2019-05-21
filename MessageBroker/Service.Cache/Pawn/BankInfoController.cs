using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace MessageBroker
{
    public class BankInfoController : BaseController
    {
        static BankInfoController()
        {
            _cache = _API_CONST.BANK_INFO.initCacheService();
            m_initDataFromDbStore = "[dbo].[mobi_bank_info_cacheInitData]";
        }
    }
}
