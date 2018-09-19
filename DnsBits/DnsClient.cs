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

            var input = DnsUtils.CreateQuestionARec(name);

            Console.WriteLine($"Sending: { input.Length }bytes data");
            socket.Send(input);

            Console.WriteLine("Receiving...");
            var output = new byte[512];
            var c = socket.Receive(output);

            Console.WriteLine($"Received {c}bytes");

            var dnsPacket = new byte[c];
            Array.Copy(output, dnsPacket, c);
            DnsUtils.ReadDnsAnswerMessage(dnsPacket);

            socket.Close();
        }

    }
}
