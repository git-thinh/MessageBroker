using CacheEngineShared;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Web.Http;

namespace MessageBroker
{
    public class BaseController : ApiController
    {
        private static int PORT_CACHE_STORE = int.Parse(ConfigurationManager.AppSettings["PORT_CACHE_STORE"]);
        //protected static string m_initDataFromDbStore;
        protected static ConcurrentDictionary<string, ICacheService> _storeCache = new ConcurrentDictionary<string, ICacheService>() { };

        protected static void initCacheService(string api_name)
        {
            try
            {
                ChannelFactory<ICacheService> factory = new ChannelFactory<ICacheService>(new BasicHttpBinding(),
                    new EndpointAddress("http://localhost:" + PORT_CACHE_STORE + "/" + api_name + "/"));
                ICacheService cache = factory.CreateChannel();
                if (_storeCache != null)
                {
                    if (!_storeCache.ContainsKey(api_name))
                    {
                        _storeCache.TryAdd(api_name, cache);
                    }
                }
            }
            catch { }
        }

        protected string m_initDataFromDbStore
        {
            get
            {
                string apiName = "";
                string[] a = this.ActionContext.Request.RequestUri.Segments;
                if (a.Length > 2) apiName = a[2];
                if (!string.IsNullOrWhiteSpace(apiName))
                {
                    apiName = apiName.Substring(0, apiName.Length - 1) + "_cacheInitData";
                    return apiName;
                }
                return string.Empty;
            }
        }

        protected ICacheService _cache
        {
            get
            {
                string apiName = "";
                string[] a = this.ActionContext.Request.RequestUri.Segments;
                if (a.Length > 2) apiName = a[2];
                if (!string.IsNullOrWhiteSpace(apiName))
                {
                    apiName = apiName.Substring(0, apiName.Length - 1);
                    ICacheService cache = null;
                    if (_storeCache.TryGetValue(apiName, out cache))
                        return cache;
                }
                return null;
            }
        }

        protected void reloadCacheByServiceNameArray(string[] arrServiceName)
        {
            foreach (var sv in arrServiceName)
            {
                _cache.initDataFromDbStore(sv + "_cacheInitData");
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

        protected oCacheResult sqlExecuteNonQuery<DTO>(string storeName, DTO objPara)
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
                }

                using (IDbConnection cnn = new SqlConnection(_DB_CONST.get_connectString_Mobility()))
                {
                    cnn.Open();
                    cnn.Execute(storeName, param, commandType: CommandType.StoredProcedure);

                    return new oCacheResult() { Ok = true, Result = new dynamic[] { } };
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
        public oCacheResult post_Search([FromBody]oCacheRequest value)
        {
            if (value == null) return new oCacheResult().ToFailConvertJson("Please check format string json of input.");

            value.RequestId = Guid.NewGuid().ToString();
            oCacheResult result = _cache.executeRequestJsonReplyCacheKey(value.ToJson()).getResultByCacheKey();
            result.Request = value;
            return result;
        }

        public HttpResponseMessage get_json([FromUri]string model = null)
        {
            string json = "{}";

            if (model == null)
            {
                string[] a = this.ActionContext.Request.RequestUri.Segments;
                if (a.Length > 2)
                {
                    model = a[2];
                    model = model.Substring(0, model.Length - 1);
                    model = "o" + string.Join("", model.Split('_').Select(x => x[0].ToString().ToUpper() + x.Substring(1)).ToArray());
                }
            }

            if (!string.IsNullOrWhiteSpace(model))
            {
                Type typeModel = Type.GetType("MessageBroker." + model + ", MessageBroker");
                if (typeModel != null)
                {
                    object item = Activator.CreateInstance(typeModel);
                    json = JsonConvert.SerializeObject(item, Formatting.Indented);
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };
        }
    }
}
