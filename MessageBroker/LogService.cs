using CSharpTest.Net.RpcLibrary;
using Google.ProtocolBuffers.Rpc;
using MessageShared.Log;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MessageBroker
{
    internal class mLogServiceAnonymous : ImLogService
    {
        public mLogResponse Send(mLogRequest logRequest)
        {
            Console.WriteLine("-> client send: {0}", logRequest.Text);
            return mLogResponse.DefaultInstance;
            //return mLogResponse.CreateBuilder().SetMessage("Server: " + logRequest.Text).Build();
        }
    }

    public class LogService
    {
        public static void Start(int port) {
            Guid iid = Marshal.GenerateGuidForType(typeof(ImLogService));
            RpcServer.CreateRpc(iid, new mLogService.ServerStub(new mLogServiceAnonymous()))
                //.AddAuthNegotiate()
                .AddAuthentication(RpcAuthentication.RPC_C_AUTHN_NONE)
                .AddAuthNegotiate()
                .AddProtocol("ncacn_ip_tcp", port.ToString())
                //.AddProtocol("ncalrpc", "Greeter")
                .StartListening();
        }
    }
}
