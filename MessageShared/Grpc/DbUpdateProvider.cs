using Grpc.Core;
using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace MessageShared
{ 
    public static class DbUpdateProvider
    {
        public static mDbUpdateService.mDbUpdateServiceClient init()
        {
            try
            {
                string HOST_DB_UPDATE = ConfigurationManager.AppSettings["HOST_DB_UPDATE"];
                string PORT_DB_UPDATE = ConfigurationManager.AppSettings["PORT_DB_UPDATE"];

                Channel channel = new Channel(HOST_DB_UPDATE + ":" + PORT_DB_UPDATE, ChannelCredentials.Insecure);

                var client = new mDbUpdateService.mDbUpdateServiceClient(channel);

                //client.writeLogText(new mLogText() { Text = Guid.NewGuid().ToString() }); 
                //channel.ShutdownAsync().Wait();

                return client;
            }
            catch { }
            return null;
        }

        public static mDbUpdateReply Find(this mDbUpdateService.mDbUpdateServiceClient cache, mDbUpdateRequest request)
        {
            try
            {
                if (cache != null)
                {
                   return cache.Send(request);
                }
            }
            catch { }
            return null;
        }
    }
}
