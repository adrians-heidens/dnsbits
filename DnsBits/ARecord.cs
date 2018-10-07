namespace DnsBits
{
    class ARecord : IResourceRecord
    {
        public string NAME { get; set; }

        public ushort TYPE { get; set; }

        public ushort CLASS { get; set; }

        public uint TTL { get; set; }

        public string IPV4 { get; set; }

        public override string ToString()
        {
            return $"ARecord(name={NAME}, " +
                $"type={(RecordType)TYPE}, " +
                $"class={(RecordClass)CLASS}, " +
                $"TTL={TTL}, " +
                $"IPV4={IPV4})";
        }
    }
}
