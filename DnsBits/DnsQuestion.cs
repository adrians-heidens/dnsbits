namespace DnsBits
{
    /// <summary>
    /// Dns question.
    /// </summary>
    public class DnsQuestion
    {
        public string Name { get; set; }

        public ushort QType { get; set; }

        public ushort QClass { get; set; }

        /// <summary>
        /// Get byte representation of the question.
        /// </summary>
        public byte[] ToBytes()
        {
            var byteWriter = new ByteWriter();

            var labels = Name.Split(".");
            foreach (var label in labels)
            {
                byteWriter.AddByte((byte)label.Length);
                byteWriter.AddString(label);
            }
            byteWriter.AddByte(0);

            byteWriter.AddUshort(QType);
            byteWriter.AddUshort(QClass);

            return byteWriter.GetValue();
        }

        /// <summary>
        /// Create new header from byte array.
        /// </summary>
        public static DnsQuestion FromBytes(byte[] bytes)
        {
            var byteReader = new ByteReader(bytes);
            return FromByteReader(byteReader);
        }

        /// <summary>
        /// Create new header from ByteReader.
        /// </summary>
        public static DnsQuestion FromByteReader(ByteReader byteReader)
        {
            var question = new DnsQuestion();

            question.Name = DnsUtils.ReadName(byteReader);
            question.QType = byteReader.GetUshort();
            question.QClass = byteReader.GetUshort();
            
            return question;
        }

        public override string ToString()
        {
            return $"DnsQuestion(" +
                $"Name='{Name}', " +
                $"QType='{(RecordType)QType}', " +
                $"QClass='{(RecordClass)QClass}')";
        }
    }
}
