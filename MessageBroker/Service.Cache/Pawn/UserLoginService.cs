using CacheEngineShared;
using System;

namespace MessageBroker
{
    public class UserLoginService : BaseServiceCache<oUserLogin> { public UserLoginService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class UserLoginBehavior : BaseServiceCacheBehavior { public UserLoginBehavior(object instance) : base(instance) { } }
}
