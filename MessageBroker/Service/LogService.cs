using CSharpTest.Net.RpcLibrary;
using Google.ProtocolBuffers.Rpc;
using MessageShared;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MessageBroker
{
    internal class mLogServiceAnonymous : ImUpdateService
    {
        private readonly IDataflowSubscribers _dataflow;

        public mLogServiceAnonymous(IDataflowSubscribers dataflow) { this._dataflow = dataflow; }

        public mLogResponse Send(mLogRequest logRequest)
        {
            Console.WriteLine("-> client send: {0}", logRequest.Text);
            _dataflow.Enqueue(new JobLogPrintOut(logRequest.Text)).Wait();
            return mLogResponse.DefaultInstance;
            //return mLogResponse.CreateBuilder().SetMessage("Server: " + logRequest.Text).Build();
        }
    }

    public class LogService
    {
        public static void Start(int port, IDataflowSubscribers dataflow) {
            Guid iid = Marshal.GenerateGuidForType(typeof(ImUpdateService));
            RpcServer.CreateRpc(iid, new mLogService.ServerStub(new mLogServiceAnonymous(dataflow)))
                //.AddAuthNegotiate()
                .AddAuthentication(RpcAuthentication.RPC_C_AUTHN_NONE)
                .AddAuthNegotiate()
                .AddProtocol("ncacn_ip_tcp", port.ToString())
                //.AddProtocol("ncalrpc", "Greeter")
                .StartListening();
        }
    }
}
