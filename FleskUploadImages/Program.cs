using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace FleskUploadImages
{
    public class oFile
    {
        public string folder { set; get; }
        public string name { set; get; }
        public string type { set; get; }
        public int size { set; get; }
    }

    class Program
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
            get { return string.IsNullOrEmpty(rootPath) == false && Directory.Exists(rootPath); }
        }

        static void freeRelease(string SessionID)
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

        static bool saveFile(string SessionID)
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
                        //stream.CopyTo(ms);
                        byte[] buf = stream.ToArray();
                        ms.Write(buf, 0, buf.Length);
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

        static void Main(string[] args)
        {
            string HOST_WS_UPLOAD = ConfigurationManager.AppSettings["HOST_WS_UPLOAD"];
            var server = new WebSocketServer(HOST_WS_UPLOAD);
            server.Start(socket =>
            {
                socket.OnOpen = () => {

                };
                socket.OnClose = () => {
                    freeRelease(socket.ConnectionInfo.Id.ToString());
                };
                socket.OnMessage = msg => {
                    switch (msg)
                    {
                        case "FILE_DELETE":
                            //fileDelete();
                            socket.Send("FILE_DELETE_SUCCESS");
                            break;
                        case "SENDING_COMPLETE":
                            bool ok = saveFile(socket.ConnectionInfo.Id.ToString());
                            if (ok)
                            {
                                socket.Send("UPLOAD_SUCCESS");
                            }
                            else
                                socket.Send("UPLOAD_FAIL");
                            socket.Close();
                            break;
                        default:
                            // Begin receive buffer of the files
                            if (msg[0] == '{' && msg[msg.Length - 1] == '}')
                            {
                                try
                                {
                                    oFile fi = JsonConvert.DeserializeObject<oFile>(msg);
                                    if (files.ContainsKey(socket.ConnectionInfo.Id.ToString()))
                                        files[socket.ConnectionInfo.Id.ToString()] = fi;
                                    else
                                        files.TryAdd(socket.ConnectionInfo.Id.ToString(), fi);

                                    var stream = new MemoryStream();
                                    streams.TryAdd(socket.ConnectionInfo.Id.ToString(), stream);

                                    socket.Send("SOCKET_BUFFERING_START");
                                }
                                catch { }
                            }
                            break;
                    }
                };
                socket.OnBinary = buffer => {
                    if (streams.ContainsKey(socket.ConnectionInfo.Id.ToString()))
                    {
                        var stream = streams[socket.ConnectionInfo.Id.ToString()];
                        stream.Write(buffer, 0, buffer.Length);
                        socket.Send("SOCKET_BUFFERING");
                    }
                };
            });
            Console.Title = HOST_WS_UPLOAD;
            Console.ReadLine();
        }
    }
}
