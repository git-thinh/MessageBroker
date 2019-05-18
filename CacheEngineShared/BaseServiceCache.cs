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
    public class BaseServiceCache<T> : ICacheService
    {
        public BaseServiceCache(IDataflowSubscribers dataflow, oCacheField[] cacheFields) { _dataflow = dataflow; _cacheFields = cacheFields; }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool update(UPDATE_TYPE type, string valKey, string jsonObject) { return _store.Update(type, _cacheFields, valKey, jsonObject); }

        private readonly IDataflowSubscribers _dataflow;
        private readonly oCacheField[] _cacheFields;

        static CacheSynchronized<T> _store = new CacheSynchronized<T>();
        public string execute(string conditons)
        {
            try
            {
                string key = Guid.NewGuid().ToString();

                var a = _store.SearchDynamic(conditons).ToArray();
                ObjectCache cache = MemoryCache.Default;
                cache.Set(key, a, new CacheItemPolicy());

                return key;
            }
            catch (Exception ex)
            {
                _dataflow.writeLog(ex.Message);
            }
            return null;
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
    }

    public class BaseServiceCacheBehavior : IServiceBehavior, IInstanceProvider
    {
        private readonly object _instance;
        public BaseServiceCacheBehavior(object instance) {
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
