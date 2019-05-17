using Google.ProtocolBuffers.Rpc;
using System.Runtime.InteropServices;

namespace MessageShared
{ 
    public static class LogProvider
    {
        public static mLogService init(string host, int port)
        {
            try
            {
                return new mLogService(RpcClient.ConnectRpc(Marshal.GenerateGuidForType(typeof(ImLogService)), "ncacn_ip_tcp", host, port.ToString()).Authenticate(RpcAuthenticationType.None));
            }
            catch { }
            return null;
        }

        public static void Write(this mLogService log, string text)
        {
            try
            {
                if (log != null)
                {
                    log.Send(mLogRequest.CreateBuilder().SetText(text).Build());
                }
            }
            catch { }
        }
    }
}
