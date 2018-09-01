using DnsBits;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class BitUtilsTests
    {
        [TestMethod]
        public void GetBitsTest()
        {
            Assert.AreEqual(0b1, BitUtils.GetBits(0b10010001, 0, 1));
            Assert.AreEqual(0b0, BitUtils.GetBits(0b10010001, 1, 1));
            Assert.AreEqual(0b0, BitUtils.GetBits(0b10010001, 6, 1));
            Assert.AreEqual(0b1, BitUtils.GetBits(0b10010001, 7, 1));
            Assert.AreEqual(0b100, BitUtils.GetBits(0b10010001, 0, 3));
            Assert.AreEqual(0b001, BitUtils.GetBits(0b10010001, 1, 3));
            Assert.AreEqual(0b010, BitUtils.GetBits(0b10010001, 2, 3));
            Assert.AreEqual(0b001, BitUtils.GetBits(0b10010001, 5, 3));
            Assert.AreEqual(0b10010001, BitUtils.GetBits(0b10010001, 0, 8));
        }

        [TestMethod]
        public void SetBitsTest()
        {
            Assert.AreEqual(0b10110001, BitUtils.SetBits(0b10010001, 2, 3, 0b110));
            Assert.AreEqual(0b10110001, BitUtils.SetBits(0b10010001, 2, 1, 0b1));
            Assert.AreEqual(0b00010001, BitUtils.SetBits(0b10010001, 0, 1, 0b0));
            Assert.AreEqual(0b10010000, BitUtils.SetBits(0b10010001, 7, 1, 0b0));
            Assert.AreEqual(0b10010010, BitUtils.SetBits(0b10010001, 6, 2, 0b10));
            Assert.AreEqual(0b11101100, BitUtils.SetBits(0b10010001, 0, 8, 0b11101100));
            Assert.AreEqual(0b11111111, BitUtils.SetBits(0b10010001, 0, 8, 0b11111111));
        }
    }
}
