using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Newtonsoft.Json;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketEngine;
using SuperSocket.SocketEngine.Configuration;
using SuperWebSocket;

namespace UploadImages
{
    public class oFile
    {
        public string folder { set; get; }
        public string name { set; get; }
        public string type { set; get; }
        public int size { set; get; }
    }

    public class Global : System.Web.HttpApplication
    {
        #region [ UPLOAD ]

        public static string rootPath = ConfigurationManager.AppSettings["rootPathUpload"];

        private static ConcurrentDictionary<string, MemoryStream> streams = new ConcurrentDictionary<string, MemoryStream>();
        private static ConcurrentDictionary<string, oFile> files = new ConcurrentDictionary<string, oFile>();

        //public string SessionID
        //{
        //    get { return this.WebSocketContext.SecWebSocketKey; }
        //}

        private bool pathIsValid
        {
            get { return string.IsNullOrWhiteSpace(rootPath) == false && Directory.Exists(rootPath); }
        }

        private void freeRelease(string SessionID)
        {
            try
            {
                oFile fi;
                files.TryRemove(SessionID, out fi);
                MemoryStream stream;
                if (streams.TryRemove(SessionID, out stream))
                    stream.Close();
            }
            catch { }
        }

        private bool saveFile(string SessionID)
        {
            try
            {
                if (files.ContainsKey(SessionID)
                    && streams.ContainsKey(SessionID))
                {
                    if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);

                    oFile fi = files[SessionID];
                    MemoryStream stream = streams[SessionID];
                    string pathFile = Path.Combine(rootPath, fi.name);

                    if (File.Exists(pathFile))
                        pathFile = Path.Combine(rootPath, DateTime.Now.ToString("yyyyMMdd-HHmmssfff-") + fi.name);

                    using (var ms = new FileStream(pathFile, FileMode.OpenOrCreate))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(ms);
                    }
                    stream.Close();
                    streams.TryRemove(SessionID, out stream);
                    return true;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return false;
        }

        private void fileDelete()
        {
        }
        #endregion

        private IBootstrap m_Bootstrap;
        private WebSocketServer m_WebSocketServer;

        void Application_Start(object sender, EventArgs e)
        {
            StartSuperWebSocketByConfig();
        }

        void StartSuperWebSocketByConfig()
        {
            m_Bootstrap = BootstrapFactory.CreateBootstrap();

            if (!m_Bootstrap.Initialize())
                return;

            var socketServer = m_Bootstrap.AppServers.FirstOrDefault(s => s.Name.Equals("SuperWebSocket")) as WebSocketServer;

            socketServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(socketServer_NewMessageReceived);
            socketServer.NewDataReceived += new SessionHandler<WebSocketSession, byte[]>(socketServer_NewBufferReceived);
            socketServer.NewSessionConnected += socketServer_NewSessionConnected;
            socketServer.SessionClosed += socketServer_SessionClosed;
            //socketServer.Session += socketServer_SessionClosed;

            m_WebSocketServer = socketServer;

            m_Bootstrap.Start();
        }

        private void socketServer_NewBufferReceived(WebSocketSession session, byte[] buffer)
        { 
            if (streams.ContainsKey(session.SessionID))
            {
                var stream = streams[session.SessionID];
                stream.Write(buffer, 0, buffer.Length);
                session.Send("SOCKET_BUFFERING");
            } 
        }

        void socketServer_NewMessageReceived(WebSocketSession session, string msg)
        {
            ////string name = session.Cookies["name"] == null ? "" : session.Cookies["name"].ToString();
            //string name = Guid.NewGuid().ToString();
            //SendToAll(name + ": " + msg);

            //Debug.WriteLine()
            switch (msg)
            {
                case "FILE_DELETE":
                    fileDelete();
                    session.Send("FILE_DELETE_SUCCESS");
                    break;
                case "SENDING_COMPLETE":
                    bool ok = saveFile(session.SessionID);
                    if (ok)
                    {
                        session.Send("UPLOAD_SUCCESS");
                    }
                    else
                        session.Send("UPLOAD_FAIL");
                    session.Close();
                    break;
                default:
                    // Begin receive buffer of the files
                    if (msg[0] == '{' && msg[msg.Length - 1] == '}')
                    {
                        try
                        {
                            oFile fi = JsonConvert.DeserializeObject<oFile>(msg);
                            if (files.ContainsKey(session.SessionID))
                                files[session.SessionID] = fi;
                            else
                                files.TryAdd(session.SessionID, fi);

                            var stream = new MemoryStream();
                            streams.TryAdd(session.SessionID, stream);

                            session.Send("SOCKET_BUFFERING_START");
                        }
                        catch { }
                    }
                    break;
            }

        }


        void socketServer_NewSessionConnected(WebSocketSession session)
        {
            ////string name = session.Cookies["name"] == null ? "" : session.Cookies["name"].ToString();
            //string name = Guid.NewGuid().ToString();
            //SendToAll("System: " + name + " connected");

            //////if (pathIsValid == false) this.Close();
            //////else
            //////{
            //////    this.Send(this.SessionID);
            //////}
        }

        void socketServer_SessionClosed(WebSocketSession session, CloseReason reason)
        {
            if (reason == CloseReason.ServerShutdown) return;
            ////string name = session.Cookies["name"] == null ? "" : session.Cookies["name"].ToString();
            //string name = Guid.NewGuid().ToString();
            //SendToAll("System: " + name + " disconnected");
            freeRelease(session.SessionID);
        }

        void SendToAll(string message)
        {
            foreach (var s in m_WebSocketServer.GetAllSessions()) 
                s.Send(message); 
        }

        void Application_End(object sender, EventArgs e)
        {
            if (m_Bootstrap != null) m_Bootstrap.Stop();
        }
    }
}
