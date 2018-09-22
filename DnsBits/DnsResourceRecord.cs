namespace DnsBits
{
    /// <summary>
    /// Dns resource record (direct representation).
    /// </summary>
    class DnsResourceRecord
    {
        /// <summary>
        /// Domain name.
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// Resource record type.
        /// </summary>
        public ushort TYPE { get; set; }

        /// <summary>
        /// Resource record class.
        /// </summary>
        public ushort CLASS { get; set; }

        /// <summary>
        /// Time to live for this record in seconds (for caching).
        /// </summary>
        /// <remarks>
        /// 0 value means no caching at all.
        /// </remarks>
        public uint TTL { get; set; }

        /// <summary>
        /// Length of the RDATA field.
        /// </summary>
        public ushort RDLENGTH { get { return (ushort)RDATA.Length; } }

        /// <summary>
        /// Content of the resource record.
        /// </summary>
        public byte[] RDATA { get; set; }

        /// <summary>
        /// Get byte representation of the resource record.
        /// </summary>
        public byte[] ToBytes()
        {
            var byteWriter = new ByteWriter();
            return byteWriter.GetValue();
        }

        /// <summary>
        /// Create new header from byte array.
        /// </summary>
        public static DnsResourceRecord FromBytes(byte[] bytes)
        {
            var byteReader = new ByteReader(bytes);
            return FromByteReader(byteReader);
        }

        /// <summary>
        /// Create new resource record from byte reader.
        /// </summary>
        public static DnsResourceRecord FromByteReader(ByteReader byteReader)
        {
            var record = new DnsResourceRecord();

            record.NAME = DnsUtils.ReadName(byteReader);
            record.TYPE = byteReader.GetUshort();
            record.CLASS = byteReader.GetUshort();
            record.TTL = byteReader.GetUint();

            var rdlength = byteReader.GetUshort();
            record.RDATA = byteReader.GetBytes(rdlength);

            return record;
        }

        public override string ToString()
        {
            return $"DnsResourceRecord()";
        }
    }
}
