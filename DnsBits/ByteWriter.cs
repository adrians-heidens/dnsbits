using System;
using System.IO;
using System.Text;

namespace DnsBits
{
    /// <summary>
    /// Build byte array from primitive data types using big-endian arrangement.
    /// </summary>
    public class ByteWriter
    {
        private MemoryStream memoryStream;
        private int bitsOffset = 0;
        private byte bitsByte = 0;

        public ByteWriter()
        {
            memoryStream = new MemoryStream();
        }

        private bool IsInBitmode
        {
            get { return bitsOffset != 0; }
        }

        /// <summary>
        /// Add ushort value.
        /// </summary>
        public void AddUshort(ushort value)
        {
            if (IsInBitmode)
            {
                throw new DnsBitsException("Writing accross byte boundaries.");
            }

            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            memoryStream.Write(bytes);
        }

        /// <summary>
        /// Add uint value.
        /// </summary>
        public void AddUint(uint value)
        {
            if (IsInBitmode)
            {
                throw new DnsBitsException("Writing accross byte boundaries.");
            }

            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            memoryStream.Write(bytes);
        }

        /// <summary>
        /// Add byte value.
        /// </summary>
        public void AddByte(byte value)
        {
            if (IsInBitmode)
            {
                throw new DnsBitsException("Writing accross byte boundaries.");
            }

            memoryStream.WriteByte(value);
        }

        /// <summary>
        /// Add string value encoded as UTF8.
        /// </summary>
        public void AddString(string value)
        {
            if (IsInBitmode)
            {
                throw new DnsBitsException("Writing accross byte boundaries.");
            }

            memoryStream.Write(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// Add byte array.
        /// </summary>
        /// <param name="value"></param>
        public void AddBytes(byte[] value)
        {
            if (IsInBitmode)
            {
                throw new DnsBitsException("Writing accross byte boundaries.");
            }

            memoryStream.Write(value);
        }

        /// <summary>
        /// Add individual bits.
        /// </summary>
        /// <param name="count">Number of bits to add.</param>
        /// <param name="value">Actual bits to add.</param>
        public void AddBits(int count, byte value)
        {
            if (count < 0 || count > 7)
            {
                throw new ArgumentOutOfRangeException("count", count, "Value must be in range [0, 7]");
            }
            if (bitsOffset + count > 8)
            {
                throw new DnsBitsException("Writing accross byte boundaries.");
            }

            if (bitsOffset == 0)
            {
                bitsByte = 0;
            }
            bitsByte = BitUtils.SetBits(bitsByte, bitsOffset, count, value);
            bitsOffset += count;
            
            if (bitsOffset == 8)
            {
                bitsOffset = 0;
                AddByte(bitsByte);
            }
        }

        /// <summary>
        /// Get built bit array.
        /// </summary>
        public byte[] GetValue()
        {
            if (IsInBitmode)
            {
                throw new DnsBitsException("Stream is not aligned with byte boundary.");
            }
            return memoryStream.ToArray();
        }

    }
}
