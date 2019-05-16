using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    [ServiceContract]
    public interface ITest
    {
        [OperationContract]
        string WhoAmI();
    }
    public class Service : ITest
    {
        string name;

        public Service(string name)
        {
            this.name = name;
        }

        public string WhoAmI()
        {
            return this.name;
        }
    }
    class MyBehavior : IServiceBehavior, IInstanceProvider
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher ed in cd.Endpoints)
                {
                    ed.DispatchRuntime.InstanceProvider = this;
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.GetInstance(instanceContext);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return new Service("John Doe");
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }
}