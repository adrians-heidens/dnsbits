using System;
using System.Collections.Generic;

namespace DnsBits
{
    public static class DnsUtils
    {
        /// <summary>
        /// Create DNS question message for A records.
        /// </summary>
        public static byte[] CreateQuestionARec()
        {
            var byteWriter = new ByteWriter();

            byteWriter.AddUshort(12345);

            // Header.
            byteWriter.AddBits(1, 0);
            byteWriter.AddBits(4, 0);
            byteWriter.AddBits(1, 0);
            byteWriter.AddBits(1, 0);
            byteWriter.AddBits(1, 0);

            byteWriter.AddBits(1, 0);
            byteWriter.AddBits(3, 0);
            byteWriter.AddBits(4, 0);

            byteWriter.AddUshort(1);
            byteWriter.AddUshort(0);
            byteWriter.AddUshort(0);
            byteWriter.AddUshort(0);

            // Question.
            byteWriter.AddByte(3);
            byteWriter.AddString("ns1");
            byteWriter.AddByte(4);
            byteWriter.AddString("test");
            byteWriter.AddByte(0);

            byteWriter.AddUshort(1);
            byteWriter.AddUshort(1);
            
            return byteWriter.GetValue();
        }

        /// <summary>
        /// Read domain name from ByteReader.
        /// </summary>
        private static string ReadName(ByteReader byteReader)
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

        /// <summary>
        /// Read bytes as DNS answer message and print it.
        /// </summary>
        public static void ReadDnsAnswerMessage(byte[] message)
        {
            var byteReader = new ByteReader(message);

            // Header.
            Console.WriteLine($">>> ID: { byteReader.GetUshort() }");

            Console.WriteLine($">>> QR: { byteReader.GetBits(1) }");
            Console.WriteLine($">>> OPCODE: { byteReader.GetBits(4) }");
            Console.WriteLine($">>> AA: { byteReader.GetBits(1) }");
            Console.WriteLine($">>> TC: { byteReader.GetBits(1) }");
            Console.WriteLine($">>> RD: { byteReader.GetBits(1) }");

            Console.WriteLine($">>> RA: { byteReader.GetBits(1) }");
            Console.WriteLine($">>> Z: { byteReader.GetBits(3) }");
            Console.WriteLine($">>> RCODE: { byteReader.GetBits(4) }");

            Console.WriteLine($">>> QDCOUNT: { byteReader.GetUshort() }");
            Console.WriteLine($">>> ANCOUNT: { byteReader.GetUshort() }");
            Console.WriteLine($">>> NSCOUNT: { byteReader.GetUshort() }");
            Console.WriteLine($">>> ARCOUNT: { byteReader.GetUshort() }");

            // Question.
            var qname = ReadName(byteReader);
            Console.WriteLine($">>> QNAME: { qname }");
            Console.WriteLine($">>> QTYPE: { byteReader.GetUshort() }");
            Console.WriteLine($">>> QCLASS: { byteReader.GetUshort() }");

            // Answer.
            var name = ReadName(byteReader);
            Console.WriteLine($">>> NAME: { name }");
            Console.WriteLine($">>> TYPE: { byteReader.GetUshort() }");
            Console.WriteLine($">>> CLASS: { byteReader.GetUshort() }");
            Console.WriteLine($">>> TTL: { byteReader.GetUint() }");
            ushort rdlen = byteReader.GetUshort();
            Console.WriteLine($">>> RDLENGTH: { rdlen }");
            Console.WriteLine($">>> RDATA: { BitConverter.ToString(byteReader.GetBytes(rdlen)) }");
            
            // Authority.

            // Additional.
        }
    }
}
