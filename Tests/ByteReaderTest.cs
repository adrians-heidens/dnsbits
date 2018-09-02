using DnsBits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Tests
{
    [TestClass]
    public class ByteReaderTest
    {
        [TestMethod]
        public void NormalOperationTest()
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine("Files", "structdata.dat"));
            var reader = new ByteReader(bytes);

            Assert.AreEqual(35000, reader.GetUshort());
            Assert.AreEqual(230, reader.GetByte());
            Assert.AreEqual("spam", reader.GetString(4));
            Assert.AreEqual(4211001100, reader.GetUint());
            Assert.AreEqual(1, reader.GetBits(1));
            Assert.AreEqual(2, reader.GetBits(4));
            Assert.AreEqual(1, reader.GetBits(3));
            Assert.AreEqual(10, reader.GetByte());
        }

        [TestMethod]
        [ExpectedException(typeof(DnsBitsException))]
        public void BitmodeBreakTest()
        {
            var reader = new ByteReader(new byte[] { 0x01, 0x02, 0x03 });

            reader.GetUshort();
            reader.GetBits(1);
            reader.GetByte();
        }

        [TestMethod]
        [ExpectedException(typeof(DnsBitsException))]
        public void BitReadBoundariesTest()
        {
            var reader = new ByteReader(new byte[] { 0x01, 0x02 });

            reader.GetBits(3);
            reader.GetBits(6);
        }

        [TestMethod]
        [ExpectedException(typeof(DnsBitsException))]
        public void EndOfStreamTest()
        {
            var reader = new ByteReader(new byte[] { 0x01, 0x02 });
            reader.GetUint();
        }
    }
}
