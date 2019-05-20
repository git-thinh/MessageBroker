using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using Microsoft.ServiceModel.WebSockets;

namespace MessageBroker
{
    public class WsServiceFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            WebSocketHost host = new WebSocketHost(serviceType, baseAddresses);
            host.AddWebSocketEndpoint();
            return host;
        }
    }

    public class UploadServiceFactory2 : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var host = new WebSocketHost(serviceType, new ServiceThrottlingBehavior { MaxConcurrentSessions = int.MaxValue, MaxConcurrentCalls = 20 }, baseAddresses);

            //var binding = WebSocketHost.CreateWebSocketBinding(https: false, sendBufferSize: 2048, receiveBufferSize: 2048);
            var binding = WebSocketHost.CreateWebSocketBinding(https: false, sendBufferSize: 65536, receiveBufferSize: 65536); //max limit are 65536 64kB
            binding.SendTimeout = TimeSpan.FromMilliseconds(500);
            binding.OpenTimeout = TimeSpan.FromDays(1);
            host.AddWebSocketEndpoint(binding);

            return host;
        }
    }
}