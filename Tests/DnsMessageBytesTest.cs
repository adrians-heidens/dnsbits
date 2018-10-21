using DnsBits;
using DnsBits.Records;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Tests
{
    [TestClass]
    public class DnsMessageBytesTest
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

        [TestMethod]
        public void MessageBytesTest()
        {
            var message = new DnsMessage();
            message.Header = new DnsHeader() {
                ID = 1234,
                QR = 0,
                QDCOUNT = 1,
                ANCOUNT = 1,
            };
            message.Question = new DnsQuestion()
            {
                QCLASS = 1,
                QTYPE = 1,
                QNAME = "f00.test",
            };
            message.Answer.Add(new ARecord()
            {
                Name = "f00.test",
                RClass = 1,
                Ttl = 10,
                IPv4 = "127.0.0.2",
            });
            Console.WriteLine(message.ToMultiString());

            Console.WriteLine($"Header is {message.Header.ToBytes().Length} bytes.");
            Console.WriteLine($"Question is {message.Question.ToBytes().Length} bytes.");

            var messageBytes = DnsUtils.DnsMessageToBytes(message);
            Console.WriteLine($"Message is {messageBytes.Length} bytes");

            var message2 = DnsUtils.ReadDnsMessage(messageBytes);
            Console.WriteLine(message2.ToMultiString());
        }
    }
}
