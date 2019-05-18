using CacheEngineShared;

namespace CacheEngine.Test
{
    public class oTest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class oTestService : BaseServiceCache<oTest>
    {
        public oTestService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel)
        {
            this.insertItems(new oTest[] {
                new oTest(){ Password = "123", UserName="admin" },
                new oTest(){ Password = "123", UserName="user" },
            });
        }
    }

    public class oTestBehavior : BaseServiceCacheBehavior { public oTestBehavior(object instance) : base(instance) { } }
}
