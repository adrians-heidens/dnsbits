using DnsBits.Records;
using System;
using System.Collections.Generic;
using System.Globalization;

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
                list.Add(u.ToString("x4"));
            }
            return String.Join(":", list);
        }

        private static IRecord ReadRecord(ByteReader byteReader)
        {
            var name = DnsUtils.ReadName(byteReader);
            var rtype = byteReader.GetUshort();
            var rclass = byteReader.GetUshort();
            var ttl = byteReader.GetUint();

            if ((RecordType)rtype == RecordType.NS)
            {
                var record = new NSRecord()
                {
                    Name = name,
                    RClass = rclass,
                    Ttl = ttl,
                };
                var rdlength = byteReader.GetUshort();
                record.Host = ReadName(byteReader);
                return record;
            }
            else if ((RecordType)rtype == RecordType.A)
            {
                var rdlength = byteReader.GetUshort();
                var ipv4 = ReadIpv4(byteReader);
                var record = new ARecord(name, ipv4)
                {
                    RClass = rclass,
                    Ttl = ttl,
                };
                return record;
            }
            else if ((RecordType)rtype == RecordType.AAAA)
            {
                var record = new AaaaRecord()
                {
                    Name = name,
                    RClass = rclass,
                    Ttl = ttl,
                };
                var rdlength = byteReader.GetUshort();
                record.IPv6 = ReadIpv6(byteReader);
                return record;
            }
            else
            {
                var record = new Record()
                {
                    Name = name,
                    RType = rtype,
                    RClass = (ushort)rclass,
                    Ttl = ttl,
                };
                var rdlength = byteReader.GetUshort();
                record.RData = byteReader.GetBytes(rdlength);
                return record;
            }
        }

        /// <summary>
        /// Parse byte array as a DNS message.
        /// </summary>
        public static DnsMessage ReadDnsMessage(byte[] message)
        {
            var byteReader = new ByteReader(message);
            var header = DnsHeader.FromByteReader(byteReader);

            DnsQuestion question = null;
            if (header.QDCOUNT == 1)
            {
                question = DnsQuestion.FromByteReader(byteReader);
            }
            else if (header.QDCOUNT > 1)
            {
                throw new DnsBitsException($"Unexpected number of questions: {header.QDCOUNT}");
            }

            var answer = new List<IRecord>();
            for (int i = 0; i < header.ANCOUNT; i++)
            {
                var record = ReadRecord(byteReader);
                answer.Add(record);
            }

            var authority = new List<IRecord>();
            for (int i = 0; i < header.NSCOUNT; i++)
            {
                var record = ReadRecord(byteReader);
                authority.Add(record);
            }

            var additional = new List<IRecord>();
            for (int i = 0; i < header.ARCOUNT; i++)
            {
                var record = ReadRecord(byteReader);
                additional.Add(record);
            }

            if (!byteReader.IsFinished)
            {
                throw new Exception("Dns data not exhausted.");
            }

            return new DnsMessage {
                Header = header,
                Question = question,
                Answer = answer,
                Authority = authority,
                Additional = additional,
            };
        }

        /// <summary>
        /// Add ipv4 value to byte writer.
        /// </summary>
        private static void AddIpv4(string ipv4, ByteWriter byteWriter)
        {
            var parts = ipv4.Split(".");
            if (parts.Length != 4)
            {
                throw new DnsBitsException($"Invalid Ipv4 value: '{ipv4}'");
            }
            foreach (var part in parts)
            {
                byteWriter.AddByte(byte.Parse(part));
            }
        }

        /// <summary>
        /// Add ipv6 value to byte writer.
        /// </summary>
        private static void AddIpv6(string ipv6, ByteWriter byteWriter)
        {
            var parts = ipv6.Split(":");
            if (parts.Length != 8)
            {
                throw new DnsBitsException($"Invalid Ipv6 value: '{ipv6}'");
            }
            foreach (var part in parts)
            {
                ushort num = ushort.Parse(part, NumberStyles.HexNumber);
                byteWriter.AddUshort(num);
            }
        }

        /// <summary>
        /// Get bytes for domain name.
        /// </summary>
        private static byte[] GetNameBytes(string name)
        {
            var byteWriter = new ByteWriter();
            var labels = name.Split(".");
            foreach (var label in labels)
            {
                byteWriter.AddByte((byte)label.Length);
                byteWriter.AddString(label);
            }
            byteWriter.AddByte(0);
            return byteWriter.GetValue();
        }

        private static void AddRecord(IRecord record, ByteWriter byteWriter)
        {
            if (record.GetType() == typeof(AaaaRecord))
            {
                var rec = (AaaaRecord)record;
                byteWriter.AddBytes(GetNameBytes(rec.Name));
                byteWriter.AddUshort(rec.RType);
                byteWriter.AddUshort(rec.RClass);
                byteWriter.AddUint(rec.Ttl);
                byteWriter.AddUshort(16); // rdlength.
                AddIpv6(rec.IPv6, byteWriter);
            }
            else if (record.GetType() == typeof(ARecord))
            {
                var rec = (ARecord)record;
                byteWriter.AddBytes(GetNameBytes(rec.Name));
                byteWriter.AddUshort(rec.RType);
                byteWriter.AddUshort(rec.RClass);
                byteWriter.AddUint(rec.Ttl);
                byteWriter.AddUshort(4); // rdlength.
                AddIpv4(rec.IPv4, byteWriter);
            }
            else if (record.GetType() == typeof(NSRecord))
            {
                var rec = (NSRecord)record;
                byteWriter.AddBytes(GetNameBytes(rec.Name));
                byteWriter.AddUshort(rec.RType);
                byteWriter.AddUshort(rec.RClass);
                byteWriter.AddUint(rec.Ttl);
                var hostBytes = GetNameBytes(rec.Host);
                byteWriter.AddUshort((ushort)hostBytes.Length); // rdlength.
                byteWriter.AddBytes(hostBytes);
            }
            else if (record.GetType() == typeof(Record))
            {
                var rec = (Record)record;
                byteWriter.AddBytes(GetNameBytes(rec.Name));
                byteWriter.AddUshort(rec.RType);
                byteWriter.AddUshort(rec.RClass);
                byteWriter.AddUint(rec.Ttl);
                byteWriter.AddUshort(rec.RDLength);
                byteWriter.AddBytes(rec.RData);
            }
            else
            {
                throw new DnsBitsException($"Unexpected record type: {record.GetType()}");
            }
        }

        /// <summary>
        /// Conver DNS message to byte array ready for transporting.
        /// </summary>
        public static byte[] DnsMessageToBytes(DnsMessage message)
        {
            var byteWriter = new ByteWriter();

            var header = message.Header;
            byteWriter.AddBytes(header.ToBytes());

            // Question.
            if (message.Question != null)
            {
                byteWriter.AddBytes(message.Question.ToBytes());
            }

            // Answers.
            foreach (var record in message.Answer)
            {
                AddRecord(record, byteWriter);
            }

            // Authority.
            foreach (var record in message.Authority)
            {
                AddRecord(record, byteWriter);
            }

            // Additional.
            foreach (var record in message.Additional)
            {
                AddRecord(record, byteWriter);
            }

            return byteWriter.GetValue();
        }

        /// <summary>
        /// Read bytes as DNS answer message and print it.
        /// </summary>
        public static void ReadAndPrintDnsMessage(byte[] message)
        {
            var dnsMessage = ReadDnsMessage(message);
            Console.WriteLine(dnsMessage.ToMultiString());
        }
    }
}
