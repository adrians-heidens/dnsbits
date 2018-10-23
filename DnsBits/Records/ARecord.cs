using System;

namespace DnsBits.Records
{
    public class ARecord : IRecord
    {
        private string ipv4 = null;

        private string name = null;

        public string Name {
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

        public ushort RType { get; } = (ushort)RecordType.A;

        public ushort RClass { get; set; } = (ushort)RecordClass.IN;

        public uint Ttl { get; set; } = 0;
        
        public string IPv4 {
            get { return ipv4; }
            set {
                if (!Validator.IsValidIpv4(value))
                {
                    throw new ArgumentException($"Invalid Ipv4 value: '{value}'");
                }
                ipv4 = Validator.NormalizeIpv4(value);
            }
        }

        public ARecord(string name, string ipv4)
        {
            Name = name;
            IPv4 = ipv4;
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
