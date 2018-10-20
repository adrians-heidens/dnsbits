namespace DnsBits.Records
{
    public class NSRecord : IRecord
    {
        public string Name { get; set; }

        public ushort RType { get; } = (ushort)RecordType.NS;

        public ushort RClass { get; set; } = (ushort)RecordClass.IN;

        public uint Ttl { get; set; }

        public string Host { get; set; }

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
