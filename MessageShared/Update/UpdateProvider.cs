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
        public bool ok { set; get; }
        public oMessageRequest request { set; get; }
        public string output { set; get; }
        public int countResult { set; get; }
        public int totalItems { set; get; }

        public oUpdateReply(mUpdateReply reply) {
            this.ok = reply.Ok;
            this.request = new oMessageRequest(reply.Request);
            this.output = reply.Output;
            this.countResult = reply.CountResult;
            this.totalItems = reply.TotalItems;
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

        public static oMessageReply Write(this mMessageService service, oMessageRequest ret)
        {
            try
            {
                if (service != null)
                {
                  mMessageReply reply = service.Send(mMessageRequest.CreateBuilder()
                        .SetKey(ret._key)
                        .SetPageNumber(ret.pageNumber)
                        .SetPageSize(ret.pageSize)
                        .SetJsonConditions(ret.jsonConditions)
                        .SetHasCache(ret.hasCache)
                        .Build());
                    return new oMessageReply(reply);
                }
            }
            catch { }
            return null;
        }
    }
}
