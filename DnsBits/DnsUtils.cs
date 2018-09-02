namespace DnsBits
{
    public static class DnsUtils
    {
        /// <summary>
        /// Create DNS question message for A records.
        /// </summary>
        public static byte[] CreateQuestionARec()
        {
            var byteWriter = new ByteWriter();

            byteWriter.AddUshort(12345);

            // Header.
            byteWriter.AddBits(1, 0);
            byteWriter.AddBits(4, 0);
            byteWriter.AddBits(1, 0);
            byteWriter.AddBits(1, 0);
            byteWriter.AddBits(1, 0);

            byteWriter.AddBits(1, 0);
            byteWriter.AddBits(3, 0);
            byteWriter.AddBits(4, 0);

            byteWriter.AddUshort(1);
            byteWriter.AddUshort(0);
            byteWriter.AddUshort(0);
            byteWriter.AddUshort(0);

            // Question.
            byteWriter.AddByte(3);
            byteWriter.AddString("ns1");
            byteWriter.AddByte(4);
            byteWriter.AddString("test");
            byteWriter.AddByte(0);

            byteWriter.AddUshort(1);
            byteWriter.AddUshort(1);
            
            return byteWriter.GetValue();
        }
    }
}
