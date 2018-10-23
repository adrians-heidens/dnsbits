using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DnsBits
{
    public static class Validator
    {
        /// <summary>
        /// Check if valid IPv6. Expect full format.
        /// </summary>
        public static bool IsValidIpv6(string ipv6)
        {
            if (ipv6 == null)
            {
                return false;
            }
            var parts = ipv6.Split(":");
            if (parts.Length != 8)
            {
                return false;
            }
            foreach (var part in parts)
            {
                ushort n = 0;
                if (!ushort.TryParse(part, NumberStyles.HexNumber, null, out n))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Remove unnecessary zeros, make uppercase.
        /// </summary>
        public static string NormalizeIPv6(string ipv6)
        {
            var parts = ipv6.Split(":");
            var partList = new List<string>();
            foreach (var part in parts)
            {
                var n = ushort.Parse(part, NumberStyles.HexNumber);
                partList.Add(n.ToString("x4"));
            }
            return string.Join(":", partList);
        }

        /// <summary>
        /// Is the string a valid IPv4 address.
        /// </summary>
        public static bool IsValidIpv4(string ipv4)
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

        /// <summary>
        /// Remove leading zeros from numbers if any.
        /// </summary>
        public static string NormalizeIpv4(string ipv4)
        {
            var parts = ipv4.Split(".");
            ipv4 = $"{byte.Parse(parts[0])}.{byte.Parse(parts[1])}." +
                $"{byte.Parse(parts[2])}.{byte.Parse(parts[3])}";
            return ipv4;
        }

        /// <summary>
        /// Is the string a valid domain name.
        /// </summary>
        public static bool IsValidName(string name)
        {
            if (name == null)
            {
                return false;
            }

            if (name.EndsWith("."))
            {
                name = name.TrimEnd('.');
            }

            var labelRegex = new Regex("^[a-z]([a-z0-9-]*[a-z0-9])?$");
            int size = 0;
            foreach (var label in name.Split("."))
            {
                var labelLower = label.ToLower();
                if (labelLower.Length > 63 || !labelRegex.IsMatch(labelLower))
                {
                    return false;
                }
                size += 1 + label.Length;
                if (size > 255)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
