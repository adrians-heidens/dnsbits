using DnsBits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Tests
{
    [TestClass]
    public class ByteReaderTest
    {
        [TestMethod]
        public void Test()
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
    }
}
