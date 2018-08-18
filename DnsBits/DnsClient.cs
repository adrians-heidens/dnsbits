using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DnsBits
{
    public class DnsClient
    {

        public void Query(string domainName)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPAddress[] ipAddresses = Dns.GetHostAddresses("192.168.1.202");
            if (ipAddresses.Length != 1)
            {
                throw new Exception("Got unexpected IP addresses");
            }

            IPEndPoint endPoint = new IPEndPoint(ipAddresses[0], 8080);

            socket.Connect(endPoint);

            var input = Encoding.UTF8.GetBytes("hello world");

            Console.WriteLine($"Sending: '{ Encoding.UTF8.GetString(input) }'");
            socket.Send(input);

            Console.WriteLine("Receiving...");
            var output = new byte[32];
            var c = socket.Receive(output);

            Console.WriteLine($"Received ({c}b): '{ Encoding.UTF8.GetString(output, 0, c) }'");

            socket.Close();
        }

    }
}
