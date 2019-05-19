using CacheEngineShared;
using System;

namespace MessageBroker
{
    public class TaiKhoanService : BaseServiceCache<oTaiKhoan> { public TaiKhoanService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class TaiKhoanBehavior : BaseServiceCacheBehavior { public TaiKhoanBehavior(object instance) : base(instance) { } }
}
