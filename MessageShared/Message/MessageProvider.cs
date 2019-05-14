using Google.ProtocolBuffers.Rpc;
using System.Runtime.InteropServices;

namespace MessageShared
{
    public class oMessageRequest
    {
        public string _key { set; get; }
        public int pageSize { set; get; }
        public int pageNumber { set; get; }
        public string jsonConditions { set; get; }
        public bool hasCache { set; get; }

        public oMessageRequest() { }
        public oMessageRequest(mMessageRequest ret) {
            this._key = ret.Key;
            this.pageSize = ret.PageSize;
            this.pageNumber = ret.PageNumber;
            this.jsonConditions = ret.JsonConditions;
            this.hasCache = ret.HasCache;
        }
    }

    public class oMessageReply
    {
        public bool ok { set; get; }
        public oMessageRequest request { set; get; }
        public string output { set; get; }
        public int countResult { set; get; }
        public int totalItems { set; get; }

        public oMessageReply(mMessageReply reply) {
            this.ok = reply.Ok;
            this.request = new oMessageRequest(reply.Request);
            this.output = reply.Output;
            this.countResult = reply.CountResult;
            this.totalItems = reply.TotalItems;
        }
    }

    public static class MessageProvider
    {
        public static mMessageService init(string host, int port)
        {
            try
            {
                return new mMessageService(RpcClient.ConnectRpc(Marshal.GenerateGuidForType(typeof(ImMessageService)), "ncacn_ip_tcp", host, port.ToString()).Authenticate(RpcAuthenticationType.None));
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
