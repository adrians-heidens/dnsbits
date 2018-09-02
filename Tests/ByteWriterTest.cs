using DnsBits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Tests
{
    [TestClass]
    public class ByteWriterTest
    {
        [TestMethod]
        public void NormalOperationTest()
        {
            ByteWriter builder = new ByteWriter();

            builder.AddUshort(35000);
            builder.AddByte(230);
            builder.AddString("spam");
            builder.AddUint(4211001100);

            builder.AddBits(1, 1);
            builder.AddBits(4, 2);
            builder.AddBits(3, 1);

            builder.AddByte(10);
            
            byte[] expected = File.ReadAllBytes(Path.Combine("Files", "structdata.dat"));
            byte[] actual = builder.GetValue();

            Assert.AreEqual(BitConverter.ToString(expected), BitConverter.ToString(actual));
        }

        [TestMethod]
        [ExpectedException(typeof(DnsBitsException))]
        public void BoundaryBitsTest()
        {
            ByteWriter writer = new ByteWriter();
            writer.AddBits(5, 0b10100);
            writer.AddBits(4, 0b1100);
        }

        [TestMethod]
        [ExpectedException(typeof(DnsBitsException))]
        public void BoundaryTest()
        {
            ByteWriter writer = new ByteWriter();
            writer.AddBits(5, 0b10100);
            writer.AddUint(123);
        }
    }
}
