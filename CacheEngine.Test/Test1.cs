using CacheEngineShared;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace CacheEngine.Test
{
    public class Test1 : ICacheFind
    {
        private readonly IDataflowSubscribers _dataflow; 

        public Test1()
        {
        }

        public Test1(IDataflowSubscribers dataflow)
        {
            _dataflow = dataflow;
        }

        public string execute()
        {
            return "Asset:  at " + DateTime.Now.ToString();
        }
    }

    public class Test1Behavior : IServiceBehavior, IInstanceProvider
    {
        private readonly IDataflowSubscribers _dataflow;
        public Test1Behavior(IDataflowSubscribers dataflow) {
            _dataflow = dataflow;
        }

        public object GetInstance(InstanceContext instanceContext) { return new Test1(_dataflow); }
         
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
