using CSharpTest.Net.RpcLibrary;
using Google.ProtocolBuffers.Rpc;
using MessageShared;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MessageBroker
{
    internal class mCacheFindServiceAnonymous : ImCacheService
    {
        private readonly IDataflowSubscribers _dataflow;

        public mCacheFindServiceAnonymous(IDataflowSubscribers dataflow) { this._dataflow = dataflow; }

        public mCacheReply Send(mCacheRequest cacheRequest)
        {

            //string _key = 1;
            //int32 pageSize = 2;
            //int32 pageNumber = 3;
            //string jsonConditions = 4;
            //bool hasCache = 5;

            bool ok = true;
            string output = "[]";
            int countResult = 4;
            int totalItems = 5;

            //_dataflow.Enqueue(new JobDbUpdate(cacheRequest)).Wait();
            //return mLogResponse.DefaultInstance;
            return mCacheReply.CreateBuilder()
                .SetOk(ok)
                .SetRequest(cacheRequest)
                .SetOutput(output)
                .SetCountResult(countResult)
                .SetTotalItems(totalItems)
                .Build();
        }
    }

    public class CacheFindService
    {
        public static void Start(int port, IDataflowSubscribers dataflow)
        {
            Guid iid = Marshal.GenerateGuidForType(typeof(ImCacheService));
            RpcServer.CreateRpc(iid, new mCacheService.ServerStub(new mCacheFindServiceAnonymous(dataflow)))
                //.AddAuthNegotiate()
                .AddAuthentication(RpcAuthentication.RPC_C_AUTHN_NONE)
                .AddAuthNegotiate()
                .AddProtocol("ncacn_ip_tcp", port.ToString())
                //.AddProtocol("ncalrpc", "Greeter")
                .StartListening();
        }
    }
}
