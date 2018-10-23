using System;

namespace DnsBits.Records
{
    public class AaaaRecord : IRecord
    {
        private string name = null;

        private string ipv6 = null;

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

        public ushort RType { get; } = (ushort)RecordType.AAAA;

        public ushort RClass { get; set; } = (ushort)RecordClass.IN;

        public uint Ttl { get; set; }

        public string IPv6 { get { return ipv6;  }
            set {
                if (!Validator.IsValidIpv6(value))
                {
                    throw new ArgumentException($"Invalid IPv6 value: '{value}'");
                }
                ipv6 = Validator.NormalizeIPv6(value);
            }
        }

        public AaaaRecord(string name, string ipv6)
        {
            Name = name;
            IPv6 = ipv6;
        }

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
