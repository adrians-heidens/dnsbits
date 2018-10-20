using DnsBits;
using DnsBits.Records;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Tests
{
    [TestClass]
    public class DnsMessageReadTest
    {
        [TestMethod]
        public void Query1Test()
        {
            byte[] messageBytes = File.ReadAllBytes(Path.Combine("Files", "dns-query.dat"));
            var message = DnsUtils.ReadDnsMessage(messageBytes);
            Console.WriteLine(message.ToMultiString());
        }

        [TestMethod]
        public void Query2Test()
        {
            byte[] messageBytes = File.ReadAllBytes(Path.Combine("Files", "dns-02-query.dat"));
            var message = DnsUtils.ReadDnsMessage(messageBytes);
            Console.WriteLine(message.ToMultiString());
        }

        [TestMethod]
        public void Response2Test()
        {
            byte[] messageBytes = File.ReadAllBytes(Path.Combine("Files", "dns-02-response.dat"));
            var message = DnsUtils.ReadDnsMessage(messageBytes);
            Console.WriteLine(message.ToMultiString());
        }
    }
}
