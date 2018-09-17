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
            ByteWriter writer = new ByteWriter();

            writer.AddUshort(35000);
            writer.AddByte(230);
            writer.AddString("spam");
            writer.AddUint(4211001100);

            writer.AddBits(1, 1);
            writer.AddBits(4, 2);
            writer.AddBits(3, 1);

            writer.AddByte(10);
            
            byte[] expected = File.ReadAllBytes(Path.Combine("Files", "structdata.dat"));
            byte[] actual = writer.GetValue();

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

        [TestMethod]
        [ExpectedException(typeof(DnsBitsException))]
        public void UnfinishedValueTest()
        {
            ByteWriter writer = new ByteWriter();
            writer.AddBits(5, 0b10100);
            writer.GetValue();
        }
    }
}
