using System;
using System.Collections.Generic;
using System.Text;

namespace DnsBits
{
    public static class DnsUtils
    {
        /// <summary>
        /// Create DNS question message for A records.
        /// </summary>
        public static byte[] CreateQuestionARec(string name)
        {
            var byteWriter = new ByteWriter();

            var header = new DnsHeader();
            header.ID = GetId();
            header.QDCOUNT = 1;
            byteWriter.AddBytes(header.ToBytes());

            var question = new DnsQuestion();
            question.QNAME = name;
            question.QTYPE = 1;
            question.QCLASS = 1;
            byteWriter.AddBytes(question.ToBytes());

            return byteWriter.GetValue();
        }

        private static ushort GetId()
        {
            Random random = new Random();
            return (ushort)random.Next(ushort.MinValue, ushort.MaxValue);
        }

        /// <summary>
        /// Read domain name from ByteReader.
        /// </summary>
        public static string ReadName(ByteReader byteReader)
        {
            var labels = new List<string>();

            var compressed = byteReader.GetBits(2);
            var length = byteReader.GetBits(6);

            while (compressed == 0 && length != 0)
            {
                labels.Add(byteReader.GetString(length));
                compressed = byteReader.GetBits(2);
                length = byteReader.GetBits(6);
            }

            if (compressed == 3)
            {
                var offset = (length << 6) | byteReader.GetByte();
                var position = byteReader.GetPosition();
                byteReader.SetPosition(offset);
                labels.Add(ReadName(byteReader));
                byteReader.SetPosition(position);
            }

            if (compressed != 0 && compressed != 3)
            {
                throw new Exception("Unexpected name compression indicator.");
            }

            return string.Join(".", labels);
        }

        private static string ReadIpv4(ByteReader byteReader)
        {
            var b1 = byteReader.GetByte();
            var b2 = byteReader.GetByte();
            var b3 = byteReader.GetByte();
            var b4 = byteReader.GetByte();
            return $"{b1}.{b2}.{b3}.{b4}";
        }

        private static string ReadIpv6(ByteReader byteReader)
        {
            var list = new List<string>();
            for (int i = 0; i < 8; i++)
            {
                var u = byteReader.GetUshort();
                list.Add(u.ToString("x2"));
            }
            return String.Join(":", list);
        }

        private static void PrintResourceRecord(IResourceRecord record)
        {
            Console.WriteLine(record);
        }

        private static IResourceRecord ReadRecord(ByteReader byteReader)
        {
            var name = DnsUtils.ReadName(byteReader);
            var rtype = byteReader.GetUshort();
            var rclass = byteReader.GetUshort();
            var ttl = byteReader.GetUint();

            if ((RecordType)rtype == RecordType.NS)
            {
                var record = new NSRecord()
                {
                    NAME = name,
                    TYPE = rtype,
                    CLASS = rclass,
                    TTL = ttl,
                };
                var rdlength = byteReader.GetUshort();
                record.HOST = ReadName(byteReader);
                return record;
            }
            else if ((RecordType)rtype == RecordType.A)
            {
                var record = new ARecord()
                {
                    NAME = name,
                    TYPE = rtype,
                    CLASS = rclass,
                    TTL = ttl,
                };
                var rdlength = byteReader.GetUshort();
                record.IPV4 = ReadIpv4(byteReader);
                return record;
            }
            else if ((RecordType)rtype == RecordType.AAAA)
            {
                var record = new AAAARecord()
                {
                    NAME = name,
                    TYPE = rtype,
                    CLASS = rclass,
                    TTL = ttl,
                };
                var rdlength = byteReader.GetUshort();
                record.IPV6 = ReadIpv6(byteReader);
                return record;
            }
            else
            {
                var record = new DnsResourceRecord()
                {
                    NAME = name,
                    TYPE = rtype,
                    CLASS = rclass,
                    TTL = ttl,
                };
                var rdlength = byteReader.GetUshort();
                record.RDATA = byteReader.GetBytes(rdlength);
                return record;
            }
        }

        /// <summary>
        /// Read bytes as DNS answer message and print it.
        /// </summary>
        public static void ReadDnsAnswerMessage(byte[] message)
        {
            var byteReader = new ByteReader(message);

            var header = DnsHeader.FromByteReader(byteReader);

            Console.WriteLine($">>> ID: { header.ID }");

            Console.WriteLine($">>> QR: { header.QR }");
            Console.WriteLine($">>> OPCODE: { header.OPCODE }");
            Console.WriteLine($">>> AA: { header.AA }");
            Console.WriteLine($">>> TC: { header.TC }");
            Console.WriteLine($">>> RD: { header.RD }");

            Console.WriteLine($">>> RA: { header.RA }");
            Console.WriteLine($">>> Z: { header.Z }");
            Console.WriteLine($">>> RCODE: { header.RCODE }");

            Console.WriteLine($">>> QDCOUNT: { header.QDCOUNT }");
            Console.WriteLine($">>> ANCOUNT: { header.ANCOUNT }");
            Console.WriteLine($">>> NSCOUNT: { header.NSCOUNT }");
            Console.WriteLine($">>> ARCOUNT: { header.ARCOUNT }");
            Console.WriteLine("");

            // Question.
            Console.WriteLine($"+++ Question ({ header.QDCOUNT }):");
            for (int i = 0; i < header.QDCOUNT; i++)
            {
                var question = DnsQuestion.FromByteReader(byteReader);
                Console.WriteLine($">>> QNAME: { question.QNAME }");
                Console.WriteLine($">>> QTYPE: { (RecordType) question.QTYPE }");
                Console.WriteLine($">>> QCLASS: { (RecordClass) question.QCLASS }");
                Console.WriteLine("");
            }

            // Answer.
            Console.WriteLine($"+++ Answer ({ header.ANCOUNT }):");
            for (int i = 0; i < header.ANCOUNT; i++)
            {
                var record = ReadRecord(byteReader);
                PrintResourceRecord(record);
            }

            // Authority.
            Console.WriteLine($"+++ Authority ({ header.NSCOUNT }):");
            for (int i = 0; i < header.NSCOUNT; i++)
            {
                var record = ReadRecord(byteReader);
                PrintResourceRecord(record);
            }

            // Additional.
            Console.WriteLine($"+++ Additional ({ header.ARCOUNT }):");
            for (int i = 0; i < header.ARCOUNT; i++)
            {
                var record = ReadRecord(byteReader);
                PrintResourceRecord(record);
            }

            if (! byteReader.IsFinished)
            {
                throw new Exception("Dns data not exhausted.");
            }
        }
    }
}
