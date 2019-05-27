using CacheEngineShared;
using MessageShared;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using TuesPechkin;

namespace MessageBroker
{
    public class JobCacheRefresh : JobBase
    {
        public override void freeResource() { refreshCacheStop(); }

        private static bool _inited = false;

        static string PORT_CACHE_STORE = ConfigurationManager.AppSettings["PORT_CACHE_STORE"];
        static ConcurrentDictionary<string, ICacheService> _caches = new ConcurrentDictionary<string, ICacheService>() { };

        static void refreshCache_init()
        {
            string[] rounters = typeof(_API_CONST).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                        .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType.Name == "String")
                        .Select(x => x.GetRawConstantValue() as string)
                        .Where(x => x.Contains("_"))
                        .ToArray();
            for (int i = 0; i < rounters.Length; i++)
            {
                string api_name = rounters[i];
                try
                {
                    ChannelFactory<ICacheService> factory = new ChannelFactory<ICacheService>(new BasicHttpBinding(),
                        new EndpointAddress("http://localhost:" + PORT_CACHE_STORE + "/" + api_name + "/"));
                    ICacheService cache = factory.CreateChannel();
                    _caches.TryAdd(api_name, cache);
                }
                catch { }
            }
        }

        static void refreshCacheStop()
        {
        }

        ////////////////////////////////////////////////////////////////////
        /// 

        public override void execute()
        {
            if (!_inited)
            {
                _inited = true;
                refreshCache_init();
                return;
            }

            foreach (var kv in _caches)
                kv.Value.initDataFromDbStore(kv.Key + "_cacheInitData", true);
        }
    }
}
