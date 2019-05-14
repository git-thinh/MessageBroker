using Google.ProtocolBuffers.Rpc;
using System.Runtime.InteropServices;

namespace MessageShared
{
    public class oUpdateRequest
    {
        public int type { set; get; }
        public string nameObject { set; get; }
        public string jsonItem { set; get; }

        public oUpdateRequest() { }
        public oUpdateRequest(mUpdateRequest ret) {
            this.type = ret.Type;
            this.nameObject = ret.NameObject;
            this.jsonItem = ret.JsonItem; 
        }
    }

    public class oUpdateReply
    {
        public string id { set; get; }
        public bool ok { set; get; }
        public oUpdateRequest request { set; get; }
        public string message { set; get; }

        public oUpdateReply(mUpdateReply reply) {
            this.id = reply.Id;
            this.request = new oUpdateRequest(reply.Request);
            this.ok = reply.Ok;
            this.message = reply.Message; 
        }
    }

    public static class UpdateProvider
    {
        public static mUpdateService init(string host, int port)
        {
            try
            {
                return new mUpdateService(RpcClient.ConnectRpc(Marshal.GenerateGuidForType(typeof(ImUpdateService)), "ncacn_ip_tcp", host, port.ToString()).Authenticate(RpcAuthenticationType.None));
            }
            catch { }
            return null;
        }

        public static oUpdateReply Write(this mUpdateService service, oUpdateRequest ret)
        {
            try
            {
                if (service != null)
                {
                    mUpdateReply reply = service.Send(mUpdateRequest.CreateBuilder()
                        .SetType(ret.type)
                        .SetNameObject(ret.nameObject)
                        .SetJsonItem(ret.jsonItem)
                        .Build());
                    return new oUpdateReply(reply);
                }
            }
            catch { }
            return null;
        }
    }
}
