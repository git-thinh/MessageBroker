using Microsoft.ServiceModel.WebSockets;
using System;

namespace MessageBroker
{
    public class LogOutputService : WebSocketService
    { 
        public override void OnOpen()
        {
            this.Send(Guid.NewGuid().ToString());
        }

        public override void OnMessage(string message) {
            this.Send(Guid.NewGuid().ToString());
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
