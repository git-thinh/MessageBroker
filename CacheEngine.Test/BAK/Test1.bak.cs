﻿using CacheEngineShared;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
////////////////////string baseAddress = "http://localhost:" + PORT_WEBAPI_CACHE + "/";
////////////////////ServiceHost host = new ServiceHost(typeof(Service), new Uri(baseAddress));
////////////////////host.AddServiceEndpoint(typeof(ITest), new BasicHttpBinding(), "");
////////////////////host.Description.Behaviors.Add(new MyBehavior());
////////////////////host.Open();

////////////////////Console.WriteLine("Host opened");

////////////////////ChannelFactory<ITest> factory = new ChannelFactory<ITest>(new BasicHttpBinding(), new EndpointAddress(baseAddress));
////////////////////ITest proxy = factory.CreateChannel();
////////////////////Console.WriteLine("WhoAmI: {0}", proxy.WhoAmI());

////////////////////((IClientChannel)proxy).Close();
////////////////////factory.Close();

////////////////////Console.Write("Press ENTER to close the host");
////////////////////Console.ReadLine();
////////////////////host.Close();
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
        public object GetInstance(InstanceContext instanceContext) => new Test1("This is Test1");

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
