namespace DnsBits.Records
{
    class NSRecord : IResourceRecord
    {
        public string NAME { get; set; }

        public ushort TYPE { get; set; }

        public ushort CLASS { get; set; }

        public uint TTL { get; set; }

        public string HOST { get; set; }

        public override string ToString()
        {
            return $"NSRecord(name={NAME}, " +
                $"type={(RecordType)TYPE}, " +
                $"class={(RecordClass)CLASS}, " +
                $"TTL={TTL}, " +
                $"HOST={HOST})";
        }
    }
}
