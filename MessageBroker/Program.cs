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

            Console.WriteLine("Service running ...");
            Console.ReadLine();
        }
    }
}
