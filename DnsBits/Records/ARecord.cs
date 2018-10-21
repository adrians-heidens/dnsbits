using System;

namespace DnsBits.Records
{
    public class ARecord : IRecord
    {
        private string ipv4 = null;

        public string Name { get; set; }

        public ushort RType { get; } = (ushort)RecordType.A;

        public ushort RClass { get; set; } = (ushort)RecordClass.IN;

        public uint Ttl { get; set; } = 0;

        public ARecord(string name, string ipv4)
        {
            Name = name;
            IPv4 = ipv4;
        }

        public string IPv4 {
            get { return ipv4; }
            set {
                if (!IsValidIpv4(value))
                {
                    throw new ArgumentException($"Invalid Ipv4 value: '{value}'");
                }

                // Normalizing the value.
                var parts = value.Split(".");
                ipv4 = $"{byte.Parse(parts[0])}.{byte.Parse(parts[1])}." +
                    $"{byte.Parse(parts[2])}.{byte.Parse(parts[3])}";
            }
        }

        /// <summary>
        /// Is the string a valid IPv4 address.
        /// </summary>
        private static bool IsValidIpv4(string ipv4)
        {
            if (ipv4 == null)
            {
                return false;
            }
            var parts = ipv4.Split(".");
            if (parts.Length != 4)
            {
                return false;
            }
            foreach (var part in parts)
            {
                byte b = 0;
                if (!byte.TryParse(part, out b))
                {
                    return false;
                }
            }
            return true;
        }
        
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
