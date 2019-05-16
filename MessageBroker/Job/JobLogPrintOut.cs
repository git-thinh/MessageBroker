using CacheEngineShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBroker
{
    public class JobLogPrintOut : JobBase
    {
        public override void freeResource() { httpServerStop(); }
        
        ////////////////////////////////////////////////////////////////////
        /// 
        
        #region [ HTTP_LISTENER ]

        static List<WebSocket> _clients = new List<WebSocket>() { };
        static readonly Object _lock = new Object();
        static HttpListener _httpListener;
        static void httpStart()
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://*:12345/log/");
            Task.Factory.StartNew(async (object obj) =>
            {
                HttpListener httpListener = (HttpListener)obj;
                httpListener.Start();

                while (true)
                {
                    HttpListenerContext httpListenerContext = await httpListener.GetContextAsync();
                    if (httpListenerContext.Request.IsWebSocketRequest)
                    {
                        httpProcessRequest(httpListenerContext);
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
                    Thread.Sleep(10);
                }
                ;
            }, _httpListener);
        }

        static async void httpProcessRequest(HttpListenerContext httpListenerContext)
        {
            WebSocketContext webSocketContext = null;
            try
            {
                webSocketContext = await httpListenerContext.AcceptWebSocketAsync(subProtocol: null);
                string ipAddress = httpListenerContext.Request.RemoteEndPoint.Address.ToString();
                //Console.WriteLine("Connected: IPAddress {0}", ipAddress);
            }
            catch //(Exception e)
            {
                httpListenerContext.Response.StatusCode = 500;
                httpListenerContext.Response.Close();
                //Console.WriteLine("Exception: {0}", e);
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
                        httpBroadCast(message);

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

        static void httpServerStop() {
            _httpListener.Stop();
        }
        
        static void httpBroadCast(string text)
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

        #endregion

        ////////////////////////////////////////////////////////////////////
        /// 

        private static bool _inited = false;
        private readonly string _message;

        public JobLogPrintOut(string message = "")
        {
            this._message = message;
        }

        ////////////////////////////////////////////////////////////////////
        /// 

        public override void execute()
        {
            if (!_inited)
            {
                _inited = true;
                httpStart();
                return;
            }

            if (string.IsNullOrWhiteSpace(_message)) return;
            httpBroadCast(_message);
        }
    }


}
