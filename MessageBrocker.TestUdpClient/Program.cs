using MessageBroker;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBrocker.TestUdpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(3000);

            int PORT_DB_NOTIFICATION_UDP = int.Parse(ConfigurationManager.AppSettings["PORT_DB_NOTIFICATION_UDP"]);
            string HOST_DB_NOTIFICATION_UDP = ConfigurationManager.AppSettings["HOST_DB_NOTIFICATION_UDP"];
            //create a new client
            var client = UdpUser.ConnectTo(HOST_DB_NOTIFICATION_UDP, PORT_DB_NOTIFICATION_UDP);

            Console.WriteLine("UDP -> " + HOST_DB_NOTIFICATION_UDP + ":" + PORT_DB_NOTIFICATION_UDP +
                "\r\n=> Message format is !pawn_info.{0}.{1}.{2} "+
                "\r\n=> {0}: Pawn_ID - {1}: Status of pawn - {2}; Message content encode UTF8"+
                "\r\nSERVER_REPLY: OK= ... " +
                "\r\n\r\nKeyboard -> to send a notification\r\n\r\n");

            //wait for reply messages from server and send them to console 
            Task.Factory.StartNew(async () => {
                while (true)
                {
                    try
                    {
                        var received = await client.Receive();
                        Console.WriteLine(received.Message);
                        if (received.Message.Contains("quit")) break;
                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex);
                    }
                }
            });

            //type ahead :-)
            string read;
            do
            {
                read = string.Format("!pawn_info.{0}.{1}.{2}{3}", new Random().Next(1, int.MaxValue), new Random().Next(1, 5), "Here is message encode UTF8, ", Guid.NewGuid().ToString());
                Console.ReadKey();
                client.Send(read);
            } while (read != "quit");
        }
    }
}
