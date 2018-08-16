using System;
using System.Net;
using System.Net.Sockets;

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

            IPEndPoint endPoint = new IPEndPoint(ipAddresses[0], 53);

            socket.Connect(endPoint);

            socket.Send(new byte[] { 0x01, 0x02 });

            socket.Close();
        }

    }
}
