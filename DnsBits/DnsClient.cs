using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DnsBits
{
    public class DnsClient
    {
        /// <summary>
        /// Resolve and return IPv4 address from hostname.
        /// </summary>
        /// <param name="hostName">Hostname to resolve</param>
        /// <returns>Ipv4 address</returns>
        private IPAddress GetIpv4Address(string hostName)
        {
            IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);
            foreach (var ipAddress in ipAddresses)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipAddress;
                }
            }
            throw new Exception($"Could not resolve hostname '{hostName}' to Ipv4 address");
        }

        public void Query(string serverHost, int serverPort, string name)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            var ipv4Address = GetIpv4Address(serverHost);
            IPEndPoint endPoint = new IPEndPoint(ipv4Address, serverPort);

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
