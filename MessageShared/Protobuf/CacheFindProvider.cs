using Google.ProtocolBuffers.Rpc;
using System.Runtime.InteropServices;

namespace MessageShared
{
    public class oCacheFind
    {
        public string _key { set; get; }
        public int pageSize { set; get; }
        public int pageNumber { set; get; }
        public string jsonConditions { set; get; }
        public bool hasCache { set; get; }

        public mCacheRequest ToRequest() {
            return mCacheRequest.CreateBuilder()
                        .SetKey(this._key)
                        .SetPageSize(this.pageSize)
                        .SetPageNumber(this.pageNumber)
                        .SetJsonConditions(this.jsonConditions)
                        .SetHasCache(this.hasCache)
                        .Build();
        }
    }

    public static class CacheFindProvider
    {
        public static mCacheService init(string host, int port)
        {
            try
            {
                return new mCacheService(RpcClient.ConnectRpc(Marshal.GenerateGuidForType(typeof(ImCacheService)), "ncacn_ip_tcp", host, port.ToString()).Authenticate(RpcAuthenticationType.None));
            }
            catch { }
            return null;
        }

        public static mCacheReply Find(this mCacheService cache, oCacheFind cacheFind)
        {
            try
            {
                if (cache != null)
                {
                   return cache.Send(cacheFind.ToRequest());
                }
            }
            catch { }
            return null;
        }
    }
}
