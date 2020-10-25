using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CommonLibrary;

namespace StreamClient
{
    public class Entry
    {
        static async Task Main(string[] args)
        {
            var str = args.Length > 0 ? args[0] : Console.ReadLine();
            var k = Int32.Parse(str);
            var ctsList = new List<CancellationTokenSource>();
            for (int i = k; i < 200 + k; ++i)
            {
                UdpClient udpClient = UdpClientFactory.CreateClient("127.0.0.1", 5577);
                var position = new Vector3(i%100*4, 0.5f, i/100*2);
                var output = new OutputLoop(udpClient, 16, $"user{i}", position);
                output.Start();
                ctsList.Add(output.cts);
            }
            while (true)
            {
                await Task.Delay(100);
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
