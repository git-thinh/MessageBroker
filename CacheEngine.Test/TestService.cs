using CacheEngineShared;
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

namespace CacheEngine.Test
{
    public class oTest
    {
        public int id { set; get; }
        public string name { set; get; }
    }

    public class TestService : ICacheService
    { 
        public bool update(UPDATE_TYPE type, string valKey, string jsonObject) { return _store.Update(type, _cacheFields, valKey, jsonObject); }

        private readonly IDataflowSubscribers _dataflow;
        private readonly oCacheField[] _cacheFields;
        public TestService(IDataflowSubscribers dataflow, oCacheField[] cacheFields) { _dataflow = dataflow; _cacheFields = cacheFields; }

        static CacheSynchronized<oTest> _store = new CacheSynchronized<oTest>();
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
               var list = JsonConvert.DeserializeObject<List<oTest>>(arrayItemJson);
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

    public class TestBehavior : IServiceBehavior, IInstanceProvider
    {
        private readonly IDataflowSubscribers _dataflow;
        private readonly oCacheField[] _cacheFields;
        public TestBehavior(IDataflowSubscribers dataflow, oCacheField[] cacheFields) { _dataflow = dataflow; _cacheFields = cacheFields; }

        public object GetInstance(InstanceContext instanceContext) { return new TestService(_dataflow, _cacheFields); }
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }
        public void ReleaseInstance(InstanceContext instanceContext, object instance) { }
        public object GetInstance(InstanceContext instanceContext, Message message) { return this.GetInstance(instanceContext); }
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
                foreach (EndpointDispatcher ed in cd.Endpoints)
                    ed.DispatchRuntime.InstanceProvider = this;
        }
    }
}
