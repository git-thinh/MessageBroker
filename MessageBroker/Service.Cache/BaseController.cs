using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;

namespace MessageBroker
{
    public class BaseController : ApiController
    {
        protected static string m_initDataFromDbStore;
        public static ICacheService _cache;

        [AttrApiInfo("Chức năng đồng bộ tất cả dữ liệu từ Database")]
        [HttpPost]
        public oCacheResult post_allDataFromDbStore()
        {
            _cache.initDataFromDbStore(m_initDataFromDbStore);
            return get_All();
        }

        [AttrApiInfo("Chức năng PING kiểm tra online")]
        [HttpGet]
        public HttpResponseMessage get_ping()
        {
            return new HttpResponseMessage() { Content = new StringContent("OK", Encoding.ASCII, "text/plain") };
        }

        [AttrApiInfo("Chức năng lấy về tất cả dữ liệu")]
        public oCacheResult get_All()
        {
            oCacheResult result = _cache.getAllJsonReplyCacheKey().getResultByCacheKey();
            return result;
        }

        [AttrApiInfo("Chức năng tìm kiếm", Description = "{\"Conditions\":\" Linq.Dynamic clause at here ... \"}")]
        public oCacheResult post_Search([FromBody]oCacheRequest request)
        {
            request.RequestId = Guid.NewGuid().ToString();
            oCacheResult result = _cache.executeReplyCacheKey(request.Conditions).getResultByCacheKey();
            result.Request = request;
            return result;
        }
    }
}
