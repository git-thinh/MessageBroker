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
    public class mDbUpdateServiceImpl : mDbUpdateService.mDbUpdateServiceBase
    {
        private readonly IDataflowSubscribers _dataflow;
        public mDbUpdateServiceImpl(IDataflowSubscribers dataflow) {
            this._dataflow = dataflow;
        }

        public override Task<mDbUpdateReply> Send(mDbUpdateRequest request, ServerCallContext context)
        {
            _dataflow.enqueue(new JobDbUpdate(request)).Wait();
            return Task.FromResult(new mDbUpdateReply() { });
        }
    }

    public class DbUpdateSerive
    {
        static Server server;
        public static void Start(IDataflowSubscribers dataflow)
        {
            string HOST_DB_UPDATE = ConfigurationManager.AppSettings["HOST_DB_UPDATE"];
            string PORT_DB_UPDATE = ConfigurationManager.AppSettings["PORT_DB_UPDATE"];
            server = new Server()
            {
                Services = { mCacheService.BindService(new mCacheFindServiceImpl(dataflow)) },
                Ports = { new ServerPort(HOST_DB_UPDATE, int.Parse(PORT_DB_UPDATE), ServerCredentials.Insecure) }
            };
            server.Start();
        }

        public static void Stop()
        {
            server.ShutdownAsync().Wait();
        }
    }
}
