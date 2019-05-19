using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MessageBroker
{
    public class UserController : ApiController
    {
        static readonly ICacheService _cache;
        static UserController()
        {
            _cache = _API_CONST.TAI_KHOAN.initCacheService();
        }

        public HttpResponseMessage get_All()
        {
            string json = _cache.getAllJson();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }

        public oCacheResult post_Login([FromBody]oTaiKhoan user)
        {
            oCacheResult result = _cache
                .executeReplyCacheKey("TenTaiKhoan=\"" + user.TenTaiKhoan + "\" And MatKhau=\"" + user.MatKhau + "\"")
                .getResultByCacheKey();
            return result;
        }

        public oCacheResult post_AddNew([FromBody]oTaiKhoan item)
        {
            item.TaiKHoanId = Guid.NewGuid().ToString();

            if (string.IsNullOrWhiteSpace(item.TenTaiKhoan))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("TenTaiKhoan is NULL or empty");

            if (string.IsNullOrWhiteSpace(item.MatKhau))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("MatKhau is NULL or empty");

            if (string.IsNullOrWhiteSpace(item.NhomKH))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("NhomKH is NULL or empty");

            oCacheResult result = _cache.insertItemReplyCacheKey(JsonConvert.SerializeObject(item)).getResultByCacheKey();
            return result;
        }
    }
}
