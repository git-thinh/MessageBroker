using Microsoft.ServiceModel.WebSockets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    /*
     * 
    class Program
    {
        static void Main() {
            LogSocketsServer.Start("ws://localhost:50053");
            Console.WriteLine(">>>>");
            Console.ReadKey();
            chatWebSocketsServer.Stop();
        }
    }

    */
    //public interface ILogOutput {
    //    void broadCast(string text);
    //}

    //public class WsUploadServer : ILogOutput
    //{
    //    private readonly Binding localBind;
    //    private readonly WebSocketHost<WsUploadService> serverLog;

    //    public WsUploadServer(string uri)
    //    {
    //        //localBind = WebSocketHost.CreateWebSocketBinding(false, 1024, 1024);
    //        localBind = WebSocketHost.CreateWebSocketBinding(false);
    //        serverLog = new WebSocketHost<WsUploadService>(new Uri(uri));            
    //        serverLog.AddWebSocketEndpoint(localBind);
    //        serverLog.Credentials.UseIdentityConfiguration = true;

    //        serverLog.Faulted += (se,ev)=> {

    //        };
    //        serverLog.Opened += (se, ev) => {
    //            Type type = se.GetType();
    //            ;
    //        };

    //        serverLog.Open();
    //    }

    //    public void broadCast(string text)
    //    {
            
    //    }

    //    public void Stop()
    //    {
    //        serverLog.Close();
    //    }
    //}

    internal static class WsUploadServiceStatic
    {
        private static Binding localBind;
        private static WebSocketHost<WsUploadService> serverLog;

        internal static void Start(string uri)
        {
            //localBind = WebSocketHost.CreateWebSocketBinding(false, 1024, 1024);
            localBind = WebSocketHost.CreateWebSocketBinding(false);
            serverLog = new WebSocketHost<WsUploadService>(new Uri(uri));
            serverLog.AddWebSocketEndpoint(localBind);
            serverLog.Credentials.UseIdentityConfiguration = true;

            serverLog.Faulted += serverChat_Faulted;
            serverLog.Opened += serverChat_Opened;

            serverLog.Open();
        }

        internal static void Stop()
        {
            serverLog.Close();
        }

        static void serverChat_Faulted(object sender, EventArgs e)
        {
        }

        static void serverChat_Opened(object sender, EventArgs e)
        {
        }

    }
}
