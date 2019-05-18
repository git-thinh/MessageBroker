using CacheEngineShared;
using Grpc.Core;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MessageShared;
using Grpc.Core.Utils;
using System.Configuration;

namespace MessageBroker
{
    public class mCacheFindServiceImpl : mCacheService.mCacheServiceBase
    {
        private readonly IDataflowSubscribers _dataflow;
        public mCacheFindServiceImpl(IDataflowSubscribers dataflow) {
            this._dataflow = dataflow;
        }

        public override Task<mCacheReply> Send(mCacheRequest request, ServerCallContext context)
        {
            ICacheFind cache = (ICacheFind)_dataflow.CacheFind;
            mCacheReply rs = cache.Find(request);
            return Task.FromResult(rs);
        }
    }

    public class CacheFindService
    {
        static Server server;
        public static void Start(IDataflowSubscribers dataflow)
        {
            string HOST_CACHE_FIND = ConfigurationManager.AppSettings["HOST_CACHE_FIND"];
            string PORT_CACHE_FIND = ConfigurationManager.AppSettings["PORT_CACHE_FIND"];
            server = new Server()
            {
                Services = { mCacheService.BindService(new mCacheFindServiceImpl(dataflow)) },
                Ports = { new ServerPort(HOST_CACHE_FIND,int.Parse(PORT_CACHE_FIND), ServerCredentials.Insecure) }
            };
            server.Start();
        }

        public static void Stop()
        {
            server.ShutdownAsync().Wait();
        }
    }
}
