using CacheEngineShared;
using CSharpTest.Net.RpcLibrary;
using Google.ProtocolBuffers.Rpc;
using MessageShared;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MessageBroker
{
    internal class mDbUpdateServiceAnonymous : ImDbUpdateService
    {
        private readonly IDataflowSubscribers _dataflow;

        public mDbUpdateServiceAnonymous(IDataflowSubscribers dataflow) { this._dataflow = dataflow; }
         
        public mDbUpdateReply Send(mDbUpdateRequest updateRequest)
        {
            string id = Guid.NewGuid().ToString(), msg = string.Empty;
            bool ok = true;

            _dataflow.Enqueue(new JobDbUpdate(updateRequest)).Wait();

            //return mLogResponse.DefaultInstance;
            return mDbUpdateReply.CreateBuilder()
                .SetOk(ok)
                .SetId(id)
                .SetMessage(msg)
                .SetRequest(updateRequest)
                .Build();
        }
    }

    public class DbUpdateService
    {
        public static void Start(int port, IDataflowSubscribers dataflow) {
            Guid iid = Marshal.GenerateGuidForType(typeof(ImDbUpdateService));
            RpcServer.CreateRpc(iid, new mDbUpdateService.ServerStub(new mDbUpdateServiceAnonymous(dataflow)))
                //.AddAuthNegotiate()
                .AddAuthentication(RpcAuthentication.RPC_C_AUTHN_NONE)
                .AddAuthNegotiate()
                .AddProtocol("ncacn_ip_tcp", port.ToString())
                //.AddProtocol("ncalrpc", "Greeter")
                .StartListening();
        }
    }
}
