using DnsBits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Tests
{
    [TestClass]
    public class DnsMessageReadTest
    {
        [TestMethod]
        public void Test()
        {
            byte[] messageBytes = File.ReadAllBytes(Path.Combine("Files", "dns-query.dat"));
            var message = DnsUtils.ReadDnsMessage(messageBytes);
            Console.WriteLine(message.ToMultiString());
        }
    }
}
