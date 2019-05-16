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

            ////////////////////string baseAddress = "http://localhost:" + PORT_WEBAPI_CACHE + "/";
            ////////////////////ServiceHost host = new ServiceHost(typeof(Service), new Uri(baseAddress));
            ////////////////////host.AddServiceEndpoint(typeof(ITest), new BasicHttpBinding(), "");
            ////////////////////host.Description.Behaviors.Add(new MyBehavior());
            ////////////////////host.Open();

            ////////////////////Console.WriteLine("Host opened");

            ////////////////////ChannelFactory<ITest> factory = new ChannelFactory<ITest>(new BasicHttpBinding(), new EndpointAddress(baseAddress));
            ////////////////////ITest proxy = factory.CreateChannel();
            ////////////////////Console.WriteLine("WhoAmI: {0}", proxy.WhoAmI());

            ////////////////////((IClientChannel)proxy).Close();
            ////////////////////factory.Close();

            ////////////////////Console.Write("Press ENTER to close the host");
            ////////////////////Console.ReadLine();
            ////////////////////host.Close();

            //[LOG_OUTPUT] Open WebSocket listener for log print output
            int PORT_LOG_OUTPUT = int.Parse(ConfigurationManager.AppSettings["PORT_LOG_OUTPUT"]);
            string HOST_LOG_OUTPUT = ConfigurationManager.AppSettings["HOST_LOG_OUTPUT"];
            var _logOutput = new LogSocketsServer("ws://" + HOST_LOG_OUTPUT + ":" + PORT_LOG_OUTPUT);

            //[LOG_INPUT] Open Login service to receive message log
            int PORT_LOG_INPUT = int.Parse(ConfigurationManager.AppSettings["PORT_LOG_INPUT"]);
            LogService.Start(PORT_LOG_INPUT, _logOutput);

            //---------------------------------------------------------------------
            //[DB_NOTIFICATION] Open UDP listener for Database notifications
            int PORT_DB_NOTIFICATION_UDP = int.Parse(ConfigurationManager.AppSettings["PORT_DB_NOTIFICATION_UDP"]);
            Tuple<IDataflowSubscribers, ILogOutput> paraDbNoti = new Tuple<IDataflowSubscribers, ILogOutput>(_dataflow, _logOutput);
            Task.Factory.StartNew(async (Object obj) =>
            {
                Tuple<IDataflowSubscribers, ILogOutput> it = (Tuple<IDataflowSubscribers, ILogOutput>)obj;
                var serverUDP = new UdpListener(PORT_DB_NOTIFICATION_UDP);
                while (true)
                {
                    var received = await serverUDP.Receive();
                    //serverUDP.Reply("copy " + received.Message, received.Sender);
                    await it.Item1.Enqueue(new JobDbSynchronize(received.Message));
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
            Console.ReadLine();
            //---------------------------------------------------------------------
            // Free resource
            //_logOutput.Stop();
        }
    }
}
