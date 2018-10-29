using DnsBits;
using System;

namespace ClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                PrintUsage();
                Environment.Exit(2);
            }

            string serverHost = "";
            int serverPort = 53;
            string queryName = "";
            string queryType = "";

            try
            {
                serverHost = args[0];
                serverPort = Int32.Parse(args[1]);
                queryName = args[2];
                queryType = args[3];
            } catch (FormatException)
            {
                PrintUsage();
                Environment.Exit(2);
            }

            Console.WriteLine($"Query: '{queryName}' on server ('{serverHost}', {serverPort})");
            DnsClient client = new DnsClient();
            client.Query(serverHost, serverPort, queryName, queryType);

            Console.WriteLine("End.");
            Console.ReadKey();
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: prog server port name type");
        }
    }
}
