using CacheEngineShared;
using System.Linq;
using System.Web.Http;

namespace MessageBroker
{
    public class PosPushNotifyService : BaseServiceCache<oPosPushNotify> { public PosPushNotifyService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class PosPushNotifyBehavior : BaseServiceCacheBehavior { public PosPushNotifyBehavior(object instance) : base(instance) { } }

    public class PosPushNotifyController : BaseController
    {
        static PosPushNotifyController()
        {
            initCacheService(_API_CONST.POS_PUSH_NOTIFY);
        }
         
        [AttrApiInfo("Thêm mới notify from POS")]
        public oCacheResult post_AddNew([FromBody]oPosPushNotify item)
        {
            if (item == null) return new oCacheResult().ToFailConvertJson("Please check format string json of input.");

            oCacheResult rs = this.sqlExecute<dtoPosPushNotify_addResult, oPosPushNotify>("pos_push_notify_createNew", item);
            rs.Request = null; 
            return rs; 
        }
    }

    public class dtoPosPushNotify_addResult
    {
        public long ID { get; set; }
        public long User_ID { get; set; }
        public long Pawn_Id { get; set; }
    }
}
