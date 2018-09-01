using System;
using System.IO;
using System.Text;

namespace DnsBits
{
    /// <summary>
    /// Read from byte array primitive data types using big-endian arrangement.
    /// </summary>
    public class ByteReader
    {
        private MemoryStream memoryStream;
        private int bitsOffset = 0;
        private byte bitsByte = 0;

        public ByteReader(byte[] content)
        {
            memoryStream = new MemoryStream(content, writable: false);
        }

        /// <summary>
        /// Get ushort value.
        /// </summary>
        public ushort GetUshort()
        {
            var bytes = new byte[2];
            memoryStream.Read(bytes);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return BitConverter.ToUInt16(bytes);
        }

        /// <summary>
        /// Get uint value.
        /// </summary>
        public uint GetUint()
        {
            var bytes = new byte[4];
            memoryStream.Read(bytes);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return BitConverter.ToUInt32(bytes);
        }

        /// <summary>
        /// Get byte value.
        /// </summary>
        public byte GetByte()
        {
            int value = memoryStream.ReadByte();
            if (value == -1)
            {
                // TODO: Choose proper exception. Add this check to other methods.
                throw new Exception("End of stream");
            }
            return (byte) value;
        }

        /// <summary>
        /// Get and decode string as UTF8.
        /// </summary>
        /// <param name="size">Number of bytes to get.</param>
        public string GetString(int size)
        {
            var bytes = new byte[size];
            memoryStream.Read(bytes);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Get bytes.
        /// </summary>
        /// <param name="count">Number of bytes to get.</param>
        public byte[] GetBytes(int count)
        {
            var bytes = new byte[count];
            memoryStream.Read(bytes);
            return bytes;
        }

        /// <summary>
        /// Get individual bits.
        /// </summary>
        /// <param name="count">Number of bits to get.</param>
        public byte GetBits(int count)
        {
            // TODO: Check if in bit mode for other methods.
            if (count < 0 || count > 7)
            {
                throw new ArgumentOutOfRangeException("count", count, "Value must be in range [0, 7]");
            }

            if (bitsOffset == 0)
            {
                bitsByte = GetByte();
            }

            byte value = BitUtils.GetBits(bitsByte, bitsOffset, count);

            bitsOffset += count;
            if (bitsOffset == 8)
            {
                bitsOffset = 0;
            }

            return value;
        }
    }
}
