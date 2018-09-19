using System;
using System.IO;
using System.Text;

namespace DnsBits
{
    /// <summary>
    /// Read from byte array primitive data types using big-endian arrangement.
    /// </summary>
    /// <remarks>
    /// Reading is not allowed accross byte boundaries. That means when it reads 5 bits from the
    /// stream then it is invalid to try to read 1 byte or 6 bits in the next read. It could
    /// be 3 bits, 1 bit followed by 2 bits or three times of 1 bit.
    /// </remarks>
    public class ByteReader
    {
        private MemoryStream memoryStream;
        private int bitsOffset = 0;
        private byte bitsByte = 0;

        public ByteReader(byte[] content)
        {
            memoryStream = new MemoryStream(content, writable: false);
        }

        private bool IsInBitmode
        {
            get { return bitsOffset != 0; }
        }

        /// <summary>
        /// Byte stream has been consumed.
        /// </summary>
        public bool IsFinished
        {
            get {
                return memoryStream.Capacity == memoryStream.Position;
            }
        }

        /// <summary>
        /// Get ushort value.
        /// </summary>
        public ushort GetUshort()
        {
            if (IsInBitmode)
            {
                throw new DnsBitsException("Reading accross byte boundaries.");
            }

            var bytes = new byte[2];
            var byteCount = memoryStream.Read(bytes);
            if (byteCount != 2)
            {
                throw new DnsBitsException("End of stream.");
            }

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
            if (IsInBitmode)
            {
                throw new DnsBitsException("Reading accross byte boundaries.");
            }

            var bytes = new byte[4];
            var byteCount = memoryStream.Read(bytes);
            if (byteCount != 4)
            {
                throw new DnsBitsException("End of stream.");
            }

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
            if (IsInBitmode)
            {
                throw new DnsBitsException("Reading accross byte boundaries.");
            }

            int value = memoryStream.ReadByte();
            if (value == -1)
            {
                throw new DnsBitsException("End of stream.");
            }
            return (byte) value;
        }

        /// <summary>
        /// Get and decode string as UTF8.
        /// </summary>
        /// <param name="size">Number of bytes to get.</param>
        public string GetString(int size)
        {
            if (IsInBitmode)
            {
                throw new DnsBitsException("Reading accross byte boundaries.");
            }

            var bytes = new byte[size];
            var byteCount = memoryStream.Read(bytes);
            if (byteCount != size)
            {
                throw new DnsBitsException("End of stream.");
            }

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Get bytes.
        /// </summary>
        /// <param name="count">Number of bytes to get.</param>
        public byte[] GetBytes(int count)
        {
            if (IsInBitmode)
            {
                throw new DnsBitsException("Reading accross byte boundaries.");
            }

            var bytes = new byte[count];
            var byteCount = memoryStream.Read(bytes);
            if (byteCount != count)
            {
                throw new DnsBitsException("End of stream.");
            }

            return bytes;
        }

        /// <summary>
        /// Get individual bits.
        /// </summary>
        /// <param name="count">Number of bits to get.</param>
        public byte GetBits(int count)
        {
            if (count < 0 || count > 7)
            {
                throw new ArgumentOutOfRangeException("count", count, "Value must be in range [0, 7]");
            }
            if (bitsOffset + count > 8)
            {
                throw new DnsBitsException("Reading accross byte boundaries.");
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

        /// <summary>
        /// Get position of current byte to read.
        /// </summary>
        public long GetPosition()
        {
            return memoryStream.Position;
        }

        /// <summary>
        /// Set position on which byte to read.
        /// </summary>
        public void SetPosition(long position)
        {
            memoryStream.Position = position;
        }
    }
}
