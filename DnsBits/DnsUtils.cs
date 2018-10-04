using System;
using System.Collections.Generic;

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

        private static void PrintResourceRecord(DnsResourceRecord record)
        {
            Console.WriteLine($">>> NAME: { record.NAME }");

            var rtype = (RecordType)record.TYPE;
            Console.WriteLine($">>> TYPE: { rtype }");

            Console.WriteLine($">>> CLASS: { (RecordClass) record.CLASS }");
            Console.WriteLine($">>> TTL: { record.TTL }");
            Console.WriteLine($">>> RDLENGTH: { record.RDLENGTH }");
            Console.WriteLine($">>> RDATA: { BitConverter.ToString(record.RDATA) }");

            Console.WriteLine("");
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
                var record = DnsResourceRecord.FromByteReader(byteReader);
                PrintResourceRecord(record);
            }

            // Authority.
            Console.WriteLine($"+++ Authority ({ header.NSCOUNT }):");
            for (int i = 0; i < header.NSCOUNT; i++)
            {
                var record = DnsResourceRecord.FromByteReader(byteReader);
                PrintResourceRecord(record);
            }

            // Additional.
            Console.WriteLine($"+++ Additional ({ header.ARCOUNT }):");
            for (int i = 0; i < header.ARCOUNT; i++)
            {
                var record = DnsResourceRecord.FromByteReader(byteReader);
                PrintResourceRecord(record);
            }

            if (! byteReader.IsFinished)
            {
                throw new Exception("Dns data not exhausted.");
            }
        }
    }
}
