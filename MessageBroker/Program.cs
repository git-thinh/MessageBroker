using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    class Program
    {
        static void Main(string[] args)
        {
            LogService.Start(50051);

            const string url = "http://localhost:9000";
            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Server started at:" + url);
                Console.ReadLine();
            }

            //Console.WriteLine("Service running ...");
            //Console.ReadLine();
        }
    }
}
