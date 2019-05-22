using CacheEngineShared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    public static class _ApiExt
    {
        public static ICacheService initCacheService(this string _api_name)
        {
            try
            {
                int PORT_CACHE_STORE = int.Parse(ConfigurationManager.AppSettings["PORT_CACHE_STORE"]);
                ChannelFactory<ICacheService> factory = new ChannelFactory<ICacheService>(new BasicHttpBinding(),
                    new EndpointAddress("http://localhost:" + PORT_CACHE_STORE + "/" + _api_name + "/"));
                return factory.CreateChannel();
            }
            catch { }
            return null;
        }
    }
}
