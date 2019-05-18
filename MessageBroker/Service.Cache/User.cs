using CacheEngineShared;

namespace MessageBroker
{
    public class oUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class oUserService : BaseServiceCache<oUser>
    {
        public oUserService(IDataflowSubscribers dataflow, oCacheField[] cacheFields) : base(dataflow, cacheFields)
        {
            this.insertItems(new oUser[] {
                new oUser(){ Password = "123", UserName="admin" },
                new oUser(){ Password = "123", UserName="user" },
            });
        }
    }

    public class oUserBehavior : BaseServiceCacheBehavior { public oUserBehavior(object instance) : base(instance) { } }
}
