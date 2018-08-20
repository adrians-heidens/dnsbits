using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests
{
    [TestClass]
    public class BitConverterTests
    {
        [TestMethod]
        public void Test()
        {
            Assert.AreEqual("AF-01", BitConverter.ToString(BitConverter.GetBytes((ushort)431)));
        }
    }
}
