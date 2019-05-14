using System;
using System.Net.WebSockets;
using System.Threading;

namespace MessageShared
{
    public static class SocketProvider
    {
        public static ClientWebSocket init(string uri)
        {
            try
            {
                ClientWebSocket _client = new ClientWebSocket();
                _client.ConnectAsync(new Uri(uri), CancellationToken.None).Wait();
                return _client;
            }
            catch { }
            return null;
        }

        public static void Send(this ClientWebSocket _client, string text)
        {
            try
            {
                if (_client != null && _client.State == WebSocketState.Open)
                {
                    string msg = Guid.NewGuid().ToString();
                    ArraySegment<byte> bytesToSend = new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(msg));
                    _client.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
                }
            }
            catch { }
        }
    }
}
