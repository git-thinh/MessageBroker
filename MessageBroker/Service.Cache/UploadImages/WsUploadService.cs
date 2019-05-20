using Microsoft.ServiceModel.WebSockets;
using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

namespace MessageBroker
{
    public class oFile
    {
        public string folder { set; get; }
        public string name { set; get; }
        public string type { set; get; }
        public int size { set; get; }
    }

    public class WsUploadService : WebSocketService
    {
        public static string rootPath = ConfigurationManager.AppSettings["rootPathUpload"];

        private static ConcurrentDictionary<string, MemoryStream> streams = new ConcurrentDictionary<string, MemoryStream>();
        private static ConcurrentDictionary<string, oFile> files = new ConcurrentDictionary<string, oFile>();

        public string SessionID
        {
            get { return this.WebSocketContext.SecWebSocketKey; }
        }

        private bool pathIsValid
        {
            get { return string.IsNullOrWhiteSpace(rootPath) == false && Directory.Exists(rootPath); }
        }

        private void freeRelease()
        {
            try
            {
                oFile fi;
                files.TryRemove(this.SessionID, out fi);
                MemoryStream stream;
                if (streams.TryRemove(this.SessionID, out stream))
                    stream.Close();
            }
            catch { }
        }

        private bool saveFile()
        {
            try
            {
                if (files.ContainsKey(this.SessionID)
                    && streams.ContainsKey(this.SessionID))
                {
                    if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);

                    oFile fi = files[this.SessionID];
                    MemoryStream stream = streams[this.SessionID];
                    string pathFile = Path.Combine(rootPath, fi.name);

                    if (File.Exists(pathFile))
                        pathFile = Path.Combine(rootPath, DateTime.Now.ToString("yyyyMMdd-HHmmssfff-") + fi.name);

                    using (var ms = new FileStream(pathFile, FileMode.OpenOrCreate))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(ms);
                    }
                    stream.Close();
                    streams.TryRemove(this.SessionID, out stream);
                    return true;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return false;
        }


        public override void OnOpen()
        {
            if (pathIsValid == false) this.Close();
            else
            {
                this.Send(this.SessionID);
            }
        }

        public override void OnMessage(string msg)
        {
            //Debug.WriteLine()
            switch (msg)
            {
                case "FILE_DELETE":
                    fileDelete();
                    this.Send("FILE_DELETE_SUCCESS");
                    break;
                case "SENDING_COMPLETE":
                    bool ok = saveFile();
                    if (ok)
                    {
                        this.Send("UPLOAD_SUCCESS");
                    }
                    else
                        this.Send("UPLOAD_FAIL");
                    this.Close();
                    break;
                default:
                    // Begin receive buffer of the files
                    if (msg[0] == '{' && msg[msg.Length - 1] == '}')
                    {
                        try
                        {
                            oFile fi = JsonConvert.DeserializeObject<oFile>(msg);
                            if (files.ContainsKey(this.SessionID))
                                files[this.SessionID] = fi;
                            else
                                files.TryAdd(this.SessionID, fi);

                            var stream = new MemoryStream();
                            streams.TryAdd(this.SessionID, stream);

                            this.Send("0");
                        }
                        catch { }
                    }
                    break;
            }
        }

        private void fileDelete()
        {
            throw new NotImplementedException();
        }

        public override void OnMessage(Byte[] buffer)
        {
            if (streams.ContainsKey(this.SessionID))
            {
                var stream = streams[this.SessionID];
                stream.Write(buffer, 0, buffer.Length);
                this.Send("SOCKET_BUFFERING");
            }
        }

        protected override void OnClose()
        {
            freeRelease();
            base.OnClose();
        }

        protected override void OnError()
        {
            freeRelease();
            base.OnError();
        }
    }
}