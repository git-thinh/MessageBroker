using Grpc.Core;
using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace MessageShared
{ 
    public static class LogProvider
    {
        public static svcLogService.svcLogServiceClient init()
        {
            try
            {
                string HOST_LOG_INPUT = ConfigurationManager.AppSettings["HOST_LOG_INPUT"];
                string PORT_LOG_INPUT = ConfigurationManager.AppSettings["PORT_LOG_INPUT"];

                Channel channel = new Channel(HOST_LOG_INPUT + ":" + PORT_LOG_INPUT, ChannelCredentials.Insecure);

                var client = new svcLogService.svcLogServiceClient(channel);

                //client.writeLogText(new mLogText() { Text = Guid.NewGuid().ToString() }); 
                //channel.ShutdownAsync().Wait();

                return client;
            }
            catch { }
            return null;
        }

        public static void Write(this svcLogService.svcLogServiceClient log, string text)
        {
            try
            {
                if (log != null)
                {
                    log.writeLogText(new mLogText() { Text = text });
                }
            }
            catch { }
        }
    }
}
