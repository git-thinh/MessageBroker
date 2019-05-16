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
        string name;

        public Test1()
        {
        }

        public Test1(string name)
        {
            this.name = name;
        }

        public string execute()
        {
            return "Asset: " + this.name + " at " + DateTime.Now.ToString();
        }
    }

    public class Test1Behavior : IServiceBehavior, IInstanceProvider
    {
        public object GetInstance(InstanceContext instanceContext) { return new Test1("This is Test1"); }
         
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
