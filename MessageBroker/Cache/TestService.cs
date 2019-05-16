using CacheEngineShared;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace MessageBroker
{
    public class TestService : ICacheFind
    {
        string name;

        public TestService()
        {
        }

        public TestService(string name)
        {
            this.name = name;
        }

        public string execute()
        {
            return "Test: " + this.name;
        }
    }

    public class TestBehavior : IServiceBehavior, IInstanceProvider
    {
        public object GetInstance(InstanceContext instanceContext) => new TestService("John Doe");

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
