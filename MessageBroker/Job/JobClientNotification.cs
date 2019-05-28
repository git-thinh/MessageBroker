using CacheEngineShared;
using MessageShared;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBroker
{
    public enum CLIENT_COMMAND
    {
        NONE = 0,
        EXPORT_PDF = 1,
        LOG_CLIENT = 2
    }

    public class JobClientNotification : JobBase
    {
        public override void freeResource() { _isStop = true; httpServerStop(); }

        ////////////////////////////////////////////////////////////////////
        /// 

        #region [ HTTP_LISTENER ]

        static List<WebSocket> _clients = new List<WebSocket>() { };
        static readonly Object _lock = new Object();
        static HttpListener _httpListener;
        static void httpStart(int port)
        {
            _httpListener = new HttpListener();
            //_httpListener.Prefixes.Add("http://*:12345/log/");
            _httpListener.Prefixes.Add("http://*:" + port + "/");
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

        static void fn_processClientCommand(string msg, WebSocket socketClient)
        {
            if (string.IsNullOrWhiteSpace(msg)) return;
            msg = msg.Trim();
            Console.WriteLine("-> APP_CLIENT: " + msg);

            string url = "", keyCache = "";
            string[] a;
            CLIENT_COMMAND cmd = CLIENT_COMMAND.NONE;
            ObjectCache cacheRuntime = MemoryCache.Default;
            int user_ID = 0;

            switch (msg[0])
            {
                case '#':
                    // #EXPORT.PDF:User_ID.hop_dong.Pawn_ID = #EXPORT.PDF:9.hop_dong.1167678
                    a = msg.Substring(1).Split(':');
                    switch (a[0])
                    {
                        case "EXPORT.PDF":
                            if (a.Length > 1)
                            {
                                a = a[1].Split('.');
                                if (a.Length > 2)
                                {
                                    if (int.TryParse(a[0], out user_ID) && user_ID > 0)
                                    {
                                        keyCache = "APP_CLIENT_SOCKET." + a[0];
                                        cacheRuntime.Set(keyCache, socketClient, new CacheItemPolicy());

                                        cmd = CLIENT_COMMAND.EXPORT_PDF;
                                        //_dataflow.enqueue(new JobPdfExport("http://localhost:9096/api/pawn_info/get_in_hop_dong?Pawn_ID=1167678&filetemp=in-hop-dong.html")).Wait();
                                        url = "http://localhost:9096/api/pawn_info/get_in_" + a[1] + "?User_ID=" + a[0] + "&Pawn_ID=" + a[2] + "&filetemp=in-" + a[1].Replace("_", "-") + ".html";
                                    }
                                }
                            }
                            break;
                    }
                    break;
                case '!':
                    break;
            } 
            switch (cmd)
            {
                case CLIENT_COMMAND.EXPORT_PDF:
                    Dataflow.enqueue(new JobPdfExport(url)).Wait();
                    break;
            }
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
                        //fn_processClientCommand(message, webSocket);
                        //Dataflow.enqueue(new )
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
                //Console.WriteLine("Exception: {0}", e.Message);
                lock (_lock) _clients.Remove(webSocket);
            }
            finally
            {
                if (webSocket != null)
                    webSocket.Dispose();
            }
        }

        static void httpServerStop()
        {
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

        private static bool _isStop = false;
        private static bool _inited = false;
        private readonly string _message;

        public JobClientNotification(string message = "")
        {
            this._message = message;
        }

        ////////////////////////////////////////////////////////////////////
        /// 

        static ConcurrentQueue<string> _storeText = new ConcurrentQueue<string>() { };

        void processMessage()
        {
            Task.Run(() =>
            {
                while (_isStop == false)
                {
                    if (_storeText.Count > 0)
                    {
                        string message;
                        if (_storeText.TryDequeue(out message) && !string.IsNullOrEmpty(message))
                        {
                            httpBroadCast(message);
                        }
                    }
                    Thread.Sleep(100);
                }
            });
        }

        public override void execute()
        {
            if (!_inited)
            {
                try
                {
                    int port = (int)getOptions("port");
                    httpStart(port);
                    processMessage();
                    _inited = true;
                }
                catch { }
                return;
            }

            if (!string.IsNullOrWhiteSpace(_message))
                _storeText.Enqueue(_message);
        }
    }


}
