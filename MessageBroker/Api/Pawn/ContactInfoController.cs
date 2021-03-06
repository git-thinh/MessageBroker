﻿using CacheEngineShared;

namespace MessageBroker
{
    public class ContactInfoService : BaseServiceCache<oContactInfo> { public ContactInfoService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class ContactInfoBehavior : BaseServiceCacheBehavior { public ContactInfoBehavior(object instance) : base(instance) { } }

    public class ContactInfoController : BaseController
    {
        static ContactInfoController()
        {
            initCacheService(_API_CONST.CONTACT_INFO);            
        }
    }
}
