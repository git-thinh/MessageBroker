class Program 
{
    static void Main(string[] args)
    {
        //create a new server
        var server = new UdpListener();

        //start listening for messages and copy the messages back to the client
        Task.Factory.StartNew(async () => {
            while (true)
            {
                var received = await server.Receive();
                server.Reply("copy " + received.Message, received.Sender);
                if (received.Message == "quit")
                    break;
            }
        });

        //create a new client
        var client = UdpUser.ConnectTo("127.0.0.1", 32123);

        //wait for reply messages from server and send them to console 
        Task.Factory.StartNew(async () => {
            while (true)
            {
                try
                {
                    var received = await client.Receive();
                    Console.WriteLine(received.Message);
                    if (received.Message.Contains("quit"))
                        break;
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
            read = Console.ReadLine();
            client.Send(read);
        } while (read != "quit");
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace desktop
{
    class Desktop
    {
        // TODO: check all errors
        static void Main(string[] args)
        {
            Int32 counter = 0;
            while (true)
            {
                SendUDP("127.0.0.1", 41181, counter.ToString(), counter.ToString().Length);
                Thread.Sleep(50);
                counter++;
            }
        }
        public static void SendUDP(string hostNameOrAddress, int destinationPort, string data, int count)
        {
            IPAddress destination = Dns.GetHostAddresses(hostNameOrAddress)[0];
            IPEndPoint endPoint = new IPEndPoint(destination, destinationPort);
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            for (int i = 0; i < count; i++)
            {
                socket.SendTo(buffer, endPoint);
            }
            socket.Close();
            System.Console.WriteLine("Sent: " + data);
        }
    }
}












