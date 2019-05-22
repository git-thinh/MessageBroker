using CacheEngineShared;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

        protected void reloadCacheByServiceNameArray(string[] arrServiceName)
        {
            foreach (var sv in arrServiceName)
            {
                sv.initCacheService().initDataFromDbStore(sv + "_cacheInitData");
            }
        }

        protected oCacheResult sqlExecute<R, DTO>(string storeName, DTO objPara)
        {
            try
            {
                DynamicParameters param = null;

                if (objPara != null)
                {
                    IDictionary<string, object> paramenters = objPara.convertToDictionary<DTO>();
                    param = new DynamicParameters();
                    foreach (var kv in paramenters)
                        param.Add("@" + kv.Key, kv.Value);
                    //param.Add("@Count", dbType: DbType.Int32, direction: ParameterDirection.Output);
                }

                using (IDbConnection cnn = new SqlConnection(_DB_CONST.get_connectString_Mobility()))
                {
                    cnn.Open();
                    object[] result;
                    if (objPara == null)
                        result = cnn.Query<R>(storeName, null, commandType: CommandType.StoredProcedure).Cast<object>().ToArray();
                    else
                        result = cnn.Query<R>(storeName, param, commandType: CommandType.StoredProcedure).Cast<object>().ToArray();

                    return new oCacheResult() { Ok = true, Result = result };
                }
            }
            catch (Exception ex)
            {
                return new oCacheResult().ToFailException(ex.Message);
            }
        }

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
