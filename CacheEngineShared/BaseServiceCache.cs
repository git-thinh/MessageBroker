using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Caching;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace CacheEngineShared
{
    public static class StringCacheExt
    {
        public static oCacheResult getResultByCacheKey(this string cacheKey)
        {
            ObjectCache cache = MemoryCache.Default;
            if (cache.Contains(cacheKey))
                return (oCacheResult)cache.Get(cacheKey);
            return null;
        }

        public static bool clearCacheIfExist(this oCacheResult rs)
        {
            if (rs == null || rs.Request == null || string.IsNullOrWhiteSpace(rs.Request.RequestId)) return true;

            ObjectCache cache = MemoryCache.Default;
            if (cache.Contains(rs.Request.RequestId))
            {
                cache.Remove(rs.Request.RequestId);
            }

            return false;
        }

        public static string ToJson(this oCacheResult rs)
        {
            if (rs == null) return "{}";
            return JsonConvert.SerializeObject(rs);
        }
    }

    public class BaseServiceCache<T> : ICacheService
    {
        public BaseServiceCache(IDataflowSubscribers dataflow, oCacheModel cacheModel) { _dataflow = dataflow; _cacheModel = cacheModel; }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string updateReplyCacheKey(UPDATE_TYPE type, string valKey, string jsonObject) { return _store.updateReplyCacheKey(type, _cacheModel, valKey, jsonObject); }

        private readonly IDataflowSubscribers _dataflow;
        private readonly oCacheModel _cacheModel;

        static CacheSynchronized<T> _store = new CacheSynchronized<T>();
        public string executeReplyCacheKey(string conditons)
        {
            string key = _store.searchDynamicReplyCacheKey(new oCacheRequest(_cacheModel.ServiceName, conditons));
            return key;
        }

        public string insertItemReplyCacheKey(string jsonItem)
        {
            string key = _store.insertItemReplyCacheKey(jsonItem);
            return key;
        }

        public string insertItemsByCacheKey(string cacheKey) {
            return _store.insertItemsByCacheKey(cacheKey);
        }

        public bool push(string arrayItemJson)
        {
            try
            {
                var list = JsonConvert.DeserializeObject<List<T>>(arrayItemJson);
                _store.Set(list);
                return true;
            }
            catch (Exception ex)
            {
                _dataflow.writeLog(ex.Message);
            }
            return false;
        }

        public bool insertItems(IList items)
        {
            _store.insertItems(items);
            return true;
        }

        public string getAllJsonReplyCacheKey()
        {
            return _store.getAllJsonReplyCacheKey();
        }
    }

    public class BaseServiceCacheBehavior : IServiceBehavior, IInstanceProvider
    {
        private readonly object _instance;
        public BaseServiceCacheBehavior(object instance)
        {
            _instance = instance;
        }
        public object GetInstance(InstanceContext instanceContext) { return _instance; }
        //public object GetInstance(InstanceContext instanceContext) { return new oUserService(_dataflow, _cacheFields); }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }
        public void ReleaseInstance(InstanceContext instanceContext, object instance) { }
        public object GetInstance(InstanceContext instanceContext, Message message) => this.GetInstance(instanceContext);
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
                foreach (EndpointDispatcher ed in cd.Endpoints)
                    ed.DispatchRuntime.InstanceProvider = this;
        }
    }
}
