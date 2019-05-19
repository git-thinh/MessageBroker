using CacheEngineShared;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MessageBroker
{
    public class TaoHopDongController : ApiController
    {
        static readonly ICacheService _cache; 
        static TaoHopDongController()
        {
            _cache = _API_CONST.TAO_HOP_DONG.initCacheService();
        } 

        public HttpResponseMessage get_All()
        {
            string json = _cache.getAllJson();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }

        public oCacheResult post_AddNew([FromBody]oHongDongKhachHang item)
        {
            item.TaiKhoanId = Guid.NewGuid().ToString();

            if (string.IsNullOrWhiteSpace(item.MaKH))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("MaKH is NULL or empty");

            if (string.IsNullOrWhiteSpace(item.TenKH))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("TenKH is NULL or empty");

            if (string.IsNullOrWhiteSpace(item.TenCuaHangTatToan))
                return new oCacheResult(new oCacheRequest("", "")).ToFailInputNULL("TenCuaHangTatToan is NULL or empty");

            oCacheResult result = _cache.insertItemReplyCacheKey(JsonConvert.SerializeObject(item)).getResultByCacheKey();
            return result;
        }
    }
}
