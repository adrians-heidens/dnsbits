namespace DnsBits
{
    /// <summary>
    /// Dns message header (direct representation).
    /// </summary>
    class DnsHeader
    {
        /// <summary>
        /// A 16 bit identifier used by the requester to match up replies.
        /// </summary>
        public ushort ID { get; set; }

        /// <summary>
        /// Specifies whether this message is a query(0), or a response(1).
        /// </summary>
        public byte QR { get; set; }

        /// <summary>
        /// A four bit field that specifies kind of query in this message.
        /// </summary>
        /// <remarks>
        /// 0 -- a standard query (QUERY)
        /// 1 -- an inverse query (IQUERY)
        /// 2 -- a server status request (STATUS)
        /// 3-15 -- reserved for future use
        /// </remarks>
        public byte OPCODE { get; set; }

        /// <summary>
        /// Authoritative Answer.
        /// </summary>
        public byte AA { get; set; }

        /// <summary>
        /// TrunCation -- specifies that this message was truncated.
        /// </summary>
        public byte TC { get; set; }

        /// <summary>
        /// Recursion desired (it directs the name server to pursue the query
        /// recursively).
        /// </summary>
        public byte RD { get; set; }

        /// <summary>
        /// Recursion Available (set or cleared in a response, and denotes
        /// whether recursive query support is available in the name server).
        /// </summary>
        public byte RA { get; set; }

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        public byte Z { get; set; }

        /// <summary>
        /// Response code.
        /// </summary>
        /// <remarks>
        /// 0 -- No error condition.
        /// 1 -- Format error - The server was unable to interpret the query.
        /// 2 -- Server failure.
        /// 3 -- Name Error (in auth response name does not exist).
        /// 4 -- Not Implemented (type of query not implemented).
        /// 5 -- Refused.
        /// 6-15 -- Reserved for future use.
        /// </remarks>
        public byte RCODE { get; set; }

        /// <summary>
        /// Number of entries in the question section.
        /// </summary>
        public ushort QDCOUNT { get; set; }

        /// <summary>
        /// Number of resource records in the answer section.
        /// </summary>
        public ushort ANCOUNT { get; set; }

        /// <summary>
        /// Number of records in authority section.
        /// </summary>
        public ushort NSCOUNT { get; set; }

        /// <summary>
        /// Number of resource records in the additional records section.
        /// </summary>
        public ushort ARCOUNT { get; set; }

        /// <summary>
        /// Get byte representation of the header.
        /// </summary>
        public byte[] ToBytes()
        {
            var byteWriter = new ByteWriter();
            
            byteWriter.AddUshort(ID);

            byteWriter.AddBits(1, QR);
            byteWriter.AddBits(4, OPCODE);
            byteWriter.AddBits(1, AA);
            byteWriter.AddBits(1, TC);
            byteWriter.AddBits(1, RD);

            byteWriter.AddBits(1, RA);
            byteWriter.AddBits(3, Z);
            byteWriter.AddBits(4, RCODE);

            byteWriter.AddUshort(QDCOUNT);
            byteWriter.AddUshort(ANCOUNT);
            byteWriter.AddUshort(NSCOUNT);
            byteWriter.AddUshort(ARCOUNT);

            return byteWriter.GetValue();
        }

        /// <summary>
        /// Create new header from byte array.
        /// </summary>
        public static DnsHeader FromBytes(byte[] bytes)
        {
            var byteReader = new ByteReader(bytes);
            return FromByteReader(byteReader);
        }

        /// <summary>
        /// Create new header from byte reader.
        /// </summary>
        public static DnsHeader FromByteReader(ByteReader byteReader)
        {
            var header = new DnsHeader();

            header.ID = byteReader.GetUshort();
            header.QR = byteReader.GetBits(1);
            header.OPCODE = byteReader.GetBits(4);
            header.AA = byteReader.GetBits(1);
            header.TC = byteReader.GetBits(1);
            header.RD = byteReader.GetBits(1);

            header.RA = byteReader.GetBits(1);
            header.Z = byteReader.GetBits(3);
            header.RCODE = byteReader.GetBits(4);

            header.QDCOUNT = byteReader.GetUshort();
            header.ANCOUNT = byteReader.GetUshort();
            header.NSCOUNT = byteReader.GetUshort();
            header.ARCOUNT = byteReader.GetUshort();

            return header;
        }

        public override string ToString()
        {
            return $"DnsHeader(ID='{ID}')";
        }
    }
}
