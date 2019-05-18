using Grpc.Core;
using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace MessageShared
{ 
    public static class CacheFindProvider
    {
        public static mCacheService.mCacheServiceClient init()
        {
            try
            {
                string HOST_CACHE_FIND = ConfigurationManager.AppSettings["HOST_CACHE_FIND"];
                string PORT_CACHE_FIND = ConfigurationManager.AppSettings["PORT_CACHE_FIND"];

                Channel channel = new Channel(HOST_CACHE_FIND + ":" + PORT_CACHE_FIND, ChannelCredentials.Insecure);

                var client = new mCacheService.mCacheServiceClient(channel);

                //client.writeLogText(new mLogText() { Text = Guid.NewGuid().ToString() }); 
                //channel.ShutdownAsync().Wait();

                return client;
            }
            catch { }
            return null;
        }

        public static mCacheReply Find(this mCacheService.mCacheServiceClient cache, mCacheRequest request)
        {
            try
            {
                if (cache != null)
                {
                   return cache.Send(request);
                }
            }
            catch { }
            return null;
        }
    }
}
