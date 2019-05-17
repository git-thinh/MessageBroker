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
            //---------------------------------------------------------------------
            // [Job process data]

            var _dataflow = new DataflowSubscribers();
            //[LOG_INPUT] Open Login service to receive message log
            LogService.Start(_dataflow);

            _dataflow.RegisterHandler<JobDbUpdate>(new JobDbUpdate());
            _dataflow.RegisterHandler<JobSyncDbToCache>(new JobSyncDbToCache());

            int PORT_CACHE_STORE = int.Parse(ConfigurationManager.AppSettings["PORT_CACHE_STORE"]);
            _dataflow.RegisterHandler<JobCacheStore>(new JobCacheStore(), new Dictionary<string, object>() { { "port", PORT_CACHE_STORE } });
            
            //[LOG_OUTPUT] Open WebSocket listener for log print output
            int PORT_LOG_OUTPUT = int.Parse(ConfigurationManager.AppSettings["PORT_LOG_OUTPUT"]);
            _dataflow.RegisterHandler<JobLogPrintOut>(new JobLogPrintOut(), new Dictionary<string, object>() { { "port", PORT_LOG_OUTPUT } });


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
                    //serverUDP.Reply("copy " + received.Message, received.Sender);
                    await dataflow.enqueue(new JobSyncDbToCache(received.Message));
                    Thread.Sleep(100);
                }
            }, _dataflow);

            //---------------------------------------------------------------------
            //[WEBAPI_ADMIN] Open WebApi to cache objects on ApiController
            int PORT_WEBAPI_ADMIN = int.Parse(ConfigurationManager.AppSettings["PORT_WEBAPI_ADMIN"]);
            WebApp.Start<Startup>("http://*:" + PORT_WEBAPI_ADMIN);

            //---------------------------------------------------------------------
            string PORT_LOG_INPUT = ConfigurationManager.AppSettings["PORT_LOG_INPUT"];
            Console.WriteLine("-> PORT_DB_NOTIFICATION_UDP: " + PORT_DB_NOTIFICATION_UDP);
            Console.WriteLine("-> PORT_LOG_INPUT: " + PORT_LOG_INPUT);
            Console.WriteLine("-> PORT_LOG_OUTPUT: " + PORT_LOG_OUTPUT);
            Console.WriteLine("-> PORT_WEBAPI_ADMIN: " + PORT_WEBAPI_ADMIN);
            //---------------------------------------------------------------------

            //_dataflow.CacheStore.serviceRegister("test", 20190517);

            //---------------------------------------------------------------------
            // [ FREE_RESOURCE ]
            //Console.WriteLine("-> Enter to free resource jobs ....");
            Console.ReadLine();
            LogService.Stop();
            _dataflow.freeResourceAllJob();
            //Thread.Sleep(1000);
            //Console.WriteLine("-> Enter to exit ....");
            //Console.ReadLine();
        }
    }
}
