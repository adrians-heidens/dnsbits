using System;

namespace DnsBits.Records
{
    /// <summary>
    /// Unknown (generic) DNS resource record.
    /// </summary>
    public class Record : IRecord
    {
        private string name = null;

        /// <summary>
        /// Domain name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                // Special record types allow empty string.
                if (value == "")
                {
                    name = value;
                }
                else if (!Validator.IsValidName(value))
                {
                    throw new ArgumentException($"Invalid name value: '{value}'");
                }
                name = value;
            }
        }

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
        /// <remarks>
        /// Should consider limiting the length.
        /// </remarks>
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
