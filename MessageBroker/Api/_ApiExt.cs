using CacheEngineShared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    public static class _ApiExt
    {
        public static Dictionary<string, string> GetQueryStrings(this HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs()
                          .ToDictionary(kv => kv.Key, kv => kv.Value,
                               StringComparer.OrdinalIgnoreCase);
        }

        //public static void initCacheService(this ConcurrentDictionary<string, ICacheService> storeCache, string api_name)
        //{
        //    try
        //    {
        //        int PORT_CACHE_STORE = int.Parse(ConfigurationManager.AppSettings["PORT_CACHE_STORE"]);
        //        ChannelFactory<ICacheService> factory = new ChannelFactory<ICacheService>(new BasicHttpBinding(),
        //            new EndpointAddress("http://localhost:" + PORT_CACHE_STORE + "/" + api_name + "/"));
        //        var cache = factory.CreateChannel();
        //        if (storeCache != null) {
        //            if (storeCache.ContainsKey(api_name))
        //            {
        //                storeCache[api_name] = cache;
        //            }
        //            else
        //            {
        //                storeCache.TryAdd(api_name, cache);
        //            }
        //        }
        //    }
        //    catch { }
        //}

        //public static ICacheService initCacheService(this string _api_name)
        //{
        //    try
        //    {
        //        int PORT_CACHE_STORE = int.Parse(ConfigurationManager.AppSettings["PORT_CACHE_STORE"]);
        //        ChannelFactory<ICacheService> factory = new ChannelFactory<ICacheService>(new BasicHttpBinding(),
        //            new EndpointAddress("http://localhost:" + PORT_CACHE_STORE + "/" + _api_name + "/"));
        //        return factory.CreateChannel();
        //    }
        //    catch { }
        //    return null;
        //}
    }
}
