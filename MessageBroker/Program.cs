using CacheEngineShared;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBroker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "F88.MessageBroker";
            //---------------------------------------------------------------------
            // [Job process data]

            var _dataflow = new DataflowSubscribers();
            //[LOG_INPUT] Open Login service to receive message log
            LogService.Start(_dataflow);

            _dataflow.RegisterHandler<JobDbUpdate>(new JobDbUpdate());
            _dataflow.RegisterHandler<JobSyncDbToCache>(new JobSyncDbToCache());

            int PORT_CLIENT_NOTIFICATION = int.Parse(ConfigurationManager.AppSettings["PORT_CLIENT_NOTIFICATION"]);
            _dataflow.RegisterHandler<JobClientNotification>(new JobClientNotification(), new Dictionary<string, object>() { { "port", PORT_CLIENT_NOTIFICATION } });

            int PORT_CACHE_STORE = int.Parse(ConfigurationManager.AppSettings["PORT_CACHE_STORE"]);
            _dataflow.RegisterHandler<JobCacheStore>(new JobCacheStore(), new Dictionary<string, object>() { { "port", PORT_CACHE_STORE } });

            //[LOG_OUTPUT] Open WebSocket listener for log print output
            int PORT_LOG_OUTPUT = int.Parse(ConfigurationManager.AppSettings["PORT_LOG_OUTPUT"]);
            _dataflow.RegisterHandler<JobLogPrintOut>(new JobLogPrintOut(), new Dictionary<string, object>() { { "port", PORT_LOG_OUTPUT } });

            string HOST_WS_UPLOAD = ConfigurationManager.AppSettings["HOST_WS_UPLOAD"];
            new UploadWsServer(_dataflow).Start(HOST_WS_UPLOAD);

            ////[DB_UPDATE] Open Login service to receive message log
            //int PORT_DB_UPDATE = int.Parse(ConfigurationManager.AppSettings["PORT_DB_UPDATE"]);
            //DbUpdateService.Start(PORT_DB_UPDATE, _dataflow);

            ////[CACHE_FIND] Open Login service to receive message log
            //int PORT_CACHE_FIND = int.Parse(ConfigurationManager.AppSettings["PORT_CACHE_FIND"]);
            //CacheFindService.Start(PORT_CACHE_FIND, _dataflow);

            //---------------------------------------------------------------------
            //[DB_NOTIFICATION] Open UDP listener for Database notifications
            int PORT_DB_NOTIFICATION_UDP = int.Parse(ConfigurationManager.AppSettings["PORT_DB_NOTIFICATION_UDP"]);
            Tuple<IDataflowSubscribers, IDataflowSubscribers> paraDbNoti = new Tuple<IDataflowSubscribers, IDataflowSubscribers>(_dataflow, _dataflow);
            Task.Factory.StartNew(async (Object obj) =>
            {
                IDataflowSubscribers dataflow = (IDataflowSubscribers)obj;
                var serverUDP = new UdpListener(PORT_DB_NOTIFICATION_UDP);
                while (true)
                {
                    var received = await serverUDP.Receive();
                    string msg = received.Message, url = "";
                    string[] a;
                    if (!string.IsNullOrWhiteSpace(msg))
                    {
                        switch (msg[0])
                        {
                            case '#':
                                //#EXPORT.PDF:hop_dong.1167678
                                a = msg.Substring(1).Split(':');
                                switch (a[0])
                                {
                                    case "EXPORT.PDF":
                                        if (a.Length > 1)
                                        {
                                            a = a[1].Split('.');
                                            if (a.Length > 1)
                                            {
                                                //_dataflow.enqueue(new JobPdfExport("http://localhost:9096/api/pawn_info/get_in_hop_dong?Pawn_ID=1167678&filetemp=in-hop-dong.html")).Wait();
                                                url = "http://localhost:9096/api/pawn_info/get_in_" + a[0] + "?Pawn_ID=" + a[1] + "&filetemp=in-" + a[0].Replace("_","-") + ".html";
                                                _dataflow.enqueue(new JobPdfExport(url)).Wait();
                                            }
                                        }
                                        break;
                                }
                                break;
                            case '!':
                                serverUDP.Reply("OK=" + msg, received.Sender);
                                break;
                        }
                        //serverUDP.Reply("copy " + received.Message, received.Sender);
                        await dataflow.enqueue(new JobSyncDbToCache(received.Message));
                    }
                    Thread.Sleep(100);
                }
            }, _dataflow);

            //---------------------------------------------------------------------
            //[WEBAPI_ADMIN] Open WebApi to cache objects on ApiController
            int PORT_WEBAPI_ADMIN = int.Parse(ConfigurationManager.AppSettings["PORT_WEBAPI_ADMIN"]);
            WebApp.Start<Startup>("http://*:" + PORT_WEBAPI_ADMIN);

            _dataflow.RegisterHandler<JobCacheRefresh>(new JobCacheRefresh());
            _dataflow.enqueue(new JobCacheRefresh()).Wait();

            _dataflow.RegisterHandler<JobPdfExport>(new JobPdfExport());


            //---------------------------------------------------------------------
            string PORT_LOG_INPUT = ConfigurationManager.AppSettings["PORT_LOG_INPUT"];
            //---------------------------------------------------------------------

            //_dataflow.CacheStore.serviceRegister("test", 20190517);

            string help = "\r\n>>> Please input command as follows: cls,clear | port | reload | exit -> Enter\r\n";
            Console.WriteLine(help);
            string cmd = Console.ReadLine().ToLower();
            while (cmd != "exit")
            {
                switch (cmd)
                {
                    case "cls":
                    case "clear":
                        Console.Clear();
                        break;
                    case "port":
                        Console.WriteLine("-> HOST_WS_UPLOAD: " + HOST_WS_UPLOAD);
                        Console.WriteLine("-> PORT_CLIENT_NOTIFICATION: " + PORT_CLIENT_NOTIFICATION);
                        Console.WriteLine("-> PORT_DB_NOTIFICATION_UDP: " + PORT_DB_NOTIFICATION_UDP);
                        Console.WriteLine("-> PORT_LOG_INPUT: " + PORT_LOG_INPUT);
                        Console.WriteLine("-> PORT_LOG_OUTPUT: " + PORT_LOG_OUTPUT);
                        Console.WriteLine("-> PORT_WEBAPI_ADMIN: " + PORT_WEBAPI_ADMIN);
                        break;
                    case "reload":
                        _dataflow.enqueue(new JobCacheRefresh()).Wait();
                        break;
                    case "pdf":
                        var client_pdf_udp = UdpUser.ConnectTo("127.0.0.1", PORT_DB_NOTIFICATION_UDP);
                        client_pdf_udp.Send("#EXPORT.PDF:hop_dong.1167678");
                        break;
                }
                Console.WriteLine(help);
                cmd = Console.ReadLine().ToLower();
            }
            //---------------------------------------------------------------------
            // [ FREE_RESOURCE ]
            //Console.ReadLine();
            LogService.Stop();
            _dataflow.freeResourceAllJob();
            Thread.Sleep(1000);
            Console.WriteLine("-> Services closing ....");
        }
    }
}
