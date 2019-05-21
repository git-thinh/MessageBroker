using CacheEngineShared;
using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    public class oFile
    {
        public int Id { set; get; }
        public string Code { set; get; }
        public string folder { set; get; }
        public string name { set; get; }
        public string nameNew { set; get; }
        public string type { set; get; }
        public int size { set; get; }
    }

    public class UploadWsServer
    {
        IDataflowSubscribers _dataflow;
        public UploadWsServer(IDataflowSubscribers dataflow) { _dataflow = dataflow; }

        #region [ UPLOAD ]

        static string rootPath = ConfigurationManager.AppSettings["rootPathUpload"];

        static ConcurrentDictionary<string, MemoryStream> streams = new ConcurrentDictionary<string, MemoryStream>();
        static ConcurrentDictionary<string, oFile> files = new ConcurrentDictionary<string, oFile>();

        bool pathIsValid
        {
            get { return string.IsNullOrEmpty(rootPath) == false && Directory.Exists(rootPath); }
        }

        void freeRelease(string SessionID)
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

        bool saveFile(IWebSocketConnection socket, string SessionID)
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

                    //if (File.Exists(pathFile))
                    //{
                    //    string fiNew = DateTime.Now.ToString("yyyyMMdd-HHmmssfff-") + fi.name;
                    //    fi.command = "FILE_CHANGE_NAME";
                    //    fi.nameNew = fiNew;
                    //    socket.Send(JsonConvert.SerializeObject(fi));
                    //    fi.command = string.Empty;
                    //    fi.name = fiNew;

                    //    pathFile = Path.Combine(rootPath, fiNew);
                    //}

                    using (var ms = new FileStream(pathFile, FileMode.OpenOrCreate))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(ms);
                        //byte[] buf = stream.ToArray();
                        //ms.Write(buf, 0, buf.Length);
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

        void fileDelete()
        {
        }

        #endregion

        static WebSocketServer server;
        public void Start(string HOST_WS_UPLOAD)
        {
            try
            {
                server = new WebSocketServer(HOST_WS_UPLOAD);
                server.Start(socket =>
                {
                    socket.OnOpen = () => { };
                    socket.OnClose = () =>
                    {
                        try
                        {
                            freeRelease(socket.ConnectionInfo.Id.ToString());
                        }
                        catch { }
                    };
                    socket.OnMessage = msg =>
                    {
                        try
                        {
                            switch (msg)
                            {
                                case "FILE_DELETE":
                                    //fileDelete();
                                    socket.Send("FILE_DELETE_SUCCESS");
                                    break;
                                case "SENDING_COMPLETE":
                                    bool ok = saveFile(socket, socket.ConnectionInfo.Id.ToString());
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

                                            // rename file = current time + file_name
                                            string fiNew = DateTime.Now.ToString("yyyyMMdd-HHmmssfff-") + fi.name;
                                            fi.Code = "FILE_CHANGE_NAME";
                                            fi.nameNew = fiNew;
                                            socket.Send(JsonConvert.SerializeObject(fi));
                                            fi.Code = string.Empty;
                                            fi.nameNew = string.Empty;
                                            fi.name = fiNew;

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
                        }
                        catch (Exception ex)
                        {
                            socket.Close();
                            _dataflow.enqueue(new JobLogPrintOut(ex.Message)).Wait();
                        }
                    };
                    socket.OnBinary = buffer =>
                    {
                        try
                        {
                            if (streams.ContainsKey(socket.ConnectionInfo.Id.ToString()))
                            {
                                var stream = streams[socket.ConnectionInfo.Id.ToString()];
                                stream.Write(buffer, 0, buffer.Length);
                                socket.Send("SOCKET_BUFFERING");
                            }
                        }
                        catch (Exception ex)
                        {
                            socket.Close();
                            _dataflow.enqueue(new JobLogPrintOut(ex.Message)).Wait();
                        }
                    };
                });
                //Console.Title = HOST_WS_UPLOAD;
                //Console.ReadLine();
            }
            catch (Exception exm)
            {
                _dataflow.enqueue(new JobLogPrintOut(exm.Message)).Wait();
            }
        }
    }
}
