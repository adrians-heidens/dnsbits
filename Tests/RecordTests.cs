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

        [TestMethod]
        public void TestAaaaRecord()
        {
            var record = new AaaaRecord(
                "f00.Test", "2001:0Db8:85A3:0000:0000:8a2e:0370:7334");
            Assert.AreEqual("f00.Test", record.Name);
            Assert.AreEqual("2001:0db8:85a3:0000:0000:8a2e:0370:7334", record.IPv6);
        }
    }
}
