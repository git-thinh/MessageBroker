using Google.ProtocolBuffers.Rpc;
using System.Configuration;
using System.Runtime.InteropServices;

namespace MessageShared
{ 
    public static class LogProvider
    {
        public static mLogService init()
        {
            try
            {
                string HOST_LOG_INPUT = ConfigurationManager.AppSettings["HOST_LOG_INPUT"];
                string PORT_LOG_INPUT = ConfigurationManager.AppSettings["PORT_LOG_INPUT"];
                return new mLogService(RpcClient.ConnectRpc(Marshal.GenerateGuidForType(typeof(ImLogService)), "ncacn_ip_tcp",
                    HOST_LOG_INPUT, PORT_LOG_INPUT).Authenticate(RpcAuthenticationType.None));
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
