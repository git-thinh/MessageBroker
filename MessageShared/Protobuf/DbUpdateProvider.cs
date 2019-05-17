using Google.ProtocolBuffers.Rpc;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MessageShared
{
    public class oDbUpdateRequest
    {
        public int Type { set; get; }
        public string NameStore { set; get; }
        public Dictionary<string, string> Parameters { set; get; }

        public mDbUpdateRequest ToRequest() {
            mParameterCollection.Builder para = mParameterCollection.CreateBuilder();
            if (Parameters != null && Parameters.Count > 0)
                foreach (var kv in Parameters)
                    para.AddItems(mParameter.CreateBuilder().SetKey(kv.Key).SetValue(kv.Value).Build());

            return mDbUpdateRequest.CreateBuilder()
                        .SetType(this.Type)
                        .SetNameStore(this.NameStore)
                        .SetParameters(para.Build())
                        .Build();
        }
    }

    public static class DbUpdateProvider
    {
        public static mDbUpdateService init(string host, int port)
        {
            try
            {
                return new mDbUpdateService(RpcClient.ConnectRpc(Marshal.GenerateGuidForType(typeof(ImDbUpdateService)), "ncacn_ip_tcp", host, port.ToString()).Authenticate(RpcAuthenticationType.None));
            }
            catch { }
            return null;
        }

        public static void Send(this mDbUpdateService service, oDbUpdateRequest update)
        {
            try
            {
                if (service != null)
                {
                    service.Send(update.ToRequest());
                }
            }
            catch { }
        }
    }
}
