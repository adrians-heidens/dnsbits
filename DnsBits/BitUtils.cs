using System;

namespace DnsBits
{
    public static class BitUtils
    {

        /// <summary>
        /// Get specified bits out of byte value.
        /// </summary>
        /// <param name="value">Value to get bits from.</param>
        /// <param name="index">From which bit to get (0 to 7).</param>
        /// <param name="count">How many bits to get (0 to 8 - index).</param>
        /// <returns>Specified bits as byte value.</returns>
        public static byte GetBits(byte value, int index, int count)
        {
            if (index < 0 || index > 7)
            {
                throw new ArgumentOutOfRangeException("index", value, "Must be in range [0, 7]");
            }
            if (count < 0 || count > 8 - index)
            {
                throw new ArgumentOutOfRangeException("count", value, "Must be in range [0, 8 - index]");
            }

            int ones = (1 << count) - 1;
            int offset = 8 - (index + count);
            int mask = ones << offset;

            int result = (value & mask) >> offset;
            return (byte) result;
        }

        /// <summary>
        /// Copy specified bits from one byte value to another.
        /// </summary>
        /// <param name="value">Byte value to copy to.</param>
        /// <param name="index">From which bit to copy (0 to 7).</param>
        /// <param name="count">How many bits to copy (0 to 8 - index).</param>
        /// <param name="source">Byte value to copy bits from.</param>
        /// <returns></returns>
        public static byte SetBits(byte value, int index, int count, byte source)
        {
            if (index < 0 || index > 7)
            {
                throw new ArgumentOutOfRangeException("index", value, "Must be in range [0, 7]");
            }
            if (count < 0 || count > 8 - index)
            {
                throw new ArgumentOutOfRangeException("count", value, "Must be in range [0, 8 - index]");
            }

            int iv = 7 - index;
            int src2 = source << (iv - count + 1);
            int m0 = (1 << count) - 1;
            int m1 = m0 << (iv - count + 1);
            int m1v = 255 - m1;
            int dst2 = m1v & value;

            int result = dst2 | src2;
            return (byte) result;
        }
    }
}
