namespace DnsBits
{
    /// <summary>
    /// Dns question (direct representation).
    /// </summary>
    class DnsQuestion
    {
        public string QNAME { get; set; }

        public ushort QTYPE { get; set; }

        public ushort QCLASS { get; set; }

        /// <summary>
        /// Get byte representation of the question.
        /// </summary>
        public byte[] ToBytes()
        {
            var byteWriter = new ByteWriter();

            var labels = QNAME.Split(".");
            foreach (var label in labels)
            {
                byteWriter.AddByte((byte)label.Length);
                byteWriter.AddString(label);
            }
            byteWriter.AddByte(0);

            byteWriter.AddUshort(QTYPE);
            byteWriter.AddUshort(QCLASS);

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

            question.QNAME = DnsUtils.ReadName(byteReader);
            question.QTYPE = byteReader.GetUshort();
            question.QCLASS = byteReader.GetUshort();
            
            return question;
        }

        public override string ToString()
        {
            return $"DnsQuestion(QNAME='{QNAME}', QTYPE='{QTYPE}', QCLASS='{QCLASS}')";
        }
    }
}
