using CacheEngineShared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace MessageBroker
{ 
    public class TestService : ICacheFind
    {
        static object _list;

        public TestService()
        {
            var dataType = new Type[] { typeof(string) };
            var genericBase = typeof(List<>);
            var combinedType = genericBase.MakeGenericType(dataType);
            var listStringInstance = Activator.CreateInstance(combinedType);
            var addMethod = listStringInstance.GetType().GetMethod("Add");
            addMethod.Invoke(listStringInstance, new object[] { "Hello World" });

            var a = (new int[] { 1, 2, 3 }).AsQueryable().Where("it > 2").ToArray();  //.("@0.Contains(\"de\")");
            var a2 = (listStringInstance as List<string>).AsQueryable().Where("it.Contains(\"rl9\")").ToArray();

            //_typeModel = typeModel;
            ////Type listType = typeof(List<>).MakeGenericType(new[] { _typeModel });
            ////_list = (IList)Activator.CreateInstance(listType);
            //var ls = JsonConvert.DeserializeObject<JObject[]>(arrayItemJson).Select(x => x.ToObject(_typeModel)).ToArray();
            //_list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_typeModel), ls);

            //var rs = _list.Where("id = 2");
            //var json = JsonConvert.SerializeObject(ls);
        }

        public string execute(string conditons)
        {
            //var ls = _list.Cast<dynamic>().AsQueryable().Where(conditons, values);
            var ls = _list.Where(conditons);
            var json = JsonConvert.SerializeObject(ls);
            return json;
        }

        public bool push(string arrayItemJson)
        {
            //var ls = JsonConvert.DeserializeObject<JObject[]>(arrayItemJson).Select(x=> x.ToObject(_typeModel)).ToArray();
            //lock(_list) foreach (var o in ls) _list.Add(o);
            return true;
        }
    }

    public class TestBehavior : IServiceBehavior, IInstanceProvider
    {
        private readonly Type _typeModel;
        private readonly string _arrayItemJson;
        public TestBehavior(Type typeModel, string arrayItemJson)
        {
            _typeModel = typeModel;
            _arrayItemJson = arrayItemJson;
        }

        public object GetInstance(InstanceContext instanceContext) => new TestService(_typeModel, _arrayItemJson);

        ////////////////////////////////////////////////////////////////////
        /// 

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
