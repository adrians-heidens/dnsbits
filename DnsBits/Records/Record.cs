using System;

namespace DnsBits.Records
{
    /// <summary>
    /// Unknown (generic) DNS resource record.
    /// </summary>
    public class Record : IRecord
    {
        /// <summary>
        /// Domain name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Resource record type.
        /// </summary>
        public ushort RType { get; set; }

        /// <summary>
        /// Resource record class.
        /// </summary>
        public ushort RClass { get; set; }

        /// <summary>
        /// Time to live for this record in seconds (for caching).
        /// </summary>
        /// <remarks>
        /// 0 value means no caching at all.
        /// </remarks>
        public uint Ttl { get; set; }

        /// <summary>
        /// Length of the RDATA field.
        /// </summary>
        public ushort RDLength { get { return (ushort)RData.Length; } }

        /// <summary>
        /// Content of the resource record.
        /// </summary>
        public byte[] RData { get; set; }

        public override string ToString()
        {
            return $"Record(name={Name}, " +
                $"RType={(RecordType)RType}, " +
                $"RClass={(RecordClass)RClass}, " +
                $"Ttl={Ttl}, " +
                $"RData={BitConverter.ToString(RData)})";
        }
    }
}
