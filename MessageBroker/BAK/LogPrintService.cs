using Microsoft.ServiceModel.WebSockets;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace WebApiShared
{
    public class LogPrintServiceFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            WebSocketHost host = new WebSocketHost(serviceType, baseAddresses);
            host.AddWebSocketEndpoint();
            return host;
        }
    }

    public class LogPrintService : WebSocketService
    {
        public override void OnOpen() {
            Debug.WriteLine("CONNECTED ...");
        }
        public override void OnMessage(string message) {
            Debug.WriteLine("->: " + message);
        }

        public override void OnMessage(Byte[] buffer) { }
        protected override void OnClose()
        {
            base.OnClose();
        }
        protected override void OnError()
        {
            base.OnError();
        }
    }
}