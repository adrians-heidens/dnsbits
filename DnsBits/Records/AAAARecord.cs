namespace DnsBits.Records
{
    class AaaaRecord : IRecord
    {
        public string Name { get; set; }

        public ushort RType { get; } = (ushort)RecordType.AAAA;

        public ushort RClass { get; set; } = (ushort)RecordClass.IN;

        public uint Ttl { get; set; }

        public string IPv6 { get; set; }

        public override string ToString()
        {
            return $"AaaaRecord(Name={Name}, " +
                $"RType={(RecordType)RType}, " +
                $"RClass={(RecordClass)RClass}, " +
                $"Ttl={Ttl}, " +
                $"IPv6={IPv6})";
        }
    }
}
