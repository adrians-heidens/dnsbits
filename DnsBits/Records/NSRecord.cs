using System;

namespace DnsBits.Records
{
    public class NSRecord : IRecord
    {
        private string name = null;

        private string host = null;

        public string Name
        {
            get { return name; }
            set
            {
                if (!Validator.IsValidName(value))
                {
                    throw new ArgumentException($"Invalid name value: '{value}'");
                }
                name = value;
            }
        }

        public ushort RType { get; } = (ushort)RecordType.NS;

        public ushort RClass { get; set; } = (ushort)RecordClass.IN;

        public uint Ttl { get; set; }

        public string Host
        {
            get { return host; }
            set
            {
                if (!Validator.IsValidName(value))
                {
                    throw new ArgumentException($"Invalid name value: '{value}'");
                }
                host = value;
            }
        }

        public override string ToString()
        {
            return $"NSRecord(Name={Name}, " +
                $"RType={(RecordType)RType}, " +
                $"RClass={(RecordClass)RClass}, " +
                $"Ttl={Ttl}, " +
                $"Host={Host})";
        }
    }
}
