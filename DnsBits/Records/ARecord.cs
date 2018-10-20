namespace DnsBits.Records
{
    public class ARecord : IRecord
    {
        public string Name { get; set; }

        public ushort RType { get; } = (ushort)RecordType.A;

        public ushort RClass { get; set; } = (ushort)RecordClass.IN;

        public uint Ttl { get; set; }

        public string IPv4 { get; set; }

        public override string ToString()
        {
            return $"ARecord(Name={Name}, " +
                $"RType={(RecordType)RType}, " +
                $"RClass={(RecordClass)RClass}, " +
                $"Ttl={Ttl}, " +
                $"IPv4={IPv4})";
        }
    }
}
