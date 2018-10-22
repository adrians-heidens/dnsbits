using DnsBits.Records;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestClass]
    public class RecordTests
    {
        [TestMethod]
        public void TestARecord()
        {
            var record = new ARecord("F00.tESt", "001.02.3.40");
            Assert.AreEqual("1.2.3.40", record.IPv4);
            Assert.AreEqual("F00.tESt", record.Name);
        }
    }
}
