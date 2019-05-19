using CacheEngineShared;

namespace MessageBroker
{
    public class TaoHopDongService : BaseServiceCache<oHongDongKhachHang> { public TaoHopDongService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class TaoHopDongBehavior : BaseServiceCacheBehavior { public TaoHopDongBehavior(object instance) : base(instance) { } }
}
