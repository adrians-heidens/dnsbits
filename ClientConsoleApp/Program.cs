using DnsBits;
using System;

namespace ClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "ns1.test";
            Console.WriteLine($"Querying name '{ name }'");

            var client = new DnsClient();
            client.Query(name);

            Console.WriteLine("End.");
            Console.ReadKey();
        }
    }
}
