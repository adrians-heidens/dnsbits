using System.Collections.Generic;
using System.Text;

namespace DnsBits
{
    public class DnsMessage
    {
        public DnsHeader Header { get; set; }

        public List<DnsQuestion> Question { get; set; }

        public List<IResourceRecord> Answer { get; set; }

        public List<IResourceRecord> Authority { get; set; }

        public List<IResourceRecord> Additional { get; set; }

        public override string ToString()
        {
            return $"DnsMessage(" +
                $"id='{Header.ID}', " +
                $"ancount={Answer.Count}, " +
                $"nscount={Authority.Count}, " +
                $"arcount={Additional.Count})";
        }

        public string ToMultiString ()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($">>> ID: { Header.ID }");

            builder.AppendLine($">>> QR: { Header.QR }");
            builder.AppendLine($">>> OPCODE: { Header.OPCODE }");
            builder.AppendLine($">>> AA: { Header.AA }");
            builder.AppendLine($">>> TC: { Header.TC }");
            builder.AppendLine($">>> RD: { Header.RD }");

            builder.AppendLine($">>> RA: { Header.RA }");
            builder.AppendLine($">>> Z: { Header.Z }");
            builder.AppendLine($">>> RCODE: { Header.RCODE }");

            builder.AppendLine($">>> QDCOUNT: { Header.QDCOUNT }");
            builder.AppendLine($">>> ANCOUNT: { Header.ANCOUNT }");
            builder.AppendLine($">>> NSCOUNT: { Header.NSCOUNT }");
            builder.AppendLine($">>> ARCOUNT: { Header.ARCOUNT }");
            builder.AppendLine("");

            builder.AppendLine($"+++ Question ({ Header.QDCOUNT }):");
            foreach (var question in Question)
            {
                builder.AppendLine($">>> QNAME: { question.QNAME }");
                builder.AppendLine($">>> QTYPE: { (RecordType)question.QTYPE }");
                builder.AppendLine($">>> QCLASS: { (RecordClass)question.QCLASS }");
            }

            builder.AppendLine("");
            builder.AppendLine($"+++ Answer ({ Header.ANCOUNT }):");
            foreach (var record in Answer)
            {
                builder.AppendLine(record.ToString());
            }

            builder.AppendLine("");
            builder.AppendLine($"+++ Authority ({ Header.NSCOUNT }):");
            foreach (var record in Authority)
            {
                builder.AppendLine(record.ToString());
            }

            builder.AppendLine("");
            builder.AppendLine($"+++ Additional ({ Header.ARCOUNT }):");
            foreach (var record in Additional)
            {
                builder.AppendLine(record.ToString());
            }

            return builder.ToString();
        }
    }
}
