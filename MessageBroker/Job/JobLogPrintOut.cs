using MessageShared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace MessageBroker
{
    public class JobLogPrintOut : IJob
    {
        static bool _inited = false;


        private readonly string _textSynchronize;
        public JobLogPrintOut(string textSynchronize)
        {
            this._textSynchronize = textSynchronize;
        }
                 
        public void execute()
        {
            using (var client = new HttpClient())
            {
                string url = "api/cusid";
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["webapi_uri_root"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var jsonRequest = JsonConvert.SerializeObject(_request);
                //var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
                var content = new StringContent(_textSynchronize, Encoding.UTF8, "text/plain");

                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    //string responseString = response.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }

    public interface IWebsocketServer {
        void broadCast(string text);
    }

    public class WebsocketServer: IWebsocketServer
    {
        public async void Start(string httpListenerPrefix)
        {
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add(httpListenerPrefix);
            httpListener.Start();
            Console.WriteLine("Listening...");

            while (true)
            {
                HttpListenerContext httpListenerContext = await httpListener.GetContextAsync();
                if (httpListenerContext.Request.IsWebSocketRequest)
                {
                    ProcessRequest(httpListenerContext);
                }
                else
                {
                    //httpListenerContext.Response.StatusCode = 400;
                    //httpListenerContext.Response.Close();

                    byte[] buffer = Encoding.UTF8.GetBytes("<HTML><BODY><h1> " + DateTime.Now.ToString() + " </h1></BODY></HTML>");
                    httpListenerContext.Response.ContentLength64 = buffer.Length;
                    Stream output = httpListenerContext.Response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
            }
        }

        static List<WebSocket> _clients = new List<WebSocket>() { };
        static readonly Object _lock = new Object();

        private async void ProcessRequest(HttpListenerContext httpListenerContext)
        {
            WebSocketContext webSocketContext = null;
            try
            {
                webSocketContext = await httpListenerContext.AcceptWebSocketAsync(subProtocol: null);
                string ipAddress = httpListenerContext.Request.RemoteEndPoint.Address.ToString();
                Console.WriteLine("Connected: IPAddress {0}", ipAddress);
            }
            catch (Exception e)
            {
                httpListenerContext.Response.StatusCode = 500;
                httpListenerContext.Response.Close();
                Console.WriteLine("Exception: {0}", e);
                return;
            }

            WebSocket webSocket = webSocketContext.WebSocket;
            lock (_lock) _clients.Add(webSocket);

            try
            {
                byte[] receiveBuffer = new byte[1024];
                while (webSocket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        lock (_lock) _clients.Remove(webSocket);
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    }
                    else
                    {
                        string message = Encoding.UTF8.GetString(receiveBuffer).TrimEnd('\0');
                        //Console.WriteLine("-> SERVER: " + message);
                        broadCast(message);

                        ////byte[] bsend = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
                        ////var adata = new ArraySegment<byte>(bsend, 0, bsend.Length);
                        ////lock (_lock)
                        ////    foreach (var socket in _clients)
                        ////        socket.SendAsync(adata, WebSocketMessageType.Text, receiveResult.EndOfMessage, CancellationToken.None).Wait();
                        ////webSocket.SendAsync(adata, WebSocketMessageType.Text, receiveResult.EndOfMessage, CancellationToken.None).Wait();

                        //await webSocket.SendAsync(adata, WebSocketMessageType.Binary, receiveResult.EndOfMessage, CancellationToken.None);
                    }
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("Exception: {0}", e);
                Console.WriteLine("Exception: {0}", e.Message);
                lock (_lock) _clients.Remove(webSocket);
            }
            finally
            {
                if (webSocket != null)
                    webSocket.Dispose();
            }
        }

        public void broadCast(string text)
        {
            byte[] bsend = Encoding.UTF8.GetBytes(text);
            var adata = new ArraySegment<byte>(bsend, 0, bsend.Length);
            lock (_lock)
            {
                foreach (var socket in _clients)
                {
                    try
                    {
                        if (socket.State == WebSocketState.Open)
                            socket.SendAsync(adata, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
                    }
                    catch { }
                }
            }
        }
    }


}
