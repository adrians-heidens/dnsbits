namespace DnsBits
{
    class AAAARecord : IResourceRecord
    {
        public string NAME { get; set; }

        public ushort TYPE { get; set; }

        public ushort CLASS { get; set; }

        public uint TTL { get; set; }

        public string IPV6 { get; set; }

        public override string ToString()
        {
            return $"AAAARecord(name={NAME}, " +
                $"type={(RecordType)TYPE}, " +
                $"class={(RecordClass)CLASS}, " +
                $"TTL={TTL}, " +
                $"IPV6={IPV6})";
        }
    }
}
