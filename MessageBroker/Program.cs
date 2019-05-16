using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Routing;
using WebApiShared;

namespace MessageBroker
{
    class Program
    {
        static void Main(string[] args)
        {
            //---------------------------------------------------------------------
            // [Job process data]

            var _dataflow = new DataflowSubscribers();
            //_dataflow.RegisterHandler<JobCache>();
            _dataflow.RegisterHandler<JobDatabase>();
            _dataflow.RegisterHandler<JobDbSynchronize>();

            //[LOG_OUTPUT] Open WebSocket listener for log print output
            int PORT_LOG_OUTPUT = int.Parse(ConfigurationManager.AppSettings["PORT_LOG_OUTPUT"]);
            _dataflow.RegisterHandler<JobLogPrintOut>(new JobLogPrintOut(), true, new Dictionary<string, object>() { { "port", PORT_LOG_OUTPUT } });

            //[LOG_INPUT] Open Login service to receive message log
            int PORT_LOG_INPUT = int.Parse(ConfigurationManager.AppSettings["PORT_LOG_INPUT"]);
            LogService.Start(PORT_LOG_INPUT, _dataflow);

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
                    await dataflow.Enqueue(new JobDbSynchronize(received.Message));
                    Thread.Sleep(100);
                }
            }, _dataflow);

            //---------------------------------------------------------------------
            //[WEBAPI_CACHE] Open WebApi to cache objects on ApiController
            int PORT_WEBAPI_CACHE = int.Parse(ConfigurationManager.AppSettings["PORT_WEBAPI_CACHE"]);
            WebApp.Start<Startup>("http://*:" + PORT_WEBAPI_CACHE);

            //---------------------------------------------------------------------
            Console.WriteLine("-> PORT_LOG_INPUT: " + PORT_LOG_INPUT);
            Console.WriteLine("-> PORT_LOG_OUTPUT: " + PORT_LOG_OUTPUT);
            Console.WriteLine("-> PORT_WEBAPI_CACHE: " + PORT_WEBAPI_CACHE);
            Console.WriteLine("-> PORT_DB_NOTIFICATION_UDP: " + PORT_DB_NOTIFICATION_UDP);
            //---------------------------------------------------------------------
            // [ FREE_RESOURCE ]
            //Console.WriteLine("-> Enter to free resource jobs ....");
            Console.ReadLine();
            _dataflow.freeResourceAllJob();
            //Thread.Sleep(1000);
            //Console.WriteLine("-> Enter to exit ....");
            //Console.ReadLine();
        }
    }
}
