using DnsBits;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class DnsClientTests
    {
        [TestMethod]
        public void CreateClient()
        {
            var client = new DnsClient();
            client.Query("www.some.example");
        }
    }
}
