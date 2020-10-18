using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace StreamClient
{
    public class Entry
    {
        static void Main(string[] args)
        {
            var str = Console.ReadLine();
            var k = Int32.Parse(str);
            var ctsList = new List<CancellationTokenSource>();
            for (int i = k; i < 200 + k; ++i)
            {
                UdpClient udpClient = UdpClientFactory.CreateClient("127.0.0.1", 5577);
                var output = new OutputLoop(udpClient, 16, $"user{i}");
                output.Start();
                ctsList.Add(output.cts);
            }
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "exit!")
                {
                    foreach (var cts in ctsList)
                    {
                        cts.Cancel();
                    }
                    Console.WriteLine("Application gracefully shutdown. Bye!");
                    break;
                }
            }
        }
    }
}
